using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Imdb
{
    public class Services
    {
        public const string BaseUrl = "http://www.imdb.com/";

        #region Patterns
        const string IMDB_ID_REGEX = @"(?<=\w\w)\d{7}";
        const string MOVIE_TITLE_YEAR_PATTERN = @"(.*)\((\d{4}).*"; //(\w+)\s.(\d+).

        const string USER_RATING_PATTERN = @"\b(?<score>\d\.\d)/10"; //"\b\d\.\d/10";
        const string USER_VOTES_PATTERN = @"\d+\,\d+";
        const string FILM_RATING_PATTERN = @"(?<=Rated\s).*(?=\sfor)";

        const string RELEASE_DATE_PATTERN = @"\d+\s\w+\s\d{4}\s";

        const string PERSON_ID = @"(?<=nm)\d+";
        #endregion

        struct SearchState
        {
            public string Code;
            public string CacheKey;
            public bool FullInfo;
        }

        /// <summary>
        /// Match a regexp against a string
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="pattern">Regex pattern</param>
        /// <returns></returns>
        private Match Match(string s, string pattern)
        {
            return Regex.Match(s, pattern, RegexOptions.IgnoreCase);
        }

        private MatchCollection MatchMany(string s, string pattern)
        {
            return Regex.Matches(s, pattern, RegexOptions.IgnoreCase);
        }

        #region Request_Funcs

        private WebClient CreateWebClient()
        {
            var Wc = new WebClient();
            Wc.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            Wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 3.5;)");
            //Wc.Proxy = Proxy
            return Wc;
        }

        private string UncompressResponse(byte[] Response, WebHeaderCollection ResponseHeaders)
        {
            string Html = "";
            using (MemoryStream Ms = new MemoryStream(Response))
            {

                switch (ResponseHeaders[HttpResponseHeader.ContentEncoding])
                {
                    case "gzip":
                        Html = new StreamReader(new GZipStream(Ms, CompressionMode.Decompress)).ReadToEnd();
                        break;
                    case "deflate":
                        Html = new StreamReader(new DeflateStream(Ms, CompressionMode.Decompress)).ReadToEnd();
                        break;
                    default:
                        Html = new StreamReader(Ms).ReadToEnd();
                        break;
                }
            }
            return Html;
        }

        #endregion

        #region FindMovie

        public delegate void FoundMoviesEventHandler(MoviesResultset M);
        /// <summary>
        /// Event fired by FindMovie method
        /// </summary>
        public event FoundMoviesEventHandler FoundMovies;

        public void FindMovie(string movieTitle)
        {
            string Url = BaseUrl + "find?s=all&q=" + movieTitle.UrlEncode();
            
            var Wr = WebRequest.Create(Url);
            Wr.Timeout = 15000;//15 secs
            Wr.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            //Wr.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 3.5;)");
            //Wr.Proxy = Proxy;

            //WebResponse Response = Wr.GetResponse();

            Wr.BeginGetResponse(new AsyncCallback(ResponseCallback), new RequestState() { Request = Wr, MovieTitle = movieTitle });
        }

        void ResponseCallback(IAsyncResult result)
        {
            MoviesResultset R = new MoviesResultset();
            try
            {
                RequestState state = (RequestState)result.AsyncState;
                WebRequest request = (WebRequest)state.Request;
                // get the Response
                HttpWebResponse Response = (HttpWebResponse)request.EndGetResponse(result);
                // process the response...                
                string Html = "";
                var haveBeenRedirected = Match(Response.ResponseUri.ToString(), IMDB_ID_REGEX);

                if (haveBeenRedirected.Success)
                {
                    R.ExactMatches = new List<Movie>();
                    R.ExactMatches.Add(new Movie() { Id = haveBeenRedirected.ToString(), Title = state.MovieTitle.CapitalizeAll() });
                    OnFoundMovies(R);
                }
                else
                {
                    switch (Response.Headers[HttpResponseHeader.ContentEncoding])
                    {
                        case "gzip":
                            Html = new StreamReader(new GZipStream(Response.GetResponseStream(), CompressionMode.Decompress)).ReadToEnd();
                            break;
                        case "deflate":
                            Html = new StreamReader(new DeflateStream(Response.GetResponseStream(), CompressionMode.Decompress)).ReadToEnd();
                            break;
                        default:
                            Html = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                            break;
                    }

                    var H = new HtmlDocument();
                    H.LoadHtml(Html);

                    //Popular titles
                    R.PopularTitles = GetTitles(H, "Popular Titles");
                    R.ExactMatches = GetTitles(H, "Titles (Exact Matches)");
                    R.PartialMatches = GetTitles(H, "Titles (Partial Matches)");

                }            
            }
            catch (Exception ex)
            {
                R.Error = true;
                R.ErrorMessage = ex.Message;
            }
            
            OnFoundMovies(R);
        }

        private List<Movie> GetTitles(HtmlDocument h, string listTitle)
        {
            var List = new List<Movie>();            
            var Nodes = h.DocumentNode.SelectNodes("id('main')//p/b[contains(.,'" + listTitle + "')]/following-sibling::*[1]/tr/td[3]");
            if (Nodes != null)
            {
                try
                {
                    HtmlNode a;
                    foreach (var N in Nodes)
                    {
                        a = N.SelectSingleNode("a");
                        List.Add(new Movie()
                        {
                            Id = Match(a.OuterHtml, IMDB_ID_REGEX).ToString(),
                            Title = a.InnerText.HtmlDecode()
                        });
                    }
                    return List;
                }
                catch {}                
            }
            return null;
        }

        protected virtual void OnFoundMovies(MoviesResultset R)
        {
            ProcessDelegate(FoundMovies, new object[] { R });
        }

        #endregion

        #region GetMovie

        public delegate void MovieParsedEventHandler(Movie M);
        /// <summary>
        /// Event fired by GetMovie method
        /// </summary>
        public event MovieParsedEventHandler MovieParsed;

        /// <summary>
        /// Get movie info from Imdb.com
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fullInfo"></param>
        /// <returns></returns>
        public Movie GetMovie(string code, bool fullInfo)
        {
            string Url = BaseUrl + "title/tt" + code;

            if (fullInfo)
                Url += "/combined";

            var Cache = new FileCache();
            var State = new SearchState() { Code = code, CacheKey = code + (fullInfo ? "-f" : ""), FullInfo = fullInfo };

            object Val = Cache.Read(State.CacheKey);
            string Html="";

            if (Val != null)
            {
                Html = Val.ToString();
                return ParseMovieHTML(code, fullInfo, ref Html, true);
            }
            else
            {
                using (WebClient Wc = CreateWebClient())
                {
                    byte[] Data = Wc.DownloadData(new Uri(Url));
                    Html = UncompressResponse(Data,Wc.ResponseHeaders);
                    Cache.Save(State.CacheKey, Html);
                    return ParseMovieHTML(code, fullInfo, ref Html, true);
                }
            }
        }

        /// <summary>
        /// Get movie info from Imdb.com asynchronously
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fullInfo">Get extra info</param>
        public void GetMovieAsync(string code, bool fullInfo)
        {            
            string Url = BaseUrl + "title/tt" + code;

            if (fullInfo)
                Url += "/combined";


            var Cache = new FileCache();
            var State = new SearchState() { Code = code, CacheKey = code + (fullInfo ? "-f" : ""), FullInfo = fullInfo };

            object Val = Cache.Read(State.CacheKey);

            if (Val != null)
            {
                string Html = Val.ToString();
                ParseMovieHTML(code, fullInfo, ref Html, false);
            }
            else
            {
                using (WebClient Wc = CreateWebClient())
                {
                    Wc.DownloadDataCompleted += new DownloadDataCompletedEventHandler(Wc_DownloadDataCompleted);                    
                    Wc.DownloadDataAsync(new Uri(Url), State);                    
                }
            }
        }

        private void Wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            string Html = "";            
            WebClient Wc = (WebClient)sender;
            SearchState State = (SearchState)e.UserState;
            if (e.Error == null)
            {
                Html = UncompressResponse(e.Result, Wc.ResponseHeaders);
                new FileCache().Save(State.CacheKey, Html);
                ParseMovieHTML(State.Code, State.FullInfo, ref Html, false);
            }
            else
            {
                OnMovieParsed(null);
            }
        }

        private Movie ParseMovieHTML(string code, bool fullInfo, ref string html, bool returnValue)
        {
            var Cult = new System.Globalization.CultureInfo("en-US");
            Movie M = new Movie();
            //Html
            HtmlNode Node;
            HtmlNodeCollection Nodes;
            var H = new HtmlDocument();
            H.LoadHtml(html);
            html = null;

            M.Id = code;

            Nodes = H.DocumentNode.SelectNodes("//html/head/title");

            if (Nodes == null)
            {//If we dont have a title, exit the function
                OnMovieParsed(null);
                return null;
            }

            var _Title = Match(Nodes[0].InnerText, MOVIE_TITLE_YEAR_PATTERN).Groups;//TODO: Optimize

            M.Title = _Title[1].ToString().CleanTitle();
            M.Year = int.Parse(_Title[2].ToString());

            M.Description = GetDescription(H);
            M.PosterUrl = GetImageUrl(H);
            M.Genres = GetGenres(H);
            M.Cast = GetCast(H);//Cast
            M.Directors = GetDirectors(H);
            M.Writers = GetWriters(H);

            //SelectSingleNode was 5 times slower in many cases than SelectNodes (Profiled with Ants Profiler 5)

            //Find div that contains user's vote and score
            Nodes = H.DocumentNode.SelectNodes("id('tn15rating')");

            double Rating = 0;
            double Votes = 0;

            if (Nodes != null)
            {
                Node = Nodes[0];
                double.TryParse(Match(Node.InnerText, USER_RATING_PATTERN).Groups["score"].ToString(),
                         System.Globalization.NumberStyles.Float, Cult, out Rating);

                double.TryParse(Match(Node.InnerText, USER_VOTES_PATTERN).ToString(),
                    System.Globalization.NumberStyles.Number, Cult, out Votes);
            }
            M.UserRating = Rating;
            M.Votes = Votes;

            //MPAA Rating
            Node = H.DocumentNode.SelectSingleNode("id('tn15content')//div[@class='info']/a[@href='/mpaa']/../../div[@class]");
            string Rated = string.Empty;

            if (Node != null)
                Rated = Match(Node.InnerText, FILM_RATING_PATTERN).ToString();

            M.Rated = Rated;


            //Release Date
            Node = H.DocumentNode.SelectSingleNode("id('tn15content')//h5[contains(.,'Release Date:')]/../div[@class]");
            DateTime? ReleaseD = null;
            if (Node != null)
            {
                DateTime R;
                if (DateTime.TryParse(Match(Node.InnerText, RELEASE_DATE_PATTERN).ToString(), out R))
                    ReleaseD = R;
            }
            M.ReleaseDate = ReleaseD;

            Node = H.DocumentNode.SelectSingleNode("id('tn15content')//h5[contains(.,'Tagline:')]/../div[@class]");
            if (Node != null)
                M.Tagline = Node.InnerText;


            Node = H.DocumentNode.SelectSingleNode("id('tn15content')//h5[contains(.,'Runtime:')]/../div[@class]");
            int Runtime = 0;
            if (Node != null)
                int.TryParse(Match(Node.InnerText, @"\d+").ToString(), out Runtime);

            M.Runtime = Runtime;
            M.Languages = GetLanguages(H);
            M.Seasons = GetSeasons(H);
            M.IsTvSerie = M.Seasons.Any();
            M.KnownTitles = GetKnownTitles(H);
            M.RecommendedFilms = GetRecommendedTitles(H);

            if (fullInfo)
            {
                M.ProductionCompanies = GetCompanies(H, "Production Companies");
                M.Distributors = GetCompanies(H, "Distributors");
                M.SpecialEffectsCompanies = GetCompanies(H, "Special Effects");
                M.OtherCompanies = GetCompanies(H, "Other Companies");
                M.MusicBy = GetPersons(H, "Original Music by");
                M.Producers = GetPersons(H, "Produced by");
                M.SpecialEffects = GetPersons(H, "Special Effects by");
                //M.Stunts = GetPersons(H, "Stunts");
                M.CastingBy = GetPersons(H, "Casting by");
                M.CostumeDesignBy = GetPersons(H, "Costume Design by");
            }

            if (returnValue == true)
                return M;
            else
            {
                OnMovieParsed(M);
                return null;
            }
        }

        protected void OnMovieParsed(Movie M)
        {
            if (MovieParsed != null)
                MovieParsed(M);
        }

        #region ParseFunctions

        private string GetDescription(HtmlDocument H)
        {
            var Nodes = H.DocumentNode.SelectNodes("id('tn15content')//div[@class='info']/h5[contains(.,'Plot:')]/../div[@class][1]");
            if (Nodes != null)
            {
                foreach (var N in Nodes[0].SelectNodes("a"))
                    N.Remove();

                return Nodes[0].InnerText.CleanHtml();
            }
            return string.Empty;
        }
        private string GetImageUrl(HtmlDocument H)
        {
            var Nodes = H.DocumentNode.SelectNodes("id('tn15lhs')//div[@class='photo']/a/img[1]");
            if (Nodes != null)
                return Nodes[0].Attributes["src"].Value;
            return string.Empty;
        }
        private List<Person> GetCast(HtmlDocument H)
        {
            var List = new List<Person>();
            var Nodes = H.DocumentNode.SelectNodes("id('tn15content')//table[@class='cast']/tr");
            if (Nodes != null)
            {
                foreach (var N in Nodes)
                {
                    var nm = N.SelectSingleNode("td[@class='nm']");
                    if (nm != null)
                    {
                        string Id = Match(nm.OuterHtml, PERSON_ID).ToString();
                        List.Add(new Person()
                        {
                            Id = Id,
                            Name = nm.InnerText,
                            Character = N.SelectSingleNode("td[@class='char']").InnerText
                        });
                    }
                }
            }
            return List;
        }
        private List<Person> GetDirectors(HtmlDocument H)
        {
            var Nodes = H.DocumentNode.SelectNodes("id('director-info')//div/a");
            if (Nodes != null)
                return Nodes.Select(N => new Person() { Id = Match(N.OuterHtml, PERSON_ID).ToString(), Name = N.InnerText }).ToList();                
            return null;
        }
        private List<Person> GetWriters(HtmlDocument H)
        {
            var Nodes = H.DocumentNode.SelectNodes("id('tn15content')//h5[contains(.,'Writer')]/../div[@class]/a");
            if (Nodes != null)
                return Nodes.Select(N => new Person() { Id = Match(N.OuterHtml, PERSON_ID).ToString(), Name = N.InnerText }).ToList();
            return null;
        }
        private List<string> GetGenres(HtmlDocument H)
        {
            var Nodes = H.DocumentNode.SelectNodes("id('tn15content')//h5[contains(.,'Genre:')]/../div[@class]/a");
            if (Nodes != null)
                return Nodes.Select(N => N.InnerText).Where(N => N != "more").ToList();                
            return null;
        }
        private List<string> GetLanguages(HtmlDocument H)
        {
            var Nodes = H.DocumentNode.SelectNodes("id('tn15content')//h5[contains(.,'Language:')]/../div[@class]/a");
            if (Nodes != null)
                return Nodes.Select(N => N.InnerText.Trim()).ToList();                
            return null;
        }
        private List<int> GetSeasons(HtmlDocument H)
        {
            var List = new List<int>();
            var Node = H.DocumentNode.SelectNodes("id('tn15content')//h5[contains(.,'Seasons:')]/../div[@class]/a");
            int i;
            if (Node != null)
                foreach (var N in Node)
                    if (int.TryParse(N.InnerText, out i))
                        List.Add(i);
            return List;
        }
        private List<string> GetKnownTitles(HtmlDocument H)
        {
            var Node = H.DocumentNode.SelectSingleNode("id('tn15content')//h5[contains(.,'Also Known As:')]/../div[@class]");
            if (Node != null)
            {
                Node.SelectSingleNode("a").Remove();
                return Node.InnerHtml.HtmlDecode().Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            return null;
        }
        private List<Movie> GetRecommendedTitles(HtmlDocument H)
        {
            var Nodes = H.DocumentNode.SelectNodes("id('tn15content')//table[@class='recs']/tbody/tr[2]/td/a");
            if (Nodes != null)
                return Nodes.Select(N => new Movie(){ Id = Match(N.OuterHtml, IMDB_ID_REGEX).ToString(),Title = N.InnerText }).ToList();
            return null;
        }
        private List<string> GetCompanies(HtmlDocument H, string sTitle)
        {
            var Nodes = H.DocumentNode.SelectNodes("id('tn15content')//b[@class='blackcatheader'][contains(.,'" + sTitle + "')]/following-sibling::*[1]/li/a");
            if (Nodes != null)
                return Nodes.Select(N => N.InnerText.HtmlDecode()).Distinct().ToList();
            return null;
        }
        private List<Person> GetPersons(HtmlDocument H, string sTitle)
        {
            var List = new List<Person>();
            var Nodes = H.DocumentNode.SelectNodes("id('tn15content')//h5[contains(.,'" + sTitle + "')]/../../../tr/td[1]");
            if (Nodes != null)
            {
                //Remove the first and the last
                Nodes.RemoveAt(0);
                Nodes.RemoveAt(Nodes.Count - 1);

                return Nodes.Select(N => new Person() { Id = Match(N.InnerHtml, IMDB_ID_REGEX).ToString(), Name = N.InnerHtml.StripHTML() }).ToList();

            }
            return List;
        }

        #endregion

        #endregion

        //Borrowed from http://weblogs.asp.net/rosherove/articles/BackgroundWorker.aspx
        private void ProcessDelegate(Delegate del, params object[] args)
        {
            Delegate temp = del;
            if (temp == null)
            {
                return;
            }
            Delegate[] delegates = temp.GetInvocationList();
            foreach (Delegate handler in delegates)
                InvokeDelegate(handler, args);
        }
        private void InvokeDelegate(Delegate del, object[] args)
        {
            System.ComponentModel.ISynchronizeInvoke synchronizer;
            synchronizer = del.Target as System.ComponentModel.ISynchronizeInvoke;
            if (synchronizer != null) //A Windows Forms object
            {
                if (synchronizer.InvokeRequired == false)
                {
                    del.DynamicInvoke(args);
                    return;
                }
                try
                {
                    synchronizer.Invoke(del, args);
                }
                catch
                { }
            }
            else //Not a Windows Forms object
            {
                del.DynamicInvoke(args);
            }
        }
    }

    public class RequestState
    {
        public WebRequest Request;
        public string MovieTitle;
        public RequestState()
        {
            Request = null;
            MovieTitle = string.Empty;
        }
    }

}
