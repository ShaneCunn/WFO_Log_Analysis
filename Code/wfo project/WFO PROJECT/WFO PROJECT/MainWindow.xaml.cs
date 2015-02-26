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
using System.IO;
using System.Text.RegularExpressions;

namespace WFO_PROJECT
{    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string NameValue;
        string authorName;
        string reg_String;
        string splitName = "";
        string[] words;
        string[] word_GrepLine;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartString_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            // ... Change Window Title.            
            string grepStartValue = textBox.Text;

           // this.Title = grepStartValue;

        }

        private void EndString_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            var textBoxEnd = sender as TextBox;
            string grepEndValue = textBoxEnd.Text;
           // this.Title = grepEndValue;

        }

        private void AboveLine_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            var textBoxLineVariable = sender as TextBox;
            string aboveValue = textBoxLineVariable.Text;
           // this.Title = aboveValue;

        }

        private void BelowLine_TextChanged_3(object sender, TextChangedEventArgs e)
        {
            var textBoxLine = sender as TextBox;
            string belowValue = textBoxLine.Text;
            //this.Title = belowValue;
        }

        private void Create_File(object sender, RoutedEventArgs e)
        {
            string temp_File = @"C:\Users\flanaganc\Desktop\blahblah.txt";
            if (NameValue != "")
            {
                  this.Title = NameValue;   
            }
            using (StreamWriter sw = File.AppendText(temp_File))
            {
                sw.Write("\r\n\r\n");
                sw.Write("Name = "+ NameValue);
                sw.Write("\r\n\r\n");
                sw.Write("# Author: {0}", authorName);
                sw.Write("\r\n\r\n");
                sw.Write("grep -n -E \".*" + reg_String + ".*\"");
            }

        }

        private void Edit_File(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();

        }       

        private void regularExpression_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBoxRegVariable = sender as TextBox;
            reg_String = textBoxRegVariable.Text;
        }

        private void Script_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBoxLine = sender as TextBox;
            NameValue =  textBoxLine.Text ;            
        }

        private void Creator_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBoxLine = sender as TextBox;
            authorName = textBoxLine.Text;  

        }
              
        public void SelectFile_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                

            }

        }
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {

            var fileName = @"C:\Users\flanaganc\Desktop\abc.txt";
            splitName = System.IO.Path.GetFileName(fileName);            
            string line;
            System.IO.StreamReader file =
            new System.IO.StreamReader(splitName);
            List<string> data = new List<string>();            
            data.Add("");
            while ((line = file.ReadLine()) != null)
            {
                Regex regex = new Regex("Name: ");
                if (regex.IsMatch(line))
                {
                    words = line.Split(' ');                    
                    data.Add(words[1].ToString());                   
                }

            }
            
          

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

            // ... Make the first item selected.
            comboBox.SelectedIndex = 0;
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
            string file_Line;
            var search_Name = @"C:\Users\flanaganc\Desktop\abc.txt";
            splitName = System.IO.Path.GetFileName(search_Name);
            string temp_File = @"C:\Users\flanaganc\Desktop\blahblah.txt";
            System.IO.StreamReader file =
            new System.IO.StreamReader(splitName);
            while ((file_Line = file.ReadLine()) != null)
            {
                Regex regex = new Regex("Name: ");
                if (regex.IsMatch(file_Line))
                {
                    word_GrepLine = file_Line.Split(' ');
                    if (word_GrepLine[1] == value)
                    {
                        file_Line = file.ReadLine();
                        if (file_Line.StartsWith("G"))
                        {
                            using (StreamWriter sw = File.AppendText(temp_File))
                            {
                                sw.Write(file_Line);                                
                            }
                        }

                    }
                }

            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

       
    }
    public class Details
    {
        public string startString { get; set; }
        public string endString { get; set; }
        public string singleString { get; set; }
        public int linesAbove { get; set; }
        public int linesBelow { get; set; }
        public string startTime { get; set; }
        public int endTime { get; set; }
    } 
    


}

