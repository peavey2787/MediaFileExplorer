using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MediaFileExplorer
{
    class Show
    {

        public string GetName()
        {
            var path = GetFullPath();
            if ( string.IsNullOrEmpty( path ) )
                return "Path not defined";
            return Path.GetFileNameWithoutExtension( path );
        }

        public string GetNameWithExtension()
        {
            var path = GetFullPath();
            if (string.IsNullOrEmpty(path))
                return "Path not defined";
            return Path.GetFileName(path);
        }

        private int TimesWatched;

        public int GetTimesWatched()
        {
            return TimesWatched;
        }

        public void SetTimesWatched(int value)
        {
                TimesWatched = value;
        }

        private string FullPath;

        public string GetFullPath()
        {
            return FullPath;
        }

        public void SetFullPath(string value)
        {
            if (!string.IsNullOrEmpty(value))
                FullPath = value;
        }

        private int Rating;

        public int GetRating()
        {
            return Rating;
        }

        public void SetRating(int value)
        {
            if(value <= 5)
                Rating = value;
        }

        private string Category;

        public string GetCategory()
        {
            return Category;
        }

        public void SetCategory(string value)
        {
            if (!string.IsNullOrEmpty(value))
                Category = value;
        }

        public Show(string fullPath, int timesWatched, int rating, string category)
        {
            FullPath = fullPath;
            TimesWatched = timesWatched;
            Rating = rating;
            Category = category;
        }
        public Show(string fullPath)
        {
            FullPath = fullPath;
            TimesWatched = 0;
            Rating = 0;
            Category = "none";
        }
        public Show(Show show)
        {
            FullPath = show.GetFullPath();
            TimesWatched = show.GetTimesWatched();
            Rating = show.GetRating();
            Category = show.GetCategory();
        }
        public Show()
        {
            FullPath = "none.avi";
            TimesWatched = 0;
            Rating = 0;
            Category = "none";
        }
    }


    class ShowsList
    {
        #region Properties
        private string ShowListPath;
        private List<Show> allShowsOnList;

        public string GetListFilePath()
        {
            if (!string.IsNullOrEmpty(ShowListPath))
                return ShowListPath;
            return "none";
        }

        public void SetListFilePath(string value)
        {
            if (!string.IsNullOrEmpty(value) && File.Exists(value))
                ShowListPath = value;
            else
                ShowListPath = "none";
        }
        


        public List<Show> GetAllShowsOnList()
        {
            return allShowsOnList;
        }

        private void SetAllShowsOnList(List<Show> value)
        {
            if(value.Count > 0)
                allShowsOnList = value;
        }

        public ShowsList(string showListPath)
        {
            allShowsOnList = new List<Show>();
            ShowListPath = showListPath;
        }
        #endregion


        public bool AddShowToList(Show show)
        {
            if (ShowIsAlreadyOnList(show.GetFullPath()) || show.GetFullPath() == "none.avi")
                return false;

            allShowsOnList.Add(show);
            updateShowList(new Show(), show);
            return true;
        }

        private void updateShowList(Show oldShow, Show newShow)
        {
            //Load the showlist file and append the new show
            XmlDocument xmlDoc = new XmlDocument();

            if (File.Exists(ShowListPath))
            {
                //while (Utility.IsFileLocked(new FileInfo(ShowListPath))) { System.Threading.Thread.Sleep(50); }
                try { xmlDoc.Load(ShowListPath); }
                catch (Exception e) { MessageBox.Show("Error in LoadShowList " + e.Message + e.InnerException); }
            }
            else
            {
                CreateShowList();
                try { xmlDoc.Load(ShowListPath); }
                catch (Exception e) { MessageBox.Show("Error in LoadShowList " + e.Message + e.InnerException); }
            }

            var rootNode = xmlDoc.DocumentElement;
            XmlNode showNode = xmlDoc.CreateElement("Show");
            showNode.InnerText = newShow.GetFullPath();

            XmlAttribute attribute = xmlDoc.CreateAttribute("Rating");
            attribute.Value = newShow.GetRating().ToString();
            showNode.Attributes.Append(attribute);

            attribute = xmlDoc.CreateAttribute("Watched");
            attribute.Value = newShow.GetTimesWatched().ToString();
            showNode.Attributes.Append(attribute);

            attribute = xmlDoc.CreateAttribute("Category");
            attribute.Value = newShow.GetCategory();
            showNode.Attributes.Append(attribute);

            XmlNodeList showsNode = xmlDoc.GetElementsByTagName("Show");
            foreach (XmlNode node in showsNode)
            {
                if(node.InnerText == newShow.GetFullPath())
                {
                    rootNode.ReplaceChild(showNode, node);
                    xmlDoc.Save(ShowListPath);
                    if (allShowsOnList.FindIndex(ind => ind.GetFullPath() == oldShow.GetFullPath()) != -1)
                        allShowsOnList[allShowsOnList.FindIndex(ind => ind.GetFullPath() == oldShow.GetFullPath())] = newShow;
                    else
                        AddShowToList(newShow);
                    return;
                }
            }
            
            rootNode.AppendChild(showNode);
            AddShowToList(newShow);
            xmlDoc.Save(ShowListPath);
            return;
        }

        public bool ShowIsAlreadyOnList(string name)
        {
            foreach (var item in GetAllShowsOnList())
            {
                if (item.GetFullPath() == name)
                    return true;
            }
            return false;
        }

        public Show GetShowFromList(string name)
        {
            Show match = new Show();
            var allShows = GetAllShowsOnList();
            foreach (var show in allShows)
            {
                if (show.GetFullPath() == name)
                    return show;
            }
            var newShow = new Show(name);
            AddShowToList(newShow);
            return newShow;
        }

        public List<Show> LoadShowList()
        {
            XmlDocument xmlShowList = new XmlDocument();
            List<Show> showList = new List<Show>();

            if (File.Exists(ShowListPath))
            {
                while (Utility.IsFileLocked(new FileInfo(ShowListPath))) { System.Threading.Thread.Sleep(50); }

                try { xmlShowList.Load(ShowListPath); }
                catch (Exception e) { MessageBox.Show("Error in LoadShowList " + e.Message + e.InnerException); }
            }
            else
            {
                CreateShowList();
            }

            var tryAgain = false;
            do
            {
                // Try to read the shows 
                try
                {
                    xmlShowList.Load(ShowListPath);
                    XmlNodeList shows = xmlShowList.GetElementsByTagName("Show");

                    foreach (XmlNode show in shows)
                    {
                        Show showToAdd = new Show();
                        var showPath = show.InnerText;
                        if (!File.Exists(showPath))
                        {
                            DriveInfo[] drives = DriveInfo.GetDrives();
                            foreach (var drive in drives)
                            {
                                if (drive.VolumeLabel == "BigCloud")
                                {
                                    // Check if just the drive letter changed
                                    if (File.Exists(drive.Name + showPath.Substring(4)))
                                    {
                                        showPath = drive.Name + showPath.Substring(4);
                                    }
                                    // If not then search for the file on the BigCloud and update the path
                                    else
                                    {
                                        var allFiles = Directory.GetFiles(drive.Name + "/Movies", "*.*", SearchOption.AllDirectories);
                                        allFiles.Concat( Directory.GetFiles( drive.Name + "/TV", "*.*", SearchOption.AllDirectories) );

                                        foreach (var file in allFiles)
                                        {
                                            if (Path.GetFileName(file) == Path.GetFileName(showPath))
                                            {
                                                showPath = Path.GetFullPath(file);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        showToAdd.SetFullPath(showPath);

                        showToAdd.SetRating(Int32.Parse(show.Attributes.GetNamedItem("Rating").Value));
                        showToAdd.SetTimesWatched(Int32.Parse(show.Attributes.GetNamedItem("Watched").Value));
                        showToAdd.SetCategory(show.Attributes.GetNamedItem("Category").Value);

                        if (ShowIsNotNullOrEmpty(showToAdd))
                            showList.Add(showToAdd);
                    }
                }
                catch (Exception e)// Most problems occur from exiting while reading/writing (Should add a lock to prevent this )
                {
                    if (e.Message.IndexOf("Access to the path") != -1)
                    {
                        tryAgain = true;
                    }
                    else
                    {
                        string error = "Error in LoadShowList " + e.Message + e.InnerException;
                        MessageBox.Show(error);
                    }
                }
            } while (tryAgain);

            if (showList.Count > 0)
                SetAllShowsOnList(showList);
            
            return showList;
        }

        private bool ShowIsNotNullOrEmpty(Show show)
        {
            if (show.GetFullPath() == "none.avi" || string.IsNullOrEmpty(show.GetFullPath()) )
                return false;
            return true;
        }

        private void CreateShowList()
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlNode rootNode = xmlDoc.CreateElement("Library");
            xmlDoc.AppendChild(rootNode);

            XmlNode showNode = xmlDoc.CreateElement("Show");
            showNode.InnerText = "FullPathHere";

            XmlAttribute attribute = xmlDoc.CreateAttribute("Rating");
            attribute.Value = "0";
            showNode.Attributes.Append(attribute);

            attribute = xmlDoc.CreateAttribute("Watched");
            attribute.Value = "0";
            showNode.Attributes.Append(attribute);

            attribute = xmlDoc.CreateAttribute("Category");
            attribute.Value = "none";
            showNode.Attributes.Append(attribute);

            rootNode.AppendChild(showNode);

            xmlDoc.Save(ShowListPath);
        }

        public void UpdateTimesWatched(ListView.ListViewItemCollection items)
        {
            foreach (ListViewItem item in items)
            {
                Show show = (Show)item.Tag;
                var newShow = new Show();
                newShow.SetFullPath(show.GetFullPath());
                newShow.SetRating(show.GetRating());
                newShow.SetCategory(show.GetCategory());
                newShow.SetTimesWatched(show.GetTimesWatched() + 1);
                updateShowList(show, newShow);
            }   
        }


        public void UpdateCategory(Show show)
        {
            updateShowList(show, show);
            /*
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ShowListPath);
            XmlNodeList showsNode = xmlDoc.GetElementsByTagName("Show");
            foreach (XmlNode showElem in showsNode)
            {
                if (showElem.InnerText == show.GetFullPath())
                {
                    showElem.Attributes.GetNamedItem("Category").Value = show.GetCategory();
                    xmlDoc.Save(ShowListPath);
                    return;
                }
            }*/
        }
    }
}
