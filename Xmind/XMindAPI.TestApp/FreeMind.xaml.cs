using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace XMindAPI.TestApp
{
    /// <summary>
    /// FreeMind.xaml 的交互逻辑
    /// </summary>
    public partial class FreeMind : Window
    {

        private FreeMindeReader reader;
        private List<FreeMindNode> data;
        private Dictionary<string, FreeMindNode> dic;


        public FreeMind()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();


            dlg.CheckFileExists = true;
            dlg.DefaultExt = ".xmind";
            dlg.Filter = "FreeMind Workbooks|*.xml;*.mm";
            dlg.Multiselect = false;

            if ((bool)dlg.ShowDialog())
            {
                reader = new FreeMindeReader(dlg.FileName);
            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (reader != null)
            {
                data = reader.SelectNodes();
                dic = reader.Dic;
            }
            Queue<string> commands = reader.CommandQueue();
            while (commands.Any())
                Debug.WriteLine(commands.Dequeue());
            //string json = JsonConvert.SerializeObject(data);
            //data= JsonConvert.DeserializeObject<List<FreeMindNode>>(json);
        }
    }
}
