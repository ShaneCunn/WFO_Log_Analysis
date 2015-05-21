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

namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for EditForDiagramWindow.xaml
    /// </summary>
    public partial class EditForDiagramWindow : Window
    {

        CheckBox graphCheckbox = new CheckBox();
        public EditForDiagramWindow()
        {
            InitializeComponent();
            
        }

        private void graphOptionDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            graphOptionDataGrid.Columns[0].Width = 24;

            StreamReader readGraphOption = new StreamReader(@"C:\Users\rgavin\Documents\aawfo\Code\WFO 1.1\graphOptions.txt");
            //StreamWriter writeGraphOption = new StreamWriter();
            string line;
            List<graphViewItems> options = new List<graphViewItems>();

            while ((line = readGraphOption.ReadLine()) != null)
            {
                graphViewItems anOption = new graphViewItems();

                //string name = line;
                anOption.graphNameColumn = line;
                options.Add(anOption);
                
                //Console.WriteLine(line);

            }
            graphOptionDataGrid.ItemsSource = options;
        }

        public class graphViewItems
        {

            //public bool comboBox { get; set; }
            public string graphNameColumn { get; set; }
            public bool graphCheckboxColumn { get; set; }
        }

        private void graphOptionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void editcheckboxheader_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void editcheckboxheader_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void editcheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void editcheckbox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void graphOptionsAdd_Click(object sender, RoutedEventArgs e)
        {
            graphOptionDataGrid.IsReadOnly = false;
            graphOptionDataGrid.CanUserAddRows = true;
        }
    }
}
