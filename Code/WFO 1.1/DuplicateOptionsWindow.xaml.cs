using System;
using System.IO;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for DuplicateOptionsWindow.xaml
    /// </summary>
    public partial class DuplicateOptionsWindow : Window
    {
        List<string> duplicates;
        string selectedScript;
        string choice = null;
        public DuplicateOptionsWindow(List<string> _duplicates, string _selectedScript)
        {
            InitializeComponent();
            duplicates = _duplicates;
            selectedScript = _selectedScript;
        }

        private void duplicatesListView_Loaded(object sender, RoutedEventArgs e)
        {
            duplicatesListView.ItemsSource = duplicates;
        }

        private void duplicatesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            choice = duplicatesListView.SelectedValue.ToString();

            var scriptFileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
            StreamReader pasteReader = new StreamReader(scriptFileName);
            string tempfile = System.IO.Path.GetTempFileName();
            StreamWriter pasteWriter = new StreamWriter(tempfile);
            string scriptLine;

            while ((scriptLine = pasteReader.ReadLine()) != null)
            {
                pasteWriter.WriteLine(scriptLine);
                if (scriptLine == "name :" + selectedScript)
                {
                    pasteWriter.WriteLine(choice);
                }
            }
            pasteReader.Close();
            pasteWriter.Close();
            pasteReader = new StreamReader(tempfile);
            pasteWriter = new StreamWriter(scriptFileName);

            while ((scriptLine = pasteReader.ReadLine()) != null)
            {
                pasteWriter.WriteLine(scriptLine);

            }
            pasteWriter.Close();
            pasteReader.Close();
            duplicatesWindow.Close();
        }

        private void duplicatesWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (choice == null)
            {
                if (MessageBox.Show("Are you sure you wish to close without adding one of these options?", "Close Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
