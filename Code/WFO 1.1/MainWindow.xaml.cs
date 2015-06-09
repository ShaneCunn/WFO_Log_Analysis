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
using System.Web.UI;
using Newtonsoft.Json;
using System.Runtime.InteropServices;


namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        /// <summary>
        /// The delimiter
        /// </summary>
        string Delimiter = "[;]";
        //contains the selected value in scriptselectcombo
        /// <summary>
        /// The selected value in scriptselectcombo
        /// </summary>
        string selectedValue;
        //assigned in scriptselectcombo_selectionchanged, this contains the file that contains all the scripts
        /// <summary>
        /// The split name assigned in scriptselectcombo_selectionchanged, this contains the file that contains all the scripts
        /// </summary>
        string splitName = "";
        //assigned in chooseoutputfolderbutton_click, this contains the folder to save to ! should also be used in select file if created there automatically
        /// <summary>
        /// The output file name 
        /// </summary>
        string outputFileName;
        //is entire address of selected file
        /// <summary>
        /// The file_ name
        /// </summary>
        string file_Name;
        //used in win2 closed
        /// <summary>
        /// The start time
        /// </summary>
        string startTime;
        //used in win2 closed
        /// <summary>
        /// The end time
        /// </summary>
        string endTime;
        //used in win2 closed
        /// <summary>
        /// The hexadecimal
        /// </summary>
        string Hex;
        //used in win2 closed
        /// <summary>
        /// The exclude
        /// </summary>
        string Exclude;
        //contains the starting main string from the editdatagrid
        /// <summary>
        /// The start string
        /// </summary>
        string startString;
        //contains the starting lines above (or date or string) string from the editdatagrid
        /// <summary>
        /// The start lines above
        /// </summary>
        string startLinesAbove;
        //contains the starting lines below (or date or string) string from the editdatagrid
        /// <summary>
        /// The start lines below
        /// </summary>
        string startLinesBelow;
        //contains the starting 'from' value string from the editdatagrid
        /// <summary>
        /// The start selection one
        /// </summary>
        string startSelectionOne;
        //contains the starting 'to' value string from the editdatagrid
        /// <summary>
        /// The start selection two
        /// </summary>
        string startSelectionTwo;
        //minimum time required to complete the parse
        /// <summary>
        /// The time
        /// </summary>
        double time;
        //maximum time required to complete the parse
        /// <summary>
        /// The time two
        /// </summary>
        double timeTwo;

        /// <summary>
        /// The count
        /// </summary>
        int count = 0;


        int scriptcount = 0;
        /// <summary>
        /// The filesize
        /// </summary>
        long filesize;


        /// <summary>
        /// The listcheck
        /// </summary>
        bool listcheck = false;

        /// <summary>
        /// The combo box
        /// </summary>
        CheckBox comboBox = new CheckBox();
        /// <summary>
        /// The grid check all box
        /// </summary>
        CheckBox gridCheckAllBox = new CheckBox();
        /// <summary>
        /// The box
        /// </summary>
        CheckBox box = new CheckBox();

        /// <summary>
        /// The selected details
        /// </summary>
        Details selectedDetails;

        /// <summary>
        /// The aboutboxwindow
        /// </summary>
        AboutBox1 aboutboxwindow = new AboutBox1();

        /// <summary>
        /// The word_ grep line
        /// </summary>
        string[] word_GrepLine;

        /// <summary>
        /// The data
        /// </summary>
        List<string> data = new List<string>();
        /// <summary>
        /// The selected lines list
        /// </summary>
        List<string> selectedLinesList = new List<string>();

        /// <summary>
        /// The listint
        /// </summary>
        List<int> listint = new List<int>();

        List<string> linelist;
        /// <summary>
        /// The list int selection
        /// </summary>
        List<int> listIntSelection = new List<int>();
        //List<int> dataIntSelection = new List<int>();

        /// <summary>
        /// The options list
        /// </summary>
        List<CheckBox> OptionsList = new List<CheckBox>();

        /// <summary>
        /// The copy lines list
        /// </summary>
        List<Details> copyLinesList = new List<Details>();
        /// <summary>
        /// The data all checked
        /// </summary>
        List<Details> dataAllChecked = new List<Details>();

        /// <summary>
        /// The list all checked
        /// </summary>
        List<ListViewItems> listAllChecked = new List<ListViewItems>();
        /// <summary>
        /// The allcheckedlist
        /// </summary>
        List<ListViewItems> allcheckedlist = new List<ListViewItems>();
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //new SplashWindow().ShowDialog();


            if (!File.Exists("ListViewScriptsTwo.txt"))
            {
                MessageBox.Show("You are missing a necessary file, please contact your provider.", "Missing file", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /*
         * start of top left
        */

        private void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileLabel.IsEnabled = true;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "LOG Files (.log)|*.log|Text Files (.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                file_Name = dlg.FileName;

                FileLabel.Text = System.IO.Path.GetFileName(file_Name);

                FileInfo f = new FileInfo(file_Name);
                filesize = f.Length;

            }
            if (file_Name != null)
            {
                string fileCheck = System.IO.Path.GetExtension(file_Name);
                if (fileCheck != ".txt" && fileCheck != ".log")
                {
                    MessageBox.Show("Invalid file format.\nPlease use file types with the extensions .log or .txt.", "Invalid file", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    FileLabel.Text = string.Empty;
                    return;

                }
                else
                {
                    OutputLabel.Text = System.IO.Path.GetDirectoryName(file_Name);
                    FileLabel.Text = System.IO.Path.GetFileName(file_Name);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ChooseOutputFolderButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ChooseOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            OutputLabel.IsEnabled = true;
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            if (file_Name != null)
            {
                string pathName;
                string fileNameRemoval = System.IO.Path.GetFileName(file_Name);
                int outputFilePath = file_Name.IndexOf("\\" + fileNameRemoval);
                pathName = file_Name.Remove(outputFilePath);
                dlg.SelectedPath = pathName;
            }
            dlg.ShowDialog();
            // Open folder for output and show in label 
            outputFileName = dlg.SelectedPath;
            string[] getName = outputFileName.Split('\\');
            OutputLabel.Text = getName.Last();
        }

        // The Xaml for this window is caleed: DatepickerWindow.xaml
        Window1 win2 = new Window1();
        private void ExtraOptionsButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                win2.ShowDialog();
                win2.Focus();
            }
            catch (Exception)
            {
                win2 = new Window1();
                win2.ShowDialog();
                win2.Focus();
            }
            win2.Closed += win2_Closed;
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
        /*
       * end of top left
       */


        /*
        * start of bottom left        
        */
        public class ListViewItems
        {
            public string gridNameColumn { get; set; }
            public bool gridCheckboxColumn { get; set; }
        }

        private void ListDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            listReload();
        }

        private void listReload()
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
                }
            }
            ListDataGrid.ItemsSource = anotherListViewList;
            ListDataGrid.Columns[1].Header = "Select Script to Parse With";
            ListDataGrid.Columns[0].Width = 24;
            ListDataGrid.Columns[1].IsReadOnly = true;
            file_open.Close();
        }

        private void ListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Reload();
            foreach (ListViewItems selected in ListDataGrid.ItemsSource)
            {
                if (selected == ListDataGrid.SelectedValue)
                {
                    ScriptSelectCombo.SelectedValue = selected.gridNameColumn;
                }
            }
        }

        private void GrepButton_Click(object sender, RoutedEventArgs e)
        {

            List<string> graphArguments = new List<string>();
            int argumentCount = 0;

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
                if (stuff.gridCheckboxColumn == true)
                //foreach (int selected in listIntSelection)
                {
                    //if (selectcount == selected)
                    //{
                    scriptsToDelete.Add(scriptName);
                    //}
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

                ListDataGrid.ItemsSource.GetEnumerator();
                Regex intregex = new Regex(@"\d+");
                Regex stringRegex = new Regex(@"[\D\d]+");
                string searchWord = "";
                char[] MyCharList = { '\\', '*', '.', '"', ']', '[' };
                int selectcount2 = 0;
                //foreach (ListViewItems Option in ListDataGrid.ItemsSource)
                foreach (string items in scriptsToDelete)
                {
                    string nextLine = "";
                    string line;
                    var fileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
                    StreamReader filePathing = new StreamReader(fileName);
                    while ((line = filePathing.ReadLine()) != null)
                    {
                        //foreach (string items in scriptsToDelete)
                        {
                            Console.WriteLine(items);
                            Regex regex = new Regex("name :" + items);
                            if (regex.IsMatch(line))
                            {
                                //if (Option.gridCheckboxColumn == true)
                                //foreach (int selected in listIntSelection)
                                //{
                                //if (selectcount2 == selected)
                                //{
                                while ((nextLine = filePathing.ReadLine()) != "--")
                                {
                                    string[] args = Regex.Split(nextLine, Delimiter);
                                    if (args.Length == 5)
                                    {

                                        args = Regex.Split(nextLine, "[;]");
                                        if (args.Length <= 5)
                                        {
                                            args[0] = args[0].TrimEnd('[');
                                            args[1] = args[1].Trim(MyCharList);
                                            args[2] = args[2].Trim(MyCharList);
                                            args[3] = args[3].Trim(MyCharList);
                                            args[4] = args[4].Trim(MyCharList);
                                            int num1;
                                            bool res = int.TryParse(args[2], out num1);

                                            if (args[2] == "DATE")
                                            {
                                                searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + args[2] + "!;!" + "type1" + "\"";
                                                graphArguments.Add(args[0]);
                                                graphArguments.Add(args[3]);
                                                graphArguments.Add(args[4]);
                                                argumentCount++;
                                            }
                                            else if (stringRegex.IsMatch(args[0]) && args[1] == "" && stringRegex.IsMatch(args[2]))
                                            {
                                                searchWord = searchWord + " \"" + args[0] + ";!;" + " " + ";!;" + args[2] + "!;!" + "type2" + "\"";
                                                graphArguments.Add(args[0]);
                                                graphArguments.Add(args[3]);
                                                graphArguments.Add(args[4]);
                                                argumentCount++;
                                            }
                                            else if (stringRegex.IsMatch(args[0]) && stringRegex.IsMatch(args[1]) && args[2] == "")
                                            {
                                                searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + " " + "!;!" + "type2" + "\"";
                                                graphArguments.Add(args[0]);
                                                graphArguments.Add(args[3]);
                                                graphArguments.Add(args[4]);
                                                argumentCount++;
                                            }
                                            else if (res == false)
                                            {
                                                searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + args[2] + "!;!" + "type4" + "\"";
                                                graphArguments.Add(args[0]);
                                                graphArguments.Add(args[3]);
                                                graphArguments.Add(args[4]);
                                                argumentCount++;
                                            }
                                            else if (res == true)
                                            {
                                                searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + args[2] + "!;!" + "type3" + "\"";
                                                graphArguments.Add(args[0]);
                                                graphArguments.Add(args[3]);
                                                graphArguments.Add(args[4]);
                                                argumentCount++;
                                            }


                                        }

                                    }
                                }
                                //}
                                //}
                                selectcount2++;
                            }
                        }
                    }
                    filePathing.Close();
                }
                Console.WriteLine(graphArguments);
                string searchWordEile = searchWord;

                perlCalled(searchWordEile, graphArguments, argumentCount);

            }
            else if (scriptsToDelete.Count == 0 && startTime != null && endTime != null && outputFileName != null && file_Name != null)
            {
                string perlDateString = startTime + "*!*" + endTime;
                perlCalled(perlDateString, graphArguments, argumentCount);
                startTime = null;
                endTime = null;
            }
        }


        Process perlprocess;
        List<Process> processList = new List<Process>();
        int processCount = 0;
        private void perlCalled(string searchWord2, List<string> graph, int argCount)
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
            processList.Add(perlprocess);
            if (processCount < 2)
            {
                progressBar.Value = 0;
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
                    time = ((fileCheck * 7) * argCount) / 60;
                    timeTwo = ((fileCheck * 10) * argCount) / 60;
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
                        progressBar.Value = 0;
                    }
                }
                else if (filesize < 400000000)
                {
                    fileCheck = ((int)filesize / 100000000);

                    time = (((fileCheck * 7) * argCount) / 60);

                    MessageBox.Show("Estimated Time of completion is less than one minute", "File Size Alert", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    if (time == 0 && fileCheck > 2)
                    {
                        time = 1;
                    }
                    time = time * 60;
                    progressBar.Visibility = Visibility.Visible;
                    Progress_label.Visibility = Visibility.Visible;
                    progressTimer();
                    progressBar.Value = 0;

                }
                perlprocess.Start();
                //Worker workerObject = new Worker();
                processCount++;

                Thread workerThread = new Thread(() => thread(graph));


                workerThread.Start();
            }
            else
            {
                MessageBox.Show("You have too many scripts running, please try again when one has exited", "Script Limit Reached", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        bool perlprocesswaskilled = false;
        private void thread(List<string> graphArg)
        {

            this.Dispatcher.Invoke((Action)(() =>
            {

                // Changes the stop button to Red when the run script button is active

                // BtnCancel.Background = Brushes.OrangeRed;
                // enable the stop button when the run script button is active
                GrepButton.IsEnabled = false;
                BtnCancel.IsEnabled = true;
                BtnCancel.Background = Brushes.Red;
            }));

            //BtnCancel.IsDefault = true;


            perlprocess.WaitForExit();
            string outputPerlFile = perlprocess.StandardOutput.ReadToEnd();



            //processList.Remove(perlprocess);
            processCount--;
            Hex = null;
            Exclude = null;
            if (perlprocesswaskilled == false && processCount == 0)
            {

                this.Dispatcher.Invoke((Action)(() =>
                {
                    GrepButton.IsEnabled = true;
                    BtnCancel.IsEnabled = false;
                    progressBar.Visibility = Visibility.Hidden;
                    Progress_label.Visibility = Visibility.Hidden;
                }));
                if (MessageBox.Show("Log file created.\n\nDo you wish to create a call graph of the output file? ", "Call Graph", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    var json = JsonConvert.SerializeObject(graphArg);
                    List<string> lineParameters = new List<string>();
                    foreach (string options in toCombo.ItemsSource)
                    {
                        lineParameters.Add(options);
                    }

                    var jsonPara = JsonConvert.SerializeObject(lineParameters);
                    using (FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "\\HTMLPage2.htm", FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            w.WriteLine("<!DOCTYPE>");
                            w.WriteLine("<html>");
                            w.WriteLine("<head>");
                            w.WriteLine("<meta http-equiv=\"X-UA-Compatible\" content=\"IE = 9\" />");
                            w.WriteLine("<title>Table Properties</title>");
                            w.WriteLine("<style type=\"text/css\"></style>");
                            w.WriteLine("<link href=\"Stylesheet1.css\" rel=\"stylesheet\"/>");
                            w.WriteLine("<script src=\"JavaScript1.js\"></script>");

                            w.WriteLine("</head>");
                            w.WriteLine("<body>");

                            w.WriteLine("<table id=\"myTable\">");
                            w.WriteLine(" <thead>");
                            w.WriteLine("<tr>");
                            w.WriteLine("<th >Arguments</th>");
                            w.WriteLine("</tr>");

                            w.WriteLine(" </thead>");
                            w.WriteLine("</table>");
                            //w.WriteLine("<style>var city1 = " + json + ";</style>");
                            w.WriteLine("<script>tableCreate(" + json + "," + jsonPara + ");</script>");
                            w.WriteLine("</body>");
                            w.WriteLine("</html>");


                        }
                        StreamReader reader = new StreamReader(outputPerlFile);
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            webBrowser1.Navigate(Directory.GetCurrentDirectory() + "\\HTMLPage2.htm");
                            this.webBrowser1.ObjectForScripting = new ScriptingHelper();
                            Graph_Tab.IsSelected = true;
                            txtBox.Text = reader.ReadToEnd();

                        }));


                    }


                }
                else if (MessageBox.Show("Log file created.\n\nDo you wish to open the output folder? ", "Open New File Location", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Process.Start(outputFileName);
                }
            }
        }


        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

            if (processList.Count <= 0)
            {
                MessageBox.Show("You are currently not running any scripts!", "Stop Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure you wish to stop all scripts?", "Stop Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
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
                    processList.Clear();
                }
            }
        }


        public void textBoxControl()
        {

            this.Title = "gdfgdfg";
        }

        NewScriptWindow scriptWindow = new NewScriptWindow();
        private void ScriptCreationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                scriptWindow.ShowDialog();
                scriptWindow.Activate();
            }
            catch (Exception)
            {
                scriptWindow = new NewScriptWindow();
                scriptWindow.ShowDialog();
                scriptWindow.Activate();
            }
            scriptWindow.Closed += scriptWindow_Closed;
        }

        void scriptWindow_Closed(object sender, EventArgs e)
        {
            listReload();
            string newscript = "";
            ComboBox_Reload();
            foreach (ListViewItems last in ListDataGrid.ItemsSource)
            {
                newscript = last.gridNameColumn;
            }
            ScriptSelectCombo.SelectedValue = null;
            ScriptSelectCombo.SelectedValue = newscript;
        }

        private void DeleteScriptsButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> scriptsToDelete = new List<string>();
            //int selectcount = 0;
            foreach (ListViewItems stuff in ListDataGrid.ItemsSource)
            {
                string scriptName = stuff.gridNameColumn;
                if (stuff.gridCheckboxColumn == true)
                //foreach (int selected in listIntSelection)
                {
                    //if (selectcount == selected)
                    //{
                    scriptsToDelete.Add(scriptName);
                    //}
                }
                //selectcount++;
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
                        //int selectcount2 = 0;
                        foreach (ListViewItems stuff in ListDataGrid.ItemsSource)
                        {
                            string scriptName = stuff.gridNameColumn;
                            if (stuff.gridCheckboxColumn == true)
                            //foreach (int selected in listIntSelection)
                            {
                                //if (selectcount2 == selected)
                                {
                                    deleteScriptsFunction(scriptName);
                                }
                            }
                            try
                            {
                                if (ScriptSelectCombo.SelectedValue.ToString() == scriptName)
                                {
                                    ScriptSelectCombo.SelectedValue = null;
                                    //ScriptSelectCombo.SelectedIndex = 0;
                                }
                            }
                            catch
                            {
                            }
                            //selectcount2++;
                        }
                    }
                    //listIntSelection.Clear();
                }
                else
                {
                    if (MessageBox.Show("Are you sure you wish to delete these scripts?", "Delete Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //int selectcount2 = 0;
                        foreach (ListViewItems stuff in ListDataGrid.ItemsSource)
                        {
                            string scriptName = stuff.gridNameColumn;
                            if (stuff.gridCheckboxColumn == true)
                            //foreach (int selected in listIntSelection)
                            {
                                //if (selectcount2 == selected)
                                // {
                                deleteScriptsFunction(scriptName);
                                try
                                {
                                    if (ScriptSelectCombo.SelectedValue.ToString() == scriptName)
                                    {
                                        ScriptSelectCombo.SelectedValue = null;
                                    }
                                }
                                catch
                                {
                                }
                                //}
                            }
                            //selectcount2++;
                        }
                    }
                    //listIntSelection.Clear();
                }
            }
        }

        private void deleteScriptsFunction(string script)
        {
            Regex regex = new Regex("^name :" + script + "$");
            string nextLine;
            string tempfile = System.IO.Path.GetTempFileName();
            string scriptLine;
            var scriptFileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
            StreamReader scriptReaderForEditing = new StreamReader(scriptFileName);
            StreamWriter tempWriterForEditing = new StreamWriter(tempfile);
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
            listReload();
            File.Delete(tempfile);
        }

        private void listcheckbox_Checked(object sender, RoutedEventArgs e)
        {
            listIntSelection.Add(ListDataGrid.SelectedIndex);
        }

        private void listcheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            listIntSelection.Remove(ListDataGrid.SelectedIndex);
        }

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
            foreach (ListViewItems tocheck in ListDataGrid.ItemsSource)
            {
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
            foreach (ListViewItems checkstuff in listAllChecked)
            {
                ListViewItems alist = new ListViewItems();
                alist.gridNameColumn = checkstuff.gridNameColumn;
                alist.gridCheckboxColumn = checkstuff.gridCheckboxColumn;
                alistoflists.Add(alist);
            }
            ListDataGrid.ItemsSource = alistoflists;
        }

        /*
         * end of bottom left
        */


        /*
         * start of top right
        */

        public class Details
        {
            public bool gridCheckbox { get; set; }
            public string singleString { get; set; }
            public string linesAbove { get; set; }
            public string linesBelow { get; set; }
            public string selectionOne { get; set; }
            public string selectionTwo { get; set; }
        }

        private void EditDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditDataGrid.SelectedItem != null)
            {
                //get data from editdatagrid
                topDateTypeCheckbox.IsChecked = false;
                selectedDetails = (Details)EditDataGrid.SelectedItem;
                startString = selectedDetails.singleString;
                startLinesAbove = selectedDetails.singleString;
                startLinesBelow = selectedDetails.singleString;
                startSelectionOne = selectedDetails.selectionOne;
                startSelectionTwo = selectedDetails.selectionTwo;

                //insert into textboxes
                EditStringTextbox.Text = selectedDetails.singleString;
                EditTopMarkerTextbox.Text = selectedDetails.linesAbove.ToString();
                EditBottomMarkerTextbox.Text = selectedDetails.linesBelow.ToString();
                fromCombo.Text = selectedDetails.selectionOne.ToString();
                toCombo.Text = selectedDetails.selectionTwo.ToString();
                EditStringTextbox.IsEnabled = true;
                EditTopMarkerTextbox.IsEnabled = true;
                EditBottomMarkerTextbox.IsEnabled = true;
                topDateTypeCheckbox.IsEnabled = true;
                if (EditTopMarkerTextbox.Text == "DATE")
                {
                    topDateTypeCheckbox.IsChecked = true;
                }
                else
                {
                    topDateTypeCheckbox.IsChecked = false;
                }
            }
        }

        private void DataReload(Regex regex)
        {
            List<Details> datalist = new List<Details>();
            EditDataGrid.IsReadOnly = false;
            //char[] mycharlist = { ']', '[' };
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
                        string to;
                        string from;
                        int count = 0;
                        Details dataForGrid = new Details();
                        dataForGrid.singleString = "Add New Line";
                        dataForGrid.linesAbove = "";
                        dataForGrid.linesBelow = "";
                        dataForGrid.selectionOne = "";
                        dataForGrid.selectionTwo = "";
                        datalist.Add(dataForGrid);
                        while ((nextLine = scriptReader.ReadLine()) != "--")
                        {
                            count++;
                            string[] argsvalue = Regex.Split(nextLine, @"\[;\]");
                            try
                            {
                                aboveMarker = argsvalue[1];
                            }
                            catch
                            {
                                aboveMarker = "";
                            }

                            try
                            {
                                belowMarker = argsvalue[2];
                            }
                            catch
                            {
                                belowMarker = "";

                            }
                            try
                            {
                                from = argsvalue[3];
                            }
                            catch
                            {
                                from = "";
                            }
                            try
                            {
                                to = argsvalue[4];
                            }
                            catch
                            {
                                to = "";
                            }
                            Details getDataForGrid = new Details();
                            getDataForGrid.singleString = argsvalue[0];
                            getDataForGrid.linesAbove = aboveMarker;
                            getDataForGrid.linesBelow = belowMarker;
                            getDataForGrid.selectionOne = from;
                            getDataForGrid.selectionTwo = to;
                            datalist.Add(getDataForGrid);
                        }
                        InitializeComponent();
                        EditDataGrid.ItemsSource = datalist;
                        EditDataGrid.Columns[1].Header = "String";
                        EditDataGrid.Columns[2].Header = "Top Marker";
                        EditDataGrid.Columns[3].Header = "Bottom Marker";
                        EditDataGrid.Columns[4].Header = "From";
                        EditDataGrid.Columns[5].Header = "To";
                        EditDataGrid.Columns[1].IsReadOnly = true;
                        EditDataGrid.Columns[2].IsReadOnly = true;
                        EditDataGrid.Columns[3].IsReadOnly = true;
                        EditDataGrid.Columns[4].IsReadOnly = true;
                        EditDataGrid.Columns[5].IsReadOnly = true;
                        EditDataGrid.Columns[0].Width = 24;
                    }
                }
            }
            scriptReader.Close();
            EditDataGrid.SelectedIndex = 0;
        }

        private void deleteMultipleLinesButton_Click(object sender, RoutedEventArgs e)
        {
            List<Details> deleteLinesList;
            deleteLinesList = null;
            if (EditDataGrid.Items.Count == 0)
            {
                MessageBox.Show("No Script Selected.", "Deletion Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                deleteLinesList = new List<Details>();
                EditDataGrid.ItemsSource.GetEnumerator();
                int selectcount = 0;
                foreach (Details stuff in EditDataGrid.ItemsSource)
                {
                    string stringCheck = stuff.singleString;
                    if (stringCheck != "Add New Line")
                    {
                        if (stuff.gridCheckbox == true)
                        //foreach (int selected in dataIntSelection)
                        {
                            //if (selectcount == selected)
                            //{
                            //    {
                            deleteLinesList.Add(stuff);
                            //    }
                            //}
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
                            //dataIntSelection.Clear();
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
                            //dataIntSelection.Clear();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Lines Selected", "Deletion Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }

        private void deleteMultipleLines(string string2Delete)
        {
            string nextLine;
            string tempfile = System.IO.Path.GetTempFileName();
            Regex regex = new Regex(selectedValue);
            string scriptLine;
            var scriptFileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
            StreamReader scriptReaderForEditing = new StreamReader(scriptFileName);
            StreamWriter tempWriterForEditing = new StreamWriter(tempfile);
            while ((scriptLine = scriptReaderForEditing.ReadLine()) != null)
            {
                if ("name :" + selectedValue == (scriptLine))
                {
                    tempWriterForEditing.WriteLine(scriptLine);

                    while ((nextLine = scriptReaderForEditing.ReadLine()) != "--")
                    {
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

        private void copyMultipleLinesButton_Click(object sender, RoutedEventArgs e)
        {
            //int selectcount = 0;
            copyLinesList = new List<Details>();

            if (EditDataGrid.Items.Count == 0)
            {
                MessageBox.Show("No Script Selected.", "Copy Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            int countselected = 0;
            foreach (Details checkcheck in EditDataGrid.ItemsSource)
            {
                string stringCheck = checkcheck.singleString;
                if (checkcheck.gridCheckbox == true)
                {
                    if (stringCheck != "Add New Line")
                    {
                        countselected++;
                    }
                }
            }

            if (countselected == 0)
            {
                MessageBox.Show("No Lines Selected.", "Copy Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            foreach (Details checkcheck in EditDataGrid.ItemsSource)
            {
                string stringCheck = checkcheck.singleString;
                if (checkcheck.gridCheckbox == true)
                {
                    if (stringCheck != "Add New Line")
                    {
                        copyLinesList.Add(checkcheck);
                    }
                }
            }
            pasteButton.IsEnabled = IsEnabled;


            //if (EditDataGrid.Items.Count == 0)
            //{
            //    MessageBox.Show("No Script Selected.", "Copy Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //}
            //else
            //{
            //    copyLinesList = new List<Details>();
            //    foreach (Details stuff in EditDataGrid.ItemsSource)
            //    {
            //        string stringCheck = stuff.singleString;
            //        foreach (int selected in dataIntSelection)
            //        {
            //            if (selectcount == selected)
            //            {
            //                if (stringCheck != "Add New Line")
            //                {
            //                    copyLinesList.Add(stuff);
            //                }
            //            }
            //            else
            //            {
            //            }
            //        }
            //        selectcount++;
            //    }
            //    if (copyLinesList.Count != 0)
            //    {
            //        pasteButton.IsEnabled = IsEnabled;
            //    }
            //    else
            //    {
            //        MessageBox.Show("No Lines Selected.", "Copy Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //    }
            // }
            //dataIntSelection.Clear();
        }

        private void pasteButton_Click(object sender, RoutedEventArgs e)
        {
            datacheckboxheader.IsChecked = false;

            List<Details> donotreplace = new List<Details>();
            List<Details> replace = new List<Details>();
            foreach (Details linesInScript in EditDataGrid.ItemsSource)
            {
                foreach (Details copied in copyLinesList)
                {
                    if (copied.singleString == linesInScript.singleString)
                    {
                        string currentCopy = copied.singleString;
                        if (MessageBox.Show("You already have a match for " + currentCopy + ", do you want to replace it?", "Copy Conflict", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
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
            while ((scriptLine = pasteReader.ReadLine()) != null)
            {
                if (regex.IsMatch(scriptLine))
                {
                    pasteWriter.WriteLine(scriptLine);
                    foreach (Details stuff in copyLinesList)
                    {
                        pasteWriter.WriteLine("{2}{5}{0}{5}{1}{5}{3}{5}{4}", stuff.linesAbove, stuff.linesBelow, stuff.singleString, stuff.selectionOne, stuff.selectionTwo, Delimiter);
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
            string newscript = editScriptNameTextbox.Text;
            ScriptSelectCombo.SelectedValue = newscript;
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
                        if (replace.singleString == mainString)
                        {
                            pasteWriter2.WriteLine("{2}{5}{0}{5}{1}{5}{3}{5}{4}", replace.linesAbove, replace.linesBelow, replace.singleString, replace.selectionOne, replace.selectionTwo, Delimiter);
                        }
                        else
                        {
                            pasteWriter2.WriteLine(nextLine2);
                        }
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
            pasteReader2 = new StreamReader(tempfile2);
            pasteWriter2 = new StreamWriter(scriptFileName2);
            string aline2;
            while ((aline2 = pasteReader2.ReadLine()) != null)
            {
                pasteWriter2.WriteLine(aline2);
            }
            pasteWriter2.Close();
            pasteReader2.Close();
            Regex regex = new Regex(selectedValue);
            DataReload(regex);
        }

        private void datagridcheckbox_Checked(object sender, RoutedEventArgs e)
        {
            //dataIntSelection.Add(EditDataGrid.SelectedIndex);
        }

        private void datagridcheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            //dataIntSelection.Remove(EditDataGrid.SelectedIndex);
        }

        private void datacheckboxheader_Checked(object sender, RoutedEventArgs e)
        {
            //dataIntSelection.Clear();
            int count = 1;
            gridCheckAllBox.IsChecked = true;
            dataAllChecked.Clear();
            foreach (Details tocheck in EditDataGrid.ItemsSource)
            {
                Details list = new Details();
                tocheck.gridCheckbox = true;
                list.gridCheckbox = tocheck.gridCheckbox;
                list.singleString = tocheck.singleString;
                list.linesAbove = tocheck.linesAbove;
                list.linesBelow = tocheck.linesBelow;
                list.selectionOne = tocheck.selectionOne;
                list.selectionTwo = tocheck.selectionTwo;
                dataAllChecked.Add(list);
                //dataIntSelection.Add(count);
                count++;
            }
            allCheckedInData();
        }

        private void datacheckboxheader_Unchecked(object sender, RoutedEventArgs e)
        {
            //dataIntSelection.Clear();
            int count = 1;
            gridCheckAllBox.IsChecked = false;
            dataAllChecked.Clear();
            try
            {
                foreach (Details tocheck in EditDataGrid.ItemsSource)
                {
                    Details list = new Details();
                    tocheck.gridCheckbox = false;
                    list.gridCheckbox = tocheck.gridCheckbox;
                    list.singleString = tocheck.singleString;
                    list.linesAbove = tocheck.linesAbove;
                    list.linesBelow = tocheck.linesBelow;
                    list.selectionOne = tocheck.selectionOne;
                    list.selectionTwo = tocheck.selectionTwo;
                    dataAllChecked.Add(list);
                    //dataIntSelection.Remove(count);
                    count++;
                }
                allCheckedInData();
            }
            catch
            {
            }
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
                alist.selectionOne = checkstuff.selectionOne;
                alist.selectionTwo = checkstuff.selectionTwo;
                alistoflists.Add(alist);
            }

            EditDataGrid.ItemsSource = alistoflists;
        }

        /*
         * end of top right
         */

        /*
         * start of bottom right
        */
        private void ScriptSelectCombo_Loaded(object sender, RoutedEventArgs e)
        {
            var fileName = Directory.GetCurrentDirectory() + "\\ListViewScriptsTwo.txt";
            splitName = System.IO.Path.GetFileName(fileName);
            ComboBox_Reload();
        }

        private void ComboBox_Reload()
        {
            string[] line_words;
            clearEditBoxes();
            data.Clear();
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
            ScriptSelectCombo.ItemsSource = null;
            ScriptSelectCombo.ItemsSource = data;
        }

        private void ScriptSelectCombo_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //ScriptSelectCombo.SelectedValue = null;
        }

        private void ScriptSelectCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ScriptSelectCombo.SelectedItem != null)
            {
                string file_Line;
                EditDataGrid.Columns[0].Visibility = Visibility.Visible;
                EditDataGrid.Columns[1].Visibility = Visibility.Visible;
                EditDataGrid.Columns[2].Visibility = Visibility.Visible;
                EditDataGrid.Columns[3].Visibility = Visibility.Visible;
                EditDataGrid.Columns[4].Visibility = Visibility.Visible;
                EditDataGrid.Columns[5].Visibility = Visibility.Visible;
                datacheckboxheader.IsChecked = false;
                StreamReader file_path = new StreamReader(splitName);
                Regex regexx = new Regex("name :");
                editScriptNameTextbox.Text = ScriptSelectCombo.SelectedItem.ToString();
                while ((file_Line = file_path.ReadLine()) != null)
                {
                    if (regexx.IsMatch(file_Line))
                    {
                        word_GrepLine = file_Line.Split(':');
                    }
                }
                selectedValue = ScriptSelectCombo.SelectedItem.ToString();
                Regex regex = new Regex("name :" + selectedValue + "$");
                DataReload(regex);
                SaveLineEditButton.IsEnabled = false;
                file_path.Close();
                editScriptNameTextbox.IsEnabled = true;
            }
            else
            {
                EditDataGrid.ItemsSource = null;
                EditDataGrid.Columns[0].Visibility = Visibility.Hidden;
                EditDataGrid.Columns[1].Visibility = Visibility.Hidden;
                EditDataGrid.Columns[2].Visibility = Visibility.Hidden;
                EditDataGrid.Columns[3].Visibility = Visibility.Hidden;
                EditDataGrid.Columns[4].Visibility = Visibility.Hidden;
                EditDataGrid.Columns[5].Visibility = Visibility.Hidden;

                editScriptNameTextbox.Text = null;
                EditStringTextbox.Text = null;
                EditTopMarkerTextbox.Text = null;
                EditBottomMarkerTextbox.Text = null;
                fromCombo.SelectedValue = null;
                toCombo.SelectedValue = null;
            }
        }

        private void editScriptNameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveLineEditButton.IsEnabled = true;
        }

        private void EditStringTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveLineEditButton.IsEnabled = true;
        }

        private void EditTopMarkerTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveLineEditButton.IsEnabled = true;
        }

        private void EditBottomMarkerTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveLineEditButton.IsEnabled = true;
        }

        private void fromCombo_Loaded(object sender, RoutedEventArgs e)
        {
            fromcomboReload();
        }

        private void fromcomboReload()
        {
            List<string> fromList = new List<string>();
            string line;
            StreamReader fromReader = new StreamReader(Directory.GetCurrentDirectory() + "\\graphOptions.txt");
            while ((line = fromReader.ReadLine()) != null)
            {
                fromList.Add(line);
            }
            fromCombo.ItemsSource = fromList;
            fromReader.Close();
        }

        private void fromCombo_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            fromCombo.SelectedValue = null;
        }

        private void toCombo_Loaded(object sender, RoutedEventArgs e)
        {
            toComboReload();
        }

        private void toComboReload()
        {
            List<string> toList = new List<string>();
            string line;
            StreamReader toReader = new StreamReader(Directory.GetCurrentDirectory() + "\\graphOptions.txt");
            while ((line = toReader.ReadLine()) != null)
            {
                toList.Add(line);
            }
            toCombo.ItemsSource = toList;
            toReader.Close();
        }

        private void toCombo_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            toCombo.SelectedValue = null;
        }

        private void clearEditBoxes()
        {
            editScriptNameTextbox.Clear();
            EditStringTextbox.Clear();
            EditTopMarkerTextbox.Clear();
            EditBottomMarkerTextbox.Clear();
        }

        private void SaveLineEditButton_Click(object sender, RoutedEventArgs e)
        {
            string editedString = EditStringTextbox.Text.TrimEnd().TrimStart();
            string editedLinesAbove = EditTopMarkerTextbox.Text;
            string editedLinesBelow = EditBottomMarkerTextbox.Text;
            string editedFromString = fromCombo.Text;
            string editedToString = toCombo.Text;
            string startScript = ScriptSelectCombo.SelectedValue.ToString();
            string editedScript = editScriptNameTextbox.Text;
            string testnew = "Add New Line";
            //if (editedString == testnew)
            //{
            //    editedString = null;
            //}
            int inttemp;
            if (ScriptSelectCombo.SelectedItem == null)
            {
                return;
            }

            List<string> stringList = new List<string>();
            List<string> scriptList = new List<string>();
            foreach (Details fromGrid in EditDataGrid.ItemsSource)
            {
                string mainstring = fromGrid.singleString;
                stringList.Add(mainstring);
            }

            foreach (ListViewItems fromList in ListDataGrid.ItemsSource)
            {
                string script = fromList.gridNameColumn;
                scriptList.Add(script);
            }

            stringList.Remove(startString);
            scriptList.Remove(startScript);
            //removing the starting selections from stringlist and scriptlist

            startString = selectedDetails.singleString;
            startLinesAbove = selectedDetails.linesAbove;
            startLinesBelow = selectedDetails.linesBelow;
            startSelectionOne = selectedDetails.selectionOne;
            startSelectionTwo = selectedDetails.selectionTwo;

            if ((startScript != editedScript) || (startString != editedString) || (startLinesAbove != editedLinesAbove) ||
                (startLinesBelow != editedLinesBelow) || (startSelectionOne != editedFromString) || (startSelectionTwo != editedToString))
            {
                //if any changes made
                if ((!string.IsNullOrWhiteSpace(editedScript)) && (!scriptList.Contains(editedScript)))
                {
                    //if edited script is not empty or not in scriptlist
                    if ((!string.IsNullOrWhiteSpace(editedString)) && (!stringList.Contains(editedString)))
                    {
                        //if edited string is not bland or not in stringlist
                        if (((string.IsNullOrWhiteSpace(editedFromString)) && (string.IsNullOrWhiteSpace(editedToString)))
                            || ((!string.IsNullOrWhiteSpace(editedFromString)) && (!string.IsNullOrWhiteSpace(editedToString))))
                        //if from and to are both not empty or from and to are both empty
                        {
                            //if from and to are both null or from and to are both not null
                            if (((int.TryParse(editedLinesAbove, out inttemp)) && ((int.TryParse(editedLinesBelow, out inttemp)) || (string.IsNullOrWhiteSpace(editedLinesBelow))))
                                //if top marker is int and (bottom marker is int or null) 
                                || ((int.TryParse(editedLinesBelow, out inttemp)) && ((int.TryParse(editedLinesAbove, out inttemp)) || (string.IsNullOrWhiteSpace(editedLinesAbove))))
                                //if bottom marker is int and (top marker is int or null)
                                || (((!int.TryParse(editedLinesAbove, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesAbove)))
                                //if top marker is not int or empty ie it's a string
                                && (((!int.TryParse(editedLinesBelow, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesBelow))) || (string.IsNullOrWhiteSpace(editedLinesBelow))))
                                //and bottom marker is not int or empty ie it's a string or null
                                || ((!int.TryParse(editedLinesBelow, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesBelow))
                                //or bottom marker is not int or empty ie it's a string
                                && (((!int.TryParse(editedLinesAbove, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesAbove))) || (string.IsNullOrWhiteSpace(editedLinesAbove))))
                                //and top marker is not int or empty ie it's a string or null
                                || ((editedLinesAbove == "DATE") && (editedLinesBelow == "DATE"))
                                //or both are dates
                                || ((string.IsNullOrWhiteSpace(editedLinesAbove)) && (string.IsNullOrWhiteSpace(editedLinesBelow))))
                            //or both are null
                            {
                                if (((!string.IsNullOrWhiteSpace(editedLinesAbove)) && (!int.TryParse(editedLinesAbove, out inttemp))) && (string.IsNullOrWhiteSpace(editedLinesBelow)))
                                {
                                    //if above is not empty and not an int ie a string, and below is empty
                                    editedLinesBelow = editedString;
                                    editedString = editedLinesAbove;
                                    editedLinesAbove = "";

                                }
                                else if (((!string.IsNullOrWhiteSpace(editedLinesBelow)) && (!int.TryParse(editedLinesBelow, out inttemp))) && (string.IsNullOrWhiteSpace(editedLinesAbove)))
                                {
                                    editedLinesAbove = "";
                                }
                                else if ((int.TryParse(editedLinesAbove, out inttemp)) && (string.IsNullOrWhiteSpace(editedLinesBelow)))
                                {
                                    editedLinesBelow = "0";
                                }
                                else if ((int.TryParse(editedLinesBelow, out inttemp)) && (string.IsNullOrWhiteSpace(editedLinesAbove)))
                                {
                                    editedLinesAbove = "0";
                                }
                                else if ((string.IsNullOrWhiteSpace(editedLinesBelow)) && (string.IsNullOrWhiteSpace(editedLinesAbove)))
                                {
                                    editedLinesAbove = "0";
                                    editedLinesBelow = "0";
                                }

                                saveChangesToLines(testnew, editedScript, editedLinesBelow, editedLinesAbove, editedString, editedToString, editedFromString);

                            }
                            else
                            {

                                MessageBox.Show("Top Marker and Bottom Marker must be of same type.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("From and To selections must be both selected or both left blank.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Main String, textbox is empty or matches an existing line in this script.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                    }
                }
                else
                {
                    MessageBox.Show("Invalid Script Name, textbox is empty or matches an existing Script.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No changes to save.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

            }




            //if ((string.IsNullOrWhiteSpace(editedString)) && (editScriptNameTextbox.Text == ScriptSelectCombo.SelectedValue.ToString()))
            //{
            //    //main string is empty or says Add New Line
            //    MessageBox.Show("You must enter a main string in the text area.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //}
            //else if (string.IsNullOrWhiteSpace(editScriptNameTextbox.Text))
            //{
            //    //script name is empty
            //    MessageBox.Show("You must enter a valid script name", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //}
            //else if ((int.TryParse(editedString, out inttemp)))
            //{
            //    //main string is an int
            //    MessageBox.Show("You must enter a main string in the text area.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //}
            //else if ((int.TryParse(editedLinesAbove, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesBelow)) && (!int.TryParse(editedLinesBelow, out inttemp)))
            //{
            //    //linesabove is int and linesbelow is not empty and not an int
            //    MessageBox.Show("Please enter a string with a string or an int with an int", "Invalid Input", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //}
            //else if ((int.TryParse(editedLinesBelow, out inttemp)) && (!int.TryParse(editedLinesAbove, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesAbove)))
            //{
            //    //linesbelow is int and linesabove is not empty but not an int
            //    MessageBox.Show("Please enter a string with a string or an int with an int", "Invalid Input", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //}
            //else if (((string.IsNullOrWhiteSpace(editedFromString)) && (editedToString != null)) || ((string.IsNullOrWhiteSpace(editedToString)) && (editedFromString != null)))
            //{
            //    //to is empty and from is selected or from is empty and to is selected
            //    MessageBox.Show("Only one message direction has been selected. Please select another or leave both empty.", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //}
            //else if ((int.TryParse(editedLinesAbove, out inttemp)) && (string.IsNullOrWhiteSpace(editedLinesBelow)))
            //{
            //    //linesabove is int and linesbelow is empty
            //    editedLinesBelow = "0";
            //    saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString, editedToString, editedFromString);
            //}
            //else if ((int.TryParse(editedLinesBelow, out inttemp)) && (string.IsNullOrWhiteSpace(editedLinesAbove)))
            //{
            //    //linesbelow is int and linesabove is empty
            //    editedLinesAbove = "0";
            //    saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString, editedToString, editedFromString);
            //}
            //else if ((!int.TryParse(editedLinesBelow, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesBelow)) && (string.IsNullOrWhiteSpace(editedLinesAbove)))
            //{
            //    //linesbelow not int and not empty and linesabove is empty
            //    editedLinesAbove = "";
            //    saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString, editedToString, editedFromString);
            //}
            //else if ((!int.TryParse(editedLinesAbove, out inttemp)) && (!string.IsNullOrWhiteSpace(editedLinesAbove)) && (string.IsNullOrWhiteSpace(editedLinesBelow)))
            //{
            //    //linesabove not int and not empty and linesbelow is empty
            //    //must set linesbelow to editedstring, editedstring to linesabove and linesabove to empty
            //    editedLinesBelow = "";
            //    saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString, editedToString, editedFromString);
            //}
            //else if ((string.IsNullOrWhiteSpace(editedLinesAbove)) && (string.IsNullOrWhiteSpace(editedLinesBelow)))
            //{
            //    //if both lineabove and linesbelow are empty, enter 0
            //    editedLinesAbove = "0";
            //    editedLinesBelow = "0";
            //    saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString, editedToString, editedFromString);
            //}
            //else
            //{
            //    //save changes to line
            //    saveChangesToLines(testnew, editedLinesBelow, editedLinesAbove, editedString, editedToString, editedFromString);
            //}
        }

        private void saveChangesToLines(string testnew, string editedScript, string editedLinesBelow, string editedLinesAbove, string editedString, string editedToString, string editedFromString)
        {
            {
                //string newscript = editScriptNameTextbox.Text;
                bool exists = false;
                foreach (Details stuff in EditDataGrid.ItemsSource)
                {
                    string mainString = stuff.singleString;
                    string above = stuff.linesAbove;
                    string below = stuff.linesBelow;
                    string to = stuff.selectionTwo;
                    string from = stuff.selectionOne;
                    if (("^" + EditStringTextbox.Text + "$" == "^" + mainString + "$") && ("^" + above + "$" == "^" + editedLinesAbove + "$") && ("^" + below + "$" == "^" + editedLinesBelow + "$") && ("^" + editScriptNameTextbox.Text + "$" == "^" + ScriptSelectCombo.SelectedValue + "$") && ("^" + to + "$" == "^" + toCombo.SelectedValue + "$") && ("^" + from + "$" == "^" + fromCombo.SelectedValue + "$"))
                    {
                        //if all entries match another line
                        exists = true;
                    }
                    else if (("^" + EditStringTextbox.Text + "$" != "^" + startString + "$") && (mainString == EditStringTextbox.Text))
                    {
                        //if startstring is different to new line and mainstring (string from foreach statement) equals the new line
                        //ie not same as started but doesn't match anything else in the list
                        //startstring is created when the textboxes are populated, it is the original string, mainstring is from the gridbox
                        exists = true;
                    }
                }
                bool scriptexists = false;
                foreach (ListViewItems stuff in ListDataGrid.ItemsSource)
                {
                    string scriptName = stuff.gridNameColumn;
                    if (scriptName == editScriptNameTextbox.Text.TrimEnd().TrimStart())
                    {
                        if (ScriptSelectCombo.SelectedValue.ToString() != scriptName)
                        {
                            scriptexists = true;
                        }
                    }
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
                            if (editScriptNameTextbox.Text != "")
                            {
                                if (regex.IsMatch("name :" + editScriptNameTextbox.Text))
                                {
                                    tempWriterForEditing.WriteLine(scriptLine);
                                }
                                else
                                {
                                    tempWriterForEditing.WriteLine("name :" + editScriptNameTextbox.Text);
                                }
                            }
                            if (EditStringTextbox.Text != "" && EditStringTextbox.Text != "Add New Line")
                            {
                                while ((nextLine = scriptReaderForEditing.ReadLine()) != "--")
                                {
                                    string[] nextString = nextLine.Split('[');
                                    string nextStr = "^" + nextString[0] + "$";
                                    //match line to be changed
                                    if ("^" + startString + "$" == nextStr)
                                    {
                                        if (editedLinesBelow == "")
                                        {
                                            tempWriterForEditing.WriteLine("{1}{5}{0}{5}{2}{5}{3}{5}{4}", editedLinesAbove, editedString, "", editedFromString, editedToString, Delimiter);
                                        }
                                        else
                                        {
                                            tempWriterForEditing.WriteLine("{2}{5}{0}{5}{1}{5}{3}{5}{4}", editedLinesAbove, editedLinesBelow, editedString, editedFromString, editedToString, Delimiter);
                                        }
                                    }

                                    else
                                    {
                                        tempWriterForEditing.WriteLine(nextLine);
                                    }
                                }
                                if (startString == testnew)
                                {
                                    if (editedLinesBelow == "")
                                    {
                                        tempWriterForEditing.WriteLine("{1}{5}{0}{5}{2}{5}{3}{5}{4}", editedLinesAbove, editedString, "", editedFromString, editedToString, Delimiter);
                                    }
                                    else
                                    {
                                        tempWriterForEditing.WriteLine("{2}{5}{0}{5}{1}{5}{3}{5}{4}", editedLinesAbove, editedLinesBelow, editedString, editedFromString, editedToString, Delimiter);
                                    }
                                }
                                tempWriterForEditing.WriteLine("--");
                            }
                            else
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
                    File.Delete(tempfile);
                    clearEditBoxes();
                    topDateTypeCheckbox.IsChecked = false;
                    SaveLineEditButton.IsEnabled = false;

                    DataReload(regex);
                    listReload();
                    ScriptSelectCombo.ItemsSource = null;
                    ComboBox_Reload();
                    ScriptSelectCombo.SelectedValue = editedScript;
                    editScriptNameTextbox.Text = editedScript;
                }
            }
            listIntSelection.Clear();

        }

        private void topDateTypeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            EditTopMarkerTextbox.Text = "DATE";
            EditBottomMarkerTextbox.Text = "DATE";
            EditTopMarkerTextbox.IsEnabled = false;
            EditBottomMarkerTextbox.IsEnabled = false;
        }

        private void topDateTypeCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            EditTopMarkerTextbox.Clear();
            EditBottomMarkerTextbox.Clear();
            EditTopMarkerTextbox.IsEnabled = true;
            EditBottomMarkerTextbox.IsEnabled = true;
        }

        private void EditGraphOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            EditForDiagramWindow diagramEditingWindow = new EditForDiagramWindow();

            try
            {
                diagramEditingWindow.ShowDialog();
                diagramEditingWindow.Activate();
            }
            catch (Exception)
            {
                diagramEditingWindow = new EditForDiagramWindow();
                diagramEditingWindow.ShowDialog();
                diagramEditingWindow.Activate();
            }
            diagramEditingWindow.Closed += diagramEditingWindow_Closed;
        }

        void diagramEditingWindow_Closed(object sender, EventArgs e)
        {
            toComboReload();
            fromcomboReload();
        }

        /*
         * end of bottom right
        */

        /*
         * start of menu etc
        */

        // Exit Button closes the application
        private void MenuItem_exitBtn(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        // Help menu button
        private void MenuItem_HelpButton(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:/Program Files (x86)/SolarWinds/SolarWindsEventLogConsolidator/EventLogConsolidator.htm");
        }

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

        //shane needs this!!!
        // About menu button
        //private void MenuItem_AboutButton(object sender, RoutedEventArgs e)
        //{
        //    System.Diagnostics.Process.Start(@"C:/Program Files (x86)/SolarWinds/SolarWindsEventLogConsolidator/EventLogConsolidator.htm");
        //}

        private void progressTimer()
        {
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
        /*
         * end of menu etc
        */

        [ComVisible(true)]
        public class ScriptingHelper
        {

            public void ShowMessage(string message)
            {
                MessageBox.Show(message);

            }

        }


        private void CopyToNewScript_Click(object sender, RoutedEventArgs e)
        {

            scriptcount = 0;
            linelist = new List<string>();
            List<string> copylist = new List<string>();

            foreach (ListViewItems stuff in ListDataGrid.ItemsSource)
            {
                scriptcount++;
                string scriptName = stuff.gridNameColumn;
                if (stuff.gridCheckboxColumn == true)
                {
                    copylist.Add(scriptName);
                }
            }

            foreach (string items in copylist)
            {
                string nextLine = "";
                string line;
                var fileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
                StreamReader filePathing = new StreamReader(fileName);
                while ((line = filePathing.ReadLine()) != null)
                {
                    {
                        Regex regex = new Regex("name :" + items);
                        if (regex.IsMatch(line))
                        {
                            while ((nextLine = filePathing.ReadLine()) != "--")
                            {
                                linelist.Add(nextLine);
                            }
                        }
                    }
                }
                filePathing.Close();
            }
            CreateNewScript();
        }

        private void CreateNewScript()
        {
            try
            {
                //scriptWindow.Show();
                scriptWindow.Topmost = true;
                scriptWindow.ShowDialog();
                scriptWindow.Activate();
                if (scriptWindow.DialogResult != null)
                {
                    scriptWindow_Closed();
                }
            }
            catch (Exception)
            {
                scriptWindow = new NewScriptWindow();
                //scriptWindow.Show();
                scriptWindow.Topmost = true;
                scriptWindow.ShowDialog();
                scriptWindow.Activate();
                if (scriptWindow.DialogResult != null)
                {
                    scriptWindow_Closed();
                }
            }
            //scriptWindow.Closed += scriptWindow_Closed;
        }


        void scriptWindow_Closed()
        {
            listReload();
            string newscript = "";
            ComboBox_Reload();
            foreach (ListViewItems last in ListDataGrid.ItemsSource)
            {
                newscript = last.gridNameColumn;
            }
            ScriptSelectCombo.SelectedValue = null;
            ScriptSelectCombo.SelectedValue = newscript;

            if (linelist != null)
            {
                copyWholeScripts();
            }
        }

        private void copyWholeScripts()
        {
            var scriptFileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
            StreamReader pasteReader = new StreamReader(scriptFileName);
            string tempfile = System.IO.Path.GetTempFileName();
            StreamWriter pasteWriter = new StreamWriter(tempfile);
            string scriptLine;
            if (scriptcount < ScriptSelectCombo.Items.Count)
            {
                List<string> totalduplicates = new List<string>();
                List<string> duplicates = new List<string>();
                foreach (string duplicatecheck in linelist)
                {
                    string[] duplicateOne = duplicatecheck.Split(new[] { Delimiter }, StringSplitOptions.None);
                    foreach (string duplicatecheck2 in linelist)
                    {

                        string[] duplicateTwo = duplicatecheck2.Split(new[] { Delimiter }, StringSplitOptions.None);
                        if (duplicatecheck == duplicatecheck2)
                        {
                            if (!totalduplicates.Contains(duplicatecheck))
                            {
                                totalduplicates.Add(duplicatecheck);
                            }
                        }
                        else if (duplicateOne[0] == duplicateTwo[0])
                        {
                            if (!duplicates.Contains(duplicatecheck))
                            {
                                duplicates.Add(duplicatecheck);
                            }
                            if (!duplicates.Contains(duplicatecheck2))
                            {
                                duplicates.Add(duplicatecheck2);
                            }
                        }
                    }
                }
                foreach (string dups in duplicates)
                {
                    totalduplicates.Remove(dups);
                }
                if (duplicates.Count > 0)
                {
                    List<string> dupdone = new List<string>();
                    List<List<string>> eachdup = new List<List<string>>();
                    foreach (string dups in duplicates)
                    {
                        List<string> dup = new List<string>();
                        string[] dupsSplit = dups.Split(new[] { Delimiter }, StringSplitOptions.None);
                        if ((!dup.Contains(dups)) && (!dupdone.Contains(dups)))
                        {
                            dup.Add(dups);
                            dupdone.Add(dups);
                            foreach (string dups2 in duplicates)
                            {
                                string[] dupsSplit2 = dups2.Split(new[] { Delimiter }, StringSplitOptions.None);
                                if ((dupsSplit[0] == dupsSplit2[0]) && (dups != dups2))
                                {
                                    dup.Add(dups2);
                                    dupdone.Add(dups2);
                                }
                            }
                            eachdup.Add(dup);
                        }
                    }
                    while ((scriptLine = pasteReader.ReadLine()) != null)
                    {
                        pasteWriter.WriteLine(scriptLine);
                        if (scriptLine == "name :" + ScriptSelectCombo.SelectedItem)
                        {
                            foreach (string copying in totalduplicates)
                            {
                                pasteWriter.WriteLine(copying);
                            }
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
                    pasteReader.Close();
                    pasteWriter.Close();
                    foreach (List<string> dupdup in eachdup)
                    {
                        DuplicateOptionsWindow dupWin = new DuplicateOptionsWindow(dupdup, ScriptSelectCombo.SelectedItem.ToString());
                        dupWin.ShowDialog();
                        //dupWin.Show();
                        if (dupWin.DialogResult != null)
                        {
                            Regex regex = new Regex(ScriptSelectCombo.SelectedItem.ToString());
                            DataReload(regex);
                            Console.WriteLine("someotherstuff");
                        }
                        //dupWin.Closed += dupWin_Closed;
                    }
                }
            }
        }


        //private void ListDataGrid_KeyDown(object sender, KeyEventArgs e)
        //{
        //    Console.WriteLine("keypressed");
        //    if (e.Key == Key.Delete)
        //    {
        //        Console.WriteLine("deletekey");

        //        e.Handled = true;
        //    }
        //}

        private void ListDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Console.WriteLine("deletekey");

                e.Handled = true;
            }
        }

        private void EditDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Console.WriteLine("deletekey");

                e.Handled = true;
            }
        }

    }
}
