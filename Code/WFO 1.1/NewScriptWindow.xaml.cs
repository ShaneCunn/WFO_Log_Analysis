using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for NewScriptWindow.xaml
    /// </summary>
    public partial class NewScriptWindow : Window
    {
        public NewScriptWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            List<string> scriptList = new List<string>();
            string line;
            var fileName = Directory.GetCurrentDirectory() + "\\ListViewScriptsTwo.txt";
            System.IO.StreamReader file_open = new System.IO.StreamReader(fileName);
            while ((line = file_open.ReadLine()) != null)
            {
                Regex regex = new Regex("name :");
                if (regex.IsMatch(line))
                {
                    string[] words = line.Split(':');
                    scriptList.Add(words[1]);
                }
            }
            file_open.Close();

            string newScriptName = scriptNamebox.Text;

            if (string.IsNullOrEmpty(scriptNamebox.Text))
            {
                MessageBox.Show("You must enter a name for the new script first.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                //MessageBox.Show("You must enter a name for the new script first.");
                return;
            }
            else
            {


                foreach (string words in scriptList)
                {
                    if (words == newScriptName.TrimEnd().TrimStart())
                    {
                        MessageBox.Show("This name is already used.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                        //MessageBox.Show("This name is already used");
                        return;
                    }
                }

            }

            string temp_File = Directory.GetCurrentDirectory() + "\\ListViewScriptsTwo.txt";
            StreamWriter sWriter = File.AppendText(temp_File);
            sWriter.Write("name :" + newScriptName);
            sWriter.Write("\r\n");
            sWriter.Write("--");
            sWriter.Write("\r\n");

            sWriter.Close();
            NewScriptWindow1.Close();
        }
    }
}
