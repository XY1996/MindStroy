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
using FreeMinLib.Reader;
using FreeMinLib.MindInfo;

namespace XMindAPI.TestApp
{
    /// <summary>
    /// FreeMind.xaml 的交互逻辑
    /// </summary>
    public partial class FreeMind : Window
    {

        private ITreeNodeGenerate reader;
        private TreeNode data;

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
                data = reader.GenerateTreeNode();
            }
            Stack<TreeNode> result = new Stack<TreeNode>();
            result.Push(data);
            while (result.Any())
            {
                TreeNode node=result.Pop();
                Debug.WriteLine(node.Text);
                foreach (var rightNode in node.RightChildNode)
                {
                    result.Push(rightNode);
                }
                foreach (var leftNode in node.LeftChildNode)
                {
                    result.Push(leftNode);
                }
              

            }
            //string json = JsonConvert.SerializeObject(data);
            //data= JsonConvert.DeserializeObject<List<FreeMindNode>>(json);
        }
    }
}
