using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateCheckboxes();

        }

        private void CreateCheckboxes()
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("C:\\Users\\rgavin\\Source\\Workspaces\\WFO  GREP project\\Code\\Scripts.txt");
            while((line = file.ReadLine()) != null)
            {
                List<CheckBox> NumList = new List<CheckBox>();
                Regex regex = new Regex("name");
                if (regex.IsMatch(line))
                {
                    string[] words = line.Split(' ');
                    CheckBox box = new CheckBox();
                    box.Tag = words[1];
                    box.Content = words[1];
                    ListView1.Items.Add(box);
                }

            }
            //List<CheckBox> NumList = new List<CheckBox>();
            //for (int i = 0; i < 50; i++)
            //{
            //    CheckBox box = new CheckBox();
            //    box.Tag = i.ToString();
            //    box.Content = i;
            //    NumList.Add(box);

            //    ListView1.Items.Add(NumList[i]);
            //} 

        }

        private void StartString_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            // ... Change Window Title.            
            string grepStartValue = textBox.Text;

            this.Title = grepStartValue;

        }

        private void EndString_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            var textBoxEnd = sender as TextBox;
            string grepEndValue = textBoxEnd.Text;
            this.Title = grepEndValue;

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            var textBoxLineVariable = sender as TextBox;
            string afterValue = textBoxLineVariable.Text;
            this.Title = afterValue;

        }

        private void TextBox_TextChanged_3(object sender, TextChangedEventArgs e)
        {
            var textBoxLine = sender as TextBox;
            string beforeValue = textBoxLine.Text;
            this.Title = beforeValue;
        }

        private void ListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
        }

       
    }
}
