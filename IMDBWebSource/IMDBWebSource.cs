using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSource;
using System.Drawing;
using IMDBWebSource.Properties;
using Imdb;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;

namespace IMDBWebSource
{
    [WebSourcePlugin]
    public class IMDBWebSource : IWebSourcePlugin
    {
        internal static Imdb.Services ImdbService = new Services();

        #region IWebSourcePlugin Members

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get { return "IMDB"; } }
        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public Image Icon { get { return Resources.imdb; } }
        /// <summary>
        /// Gets the search URL.
        /// </summary>
        /// <value>The search URL.</value>
        public string SearchURL { get { return Services.BaseUrl; } }

        /// <summary>
        /// Searches the specified search string.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        public List<IWebSearchResult> Search(string searchString)
        {
            results = null;
            ImdbService.FoundMovies += new Services.FoundMoviesEventHandler(imdb_SearchResultsDownloaded);
            ImdbService.FindMovie(searchString);
            while (results == null)
            {
                Thread.Sleep(10);
                System.Windows.Forms.Application.DoEvents();
            }
            ImdbService.FoundMovies -= imdb_SearchResultsDownloaded;

            List<IWebSearchResult> webResults = new List<IWebSearchResult>();
            if (results.PopularTitles != null)
                results.PopularTitles.ForEach(m => webResults.Add(new IMDBWebSearchResult(m)));
            if (results.ExactMatches != null)
                results.ExactMatches.ForEach(m => webResults.Add(new IMDBWebSearchResult(m)));
            if (results.PartialMatches != null)
                results.PartialMatches.ForEach(m => webResults.Add(new IMDBWebSearchResult(m)));

            return webResults;
        }

        private MoviesResultset results = null;
        /// <summary>
        /// Imdb_s the search results downloaded.
        /// </summary>
        /// <param name="M">The M.</param>
        private void imdb_SearchResultsDownloaded(MoviesResultset M)
        {
            try
            {
                results = M;
            }
            catch { }
            finally
            {
                if (results == null)
                    results = new MoviesResultset();
            }
        }

