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
using System.Windows.Navigation;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for EditForDiagramWindow.xaml
    /// </summary>
    public partial class EditForDiagramWindow : Window
    {
        List<graphViewItems> options = new List<graphViewItems>();
        List<string> stuffs = new List<string>();

        CheckBox graphCheckbox = new CheckBox();
        public EditForDiagramWindow()
        {
            InitializeComponent();

            StreamReader readGraphOption = new StreamReader(Directory.GetCurrentDirectory() + "\\graphOptions.txt");
            //StreamWriter writeGraphOption = new StreamWriter();
            string line;
            options = new List<graphViewItems>();

            while ((line = readGraphOption.ReadLine()) != null)
            {
                string anOption;
                anOption = line;
                stuffs.Add(anOption);
            }
            readGraphOption.Close();
        }


        private void graphOptionDataGrid_Loaded(object sender, RoutedEventArgs e)
        {

            StreamReader readGraphOption = new StreamReader(Directory.GetCurrentDirectory() + "\\graphOptions.txt");
            string line;
            options = new List<graphViewItems>();

            while ((line = readGraphOption.ReadLine()) != null)
            {
                graphViewItems anOption = new graphViewItems();
                anOption.graphNameColumn = line;
                options.Add(anOption);
            }
            graphOptionDataGrid.ItemsSource = options;
            readGraphOption.Close();
        }

        public class graphViewItems
        {
            public string graphNameColumn { get; set; }
        }

        private void graphOptionsSave_Click(object sender, RoutedEventArgs e)
        {
            graphOptionsSaveFunction();
        }

        private void graphOptionsSaveFunction()
        {
            stuffs.Clear();
            StreamWriter writeGraphOption = new StreamWriter(Directory.GetCurrentDirectory() + "\\graphOptions.txt");
            graphOptionDataGrid.SelectAll();

            foreach (graphViewItems anOption in graphOptionDataGrid.ItemsSource)
            {
                writeGraphOption.WriteLine(anOption.graphNameColumn);
                stuffs.Add(anOption.graphNameColumn);
            }
            writeGraphOption.Close();
        }

        private void graphOptionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void graphOptionsClose_Click(object sender, RoutedEventArgs e)
        {
            bool change = false;
            int listCount = stuffs.Count();
            List<string> newList = new List<string>();
            foreach (graphViewItems Stuffa in graphOptionDataGrid.ItemsSource)
            {
                newList.Add(Stuffa.graphNameColumn);
            }

            if ((!stuffs.All(newList.Contains)) || (!newList.All(stuffs.Contains)) || (newList.Count != stuffs.Count))
            {
                if (MessageBox.Show("Do you wish to save changes before closing?", "Save Changes", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    graphOptionsSaveFunction();
                }
                diagramWindow.Close();
            }
            else
            {
                diagramWindow.Close();
            }
        }
    }
}
