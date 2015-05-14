
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
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

using System.Threading;

namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string value;
        double time;
        double timeTwo;
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
        string outputFileName;
        int count = 0;
        string grepStartValue;
        string grepEndValue;
        string aboveValue;
        string belowValue;
        string file_Name;
        CheckBox comboBox = new CheckBox();
        CheckBox gridCheckAllBox = new CheckBox();

        string startTime;
        string endTime;
        string Hex;
        string Exclude;
        long filesize;

        string startTimeValue = "";
        string endTimeValue = "";
        List<int> listint = new List<int>();

        List<CheckBox> OptionsList = new List<CheckBox>();
        CheckBox box = new CheckBox();




        public MainWindow()
        {
            InitializeComponent();
            new SplashWindow().ShowDialog();
            if (!File.Exists("ListViewScriptsTwo.txt"))
            {
                MessageBox.Show("You are missing a necessary file, please contact your provider.", "Missing file", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

            }
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
            dlg.Filter = "LOG Files (.log)|*.log|Text Files (.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                file_Name = dlg.FileName;

                File_Label.Text = System.IO.Path.GetFileName(file_Name);

                FileInfo f = new FileInfo(file_Name);
                filesize = f.Length;

            }
            if (file_Name != null)
            {
                string fileCheck = System.IO.Path.GetExtension(file_Name);
                if (fileCheck != ".txt" && fileCheck != ".log")
                {
                    MessageBox.Show("Invalid file format.\nPlease use file types with the extensions .log or .txt.", "Invalid file", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    File_Label.Text = string.Empty;
                    return;

                }
                else
                {
                    outputFileLabel.Text = System.IO.Path.GetDirectoryName(file_Name);
                    File_Label.Text = System.IO.Path.GetFileName(file_Name);
                }
            }
        }

        private void GrepButton_Click(object sender, RoutedEventArgs e)
        {
            string fileRemoval;
            int outputFilePathIndex;
            if (outputFileName == null && file_Name != null)
            {
                fileRemoval = System.IO.Path.GetFileName(file_Name);
                outputFilePathIndex = file_Name.IndexOf("\\" + fileRemoval);
                outputFileName = file_Name.Remove(outputFilePathIndex);
            }
            else if (outputFileName != null && file_Name != null)
            {
                fileRemoval = System.IO.Path.GetFileName(file_Name);
                outputFilePathIndex = file_Name.IndexOf("\\" + fileRemoval);
                outputFileName = file_Name.Remove(outputFilePathIndex);
            }
            List<string> scriptsToDelete = new List<string>();
            int selectcount = 0;
            foreach (ListViewItems stuff in ListDataGrid.ItemsSource)
            {
                string scriptName = stuff.gridNameColumn;
                foreach (int selected in listIntSelection)
                {
                    if (selectcount == selected)
                    {
                        scriptsToDelete.Add(scriptName);
                    }
                }
                selectcount++;
            }
            if (file_Name == null)
            {
                MessageBox.Show("No Log File Selected", "Data Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            else if (scriptsToDelete.Count == 0 && startTime == null && endTime == null)
            {
                MessageBox.Show("No Scripts Selected or Dates Selected", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            else if (scriptsToDelete.Count == 0 && startTime != null && endTime == null)
            {
                MessageBox.Show("Start Date has not been set!", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            else if (scriptsToDelete.Count == 0 && startTime == null && endTime != null)
            {
                MessageBox.Show("End Date has not been set!", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            else if (scriptsToDelete.Count != 0 && startTime != null && endTime != null)
            {
                MessageBox.Show("Dates and Scripts can not be searched at the same time", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            else if (scriptsToDelete.Count != 0 && startTime == null && endTime == null)
            {

                count = 0;
                ListDataGrid.ItemsSource.GetEnumerator();
                Regex intregex = new Regex(@"\d+");
                Regex stringRegex = new Regex(@"[\D\d]+");
                string searchWord = "";
                char[] MyCharList = { '\\', '*', '.', '"', ']', '[' };
                int selectcount2 = 0;
                foreach (ListViewItems Option in ListDataGrid.ItemsSource)
                {
                    string nextLine = "";
                    string line;
                    var fileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
                    StreamReader filePathing = new StreamReader(fileName);
                    while ((line = filePathing.ReadLine()) != null)
                    {
                        Regex regex = new Regex("name :");
                        if (regex.IsMatch(line))
                        {
                            foreach (int selected in listIntSelection)
                            {
                                if (selectcount2 == selected)
                                {
                                    while ((nextLine = filePathing.ReadLine()) != "--")
                                    {
                                        string[] args = Regex.Split(nextLine, "[;]");
                                        if (args.Length == 3)
                                        {
                                            args[0] = args[0].TrimEnd('[');
                                            args[1] = args[1].Trim(MyCharList);
                                            args[2] = args[2].Trim(MyCharList);
                                            int num1;
                                            bool res = int.TryParse(args[2], out num1);
                                            if (args[2] == "DATE")
                                            {
                                                searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + args[2] + "!;!" + "type1" + "\"";
                                            }
                                            else if (stringRegex.IsMatch(args[0]) && args[1] == "" && stringRegex.IsMatch(args[2]))
                                            {
                                                searchWord = searchWord + " \"" + args[0] + ";!;" + " " + ";!;" + args[2] + "!;!" + "type2" + "\"";
                                            }
                                            else if (stringRegex.IsMatch(args[0]) && stringRegex.IsMatch(args[1]) && args[2] == "")
                                            {
                                                searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + " " + "!;!" + "type2" + "\"";
                                            }
                                            else if (res == false)
                                            {
                                                searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + args[2] + "!;!" + "type4" + "\"";
                                            }
                                            else if (res == true)
                                            {
                                                searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + args[2] + "!;!" + "type3" + "\"";
                                            }

                                        }
                                    }
                                }
                            }
                            selectcount2++;
                        }
                    }
                    filePathing.Close();
                }
                string searchWordEile = searchWord;
                perlCalled(searchWordEile);
            }
            else if (scriptsToDelete.Count == 0 && startTime != null && endTime != null && outputFileName != null && file_Name != null)
            {
                string perlDateString = startTime + "*!*" + endTime;
                perlCalled(perlDateString);
                startTime = null;
                endTime = null;
            }
        }

        Process perlprocess;
        List<Process> processList = new List<Process>();
        int processCount = 0;
        private void perlCalled(string searchWord2)
        {
         
            int fileCheck;
            if (Hex == null && Exclude != null)
            {
                searchWord2 = "\"" + file_Name + "\"" + " " + "\"" + outputFileName + "\"" + " " + searchWord2 + "@;;@" + Hex + "@;;@" + Exclude;
                string processname = "perlprocess" + processCount.ToString();

            }
            else if (Hex != null && Exclude == null)
            {
                searchWord2 = "\"" + file_Name + "\"" + " " + "\"" + outputFileName + "\"" + " " + searchWord2 + "@;;@" + Hex;
                string processname = "perlprocess" + processCount.ToString();

            }
            else if (Hex != null && Exclude != null)
            {
                searchWord2 = "\"" + file_Name + "\"" + " " + "\"" + outputFileName + "\"" + " " + searchWord2 + "@;;@" + Hex + "@;;@" + Exclude;
                string processname = "perlprocess" + processCount.ToString();

            }
            else if (Hex == null && Exclude == null)
            {
                searchWord2 = "\"" + file_Name + "\"" + " " + "\"" + outputFileName + "\"" + " " + searchWord2;
                string processname = "perlprocess" + processCount.ToString();
            }


            perlprocess = new Process();
            if (processCount < 2)
            {
                processList.Add(perlprocess);


                ProcessStartInfo perlStartInfo = new ProcessStartInfo("perl.exe");
                perlStartInfo.Arguments = string.Format("StringSearchWithNLines.pl" + " " + searchWord2);
                perlStartInfo.UseShellExecute = false;
                perlStartInfo.RedirectStandardOutput = true;
                perlStartInfo.RedirectStandardError = true;
                perlStartInfo.CreateNoWindow = true;
                //Process perl = new Process();
                perlprocess.StartInfo = perlStartInfo;

                if (filesize > 400000000)
                {
                    fileCheck = ((int)filesize / 100000000);
                    time = ((fileCheck * 7) * count) / 60;
                    timeTwo = ((fileCheck * 10) * count) / 60;
                    if (time == 0 && timeTwo == 0)
                    {
                        time = (fileCheck * 7);
                        MessageBox.Show("This is a large file, Estimated Time of completion will be less than 2 minutes", "FilSize Alert", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                    }
                    else
                    {
                        time = Math.Ceiling(time);
                        timeTwo = Math.Ceiling(timeTwo);
                        MessageBox.Show("This is a large file, Estimated Time of completion will be between " + time + " to " + timeTwo + " minutes", "File Size Alert", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        time = time * 60;
                        progressBar.Visibility = Visibility.Visible;
                        Progress_label.Visibility = Visibility.Visible;
                        progressTimer();
                    }
                }
                else if (filesize < 400000000)
                {

                    fileCheck = ((int)filesize / 100000000);
                    time = (((fileCheck * 7) * count) / 60);
                    timeTwo = (((fileCheck * 10) * count) / 60);
                    MessageBox.Show("Estimated Time of completion is less than one minute", "File Size Alert", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    if (time == 0  && fileCheck > 2)
                    {
                        time = 1;
                    }
                    time = time * 60;
                    progressBar.Visibility = Visibility.Visible;
                    Progress_label.Visibility = Visibility.Visible;
                    progressTimer();
                    

                }
                perlprocess.Start();
                //Worker workerObject = new Worker();
                processCount++;
                Thread workerThread = new Thread(thread);
                workerThread.Start();     
            }
            else
            {
              
                MessageBox.Show("You have too many scripts running, please try again when one has exited", "Script Limit Reached", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        // bool scriptsCancelledWindowCheck = false;
        bool perlprocesswaskilled = false;
        private void thread()
        {

            this.Dispatcher.Invoke((Action)(() =>
            {
                BtnCancel.IsEnabled = true;
                BtnCancel.Background = Brushes.Red;
            })); 

            perlprocess.WaitForExit();

            //processList.Remove(perlprocess);
            processCount--;
            Hex = null;
            Exclude = null;
            if (perlprocesswaskilled == false && processCount == 0)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    BtnCancel.IsEnabled = false;

                    progressBar.Visibility = Visibility.Hidden;
                    Progress_label.Visibility = Visibility.Hidden;
                }));                
                if (MessageBox.Show("Log file created.\n\nDo you wish to open the output folder? ", "Open New File Location", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                   
                    Process.Start(outputFileName);

                }

            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (file_Name != null)
            {
                GrepButton.IsEnabled = true;
            }
            deleteScriptsButton.IsEnabled = true;
           

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("checkbox is unchecked");
        }

        private void Checkbox_Loaded(object sender, RoutedEventArgs e)
        {
        }
        //CheckBox gridcheckbox;
        private void DataReload(Regex regex)
        {
            List<Details> datalist = new List<Details>();
            DataGrid1.IsReadOnly = false;
            char[] mycharlist = { ']', '[' };
            string scriptLine;
            var scriptFileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
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
                        string aboveMarker;
                        string belowMarker;
                        string nextLine = null;
                        //gridcheckbox = null;
                        int count = 0;

                        Details dataForGrid = new Details();
                        //dataForGrid.gridCheckbox = null;
                        dataForGrid.singleString = "Add New Line";
                        dataForGrid.linesAbove = "";
                        dataForGrid.linesBelow = "";
                        datalist.Add(dataForGrid);

                        while ((nextLine = scriptReader.ReadLine()) != "--")
                        {
                            count++;
                            //gridcheckbox = new CheckBox();
                            //gridcheckbox.Checked += gridCheckBox_Checked;
                            string[] argsvalue = Regex.Split(nextLine, @"\[;\]");

                            try
                            {
                                aboveMarker = argsvalue[1].Trim(mycharlist);
                            }
                            catch
                            {
                                aboveMarker = "";
                            }

                            try
                            {
                                belowMarker = argsvalue[2].Trim(mycharlist);
                            }
                            catch
                            {
                                belowMarker = "";
                            }
                            Details getDataForGrid = new Details();

                            getDataForGrid.singleString = argsvalue[0].TrimEnd(mycharlist);
                            getDataForGrid.linesAbove = aboveMarker;
                            getDataForGrid.linesBelow = belowMarker;
                            datalist.Add(getDataForGrid);
                            //string aboveMarker = (argsvalue[1]);
                            //string belowMarker = (argsvalue[2]);
                            //datalist.Add(new Details { singleString = argsvalue[0].TrimEnd(mycharlist), linesAbove = aboveMarker, linesBelow = belowMarker });

                        }
                        DataGrid1.ItemsSource = datalist;
                        //CheckBox checkAll = new CheckBox();
                        //checkAll.Checked += checkAll_Checked;
                        //DataGrid1.Columns[0].Header = checkAll;
                        DataGrid1.Columns[1].Header = "String";
                        DataGrid1.Columns[2].Header = "Top Marker";
                        DataGrid1.Columns[3].Header = "Bottom Marker";
                        DataGrid1.Columns[1].IsReadOnly = true;
                        DataGrid1.Columns[2].IsReadOnly = true;
                        DataGrid1.Columns[3].IsReadOnly = true;

                        DataGrid1.Columns[0].Width = 24;

                        //DataGrid1.Columns[2].Width = 500;
                        //DataGrid1.Columns[3].Width = 500;               
                    }
                }
            }
            scriptReader.Close();

            DataGrid1.SelectedIndex = 0;
        }

        bool listcheck = false;

        private void listReload2()
        {
            List<ListViewItems> anotherListViewList = new List<ListViewItems>();
            string line;
            var fileName = Directory.GetCurrentDirectory() + "\\ListViewScriptsTwo.txt";
            System.IO.StreamReader file_open = new System.IO.StreamReader(fileName);
            while ((line = file_open.ReadLine()) != null)
            {
                Regex regex = new Regex("name :");
                if (regex.IsMatch(line))
                {
                    string[] words = line.Split(':');
                    ListViewItems list = new ListViewItems();
                    list.gridNameColumn = words[1].ToString();
                    list.gridCheckboxColumn = listcheck;
                    anotherListViewList.Add(list);
                    //anotherListViewList.Add(words[1]).ToString());

                    //comboBox = new CheckBox();

                    //listcheckbox = new CheckBox();

                    //ListDataGrid.Columns.

                    //anotherListViewList.Add(new ListViewItems { gridNameColumn = (words[1]).ToString() });
                }
            }
            ListDataGrid.ItemsSource = anotherListViewList;
            //CheckBox listcheckboxtop = new CheckBox();

            //listcheckboxtop.Checked += listCheckBoxtop_Checked;

            //ListDataGrid.Columns[0].Header = listcheckboxtop;
            ListDataGrid.Columns[1].Header = "Select Script to Parse With";

            //ListDataGrid.Columns[0].CellStyle = ListDataGrid.Columns[0].;

            //ListDataGrid.SelectionMode
            ListDataGrid.Columns[0].Width = 24;
            ListDataGrid.Columns[1].IsReadOnly = true;

            file_open.Close();
        }

        public class ListViewItems
        {

            //public bool comboBox { get; set; }
            public string gridNameColumn { get; set; }
            public bool gridCheckboxColumn { get; set; }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboOne.SelectedItem != null)
            {
                DataGrid1.Columns[0].Visibility = Visibility.Visible;
                DataGrid1.Columns[1].Visibility = Visibility.Visible;
                DataGrid1.Columns[2].Visibility = Visibility.Visible;
                DataGrid1.Columns[3].Visibility = Visibility.Visible;

                datacheckboxheader.IsChecked = false;
                // File.WriteAllText(Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt", string.Empty);
                var search_Name = Directory.GetCurrentDirectory() + "\\ListViewScriptsTwo.txt";
                splitName = System.IO.Path.GetFileName(search_Name);
                StreamReader file_path = new StreamReader(splitName);
                //temp_copy = Directory.GetCurrentDirectory() + "\\RegScript2DontDelete.txt";
                // StreamWriter sw = File.AppendText(temp_copy);
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
                                            //CheckBox fnsdfn = new CheckBox();

                                            //sw.Write(file_Line);
                                            //sw.Write("\r\n");
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

                //if (ComboOne.SelectedItem != "Script_Selection")
                //{
                //Regex regex = new Regex(ComboOne.SelectedItem.ToString() + "$");
                //DataReload(regex);
                //Console.WriteLine(regex);

                selectedValue = ComboOne.SelectedItem.ToString();
                //string[] values = selectedValue.Split(':');
                //selectedValue = values.ToString();
                //values = selectedValue.Split(' ');
                //selectedValue = values[0] + "$";
                Regex regex = new Regex("name :" + selectedValue + "$");
                DataReload(regex);
                //}


                btn_Save_New_Line.IsEnabled = false;
                file_path.Close();
                //sw.Close();
                //dataGridView1.ItemsSource = LoadCollectionData();
                editScriptNameTextbox.IsEnabled = true;

            }
            else
            {
                DataGrid1.ItemsSource = null;
                DataGrid1.Columns[0].Visibility = Visibility.Hidden;
                DataGrid1.Columns[1].Visibility = Visibility.Hidden;
                DataGrid1.Columns[2].Visibility = Visibility.Hidden;
                DataGrid1.Columns[3].Visibility = Visibility.Hidden;
            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox_Reload();
        }

        private void clearEditBoxes()
        {
            editScriptNameTextbox.Clear();
            Add_New_Line_Textbox_1.Clear();
            Add_New_Line_Textbox__2.Clear();
            Add_New_Line_Textbox__3.Clear();
        }

        private void ComboBox_Reload()
        {
            clearEditBoxes();
            data.Clear();

            //ComboOne.UpdateDefaultStyle();
            var fileName = Directory.GetCurrentDirectory() + "\\ListViewScriptsTwo.txt";
            splitName = System.IO.Path.GetFileName(fileName);
            string line;
            StreamReader file = new StreamReader(splitName);
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

        NewScriptWindow scriptWindow = new NewScriptWindow();
        private void Create_File(object sender, RoutedEventArgs e)
        {
            try
            {
                scriptWindow.Show();
                scriptWindow.Activate();
            }
            catch (Exception)
            {
                scriptWindow = new NewScriptWindow();
                scriptWindow.Show();
                scriptWindow.Activate();
            }


            scriptWindow.Closed += scriptWindow_Closed;
        }

        void scriptWindow_Closed(object sender, EventArgs e)
        {
            listReload2();
            string newscript = "";
            ComboBox_Reload();
            foreach (ListViewItems last in ListDataGrid.ItemsSource)
            {
                newscript = last.gridNameColumn;
            }
            ComboOne.SelectedValue = newscript;
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
        //    StreamReader Edit_ThirdFile = new StreamReader(Directory.GetCurrentDirectory() + "\\ListViewScriptsTwo.txt");
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
        //    List<string> lines = File.ReadLines(Directory.GetCurrentDirectory() + "\\ListViewScriptsTwo.txt").ToList();


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
        //    File.Copy(filename, Directory.GetCurrentDirectory() + "\\ListViewScriptsTwo.txt", true);
        //    File.Delete(filename);
        //}




        private void Script_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBoxLine = sender as TextBox;
            NameValue = textBoxLine.Text;
            scriptCreationButton.IsEnabled = true;
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
            //comboBox.SelectedIndex = 1;

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
        // Exit Button that close the application
        private void MenuItem_exitBtn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //  button that a opens a Blank new Window
        private void Button_NewWindow(object sender, RoutedEventArgs e)
        {
            Window win2 = new Window();
            win2.Show();
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
            outputFileLabel.Text = splitName.Last();

        }

        private void DataGrid1_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ComboOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }
        Details selectedDetails;
        string startString;
        string startLinesAbove;
        string startLinesBelow;
        //bool selectionChecked;

        public class Details
        {
            public bool gridCheckbox { get; set; }
            public string singleString { get; set; }
            public string linesAbove { get; set; }
            public string linesBelow { get; set; }

            //public string startString { get; set; }
            //public string endString { get; set; }
            //public string startTime { get; set; }
            //public string endTime { get; set; }
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (DataGrid1.SelectedItem != null)
            {
                topDateTypeCheckbox.IsChecked = false;

                selectedDetails = (Details)DataGrid1.SelectedItem;
                startString = selectedDetails.singleString;
                startLinesAbove = selectedDetails.singleString;
                startLinesBelow = selectedDetails.singleString;
                //selectionChecked = selectedDetails.gridCheckbox;
                Add_New_Line_Textbox_1.Text = selectedDetails.singleString;
                Add_New_Line_Textbox__2.Text = selectedDetails.linesAbove.ToString();
                Add_New_Line_Textbox__3.Text = selectedDetails.linesBelow.ToString();
                Add_New_Line_Textbox_1.IsEnabled = true;
                Add_New_Line_Textbox__2.IsEnabled = true;
                Add_New_Line_Textbox__3.IsEnabled = true;
                topDateTypeCheckbox.IsEnabled = true;

                if (Add_New_Line_Textbox__2.Text == "DATE")
                {
                    topDateTypeCheckbox.IsChecked = true;
                }
                else
                {
                    topDateTypeCheckbox.IsChecked = false;

                }
                //stringCheckBox.IsEnabled = true;




                //if (Add_New_Line_Textbox__2.Text == "STRING" || Add_New_Line_Textbox__3.Text == "")
                //{
                //    //stringCheckBox.IsChecked = true;
                //}
                //else
                //{
                //    //stringCheckBox.IsChecked = false;
                //}

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
                var search_Name = Directory.GetCurrentDirectory() + "\\ListViewScriptsTwo.txt";
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
                                            // CheckBox fnsdfn = new CheckBox();

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
            var scriptFileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
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
            File.Delete(tempfile);
        }

        private void checkAll_Checked(object sender, RoutedEventArgs e)
        {

            //DataGrid1.ItemsSource.GetEnumerator();
            //foreach (Details stuff in DataGrid1.ItemsSource)
            //{
            //    Console.WriteLine(stuff.singleString);
            //    /stuff.gridCheckbox = true;
            //    gridcheckbox.IsChecked = true;
            //    Console.WriteLine(stuff.gridCheckbox);
            //    //string stringCheck = stuff.singleString;
            //    //bool checkboxCheck = stuff.gridCheckbox;
            //    //stuff.gridCheckbox.Equals(true);

            //}

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
            //string nextLine;

            string editedString = Add_New_Line_Textbox_1.Text.TrimEnd().TrimStart();
            string editedLinesAbove = Add_New_Line_Textbox__2.Text;
            string editedLinesBelow = Add_New_Line_Textbox__3.Text;

            string testnew = "Add New Line";

            int inttemp;
            //string stringtemp;
            //string testnew = "Add New Line";
            ////while reading the file
            //Console.WriteLine(editScriptNameTextbox.Text);
            //Console.WriteLine(ComboOne.SelectedValue);

            if(ComboOne.SelectedItem == null)
            {
                return;
            }

            if ((editedString == testnew || string.IsNullOrWhiteSpace(editedString)) && (editScriptNameTextbox.Text == ComboOne.SelectedValue.ToString()))
            {
                //main string is not empty or says Add New Line

                MessageBox.Show("You must enter a main string in the text area.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else if ((int.TryParse(editedString, out inttemp)))
            {
                MessageBox.Show("You must enter a main string in the text area.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

            }
            else if ((int.TryParse(editedLinesAbove, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesBelow)) && (!int.TryParse(editedLinesBelow, out inttemp)))
            {
                //linesabove is int and linesbelow is not empty and not an int
                MessageBox.Show("Please enter a string with a string or an int with an int", "Invalid Input", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else if ((int.TryParse(editedLinesBelow, out inttemp)) && (!int.TryParse(editedLinesAbove, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesAbove)))
            {
                //linesbelow is int and linesabove is not empty but not an int
                MessageBox.Show("Please enter a string with a string or an int with an int", "Invalid Input", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else if ((int.TryParse(editedLinesAbove, out inttemp)) && (string.IsNullOrWhiteSpace(editedLinesBelow)))
            {
                //linesabove is int and linesbelow is empty
                editedLinesBelow = "0";
                saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString);
            }
            else if ((int.TryParse(editedLinesBelow, out inttemp)) && (string.IsNullOrWhiteSpace(editedLinesAbove)))
            {
                //linesbelow is int and linesabove is empty
                editedLinesAbove = "0";
                saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString);

            }
            else if ((!int.TryParse(editedLinesBelow, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesBelow)) && (string.IsNullOrWhiteSpace(editedLinesAbove)))
            {
                //linesbelow not int and not empty and linesabove is empty
                //MessageBox.Show("Please enter a string with a string or an int with an int", "Invalid Input", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                editedLinesAbove = "";
                saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString);

            }
            else if ((!int.TryParse(editedLinesAbove, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesAbove)) && (string.IsNullOrWhiteSpace(editedLinesBelow)))
            {
                //linesabove not int and not empty and linesbelow is empty
                //must set linesbelow to editedstring, editedstring to linesabove and linesabove to empty

                editedLinesBelow = "";
                saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString);
            }
            else if ((string.IsNullOrWhiteSpace(editedLinesAbove)) && (string.IsNullOrWhiteSpace(editedLinesBelow)))
            {
                //if both lineabove and linesbelow are empty, enter 0
                editedLinesAbove = "0";
                editedLinesBelow = "0";
                saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString);

            }
            else
            {
                //save changes to line
                saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString);
            }

        }

        private void saveChangesToLines(string testnew, string editedLinesBelow, string editedLinesAbove, string editedString)
        {
            {
                bool exists = false;
                //foreach ()
                foreach (Details stuff in DataGrid1.ItemsSource)
                {
                    string mainString = stuff.singleString;
                    string above = stuff.linesAbove;
                    string below = stuff.linesBelow;
                    if (("^" + Add_New_Line_Textbox_1.Text + "$" == "^" + mainString + "$") && ("^" + above + "$" == "^" + editedLinesAbove + "$") && ("^" + below + "$" == "^" + editedLinesBelow + "$") && ("^" + editScriptNameTextbox.Text + "$" == "^" + ComboOne.SelectedValue + "$"))
                    {
                        //if all entries match another line
                        exists = true;
                        // MessageBox.Show("This string is already searched in this script", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                    else if (("^" + Add_New_Line_Textbox_1.Text + "$" != "^" + startString + "$") && (mainString == Add_New_Line_Textbox_1.Text))
                    {
                        //if startstring is different to new line and mainstring (string from foreach statement) equals the new line
                        //ie not same as started but doesn't match anything else in the list
                        //startstring is created when the textboxes are populated, it is the original string, mainstring is from the gridbox
                        exists = true;

                    }
                }
                //List<ListViewItems> anotherListViewList = new List<ListViewItems>();
                bool scriptexists = false;

                foreach (ListViewItems stuff in ListDataGrid.ItemsSource)
                {
                    string scriptName = stuff.gridNameColumn;

                    if (scriptName == editScriptNameTextbox.Text.TrimEnd().TrimStart())
                    {
                        if (ComboOne.SelectedValue.ToString() != scriptName)
                        {
                            scriptexists = true;
                        }
                    }
                    //string mainString = stuff.singleString;
                    //string above = stuff.linesAbove;
                    //string below = stuff.linesBelow;
                    //if (("^" + Add_New_Line_Textbox_1.Text + "$" == "^" + mainString + "$") && ("^" + above + "$" == "^" + editedLinesAbove + "$") && ("^" + below + "$" == "^" + editedLinesBelow + "$") && ("^" + editScriptNameTextbox.Text + "$" == "^" + ComboOne.SelectedValue + "$"))
                    //{
                    //    //if all entries match another line
                    //    exists = true;
                    //    // MessageBox.Show("This string is already searched in this script", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    //}
                    //else if (("^" + Add_New_Line_Textbox_1.Text + "$" != "^" + startString + "$") && (mainString == Add_New_Line_Textbox_1.Text))
                    //{
                    //    //if startstring is different to new line and mainstring (string from foreach statement) equals the new line
                    //    //ie not same as started but doesn't match anything else in the list
                    //    //startstring is created when the textboxes are populated, it is the original string, mainstring is from the gridbox
                    //    exists = true;

                    //}
                }



                if (exists == true)
                {
                    exists = false;
                    MessageBox.Show("String is already used", "Duplication Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else if (scriptexists == true)
                {
                    scriptexists = false;
                    MessageBox.Show("Script name is already used.", "Duplication Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                {

                    //saves changes to lines
                    string nextLine;

                    string tempfile = System.IO.Path.GetTempFileName();
                    Regex regex = new Regex("^name :" + selectedValue + "$");
                    string scriptLine;
                    var scriptFileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
                    StreamReader scriptReaderForEditing = new StreamReader(scriptFileName);
                    StreamWriter tempWriterForEditing = new StreamWriter(tempfile);

                    while ((scriptLine = scriptReaderForEditing.ReadLine()) != null)
                    {
                        //match script to be changed
                        if (regex.IsMatch(scriptLine))
                        {
                            if (regex.IsMatch("name :" + editScriptNameTextbox.Text))
                            {
                                tempWriterForEditing.WriteLine(scriptLine);
                            }
                            else
                            {
                                tempWriterForEditing.WriteLine("name :" + editScriptNameTextbox.Text);
                            }

                            if (Add_New_Line_Textbox_1.Text != "")
                            {
                                //Regex regex2 = new Regex(startString);


                                while ((nextLine = scriptReaderForEditing.ReadLine()) != "--")
                                {
                                    string[] nextString = nextLine.Split('[');
                                    string nextStr = "^" + nextString[0] + "$";
                                    //match line to be changed
                                    //if (regex2.IsMatch(nextStr))

                                    //if (startString == nextLine)
                                    if ("^" + startString + "$" == nextStr)
                                    {
                                        if (editedLinesBelow == "")
                                        {
                                            tempWriterForEditing.WriteLine("{1}[;]{0}[;]{2}", editedLinesAbove, editedString, "");
                                        }
                                        else
                                        {
                                            tempWriterForEditing.WriteLine("{2}[;]{0}[;]{1}", editedLinesAbove, editedLinesBelow, editedString);
                                        }
                                    }

                                    else
                                    {
                                        tempWriterForEditing.WriteLine(nextLine);
                                    }
                                }
                                //if (regex2.IsMatch(testnew))
                                if (startString == testnew)
                                {
                                    if (editedLinesBelow == "")
                                    {
                                        tempWriterForEditing.WriteLine("{1}[;]{0}[;]{2}", editedLinesAbove, editedString, "");
                                    }
                                    else
                                    {
                                        tempWriterForEditing.WriteLine("{2}[;]{0}[;]{1}", editedLinesAbove, editedLinesBelow, editedString);
                                    }
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
                    scriptReaderForEditing = new StreamReader(tempfile);
                    tempWriterForEditing = new StreamWriter(scriptFileName);
                    string aline;
                    while ((aline = scriptReaderForEditing.ReadLine()) != null)
                    {
                        tempWriterForEditing.WriteLine(aline);

                    }
                    tempWriterForEditing.Close();
                    scriptReaderForEditing.Close();

                    clearEditBoxes();
                    //Add_New_Line_Textbox_1.Text = "";
                    //Add_New_Line_Textbox__2.Text = "";
                    //Add_New_Line_Textbox__3.Text = "";
                    topDateTypeCheckbox.IsChecked = false;
                    btn_Save_New_Line.IsEnabled = false;

                    DataReload(regex);
                    listReload2();
                    ComboBox_Reload();
                    string newscript = editScriptNameTextbox.Text;
                    ComboOne.SelectedValue = newscript;
                    File.Delete(tempfile);
                    
                }
            }
            listIntSelection.Clear();
        }

        string Delimiter = "[;]";
        private void deleteMultipleLines(string string2Delete)
        {

            string nextLine;
            string tempfile = System.IO.Path.GetTempFileName();
            Regex regex = new Regex(selectedValue);
            string scriptLine;
            var scriptFileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
            StreamReader scriptReaderForEditing = new StreamReader(scriptFileName);
            StreamWriter tempWriterForEditing = new StreamWriter(tempfile);
            //Regex regex2 = new Regex(string2Delete);
            while ((scriptLine = scriptReaderForEditing.ReadLine()) != null)
            {
                if ("name :" + selectedValue == (scriptLine))
                {
                    tempWriterForEditing.WriteLine(scriptLine);

                    while ((nextLine = scriptReaderForEditing.ReadLine()) != "--")
                    {
                        //if (regex2.IsMatch(nextLine))
                        string[] nextLineArray = nextLine.Split(new[] { Delimiter }, StringSplitOptions.None);
                        string nextLineString = nextLineArray[0];
                        if (string2Delete == nextLineString)
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
            scriptReaderForEditing = new StreamReader(tempfile);
            tempWriterForEditing = new StreamWriter(scriptFileName);
            string aline;
            while ((aline = scriptReaderForEditing.ReadLine()) != null)
            {
                tempWriterForEditing.WriteLine(aline);

            }
            tempWriterForEditing.Close();
            scriptReaderForEditing.Close();
            DataReload(regex);
            File.Delete(tempfile);
        }

        private void btn_Save_New_Line_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void save_Script(object sender, RoutedEventArgs e)
        {
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

        private void deleteScriptsFunction(string script)
        {

            Regex regex = new Regex("^name :" + script + "$");


            string nextLine;
            string tempfile = System.IO.Path.GetTempFileName();
            //Regex regex = new Regex(selectedValue);
            string scriptLine;
            var scriptFileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
            StreamReader scriptReaderForEditing = new StreamReader(scriptFileName);
            StreamWriter tempWriterForEditing = new StreamWriter(tempfile);
            //Regex regex2 = new Regex(string2Delete);
            //foreach (string script in scriptsToDelete)
            //{
            while ((scriptLine = scriptReaderForEditing.ReadLine()) != null)
            {


                if (regex.IsMatch(scriptLine))
                {
                    while ((nextLine = scriptReaderForEditing.ReadLine()) != "--")
                    {
                    }
                }
                else
                {
                    tempWriterForEditing.WriteLine(scriptLine);

                }
            }


            tempWriterForEditing.Close();
            scriptReaderForEditing.Close();
            scriptReaderForEditing = new StreamReader(tempfile);
            tempWriterForEditing = new StreamWriter(scriptFileName);
            string aline;
            while ((aline = scriptReaderForEditing.ReadLine()) != null)
            {
                tempWriterForEditing.WriteLine(aline);

            }
            tempWriterForEditing.Close();
            scriptReaderForEditing.Close();
            DataReload(regex);
            listReload2();
            File.Delete(tempfile);
        }

        private void deleteScriptsButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> scriptsToDelete = new List<string>();
            int selectcount = 0;
            foreach (ListViewItems stuff in ListDataGrid.ItemsSource)
            {

                string scriptName = stuff.gridNameColumn;

                //if (scriptCheckbox == true)
                //{
                //    scriptsToDelete.Add(scriptName);

                //}

                foreach (int selected in listIntSelection)
                {
                    if (selectcount == selected)
                    {
                        scriptsToDelete.Add(scriptName);
                    }
                }
                selectcount++;
            }

            if (scriptsToDelete.Count == 0)
            {
                MessageBox.Show("No Scripts Selected", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                if (scriptsToDelete.Count == 1)
                {
                    if (MessageBox.Show("Are you sure you wish to delete this script?", "Delete Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        int selectcount2 = 0;
                        foreach (ListViewItems stuff in ListDataGrid.ItemsSource)
                        {
                            string scriptName = stuff.gridNameColumn;
                            foreach (int selected in listIntSelection)
                            {

                                if (selectcount2 == selected)
                                {

                                    //}
                                    //}

                                    //if (scriptCheckbox == true)
                                    //{

                                    deleteScriptsFunction(scriptName);

                                }

                            }
                            try
                            {
                                if (ComboOne.SelectedValue.ToString() == scriptName)
                                {
                                    ComboOne.SelectedIndex = 0;
                                }
                            }
                            catch
                            {

                            }

                            selectcount2++;

                        }
                    }
                    listIntSelection.Clear();
                }
                else
                {
                    if (MessageBox.Show("Are you sure you wish to delete these scripts?", "Delete Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        int selectcount2 = 0;
                        foreach (ListViewItems stuff in ListDataGrid.ItemsSource)
                        {
                            string scriptName = stuff.gridNameColumn;

                            foreach (int selected in listIntSelection)
                            {
                                if (selectcount2 == selected)
                                {

                                    //}

                                    //if (scriptCheckbox == true)
                                    //{
                                    deleteScriptsFunction(scriptName);
                                    try
                                    {
                                        if (ComboOne.SelectedValue.ToString() == scriptName)
                                        {
                                            ComboOne.SelectedIndex = 0;
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            selectcount2++;
                        }
                    }
                    listIntSelection.Clear();

                }
            }
            
        }


        private void Add_New_Line_Textbox_1_Loaded(object sender, RoutedEventArgs e)
        {
        }


        private void editScriptNameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void lvInvDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void topDateTypeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            Add_New_Line_Textbox__2.Text = "DATE";
            Add_New_Line_Textbox__3.Text = "DATE";
            Add_New_Line_Textbox__2.IsEnabled = false;
            Add_New_Line_Textbox__3.IsEnabled = false;
            //stringCheckBox.IsEnabled = false;
        }



        private void topDateTypeCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            Add_New_Line_Textbox__2.Clear();
            Add_New_Line_Textbox__3.Clear();
            Add_New_Line_Textbox__2.IsEnabled = true;
            Add_New_Line_Textbox__3.IsEnabled = true;

            //stringCheckBox.IsEnabled = true;
        }

        private void deleteMultipleLinesButton_Click(object sender, RoutedEventArgs e)
        {
            List<Details> deleteLinesList;

            deleteLinesList = null;
            if (DataGrid1.Items.Count == 0)
            {
                MessageBox.Show("No Script Selected.", "Deletion Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

            }
            else
            {
                deleteLinesList = new List<Details>();
                DataGrid1.ItemsSource.GetEnumerator();
                int selectcount = 0;
                foreach (Details stuff in DataGrid1.ItemsSource)
                {
                    string stringCheck = stuff.singleString;
                    //bool checkboxCheck = stuff.gridCheckbox;


                    if (stringCheck != "Add New Line")
                    {
                        //if (checkboxCheck == true)
                        foreach (int selected in dataIntSelection)
                        {
                            if (selectcount == selected)
                            {
                                {
                                    deleteLinesList.Add(stuff);
                                    //deleteMultipleLines(stringCheck);
                                }
                            }
                        }
                    }
                    selectcount++;
                }
                if (deleteLinesList.Count != 0)
                {
                    deleteMultipleLinesButton.Visibility = Visibility.Visible;
                    if (deleteLinesList.Count == 1)
                    {
                        if (MessageBox.Show("Are you sure you wish to delete this line?", "Delete Check", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            foreach (Details lines in deleteLinesList)
                            {
                                string stringSelected = lines.singleString;
                                deleteMultipleLines(stringSelected);
                            }
                            datacheckboxheader.IsChecked = false;
                            deleteLinesList.Clear();
                            dataIntSelection.Clear();
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Are you sure you wish to delete these lines?", "Delete Check", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            foreach (Details lines in deleteLinesList)
                            {
                                string stringSelected = lines.singleString;
                                deleteMultipleLines(stringSelected);

                            }
                            datacheckboxheader.IsChecked = false;
                            deleteLinesList.Clear();
                            dataIntSelection.Clear();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("No Lines Selected", "Deletion Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            //dataIntSelection.Clear();
        }

        List<string> selectedLinesList = new List<string>();
        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            selectedLinesList.Add(this.startString);
            //foreach (string line in selectedLinesList)
            //{
            //    Console.WriteLine(line);
            //}
        }


        private void unCheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            selectedLinesList.Remove(this.startString);
        }



        private void listCheckBoxtop_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("damn checkboxes!");
        }

        private void gridCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("damn checkboxes!");

        }

        List<Details> copyLinesList;
        private void copyMultipleLinesButton_Click(object sender, RoutedEventArgs e)
        {
            int selectcount = 0;
            copyLinesList = null;
            if (DataGrid1.Items.Count == 0)
            {
                MessageBox.Show("No Script Selected.", "Copy Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

            }
            else
            {
                copyLinesList = new List<Details>();
                foreach (Details stuff in DataGrid1.ItemsSource)
                {
                    string stringCheck = stuff.singleString;
                    //bool checkboxCheck = stuff.gridCheckbox;
                    //if (checkboxCheck == true)
                    foreach (int selected in dataIntSelection)
                    {
                        if (selectcount == selected)
                        {
                            if (stringCheck != "Add New Line")
                            {
                                copyLinesList.Add(stuff);
                            }
                        }
                        else
                        {
                        }
                    }
                    selectcount++;
                }
                if (copyLinesList.Count != 0)
                {
                    pasteButton.IsEnabled = IsEnabled;
                }
                else
                {
                    MessageBox.Show("No Lines Selected.", "Copy Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            dataIntSelection.Clear();
        }

        private void pasteButton_Click(object sender, RoutedEventArgs e)
        {
            //bool replace = false;
            List<Details> donotreplace = new List<Details>();

            List<Details> replace = new List<Details>();
            foreach (Details linesInScript in DataGrid1.ItemsSource)
            {
                foreach (Details copied in copyLinesList)
                {
                    if (copied.singleString == linesInScript.singleString)
                    {
                        string currentCopy = copied.singleString;
                        if (MessageBox.Show("You already have a match for " + currentCopy + ", do you want to replace it?", "Copy Conflict", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)

                        //MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("You already have a match for " + currentCopy + ", do you want to replace it?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                        //if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            //replace = true;
                            //Console.WriteLine(copied.singleString + " " + copied.linesAbove + " " + copied.linesBelow);

                            replace.Add(copied);
                            donotreplace.Add(copied);
                        }
                        else
                        {
                            donotreplace.Add(copied);
                        }
                    }
                }
            }
            foreach (Details dontReplace in donotreplace)
            {
                copyLinesList.Remove(dontReplace);
            }


            pasteButton.IsEnabled = false;
            string scriptLine;
            string nextLine;
            string tempfile = System.IO.Path.GetTempFileName();
            Regex regex = new Regex(selectedValue);
            var scriptFileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
            StreamReader pasteReader = new StreamReader(scriptFileName);
            StreamWriter pasteWriter = new StreamWriter(tempfile);
            //StreamWriter scriptWriterForEditing = new StreamWriter(scriptFileName);

            //string testnew = "Add New Line";
            //while reading the file
            while ((scriptLine = pasteReader.ReadLine()) != null)
            {
                //match script to be changed
                if (regex.IsMatch(scriptLine))
                {
                    pasteWriter.WriteLine(scriptLine);
                    foreach (Details stuff in copyLinesList)
                    {
                        //string stringCheck = stuff.singleString;
                        pasteWriter.WriteLine("{2}[;]{0}[;]{1}", stuff.linesAbove, stuff.linesBelow, stuff.singleString);
                    }

                    while ((nextLine = pasteReader.ReadLine()) != "--")
                    {
                        pasteWriter.WriteLine(nextLine);

                    }

                    pasteWriter.WriteLine("--");
                }
                else
                {
                    pasteWriter.WriteLine(scriptLine);
                }

            }
            pasteWriter.Close();
            pasteReader.Close();
            //var testfile = Directory.GetCurrentDirectory() + @"\test.txt";
            pasteReader = new StreamReader(tempfile);
            pasteWriter = new StreamWriter(scriptFileName);
            string aline;
            while ((aline = pasteReader.ReadLine()) != null)
            {
                pasteWriter.WriteLine(aline);

            }
            pasteWriter.Close();
            pasteReader.Close();
            DataReload(regex);
            ComboBox_Reload();
            string newscript = editScriptNameTextbox.Text;
            ComboOne.SelectedValue = newscript;
            copyLinesList.Clear();

            if (replace.Count != 0)
            {
                foreach (Details replaceitem in replace)
                {
                    replaceFromCopy(replaceitem);
                }
            }
        }

        private void replaceFromCopy(Details replace)
        {
            string scriptLine2;
            string nextLine2;
            string tempfile2 = System.IO.Path.GetTempFileName();
            Regex regex2 = new Regex(selectedValue);
            var scriptFileName2 = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
            StreamReader pasteReader2 = new StreamReader(scriptFileName2);
            StreamWriter pasteWriter2 = new StreamWriter(tempfile2);
            //StreamWriter scriptWriterForEditing = new StreamWriter(scriptFileName);

            //string testnew = "Add New Line";
            //while reading the file
            while ((scriptLine2 = pasteReader2.ReadLine()) != null)
            {
                //match script to be edited
                if (regex2.IsMatch(scriptLine2))
                {
                    pasteWriter2.WriteLine(scriptLine2);
                    while ((nextLine2 = pasteReader2.ReadLine()) != "--")
                    {
                        string[] mainStrings = nextLine2.Split(new[] { Delimiter }, StringSplitOptions.None);
                        string mainString = mainStrings[0];
                        //foreach (Details doreplace in replace)
                        //{
                        if (replace.singleString == mainString)
                        {
                            pasteWriter2.WriteLine("{2}[;]{0}[;]{1}", replace.linesAbove, replace.linesBelow, replace.singleString);
                        }
                        else
                        {
                            pasteWriter2.WriteLine(nextLine2);
                        }
                        //}
                    }
                    pasteWriter2.WriteLine("--");
                }
                else
                {
                    pasteWriter2.WriteLine(scriptLine2);
                }
            }

            pasteReader2.Close();
            pasteWriter2.Close();
            //var testfile = Directory.GetCurrentDirectory() + @"\test.txt";
            pasteReader2 = new StreamReader(tempfile2);
            pasteWriter2 = new StreamWriter(scriptFileName2);
            string aline2;
            while ((aline2 = pasteReader2.ReadLine()) != null)
            {
                //Console.WriteLine(aline);
                pasteWriter2.WriteLine(aline2);

            }
            pasteWriter2.Close();
            pasteReader2.Close();
            Regex regex = new Regex(selectedValue);

            DataReload(regex);

        }


        private void editScriptNameTextbox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            btn_Save_New_Line.IsEnabled = true;
        }

        private void Add_New_Line_Textbox_1_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            btn_Save_New_Line.IsEnabled = true;
        }

        private void Add_New_Line_Textbox__2_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_Save_New_Line.IsEnabled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Start_Time__ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            startTimeValue = e.NewValue.ToString();
            string[] splitValue = startTimeValue.Split(' ');
            startTimeValue = startTimeValue.Remove(8);
            startTimeValue = startTimeValue + splitValue[1];



        }

        private void End_time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            endTimeValue = e.NewValue.ToString();
            string[] splitValueTwo = endTimeValue.Split(' ');
            endTimeValue = endTimeValue.Remove(8);
            endTimeValue = endTimeValue + splitValueTwo[1];

        }

        private void Add_New_Line_Textbox__3_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_Save_New_Line.IsEnabled = true;
        }

        private void ComboBox_Loaded_1(object sender, RoutedEventArgs e)
        {
            List<string> copyData = new List<string>();
            copyData.Add("Copy_To");

            foreach (string copystuff in data)
            {
                copyData.Add(copystuff);
            }
            copyData.RemoveAt(1);
            //copyComboBox.ItemsSource = copyData;
            //copyComboBox.AddHandler(data);
            //data.Clear();
            ////ComboOne.UpdateDefaultStyle();
            //var fileName = Directory.GetCurrentDirectory() + "\\ListViewScriptsTwo.txt";
            //splitName = System.IO.Path.GetFileName(fileName);
            //string line;
            //StreamReader file = new StreamReader(splitName);
            //data.Add("Script_Selection");
            //while ((line = file.ReadLine()) != null)
            //{
            //    Regex regex = new Regex("name :");
            //    if (regex.IsMatch(line))
            //    {
            //        line_words = line.Split(':');
            //        data.Add(line_words[1].ToString());
            //    }
            //}
            //file.Close();
        }

        private void ListDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            listReload2();
        }

        private void TempDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Console.WriteLine(ListDataGrid.SelectedIndex);
            foreach (ListViewItems selected in ListDataGrid.ItemsSource)
            {
                if (selected == ListDataGrid.SelectedValue)
                {
                    //ComboOne.Initialized;
                    //ComboOne.AllowDrop = true;
                    //ComboOne.Text = selected.gridNameColumn;
                    //ComboOne.SelectionChanged += ComboOne_SelectionChanged;

                    //ComboOne.Loaded += ComboBox_Loaded;
                    //ComboOne.SelectedItem = selected.gridNameColumn;
                    //ComboBox_Reload();
                    //ComboOne.IsDropDownOpen = true;
                    //ListDataGrid.SelectionChanged += ComboBox_SelectionChanged;
                    //ComboOne
                    //ComboOne.IsDropDownOpen = true;
                    ComboOne.SelectedItem = selected.gridNameColumn;

                    Console.WriteLine(selected.gridNameColumn);
                }
            }
            Console.WriteLine(ListDataGrid.SelectedValue);


            //        MouseGesture CutCmdMouseGesture = new MouseGesture(
            //MouseAction.LeftClick);
            //        CutCmdMouseGesture.MouseAction.Equals(true);

            //MouseBinding CutMouseBinding = new MouseBinding(
            //    ApplicationCommands.Cut,
            //    CutCmdMouseGesture);

            // RootWindow is an instance of Window.

            //MainWindow.InputBindings.Add(CutMouseBinding);

            //MouseGesture CutCmdMouseGesture = new MouseGesture(MouseAction.LeftClick);
            //Window.InputBindings.Add(CutMouseBinding);
            //MouseAction.LeftClick;
        }

        Window1 win2 = new Window1();
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

            try
            {
                win2.Show();

            }
            catch (Exception)
            {
                win2 = new Window1();
                win2.Show();
            }
            win2.Closed += win2_Closed;
        }

        private void WFO_Grep_Tool_Closed(object sender, EventArgs e)
        {
            scriptWindow.Close();
            win2.Close();

            perlprocesswaskilled = true;
            foreach (Process process in processList)
            {
                process.Kill();
            }
            processList.Clear();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (processList.Count <= 0)
            {
                MessageBox.Show("You are currently not running any scripts!", "Stop Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure you wish to stop all scripts?", "Stop Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)

                //MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you wish to stop all scripts?", "Stop Confirmation", System.Windows.MessageBoxButton.YesNo);
                //if (messageBoxResult == MessageBoxResult.Yes)
                {
                    foreach (Process process in processList)
                    {
                        try
                        {
                            process.Kill();

                        }
                        catch
                        {
                        }
                    }
                    perlprocesswaskilled = true;
                    MessageBox.Show("You quit while still running scripts, the data in the files may not be correct.", "Data Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    //MessageBox.Show("You quit while still running scripts, the data in the files may not be correct", "Data Warning");

                    processList.Clear();
                }
            }

        }
        void win2_Closed(object sender, EventArgs e)
        {
            int counter;
            string splitstartTime = "";
            int hours;
            string ABG;

            try
            {

                Hex = win2.hexString.ToString();

            }
            catch (Exception)
            {
                Hex = null;

            }

            try
            {
                Exclude = win2.excludeString.ToString();

            }
            catch (Exception)
            {
                Exclude = null;
            }

            try
            {
                startTime = win2.startOne.ToString();
                endTime = win2.endOne.ToString();

            }
            catch (NullReferenceException)
            {
                return;
            }

            try
            {
                splitstartTime = startTime.Split(' ')[2];

            }
            catch (Exception)
            {
                splitstartTime = startTime.Split('/')[2];
                splitstartTime = splitstartTime.Split(' ')[0];
                startTime = startTime.Replace("/" + splitstartTime, "");
                startTime = startTime.Insert(0, splitstartTime + "/");

                splitstartTime = endTime.Split('/')[2];
                splitstartTime = splitstartTime.Split(' ')[0];
                endTime = endTime.Replace("/" + splitstartTime, "");
                endTime = endTime.Insert(0, splitstartTime + "/");
            }




            splitstartTime = startTime.Split('/')[2];
            splitstartTime = splitstartTime.Split(' ')[0];
            startTime = startTime.Replace("/" + splitstartTime, "");
            startTime = startTime.Insert(0, splitstartTime + "/");
            startTime = startTime.Replace(" " + splitstartTime, "");
            splitstartTime = startTime.Split('/')[1];
            int split = Convert.ToInt32(splitstartTime);
            if (split < 10)
            {
                startTime = startTime.Replace("/" + splitstartTime, "/0" + split.ToString());
            }
            splitstartTime = startTime.Split(' ')[2];
            splitstartTime = startTime.Replace(" " + splitstartTime, "");
            splitstartTime = splitstartTime.Replace("/", "");
            splitstartTime = splitstartTime.Replace(":", "");
            splitstartTime = splitstartTime.Replace(" ", "");




            splitstartTime = endTime.Split('/')[2];
            splitstartTime = splitstartTime.Split(' ')[0];
            endTime = endTime.Replace("/" + splitstartTime, "");
            endTime = endTime.Insert(0, splitstartTime + "/");
            endTime = endTime.Replace(" " + splitstartTime, "");

            splitstartTime = endTime.Split('/')[1];
            split = Convert.ToInt32(splitstartTime);
            if (split < 10)
            {
                endTime = endTime.Replace("/" + splitstartTime, "/0" + split.ToString());
            }


            splitstartTime = endTime.Split(' ')[2];
            splitstartTime = endTime.Replace(" " + splitstartTime, "");
            splitstartTime = splitstartTime.Replace("/", "");
            splitstartTime = splitstartTime.Replace(":", "");
            splitstartTime = splitstartTime.Replace(" ", "");



       

            startTime = startTime.Replace("/", "-");
            splitstartTime = startTime.Split(' ')[2];
            startTime = startTime.Replace(" " + splitstartTime, "");
            if (splitstartTime == "PM")
            {
                splitstartTime = startTime.Split(':')[0];
                splitstartTime = splitstartTime.Split(' ')[1];
                if (splitstartTime != "12")
                {
                    counter = Convert.ToInt32(splitstartTime) + 12;
                    ABG = splitstartTime.ToString() + ":";
                    hours = startTime.IndexOf(ABG);
                    startTime = startTime.Replace(ABG, "");
                    startTime = startTime.Insert(hours, counter.ToString() + ":");
                }

            }
            else if (splitstartTime == "AM")
            {
                splitstartTime = startTime.Split(':')[0];
                splitstartTime = splitstartTime.Split(' ')[1];
                if (splitstartTime != "12" && splitstartTime != "11" && splitstartTime != "10")
                {
                    startTime = startTime.Insert(10, "0");

                }

            }
            splitstartTime = startTime.Split('-')[2];
            splitstartTime = splitstartTime.Split(' ')[0];
            counter = Convert.ToInt32(splitstartTime);
            for (int i = 0; i <= 9; i++)
            {
                if (counter == i)
                {
                    startTime = startTime.Replace("-" + splitstartTime, "-0" + splitstartTime);
                }

            }

            startTime = startTime.Replace(" ", "");




            endTime = endTime.Replace("/", "-");
            splitstartTime = endTime.Split(' ')[2];
            endTime = endTime.Replace(" " + splitstartTime, "");
            if (splitstartTime == "PM")
            {
                splitstartTime = endTime.Split(':')[0];
                splitstartTime = splitstartTime.Split(' ')[1];
                if (splitstartTime != "12")
                {
                    counter = Convert.ToInt32(splitstartTime) + 12;
                    ABG = splitstartTime.ToString() + ":";
                    hours = endTime.IndexOf(ABG);
                    endTime = endTime.Replace(ABG, "");
                    endTime = endTime.Insert(hours, counter.ToString() + ":");
                }

            }
            else if (splitstartTime == "AM")
            {
                splitstartTime = endTime.Split(':')[0];
                splitstartTime = splitstartTime.Split(' ')[1];
                if (splitstartTime != "12" && splitstartTime != "11" && splitstartTime != "10")
                {
                    endTime = endTime.Insert(10, "0");

                }

            }

            splitstartTime = endTime.Split('-')[2];
            splitstartTime = splitstartTime.Split(' ')[0];
            counter = Convert.ToInt32(splitstartTime);
            for (int l = 0; l <= 9; l++)
            {
                if (counter == l)
                {
                    endTime = endTime.Replace("-" + splitstartTime, "-0" + splitstartTime);
                }

            }
            endTime = endTime.Replace(" ", "");



        }
        // Help menu button
        private void MenuItem_HelpButton(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:/Program Files (x86)/SolarWinds/SolarWindsEventLogConsolidator/EventLogConsolidator.htm");
        }
        // About menu button
        //private void MenuItem_AboutButton(object sender, RoutedEventArgs e)
        //{
        //    System.Diagnostics.Process.Start(@"C:/Program Files (x86)/SolarWinds/SolarWindsEventLogConsolidator/EventLogConsolidator.htm");
        //}

        AboutBox1 aboutboxwindow = new AboutBox1();
        private void MenuItem_AboutButton(object sender, RoutedEventArgs e)
        {
            try
            {
                aboutboxwindow.Show();
                aboutboxwindow.Show();
            }
            catch (Exception)
            {
                aboutboxwindow = new AboutBox1();
                aboutboxwindow.Show();

                aboutboxwindow.Activate();
            }

        }

        private void ListDataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //ListDataGrid.IsEnabled = true;

            Console.WriteLine("aaah stuff");

            //ListDataGrid.SelectedItem = ListDataGrid.ItemsSource.Equals(true);
            //DataGridCell cell = sender as DataGridCell;
            //Console.WriteLine(cell.Content);
            //if (!cell.IsEditing)
            //{
            //    // enables editing on single click
            //    if (!cell.IsFocused)
            //        cell.Focus();
            //    if (!cell.IsSelected)
            //        cell.IsSelected = true;
            //}
        }

        private void ListDataGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            ListDataGrid.IsEnabled = true;
            //ListDataGrid.
            //Console.WriteLine("aaah stuff");
        }

        List<int> listIntSelection = new List<int>();
        private void listcheckbox_Checked(object sender, RoutedEventArgs e)
        {
            listIntSelection.Add(ListDataGrid.SelectedIndex);
        }

        private void listcheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            listIntSelection.Remove(ListDataGrid.SelectedIndex);
        }



        List<ListViewItems> listAllChecked = new List<ListViewItems>();


        private void progressTimer() {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();
        
        
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try 
	        {

                int j = Convert.ToInt32(time * .97);
                if (j < 100)
                {
                    j = j * 10;
                }
                for (int i = 0; i < 100; i++)
                {
                    (sender as BackgroundWorker).ReportProgress(i);
                    Thread.Sleep(j);
                    //j += 100 / Convert.ToInt32(timeTwo);
                }
	        }
	        catch (Exception)
	        {
		       
	        }
            
          
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }


        List<ListViewItems> allcheckedlist = new List<ListViewItems>();
        private void listcheckboxheader_Checked(object sender, RoutedEventArgs e)
        {
            listIntSelection.Clear();
            int count = 1;
            comboBox.IsChecked = true;
            listAllChecked.Clear();
            Console.WriteLine("checked");
            foreach (ListViewItems tocheck in ListDataGrid.ItemsSource)
            {
                //Console.WriteLine(tocheck.gridCheckboxColumn);
                ListViewItems list = new ListViewItems();

                list.gridNameColumn = tocheck.gridNameColumn;
                tocheck.gridCheckboxColumn = true;
                list.gridCheckboxColumn = tocheck.gridCheckboxColumn;
                listAllChecked.Add(list);
                listIntSelection.Add(count);
                count++;
            }
            //ListDataGrid.ItemsSource = allcheckedlist;
            allCheckedInList();
        }

        private void listcheckboxheader_Unchecked(object sender, RoutedEventArgs e)
        {
            listIntSelection.Clear();
            listAllChecked.Clear();
            Console.WriteLine("unchecked");

            foreach (ListViewItems tocheck in ListDataGrid.ItemsSource)
            {
                //Console.WriteLine(tocheck.gridCheckboxColumn);
                ListViewItems list = new ListViewItems();

                list.gridNameColumn = tocheck.gridNameColumn;
                tocheck.gridCheckboxColumn = false;
                list.gridCheckboxColumn = tocheck.gridCheckboxColumn;
                listAllChecked.Add(list);
                listIntSelection.Remove(count);
                count++;
            }
            allCheckedInList();
        }

        private void allCheckedInList()
        {
            List<ListViewItems> alistoflists = new List<ListViewItems>();
            foreach(ListViewItems checkstuff in listAllChecked)
            {
                ListViewItems alist = new ListViewItems();
                alist.gridNameColumn = checkstuff.gridNameColumn;
                alist.gridCheckboxColumn = checkstuff.gridCheckboxColumn;
                alistoflists.Add(alist);
            }
            
            ListDataGrid.ItemsSource=alistoflists;
        }




/// <summary>
/// ///////////////////////////////////////////////////////////////////////////////////////////
/// </summary>



        List<int> dataIntSelection = new List<int>();
        private void datagridcheckbox_Checked(object sender, RoutedEventArgs e)
        {
            dataIntSelection.Add(DataGrid1.SelectedIndex);
        }

        private void datagridcheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            dataIntSelection.Remove(DataGrid1.SelectedIndex);
        }

        List<Details> dataAllChecked = new List<Details>();
        private void datacheckboxheader_Checked(object sender, RoutedEventArgs e)
        {
            dataIntSelection.Clear();
            int count = 1;
            gridCheckAllBox.IsChecked = true;
            dataAllChecked.Clear();
            foreach (Details tocheck in DataGrid1.ItemsSource)
            {
                //Console.WriteLine(tocheck.gridCheckboxColumn);
                Details list = new Details();

                tocheck.gridCheckbox = true;
                list.gridCheckbox = tocheck.gridCheckbox;
                list.singleString = tocheck.singleString;
                list.linesAbove = tocheck.linesAbove;
                list.linesBelow = tocheck.linesBelow;

                dataAllChecked.Add(list);
                dataIntSelection.Add(count);
                count++;
            }
            //ListDataGrid.ItemsSource = allcheckedlist;
            allCheckedInData();
        }

        private void datacheckboxheader_Unchecked(object sender, RoutedEventArgs e)
        {
            dataIntSelection.Clear();
            int count = 1;
            gridCheckAllBox.IsChecked = false;
            dataAllChecked.Clear();
            foreach (Details tocheck in DataGrid1.ItemsSource)
            {
                //Console.WriteLine(tocheck.gridCheckboxColumn);
                Details list = new Details();

                tocheck.gridCheckbox = false;
                list.gridCheckbox = tocheck.gridCheckbox;
                list.singleString = tocheck.singleString;
                list.linesAbove = tocheck.linesAbove;
                list.linesBelow = tocheck.linesBelow;

                dataAllChecked.Add(list);
                dataIntSelection.Remove(count);
                count++;
            }
            //ListDataGrid.ItemsSource = allcheckedlist;
            allCheckedInData();
        }

        private void allCheckedInData()
        {
            List<Details> alistoflists = new List<Details>();
            foreach (Details checkstuff in dataAllChecked)
            {
                Details alist = new Details();
                alist.gridCheckbox = checkstuff.gridCheckbox;
                alist.singleString = checkstuff.singleString;
                alist.linesAbove = checkstuff.linesAbove;
                alist.linesBelow = checkstuff.linesBelow;
                alistoflists.Add(alist);
            }

            DataGrid1.ItemsSource = alistoflists;
        }

        EditForDiagramWindow diagramEditingWindow = new EditForDiagramWindow();
        private void editForGraph_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                diagramEditingWindow.Show();
                diagramEditingWindow.Activate();
            }
            catch (Exception)
            {
                diagramEditingWindow = new EditForDiagramWindow();
                diagramEditingWindow.Show();
                diagramEditingWindow.Activate();
            }
            diagramEditingWindow.Closed += diagramEditingWindow_Closed;
        }

        void diagramEditingWindow_Closed(object sender, EventArgs e)
        {

        }

    }
}


               

                
        
