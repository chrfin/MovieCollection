using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WebSource
{
    /// <summary>
    /// Defines a IWebSourcePlugin.
    /// </summary>
    public sealed class WebSourcePlugin : Attribute { }

    /// <summary>
    /// Searches for movies on the specified site.
    /// </summary>
    public interface IWebSourcePlugin
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <value>The icon.</value>
        Image Icon { get; }
        /// <summary>
        /// Gets the search URL.
        /// </summary>
        /// <value>The search URL.</value>
        string SearchURL { get; }

        /// <summary>
        /// Searches the specified search string.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        List<IWebSearchResult> Search(string searchString);
    }

    /// <summary>
    /// Search result of a IWebSourcePlugin.
    /// </summary>
    public interface IWebSearchResult
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }
        /// <summary>
        /// Gets the original title.
        /// </summary>
        /// <value>The original title.</value>
        string OriginalTitle { get; }
        /// <summary>
        /// Gets the year.
        /// </summary>
        /// <value>The year.</value>
        int Year { get; }
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        string URL { get; }

        /// <summary>
        /// Loads the details.
        /// </summary>
        /// <returns></returns>
        IWebMovieDetails LoadDetails();
    }

    /// <summary>
    /// Details of a Movie.
    /// </summary>
    public interface IWebMovieDetails
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        string URL { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }
        /// <summary>
        /// Gets the original title.
        /// </summary>
        /// <value>The original title.</value>
        string OriginalTitle { get; }
        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <value>The country.</value>
        string Country { get; }
        /// <summary>
        /// Gets the year.
        /// </summary>
        /// <value>The year.</value>
        int Year { get; }
        /// <summary>
        /// Gets the genres.
        /// </summary>
        /// <value>The genres.</value>
        List<IWebGenre> Genres { get; }
        /// <summary>
        /// Gets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        string ImageURL { get; }
        /// <summary>
        /// Gets the director.
        /// </summary>
        /// <value>The director.</value>
        IWebPerson Director { get; }
        /// <summary>
        /// Gets the cast.
        /// </summary>
        /// <value>The cast.</value>
        Dictionary<IWebPerson, string> Cast { get; }
        /// <summary>
        /// Gets the plot.
        /// </summary>
        /// <value>The plot.</value>
        string Plot { get; }
        /// <summary>
        /// Gets the rating.
        /// </summary>
        /// <value>The rating.</value>
        double Rating { get; }
    }

    /// <summary>
    /// A Person (Actor/Director/...).
    /// </summary>
    public interface IWebPerson
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        string URL { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        /// <summary>
        /// Gets the picture URL.
        /// </summary>
        /// <value>The picture URL.</value>
        string PictureURL { get; }
    }

    /// <summary>
    /// A Genre of a Movie.
    /// </summary>
    public interface IWebGenre
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }
    }
}
