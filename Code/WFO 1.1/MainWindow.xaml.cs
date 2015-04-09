﻿

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Diagnostics;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string value;
        string pathName;
        string selectedValue;
        List<string> data = new List<string>();
        string NameValue;
        string authorName;
        string temp_copy;
        string splitName = "";
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
        CheckBox comboBox;

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
            dlg.Filter = "Text Files (.txt)|*.txt|LOG Files (.log)|*.log";
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                file_Name = dlg.FileName;

                File_Label.Content = System.IO.Path.GetFileName(file_Name);
                ListView1.IsEnabled = true;

            }
            if (file_Name != null)
            {
                string fileCheck = Path.GetExtension(file_Name);               
                if (fileCheck != ".txt" && fileCheck != ".log")
                {
                    MessageBox.Show("Invalid file format.\nPlease use file types with the extensions .log or .txt.", "Invalid file", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    File_Label.Content = string.Empty;
                    return;

                }

                File_Label.Content = System.IO.Path.GetFileName(file_Name);
            }
        }

        private void GrepButton_Click(object sender, RoutedEventArgs e)
        {
            if (pathName == null)
            {
                string fileRemoval = System.IO.Path.GetFileName(file_Name);
                int outputFilePathIndex = file_Name.IndexOf("\\" + fileRemoval);
                outputFileName = file_Name.Remove(outputFilePathIndex);


                string searchWord = "";

                //searchWord = searchWord.Remove();
                //Console.WriteLine(file_Name);
                //int lineCount = 0;            
                char[] MyCharList = { '\\', '*', '.', '"', ']', '[' };
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

                        StreamReader filePathing = new StreamReader(fileName);
                        //StreamWriter writefile = new StreamWriter(fileName);

                        while ((line = filePathing.ReadLine()) != null)
                        {

                            Regex regex = new Regex("name :");
                            if (regex.IsMatch(line))
                            {
                                //writefile.WriteAsync("aaah");
                                string[] script_CheckboxName = line.Split(':');
                                if (script_CheckboxName[1] == Option.Content.ToString())
                                {

                                    while ((nextLine = filePathing.ReadLine()) != "--")
                                    {


                                        string[] args = Regex.Split(nextLine, "[;]");
                                        args[0] = args[0].TrimEnd('[');
                                        args[1] = args[1].Trim(MyCharList);
                                        args[2] = args[2].Trim(MyCharList);

                                        //string arg = args[4];

                                        searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + args[2] + "\"";
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
                        filePathing.Close();
                    }

                }
                string searchWordEile = searchWord;
                perlCalled(searchWordEile);
            }

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

            GrepButton.IsEnabled = true;
            deleteScriptsButton.IsEnabled = true;

            
            //string value = CheckBox.

            //string value = ListView1.SelectedItem.ToString();
            //value = CheckBox.CheckedEvent.ToString();
            //Console.WriteLine(value);
            //int lastSelectedIndex = 0;
            //string lastSelectedValue = string.Empty;
            //if (CheckBox.IsCheckedProperty == true)
            //{

            //}
            //Console.WriteLine(comboBox.Content);
            //string chosenOne = "";
            //for (ContentProperty)
            //foreach (CheckBox Option in OptionsList)
            //{
            //    if (Option.IsChecked == true)
            //    {
            //        int thisIndex = OptionsList.IndexOf(Option);
            //        //int thisIndex = CheckBoxList1.Items.IndexOf(listitem);

            //        if (lastSelectedIndex > thisIndex)
            //        {
            //            //lastSelectedIndex = thisIndex;
            //            lastSelectedValue = Option.Content.ToString();
            //        }
            //    }
            //}

            //foreach (CheckBox Option in OptionsList)
            //{
            //    //string nextLine = "";
            //    //lineCount += 1;

            //    if (Option.IsChecked == true)
            //    {
            //        //Console.WriteLine(Option.Content);
            //        chosenOne = Option.Content.ToString();
            //    }
            //}
            //Console.WriteLine(chosenOne);

            //Console.WriteLine(lastSelectedIndex);
            //Console.WriteLine(lastSelectedValue);
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

            listReload();
        }

        private void listReload()
        {
            OptionsList.Clear();
            ListView1.Items.Clear();
            string line;
            var fileName = Directory.GetCurrentDirectory() + "\\RegScripts.txt";
            System.IO.StreamReader file_open = new System.IO.StreamReader(fileName);
            while ((line = file_open.ReadLine()) != null)
            {
                Regex regex = new Regex("name :");
                if (regex.IsMatch(line))
                {

                    string[] words = line.Split(':');
                    comboBox = new CheckBox();
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
            file_open.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboOne.SelectedItem != null)
            {

                File.WriteAllText(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", string.Empty);
                var search_Name = Directory.GetCurrentDirectory() + "\\RegScripts.txt";
                splitName = System.IO.Path.GetFileName(search_Name);
                StreamReader file_path = new StreamReader(splitName);
                temp_copy = Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt";
                StreamWriter sw = File.AppendText(temp_copy);
                Regex regexx = new Regex("name :");
                editScriptNameTextbox.Text = ComboOne.SelectedItem.ToString();
                while ((file_Line = file_path.ReadLine()) != null)
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
                                            file_Line = file_path.ReadLine();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (ComboOne.SelectedItem != "Script_Selection")
                {
                    //Regex regex = new Regex(ComboOne.SelectedItem.ToString() + "$");
                    //DataReload(regex);
                    //Console.WriteLine(regex);

                    selectedValue = ComboOne.SelectedItem.ToString() + "$";
                    //string[] values = selectedValue.Split(':');
                    //selectedValue = values.ToString();
                    //values = selectedValue.Split(' ');
                    //selectedValue = values[0] + "$";
                    Regex regex = new Regex(selectedValue);
                    DataReload(regex);
                }

                             

                file_path.Close();
                sw.Close();
                //dataGridView1.ItemsSource = LoadCollectionData();

            }
        }
                     
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {

            data.Clear();
            //ComboOne.UpdateDefaultStyle();
            var fileName = Directory.GetCurrentDirectory() + "\\RegScripts.txt";
            splitName = System.IO.Path.GetFileName(fileName);
            string line;
            StreamReader file = new StreamReader(splitName);
            data.Add("Script_Selection");
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
            listReload();
        }


        private void Creator_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBoxLine = sender as TextBox;
            authorName = textBoxLine.Text;
        }


        private void edit_Script(object sender, RoutedEventArgs e)
        {
            //dataGridView1.IsReadOnly = false;
        }


        //private void save_Script(object sender, RoutedEventArgs e)
        //{
        //    dataGridView1.IsReadOnly = true;
        //    string lineReader;
        //    Clipboard.Clear();
        //    Regex saveRegex = new Regex("name :");
        //    dataGridView1.SelectAllCells();
        //    dataGridView1.ClipboardCopyMode = DataGridClipboardCopyMode.ExcludeHeader;
        //    ApplicationCommands.Copy.Execute(null, dataGridView1);
        //    File.WriteAllText(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", Clipboard.GetText(TextDataFormat.Text));
        //    dataGridView1.UnselectAllCells();

        //    if (new FileInfo(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt").Length == 0)
        //    {
        //        MessageBox.Show("DataGrid is empty");
        //        return;
        //    }

        //    List<string> stringList = File.ReadLines(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt").ToList();

        //    foreach (string line in stringList)
        //    {
        //        var columnsTwo = line.Split('\t');
        //        if (columnsTwo[3] == "")
        //        {
        //            MessageBox.Show("Single String parameter is empty");
        //            return;
        //        }
        //    }


        //    string filename = System.IO.Path.GetTempFileName();
        //    StreamReader Edit_File = new StreamReader(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt");
        //    StreamReader Edit_ThirdFile = new StreamReader(Directory.GetCurrentDirectory() + "\\RegScripts.txt");
        //    string text = File.ReadAllText(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt");
        //    while ((file_Line = Edit_File.ReadLine()) != null)
        //    {
        //        string[] line_Tab = file_Line.Split('\t');
        //        text = text.Replace(file_Line, "grep -n -E -B " + line_Tab[1] + " -A " + line_Tab[2] + " " + "\"" + line_Tab[3] + "\"");
        //        File.WriteAllText(filename, text);

        //    }
        //    Edit_File.Close();
        //    File.Copy(filename, Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", true);


        //    StreamReader Edit_SecondFile = new StreamReader(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt");
        //    List<string> lines = File.ReadLines(Directory.GetCurrentDirectory() + "\\RegScripts.txt").ToList();


        //    while ((lineReader = Edit_ThirdFile.ReadLine()) != null)
        //    {
        //        if (saveRegex.IsMatch(lineReader))
        //        {
        //            grepName = lineReader.Split(':');

        //        }
        //        else
        //        {
        //            if (grepName[1] == value)
        //            {
        //                while ((file_Line = Edit_SecondFile.ReadLine()) != null)
        //                {
        //                    if (lineReader.StartsWith("--"))
        //                    {
        //                        int index = lines.IndexOf("name :" + value);
        //                        // TODO: Validation (if index is -1, we couldn't find it)
        //                        lines.Insert(index + 1, file_Line);
        //                        File.WriteAllLines(filename, lines);
        //                    }
        //                    else
        //                    {
        //                        if (lineReader.StartsWith("grep"))
        //                        {
        //                            int index = lines.IndexOf(lineReader);
        //                            // TODO: Validation (if index is -1, we couldn't find it)
        //                            lines.RemoveAt(index);
        //                            lines.Insert(index, file_Line);
        //                            File.WriteAllLines(filename, lines);
        //                            break;
        //                        }
        //                    }
        //                }
        //                if (file_Line == null && lineReader.StartsWith("grep"))
        //                {
        //                    int indexTwo = lines.IndexOf(lineReader);
        //                    lines.RemoveAt(indexTwo);
        //                    File.WriteAllLines(filename, lines);
        //                }
        //            }
        //        }
        //    }
        //    Edit_SecondFile.Close();
        //    Edit_ThirdFile.Close();
        //    File.Copy(filename, Directory.GetCurrentDirectory() + "\\RegScripts.txt", true);
        //    File.Delete(filename);
        //}




        private void Script_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBoxLine = sender as TextBox;
            NameValue = textBoxLine.Text;
            scriptCreationButton.IsEnabled = true;
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

                        //Single_String = columnsTwo[1].TrimStart(MyCharListTwo).TrimEnd(MyCharListTwo),

                        //Lines_After = Int32.Parse(columns[4]),

                        //Lines_Before = Int32.Parse(columns[6]),

                        //Start_Time = "None",

                        //End_Time = "None",

                        //Start_String = "None",

                        //End_String = "None",

                    });
                }
                file_stuff = file_one.ReadLine();
            }
            file_one.Close();
            return authors;
        }

        public void showScriptCreation()
        {
            //scriptNamebox.IsEnabled = true;
            //scriptNamebox.IsReadOnly = false;
            //scriptCreationButton.IsEnabled = true;
        }

        public void hideScriptCreation()
        {
            //scriptNamebox.IsEnabled = false;
            //scriptNamebox.IsReadOnly = true;
            //scriptCreationButton.IsEnabled = false;

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


            //currentCell = dataGridView1.SelectedIndex;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }


        //private void Button_Click_2(object sender, RoutedEventArgs e)
        //{
        //    string filename = System.IO.Path.GetTempFileName();
        //    dataGridView1.SelectAllCells();
        //    dataGridView1.ClipboardCopyMode = DataGridClipboardCopyMode.ExcludeHeader;
        //    ApplicationCommands.Copy.Execute(null, dataGridView1);
        //    File.WriteAllText(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", Clipboard.GetText(TextDataFormat.Text));
        //    dataGridView1.UnselectAllCells();


        //    List<string> lines = File.ReadLines(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt").ToList();

        //    //int index = lines.IndexOf(lineReader);
        //    // TODO: Validation (if index is -1, we couldn't find it)
        //    lines.RemoveAt(currentCell);
        //    File.WriteAllLines(filename, lines);
        //    File.Copy(filename, Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", true);
        //    File.Delete(filename);
        //}

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
            // dlg.RootFolder = Environment.SpecialFolder.MyComputer;            
            if (file_Name != null)
            {
                string fileNameRemoval = System.IO.Path.GetFileName(file_Name);
                int outputFilePath = file_Name.IndexOf("\\" + fileNameRemoval);
                pathName = file_Name.Remove(outputFilePath);

                dlg.SelectedPath = pathName;
            }
            dlg.ShowDialog();
            // Open folder for output and show in label 
            outputFileName = dlg.SelectedPath;
            string[] splitName = outputFileName.Split('\\');
            outputFileLabel.Content = splitName.Last();
        }

        private void DataGrid1_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ComboOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataGrid1.IsReadOnly = false;
            Regex regex = new Regex("");
            string selectedScript = null;
            if (ComboOne.SelectedItem != null)
            {
                if (ComboOne.SelectedItem.ToString() != "Script_Selection")
                {
                    selectedScript = ComboOne.SelectedItem.ToString();
                    regex = new Regex(selectedScript + "$");
                }
            }

            string scriptLine;
            var scriptFileName = Directory.GetCurrentDirectory() + @"\RegScripts.txt";
            StreamReader scriptReader = new StreamReader(scriptFileName);
            while ((scriptLine = scriptReader.ReadLine()) != null)
            {
                if (regex.IsMatch(""))
                {
                }
                else
                {
                    if ((regex).IsMatch(scriptLine))
                    {
                        DataGrid1.Items.Clear();
                        DataGrid1.Columns.Clear();
                        DataGridCheckBoxColumn grepCheckboxes = new DataGridCheckBoxColumn();
                        DataGrid1.Columns.Add(grepCheckboxes);
                        DataGrid1.Columns.Add(new DataGridTextColumn { Header = "String", Binding = new Binding("singleString") });
                        DataGrid1.Columns.Add(new DataGridTextColumn { Header = "Top Signal", Binding = new Binding("linesAbove") });
                        DataGrid1.Columns.Add(new DataGridTextColumn { Header = "Bottom Signal", Binding = new Binding("linesBelow") });
                        string nextLine;
                        while ((nextLine = scriptReader.ReadLine()) != "--")
                        {

                            string[] words = nextLine.Split('"');
                            string[] args = (words[0]).Split(' ');
                            string arg = words[1];
                            int aboveMarker = int.Parse(args[4]);
                            int belowMarker = int.Parse(args[6]);
                            DataGrid1.Items.Add(new Details { linesBelow = belowMarker, linesAbove = aboveMarker, singleString = arg });
                        }
                    }
                }
            }
            scriptReader.Close();
        }
        Details selectedDetails;
        string startString;
        string startLinesAbove;
        string startLinesBelow;

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

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid1.SelectedItem != null)
            {
                selectedDetails = (Details)DataGrid1.SelectedItem;
                startString = selectedDetails.singleString;
                startLinesAbove = selectedDetails.singleString;
                startLinesBelow = selectedDetails.singleString;
                Add_New_Line_Textbox_1.Text = selectedDetails.singleString;
                Add_New_Line_Textbox__2.Text = selectedDetails.linesAbove.ToString();
                Add_New_Line_Textbox__3.Text = selectedDetails.linesBelow.ToString();
            }
        }
        private void dataGridView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Copy_Script_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                //dataGridView1.ItemsSource = LoadCollectionData();

            }

        }

        private void Add_New_Line_Textbox_1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_Save_New_Line_Click(object sender, RoutedEventArgs e)
        {
            string nextLine;

            string editedString = Add_New_Line_Textbox_1.Text;
            string editedLinesAbove = Add_New_Line_Textbox__2.Text;
            string editedLinesBelow = Add_New_Line_Textbox__3.Text;

            string tempfile = System.IO.Path.GetTempFileName();


            Regex regex = new Regex(selectedValue);
            string scriptLine;
            var scriptFileName = Directory.GetCurrentDirectory() + @"\RegScripts.txt";
            StreamReader scriptReaderForEditing = new StreamReader(scriptFileName);
            StreamWriter tempWriterForEditing = new StreamWriter(tempfile);
            //StreamWriter scriptWriterForEditing = new StreamWriter(scriptFileName);
            Regex regex2 = new Regex(startString);
            string testnew = "Add New Line";
            //while reading the file
            while ((scriptLine = scriptReaderForEditing.ReadLine()) != null)
            {
                //match script to be changed
                if (regex.IsMatch(scriptLine))
                {
                    tempWriterForEditing.WriteLine(scriptLine);

                    while ((nextLine = scriptReaderForEditing.ReadLine()) != "--")
                    {
                        //match line to be changed
                        if (regex2.IsMatch(nextLine))
                        {
                            tempWriterForEditing.WriteLine("\"{2}\"[;]{0}[;]{1}", editedLinesAbove, editedLinesBelow, editedString);
                        }

                        else
                        {
                            tempWriterForEditing.WriteLine(nextLine);
                        }
                    }
                    if (regex2.IsMatch(testnew))
                    {
                        tempWriterForEditing.WriteLine("\"{2}\"[;]{0}[;]{1}", editedLinesAbove, editedLinesBelow, editedString);
                    }
                    tempWriterForEditing.WriteLine("--");

                }
                else
                {
                    tempWriterForEditing.WriteLine(scriptLine);
                }

            }
            tempWriterForEditing.Close();
            scriptReaderForEditing.Close();
            //var testfile = Directory.GetCurrentDirectory() + @"\test.txt";
            scriptReaderForEditing = new StreamReader(tempfile);
            tempWriterForEditing = new StreamWriter(scriptFileName);
            string aline;
            while ((aline = scriptReaderForEditing.ReadLine()) != null)
            {
                //Console.WriteLine(aline);
                tempWriterForEditing.WriteLine(aline);

            }
            tempWriterForEditing.Close();
            scriptReaderForEditing.Close();
            DataReload(regex);


        }

        private void ListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //selectedValue = null;
            if (ListView1.SelectedItem != null)
            {
                selectedValue = ListView1.SelectedItem.ToString();
                string[] values = selectedValue.Split(':');
                selectedValue = values[1].ToString();
                values = selectedValue.Split(' ');
                selectedValue = values[0] + "$";
                Regex regex = new Regex(selectedValue);
                DataReload(regex);
            }

            //if (ListView1.SelectedItem != null)
            //{
            //    selectedValue = ListView1.SelectedItem.ToString();
            //    string[] values = selectedValue.Split(':');
            //    selectedValue = values[1].ToString();
            //    values = selectedValue.Split(' ');
            //    selectedValue = values[0] + "$";
            //    Regex regex = new Regex(selectedValue);
            //    DataReload(regex);
            //}

        }


        private void DataReload(Regex regex)
        {
            char[] mycharlist = { ']', '[' };
            string scriptLine;
            var scriptFileName = Directory.GetCurrentDirectory() + @"\RegScripts.txt";
            StreamReader scriptReader = new StreamReader(scriptFileName);
            while ((scriptLine = scriptReader.ReadLine()) != null)
            {
                if (regex.IsMatch(""))
                {
                }
                else
                {
                    if ((regex).IsMatch(scriptLine))
                    {
                        DataGrid1.Items.Clear();
                        DataGrid1.Columns.Clear();
                        DataGridCheckBoxColumn grepCheckboxes = new DataGridCheckBoxColumn();
                        DataGrid1.Columns.Add(grepCheckboxes);
                        DataGrid1.Columns.Add(new DataGridTextColumn { Header = "String", Binding = new Binding("singleString") });
                        DataGrid1.Columns.Add(new DataGridTextColumn { Header = "Top Signal", Binding = new Binding("linesAbove") });
                        DataGrid1.Columns.Add(new DataGridTextColumn { Header = "Bottom Signal", Binding = new Binding("linesBelow") });
                        string nextLine = null;

                        while ((nextLine = scriptReader.ReadLine()) != "--")
                        {

                            string[] argsvalue = Regex.Split(nextLine, "[;]");
                            argsvalue[1] = argsvalue[1].Trim(mycharlist);
                            argsvalue[2] = argsvalue[2].Trim(mycharlist);
                            int aboveMarker = int.Parse(argsvalue[1]);
                            int belowMarker = int.Parse(argsvalue[2]);
                            DataGrid1.Items.Add(new Details { linesBelow = belowMarker, linesAbove = aboveMarker, singleString = argsvalue[0].TrimEnd(mycharlist) });
                        }

                    }
                }
            }
            //DataGrid1.Items.Add(new DataGridRow());
            DataGrid1.Items.Add(new Details { linesBelow = 0, linesAbove = 0, singleString = "Add New Line" });
            scriptReader.Close();
        }

        //private void DataGrid1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    //Console.WriteLine("wtf is going on " + DataGrid1.SelectedItem);
        //    //DataGrid1.
        //    selectedDetails = (Details)DataGrid1.SelectedItem;
        //    startString = selectedDetails.singleString;
        //    //Console.WriteLine(selectedDetails.singleString);
        //    startLinesAbove = selectedDetails.singleString;
        //    startLinesBelow = selectedDetails.singleString;
        //    Add_New_Line_Textbox_1.Text = selectedDetails.singleString;
        //    Add_New_Line_Textbox__2.Text = selectedDetails.linesAbove.ToString();
        //    Add_New_Line_Textbox__3.Text = selectedDetails.linesBelow.ToString();
        //}

        private void btn_Save_New_Line_Click_1(object sender, RoutedEventArgs e)
        {
            string nextLine;

            string editedString = Add_New_Line_Textbox_1.Text;
            string editedLinesAbove = Add_New_Line_Textbox__2.Text;
            string editedLinesBelow = Add_New_Line_Textbox__3.Text;

            string tempfile = System.IO.Path.GetTempFileName();


            Regex regex = new Regex(selectedValue);
            string scriptLine;
            var scriptFileName = Directory.GetCurrentDirectory() + @"\RegScripts.txt";
            StreamReader scriptReaderForEditing = new StreamReader(scriptFileName);
            StreamWriter tempWriterForEditing = new StreamWriter(tempfile);
            //StreamWriter scriptWriterForEditing = new StreamWriter(scriptFileName);
            
            string testnew = "Add New Line";
            //while reading the file
            while ((scriptLine = scriptReaderForEditing.ReadLine()) != null)
            {
                //match script to be changed
                if (regex.IsMatch(scriptLine))
                {
                    if (regex.IsMatch(editScriptNameTextbox.Text))
                    {
                        tempWriterForEditing.WriteLine(scriptLine);
                    }
                    else
                    {
                        tempWriterForEditing.WriteLine("name :" + editScriptNameTextbox.Text);
                    }

                    if (Add_New_Line_Textbox_1.Text != "")
                    {
                        Regex regex2 = new Regex(startString);

                        while ((nextLine = scriptReaderForEditing.ReadLine()) != "--")
                        {
                            //match line to be changed
                            if (regex2.IsMatch(nextLine))
                            {
                                tempWriterForEditing.WriteLine("\"{2}\"[;]{0}[;]{1}", editedLinesAbove, editedLinesBelow, editedString);
                            }

                            else
                            {
                                tempWriterForEditing.WriteLine(nextLine);
                            }
                        }
                        if (regex2.IsMatch(testnew))
                        {
                            //tempWriterForEditing.WriteLine("grep -n -E -B {0} -A {1} \"{2}\"", editedLinesAbove, editedLinesBelow, editedString);
                            tempWriterForEditing.WriteLine("\"{2}\"[;]{0}[;]{1}", editedLinesAbove, editedLinesBelow, editedString);
                        }
                        tempWriterForEditing.WriteLine("--");
                    }

                }
                else
                {
                    tempWriterForEditing.WriteLine(scriptLine);
                }

            }
            tempWriterForEditing.Close();
            scriptReaderForEditing.Close();
            //var testfile = Directory.GetCurrentDirectory() + @"\test.txt";
            scriptReaderForEditing = new StreamReader(tempfile);
            tempWriterForEditing = new StreamWriter(scriptFileName);
            string aline;
            while ((aline = scriptReaderForEditing.ReadLine()) != null)
            {
                //Console.WriteLine(aline);
                tempWriterForEditing.WriteLine(aline);

            }
            tempWriterForEditing.Close();
            scriptReaderForEditing.Close();
            DataReload(regex);

            //DataGrid1.UpdateLayout();
            //DataGrid1.UnselectAllCells()
            //DataGrid1.UnselectAll()
        }

        private void deleteLineButton_Click(object sender, RoutedEventArgs e)
        {
            string nextLine;
            string tempfile = System.IO.Path.GetTempFileName();
            Regex regex = new Regex(selectedValue);
            string scriptLine;
            var scriptFileName = Directory.GetCurrentDirectory() + @"\RegScripts.txt";
            StreamReader scriptReaderForEditing = new StreamReader(scriptFileName);
            StreamWriter tempWriterForEditing = new StreamWriter(tempfile);
            //StreamWriter scriptWriterForEditing = new StreamWriter(scriptFileName);
            Regex regex2 = new Regex(startString);
            while ((scriptLine = scriptReaderForEditing.ReadLine()) != null)
            {
                if (regex.IsMatch(scriptLine))
                {
                    tempWriterForEditing.WriteLine(scriptLine);

                    while ((nextLine = scriptReaderForEditing.ReadLine()) != "--")
                    {
                        if (regex2.IsMatch(nextLine))
                        {
                        }

                        else
                        {
                            tempWriterForEditing.WriteLine(nextLine);
                        }
                    }
                    tempWriterForEditing.WriteLine("--");
                }
                else
                {
                    tempWriterForEditing.WriteLine(scriptLine);
                }
            }
            tempWriterForEditing.Close();
            scriptReaderForEditing.Close();
            //var testfile = Directory.GetCurrentDirectory() + @"\test.txt";
            scriptReaderForEditing = new StreamReader(tempfile);
            tempWriterForEditing = new StreamWriter(scriptFileName);
            string aline;
            while ((aline = scriptReaderForEditing.ReadLine()) != null)
            {
                //Console.WriteLine(aline);
                tempWriterForEditing.WriteLine(aline);

            }
            tempWriterForEditing.Close();
            scriptReaderForEditing.Close();
            DataReload(regex);
        }

        private void btn_Save_New_Line_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void save_Script(object sender, RoutedEventArgs e)
        {

            ////this.Title = "adfsd";
            //Console.WriteLine("item selected");
            //////var comboBox = sender as ComboBox;
            ////// ... Set SelectedItem as Window Title.

            //value = ListView1.SelectedItem.ToString();
            //Console.WriteLine(value);
            ////this.Title = "Selected: " + value;
            //foreach (CheckBox Option in OptionsList)
            //{
            //    if (Option.IsChecked == true)
            //    {
            //        Console.WriteLine(Option.Content);
            //    }
            //}
            //    File.WriteAllText(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", string.Empty);
            //    var search_Name = Directory.GetCurrentDirectory() + "\\RegScripts.txt";
            //    splitName = System.IO.Path.GetFileName(search_Name);
            //    StreamReader fileRead = new StreamReader(splitName);
            //    temp_copy = Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt";
            //    StreamWriter sw = File.AppendText(temp_copy);
            //    hideScriptCreation();
            //    Regex regexx = new Regex("name :");
            //    while ((file_Line = fileRead.ReadLine()) != null)
            //    {
            //        if (regexx.IsMatch(file_Line))
            //        {
            //            word_GrepLine = file_Line.Split(':');
            //        }
            //        else
            //        {
            //            if (word_GrepLine[1] == value)
            //            {
            //                if (file_Line.StartsWith("grep"))
            //                {
            //                    for (int i = 0; i < 999; i++)
            //                    {
            //                        if (file_Line.StartsWith("name :") || file_Line.StartsWith("-"))
            //                        {
            //                            break;
            //                        }
            //                        else
            //                        {
            //                            if (file_Line.StartsWith("grep"))
            //                            {
            //                                CheckBox fnsdfn = new CheckBox();

            //                                sw.Write(file_Line);
            //                                sw.Write("\r\n");
            //                                file_Line = fileRead.ReadLine();
            //                            }
            //                            else
            //                            {
            //                                break;
            //                            }

            //                            count++;
            //                        }
            //                    }
            //                }

            //                if (regexx.IsMatch(file_Line))
            //                {
            //                    break;
            //                }
            //            }
            //        }


            //        fileRead.Close();
            //        sw.Close();
            //        DataGrid1.ItemsSource = LoadCollectionData();
            //    }

        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void checkMethod()
        {

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void deleteScriptsButton_Click(object sender, RoutedEventArgs e)
        {
            string tempFileName = System.IO.Path.GetTempFileName();
            List<string> findingScripts = File.ReadLines(Directory.GetCurrentDirectory() + "\\RegScripts.txt").ToList();
            int counter = findingScripts.Count;
            int undercount = 0;
            var deleteResult = MessageBox.Show("Are you sure you wish to delete theses scripts?", "Delete Script",
                           System.Windows.Forms.MessageBoxButtons.YesNo,
                           System.Windows.Forms.MessageBoxIcon.Question);

            if (deleteResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
            {
                if (deleteResult == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (CheckBox Option in OptionsList)
                    {
                        //lineCount += 1;
                        if (Option.IsChecked == true)
                        {
                            string deleteScripts = Option.Content.ToString();
                            int deleteIndex = findingScripts.IndexOf("name :" + deleteScripts);

                            for (int i = deleteIndex; i <= counter; i++)
                            {

                                if (findingScripts[i - 1] == "--")
                                {
                                    findingScripts.RemoveRange(deleteIndex, undercount);
                                    File.WriteAllLines(tempFileName, findingScripts);
                                    File.Copy(tempFileName, Directory.GetCurrentDirectory() + "\\RegScripts.txt", true);
                                    OptionsList.Remove(Option);
                                    listReload();
                                    return;


                                }
                                undercount++;
                            }
                        }
                    }
                }
            }

        }

        private void Add_New_Line_Textbox_1_Loaded(object sender, RoutedEventArgs e)
        {
            Add_New_Line_Textbox_1.Text = "";
        }

        private void editScriptNameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void lvInvDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }

}
