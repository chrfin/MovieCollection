using System.Linq;
using System.Collections.Generic;

namespace Imdb
{
    public class MoviesResultset
    {
        public List<Movie> ExactMatches { get; set; }
        public List<Movie> PartialMatches { get; set; }
        public List<Movie> PopularTitles { get; set; }

        public bool Error { get; set; }
        public string ErrorMessage { get; set; }

        public bool Any()
        {
            bool b = ExactMatches != null ? ExactMatches.Any() : false;
            bool b2 = PartialMatches != null ? PartialMatches.Any() : false;
            bool b3 = PopularTitles != null ? PopularTitles.Any() : false;
            return (b || b2 || b3);
        }
    }
}
