using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Win32;
using XMindAPI.LIB;
using System;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Windows.Input;

/// XMind API for C#
/// ----------------
/// 
/// XMind API test project.
///
/// Note: This code is part of a CodePlex project: http://XMindAPI.codeplex.com
/// 
/// CHANGE LOG:
/// ==================================================================================================================
/// #       AUTHOR                  DATE          DESCRIPTION
/// ------  ----------------------  ------------  --------------------------------------------------------------------
/// 1.      Rudi Rademeyer          2011/06/04    Create. 
/// 2.      Rudi Rademeyer          2012/03/08    Add "Read" tab to test XMind workbook reader functionlity.
/// 
/// ==================================================================================================================
namespace XMindAPI.TestApp
{
    public partial class MainWindow : Window
    {
        private enum apiMode
        {
            Create,
            Read
        }

        private apiMode _mode; 
        private FileData _xMindFileData = new FileData();
        private FileData _reportFileData = new FileData();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Read_Click(object sender, RoutedEventArgs e)
        {
            Cursor prevCursor = Cursor;
            Cursor = Cursors.Wait;

            XMindWorkBook xwb = new XMindWorkBook(_xMindFileData.FileName, true);

            // Read workbook content and format into a report containing parent and child by sheet:
            List<XMindSheet> sheets = xwb.GetSheetInfo();

            List<string> repLines = new List<string>();
            repLines.Add("Parent,Child");

            foreach (XMindSheet sheet in sheets)
            {
                foreach (XMindTopic topic in sheet.TopicFlatList)
                {
                    repLines.Add((topic.Parent == null ? string.Empty : RemoveCrLf(topic.Parent.Name)) + "," + RemoveCrLf(topic.Name));
                }
            }

            File.WriteAllLines(_reportFileData.FileName, repLines.ToArray());

            _reportFileData.OnPropertyChanged("VisibleIfFileExists");
            Cursor = prevCursor;
        }

        private string RemoveCrLf(string str)
        {
            return str.Replace(Convert.ToChar(10).ToString(), " ").Replace(Convert.ToChar(13).ToString(), " ");
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            // Create a new, empty workbook. If the workbook exists it will be overwritten:
            XMindWorkBook xwb = new XMindWorkBook(_xMindFileData.FileName);

            // Create a new sheet (at least one per workbook required):
            string sheetId = xwb.AddSheet("Vehicles");

            string centralTopicId = xwb.AddCentralTopic(sheetId, "Brands",
                XMindStructure.FishboneRightHeaded);

            string mazdaTopicId = xwb.AddTopic(centralTopicId, "Mazda");
            string fordTopicId = xwb.AddTopic(centralTopicId, "Ford");
            string bmwTopicId = xwb.AddTopic(centralTopicId, "BMW");
            string nissanTopicId = xwb.AddTopic(centralTopicId, "Nissan", XMindStructure.SpreadSheet);

            string cx7TopicId = xwb.AddTopic(mazdaTopicId, "CX7");
            xwb.AddTopic(mazdaTopicId, "323");
            xwb.AddTopic(mazdaTopicId, "Mazda6");

            xwb.AddTopic(fordTopicId, "Bantam");
            xwb.AddTopic(fordTopicId, "Focus");
            xwb.AddTopic(fordTopicId, "Ranger");

            xwb.AddTopic(bmwTopicId, "3 series");
            xwb.AddTopic(bmwTopicId, "5 series");
            xwb.AddTopic(bmwTopicId, "7 series");

            xwb.AddTopic(nissanTopicId, "Nirvada");
            xwb.AddTopic(nissanTopicId, "Sentra");
            xwb.AddTopic(nissanTopicId, "Micra");

            xwb.AddLabel(cx7TopicId, "This is a SUV");

            xwb.AddMarker(bmwTopicId, XMindMarkers.FlagBlue);

            xwb.CollapseChildren(bmwTopicId);

            xwb.Save();

            _xMindFileData.OnPropertyChanged("VisibleIfFileExists");
        }

        private void NavigateUri_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            switch (_mode)
            {
                case apiMode.Create:

                    _reportFileData.FileName = null;

                    dlg.CheckFileExists = false;
                    dlg.DefaultExt = ".xmind";
                    dlg.Filter = "XMind Workbooks (.xmind)|*.xmind";

                    if ((bool)dlg.ShowDialog())
                    {
                        _xMindFileData.FileName = dlg.FileName;
                    }
                    else
                    {
                        _xMindFileData.FileName = null;
                    }

                    LayoutRoot.DataContext = _xMindFileData;

                    break;
                case apiMode.Read:
                    
                    dlg.CheckFileExists = true;
                    dlg.DefaultExt = ".xmind";
                    dlg.Filter = "XMind Workbooks (.xmind)|*.xmind";

                    if ((bool)dlg.ShowDialog())
                    {
                        _xMindFileData.FileName = dlg.FileName;

                        // Change the extension to txt for the report: 
                        string[] fileNameStrings = dlg.FileName.Split('.');
                        fileNameStrings[fileNameStrings.Count() - 1] = "txt";

                        StringBuilder txtReportFileName = new StringBuilder(string.Empty, 64);
                        foreach (string str in fileNameStrings)
                        {
                            txtReportFileName.Append(str + ".");
                        }

                        _reportFileData.FileName = txtReportFileName.ToString().TrimEnd('.');

                        // Try to delete the text file report if it exists:
                        File.Delete(_reportFileData.FileName);
                        _reportFileData.OnPropertyChanged("VisibleIfFileExists");
                    }
                    else
                    {
                        _xMindFileData.FileName = null;
                        _reportFileData.FileName = null;
                    }

                    LayoutRoot.DataContext = _reportFileData;
                    
                    break;
            }
        }

        private void Tabs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                _mode = (apiMode)Enum.Parse(_mode.GetType(), (e.AddedItems[0] as TabItem).Tag.ToString());
            }

            LayoutRoot.DataContext = null;
            _xMindFileData.FileName = null;
            _reportFileData.FileName = null;

            switch (_mode)
            {
                case apiMode.Create:
                    LayoutRoot.DataContext = _xMindFileData;
                    break;
                case apiMode.Read:
                    LayoutRoot.DataContext = _reportFileData;
                    break;
            }
        }
    }

    // Data structure for UI data binding:
    public class FileData : INotifyPropertyChanged
    {
        public string FileName 
        { 
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = (value == string.Empty ? null : value);
                OnPropertyChanged("FileName");
                OnPropertyChanged("VisibleIfFileSelected");
                OnPropertyChanged("VisibleIfFileExists");
                OnPropertyChanged("FileUri");
            }
        }
        private string _fileName;

        public string FileUri
        {
            get
            {
                return "file" + "://" + _fileName;
            }
        }

        public Visibility VisibleIfFileSelected
        {
            get
            {
                if (_fileName == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        public Visibility VisibleIfFileExists
        {
            get
            {
                if (_fileName == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return (File.Exists(_fileName) == true ? Visibility.Visible : Visibility.Collapsed);
                }
            }
        }

        public event PropertyChangedEventHandler  PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
