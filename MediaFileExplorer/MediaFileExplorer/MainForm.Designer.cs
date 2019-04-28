namespace MediaFileExplorer
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.getMovies = new System.Windows.Forms.Button();
            this.moviesPath = new System.Windows.Forms.TextBox();
            this.MoviesListView = new System.Windows.Forms.ListView();
            this.LargeImageList = new System.Windows.Forms.ImageList(this.components);
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.Movies = new System.Windows.Forms.Button();
            this.ChangeMoviesPath = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.ChangeMoviesLocation = new System.Windows.Forms.ToolStripTextBox();
            this.TVShows = new System.Windows.Forms.Button();
            this.ChangeTVPath = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.ChangeTVLocation = new System.Windows.Forms.ToolStripTextBox();
            this.Back = new System.Windows.Forms.Button();
            this.PlayList = new System.Windows.Forms.ListView();
            this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Play = new System.Windows.Forms.Button();
            this.PlaylistFileName = new System.Windows.Forms.TextBox();
            this.ClearList = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewProgressBar = new System.Windows.Forms.ProgressBar();
            this.removeFromPlaylist = new System.Windows.Forms.Button();
            this.searchByCategoryListView = new System.Windows.Forms.ListView();
            this.CategoriesHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.searchByCategoryButton = new System.Windows.Forms.Button();
            this.ChangeMoviesPath.SuspendLayout();
            this.ChangeTVPath.SuspendLayout();
            this.SuspendLayout();
            // 
            // getMovies
            // 
            this.getMovies.Location = new System.Drawing.Point(53, 5);
            this.getMovies.Name = "getMovies";
            this.getMovies.Size = new System.Drawing.Size(98, 20);
            this.getMovies.TabIndex = 0;
            this.getMovies.Text = "Get Movies";
            this.getMovies.UseVisualStyleBackColor = true;
            this.getMovies.Click += new System.EventHandler(this.getMovies_Click);
            // 
            // moviesPath
            // 
            this.moviesPath.Location = new System.Drawing.Point(157, 5);
            this.moviesPath.Name = "moviesPath";
            this.moviesPath.Size = new System.Drawing.Size(377, 20);
            this.moviesPath.TabIndex = 1;
            this.moviesPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.moviesPath_KeyDown);
            // 
            // MoviesListView
            // 
            this.MoviesListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.MoviesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MoviesListView.FullRowSelect = true;
            this.MoviesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.MoviesListView.HideSelection = false;
            this.MoviesListView.LabelEdit = true;
            this.MoviesListView.LargeImageList = this.LargeImageList;
            this.MoviesListView.Location = new System.Drawing.Point(10, 71);
            this.MoviesListView.MultiSelect = false;
            this.MoviesListView.Name = "MoviesListView";
            this.MoviesListView.Size = new System.Drawing.Size(821, 519);
            this.MoviesListView.TabIndex = 2;
            this.MoviesListView.UseCompatibleStateImageBehavior = false;
            this.MoviesListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MoviesListView_MouseDoubleClick);
            this.MoviesListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoviesListView_MouseDown);
            // 
            // LargeImageList
            // 
            this.LargeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("LargeImageList.ImageStream")));
            this.LargeImageList.TransparentColor = System.Drawing.Color.Maroon;
            this.LargeImageList.Images.SetKeyName(0, "unknown.png");
            this.LargeImageList.Images.SetKeyName(1, "folder.png");
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.Location = new System.Drawing.Point(12, 595);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(72, 13);
            this.ProgressLabel.TabIndex = 3;
            this.ProgressLabel.Text = "Progress here";
            // 
            // Movies
            // 
            this.Movies.Location = new System.Drawing.Point(167, 31);
            this.Movies.Name = "Movies";
            this.Movies.Size = new System.Drawing.Size(103, 34);
            this.Movies.TabIndex = 4;
            this.Movies.Tag = "";
            this.Movies.Text = "Movies";
            this.Movies.UseVisualStyleBackColor = true;
            this.Movies.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Movies_MouseClick);
            this.Movies.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Movies_MouseDown);
            // 
            // ChangeMoviesPath
            // 
            this.ChangeMoviesPath.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.ChangeMoviesLocation});
            this.ChangeMoviesPath.Name = "ChangeMoviesPath";
            this.ChangeMoviesPath.ShowImageMargin = false;
            this.ChangeMoviesPath.Size = new System.Drawing.Size(136, 47);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.toolStripTextBox1.Enabled = false;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.ReadOnly = true;
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 16);
            this.toolStripTextBox1.Text = "Enter Path: ";
            // 
            // ChangeMoviesLocation
            // 
            this.ChangeMoviesLocation.AcceptsReturn = true;
            this.ChangeMoviesLocation.Name = "ChangeMoviesLocation";
            this.ChangeMoviesLocation.Size = new System.Drawing.Size(100, 23);
            this.ChangeMoviesLocation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChangeMoviesLocation_KeyDown);
            this.ChangeMoviesLocation.TextChanged += new System.EventHandler(this.ChangeMoviesLocation_TextChanged);
            // 
            // TVShows
            // 
            this.TVShows.Location = new System.Drawing.Point(291, 32);
            this.TVShows.Name = "TVShows";
            this.TVShows.Size = new System.Drawing.Size(100, 33);
            this.TVShows.TabIndex = 5;
            this.TVShows.Tag = "";
            this.TVShows.Text = "TV Shows";
            this.TVShows.UseVisualStyleBackColor = true;
            this.TVShows.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TVShows_MouseClick);
            this.TVShows.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TVShows_MouseDown);
            // 
            // ChangeTVPath
            // 
            this.ChangeTVPath.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox2,
            this.ChangeTVLocation});
            this.ChangeTVPath.Name = "ChangeMoviesPath";
            this.ChangeTVPath.ShowImageMargin = false;
            this.ChangeTVPath.Size = new System.Drawing.Size(136, 47);
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.toolStripTextBox2.Enabled = false;
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.ReadOnly = true;
            this.toolStripTextBox2.Size = new System.Drawing.Size(100, 16);
            this.toolStripTextBox2.Text = "Enter Path: ";
            // 
            // ChangeTVLocation
            // 
            this.ChangeTVLocation.AcceptsReturn = true;
            this.ChangeTVLocation.Name = "ChangeTVLocation";
            this.ChangeTVLocation.Size = new System.Drawing.Size(100, 23);
            this.ChangeTVLocation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChangeTVLocation_KeyDown);
            this.ChangeTVLocation.TextChanged += new System.EventHandler(this.ChangeTVLocation_TextChanged);
            // 
            // Back
            // 
            this.Back.Location = new System.Drawing.Point(10, 32);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(88, 31);
            this.Back.TabIndex = 6;
            this.Back.Text = "Back";
            this.Back.UseVisualStyleBackColor = true;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // PlayList
            // 
            this.PlayList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlayList.AutoArrange = false;
            this.PlayList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Title});
            this.PlayList.FullRowSelect = true;
            this.PlayList.GridLines = true;
            this.PlayList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.PlayList.Location = new System.Drawing.Point(840, 71);
            this.PlayList.MultiSelect = false;
            this.PlayList.Name = "PlayList";
            this.PlayList.Size = new System.Drawing.Size(190, 519);
            this.PlayList.TabIndex = 7;
            this.PlayList.UseCompatibleStateImageBehavior = false;
            this.PlayList.View = System.Windows.Forms.View.Details;
            this.PlayList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PlayList_MouseDown);
            this.PlayList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PlayList_MouseMove);
            this.PlayList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PlayList_MouseUp);
            // 
            // Title
            // 
            this.Title.Text = "Title";
            this.Title.Width = 184;
            // 
            // Play
            // 
            this.Play.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Play.Location = new System.Drawing.Point(969, 28);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(61, 39);
            this.Play.TabIndex = 8;
            this.Play.Text = "Play";
            this.Play.UseVisualStyleBackColor = true;
            this.Play.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Play_MouseClick);
            // 
            // PlaylistFileName
            // 
            this.PlaylistFileName.AcceptsReturn = true;
            this.PlaylistFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PlaylistFileName.Location = new System.Drawing.Point(840, 5);
            this.PlaylistFileName.Name = "PlaylistFileName";
            this.PlaylistFileName.Size = new System.Drawing.Size(190, 20);
            this.PlaylistFileName.TabIndex = 9;
            this.PlaylistFileName.Text = "VLC_Playlist";
            this.PlaylistFileName.TextChanged += new System.EventHandler(this.PlaylistFileName_TextChanged);
            this.PlaylistFileName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PlaylistFileName_KeyDown);
            // 
            // ClearList
            // 
            this.ClearList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearList.Location = new System.Drawing.Point(840, 46);
            this.ClearList.Name = "ClearList";
            this.ClearList.Size = new System.Drawing.Size(59, 21);
            this.ClearList.TabIndex = 10;
            this.ClearList.Text = "Clear List";
            this.ClearList.UseVisualStyleBackColor = true;
            this.ClearList.Click += new System.EventHandler(this.ClearList_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(761, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Playlist Name:";
            // 
            // listViewProgressBar
            // 
            this.listViewProgressBar.Location = new System.Drawing.Point(278, 198);
            this.listViewProgressBar.MarqueeAnimationSpeed = 20;
            this.listViewProgressBar.Name = "listViewProgressBar";
            this.listViewProgressBar.Size = new System.Drawing.Size(321, 23);
            this.listViewProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.listViewProgressBar.TabIndex = 12;
            this.listViewProgressBar.Visible = false;
            // 
            // removeFromPlaylist
            // 
            this.removeFromPlaylist.Location = new System.Drawing.Point(905, 46);
            this.removeFromPlaylist.Name = "removeFromPlaylist";
            this.removeFromPlaylist.Size = new System.Drawing.Size(58, 21);
            this.removeFromPlaylist.TabIndex = 13;
            this.removeFromPlaylist.Text = "Remove";
            this.removeFromPlaylist.UseVisualStyleBackColor = true;
            this.removeFromPlaylist.Click += new System.EventHandler(this.removeFromPlaylist_Click);
            // 
            // searchByCategoryListView
            // 
            this.searchByCategoryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CategoriesHeader});
            this.searchByCategoryListView.HideSelection = false;
            this.searchByCategoryListView.HoverSelection = true;
            this.searchByCategoryListView.Location = new System.Drawing.Point(531, 32);
            this.searchByCategoryListView.Name = "searchByCategoryListView";
            this.searchByCategoryListView.Size = new System.Drawing.Size(156, 255);
            this.searchByCategoryListView.TabIndex = 14;
            this.searchByCategoryListView.UseCompatibleStateImageBehavior = false;
            this.searchByCategoryListView.View = System.Windows.Forms.View.Details;
            this.searchByCategoryListView.Visible = false;
            this.searchByCategoryListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.searchByCategoryListView_ItemSelectionChanged);
            this.searchByCategoryListView.Leave += new System.EventHandler(this.searchByCategoryListView_Leave);
            this.searchByCategoryListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.searchByCategoryListView_MouseDown);
            this.searchByCategoryListView.MouseEnter += new System.EventHandler(this.searchByCategoryListView_MouseEnter);
            this.searchByCategoryListView.MouseLeave += new System.EventHandler(this.searchByCategoryListView_MouseLeave);
            // 
            // CategoriesHeader
            // 
            this.CategoriesHeader.Text = "Categories:";
            this.CategoriesHeader.Width = 151;
            // 
            // searchByCategoryButton
            // 
            this.searchByCategoryButton.Location = new System.Drawing.Point(422, 32);
            this.searchByCategoryButton.Name = "searchByCategoryButton";
            this.searchByCategoryButton.Size = new System.Drawing.Size(112, 31);
            this.searchByCategoryButton.TabIndex = 15;
            this.searchByCategoryButton.Text = "Search By Category:";
            this.searchByCategoryButton.UseVisualStyleBackColor = true;
            this.searchByCategoryButton.Click += new System.EventHandler(this.searchByCategoryButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 617);
            this.Controls.Add(this.searchByCategoryButton);
            this.Controls.Add(this.searchByCategoryListView);
            this.Controls.Add(this.removeFromPlaylist);
            this.Controls.Add(this.listViewProgressBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ClearList);
            this.Controls.Add(this.PlaylistFileName);
            this.Controls.Add(this.Play);
            this.Controls.Add(this.PlayList);
            this.Controls.Add(this.Back);
            this.Controls.Add(this.TVShows);
            this.Controls.Add(this.Movies);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.MoviesListView);
            this.Controls.Add(this.moviesPath);
            this.Controls.Add(this.getMovies);
            this.Name = "MainForm";
            this.Text = "Media File Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ChangeMoviesPath.ResumeLayout(false);
            this.ChangeMoviesPath.PerformLayout();
            this.ChangeTVPath.ResumeLayout(false);
            this.ChangeTVPath.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getMovies;
        private System.Windows.Forms.TextBox moviesPath;
        private System.Windows.Forms.ListView MoviesListView;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.ImageList LargeImageList;
        private System.Windows.Forms.Button Movies;
        private System.Windows.Forms.ContextMenuStrip ChangeMoviesPath;
        private System.Windows.Forms.ToolStripTextBox ChangeMoviesLocation;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.Button TVShows;
        private System.Windows.Forms.ContextMenuStrip ChangeTVPath;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.ToolStripTextBox ChangeTVLocation;
        private System.Windows.Forms.Button Back;
        private System.Windows.Forms.ListView PlayList;
        private System.Windows.Forms.ColumnHeader Title;
        private System.Windows.Forms.Button Play;
        private System.Windows.Forms.TextBox PlaylistFileName;
        private System.Windows.Forms.Button ClearList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar listViewProgressBar;
        private System.Windows.Forms.Button removeFromPlaylist;
        private System.Windows.Forms.ListView searchByCategoryListView;
        private System.Windows.Forms.Button searchByCategoryButton;
        private System.Windows.Forms.ColumnHeader CategoriesHeader;
    }
}

