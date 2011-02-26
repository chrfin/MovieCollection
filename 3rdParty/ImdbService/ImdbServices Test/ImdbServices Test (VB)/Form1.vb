Imports System.Diagnostics
Imports System.Linq
Imports Imdb

Public Class Form1
    Dim S As New Stopwatch()
    Dim WithEvents _Imdb As New Services

#Region "Debug"

    Sub startTimer()
        S.Reset()
        S.Start()
    End Sub

    Sub stopTimer()
        S.Stop()
        lbl_elapsedt.Text = "ELAPSED TIME: " + S.ElapsedMilliseconds.ToString() + " ms"
    End Sub

#End Region

    Private Sub _Imdb_FoundMovies(ByVal M As Imdb.MoviesResultset) Handles _Imdb.FoundMovies
        stopTimer()
        btn_search.Enabled = True
        If (M.Any() = False) Then
            Return
        End If
        treeView1.BeginUpdate()
        treeView1.Nodes.Clear()

        If (M.PopularTitles IsNot Nothing) Then
            treeView1.Nodes.Add("Popular titles") _
            .Nodes.AddRange(M.PopularTitles.Select(Function(X) New TreeNode With {.Text = X.Title, .Tag = X.Id}).ToArray())
        End If

        If (M.ExactMatches IsNot Nothing) Then
            treeView1.Nodes.Add("Exact matches") _
                    .Nodes.AddRange(M.ExactMatches.Select(Function(X) New TreeNode() With {.Text = X.Title, .Tag = X.Id}).ToArray())

        End If

        If (M.PartialMatches IsNot Nothing) Then
            treeView1.Nodes.Add("Partial matches") _
                    .Nodes.AddRange(M.PartialMatches.Select(Function(X) New TreeNode() With {.Text = X.Title, .Tag = X.Id}).ToArray())
        End If

        treeView1.EndUpdate()
    End Sub

    Private Sub _Imdb_MovieParsed(ByVal M As Imdb.Movie) Handles _Imdb.MovieParsed
        stopTimer()
        treeView1.Enabled = True
        lbl_title.Text = M.Title
        lbl_year.Text = M.Year.ToString()
        If (M.Genres IsNot Nothing) Then
            lbl_genres.Text = String.Join(",", M.Genres.ToArray())
        End If
        If (M.ProductionCompanies IsNot Nothing) Then
            lbl_studio.Text = M.ProductionCompanies.FirstOrDefault()
        End If

        lbl_rating.Text = M.UserRating.ToString()
        txt_plot.Text = M.Description
        If (chkDownloadImg.Checked) Then
            download_Poster(M.PosterUrl)
        End If
    End Sub

    Private Sub download_Poster(ByVal imgUrl As String)
        pictureBox1.Image = My.Resources.ajax_loader
        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
        Using Wc As New System.Net.WebClient()
            AddHandler Wc.DownloadDataCompleted, AddressOf Wc_DownloadDataCompleted
            Wc.DownloadDataAsync(New Uri(imgUrl))
        End Using
    End Sub

    Private Sub Wc_DownloadDataCompleted(ByVal sender As Object, ByVal e As System.Net.DownloadDataCompletedEventArgs)
        RemoveHandler CType(sender, System.Net.WebClient).DownloadDataCompleted, AddressOf Wc_DownloadDataCompleted
        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        pictureBox1.Image = System.Drawing.Image.FromStream(New System.IO.MemoryStream(e.Result))
    End Sub

    Private Sub btn_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_search.Click
        btn_search.Enabled = False
        startTimer()
        _Imdb.FindMovie(txt_title.Text)
    End Sub

    Private Sub treeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeView1.AfterSelect
        Dim id As Integer
        If (e.Node.Tag Is Nothing) Then
            Return
        End If
        If (Integer.TryParse(e.Node.Tag.ToString(), id)) Then
            treeView1.Enabled = False
            startTimer()
            _Imdb.GetMovieAsync(e.Node.Tag.ToString(), False) '//0499549 avatar  //0133093 matrix
        End If
    End Sub
End Class
