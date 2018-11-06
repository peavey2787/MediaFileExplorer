using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PopulateListViewAsynchronously
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
        

        private List<Show> allShowsOnList;

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
            allShowsOnList.Add(show);
            updateShowList(show);
            return true;
        }

        private void updateShowList(Show show)
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
            showNode.InnerText = show.GetFullPath();

            XmlAttribute attribute = xmlDoc.CreateAttribute("Rating");
            attribute.Value = show.GetRating().ToString();
            showNode.Attributes.Append(attribute);

            attribute = xmlDoc.CreateAttribute("Watched");
            attribute.Value = show.GetTimesWatched().ToString();
            showNode.Attributes.Append(attribute);

            attribute = xmlDoc.CreateAttribute("Category");
            attribute.Value = show.GetCategory();
            showNode.Attributes.Append(attribute);

            rootNode.AppendChild(showNode);

            xmlDoc.Save(ShowListPath);
        }

        public bool ShowIsAlreadyOnList(string name)
        {
            foreach (var item in GetAllShowsOnList())
            {
                if (item.GetName() == name)
                    return true;
            }
            return false;
        }

        public Show GetShowFromList(string name)
        {
            Show match = new Show();
            foreach (var show in GetAllShowsOnList())
            {
                if (show.GetName() == name)
                    return show;
            }
            return match;
        }

        public List<Show> LoadShowList()
        {
            XmlDocument xmlShowList = new XmlDocument();
            List<Show> showList = new List<Show>();

            if (File.Exists(ShowListPath))
            {
                //while (Utility.IsFileLocked(new FileInfo(ShowListPath))) { System.Threading.Thread.Sleep(50); }

                try { xmlShowList.Load(ShowListPath); }
                catch (Exception e) { MessageBox.Show("Error in LoadShowList " + e.Message + e.InnerException); }
            }
            else
            {
                CreateShowList();
            }            

            // Try to read the shows 
            try
            {
                xmlShowList.Load(ShowListPath);
                XmlNodeList shows = xmlShowList.GetElementsByTagName("Show");
                
                foreach (XmlNode show in shows)
                {
                    Show showToAdd = new Show();
                    showToAdd.SetFullPath(show.InnerText);
                    showToAdd.SetRating( Int32.Parse( show.Attributes.GetNamedItem( "Rating" ).Value ) );
                    showToAdd.SetTimesWatched( Int32.Parse( show.Attributes.GetNamedItem( "Watched" ).Value ) );
                    showToAdd.SetCategory( show.Attributes.GetNamedItem( "Category" ).Value );

                    if (ShowIsNotNullOrEmpty(showToAdd))
                        showList.Add(showToAdd);
                }
            }
            catch (Exception e)// Most problems occur from exiting while reading/writing (Should add a lock to prevent this )
            {
                string error = "Error in LoadShowList " + e.Message + e.InnerException;
                MessageBox.Show(error);
            }

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
            System.Xml.XmlDocument xmlShowList = new System.Xml.XmlDocument();
            xmlShowList.Load(ShowListPath);

            System.Xml.XmlNodeList showsNode = xmlShowList.GetElementsByTagName("Show");
            foreach (System.Xml.XmlNode showElem in showsNode)
            {

                foreach (System.Xml.XmlNode attributes in showElem.ChildNodes)
                {
                    Show listShow = new Show();

                    bool update = false;

                    foreach (ListViewItem item in items)
                    {
                        Show show = (Show)item.Tag;

                            switch (attributes.Name)
                            {
                                case "Name":
                                    if (show.GetName() == attributes.InnerText)
                                    {
                                        update = true;
                                    }
                                    break;
                                case "Watched":
                                    if(update)
                                    {
                                        attributes.InnerText = show.GetTimesWatched().ToString();
                                    }
                                    break;
                            }
                    }
                }                
            }

            //while (Utility.IsFileLocked(new FileInfo(ShowListPath))) { System.Threading.Thread.Sleep(50); }
            xmlShowList.Save(ShowListPath);


        }


        public void UpdateCategory(Show show)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ShowListPath);
            //XmlNodeList xnList = xml.SelectNodes("/Library/Show[@='M']");
            int i = 0;
            XmlNodeList showsNode = xmlDoc.GetElementsByTagName("Show");
            int index = 0;
            foreach (XmlNode showElem in showsNode)
            {
                if (showElem.InnerText == show.GetFullPath())
                {
                    showElem.Attributes.GetNamedItem("Category").Value = show.GetCategory();
                    xmlDoc.Save(ShowListPath);
                    return;
                }
            }
        }
    }
}
