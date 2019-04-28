using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MediaFileExplorer
{
    public partial class MainForm : Form
    {
        #region Form Variables

        ListViewItem clickedPlayListItem;
        List<KeyValuePair<string, int>> lastLocation = new List<KeyValuePair<string, int>>();
        private bool backButtonClicked = false;
        private string InvalidPathError = "Invalid Path!";
        private string defaultMoviesPath = "D:\\Movies";
        private string defaultTVPath = "D:\\TV";
        private string startingPath = "";
        private string playListSavePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
        private string playListExtension = ".xspf";
        private string showListPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ShowList.xml";
        private List<string> categories = new List<string>();
        ShowsList ShowsList;
        private string[] defaultMovieCategories = { "Comdey", "Action", "Drama", "Horror", "Animated", "Vampire", "Chick Flick", "True Story", "Everyone Dies" };

        #endregion

        #region Form Open/Close

        public MainForm()
        {
            InitializeComponent();

            PlayList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            Utility.DoubleBuffering(MoviesListView, true);
            MoviesListView.ShowItemToolTips = true;

            SetupCategoriesList();

            var movies = Properties.Settings.Default.MoviesPath;
            if ( !string.IsNullOrEmpty(movies) && Directory.Exists(movies) )
                Movies.Tag = movies;
            else
                Movies.Tag = defaultMoviesPath;

            var tv = Properties.Settings.Default.TVPath;
            if (!string.IsNullOrEmpty(tv) && Directory.Exists(tv))
                TVShows.Tag = tv;
            else
                TVShows.Tag = defaultTVPath;

            startingPath = Properties.Settings.Default.StartingPath;
            if ( string.IsNullOrEmpty( startingPath) )
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach( var drive in drives )
                {
                    if ( drive.VolumeLabel == "BigCloud" )
                    {
                        startingPath = drive.RootDirectory.FullName + "/Movies";
                        Properties.Settings.Default.StartingPath = startingPath;
                    }
                }
            }
            SetupShowList();
        }

        private void SetupCategoriesList()
        {
            if (Properties.Settings.Default.Categories == null)
            {
                Properties.Settings.Default.Categories = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.Categories.AddRange(defaultMovieCategories);
            }
            
            categories = categories.Union(Properties.Settings.Default.Categories.Cast<string>().ToList()).ToList();

            foreach(var category in categories)
            {
                var li = new ListViewItem(category);
                searchByCategoryListView.Items.Add(li);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            while (!MoviesListView.Visible) { Thread.Sleep(50); }
            MoviesListView.Columns.Add(" ", MoviesListView.Width);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Categories.AddRange(categories.ToArray()); 
            Properties.Settings.Default.MoviesPath = Movies.Tag.ToString();
            Properties.Settings.Default.TVPath = TVShows.Tag.ToString();
            Properties.Settings.Default.Save();
        }

        private async void SetupShowList()
        {
            ShowsList = new ShowsList(showListPath);

            await Task.Run(() => { ShowsList.LoadShowList(); }).ContinueWith(t =>
            {
            //ShowsList.AddShowToList(new Show("Time Machine",0,"none",0,"disney") );
            //ShowsList.SyncMediaFiles();
            if (Directory.Exists(startingPath))
            {
                Utility.InvokeIfRequired(moviesPath, () => { moviesPath.Text = startingPath; } );
                    PopulateListView(startingPath);
                }
            });
        }
        #endregion

        private async void PopulateListView(string path)
        {
            int selectedIndex = 0;
            var previousLocation = "";

            Utility.InvokeIfRequired(listViewProgressBar, () => { listViewProgressBar.Visible = true; });
            Utility.InvokeIfRequired(ProgressLabel, () => { ProgressLabel.Text = "Starting to get files..."; });
            Utility.InvokeIfRequired(moviesPath, () => { previousLocation = moviesPath.Text; moviesPath.Text = path; });
            Utility.InvokeIfRequired(MoviesListView, () => { 
                if (MoviesListView.SelectedItems.Count > 0)
                    selectedIndex = MoviesListView.SelectedIndices[0];
                MoviesListView.Items.Clear();
            });

            if (!backButtonClicked && !File.Exists(path) && lastLocation.Count == 0 || lastLocation.Last().Key != path)
                    lastLocation.Add(new KeyValuePair<string, int>(previousLocation, selectedIndex));             

            await Task.Run(async () =>
            {
                if (Directory.Exists(path))
                {
                    var movies = Directory.GetFiles(path);

                    var listItemsFiles = new List<ListViewItem>();
                    var listItemsFolders = new List<ListViewItem>();
                    var show = new Show(); 

                    foreach (var movie in movies)
                    {
                        if ( Utility.IsMediaFile( movie ) && 
                            ( movie.ToLower() ).IndexOf("sample") != 0)
                        {
                            await Task.Run(() => {
                                show = ShowsList.GetShowFromList(movie);
                                show.GetCategory();
                                if ( ShowsList.ShowIsAlreadyOnList( show.GetName() ) )
                                    show = ShowsList.GetShowFromList( show.GetFullPath() );
                                else
                                    ShowsList.AddShowToList(show);
                            });

                            ListViewItem li = new ListViewItem();
                            li.Text = show.GetName();
                            li.Name = show.GetFullPath();
                            li.Tag = show;
                            li.ImageKey = "unknown.png";
                            li.ToolTipText = GenerateShowToolTipText(show);
                            listItemsFiles.Add(li);
                        }                        
                    }


                    Utility.InvokeIfRequired(listViewProgressBar, () => { listViewProgressBar.Visible = false; });

                    var folders = Directory.GetDirectories(path);
                    foreach (var folder in folders)
                    {
                        ListViewItem li = new ListViewItem();
                        var added = false;

                        // If movie is only media file inside folder only add the movie
                        show = new Show(Path.GetFullPath(folder));
                        var subFiles = Directory.GetFiles(folder);

                        if (subFiles.Length <= 10)
                        {
                            foreach (var file in subFiles)
                            {
                                if (Utility.IsMediaFile(file))
                                {
                                    var fi = new FileInfo(file);
                                    // 60mbs
                                    if (fi.Length >= 60000000)
                                    {
                                        // If there are more than 1 movie in the folder then add the folder
                                        if (added)
                                        {
                                            added = false;
                                            show.SetFullPath(Path.GetFullPath(folder));
                                            listItemsFiles.RemoveAt(listItemsFiles.Count - 1);
                                            break;
                                        }
                                        li = new ListViewItem();
                                        show.SetFullPath(fi.FullName);
                                        li.Text = show.GetName();
                                        li.Name = show.GetFullPath();
                                        li.Tag = show;
                                        li.ImageKey = "unknown.png";
                                        li.ToolTipText = GenerateShowToolTipText(show);
                                        listItemsFiles.Add(li); 
                                        added = true;
                                    }
                                }
                            }
                        }

                        if (!added)
                        {
                            li.Text = show.GetName();
                            li.Name = show.GetFullPath();
                            li.Tag = show;
                            li.ImageKey = "folder.png";
                            listItemsFolders.Add(li);
                        }
                    }
                    Utility.InvokeIfRequired(MoviesListView, () => { MoviesListView.Items.AddRange(listItemsFiles.ToArray()); });
                    Utility.InvokeIfRequired(MoviesListView, () => { MoviesListView.Items.AddRange(listItemsFolders.ToArray()); });

                    
                    Utility.InvokeIfRequired(ProgressLabel, () => { ProgressLabel.Text = "Finished getting all files."; });
                }

                if (backButtonClicked && MoviesListView.Items.Count >= lastLocation.Last().Value)
                {
                    Utility.InvokeIfRequired(MoviesListView, () =>
                    {
                        MoviesListView.EnsureVisible(lastLocation.Last().Value);
                        MoviesListView.Items[lastLocation.Last().Value].Selected = true;
                        MoviesListView.Select();
                    });

                    lastLocation.RemoveAt(lastLocation.Count - 1);
                    backButtonClicked = false;
                }
            });
        }



        #region Movies/Back/Clear list Clicks

        private async void getMovies_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(moviesPath.Text))
                PopulateListView(moviesPath.Text);
            else
            {
                Utility.InvokeIfRequired(listViewProgressBar, () => { listViewProgressBar.Visible = true; });
                Utility.InvokeIfRequired(ProgressLabel, () => { ProgressLabel.Text = "Searching for files..."; });
                Utility.InvokeIfRequired(MoviesListView, () => { MoviesListView.Items.Clear(); });

                await Task.Run( () =>
                {
                    var listItemsFiles = new List<ListViewItem>();
                    DriveInfo[] drives = DriveInfo.GetDrives();
                    foreach (var drive in drives)
                    {
                        if (drive.VolumeLabel == "BigCloud")
                        {
                            var allFiles = Directory.GetFiles(drive.Name + "/Movies", "*.*", SearchOption.AllDirectories);
                            allFiles.Concat(Directory.GetFiles(drive.Name + "/TV", "*.*", SearchOption.AllDirectories));

                            foreach (var file in allFiles)
                            {
                                if ( Utility.IsMediaFile( file ) && Path.GetFileName( file ).ToLower().IndexOf( moviesPath.Text.ToLower() ) != -1 )
                                {
                                    ListViewItem li = new ListViewItem();
                                    var show = new Show(Path.GetFullPath(file));
                                    li.Text = show.GetName();
                                    li.Name = show.GetFullPath();
                                    li.Tag = show;
                                    li.ImageKey = "unknown.png";
                                    li.ToolTipText = GenerateShowToolTipText(show);
                                    listItemsFiles.Add(li);
                                }
                            }

                            if (listItemsFiles.Count == 0)
                            {
                                ListViewItem li = new ListViewItem();
                                var show = new Show("FULL PATH HERE");
                                li.Text = "No Shows Found!";
                                li.Name = show.GetFullPath();
                                li.Tag = show;
                                li.ImageKey = "unknown.png";
                                li.ToolTipText = GenerateShowToolTipText(show);
                                listItemsFiles.Add(li);
                            }
                        }
                    }
                    Utility.InvokeIfRequired(listViewProgressBar, () => { listViewProgressBar.Visible = false; });
                    Utility.InvokeIfRequired(MoviesListView, () => { MoviesListView.Items.AddRange(listItemsFiles.ToArray()); });
                    Utility.InvokeIfRequired(ProgressLabel, () => { ProgressLabel.Text = "Finished getting all files."; });
                });              

            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            if ( lastLocation.Count > 1 && Directory.Exists(lastLocation.Last().Key) )
            {
                backButtonClicked = true;
                PopulateListView(lastLocation.Last().Key);
            }
        }
        private void ClearList_Click(object sender, EventArgs e)
        {
            PlayList.Items.Clear();
        }
        
        #endregion
        
        #region Movies Button

        private void Movies_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                PopulateListView(Movies.Tag.ToString());
        }

        private void Movies_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
            {
                ChangeMoviesLocation.TextBox.Text = Movies.Tag.ToString();
                ResizeContextMenu(ChangeMoviesPath);
                ChangeMoviesPath.Show(Cursor.Position);
            }
        }
        
        private void ChangeMoviesLocation_KeyDown(object sender, KeyEventArgs e)
        {
            var path = ChangeMoviesLocation.TextBox.Text.ToString();

            if (e.KeyData == Keys.Enter && Directory.Exists(path))
            {
                Movies.Tag = path;
                moviesPath.Text = path;
                ChangeMoviesPath.Hide();
            }
            else if (e.KeyData == Keys.Enter && !Directory.Exists(path))
                ChangeMoviesPath.Items.Add(InvalidPathError);
        }

        private void ChangeMoviesLocation_TextChanged(object sender, EventArgs e)
        {
            ResizeContextMenu(ChangeMoviesPath);

            if (ChangeMoviesPath.Items.ContainsKey(InvalidPathError))
                ChangeMoviesPath.Items.RemoveByKey(InvalidPathError);
        }
        #endregion

        #region TV Button
        private void TVShows_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                PopulateListView(TVShows.Tag.ToString());
        }

        private void TVShows_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
            {
                ChangeTVLocation.TextBox.Text = TVShows.Tag.ToString();
                ChangeTVPath.Show(Cursor.Position);
                ResizeContextMenu(ChangeTVPath);
            }
        }

        private void ChangeTVLocation_KeyDown(object sender, KeyEventArgs e)
        {
            var path = ChangeTVLocation.TextBox.Text.ToString();

            if (e.KeyData == Keys.Enter && Directory.Exists(path) )
            {
                TVShows.Tag = path;
                moviesPath.Text = path;
                ChangeTVPath.Hide();
            }
            else if(e.KeyData == Keys.Enter && !Directory.Exists(path) )
                ChangeTVPath.Items.Add(InvalidPathError);
        }

        private void ChangeTVLocation_TextChanged(object sender, EventArgs e)
        {
            ResizeContextMenu(ChangeTVPath);
            if (ChangeTVPath.Items.ContainsKey(InvalidPathError))
                ChangeTVPath.Items.RemoveByKey(InvalidPathError);
        }

        #endregion

        // For Movies/TV Buttons
        public void ResizeContextMenu (Control control)
        {
            Size size = TextRenderer.MeasureText(control.Controls[1].Text, control.Font);
            control.Controls[1].Width = size.Width;
            control.Width += size.Width;
            control.Controls[1].Width = size.Width;
        }


        #region Shows Available List View

        private void MoviesListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = MoviesListView.HitTest(e.X, e.Y);
            ListViewItem selectedItem = info.Item;
            Show show = (Show) selectedItem.Tag;

            if (selectedItem != null && e.Button == MouseButtons.Left)
            {
                if (File.Exists(show.GetFullPath()))
                {
                    ListViewItem li = new ListViewItem(show.GetName());
                    li.Name = show.GetName();
                    li.Tag = show;
                    PlayList.Items.Add(li);
                }

                else if (e.Clicks > 1 && Directory.Exists(show.GetFullPath()))
                {
                    PopulateListView(show.GetFullPath());
                }
            }
            else
                MoviesListView.SelectedItems.Clear(); // No item selected            
        }

        private void MoviesListView_MouseDown(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = MoviesListView.HitTest(e.X, e.Y);
            ListViewItem li = info.Item;

            if (li != null && e.Button == MouseButtons.Left)
            {
                // single click
            }
            else if (e.Button == MouseButtons.Right)
            {
                Show selected = (Show)li.Tag;
                Show show = ShowsList.GetShowFromList(selected.GetName());
                MoviesListView.ContextMenuStrip = CreateCategoryContextMenu(show);
            }
            else
            {
                MoviesListView.SelectedItems.Clear();
                //"No item is selected";
            }
        }

        private ContextMenuStrip CreateCategoryContextMenu(Show show)
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.ShowCheckMargin = false;
            cms.ShowImageMargin = false;
            cms.ShowItemToolTips = false;

            ToolStripDropDownButton dropDown = new ToolStripDropDownButton("Categories");
            dropDown.DropDownDirection = ToolStripDropDownDirection.Right;
            dropDown.DropDownItemClicked += DropDown_DropDownItemClicked;
            dropDown.DropDown.Closing += DropDown_Closing;
            dropDown.MouseHover += dropDown_MouseHover;

            ToolStripTextBox txtbox = new ToolStripTextBox();
            txtbox.Click += Txtbox_Click;
            txtbox.KeyDown += AddCategoriesTxtbox_KeyDown;
            txtbox.Text = "Add Category";
            dropDown.DropDownItems.Add(txtbox);

            foreach (var category in categories)
            {                
                ToolStripMenuItem item = new ToolStripMenuItem(category);
                item.Name = category;

                if (show.GetCategory() == category)
                    item.Checked = true;

                dropDown.DropDownItems.Add(item);
            }

            cms.Items.Add(dropDown);

            cms.MinimumSize = new Size(100, cms.Height);
            return cms;
        }

        private void dropDown_MouseHover(object sender, EventArgs e)
        {
            var send = (ToolStripDropDownButton)sender;
            send.ShowDropDown();
        }

        private void AddCategoriesTxtbox_KeyDown(object sender, KeyEventArgs e)
        {
            var txtbox = ((ToolStripTextBox)sender);

            if (e.KeyCode == Keys.Enter)
            {
                var category = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtbox.Text);
                var dropDown = txtbox.GetCurrentParent();

                if (!categories.Contains(category))
                {
                    txtbox.Text = "Add Category";                    

                    ToolStripMenuItem newCategory = new ToolStripMenuItem(category);
                    newCategory.Name = category;
                    dropDown.Items.Add(newCategory);
                    categories.Add(category);
                    searchByCategoryListView.Items.Add(category);
                }
                else
                {
                    //txtbox.Text = string.Empty;
                    ToolTip addCategoryToolTip = new ToolTip();
                    addCategoryToolTip.ToolTipIcon = ToolTipIcon.Error;
                    addCategoryToolTip.Show("   Category already in the list!", txtbox.Control, 5000);
                }
            }
            else if(txtbox.Text == "Add Category")
            {
                txtbox.Text = string.Empty;
            }
        }

        private void Txtbox_Click(object sender, EventArgs e)
        {
            ((ToolStripTextBox)sender).Text = string.Empty;
        }

        private void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
            {
                e.Cancel = true;
            }
        }

        private void DropDown_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ListViewItem li = MoviesListView.SelectedItems[0];
            var show = (Show)li.Tag;
            var dropDownButton = (ToolStripDropDownButton)sender;
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)e.ClickedItem;

            if (!clickedItem.Checked)
            {
                clickedItem.Checked = true;
                show.SetCategory(clickedItem.Text);
                li.Tag = show;
                li.ToolTipText = GenerateShowToolTipText(show);
                ShowsList.UpdateCategory(show);

                foreach (ToolStripItem item in dropDownButton.DropDownItems)
                {
                    if (item.Text != clickedItem.Text && !(item is ToolStripTextBox) )
                    {
                        ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                        if (tsmi.Checked)
                        {
                            tsmi.Checked = false;
                            break;
                        }
                    }
                }
                dropDownButton.GetCurrentParent().Hide();
            }
        }

        private string GenerateShowToolTipText(Show show)
        {
            return "Watched: " + show.GetTimesWatched() + Environment.NewLine
            + "Rating: " + show.GetRating() + Environment.NewLine
            + "Category: " + show.GetCategory();
        }


        #endregion

        #region Vlc Playlist

        private void Play_MouseClick(object sender, MouseEventArgs e)
        {
            if(PlayList.Items.Count > 0 && !string.IsNullOrEmpty(PlaylistFileName.Text) )
            {
                var savePath = playListSavePath + PlaylistFileName.Text + playListExtension;
                if (File.Exists(savePath))
                    File.Delete(savePath);
                CreateVLCPlaylist(PlayList.Items, savePath);
                System.Diagnostics.Process.Start(savePath);
                ShowsList.UpdateTimesWatched(PlayList.Items);
            }
            else if(string.IsNullOrEmpty(PlaylistFileName.Text))
                MessageBox.Show("Please enter a Playlist filename!");
            else if (PlayList.Items.Count == 0)
                MessageBox.Show("Please add at least 1 show to the Playlist!");
        }

        private void PlaylistFileName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && !string.IsNullOrEmpty(PlaylistFileName.Text))
                playListSavePath = PlaylistFileName.Text;
        }

        private void PlaylistFileName_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(PlaylistFileName.Text) && PlaylistFileName.Text.Length < 255)
            {
                PlaylistFileName.Text = Utility.CleanFileName(PlaylistFileName.Text);
                PlaylistFileName.SelectionStart = PlaylistFileName.Text.Length;
            }
        }

        private void CreateVLCPlaylist(ListView.ListViewItemCollection shows, string savePath)
        {
            using (XmlTextWriter xmldoc = new XmlTextWriter(savePath, Encoding.UTF8))
            {
                xmldoc.Formatting = Formatting.Indented;
                xmldoc.Indentation = 2;
                xmldoc.WriteStartDocument(true);                                //<xml start>
                xmldoc.WriteStartElement("playlist");                           //<Playlist version="1" xmlns="http://xspf.org/ns/0/" xmlns:vls="http://www.videolan.org/vlc/playlist/ns/0/">
                xmldoc.WriteStartAttribute("version");
                xmldoc.WriteValue(1);
                xmldoc.WriteEndAttribute();
                xmldoc.WriteStartAttribute("xmlns");
                xmldoc.WriteValue("http://xspf.org/ns/0/");
                xmldoc.WriteEndAttribute();
                xmldoc.WriteStartAttribute("xmlns:vlc");
                xmldoc.WriteValue("http://www.videolan.org/vlc/playlist/ns/0/");
                xmldoc.WriteEndAttribute();
                xmldoc.WriteStartElement("title");                                  //<title>
                xmldoc.WriteString("Playlist");                                        //Playlist
                xmldoc.WriteEndElement();                                           //</title>             
                xmldoc.WriteStartElement("trackList");                              //<tracklist>

                int i = 0;
                Show show = new Show();
                foreach (ListViewItem item in shows)
                {
                    show = (Show)item.Tag;
                    if (item != null && !string.IsNullOrEmpty(item.Text) && !string.IsNullOrEmpty(item.Name))
                    {
                        //START TRACK        
                        xmldoc.WriteStartElement("track");
                        xmldoc.WriteStartElement("location");
                        //PUT LOCATION OF FILE HERE
                        xmldoc.WriteString("file:///" + show.GetFullPath() );
                        xmldoc.WriteEndElement();//</location>
                        xmldoc.WriteStartElement("title");
                        //PUT TITLE OF AMediaFile HERE 
                        xmldoc.WriteString( show.GetNameWithExtension() );
                        xmldoc.WriteEndElement();//</title>
                        xmldoc.WriteStartElement("duration");
                        //PUT HOW LONG THE MediaFile IS HERE
                        xmldoc.WriteString("0:0:0");
                        xmldoc.WriteEndElement();//</duration>
                        xmldoc.WriteStartElement("extension");
                        xmldoc.WriteStartAttribute("application");
                        xmldoc.WriteValue("http://www.videolan.org/vlc/playlist/0");
                        xmldoc.WriteEndAttribute();
                        xmldoc.WriteStartElement("vlc:id");
                        //PUT THE COUNTER HERE
                        xmldoc.WriteValue(i);
                        xmldoc.WriteEndElement();//</vlc:id>
                        xmldoc.WriteEndElement();//</extension>
                        xmldoc.WriteEndElement();//</track>
                        i++;
                        //END TRACK
                    }
                }

                xmldoc.WriteEndElement();                                           //</tracklist>
                                                                                    //start
                xmldoc.WriteStartElement("extension");
                xmldoc.WriteStartAttribute("application");
                xmldoc.WriteValue("http://www.videolan.org/vlc/playlist/0");
                xmldoc.WriteEndAttribute();

                for (int d = 0; d < i; d++)
                {
                    xmldoc.WriteStartElement("vlc:item");
                    xmldoc.WriteStartAttribute("tid");
                    xmldoc.WriteValue(d);
                    xmldoc.WriteEndAttribute();
                    xmldoc.WriteEndElement();//</vlc:item>
                }
                //end
                xmldoc.WriteEndElement();                                       //</extension>
                xmldoc.WriteEndElement();                                       //</Playlist>
                xmldoc.WriteEndDocument();                                      //<xml end>              
            }

        }

        private void PlayList_MouseDown(object sender, MouseEventArgs e)
        {
            clickedPlayListItem = PlayList.GetItemAt(e.X, e.Y);
        }

        private void PlayList_MouseMove(object sender, MouseEventArgs e)
        {
            if (clickedPlayListItem == null)
                return;

            // Show the user that a drag operation is happening
            Cursor = Cursors.Hand;
        }

        private void PlayList_MouseUp(object sender, MouseEventArgs e)
        {
            // use 0 instead of e.X so that you don't have
            // to keep inside the columns while dragging
            ListViewItem itemOver = PlayList.GetItemAt(0, e.Y);

            if (itemOver == null)
                return;

            Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);

            // find out if we insert before or after the item the mouse is over
            bool insertBefore;
            if (e.Y < rc.Top + (rc.Height / 2))
                insertBefore = true;
            else
                insertBefore = false;

            // if we dropped the item on itself, nothing is to be done
            if (clickedPlayListItem != itemOver)
            {
                if (insertBefore)
                {
                    PlayList.Items.Remove(clickedPlayListItem);
                    PlayList.Items.Insert(itemOver.Index, clickedPlayListItem);
                }
                else
                {
                    PlayList.Items.Remove(clickedPlayListItem);
                    PlayList.Items.Insert(itemOver.Index + 1, clickedPlayListItem);
                }
            }
        }


        #endregion

        private void removeFromPlaylist_Click(object sender, EventArgs e)
        {
            try
            {
                PlayList.SelectedItems[0].Remove();
            }
            catch (Exception) { }
        }

        private void moviesPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                getMovies.PerformClick();
            }
        }

        private bool searchByCategoryToggle = false;
        private void searchByCategoryButton_Click(object sender, EventArgs e)
        {
            if (searchByCategoryToggle)
            {
                searchByCategoryListView.Visible = false;
                searchByCategoryToggle = false;
            }
            else
            {
                searchByCategoryListView.Visible = true;
                searchByCategoryToggle = true;
            }
        }
        private void searchByCategoryListView_Click(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            searchByCategoryListView.Visible = false;
            searchByCategoryToggle = false;
            Controls["categoryPanel"].Visible = false;
        }
        private async void searchByCategoryListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (searchByCategoryListView.SelectedItems.Count > 0)
            {
                Utility.InvokeIfRequired(listViewProgressBar, () => { listViewProgressBar.Visible = true; });
                Utility.InvokeIfRequired(ProgressLabel, () => { ProgressLabel.Text = "Searching for files..."; });
                Utility.InvokeIfRequired(MoviesListView, () => { MoviesListView.Items.Clear(); });
                var searchCategory = searchByCategoryListView.SelectedItems[0].Text;

                await Task.Run(() =>
                {
                    var listItemsFiles = new List<ListViewItem>();
                    foreach (var show in ShowsList.GetAllShowsOnList())
                    {
                        if (show.GetCategory() == searchCategory)
                        {
                            ListViewItem li = new ListViewItem();
                            li.Text = show.GetName();
                            li.Name = show.GetFullPath();
                            li.Tag = show;
                            li.ImageKey = "unknown.png";
                            li.ToolTipText = GenerateShowToolTipText(show);
                            listItemsFiles.Add(li);
                        }
                    }

                    if (listItemsFiles.Count == 0)
                    {
                        ListViewItem li = new ListViewItem();
                        var show = new Show("FULL PATH HERE");
                        li.Text = "No Shows Found!";
                        li.Name = show.GetFullPath();
                        li.Tag = show;
                        li.ImageKey = "unknown.png";
                        li.ToolTipText = GenerateShowToolTipText(show);
                        listItemsFiles.Add(li);
                    }

                    Utility.InvokeIfRequired(listViewProgressBar, () => { listViewProgressBar.Visible = false; });
                    Utility.InvokeIfRequired(MoviesListView, () => { MoviesListView.Items.AddRange(listItemsFiles.ToArray()); });
                    Utility.InvokeIfRequired(ProgressLabel, () => { ProgressLabel.Text = "Finished getting all files."; });
                });
            }
        }

        private void searchByCategoryListView_MouseDown(object sender, MouseEventArgs e)
        {
            searchByCategoryListView.Visible = false;
            searchByCategoryToggle = false;
            Controls["categoryPanel"].Visible = false;
        }

        private void searchByCategoryListView_Leave(object sender, EventArgs e)
        {
            searchByCategoryListView.Visible = false;
            searchByCategoryToggle = false;
            Controls["categoryPanel"].Visible = false;
        }

        private void searchByCategoryListView_MouseLeave(object sender, EventArgs e)
        {
            var mousePos = PointToClient(MousePosition);
            if ( !searchByCategoryListView.Bounds.Contains(mousePos) )
            { 
                searchByCategoryListView.Visible = false;
                searchByCategoryToggle = false;
                Controls["categoryPanel"].Visible = false;
            }
        }
        
        private void searchByCategoryListView_MouseEnter(object sender, EventArgs e)
        {
            int x = 0;
            Panel panel = new Panel();
            panel.Name = "categoryPanel";
            panel.Top = searchByCategoryListView.Location.Y + 1;
            panel.Left = searchByCategoryListView.Location.X + 100;
            panel.Height = searchByCategoryListView.Height - 20;
            panel.Width = 50;
            panel.BackColor = Color.White;

            foreach (ListViewItem item in searchByCategoryListView.Items)
            {
                Button btn = new Button();
                btn.Name = "categoryRemoveButton" + item.Text;
                btn.Click += searchByCategoryRemoveItem_Click;
                btn.Top = item.Position.Y + 1;
                btn.Left = 0;// searchByCategoryListView.Location.X + item.Position.X + 130;
                btn.Width = 30;
                btn.Height = 18;
                btn.Text = "X";
                panel.Controls.Add(btn);
                btn.BringToFront();
                x++;
            }
            Controls.Add(panel);
            panel.BringToFront();
        }

        private void searchByCategoryRemoveItem_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var name = btn.Name.Substring(20);
            
            if ( categories.Contains( name ) )
            {
                categories.Remove(name);
                Properties.Settings.Default.Categories.AddRange(categories.ToArray());
                searchByCategoryListView.FindItemWithText(name).Remove();
                searchByCategoryListView.Refresh();
                btn.Dispose();
            }
        }
    }


    public static class Utility
    {
        public static void InvokeIfRequired(this Control control, MethodInvoker action)
        {
            /*while (!control.Visible)
            {
                Thread.Sleep(50);
            }*/

            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static bool IsMediaFile(string path)
        {
            string[] mediaExtensions = {
                ".AVI", ".MP4", ".MKV", ".M4V", ".FLV",
                ".MPG", ".MPEG", ".OGG", ".WMA", ".WAV",
                ".DIVX", ".WMV",
            };

            return -1 != Array.IndexOf(mediaExtensions, Path.GetExtension(path).ToUpperInvariant());
        }

        public static string CleanFileName(string input)
        {
            string cleanString = "";
            foreach (char letter in input)
            {
                if (char.IsLetterOrDigit(letter) && letter != ' ')
                    cleanString += letter;
                else
                    cleanString += '_';
            }
            return cleanString;
        }

        public static void DoubleBuffering(this Control control, bool enable)
        {
            var method = typeof(Control).GetMethod("SetStyle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            method.Invoke(control, new object[] { ControlStyles.OptimizedDoubleBuffer, enable });
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
