using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSource;
using System.Net;
using System.Drawing;
using OFDBWebSource.Properties;

namespace OFDBWebSource
{
    [WebSourcePlugin]
    public class OFDBWebSource : IWebSourcePlugin
    {
        #region IWebSourcePlugin Members

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get { return "OFDB"; } }
        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public Image Icon { get { return Resources.ofdb; } }
        /// <summary>
        /// Gets the search URL.
        /// </summary>
        /// <value>The search URL.</value>
        public string SearchURL { get { return "http://www.ofdb.de/view.php?page=suchergebnis&Kat=Titel&SText={0}"; } }

        /// <summary>
        /// Searches the specified search string.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        public List<IWebSearchResult> Search(string searchString)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string response = webClient.DownloadString(string.Format(SearchURL, searchString.Replace(' ', '+')));

            response = response.Substring(response.IndexOf("Ergebnis der Suchanfrage"));

            List<IWebSearchResult> results = new List<IWebSearchResult>();

            int index = 0;
            while ((index = response.IndexOf(". <a href=\"film")) >= 0)
            {
                string url = "http://www.ofdb.de/film/" + response.Substring(index + 16, response.IndexOf("\"", index + 16) - (index + 16));

                int tipIndex = response.IndexOf("onmouseover=\"Tip(", index);
                if (tipIndex > 0)
                    response = response.Remove(tipIndex, response.IndexOf(")", tipIndex) - tipIndex);

                int nameIndex = response.IndexOf(">", tipIndex) + 1;
                int nameLength = response.IndexOfAny(new char[] { '<', '(' }, nameIndex) - nameIndex;
                string name = response.Substring(nameIndex, nameLength);

                OFDBWebSearchResult result = new OFDBWebSearchResult(name, url);

                int yearIndex = response.IndexOf("(", nameIndex) + 1;
                while (response[yearIndex + 4] != ')') yearIndex = response.IndexOf('(', yearIndex + 1) + 1;
                int orgNameIndex = response.IndexOf(" / ", nameIndex, yearIndex - nameIndex) + 3;
                if (orgNameIndex > 0)
                {
                    int orgNameLength = yearIndex - orgNameIndex - 9;
                    result.OriginalTitle = response.Substring(orgNameIndex, orgNameLength);
                }

                try { result.Year = Convert.ToInt32(response.Substring(yearIndex, 4)); }
                catch { result.Year = 0; }

                response = response.Remove(0, yearIndex + 5);

                if (results.Find(r => r.URL == result.URL) == null)
                    results.Add(result);
            }

