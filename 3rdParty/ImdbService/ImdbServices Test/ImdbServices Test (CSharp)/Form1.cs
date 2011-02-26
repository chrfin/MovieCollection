using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ImdbServices_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Stopwatch S = new Stopwatch();
        Imdb.Services Imdb = new Imdb.Services();

        #region Debug
        void startTimer()
        {
            S.Reset();
            S.Start();            
        }
        void stopTimer()
        {
            S.Stop();
            lbl_elapsedt.Text = "ELAPSED TIME: " + S.ElapsedMilliseconds.ToString() + " ms";
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            Imdb.FoundMovies+=new Imdb.Services.FoundMoviesEventHandler(Imdb_FoundMovies);
            Imdb.MovieParsed+=new Imdb.Services.MovieParsedEventHandler(Imdb_MovieParsed);
        }

        void Imdb_FoundMovies(Imdb.MoviesResultset M)
        {
            stopTimer();
            btn_search.Enabled = true;
            if (!M.Any())
                return;
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            if (M.PopularTitles != null)
                treeView1.Nodes.Add("Popular titles")
                    .Nodes.AddRange(M.PopularTitles.Select(X => new TreeNode() { Text = X.Title, Tag = X.Id }).ToArray());

            if (M.ExactMatches != null)
                treeView1.Nodes.Add("Exact matches")
                    .Nodes.AddRange(M.ExactMatches.Select(X => new TreeNode() { Text = X.Title, Tag = X.Id }).ToArray());
            
            if (M.PartialMatches != null)
                treeView1.Nodes.Add("Partial matches")
                    .Nodes.AddRange(M.PartialMatches.Select(X => new TreeNode() { Text = X.Title, Tag = X.Id }).ToArray());
            
            treeView1.EndUpdate();
        }
       
        void Imdb_MovieParsed(Imdb.Movie M)
        {
            stopTimer();
            treeView1.Enabled = true;
            lbl_title.Text = M.Title;
            lbl_year.Text = M.Year.ToString();
            if(M.Genres != null)
                lbl_genres.Text = string.Join(",", M.Genres.ToArray());
            if (M.ProductionCompanies != null)
                lbl_studio.Text = M.ProductionCompanies.FirstOrDefault();
            lbl_rating.Text = M.UserRating.ToString();            
            txt_plot.Text = M.Description;
            if (chkDownloadImg.Checked)
                download_Poster(M.PosterUrl);
        }

        private void download_Poster(string imgUrl)
        {
            pictureBox1.Image = Properties.Resources.ajax_loader;
            using (var Wc = new System.Net.WebClient())
            {
                Wc.DownloadDataCompleted += new System.Net.DownloadDataCompletedEventHandler(Wc_DownloadDataCompleted);
                Wc.DownloadDataAsync(new Uri(imgUrl));
            }
        }

        void Wc_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            ((System.Net.WebClient)sender).DownloadDataCompleted -= Wc_DownloadDataCompleted;
            pictureBox1.Image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(e.Result));
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            btn_search.Enabled = false;
            startTimer();            
            Imdb.FindMovie(txt_title.Text);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int id;
            if (e.Node.Tag == null) return;
            if (int.TryParse(e.Node.Tag.ToString(), out id))
            {
                treeView1.Enabled = false;
                startTimer();
                Imdb.GetMovieAsync(e.Node.Tag.ToString(), false); //0499549 avatar  //0133093 matrix
            }
        }
    }
}
