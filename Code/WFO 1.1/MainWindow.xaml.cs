//new

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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string value;
        List<string> data = new List<string>();
        string NameValue;
        string authorName;
        string temp_copy;
        string splitName = "";
        string[] grepName;
        string[] line_words;
        string[] word_GrepLine;
        string file_Line;
        string outputFileName = Directory.GetCurrentDirectory();
        int count = 0;
        string grepStartValue;
        string grepEndValue;
        string aboveValue;
        string belowValue;
        string file_Name;
        int currentCell;

        List<CheckBox> OptionsList = new List<CheckBox>();
        CheckBox box = new CheckBox();


        public MainWindow()
        {
            InitializeComponent();
        }
        public void CreateCheckboxes()
        {
        }

        public void box_CLick(object sender, RoutedEventArgs e)
        {
        }

        private void StartString_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            grepStartValue = textBox.Text;
            this.Title = grepStartValue;
        }

        private void EndString_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            var textBoxEnd = sender as TextBox;
            grepEndValue = textBoxEnd.Text;
            this.Title = grepEndValue;
        }



        private void AboveLine_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            var textBoxLineVariable = sender as TextBox;
            aboveValue = textBoxLineVariable.Text;
            this.Title = aboveValue;
        }

        private void BelowLine_TextChanged_3(object sender, TextChangedEventArgs e)
        {
            var textBoxLine = sender as TextBox;
            belowValue = textBoxLine.Text;
            this.Title = belowValue;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                file_Name = dlg.FileName;
                File_Label.Content = System.IO.Path.GetFileName(file_Name);

            }
            ListView1.IsEnabled = true;
            GrepButton.IsEnabled = true;
        }

        private void GrepButton_Click(object sender, RoutedEventArgs e)
        {
            string searchWord = "";

            //searchWord = searchWord.Remove();
            //Console.WriteLine(file_Name);
            //int lineCount = 0;            
            char[] MyCharList = { '\\', '*', '.', '"' };
            //string tempPerlFileAddress = Directory.GetCurrentDirectory() + @"\TempScript.txt";
            foreach (CheckBox Option in OptionsList)
            {
                string nextLine = "";
                //lineCount += 1;
                if (Option.IsChecked == true)
                {
                    //searchWord = String.Empty;
                    string line;
                    var fileName = Directory.GetCurrentDirectory() + @"\RegScripts.txt";

                    StreamReader file = new StreamReader(fileName);
                    //StreamWriter writefile = new StreamWriter(fileName);

                    while ((line = file.ReadLine()) != null)
                    {

                        Regex regex = new Regex("name :");
                        if (regex.IsMatch(line))
                        {
                            //writefile.WriteAsync("aaah");
                            string[] script_CheckboxName = line.Split(':');
                            if (script_CheckboxName[1] == Option.Content.ToString())
                            {

                                while ((nextLine = file.ReadLine()) != "--")
                                {

                                    string[] words = nextLine.Split('"');
                                    string[] args = (words[0]).Split(' ');
                                    //string arg = args[4];

                                    searchWord = searchWord + " \"" + words[1] + ":" + args[4] + ":" + args[6] + "\"";
                                    //someText = someText + nextLine + "--";
                                    //    if (nextLine == "-")
                                    //    {
                                    //        break;
                                    //    }
                                    //    string[] search = nextLine.Split(' ');
                                    //    string searchWord = "inum=833275000008366";
                                    //    //searchWord = searchWord.TrimEnd(MyCharList);
                                    //    perlCalled(searchWord);
                                    //    //someText = someText + nextLine + Environment.NewLine;
                                    //}
                                }
                            }
                        }
                    }
                }

            }
            string searchWordEile = searchWord;         
            perlCalled(searchWordEile);
           
        }



        private void perlCalled(string searchWord2)
        {

            //Console.WriteLine(searchWord2);
            searchWord2 = "\"" + file_Name + "\"" + " " + "\"" + outputFileName + "\"" + " " + searchWord2;
            ProcessStartInfo perlStartInfo = new ProcessStartInfo("perl.exe");
            perlStartInfo.Arguments = string.Format("StringSearchWithNLines.pl" + " " + searchWord2);
            perlStartInfo.UseShellExecute = false;
            perlStartInfo.RedirectStandardOutput = true;
            perlStartInfo.RedirectStandardError = true;
            perlStartInfo.CreateNoWindow = true;

            Process perl = new Process();
            perl.StartInfo = perlStartInfo;
            perl.Start();
            perl.WaitForExit();
            //searchWord2 = null;
            //MessageBox.Show("File Created!!");

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("checkbox is unchecked");
        }

        private void Checkbox_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ListView1_Loaded(object sender, RoutedEventArgs e)
        {
            OptionsList.Clear();
            ListView1.Items.Clear();
            string line;
            var fileName = Directory.GetCurrentDirectory() + "\\RegScripts.txt";
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                Regex regex = new Regex("name :");
                if (regex.IsMatch(line))
                {

                    string[] words = line.Split(':');
                    CheckBox comboBox = new CheckBox();
                    string boxName = words[1];
                    //comboBox.Name = boxName;
                    comboBox.Content = (words[1]).ToString();
                    comboBox.IsChecked = false;
                    OptionsList.Add(comboBox);
                    ListView1.Items.Add(comboBox);
                    comboBox.Checked += CheckBox_Checked;
                    comboBox.Unchecked += CheckBox_Unchecked;
                }
            }
            file.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var comboBox = sender as ComboBox;
            // ... Set SelectedItem as Window Title.           
            value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
            if (value == "Add new script?")
            {
                showScriptCreation();
            }
            else
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", string.Empty);
                var search_Name = Directory.GetCurrentDirectory() + "\\RegScripts.txt";
                splitName = System.IO.Path.GetFileName(search_Name);
                StreamReader file = new StreamReader(splitName);
                temp_copy = Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt";
                StreamWriter sw = File.AppendText(temp_copy);
                hideScriptCreation();
                Regex regexx = new Regex("name :");
                while ((file_Line = file.ReadLine()) != null)
                {
                    if (regexx.IsMatch(file_Line))
                    {
                        word_GrepLine = file_Line.Split(':');
                    }
                    else
                    {
                        if (word_GrepLine[1] == value)
                        {
                            if (file_Line.StartsWith("grep"))
                            {
                                for (int i = 0; i < 999; i++)
                                {
                                    if (file_Line.StartsWith("name :") || file_Line.StartsWith("-"))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (file_Line.StartsWith("grep"))
                                        {
                                            CheckBox fnsdfn = new CheckBox();

                                            sw.Write(file_Line);
                                            sw.Write("\r\n");
                                            file_Line = file.ReadLine();
                                        }
                                        else
                                        {
                                            break;
                                        }

                                        count++;
                                    }
                                }
                            }

                            if (regexx.IsMatch(file_Line))
                            {
                                break;
                            }
                        }
                    }

                }
                file.Close();
                sw.Close();
                dataGridView1.ItemsSource = LoadCollectionData();
            }
        }






        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {

            data.Clear();
            ComboOne.UpdateDefaultStyle();
            var fileName = Directory.GetCurrentDirectory() + "\\RegScripts.txt";
            splitName = System.IO.Path.GetFileName(fileName);
            string line;
            StreamReader file =
            new StreamReader(splitName);
            data.Add("");
            data.Add("Add new script?");
            while ((line = file.ReadLine()) != null)
            {
                Regex regex = new Regex("name :");
                if (regex.IsMatch(line))
                {
                    line_words = line.Split(':');
                    data.Add(line_words[1].ToString());
                }
            }
            file.Close();
        }

        private void Create_File(object sender, RoutedEventArgs e)
        {

            string temp_File = Directory.GetCurrentDirectory() + "\\RegScripts.txt";
            StreamWriter sWriter = File.AppendText(temp_File);
            sWriter.Write("\r\n");
            sWriter.Write("name :" + NameValue);
            sWriter.Write("\r\n");
            sWriter.Write("--");
            MessageBox.Show("Script created!");
            sWriter.Close();
            data.Add(NameValue);
        }


        private void Creator_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBoxLine = sender as TextBox;
            authorName = textBoxLine.Text;
        }


        private void edit_Script(object sender, RoutedEventArgs e)
        {
            dataGridView1.IsReadOnly = false;
        }


        private void save_Script(object sender, RoutedEventArgs e)
        {
            dataGridView1.IsReadOnly = true;
            string lineReader;
            Clipboard.Clear();
            Regex saveRegex = new Regex("name :");
            dataGridView1.SelectAllCells();
            dataGridView1.ClipboardCopyMode = DataGridClipboardCopyMode.ExcludeHeader;
            ApplicationCommands.Copy.Execute(null, dataGridView1);
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", Clipboard.GetText(TextDataFormat.Text));
            dataGridView1.UnselectAllCells();

            if (new FileInfo(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt").Length == 0)
            {
                MessageBox.Show("DataGrid is empty");
                return;
            }

            List<string> stringList = File.ReadLines(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt").ToList();

            foreach (string line in stringList)
            {
                var columnsTwo = line.Split('\t');
                if (columnsTwo[3] == "")
                {
                    MessageBox.Show("Single String parameter is empty");
                    return;
                }
            }


            string filename = System.IO.Path.GetTempFileName();
            StreamReader Edit_File = new StreamReader(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt");
            StreamReader Edit_ThirdFile = new StreamReader(Directory.GetCurrentDirectory() + "\\RegScripts.txt");
            string text = File.ReadAllText(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt");
            while ((file_Line = Edit_File.ReadLine()) != null)
            {
                string[] line_Tab = file_Line.Split('\t');
                text = text.Replace(file_Line, "grep -n -E -B " + line_Tab[1] + " -A " + line_Tab[2] + " " + "\"" + line_Tab[3] + "\"");
                File.WriteAllText(filename, text);

            }
            Edit_File.Close();
            File.Copy(filename, Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", true);


            StreamReader Edit_SecondFile = new StreamReader(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt");
            List<string> lines = File.ReadLines(Directory.GetCurrentDirectory() + "\\RegScripts.txt").ToList();


            while ((lineReader = Edit_ThirdFile.ReadLine()) != null)
            {
                if (saveRegex.IsMatch(lineReader))
                {
                    grepName = lineReader.Split(':');

                }
                else
                {
                    if (grepName[1] == value)
                    {
                        while ((file_Line = Edit_SecondFile.ReadLine()) != null)
                        {
                            if (lineReader.StartsWith("--"))
                            {
                                int index = lines.IndexOf("name :" + value);
                                // TODO: Validation (if index is -1, we couldn't find it)
                                lines.Insert(index + 1, file_Line);
                                File.WriteAllLines(filename, lines);
                            }
                            else
                            {
                                if (lineReader.StartsWith("grep"))
                                {
                                    int index = lines.IndexOf(lineReader);
                                    // TODO: Validation (if index is -1, we couldn't find it)
                                    lines.RemoveAt(index);
                                    lines.Insert(index, file_Line);
                                    File.WriteAllLines(filename, lines);
                                    break;
                                }
                            }
                        }
                        if (file_Line == null && lineReader.StartsWith("grep"))
                        {
                            int indexTwo = lines.IndexOf(lineReader);
                            lines.RemoveAt(indexTwo);
                            File.WriteAllLines(filename, lines);
                        }
                    }
                }
            }
            Edit_SecondFile.Close();
            Edit_ThirdFile.Close();
            File.Copy(filename, Directory.GetCurrentDirectory() + "\\RegScripts.txt", true);
            File.Delete(filename);
        }




        private void Script_Name_TextChanged(object sender, TextChangedEventArgs e)
        {

            var textBoxLine = sender as TextBox;
            NameValue = textBoxLine.Text;      

        }


        private List<Details> LoadCollectionData()
        {
            string tempSecondFile = Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt";
            List<Details> authors = new List<Details>();
            StreamReader file_one = new StreamReader(tempSecondFile);

            List<CheckBox> OptionsListTwo = new List<CheckBox>();
            char[] MyCharListTwo = { '\\', '*', '.', '"' };
            string file_stuff = file_one.ReadLine();
            for (int i = 0; i < count; i++)
            {
                if (file_stuff != null)
                {


                    var columns = file_stuff.Split(' ');
                    var columnsTwo = file_stuff.Split('"');
                    if (columnsTwo[1] == "")
                    {
                        columnsTwo[1] = "Empty String";
                    }
                    authors.Add(new Details()
                    {

                        singleString = columnsTwo[1].TrimStart(MyCharListTwo).TrimEnd(MyCharListTwo),

                        linesBelow = Int32.Parse(columns[4]),

                        linesAbove = Int32.Parse(columns[6]),

                        startTime = "None",

                        endTime = "None",

                        startString = "None",

                        endString = "None",

                    });
                }
                file_stuff = file_one.ReadLine();
            }
            file_one.Close();
            return authors;
        }

        public void showScriptCreation()
        {
            scriptNamebox.IsEnabled = true;
            scriptNamebox.IsReadOnly = false;
            ScriptNameLabel.IsEnabled = true;
            RegularExpressionLabel.IsEnabled = true;
            RegularExpressionbox.IsReadOnly = false;
            RegularExpressionbox.IsEnabled = true;
            RegularExpressionbox.IsReadOnly = false;
            RegularExpressionLabel.IsEnabled = true;
            scriptCreationButton.IsEnabled = true;
        }

        public void hideScriptCreation()
        {
            scriptNamebox.IsEnabled = false;
            scriptNamebox.IsReadOnly = true;
            ScriptNameLabel.IsEnabled = false;
            RegularExpressionLabel.IsEnabled = false;
            RegularExpressionbox.IsReadOnly = true;
            RegularExpressionbox.IsEnabled = false;
            RegularExpressionbox.IsReadOnly = true;
            RegularExpressionLabel.IsEnabled = false;
            scriptCreationButton.IsEnabled = false;

        }




        private void ComboOne_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var combobox = sender as ComboBox;
            ComboBox_Loaded(combobox, e);
            combobox.ItemsSource = null;
            //this.ComboOne.Items.Clear();


            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

            // ... Make the first item selected.
            comboBox.SelectedIndex = 0;

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Form dlg1 = new System.Windows.Forms.Form();
            dlg1.ShowDialog();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {


            currentCell = dataGridView1.SelectedIndex;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string filename = System.IO.Path.GetTempFileName();
            dataGridView1.SelectAllCells();
            dataGridView1.ClipboardCopyMode = DataGridClipboardCopyMode.ExcludeHeader;
            ApplicationCommands.Copy.Execute(null, dataGridView1);
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", Clipboard.GetText(TextDataFormat.Text));
            dataGridView1.UnselectAllCells();


            List<string> lines = File.ReadLines(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt").ToList();

            //int index = lines.IndexOf(lineReader);
            // TODO: Validation (if index is -1, we couldn't find it)
            lines.RemoveAt(currentCell);
            File.WriteAllLines(filename, lines);
            File.Copy(filename, Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", true);
            File.Delete(filename);
        }

        private void chkSelectHeader_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void dataGridView1_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {


        }

        private void MenuItem_exitBtn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }






        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult fileResult = dlg.ShowDialog();
            // Open folder for output and show in label 
            outputFileName = dlg.SelectedPath;
            string[] splitName = outputFileName.Split('\\');
            outputFileLabel.Content = splitName.Last();
        }

    }
    public class Details
    {
        public int linesBelow { get; set; }
        public int linesAbove { get; set; }
        public string singleString { get; set; }
        public string startString { get; set; }
        public string endString { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }

    }

}