        #endregion
    }

    public class IMDBWebSearchResult : IWebSearchResult
    {
        private Movie baseMovie;

        /// <summary>
        /// Initializes a new instance of the <see cref="IMDBWebSearchResult"/> class.
        /// </summary>
        /// <param name="movie">The movie.</param>
        public IMDBWebSearchResult(Movie movie)
        {
            baseMovie = movie;

            Title = movie.Title;
            OriginalTitle = movie.Title;
            Year = movie.Year;
            URL = "http://www.imdb.com/title/tt" + movie.Id;
        }

        #region IWebSearchResult Members

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets the original title.
        /// </summary>
        /// <value>The original title.</value>
        public string OriginalTitle { get; set; }
        /// <summary>
        /// Gets the year.
        /// </summary>
        /// <value>The year.</value>
        public int Year { get; set; }
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string URL { get; set; }

        /// <summary>
        /// Loads the details.
        /// </summary>
        /// <returns></returns>
        public IWebMovieDetails LoadDetails()
        {
            resultMovie = null;
            IMDBWebSource.ImdbService.MovieParsed += new Services.MovieParsedEventHandler(Imdb_MovieInfoDownloaded);
            IMDBWebSource.ImdbService.GetMovieAsync(baseMovie.Id, true);
            while (resultMovie == null)
            {
                Thread.Sleep(10);
                System.Windows.Forms.Application.DoEvents();
            }

            baseMovie = resultMovie;

            return new IMDBMovieDetials()
                {
                    URL = "http://www.imdb.com/title/tt" + baseMovie.Id,
                    Title = baseMovie.Title,
                    OriginalTitle = baseMovie.Title,
                    Country = string.Empty,
                    Year = baseMovie.Year,
                    Genres = new List<IWebGenre>(GetGenresFromStrings(baseMovie.Genres)),
                    ImageURL = baseMovie.PosterUrl,
                    Director = baseMovie.Directors.Count > 0 ?
                        new IMDBPerson(baseMovie.Directors[0].Name, "http://www.imdb.com/name/nm" + baseMovie.Directors[0].Id) : null,
                    Cast = new Dictionary<IWebPerson, string>(GetCastFromPersonList(baseMovie.Cast)),
                    Plot = baseMovie.Description,
                    Rating = baseMovie.UserRating
                };
        }

        private Movie resultMovie = null;
        /// <summary>
        /// Imdb_s the movie info downloaded.
        /// </summary>
        /// <param name="M">The M.</param>
        private void Imdb_MovieInfoDownloaded(Movie M)
        {
            try
            {
                resultMovie = M;

                string description = GetFullPlotSummary(resultMovie.Id);
                if (!string.IsNullOrEmpty(description))
                    resultMovie.Description = description;
            }
            catch { }
            finally
            {
                if (resultMovie == null)
                    resultMovie = new Movie();
            }
        }

        #endregion

        /// <summary>
        /// Gets the full plot summary.
        /// </summary>
        /// <param name="ImdbID">The imdb ID.</param>
        /// <returns></returns>
        private string GetFullPlotSummary(string ImdbID)
        {
            string url = string.Format("http://www.imdb.com/title/tt{0}/plotsummary", ImdbID);
            WebClient client = new WebClient();
            string page = client.DownloadString(url);

            //filter for plot
            Match plotMatch = Regex.Match(page, "<p class=\"plotpar\"[^>]*>(.*?)(<i[^>]*>(.*?)</i>)?</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!plotMatch.Success)
                return null;

            //remove html tags, decode entities and remove whitespace
            string plot = Regex.Replace(plotMatch.Groups[1].Value, "<[^>]*>", string.Empty);
            plot = plot.Replace("\n", string.Empty).Replace("\r", string.Empty);
            plot = System.Web.HttpUtility.HtmlDecode(plot).Trim();

            return plot;
        }

        /// <summary>
        /// Gets the genres from strings.
        /// </summary>
        /// <param name="genres">The genres.</param>
        /// <returns></returns>
        private List<IWebGenre> GetGenresFromStrings(List<string> genres)
        {
            List<IWebGenre> webGenres = new List<IWebGenre>();
            genres.ForEach(g => webGenres.Add(new IMDBGenre(g)));

            return webGenres;
        }

        /// <summary>
        /// Gets the cast from person list.
        /// </summary>
        /// <param name="persons">The persons.</param>
        /// <returns></returns>
        private Dictionary<IWebPerson, string> GetCastFromPersonList(List<Person> persons)
        {
            Dictionary<IWebPerson, string> cast = new Dictionary<IWebPerson, string>();
            persons.ForEach(p => cast.Add(new IMDBPerson(p.Name, "http://www.imdb.com/name/nm" + p.Id), p.Character));

            return cast;
        }
    }

    public class IMDBMovieDetials : IWebMovieDetails
    {
        #region IWebMovieDetails Members

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string URL { get; set; }
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets the original title.
        /// </summary>
        /// <value>The original title.</value>
        public string OriginalTitle { get; set; }
        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <value>The country.</value>
        public string Country { get; set; }
        /// <summary>
        /// Gets the year.
        /// </summary>
        /// <value>The year.</value>
        public int Year { get; set; }
        /// <summary>
        /// Gets the genres.
        /// </summary>
        /// <value>The genres.</value>
        public List<IWebGenre> Genres { get; set; }
        /// <summary>
        /// Gets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageURL { get; set; }
        /// <summary>
        /// Gets the director.
        /// </summary>
        /// <value>The director.</value>
        public IWebPerson Director { get; set; }
        /// <summary>
        /// Gets the cast.
        /// </summary>
        /// <value>The cast.</value>
        public Dictionary<IWebPerson, string> Cast { get; set; }
        /// <summary>
        /// Gets the plot.
        /// </summary>
        /// <value>The plot.</value>
        public string Plot { get; set; }
        /// <summary>
        /// Gets the rating.
        /// </summary>
        /// <value>The rating.</value>
        public double Rating { get; set; }

        #endregion
    }

    public class IMDBPerson : IWebPerson
    {
        #region IPerson Members

        public string URL { get; internal set; }

        public string Name { get; internal set; }

        public string PictureURL { get; internal set; }

        #endregion

        public IMDBPerson(string name, string url)
        {
            Name = name;
            URL = url;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class IMDBGenre : IWebGenre
    {
        #region IGenre Members

        public string Title { get; private set; }

        #endregion

        public IMDBGenre(string title)
        {
            Title = title;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