            return results;
        }

        #endregion
    }

    public class OFDBWebSearchResult : IWebSearchResult
    {
        #region IWebSearchResult Members

        public string Title { get; private set; }
        public string URL { get; private set; }
        public string OriginalTitle { get; internal set; }
        public int Year { get; internal set; }

        private OFDBMovieDetails details;
        public IWebMovieDetails LoadDetails()
        {
            if (details != null)
                return details;

            details = new OFDBMovieDetails(Title, URL);
            details.Genres = new List<IWebGenre>();
            details.Cast = new Dictionary<IWebPerson, string>();

            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;

            string response = webClient.DownloadString(URL);

            response = response.Substring(response.IndexOf("&Uuml;bersicht der Filmdaten"));
            int imageIndex = response.IndexOf("http://img.ofdb.de");
            int imageLength = response.IndexOf("\"", imageIndex + 5) - imageIndex;
            details.ImageURL = response.Substring(imageIndex, imageLength);
            if (details.ImageURL == @"http://img.ofdb.de/film/na.gif")
                details.ImageURL = string.Empty;

            response = response.Substring(response.IndexOf("Genre(s):"));
            int genreIndex;
            while ((genreIndex = response.IndexOf("view.php?page=genre")) > 0)
            {
                genreIndex = response.IndexOf(">", genreIndex) + 1;
                int genreLength = response.IndexOf("<", genreIndex) - genreIndex;
                details.Genres.Add(new OFDBGenre(response.Substring(genreIndex, genreLength)));
                response = response.Substring(genreIndex + genreLength);
            }

            response = response.Substring(response.IndexOf("Originaltitel:"));
            int orgIndex = response.IndexOf("<b>") + 3;
            int orgLength = response.IndexOf("</b>", orgIndex) - orgIndex;
            details.OriginalTitle = response.Substring(orgIndex, orgLength);

            int countIndex = response.IndexOf(">", response.IndexOf("view.php?page=blaettern&Kat=Land")) + 1;
            int countLength = response.IndexOf("<", countIndex) - countIndex;
            details.Country = response.Substring(countIndex, countLength);

            int yearIndex = response.IndexOf(">", response.IndexOf("view.php?page=blaettern&Kat=Jahr")) + 1;
            int yearLength = response.IndexOf("<", yearIndex) - yearIndex;
            details.Year = Convert.ToInt32(response.Substring(yearIndex, yearLength));

            int dirIndex = response.IndexOf(">", response.IndexOf("view.php?page=liste&Name")) + 1;
            int dirLength = response.IndexOf("<", dirIndex) - dirIndex;

            int persURLstart = response.IndexOf("href=\"", response.IndexOf("class=\"Daten\"><b><br>&raquo; <a")) + 6;
            int persURLlength = response.IndexOf("\">detaillierte", persURLstart) - persURLstart;
            personsURL = "http://www.ofdb.de/" + response.Substring(persURLstart, persURLlength);
            string dirName = response.Substring(dirIndex, dirLength);
            PersonDetails? dirDetails = GetPersonDetails(dirName);
            details.Director = new OFDBPerson(dirName, dirDetails.HasValue ? dirDetails.Value.URL : string.Empty);

            response = response.Substring(response.IndexOf("Darsteller:"));
            while (response.IndexOf("view.php?page=liste&Name") > 0 && response.IndexOf("detaillierte Cast") > response.IndexOf("view.php?page=liste&Name"))
            {
                int persIndex = response.IndexOf(">", response.IndexOf("view.php?page=liste&Name")) + 1;
                int persLength = response.IndexOf("<", persIndex) - persIndex;
                string name = response.Substring(persIndex, persLength);
                PersonDetails? persDetails = GetPersonDetails(name);
                details.Cast.Add(new OFDBPerson(name, persDetails.HasValue ? persDetails.Value.URL : string.Empty), persDetails.HasValue ? persDetails.Value.Role.Trim() : string.Empty);
                response = response.Substring(persIndex + persLength);
            }

            int plotIndex = response.IndexOf("<b>Inhalt:</b> ") + 15;
            if (plotIndex > 14)
            {
                int plotLength = response.IndexOf("<", plotIndex) - plotIndex;
                details.Plot = response.Substring(plotIndex, plotLength);

                int plotDetailsURLstart = response.IndexOf("href=", plotIndex) + 6;
                if (plotDetailsURLstart > 5)
                {
                    int plotDetailsURLlength = response.IndexOf("\">", plotDetailsURLstart) - plotDetailsURLstart;
                    string plotDetailsURL = "http://www.ofdb.de/" + response.Substring(plotDetailsURLstart, plotDetailsURLlength);
                    if (plotDetailsURL.Contains("plot"))
                    {
                        string plotDetailsHTML = webClient.DownloadString(plotDetailsURL);
                        int plotDetailsStart = plotDetailsHTML.IndexOf("</b></b><br><br>", plotDetailsHTML.IndexOf("Ansicht einer Inhaltsangabe")) + 16;
                        int plotDetailsLength = plotDetailsHTML.IndexOf("</font></p>", plotDetailsStart) - plotDetailsStart;
                        details.Plot = plotDetailsHTML.Substring(plotDetailsStart, plotDetailsLength).Replace("<br />", string.Empty);
                    }
                }
            }

            int ratingIndex = response.IndexOf("Note: ") + 6;
            details.Rating = Convert.ToDouble(response.Substring(ratingIndex, 4).Replace(".", ","));

            return details;
        }

        #endregion

        public OFDBWebSearchResult(string title, string url)
        {
            Title = title;
            URL = url;
        }

        private struct PersonDetails
        {
            public string Role { get; set; }
            public string URL { get; set; }
        }
        private string personsURL = string.Empty;
        private List<string> personRows = null;
        private PersonDetails? GetPersonDetails(string name)
        {
            if (personsURL == string.Empty)
                return null;

            try
            {
                if (personRows == null)
                {
                    LoadRows();
                }

                if (personRows.FindIndex(r => r.Contains(name)) < 0)
                    return null;

                string nameRow = personRows.Find(r => r.Contains(name));
                int URLstart = nameRow.IndexOf("href=") + 6;
                int URLlength = nameRow.IndexOf("\">", URLstart) - URLstart;
                string URL = "http://www.ofdb.de/" + nameRow.Substring(URLstart, URLlength);

                int roleStart = nameRow.IndexOf("... ") + 4;
                string role;
                if (roleStart == 3)
                    role = string.Empty;
                else
                {
                    int roleLength = nameRow.IndexOf("<", roleStart) - roleStart;
                    role = nameRow.Substring(roleStart, roleLength);
                }

                return new PersonDetails() { Role = role, URL = URL };
            }
            catch { return null; }
        }
        private void LoadRows()
        {
            string personsHTML = string.Empty;
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            personsHTML = webClient.DownloadString(personsURL);
            personsHTML = personsHTML.Substring(personsHTML.IndexOf("<b>Achtung:</b> Die folgende Cast-/Crew-Ansicht erhebt keinen Anspruch auf Vollst&auml;ndigkeit."));

            personRows = new List<string>();

            int dirStart = personsHTML.IndexOf("<tr valign");
            int dirLength = personsHTML.IndexOf("</tr>", dirStart) + 5 - dirStart;
            personRows.Add(personsHTML.Substring(dirStart, dirLength));

            personsHTML = personsHTML.Substring(personsHTML.IndexOf("<table", dirStart));
            int pos = 0;
            int max = personsHTML.IndexOf("</table>") - 5;
            while (pos < max)
            {
                int start = personsHTML.IndexOf("<tr valign", pos);
                int length = personsHTML.IndexOf("</tr>", start) + 5 - start;
                pos = start + length;
                personRows.Add(personsHTML.Substring(start, length));
            }
        }
    }

    public class OFDBMovieDetails : IWebMovieDetails
    {
        #region IMovieDetails Members

        public string URL { get; private set; }

        public string Title { get; private set; }

        public string OriginalTitle { get; internal set; }

        public string Country { get; internal set; }

        public int Year { get; internal set; }

        public List<IWebGenre> Genres { get; internal set; }

        public string ImageURL { get; internal set; }

        public IWebPerson Director { get; internal set; }

        public Dictionary<IWebPerson, string> Cast { get; internal set; }

        public string Plot { get; internal set; }

        public double Rating { get; internal set; }

        #endregion

        public OFDBMovieDetails(string title, string url)
        {
            Title = title;
            URL = url;
        }
    }

    public class OFDBPerson : IWebPerson
    {
        #region IPerson Members

        public string URL { get; internal set; }

        public string Name { get; internal set; }

        public string PictureURL { get; internal set; }

        #endregion

        public OFDBPerson(string name, string url)
        {
            Name = name;
            URL = url;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class OFDBGenre : IWebGenre
    {
        #region IGenre Members

        public string Title { get; private set; }

        #endregion

        public OFDBGenre(string title)
        {
            Title = title;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
