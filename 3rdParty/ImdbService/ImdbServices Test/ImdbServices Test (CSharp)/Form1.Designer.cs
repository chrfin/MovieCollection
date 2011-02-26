namespace ImdbServices_Test
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_studio = new System.Windows.Forms.Label();
            this.btn_search = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_plot = new System.Windows.Forms.TextBox();
            this.txt_title = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Label2 = new System.Windows.Forms.Label();
            this.lbl_rating = new System.Windows.Forms.Label();
            this.lbl_genres = new System.Windows.Forms.Label();
            this.lbl_year = new System.Windows.Forms.Label();
            this.lbl_title = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_elapsedt = new System.Windows.Forms.Label();
            this.chkDownloadImg = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(3, 16);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(232, 304);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(123, 138);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Studio:";
            // 
            // lbl_studio
            // 
            this.lbl_studio.AutoSize = true;
            this.lbl_studio.Location = new System.Drawing.Point(167, 138);
            this.lbl_studio.Name = "lbl_studio";
            this.lbl_studio.Size = new System.Drawing.Size(10, 13);
            this.lbl_studio.TabIndex = 19;
            this.lbl_studio.Text = "-";
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(268, 18);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(75, 23);
            this.btn_search.TabIndex = 9;
            this.btn_search.Text = "Search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(123, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Plot:";
            // 
            // txt_plot
            // 
            this.txt_plot.BackColor = System.Drawing.Color.White;
            this.txt_plot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_plot.Location = new System.Drawing.Point(170, 162);
            this.txt_plot.Multiline = true;
            this.txt_plot.Name = "txt_plot";
            this.txt_plot.ReadOnly = true;
            this.txt_plot.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_plot.Size = new System.Drawing.Size(218, 75);
            this.txt_plot.TabIndex = 0;
            // 
            // txt_title
            // 
            this.txt_title.Location = new System.Drawing.Point(58, 20);
            this.txt_title.Name = "txt_title";
            this.txt_title.Size = new System.Drawing.Size(189, 20);
            this.txt_title.TabIndex = 8;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(123, 110);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(41, 13);
            this.Label5.TabIndex = 16;
            this.Label5.Text = "Rating:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Title:";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(123, 81);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(44, 13);
            this.Label4.TabIndex = 15;
            this.Label4.Text = "Genres:";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(123, 55);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(32, 13);
            this.Label3.TabIndex = 14;
            this.Label3.Text = "Year:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.lbl_studio);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txt_plot);
            this.panel1.Controls.Add(this.Label5);
            this.panel1.Controls.Add(this.Label4);
            this.panel1.Controls.Add(this.Label3);
            this.panel1.Controls.Add(this.Label2);
            this.panel1.Controls.Add(this.lbl_rating);
            this.panel1.Controls.Add(this.lbl_genres);
            this.panel1.Controls.Add(this.lbl_year);
            this.panel1.Controls.Add(this.lbl_title);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(268, 76);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(402, 304);
            this.panel1.TabIndex = 10;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(123, 28);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(30, 13);
            this.Label2.TabIndex = 13;
            this.Label2.Text = "Title:";
            // 
            // lbl_rating
            // 
            this.lbl_rating.AutoSize = true;
            this.lbl_rating.Location = new System.Drawing.Point(167, 110);
            this.lbl_rating.Name = "lbl_rating";
            this.lbl_rating.Size = new System.Drawing.Size(10, 13);
            this.lbl_rating.TabIndex = 12;
            this.lbl_rating.Text = "-";
            // 
            // lbl_genres
            // 
            this.lbl_genres.AutoSize = true;
            this.lbl_genres.Location = new System.Drawing.Point(167, 81);
            this.lbl_genres.Name = "lbl_genres";
            this.lbl_genres.Size = new System.Drawing.Size(10, 13);
            this.lbl_genres.TabIndex = 11;
            this.lbl_genres.Text = "-";
            // 
            // lbl_year
            // 
            this.lbl_year.AutoSize = true;
            this.lbl_year.Location = new System.Drawing.Point(167, 55);
            this.lbl_year.Name = "lbl_year";
            this.lbl_year.Size = new System.Drawing.Size(10, 13);
            this.lbl_year.TabIndex = 10;
            this.lbl_year.Text = "-";
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Location = new System.Drawing.Point(167, 28);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(10, 13);
            this.lbl_title.TabIndex = 9;
            this.lbl_title.Text = "-";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(24, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(90, 130);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 323);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Results";
            // 
            // lbl_elapsedt
            // 
            this.lbl_elapsedt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_elapsedt.Location = new System.Drawing.Point(471, 23);
            this.lbl_elapsedt.Name = "lbl_elapsedt";
            this.lbl_elapsedt.Size = new System.Drawing.Size(199, 13);
            this.lbl_elapsedt.TabIndex = 12;
            this.lbl_elapsedt.Text = "ELAPSED TIME: 0";
            this.lbl_elapsedt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkDownloadImg
            // 
            this.chkDownloadImg.AutoSize = true;
            this.chkDownloadImg.Location = new System.Drawing.Point(351, 23);
            this.chkDownloadImg.Name = "chkDownloadImg";
            this.chkDownloadImg.Size = new System.Drawing.Size(137, 17);
            this.chkDownloadImg.TabIndex = 13;
            this.chkDownloadImg.Text = "Download movie poster";
            this.chkDownloadImg.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 401);
            this.Controls.Add(this.chkDownloadImg);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.txt_title);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbl_elapsedt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        internal System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbl_studio;
        private System.Windows.Forms.Button btn_search;
        internal System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_plot;
        private System.Windows.Forms.TextBox txt_title;
        internal System.Windows.Forms.Label Label5;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Label lbl_rating;
        private System.Windows.Forms.Label lbl_genres;
        private System.Windows.Forms.Label lbl_year;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_elapsedt;
        private System.Windows.Forms.CheckBox chkDownloadImg;

    }
}

