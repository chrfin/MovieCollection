<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.chkDownloadImg = New System.Windows.Forms.CheckBox
        Me.btn_search = New System.Windows.Forms.Button
        Me.txt_title = New System.Windows.Forms.TextBox
        Me.label1 = New System.Windows.Forms.Label
        Me.panel1 = New System.Windows.Forms.Panel
        Me.label8 = New System.Windows.Forms.Label
        Me.lbl_studio = New System.Windows.Forms.Label
        Me.label6 = New System.Windows.Forms.Label
        Me.txt_plot = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lbl_rating = New System.Windows.Forms.Label
        Me.lbl_genres = New System.Windows.Forms.Label
        Me.lbl_year = New System.Windows.Forms.Label
        Me.lbl_title = New System.Windows.Forms.Label
        Me.pictureBox1 = New System.Windows.Forms.PictureBox
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.treeView1 = New System.Windows.Forms.TreeView
        Me.lbl_elapsedt = New System.Windows.Forms.Label
        Me.panel1.SuspendLayout()
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkDownloadImg
        '
        Me.chkDownloadImg.AutoSize = True
        Me.chkDownloadImg.Location = New System.Drawing.Point(351, 23)
        Me.chkDownloadImg.Name = "chkDownloadImg"
        Me.chkDownloadImg.Size = New System.Drawing.Size(137, 17)
        Me.chkDownloadImg.TabIndex = 20
        Me.chkDownloadImg.Text = "Download movie poster"
        Me.chkDownloadImg.UseVisualStyleBackColor = True
        '
        'btn_search
        '
        Me.btn_search.Location = New System.Drawing.Point(268, 18)
        Me.btn_search.Name = "btn_search"
        Me.btn_search.Size = New System.Drawing.Size(75, 23)
        Me.btn_search.TabIndex = 16
        Me.btn_search.Text = "Search"
        Me.btn_search.UseVisualStyleBackColor = True
        '
        'txt_title
        '
        Me.txt_title.Location = New System.Drawing.Point(58, 20)
        Me.txt_title.Name = "txt_title"
        Me.txt_title.Size = New System.Drawing.Size(189, 20)
        Me.txt_title.TabIndex = 15
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(12, 23)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(30, 13)
        Me.label1.TabIndex = 18
        Me.label1.Text = "Title:"
        '
        'panel1
        '
        Me.panel1.BackColor = System.Drawing.Color.White
        Me.panel1.Controls.Add(Me.label8)
        Me.panel1.Controls.Add(Me.lbl_studio)
        Me.panel1.Controls.Add(Me.label6)
        Me.panel1.Controls.Add(Me.txt_plot)
        Me.panel1.Controls.Add(Me.Label5)
        Me.panel1.Controls.Add(Me.Label4)
        Me.panel1.Controls.Add(Me.Label3)
        Me.panel1.Controls.Add(Me.Label2)
        Me.panel1.Controls.Add(Me.lbl_rating)
        Me.panel1.Controls.Add(Me.lbl_genres)
        Me.panel1.Controls.Add(Me.lbl_year)
        Me.panel1.Controls.Add(Me.lbl_title)
        Me.panel1.Controls.Add(Me.pictureBox1)
        Me.panel1.Location = New System.Drawing.Point(268, 76)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(402, 304)
        Me.panel1.TabIndex = 17
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(123, 138)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(40, 13)
        Me.label8.TabIndex = 20
        Me.label8.Text = "Studio:"
        '
        'lbl_studio
        '
        Me.lbl_studio.AutoSize = True
        Me.lbl_studio.Location = New System.Drawing.Point(167, 138)
        Me.lbl_studio.Name = "lbl_studio"
        Me.lbl_studio.Size = New System.Drawing.Size(10, 13)
        Me.lbl_studio.TabIndex = 19
        Me.lbl_studio.Text = "-"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(123, 164)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(28, 13)
        Me.label6.TabIndex = 18
        Me.label6.Text = "Plot:"
        '
        'txt_plot
        '
        Me.txt_plot.BackColor = System.Drawing.Color.White
        Me.txt_plot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_plot.Location = New System.Drawing.Point(170, 162)
        Me.txt_plot.Multiline = True
        Me.txt_plot.Name = "txt_plot"
        Me.txt_plot.ReadOnly = True
        Me.txt_plot.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_plot.Size = New System.Drawing.Size(218, 75)
        Me.txt_plot.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(123, 110)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(41, 13)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Rating:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(123, 81)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(44, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Genres:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(123, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Year:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(123, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Title:"
        '
        'lbl_rating
        '
        Me.lbl_rating.AutoSize = True
        Me.lbl_rating.Location = New System.Drawing.Point(167, 110)
        Me.lbl_rating.Name = "lbl_rating"
        Me.lbl_rating.Size = New System.Drawing.Size(10, 13)
        Me.lbl_rating.TabIndex = 12
        Me.lbl_rating.Text = "-"
        '
        'lbl_genres
        '
        Me.lbl_genres.AutoSize = True
        Me.lbl_genres.Location = New System.Drawing.Point(167, 81)
        Me.lbl_genres.Name = "lbl_genres"
        Me.lbl_genres.Size = New System.Drawing.Size(10, 13)
        Me.lbl_genres.TabIndex = 11
        Me.lbl_genres.Text = "-"
        '
        'lbl_year
        '
        Me.lbl_year.AutoSize = True
        Me.lbl_year.Location = New System.Drawing.Point(167, 55)
        Me.lbl_year.Name = "lbl_year"
        Me.lbl_year.Size = New System.Drawing.Size(10, 13)
        Me.lbl_year.TabIndex = 10
        Me.lbl_year.Text = "-"
        '
        'lbl_title
        '
        Me.lbl_title.AutoSize = True
        Me.lbl_title.Location = New System.Drawing.Point(167, 28)
        Me.lbl_title.Name = "lbl_title"
        Me.lbl_title.Size = New System.Drawing.Size(10, 13)
        Me.lbl_title.TabIndex = 9
        Me.lbl_title.Text = "-"
        '
        'pictureBox1
        '
        Me.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pictureBox1.Location = New System.Drawing.Point(24, 28)
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.Size = New System.Drawing.Size(90, 130)
        Me.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pictureBox1.TabIndex = 0
        Me.pictureBox1.TabStop = False
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.treeView1)
        Me.groupBox1.Location = New System.Drawing.Point(12, 60)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(238, 323)
        Me.groupBox1.TabIndex = 14
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Results"
        '
        'treeView1
        '
        Me.treeView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.treeView1.HideSelection = False
        Me.treeView1.Location = New System.Drawing.Point(3, 16)
        Me.treeView1.Name = "treeView1"
        Me.treeView1.Size = New System.Drawing.Size(232, 304)
        Me.treeView1.TabIndex = 0
        '
        'lbl_elapsedt
        '
        Me.lbl_elapsedt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_elapsedt.Location = New System.Drawing.Point(471, 23)
        Me.lbl_elapsedt.Name = "lbl_elapsedt"
        Me.lbl_elapsedt.Size = New System.Drawing.Size(199, 13)
        Me.lbl_elapsedt.TabIndex = 19
        Me.lbl_elapsedt.Text = "ELAPSED TIME: 0"
        Me.lbl_elapsedt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(682, 401)
        Me.Controls.Add(Me.chkDownloadImg)
        Me.Controls.Add(Me.btn_search)
        Me.Controls.Add(Me.txt_title)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.panel1)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.lbl_elapsedt)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.panel1.ResumeLayout(False)
        Me.panel1.PerformLayout()
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents chkDownloadImg As System.Windows.Forms.CheckBox
    Private WithEvents btn_search As System.Windows.Forms.Button
    Private WithEvents txt_title As System.Windows.Forms.TextBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents panel1 As System.Windows.Forms.Panel
    Friend WithEvents label8 As System.Windows.Forms.Label
    Private WithEvents lbl_studio As System.Windows.Forms.Label
    Friend WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents txt_plot As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents lbl_rating As System.Windows.Forms.Label
    Private WithEvents lbl_genres As System.Windows.Forms.Label
    Private WithEvents lbl_year As System.Windows.Forms.Label
    Private WithEvents lbl_title As System.Windows.Forms.Label
    Private WithEvents pictureBox1 As System.Windows.Forms.PictureBox
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents treeView1 As System.Windows.Forms.TreeView
    Private WithEvents lbl_elapsedt As System.Windows.Forms.Label

End Class
