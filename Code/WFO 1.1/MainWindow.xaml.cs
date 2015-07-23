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


/// <summary>
/// The ACR Project namespace.
/// </summary>
namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Global variables
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The delimiter - used to split items in a line within a Group
        /// </summary>
        string DelimiterRead = @"\[;\]";
        /// <summary>
        /// The delimiter - used to join items in a line within a Group
        /// </summary>
        string DelimiterWrite = "[;]";
        /// <summary>
        /// The selected value in Groupselectcombo - ie the current selected Group
        /// </summary>
        string selectedValue;
        /// <summary>
        /// The split name assigned in Groupselectcombo_selectionchanged, this contains the file that contains all the scripts
        /// </summary>
        string categoryFile = System.IO.Path.GetFileName("ListViewScriptsTwo.txt");
        /// <summary>
        /// The output file name, assigned in chooseoutputfolderbutton_click, this contains the folder to save to ! should also be used in select file if created there automatically
        /// </summary>
        string outputFileName;
        /// <summary>
        /// The file_ name is the entire address of selected file
        /// </summary>
        string file_Name;
        /// <summary>
        /// The start time - received from win2 closed
        /// </summary>
        string startTime;
        /// <summary>
        /// The end time - received from win2 closed
        /// </summary>
        string endTime;
        /// <summary>
        /// The exclude - received from win2 closed
        /// </summary>
        string Exclude;
        /// <summary>
        /// The minimum time required to complete the parse
        /// </summary>
        double time;
        /// <summary>
        /// The maximum time required to complete the parse
        /// </summary>
        double timeTwo;
        /// <summary>
        /// The process count is used to count the number of scripts running
        /// </summary>
        int processCount = 0;
        /// <summary>
        /// The filesize
        /// </summary>
        long filesize;
        /// <summary>
        /// The perlprocess was killed
        /// </summary>
        bool perlprocesswaskilled = false;
        /// <summary>
        /// The combochoice is a list of strings used to select locations for graph
        /// </summary>
        List<string> combochoice = new List<string>();
        /// <summary>
        /// The linelist stores the lines to be copied to a new script
        /// </summary>
        List<string> linelist;
        /// <summary>
        /// The dup string list - a list of the chosen duplicates
        /// </summary>
        List<string> dupStrList = new List<string>();
        /// <summary>
        /// The copy lines list
        /// </summary>
        List<LineItem> copyLinesList = new List<LineItem>();
        /// <summary>
        /// The data all checked, used to check all LineItem
        /// </summary>
        List<LineItem> dataAllChecked = new List<LineItem>();
        /// <summary>
        /// The list all checked,  used to check all scripts
        /// </summary>
        List<GroupItem> listAllChecked = new List<GroupItem>();
        /// <summary>
        /// The data load list stores the data loaded into data grid, it is used to check if changes are made. 
        /// </summary>
        List<LineItem> dataLoadList = new List<LineItem>();
        /// <summary>
        /// The datalist stores the current data in the data grid.
        /// </summary>
        List<LineItem> datalist = new List<LineItem>();
        /// <summary>
        /// The perlprocess is used for creating a sepearate thread to run the perl
        /// </summary>
        Process perlprocess;
        /// <summary>
        /// The process list contains all perl scripts currently runing;
        /// </summary>
        List<Process> processList = new List<Process>();
        /// <summary>
        /// A newscript window
        /// </summary>
        NewScriptWindow scriptWindow = new NewScriptWindow();
        /// <summary>
        /// The aboutboxwindow is the window that shows the about information.
        /// </summary>
        AboutBox1 aboutboxwindow = new AboutBox1();
        // The Xaml for this window is called: DatepickerWindow.xaml
        /// <summary>
        /// The win2
        /// </summary>
        Window1 win2 = new Window1();
        /// <summary>
        /// Class containing singlestring, above, below, to, from, exclude and xml of an object.
        /// </summary>
        public class LineItem
        {
            /// <summary>
            /// Gets or sets if checkbox is checked
            /// </summary>
            /// <value><c>true</c> if [grid checkbox]; otherwise, <c>false</c>.</value>
            public bool gridCheckbox { get; set; }
            /// <summary>
            /// Gets or sets the single string.
            /// </summary>
            /// <value>The single string.</value>
            public string searchString { get; set; }
            /// <summary>
            /// Gets or sets the lines above.
            /// </summary>
            /// <value>The lines above.</value>
            public string linesAbove { get; set; }
            /// <summary>
            /// Gets or sets the lines below.
            /// </summary>
            /// <value>The lines below.</value>
            public string linesBelow { get; set; }
            /// <summary>
            /// Gets or sets the selection one. From location selected.
            /// </summary>
            /// <value>The selection one.</value>
            public string selectionFrom { get; set; }
            /// <summary>
            /// Gets or sets the selection two. To location selected.
            /// </summary>
            /// <value>The selection two.</value>
            public string selectionTo { get; set; }
            /// <summary>
            /// Gets or sets a value for exclude checkbox.
            /// </summary>
            /// <value>
            ///   <c>true</c> if exclude; otherwise, <c>false</c>.
            /// </value>
            public bool exclude { get; set; }
            /// <summary>
            /// Gets or sets a value for xml checkbox
            /// </summary>
            /// <value>
            ///   <c>true</c> if XML; otherwise, <c>false</c>.
            /// </value>
            public bool xml { get; set; }
            /// <summary>
            /// Gets or sets the dropdown list from.
            /// </summary>
            /// <value>
            /// From.
            /// </value>
            public ObservableCollection<string> fromCollection { get; set; }

            /// <summary>
            /// Gets or sets the dropdown list to.
            /// </summary>
            /// <value>
            /// To.
            /// </value>
            public ObservableCollection<string> toCollection { get; set; }

        }
        /// <summary>
        /// Class GroupItem. A class containing gridNameColumn and gridCheckboxColumn, used for populating the listview (on left)
        /// </summary>
        public class GroupItem
        {
            /// <summary>
            /// Gets or sets the grid name column.
            /// </summary>
            /// <value>The grid name column.</value>
            public string gridNameColumn { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether grid checkbox column is checked.
            /// </summary>
            /// <value><c>true</c> if [grid checkbox column]; otherwise, <c>false</c>.</value>
            public bool gridCheckboxColumn { get; set; }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //MAIN
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //check to ensure that the file containing all the lines is available
            if (!File.Exists(categoryFile))
            {
                MessageBox.Show("You are missing a necessary file, please contact your provider.", "Missing file", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Script List Populating Functions
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Stops delete button from deleting lines from list grid (left).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs" /> instance containing the event data.</param>
        private void GroupDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if key pressed is delete - do nothing.
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the Loaded event of the GroupDataGrid control. - simply calls listreload
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void GroupDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //call list reload
            listReload();
        }

        /// <summary>
        /// Reloads the GroupDataGrid list (on the left).
        /// </summary>
        private void listReload()
        {
            //Create a list of GroupItem - a list of categories
            List<GroupItem> listOfCats = new List<GroupItem>();
            string line;
            //Open file that contains the categories and rules
            System.IO.StreamReader reader = new System.IO.StreamReader(categoryFile);
            //Read each line
            while ((line = reader.ReadLine()) != null)
            {
                //If the line starts with "name :"
                Regex regex = new Regex("^name :");
                if (regex.IsMatch(line))
                {
                    //Extract the name - it is after "name :"
                    string[] words = line.Split(':');
                    GroupItem cats = new GroupItem();
                    cats.gridNameColumn = words[1].ToString();
                    cats.gridCheckboxColumn = false;
                    //add category to list
                    listOfCats.Add(cats);
                }
            }
            //add list of categories to itemssource of GroupDataGrid
            GroupDataGrid.ItemsSource = listOfCats;
            GroupDataGrid.Columns[1].Header = "Select Group";
            GroupDataGrid.Columns[0].Width = 24;
            GroupDataGrid.Columns[1].IsReadOnly = true;
            reader.Close();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Line List Populating Functions
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Loads desired values into the Datagrid (on the right).
        /// </summary>
        /// <param name="regex">The regex.</param>
        private void DataReload(Regex regex)
        {
            //make dataLoadList and datalist new lists of LineItem
            dataLoadList = new List<LineItem>();
            datalist = new List<LineItem>();
            LineDataGrid.IsReadOnly = false;
            //make combochoice a new list of strings
            combochoice = new List<string>();
            //Used to read a line
            string line;
            //create reader to read line
            StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + @"\graphOptions.txt");

            //read through file and add each line to combochoice
            while ((line = reader.ReadLine()) != null)
            {
                combochoice.Add(line);
            }
            reader.Close();

            //reassign reader to read file containing categories and rules
            reader = new StreamReader(categoryFile);

            //read through the file until end
            while ((line = reader.ReadLine()) != null)
            {
                //if nothing is selected, do nothing.
                if (regex.IsMatch(""))
                {
                }
                else
                {
                    //if the line contains "name :" + current selected script
                    if ((regex).IsMatch(line))
                    {
                        //string to read next line
                        string nextLine;

                        //create variable of type LineItem called dets
                        LineItem dets = new LineItem();
                        //add "add new line" with DATE type etc to dets
                        dets.searchString = "Add New Line";
                        dets.linesAbove = "DATE";
                        dets.linesBelow = "DATE";
                        dets.selectionFrom = "";
                        dets.selectionTo = "";
                        dets.exclude = false;
                        dets.xml = false;
                        dets.fromCollection = new ObservableCollection<string>(combochoice);
                        dets.toCollection = new ObservableCollection<string>(combochoice);
                        //add dets to datalist
                        datalist.Add(dets);

                        //until reader gets to line containing "--", ie end of category
                        while ((nextLine = reader.ReadLine()) != "--")
                        {
                            //create type LineItem called getDataForGrid (itemssource for datagrid - will change as grid is edited) and 
                            //another called initialDataInGrid (used to check if changes made - this won't change)
                            LineItem getDataForGrid = new LineItem();
                            LineItem initialDataInGrid = new LineItem();
                            //split line by delimeter
                            string[] argsvalue = Regex.Split(nextLine, DelimiterRead);
                            //add lines above to getDataForGrid and initialDataInGrid, if empty set to empty
                            try
                            {
                                getDataForGrid.linesAbove = argsvalue[1];
                                initialDataInGrid.linesAbove = argsvalue[1];
                            }
                            catch
                            {
                                getDataForGrid.linesAbove = "";
                                initialDataInGrid.linesAbove = "";
                            }
                            //add lines below to getDataForGrid and initialDataInGrid, if empty set to empty
                            try
                            {
                                getDataForGrid.linesBelow = argsvalue[2];
                                initialDataInGrid.linesBelow = argsvalue[2];
                            }
                            catch
                            {
                                getDataForGrid.linesBelow = "";
                                initialDataInGrid.linesBelow = "";
                            }
                            //add selectionFrom to getDataForGrid and initialDataInGrid, if empty set to empty
                            try
                            {
                                getDataForGrid.selectionFrom = argsvalue[3];
                                initialDataInGrid.selectionFrom = argsvalue[3];
                            }
                            catch
                            {
                                getDataForGrid.selectionFrom = "";
                                initialDataInGrid.selectionFrom = "";
                            }
                            //add selectionTo to getDataForGrid and initialDataInGrid, if empty set to empty
                            try
                            {
                                getDataForGrid.selectionTo = argsvalue[4];
                                initialDataInGrid.selectionTo = argsvalue[4];
                            }
                            catch
                            {
                                getDataForGrid.selectionTo = "";
                                initialDataInGrid.selectionTo = "";
                            }
                            //add xml to getDataForGrid and initialDataInGrid - convert from string to bool, if empty set to false
                            try
                            {
                                if (argsvalue[5] == "xmltrue")
                                {
                                    getDataForGrid.xml = true;
                                    initialDataInGrid.xml = true;
                                }
                                else
                                {
                                    getDataForGrid.xml = false;
                                    initialDataInGrid.xml = false;
                                }
                            }
                            catch
                            {
                                getDataForGrid.xml = false;
                                initialDataInGrid.xml = false;
                            }
                            //add exclude to getDataForGrid and initialDataInGrid - convert from string to bool, if empty set to false
                            try
                            {
                                if (argsvalue[6] == "excludetrue")
                                {
                                    getDataForGrid.exclude = true;
                                    initialDataInGrid.exclude = true;
                                }
                                else
                                {
                                    getDataForGrid.exclude = false;
                                    initialDataInGrid.exclude = false;
                                }
                            }
                            catch
                            {
                                getDataForGrid.exclude = false;
                                initialDataInGrid.exclude = false;
                            }

                            //add searchString, fromCollection and toCollection to getDataForGrid
                            getDataForGrid.searchString = argsvalue[0];
                            getDataForGrid.fromCollection = new ObservableCollection<string>(combochoice);
                            getDataForGrid.toCollection = new ObservableCollection<string>(combochoice);
                            //add getDataForGrid to datalist
                            datalist.Add(getDataForGrid);

                            //add searchString, fromCollection and toCollection to initialDataInGrid
                            initialDataInGrid.searchString = argsvalue[0];
                            initialDataInGrid.fromCollection = new ObservableCollection<string>(combochoice);
                            initialDataInGrid.toCollection = new ObservableCollection<string>(combochoice);
                            //add initialDataInGrid to dataLoadList
                            dataLoadList.Add(initialDataInGrid);

                        }
                    }
                }
            }
            InitializeComponent();
            //set datagrid itemssource equal to datalist
            LineDataGrid.ItemsSource = datalist;
            //Adjust columns as necessary
            LineDataGrid.Columns[1].IsReadOnly = false;
            LineDataGrid.Columns[2].IsReadOnly = false;
            LineDataGrid.Columns[3].IsReadOnly = false;
            LineDataGrid.Columns[4].IsReadOnly = false;
            LineDataGrid.Columns[5].IsReadOnly = false;
            LineDataGrid.Columns[6].IsReadOnly = false;
            LineDataGrid.Columns[7].IsReadOnly = false;

            //make columns visible
            LineDataGrid.Columns[0].Visibility = Visibility.Visible;
            LineDataGrid.Columns[1].Visibility = Visibility.Visible;
            LineDataGrid.Columns[2].Visibility = Visibility.Visible;
            LineDataGrid.Columns[3].Visibility = Visibility.Visible;
            LineDataGrid.Columns[4].Visibility = Visibility.Visible;
            LineDataGrid.Columns[5].Visibility = Visibility.Visible;
            LineDataGrid.Columns[6].Visibility = Visibility.Visible;
            LineDataGrid.Columns[7].Visibility = Visibility.Visible;
            //close reader
            reader.Close();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Script Combobox Populating Functions
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// empty script select combo every time left mouse button pressed on it.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void ScriptSelectCombo_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //set scriptselectcombo to null
            ScriptSelectCombo.SelectedValue = null;
        }

        /// <summary>
        /// The ScriptSelectCombo is the combobox that allows the user to select a category to edit. Calls combobox_reload to reload the script select combo.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ScriptSelectCombo_Loaded(object sender, RoutedEventArgs e)
        {
            //call ComboBox_Reload
            ComboBox_Reload();
        }

        /// <summary>
        /// Loads data into the script select combo box.
        /// </summary>
        private void ComboBox_Reload()
        {
            //create string array
            string[] line_words;
            //create a list of strings called data
            List<string> data = new List<string>();
            //create a string for reading lines in the script file
            string line;
            //create a streamreader
            StreamReader reader = new StreamReader(categoryFile);
            //read file until end
            while ((line = reader.ReadLine()) != null)
            {
                //create regex
                Regex regex = new Regex("^name :");
                //check if "name :" is on the line
                if (regex.IsMatch(line))
                {
                    //if so split line, take text after ":" and add to data - a list of strings
                    line_words = line.Split(':');
                    data.Add(line_words[1].ToString());
                }
            }
            //close the reader
            reader.Close();
            //clear the ScriptSelectCombo itemssource, then add data.
            ScriptSelectCombo.ItemsSource = null;
            ScriptSelectCombo.ItemsSource = data;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Selections - can be done using GroupDataGrid on the left or ScriptSelectCombo on the right.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Handles the SelectionChanged event of the GroupDataGrid control.
        /// sets the script combobox selection which ultimately loads the data grid (on right)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        private void GroupDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //check if there are changes to save
            //create bool called editsMade set to false
            bool editsMade = false;
            //create list of strings called beforeDetsStr - holds what is in datagrid when it was loaded or saved
            List<string> beforeDetsStr = new List<string>();
            //create list of strings called afterDetsStr - holds what is in datagrid now
            List<string> afterDetsStr = new List<string>();
            //for each loaded in dataLoadList - create list of strings instead of list of LineItem
            foreach (LineItem loaded in dataLoadList)
            {
                //if exclude is true
                if (loaded.exclude == true)
                {
                    //xml is also a bool and will need to be converted to a string. -> xmltrue - excludetrue
                    if (loaded.xml == true)
                    {
                        string beforestring = loaded.searchString + DelimiterWrite + loaded.linesAbove + DelimiterWrite + loaded.linesBelow + DelimiterWrite + loaded.selectionFrom + DelimiterWrite + loaded.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludetrue";
                        //add to beforeDetsStr
                        beforeDetsStr.Add(beforestring);
                    }
                    //-> xmlfalse - excludetrue
                    else
                    {
                        string beforestring = loaded.searchString + DelimiterWrite + loaded.linesAbove + DelimiterWrite + loaded.linesBelow + DelimiterWrite + loaded.selectionFrom + DelimiterWrite + loaded.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludetrue";
                        //add to beforeDetsStr
                        beforeDetsStr.Add(beforestring);
                    }
                }
                else
                {
                    //-> xmltrue - excludefalse
                    if (loaded.xml == true)
                    {
                        string beforestring = loaded.searchString + DelimiterWrite + loaded.linesAbove + DelimiterWrite + loaded.linesBelow + DelimiterWrite + loaded.selectionFrom + DelimiterWrite + loaded.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludefalse";
                        //add to beforeDetsStr
                        beforeDetsStr.Add(beforestring);
                    }
                    //-> xmlfalse - excludefalse
                    else
                    {

                        string beforestring = loaded.searchString + DelimiterWrite + loaded.linesAbove + DelimiterWrite + loaded.linesBelow + DelimiterWrite + loaded.selectionFrom + DelimiterWrite + loaded.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludefalse";
                        //add to beforeDetsStr
                        beforeDetsStr.Add(beforestring);
                    }
                }
            }
            //for each current in datalist - create list of strings instead of list of LineItem
            foreach (LineItem current in datalist)
            {
                //if not "Add New Line"
                if (current.searchString != "Add New Line")
                {
                    if (current.exclude == true)
                    {
                        //xml is also a bool and will need to be converted to a string. -> xmltrue - excludetrue
                        if (current.xml == true)
                        {
                            string currentstring = current.searchString + DelimiterWrite + current.linesAbove + DelimiterWrite + current.linesBelow + DelimiterWrite + current.selectionFrom + DelimiterWrite + current.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludetrue";
                            //add to afterDetsStr
                            afterDetsStr.Add(currentstring);
                        }
                        //-> xmlfalse - excludetrue
                        else
                        {
                            string currentstring = current.searchString + DelimiterWrite + current.linesAbove + DelimiterWrite + current.linesBelow + DelimiterWrite + current.selectionFrom + DelimiterWrite + current.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludetrue";
                            //add to afterDetsStr
                            afterDetsStr.Add(currentstring);
                        }
                    }
                    else
                    {
                        //-> xmltrue - excludefalse
                        if (current.xml == true)
                        {
                            string currentstring = current.searchString + DelimiterWrite + current.linesAbove + DelimiterWrite + current.linesBelow + DelimiterWrite + current.selectionFrom + DelimiterWrite + current.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludefalse";
                            //add to afterDetsStr
                            afterDetsStr.Add(currentstring);
                        }
                        //-> xmlfalse - excludefalse
                        else
                        {

                            string currentstring = current.searchString + DelimiterWrite + current.linesAbove + DelimiterWrite + current.linesBelow + DelimiterWrite + current.selectionFrom + DelimiterWrite + current.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludefalse";
                            //add to afterDetsStr
                            afterDetsStr.Add(currentstring);
                        }
                    }
                }
            }

            //foreach detsStr in beforeDetsStr
            foreach (string detsStr in beforeDetsStr)
            {
                //if not in afterDetsStr - ie no longer in the grid
                if (!afterDetsStr.Contains(detsStr))
                {
                    //set editsMade to true
                    editsMade = true;
                }
            }
            //for each detsStr in afterDetsStr
            foreach (string detsStr in afterDetsStr)
            {
                //if not in beforeDetsStr - ie new to the grid
                if (!beforeDetsStr.Contains(detsStr))
                {
                    //set editsMade to true
                    editsMade = true;
                }
            }
            //for all groups in groupdatagrids itemssource
            foreach (GroupItem group in GroupDataGrid.ItemsSource)
            {
                //when selected item is found
                if (group == GroupDataGrid.SelectedItem)
                {
                    //if scriptselectcombo is not null
                    if (ScriptSelectCombo.SelectedValue != null)
                    {
                        //if sciptselectcombos selection is not the same as what is in the textbox
                        if (ScriptSelectCombo.SelectedValue.ToString() != editScriptNameTextbox.Text.ToString())
                        {
                            //editsmade is true
                            editsMade = true;
                        }
                    }
                }
            }


            //if editsMade is true
            if (editsMade == true)
            {
                //display messagebox, saying edits have been made, would user like to save or ignore changes?
                if (MessageBox.Show("Changes have been made to the current group, would you like to save?", "Group Edited Alert", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {

                    //call save function and return
                    save();
                    //set selected item in GroupDataGrid to selected Value - original value
                    foreach (GroupItem group in GroupDataGrid.ItemsSource)
                    {
                        //find selected group
                        if (group.gridNameColumn == selectedValue)
                        {
                            //set groupdatagrids selection to group
                            GroupDataGrid.SelectedValue = group;
                            //set scriptselectcombos selection to selectedValue
                            ScriptSelectCombo.SelectedValue = selectedValue;
                        }
                    }
                }
            }
            //for all items is the GroupDataGrid
            foreach (GroupItem selected in GroupDataGrid.ItemsSource)
            {
                //if the selected item
                if (selected == GroupDataGrid.SelectedValue)
                {
                    //set the ScriptSelectCombo selection to be the same as the GroupDataGrid selection
                    ScriptSelectCombo.SelectedValue = selected.gridNameColumn;
                }
            }
        }

        /// <summary>
        /// If selection changed in script select combo, displays columns and call datareload to fill them.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ScriptSelectCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if an item in the ScriptSelectCombo is selected
            if (ScriptSelectCombo.SelectedItem != null)
            {
                //set all columns in the LineDataGrid to visible
                LineDataGrid.Columns[0].Visibility = Visibility.Visible;
                LineDataGrid.Columns[1].Visibility = Visibility.Visible;
                LineDataGrid.Columns[2].Visibility = Visibility.Visible;
                LineDataGrid.Columns[3].Visibility = Visibility.Visible;
                LineDataGrid.Columns[4].Visibility = Visibility.Visible;
                LineDataGrid.Columns[5].Visibility = Visibility.Visible;
                LineDataGrid.Columns[6].Visibility = Visibility.Visible;
                LineDataGrid.Columns[7].Visibility = Visibility.Visible;
                //ensure the datacheckboxheader is not checked
                datacheckboxheader.IsChecked = false;
                //add the selected script name to the editScriptNameTextbox
                editScriptNameTextbox.Text = ScriptSelectCombo.SelectedItem.ToString();
                //set selectedValue to the selected script
                selectedValue = ScriptSelectCombo.SelectedItem.ToString();
                //create regex of "name : + selectedValue"
                Regex regex = new Regex("^name :" + selectedValue + "$");
                //call datareload, passing regex to it
                DataReload(regex);
                //enable the editScriptNameTextbox so the script name can be edited
                editScriptNameTextbox.IsEnabled = true;
            }
            else
            {
                //if selected item is null, set itemssource to null.
                LineDataGrid.ItemsSource = null;
                //set columns in LineDataGrid to hidden
                LineDataGrid.Columns[0].Visibility = Visibility.Hidden;
                LineDataGrid.Columns[1].Visibility = Visibility.Hidden;
                LineDataGrid.Columns[2].Visibility = Visibility.Hidden;
                LineDataGrid.Columns[3].Visibility = Visibility.Hidden;
                LineDataGrid.Columns[4].Visibility = Visibility.Hidden;
                LineDataGrid.Columns[5].Visibility = Visibility.Hidden;
                LineDataGrid.Columns[6].Visibility = Visibility.Hidden;
                LineDataGrid.Columns[7].Visibility = Visibility.Hidden;
                //empty editScriptNameTextbox
                editScriptNameTextbox.Text = null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Menu Functions
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Handles the exitBtn event of the MenuItem control. Exit Button closes the application
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItem_exitBtn(object sender, RoutedEventArgs e)
        {
            //close the application
            this.Close();
        }

        /// <summary>
        /// Handles the HelpButton event of the MenuItem control. Opens help html.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItem_HelpButton(object sender, RoutedEventArgs e)
        {
            //opens the help file in a html
            System.Diagnostics.Process.Start(@"ACR_Help.html");
        }

        /// <summary>
        /// Handles the AboutButton event of the MenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItem_AboutButton(object sender, RoutedEventArgs e)
        {
            //open the about button window.
            try
            {
                //if it already exists, show instance of AboutBox1
                aboutboxwindow.Show();
                aboutboxwindow.Show();
            }
            catch (Exception)
            {
                //if it doesn't exist create AboutBox1
                aboutboxwindow = new AboutBox1();
                aboutboxwindow.Show();
                aboutboxwindow.Activate();
            }
        }

        /// <summary>
        /// Handles the Closed event of the WFO_Grep_Tool control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WFO_Grep_Tool_Closed(object sender, EventArgs e)
        {
            //close any windows that might be open
            scriptWindow.Close();
            win2.Close();
            //kill any perl processes still running
            perlprocesswaskilled = true;
            foreach (Process process in processList)
            {
                process.Kill();
            }
            //clear processList
            processList.Clear();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Save Changes Functions. Used to save changes to scriptname and lines.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       

        /// <summary>
        /// Checks for duplicates and errors in the datagrid and if none found will save any changes, else error message created.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void save()
        {
            //create a list of strings called before
            List<string> before = new List<string>();
            //create a list of strings called current
            List<string> current = new List<string>();
            //create a list of strings call existingScripts
            List<string> existingScripts = new List<string>();
            //add all the script names from the GroupDataGrid to existingScripts
            foreach (GroupItem cats in GroupDataGrid.ItemsSource)
            {
                existingScripts.Add(cats.gridNameColumn);
            }
            //if selected script is null, create message to tell user no script has been selected
            if (string.IsNullOrWhiteSpace(ScriptSelectCombo.SelectedValue.ToString()))
            {
                MessageBox.Show("No Script Selected", "Invaild name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            //create a bool namechanged - set to false
            bool namechanged = false;
            //create a match of special characters
            Match matchChar = Regex.Match(editScriptNameTextbox.Text, "[?@+%!\"\\[\\]*;]", RegexOptions.IgnoreCase);
            //if text is different in editScriptNameTextbox to that in scriptSelectCombo - script name has been edited
            if (editScriptNameTextbox.Text != ScriptSelectCombo.SelectedValue.ToString())
            {
                //if empty
                if (string.IsNullOrWhiteSpace(editScriptNameTextbox.Text))
                {
                    MessageBox.Show("Script name can not be Null.", "Invaild name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                //if special characters
                else if (matchChar.Success)
                {
                    MessageBox.Show("Script name can not contain any special characters.", "Invaild name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                //if scriptname already exists (existingscripts contains all scripts in GroupDataGrid, was created at start of this function)
                else if (existingScripts.Contains(editScriptNameTextbox.Text))
                {
                    MessageBox.Show("Script name is already in use.", "Invaild name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    //otherwise, call savenewscriptname function and set namechanged to true
                    SaveNewScriptName();
                    namechanged = true;
                }
            }

            //for each item in dataLoadList - the initial values loaded into the grid
            foreach (LineItem dets in dataLoadList)
            {
                //Take the dets and merge them into a string, split by the delimeter, this will be used to compare with the current values in the grid to see if edits have been made
                //Exclude is a bool, it needs to be converted to a string
                if (dets.exclude == true)
                {
                    //xml is also a bool and will need to be converted to a string. -> xmltrue - excludetrue
                    if (dets.xml == true)
                    {
                        string beforestring = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludetrue";
                        before.Add(beforestring);
                    }
                    //-> xmlfalse - excludetrue
                    else
                    {
                        string beforestring = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludetrue";
                        before.Add(beforestring);
                    }
                }
                else
                {
                    //-> xmltrue - excludefalse
                    if (dets.xml == true)
                    {

                        string beforestring = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludefalse";
                        before.Add(beforestring);
                    }
                    //-> xmlfalse - excludefalse
                    else
                    {

                        string beforestring = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludefalse";
                        before.Add(beforestring);
                    }
                }
            }


            //for each item in dataList - the current values in the grid
            foreach (LineItem dets in datalist)
            {
                //if not "add new line"
                if (dets.searchString != "Add New Line")
                {
                    //if Date is in linesabove in any format, set to DATE
                    if ((dets.linesAbove.IndexOf("DATE", StringComparison.OrdinalIgnoreCase)) >= 0)
                    {
                        dets.linesAbove = "DATE";
                    }
                    //if Date is in linesbelow in any format, set to DATE
                    if ((dets.linesBelow.IndexOf("DATE", StringComparison.OrdinalIgnoreCase)) >= 0)
                    {
                        dets.linesBelow = "DATE";
                    }

                    //if search string is empty, send error message and return
                    if (string.IsNullOrWhiteSpace(dets.searchString))
                    {
                        MessageBox.Show("Nothing in string on line " + LineDataGrid.SelectedIndex, "Invaild string", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                    //set LineDataGrids selected item to dets
                    LineDataGrid.SelectedItem = dets;
                    //create a string with current searchsting
                    string selectedString = dets.searchString;
                    //for each item it the datalist
                    foreach (LineItem dets2 in datalist)
                    {
                        //if dets 2 is ntot the LineDataGrids selected item
                        if (dets2 != LineDataGrid.SelectedItem)
                        {
                            //if a match of searchstrings is found - show error message containing string
                            if (selectedString == dets2.searchString)
                            {
                                MessageBox.Show("There is a duplication of strings, please ensure all are unique to continue, dup = " + selectedString, "Invaild string", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }//exit second foreach loop
                    //create temporary int inttemp
                    int inttemp;

                    //if number in bottom but blank in top, set top to 0
                    if (string.IsNullOrWhiteSpace(dets.linesAbove))
                    {
                        if (int.TryParse(dets.linesBelow, out inttemp))
                        {
                            dets.linesAbove = "0";
                        }
                    }

                    //if number in top but blank in bottom, set bottom to 0
                    if (string.IsNullOrWhiteSpace(dets.linesBelow))
                    {
                        if (int.TryParse(dets.linesAbove, out inttemp))
                        {
                            dets.linesBelow = "0";
                        }
                    }

                    //if int in top
                    if (int.TryParse(dets.linesAbove, out inttemp))
                    {
                        //if "+" or "-", error message - no special characters
                        if (dets.linesAbove.Contains("-") || dets.linesAbove.Contains("+"))
                        {
                            MessageBox.Show("Integer values must not contain any special characters.", "integer Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            return;
                        }
                    }

                    //if int in bottom
                    if (int.TryParse(dets.linesBelow, out inttemp))
                    {
                        //if "+" or "-", error message - no special characters
                        if (dets.linesBelow.Contains("-") || dets.linesBelow.Contains("+"))
                        {
                            MessageBox.Show("Integer values must not contain any special characters.", "integer Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            return;
                        }
                    }

                    //if linesAbove not empty and not an int (ie it's a string) and linesBelow is empty, set linesBelow to ""
                    if (((!string.IsNullOrWhiteSpace(dets.linesAbove)) && (!int.TryParse(dets.linesAbove, out inttemp))) && string.IsNullOrWhiteSpace(dets.linesBelow))
                    {
                        dets.linesBelow = "";
                    }

                    //if linesBelow not empty and not an int (ie it's a string) and linesAbove is empty, set linesAbove to ""
                    if (((!string.IsNullOrWhiteSpace(dets.linesBelow)) && (!int.TryParse(dets.linesBelow, out inttemp))) && string.IsNullOrWhiteSpace(dets.linesAbove))
                    {
                        dets.linesAbove = "";
                    }

                    //if linesAbove is "DATE" and linesBelow is empty, set linesBelow to "DATE"
                    if ((dets.linesAbove == "DATE") && (string.IsNullOrWhiteSpace(dets.linesBelow)))
                    {
                        dets.linesBelow = "DATE";
                    }

                    //if linesBelow is "DATE" and linesAbove is empty, set linesAbove to "DATE"
                    if ((dets.linesBelow == "DATE") && (string.IsNullOrWhiteSpace(dets.linesAbove)))
                    {
                        dets.linesAbove = "DATE";
                    }

                    //if linesAbove is empty and linesBelow is empty, set both linesAbove and linesBelow to "DATE"
                    if (string.IsNullOrWhiteSpace(dets.linesAbove) && string.IsNullOrWhiteSpace(dets.linesBelow))
                    {
                        dets.linesAbove = "DATE";
                        dets.linesBelow = "DATE";
                    }

                    //if linesAbove is an int and linesBelow is not an int OR linesBelow is an int and linesAbove is not an int, display error message
                    if (((int.TryParse(dets.linesAbove, out inttemp)) && (!int.TryParse(dets.linesBelow, out inttemp))) || ((!int.TryParse(dets.linesAbove, out inttemp)) && (int.TryParse(dets.linesBelow, out inttemp))))
                    {
                        MessageBox.Show(dets.linesAbove + " not the same type as " + dets.linesBelow + " on line " + LineDataGrid.SelectedIndex + ". \nBoth must be Integers, Strings or of type Date.", "Type Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }

                    //if linesAbove is "DATE" and linesBelow is not "DATE" OR linesAbove is not "DATE" and linesBelow is "DATE", display error message
                    if ((dets.linesAbove == "DATE" && dets.linesBelow != "DATE") || (dets.linesAbove != "DATE" && dets.linesBelow == "DATE"))
                    {
                        MessageBox.Show(dets.linesAbove + " not the same type as " + dets.linesBelow + " on line " + LineDataGrid.SelectedIndex + ". \nBoth must be Integers, Strings or of type Date.", "Type Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }

                    //Take the dets and merge them into a string, split by the delimeter, this will be used to compare with the original values in the grid to see if edits have been made
                    //Exclude is a bool, it needs to be converted to a string
                    if (dets.exclude == true)
                    {
                        //-> xmltrue - excludetrue
                        if (dets.xml == true)
                        {
                            string currentstring = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludetrue";
                            current.Add(currentstring);
                        }
                        //-> xmlfalse - excludetrue
                        else
                        {
                            string currentstring = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludetrue";
                            current.Add(currentstring);
                        }
                    }
                    else
                    {
                        //-> xmltrue - excludefalse
                        if (dets.xml == true)
                        {
                            string currentstring = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludefalse";
                            current.Add(currentstring);
                        }
                        //-> xmlfalse - excludefalse
                        else
                        {
                            string currentstring = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludefalse";
                            current.Add(currentstring);
                        }
                    }
                }
            }

            //create int checkForChangesCount
            int checkForChangesCount = 0;
            //foreach beforedet in current - current items in LineDataGrid
            foreach (string beforedet in current)
            {
                //if before list contains beforedet
                if (!before.Contains(beforedet))
                {
                    //call saveGridChanges, passing the current list
                    saveGridChanges(current);
                    //add to check for changes count
                    checkForChangesCount++;
                }
            }
            //for each beforedet in before
            foreach (string beforedet in before)
            {
                //if current list contains beforedet
                if (!current.Contains(beforedet))
                {
                    //call saveGridChanges, passing the current list
                    saveGridChanges(current);
                    //add to check for changes count
                    checkForChangesCount++;
                }
            }
            //if checkforchangescount is 0 and namechanged is false - ie not changes to lines or name of script, display error message
            if ((checkForChangesCount == 0) && (namechanged == false))
            {
                MessageBox.Show("No changes made", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }
            //create regex with current selected value
            Regex regex = new Regex("^name :" + ScriptSelectCombo.SelectedValue + "$");
            //call datareload and pass regex
            DataReload(regex);
            //reload GroupDataGrid
            listReload();
            //create string containg (possibly edited) script name
            string a = editScriptNameTextbox.Text.ToString();
            //call combobox_reload
            ComboBox_Reload();
            //set selected script to name in textbox
            ScriptSelectCombo.SelectedValue = a;

        }

        /// <summary>
        /// If scripts name has been changed and save has been pressed, this will check for duplicates and save the new name to file.
        /// </summary>
        private void SaveNewScriptName()
        {
            //create a string for reading lines in the script file
            string line;
            //create a temporary file
            string tempfile = System.IO.Path.GetTempFileName();
            //create a regex using "^name :" and selected value
            Regex regex = new Regex("^name :" + selectedValue + "$");
            //create a streamreader to read from categoryFile
            StreamReader reader = new StreamReader(categoryFile);
            //create a streamwriter to write to tempfile
            StreamWriter writer = new StreamWriter(tempfile);
            //read file until end
            while ((line = reader.ReadLine()) != null)
            {
                //match script to be changed
                if (regex.IsMatch(line))
                {
                    //if match found, replace with new name and write to tempfile
                    writer.WriteLine("name :" + editScriptNameTextbox.Text);
                }
                else
                {
                    //write all other lines as normal to tempfile
                    writer.WriteLine(line);
                }
            }
            //close writer
            writer.Close();
            //close reader
            reader.Close();
            //assign reader to tempfile
            reader = new StreamReader(tempfile);
            //assign writer to categoryfile
            writer = new StreamWriter(categoryFile);
            //read each line in tempfile until the end
            while ((line = reader.ReadLine()) != null)
            {
                //write all lines to categoryfile
                writer.WriteLine(line);
            }
            //close writer
            writer.Close();
            //close reader
            reader.Close();
            //delete tempfile
            File.Delete(tempfile);
        }

        /// <summary>
        /// If data in the datagrid has been changed and save has been pressed, this will check for duplicates and save them to file.
        /// </summary>
        /// <param name="current">The current.</param>
        private void saveGridChanges(List<string> current)
        {
            //create a string for reading lines in the script file
            string line;
            //create a temporary file
            string tempfile = System.IO.Path.GetTempFileName();
            //create a regex using "^name :" and selected value
            Regex regex = new Regex("^name :" + editScriptNameTextbox.Text + "$");
            //create a streamreader to read from categoryFile
            StreamReader reader = new StreamReader(categoryFile);
            //create a streamwriter to write to tempfile
            StreamWriter writer = new StreamWriter(tempfile);
            //read file until end
            while ((line = reader.ReadLine()) != null)
            {
                //write line to tempfile
                writer.WriteLine(line);
                //match script to be changed
                if (regex.IsMatch(line))
                {
                    string nextline;
                    while ((nextline = reader.ReadLine()) != "--")
                    {
                    }
                    //replace old data with new data for selected script
                    //for each string in current
                    foreach (string currentDets in current)
                    {
                        //write to tempfile
                        writer.WriteLine(currentDets);
                    }
                    //write "--" to finish
                    writer.WriteLine("--");
                }
            }
            //close writer
            writer.Close();
            //close reader
            reader.Close();
            //assign reader to tempfile
            reader = new StreamReader(tempfile);
            //assign writer to categoryFile
            writer = new StreamWriter(categoryFile);
            //read each line in tempfile until the end
            while ((line = reader.ReadLine()) != null)
            {
                //write all lines to categoryfile
                writer.WriteLine(line);
            }
            //close writer
            writer.Close();
            //close reader
            reader.Close();
            //delete tempfile
            File.Delete(tempfile);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Edit graph options file - to and from options.
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Opens window to allow editing of graph options - to and from values
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void EditGraphOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            //create EditForDiagramWindow called diagramEditingWindow
            EditForDiagramWindow diagramEditingWindow = new EditForDiagramWindow();
            try
            {
                //show dialog if it already exists
                diagramEditingWindow.ShowDialog();
                diagramEditingWindow.Activate();
            }
            catch (Exception)
            {
                //if not create a new instance of the window
                diagramEditingWindow = new EditForDiagramWindow();
                diagramEditingWindow.ShowDialog();
                diagramEditingWindow.Activate();
            }
            //if closed
            if (DialogResult == null)
            {
                //call diagramEditingWindow_Closed()
                diagramEditingWindow_Closed();
            }
        }

        /// <summary>
        /// When diagram editing window has closed, call datareload to reset data grid (on right)
        /// </summary>
        void diagramEditingWindow_Closed()
        {
            //create regex using "name :" + selected script
            Regex regex = new Regex("^name :" + ScriptSelectCombo.SelectedValue + "$");
            //call data reload and send it regex
            DataReload(regex);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Select File to open and Folder to save output.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Handles the Click event of the ChooseFileButton control.
        /// Allows the user to select a log or text file to parse
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            //enable fileLabel
            FileLabel.IsEnabled = true;
            //Open an OpenFileDialog window called dlg
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //set result from dlg to a nullable bool called result
            Nullable<bool> result = dlg.ShowDialog();

            // if result is true 
            if (result == true)
            {
                //assign file_name to the file returned
                file_Name = dlg.FileName;
                //set text in filelabel to file_name - display selected file
                FileLabel.Text = System.IO.Path.GetFileName(file_Name);
                //create fileinfo called f and assign file_name to it
                FileInfo f = new FileInfo(file_Name);
                //get size of file
                filesize = f.Length;
            }
            //if file_name is not null
            if (file_Name != null)
            {
                //create string fileCheck and assign file_names extension to it
                string fileCheck = System.IO.Path.GetExtension(file_Name);
                //if filecheck isn't a txt or log file
                if (fileCheck != ".txt" && fileCheck != ".log")
                {
                    //error message, set filelabel to empty, and return.
                    MessageBox.Show("Invalid file format.\nPlease use file types with the extensions .log or .txt.", "Invalid file", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    FileLabel.Text = string.Empty;
                    return;
                }
                else
                {
                    //set outputFileName and outputLabel to file_name directory name
                    outputFileName = System.IO.Path.GetDirectoryName(file_Name);
                    OutputLabel.Text = System.IO.Path.GetDirectoryName(file_Name);
                    //set FileLabel to selected files name
                    FileLabel.Text = System.IO.Path.GetFileName(file_Name);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ChooseOutputFolderButton control.
        /// Allows the user to select an output folder
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ChooseOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            //enable outputlabel
            OutputLabel.IsEnabled = true;
            //create FolderBrowserDialog to select output folder
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            //if file_name is not null
            if (file_Name != null)
            {
                //create string called pathName
                string pathName;
                //create string called fileNameRemoval assigned to file name
                string fileNameRemoval = System.IO.Path.GetFileName(file_Name);
                //create int outputFilePath assigned to filename index and fileNameRemoval
                int outputFilePath = file_Name.IndexOf("\\" + fileNameRemoval);
                //assign pathname to file name with outputfilepath removed
                pathName = file_Name.Remove(outputFilePath);
                //set dlg.selectedpath to pathname
                dlg.SelectedPath = pathName;
            }
            //show dialog
            dlg.ShowDialog();
            //outputfilename equals dlg.selectedpath 
            outputFileName = dlg.SelectedPath;
            //create string array getName and use to split outputFileName with "\\"
            string[] getName = outputFileName.Split('\\');
            //assign outputlabel to last string in getname
            OutputLabel.Text = getName.Last();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Choose extra options - dates and exclude.
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Handles the Click event of the ExtraOptionsButton control.
        /// Opens the Extra Option as a new window
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
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
            //win2.Closed += win2_Closed;
            if (DialogResult == null)
            {
                extraOptionsClosed();
            }
        }

        /// <summary>
        /// Extras the options closed.
        /// passes the Exclude string value to the main console
        /// And also the date string information
        /// while converting the time stamps from 12 hr to 24 hours
        /// </summary>
        void extraOptionsClosed()
        {
            int counter;
            string splitstartTime = "";
            int hours;
            string ABG;
            try
            {
                // retrieves the excludestring from the Win2 window
                Exclude = win2.excludeString.ToString();
            }
            catch (Exception)
            {
                Exclude = null;
            }
            try
            {
                // retrieves the startTime from the Win2 window
                startTime = win2.startOne.ToString();
                // retrieves the endTime from the Win2 window
                endTime = win2.endOne.ToString();
            }
            catch (NullReferenceException)
            {
                startTime = null;
                endTime = null;
                return;
            }
            try
            {
                splitstartTime = startTime.Split(' ')[2];
            }
            catch (Exception)
            {
                // the time picker has problems on certain time formats such as yyyy-mm-dd and dd-mmm-yy, the problem using these formats is that i'm unable to tell if the user entered in 3 AM or 3 PM
                // as the time will always come out in a 12hr format like 3:00:00 for AM and 3:00:00 for PM
                MessageBox.Show("Your current time format is not compatible with the time picker,\nPlease choose from one of the compatible time formats located in the help file. ", "Time Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }


            // splits the year from the string and inserts it at the beginning of the time stamp 
            splitstartTime = startTime.Split('/')[2];
            splitstartTime = splitstartTime.Split(' ')[0];
            startTime = startTime.Replace("/" + splitstartTime, "");
            startTime = startTime.Insert(0, splitstartTime + "/");
            startTime = startTime.Replace(" " + splitstartTime, "");



            // splits the month from the string and if its less then ten adds in a zero
            // some formats include the zero while some only leave in a single digit 
            // the full format is need for searching the log file ie yyyy-mm-dd not yyyy-m-dd
            splitstartTime = startTime.Split('/')[1];
            int split = Convert.ToInt32(splitstartTime);
            if (split < 10)
            {
                startTime = startTime.Replace("/" + splitstartTime, "/0" + split.ToString());
            }


            // splits the year from the string and inserts it at the beginning of the time stamp 
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

            // replaces all the forward slashes with - as this is the time format in the log file
            startTime = startTime.Replace("/", "-");

            // Removes the am or pm string for the end of the time stamp
            splitstartTime = startTime.Split(' ')[2];
            startTime = startTime.Replace(" " + splitstartTime, "");


            // converts the 12 hr time  to 24 hr time  for the start string
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

            // checks to see if the day value is a single digit and adds in a zero if true
            if (splitstartTime.Length == 1)
            {
                startTime = startTime.Replace("-" + splitstartTime, "-0" + splitstartTime);
            }

            //removes the spaces from the string as this causes errors when passed to the perl as it thinks it is two different strings
            startTime = startTime.Replace(" ", "");



            // replaces all the forward slashes with - as this is the time format in the log file
            endTime = endTime.Replace("/", "-");

            // Removes the am or pm string for the end of the time stamp
            splitstartTime = endTime.Split(' ')[2];
            endTime = endTime.Replace(" " + splitstartTime, "");

            // converts the 12 hr time  to 24 hr time  for the start string
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
            if (splitstartTime.Length == 1)
            {
                endTime = endTime.Replace("-" + splitstartTime, "-0" + splitstartTime);
            }

            endTime = endTime.Replace(" ", "");
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Run Functions
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Handles the Click event of the Run_Button control. Sorts all selected scripts for the perl script - creates searchword for perlcalled
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Run_Button_Click(object sender, RoutedEventArgs e)
        {
            //create list of strings called graphArguments
            List<string> graphArguments = new List<string>();
            //create int called argumentCount and assign to 0
            int argumentCount = 0;
            //create list of strings called scriptsToRun
            List<string> scriptsToRun = new List<string>();
            //create int called selectcount and assign to 0 - used to ensure some items are checked
            int selectcount = 0;
            //for each items in GroupDataGrids itemssource
            foreach (GroupItem cats in GroupDataGrid.ItemsSource)
            {
                //create string scriptname and assign it to cats name column
                string scriptName = cats.gridNameColumn;
                //if checkbox is true ie checked
                if (cats.gridCheckboxColumn == true)
                {
                    //add scriptname to scriptstorun
                    scriptsToRun.Add(scriptName);
                }
                //add 1 to selectcount
                selectcount++;
            }

            //if no log file selected, error message, and return
            if (file_Name == null)
            {
                MessageBox.Show("No Log File Selected", "Data Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            //if no scripts selected AND no start time and end time selected, error message, and return
            else if (scriptsToRun.Count == 0 && startTime == null && endTime == null)
            {
                MessageBox.Show("No Scripts Selected or Dates Selected", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            //if no scripts selected AND start time is selected but end time is not, error message, and return
            else if (scriptsToRun.Count == 0 && startTime != null && endTime == null)
            {
                MessageBox.Show("Start Date has not been set!", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            //if no scripts selected AND end time is selected but start time is not, error message, and return
            else if (scriptsToRun.Count == 0 && startTime == null && endTime != null)
            {
                MessageBox.Show("End Date has not been set!", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            //if scripts are selected AND end time and start time are selected, error message, and return
            else if (scriptsToRun.Count != 0 && startTime != null && endTime != null)
            {
                MessageBox.Show("Dates and Scripts can not be searched at the same time", "Input Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            //if scripts are selected AND end time and start time are not selected
            else if (scriptsToRun.Count != 0 && startTime == null && endTime == null)
            {
                //create empty string called searchword
                string searchWord = "";
                //for all selected scripts
                foreach (string selectedScripts in scriptsToRun)
                {
                    //create a string for reading lines in the script file
                    string line;
                    //create a streamreader to read from categoryFile
                    StreamReader reader = new StreamReader(categoryFile);
                    //read file until end
                    while ((line = reader.ReadLine()) != null)
                    {
                        //create a regex using "^name :" and items
                        Regex regex = new Regex("^name :" + selectedScripts + "$");
                        //find selected group on line
                        if (regex.IsMatch(line))
                        {
                            //create empty string nextline continue reading file
                            string nextLine = "";
                            //read until "--"
                            while ((nextLine = reader.ReadLine()) != "--")
                            {
                                //create a string array called args used to store the line split by the delimiter
                                string[] args = Regex.Split(nextLine, DelimiterRead);
                                //if there are 7 split pieces
                                if (args.Length == 7)
                                {
                                    //use args to split
                                    if (args[6] == "excludefalse")
                                    {
                                        //create char called MyCharList and add special chars
                                        char[] MyCharList = { ']', '[' };
                                        //remove special characters from all pieces of args
                                        //searchstring
                                        args[0] = args[0].TrimEnd('[');
                                        //lines above
                                        args[1] = args[1].Trim(MyCharList);
                                        //lines below
                                        args[2] = args[2].Trim(MyCharList);
                                        //from
                                        args[3] = args[3].Trim(MyCharList);
                                        //to
                                        args[4] = args[4].Trim(MyCharList);
                                        //xml
                                        args[5] = args[5].Trim(MyCharList);
                                        //exclude
                                        args[6] = args[6].Trim(MyCharList);

                                        //create int called num1
                                        int num1;
                                        //create bool called res and assign value based of whether args[2] is an int
                                        bool res = int.TryParse(args[2], out num1);
                                        Match match = Regex.Match(args[0], "[?@+%!\"()]", RegexOptions.IgnoreCase);
                                        //create int called matchCheck and assign to 0
                                        int matchCheck = 0;

                                        while (match.Success)
                                        {
                                            if (matchCheck == 0)
                                            {
                                                var index = match.Index;
                                                args[0] = args[0].Insert(index, "\\");
                                                match = match.NextMatch();
                                                matchCheck++;

                                            }
                                            else if (matchCheck > 0)
                                            {
                                                var index = match.Index;
                                                args[0] = args[0].Insert((index + (1 * matchCheck)), "\\");
                                                match = match.NextMatch();
                                                matchCheck++;
                                            }

                                        }
                                        matchCheck = 0;
                                        graphArguments.Add(args[0]);
                                        graphArguments.Add(args[3]);
                                        graphArguments.Add(args[4]);

                                        Regex stringRegex = new Regex(@"[\D\d]+");

                                        //if args[2] contains "DATE"
                                        if (args[2] == "DATE")
                                        {
                                            //set searchword equal to searchword + \ + required args and type (seperated by ";!;" and "!;!")
                                            searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + args[2] + "!;!" + "type1" + "!;!" + args[5] + "\"";
                                            //add one to argument count
                                            argumentCount++;
                                        }

                                        else if (stringRegex.IsMatch(args[0]) && args[1] == "" && stringRegex.IsMatch(args[2]))
                                        {
                                            searchWord = searchWord + " \"" + args[0] + ";!;" + " " + ";!;" + args[2] + "!;!" + "type2" + "!;!" + args[5] + "\"";

                                            argumentCount++;
                                        }
                                        else if (stringRegex.IsMatch(args[0]) && stringRegex.IsMatch(args[1]) && args[2] == "")
                                        {
                                            searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + " " + "!;!" + "type2" + "!;!" + args[5] + "\"";

                                            argumentCount++;
                                        }
                                        else if (res == false)
                                        {
                                            searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + args[2] + "!;!" + "type4" + "!;!" + args[5] + "\"";

                                            argumentCount++;
                                        }
                                        else if (res == true)
                                        {
                                            searchWord = searchWord + " \"" + args[0] + ";!;" + args[1] + ";!;" + args[2] + "!;!" + "type3" + "!;!" + args[5] + "\"";

                                            argumentCount++;
                                        }


                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }
                            }
                        }
                    }

                    reader.Close();
                }
                //call perlcalled passing searchword, graph arguments and argument count
                perlCalled(searchWord, graphArguments, argumentCount);

            }
            //if scripts are not selected AND end time and start time are selected
            else if (scriptsToRun.Count == 0 && startTime != null && endTime != null && outputFileName != null && file_Name != null)
            {
                //create string called perlDateString and assign start time + "*!*" + end time
                string perlDateString = startTime + "*!*" + endTime;
                //call perlcalled passing perlDateString, graph arguments and argument count
                perlCalled(perlDateString, graphArguments, argumentCount);
                //set start time and end time to null
                startTime = null;
                endTime = null;
            }
        }

        /// <summary>
        /// Calls the perl, passing the scripts desired. Creates a thread to watch it.
        /// </summary>
        /// <param name="searchWord2">The search word2.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="argCount">The argument count.</param>
        private void perlCalled(string searchWord2, List<string> graph, int argCount)
        {
            //if exclude (in extra options) is not null
            if (Exclude != null)
            {
                //add input and output file/folder locations to searchword2 and add exclude
                searchWord2 = "\"" + file_Name + "\"" + " " + "\"" + outputFileName + "\"" + " " + searchWord2 + "@;;@" + Exclude;

            }
            else if (Exclude == null)
            {
                //add input and output file/folder locations to searchword2 - but don't add exclude
                searchWord2 = "\"" + file_Name + "\"" + " " + "\"" + outputFileName + "\"" + " " + searchWord2;
            }
            //create string processname and assign "perlprocess" and processcount
            string processname = "perlprocess" + processCount.ToString();
            //make perlprocess a new process
            perlprocess = new Process();
            //add perlprocess ot the process list
            processList.Add(perlprocess);
            //if there are less than two processes - (can be increased to allow multiple scripts to run at once)
            if (processCount < 2)
            {
                //create int called filecheck
                int fileCheck;
                //create processStartInfo called perlStartInfo and assign perl.exe to it
                ProcessStartInfo perlStartInfo = new ProcessStartInfo("perl.exe");
                //add the arguments - call perl program and send it searchWord2
                perlStartInfo.Arguments = string.Format("StringSearchWithNLines.pl" + " " + searchWord2);
                //use shell execute is false
                perlStartInfo.UseShellExecute = false;
                //redirect output is true
                perlStartInfo.RedirectStandardOutput = true;
                //redirect error is true
                perlStartInfo.RedirectStandardError = true;
                //stop console window from being created
                perlStartInfo.CreateNoWindow = true;
                //add perlStartInfor to perlprocess
                perlprocess.StartInfo = perlStartInfo;
                //if file is greater than 600 mb
                if (filesize > 600000000)
                {

                    fileCheck = ((int)filesize / 100000000);
                    // mulipies the average time it takes to parse a log file, it takes 7 seconds to parse a 100mb file, then mulipied by the number of Search Args (5) then divide by 60 to get the mins.
                    // ie a file of 200 mb and 5 args = ((2 * 7) * 5) / 60 =  1 mins
                    time = ((fileCheck * 7) * argCount) / 60;
                    timeTwo = ((fileCheck * 10) * argCount) / 60;
                    if (time == 0 && timeTwo == 0)
                    {

                        if (MessageBox.Show("This is a large file, Estimated Time of completion will be less than 2 minutes\nContinue?", "File Size Alert", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            // shows the hidden progress bar
                            progressBar.Visibility = Visibility.Visible;
                            Progress_label.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        // rounds time and timeTwo to the nears minute
                        time = Math.Ceiling(time);
                        timeTwo = Math.Ceiling(timeTwo);
                        if (MessageBox.Show("This is a large file, Estimated Time of completion will be between " + time + " to " + timeTwo + " minutes\nContinue?", "File Size Alert", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {

                            progressBar.Visibility = Visibility.Visible;
                            Progress_label.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (filesize < 600000000)
                {
                    fileCheck = ((int)filesize / 100000000);

                    time = (((fileCheck * 7) * argCount) / 60);
                    if (time == 0)
                    {
                        if (MessageBox.Show("Estimated Time of completion is less than one minute.\nContinue?", "File Size Alert", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            progressBar.Visibility = Visibility.Visible;
                            Progress_label.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            return;
                        }

                    }
                    else
                    {
                        time = Math.Ceiling(time);
                        if (MessageBox.Show("Estimated Time of completion is " + time + " minute.\nContinue?", "File Size Alert", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            progressBar.Visibility = Visibility.Visible;
                            Progress_label.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            return;
                        }

                    }


                }
                //start perlprocess
                perlprocess.Start();
                //add one to processcount
                processCount++;
                //create thread called workerThread and pass it to thread with the required graph data
                Thread workerThread = new Thread(() => threadFunction(graph));
                //start workerThread
                workerThread.Start();
            }
            //already running max number of scripts - show messagebox
            else
            {
                MessageBox.Show("You have too many scripts running, please try again when one has exited", "Script Limit Reached", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// New thread, waits for perl to end, enables cancel button, queries user after textfile creation, creates html.
        /// </summary>
        /// <param name="graphArg">The graph argument.</param>
        private void threadFunction(List<string> graphArg)
        {
            //create empty string outputPerlFile
            string outputPerlFile = "";
            //create fileinfo f
            FileInfo f;

            // Changes the stop button to Red and enabled when the run script button is active, disables the run button
            this.Dispatcher.Invoke((Action)(() =>
            {
                BtnCancel.IsEnabled = true;
                BtnCancel.Background = Brushes.Red;
                Run_Button.IsEnabled = false;
            }));

            //tell this thread to wait for the perl process to exit
            perlprocess.WaitForExit();

            try
            {
                //assign outputPerlFile to the name of the file perl has created
                outputPerlFile = perlprocess.StandardOutput.ReadToEnd();
            }
            catch (Exception)
            {
            }

            //subtract one from the processCount
            processCount--;
            //set Exclude to null
            Exclude = null;
            //if process was killed or no processes left
            if (perlprocesswaskilled == false && processCount == 0)
            {
                //changes the cancel button to disabled, hide progress bar and label and enable run button
                this.Dispatcher.Invoke((Action)(() =>
                {
                    BtnCancel.IsEnabled = false;
                    progressBar.Visibility = Visibility.Hidden;
                    Progress_label.Visibility = Visibility.Hidden;
                    Run_Button.IsEnabled = true;
                }));

                try
                {
                    //assign f to outputPerlFile
                    f = new FileInfo(outputPerlFile);
                }
                //if not, show messagebox
                catch (Exception)
                {
                    MessageBox.Show("Perl output file creation error.\nPlease contact developer.", "Output File Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;

                }

                //if the file is empty
                if (f.Length == 0)
                {
                    //show messagebox asking if user wishes to delete empty file
                    if (MessageBox.Show("Output File is empty.\n\n Do you wish to delete the output file?", "Empty Output File", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        try
                        {
                            //if yes - try to delete empty file
                            System.IO.File.Delete(outputPerlFile);
                        }
                        catch (System.IO.IOException)
                        {
                            //if not able to delete, show messagebox
                            MessageBox.Show("Unable to delete file.\nPlease try to delete the file manually.", "File Deletion", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                //if the file is not empty
                else if (MessageBox.Show("Log file created.\n\nDo you wish to create a call graph of the output file? ", "Call Graph", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    // converts the name of the file to a json format so it can be passed to the javascript
                    var PerlFileName = JsonConvert.SerializeObject(System.IO.Path.GetFileName(outputPerlFile));

                    //stores the contents of the diagramArgs file in a list and converts to a json format 
                    var logFile = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\diagramArgs.txt");
                    List<string> LogList = new List<string>(logFile);
                    var jsonDiagram = JsonConvert.SerializeObject(LogList);


                    // converts the list of the to and from strings for each main string and changes it to a json format 
                    var json = JsonConvert.SerializeObject(graphArg);



                    List<string> lineParameters = new List<string>();
                    // stores all the strings inside the diagram options window into the list lineparameters
                    foreach (string options in combochoice)
                    {
                        if (options != "No Selection")
                        {
                            lineParameters.Add(options);
                        }
                    }

                    // converts the list to a json format so it can be passed to the javascript
                    var jsonPara = JsonConvert.SerializeObject(lineParameters);

                    try
                    {
                        // 
                        using (FileStream fs = new FileStream(Directory.GetCurrentDirectory() + @"\HTMLPage2.htm", FileMode.Create))
                        {

                            // creates the html page for the call graph while also passing the json strings
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                w.WriteLine("<!DOCTYPE>");
                                w.WriteLine("<html>");
                                w.WriteLine("<head>");
                                w.WriteLine("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge,chrome=1\" />");
                                w.WriteLine("<title>Table Properties</title>");
                                w.WriteLine("<style type=\"text/css\"></style>");
                                w.WriteLine("<script src='http://code.jquery.com/jquery-latest.min.js' type='text/javascript'></script>");
                                w.WriteLine("<link href=\"Stylesheet1.css\" rel=\"stylesheet\"/>");
                                w.WriteLine("<script src=\"JavaScript1.js\"></script>");
                                w.WriteLine("</head>");
                                w.WriteLine("<body>");
                                w.WriteLine("<div id=\"wrapper\">");
                                w.WriteLine("<div class=fixed>");
                                w.WriteLine("<table id=\"myTableone\">");
                                w.WriteLine("<thead>");
                                w.WriteLine("  <tr><th style=\"height:45px; widht:10% \">Time</th></tr>");
                                w.WriteLine("  </thead>");
                                w.WriteLine("</table>");
                                w.WriteLine("</div>");
                                w.WriteLine("<div class = \"topHalfDiv\";>");
                                w.WriteLine("<table id=\"myTable\">");
                                w.WriteLine(" <thead>");
                                w.WriteLine("<tr>");
                                w.WriteLine("<th style=\"height:26px\">Time</th>");
                                w.WriteLine("</tr>");
                                w.WriteLine(" </thead>");
                                w.WriteLine("</table>");
                                w.WriteLine("</div>");
                                w.WriteLine("<div class = \"bottomHalfDiv\";>");
                                w.WriteLine("<p></p> ");
                                w.WriteLine("<label>Call Graph file " + PerlFileName + " is located at the following address " + outputFileName + "</label>");
                                w.WriteLine("<p></p> ");
                                w.WriteLine("<input type=\"file\" id=\"file-input\" />");
                                w.WriteLine("<p></p> ");
                                w.WriteLine("<textarea spellcheck=\"false\" wrap = \"off\"  class= \"text\" readonly></textarea></div>");
                                w.WriteLine("</div>");
                                w.WriteLine("</div>");
                                //w.WriteLine("<style>var city1 = " + json + ";</style>");
                                w.WriteLine("<script>tableCreate(" + json + "," + jsonPara + "," + jsonDiagram + "," + PerlFileName + ");</script>");
                                w.WriteLine("<script>document.getElementById('file-input').addEventListener('change',readSingleFile);</script>");
                                w.WriteLine("</body>");
                                w.WriteLine("</html>");
                            }
                        }

                    }

                    //if not possible, show messagebox
                    catch (Exception)
                    {
                        MessageBox.Show("HTML file missing.\n\nPlease Contact Developer", "HTML File", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        return;
                    }

                    //create streamreader to read from outputPerlFile - new file created
                    StreamReader reader = new StreamReader(outputPerlFile);
                    try
                    {
                        //load in HTMLPage2.htm
                        System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + @"\HTMLPage2.htm");
                    }
                    catch (Exception)
                    {
                        //otherwise shoe messagebox
                        MessageBox.Show("Unable to open to open browser.\n", "Browser Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }

                }

                //else if user wishes to view output folder
                else if (MessageBox.Show("Log file created.\n\nDo you wish to open the output folder? ", "Open New File Location", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    //open folder
                    Process.Start(outputFileName);
                }
            }
        }


        /// <summary>
        /// Handles the Click event of the StopButton control. Cancels the perl script running.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            //if processList contains no processes
            if (processList.Count <= 0)
            {
                //messagebox - not running any scripts - shouldn't be used as button will be disabled if no scripts running
                MessageBox.Show("You are currently not running any scripts!", "Stop Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                //if processList is not empty, ask user if they wish to stop all running scripts
                if (MessageBox.Show("Are you sure you wish to stop all scripts?", "Stop Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    //if yes, for each process in processList
                    foreach (Process process in processList)
                    {
                        //try to kill the process
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {
                        }
                    }
                    //set perlprocesswaskilled to true
                    perlprocesswaskilled = true;
                    //show messagebox informing that data in files may now be incorrect 
                    MessageBox.Show("You quit while still running scripts, the data in the files may not be correct.", "Data Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    //clear the processList
                    processList.Clear();
                    //hide the progress bar and label
                    progressBar.Visibility = Visibility.Hidden;
                    Progress_label.Visibility = Visibility.Hidden;
                    //disable cancel button
                    BtnCancel.IsEnabled = false;
                    //enable run button
                    Run_Button.IsEnabled = true;

                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Create Script Functions
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Call create new script function.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ScriptCreationButton_Click(object sender, RoutedEventArgs e)
        {
            //call CreateNewScript function
            CreateNewScript();
        }

        /// <summary>
        /// Calls new script window.
        /// </summary>
        private void CreateNewScript()
        {
            try
            {
                //try to show and activate scriptWindow
                scriptWindow.Topmost = true;
                scriptWindow.ShowDialog();
                scriptWindow.Activate();
            }
            catch (Exception)
            {
                //otherwise, create, show and activate scriptWindow
                scriptWindow = new NewScriptWindow();
                scriptWindow.Topmost = true;
                scriptWindow.ShowDialog();
                scriptWindow.Activate();
            }
            //if scriptWindow returns something, call newScriptWindow_Closed function
            if (scriptWindow.DialogResult != null)
            {
                newScriptWindow_Closed();
            }
        }

        /// <summary>
        /// when new script window is closed, reload list, and set script select combo to new script. If copy list isn't empty, call copyWholeScripts.
        /// </summary>
        void newScriptWindow_Closed()
        {
            //call listReload
            listReload();
            //create string called newscript and assign to ""
            string newscript = "";
            //reload the combobox
            ComboBox_Reload();
            //for all categories in GroupDataGrid itemssource
            foreach (GroupItem cats in GroupDataGrid.ItemsSource)
            {
                //set newscript equal to the gridNameColumn - ie list of line groups
                newscript = cats.gridNameColumn;
            }
            //set ScriptSelectCombo to null and then to newscript
            ScriptSelectCombo.SelectedValue = null;
            ScriptSelectCombo.SelectedValue = newscript;
            //if linelist is not null
            if (linelist != null)
            {
                //call copyWholeScripts window - this adds copied groups to the new script
                copyWholeScripts();
            }
        }

        /// <summary>
        /// Copies all selected scripts into a list, call create new script function.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CopyToNewScript_Click(object sender, RoutedEventArgs e)
        {
            //make linelist a new list of strings to contain lines to be transferred
            linelist = new List<string>();
            //create a list of strings called copylist
            List<string> copylist = new List<string>();
            //for all group in GroupDataGrid itemssource
            foreach (GroupItem groups in GroupDataGrid.ItemsSource)
            {
                //create string called scriptName and assign it the name of the group
                string scriptName = groups.gridNameColumn;
                //if the checkbox is checked
                if (groups.gridCheckboxColumn == true)
                {
                    //add the scriptname to copylist
                    copylist.Add(scriptName);
                }
            }
            //for all strings in copylist
            foreach (string items in copylist)
            {
                //create string called nextLine equal to ""
                string nextLine = "";
                //create a string called line for reading
                string line;
                //var fileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
                StreamReader reader = new StreamReader(categoryFile);
                //while file isn't at the end read line
                while ((line = reader.ReadLine()) != null)
                {
                    {
                        //create regex with "name :" + items
                        Regex regex = new Regex("^name :" + items + "$");
                        //if regex matches a line - ie a selected group has been found in the file
                        if (regex.IsMatch(line))
                        {
                            //while line isn't "--"
                            while ((nextLine = reader.ReadLine()) != "--")
                            {
                                //add line to linelist - add lines from selected scripts to list
                                linelist.Add(nextLine);
                            }
                        }
                    }
                }
                //close reader
                reader.Close();
            }
            //is linelist contains any lines, call createNewScript
            if (linelist.Count > 0)
            {
                CreateNewScript();
            }
            //else show messagebox - you selected no lines
            else
            {
                MessageBox.Show("You have not selected any lines", "Selection Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Copies lines from selected scripts into new script, calling duplicate window if necessary.
        /// </summary>
        private void copyWholeScripts()
        {
            //create list of strings called totalduplicates - this will contains exact matches - including from, to, type, etc.
            List<string> detsToAdd = new List<string>();
            //create list of strings called duplicates, this will contain partial duplicates - will be used to give user a choice of which to keep, searchstring will be same but from, to, type etc will be different
            List<string> duplicates = new List<string>();
            //for all strings duplicatecheck in linelist
            foreach (string duplicatecheck in linelist)
            {
                //create array of strings called duplicateOne which holds the split string
                string[] duplicateOne = duplicatecheck.Split(new[] { DelimiterWrite }, StringSplitOptions.None);
                //for all strings duplicatecheck2 in linelist
                foreach (string duplicatecheck2 in linelist)
                {
                    //create array of strings called duplicateTwo which holds the split string
                    string[] duplicateTwo = duplicatecheck2.Split(new[] { DelimiterWrite }, StringSplitOptions.None);
                    //if whole line is identical  
                    string a = duplicateOne[0];
                    string b = duplicateTwo[0];
                    if (duplicatecheck == duplicatecheck2)
                    {
                        //if not in totalduplicates
                        if (!detsToAdd.Contains(duplicatecheck))
                        {
                            //add to totalduplicates
                            detsToAdd.Add(duplicatecheck);
                        }
                    }
                    //else if searchstring equals searchstring
                    else if (duplicateOne[0] == duplicateTwo[0])
                    {
                        //if duplicatecheck is not in duplicates
                        if (!duplicates.Contains(duplicatecheck))
                        {
                            //add to duplicates
                            duplicates.Add(duplicatecheck);
                        }
                        //if duplicatecheck2 is not in duplicates
                        if (!duplicates.Contains(duplicatecheck2))
                        {
                            //add to duplicates
                            duplicates.Add(duplicatecheck2);
                        }
                    }
                }
            }
            //for all strings dups in duplicates
            foreach (string dups in duplicates)
            {
                //remove dups from totalduplicates
                detsToAdd.Remove(dups);
            }
            //if duplicates or totalduplicates contains strings
            if (duplicates.Count > 0 || detsToAdd.Count > 0)
            {
                //create list of strings called dupdone
                List<string> dupdone = new List<string>();
                //create list of list of strings called eachdup
                List<List<string>> eachdup = new List<List<string>>();
                //for all strings dups in duplicates
                foreach (string dups in duplicates)
                {
                    //create list of strings called dup
                    List<string> dup = new List<string>();
                    //create array of strings called dupsSplit and use to store split dups
                    string[] dupsSplit = dups.Split(new[] { DelimiterWrite }, StringSplitOptions.None);
                    //if dups not in dup and dups not in dupdone
                    if ((!dup.Contains(dups)) && (!dupdone.Contains(dups)))
                    {
                        //add to dup
                        dup.Add(dups);
                        //add to dupdone
                        dupdone.Add(dups);
                        //for all strings dups2 in duplicates
                        foreach (string dups2 in duplicates)
                        {
                            //create array of strings called dupsSplit2 containing split dups2
                            string[] dupsSplit2 = dups2.Split(new[] { DelimiterWrite }, StringSplitOptions.None);
                            //if single string in dupsSplit equals single string in dupsSplit2 and dups not equal to dups2 - ie same searchstring but not same elsewhere
                            if ((dupsSplit[0] == dupsSplit2[0]) && (dups != dups2))
                            {
                                //add dups2 to dup
                                dup.Add(dups2);
                                //add dups2 to dupdone
                                dupdone.Add(dups2);
                            }
                        }
                        //add dup to eachdup
                        eachdup.Add(dup);
                    }
                }

                dupStrList = new List<string>();
                //for all list of strings dupdup in eachdup
                foreach (List<string> dupStr in eachdup)
                {
                    //create string containing "copytonewscript" - used to differentiate in DuplicateOptionsWindow
                    string type = "copytonewscript";
                    //create new DuplicateOptionsWindow called dupWin and passing type, dupdup, and selected script to it
                    DuplicateOptionsWindow dupWin = new DuplicateOptionsWindow(type, dupStr, ScriptSelectCombo.SelectedItem.ToString(), import_selection);
                    //show dupwin
                    dupWin.ShowDialog();
                }

                //for each string in dupStrList - these are the return values from the duplicate options window
                foreach (string dup in dupStrList)
                {
                    //add selection to totalduplicates
                    detsToAdd.Add(dup);
                }

                //assign datalist to a new list
                datalist = new List<LineItem>();

                //create LineItem type called newdets
                LineItem det = new LineItem();
                //add "add new line" with DATE type etc to dets
                det.searchString = "Add New Line";
                det.linesAbove = "DATE";
                det.linesBelow = "DATE";
                det.selectionFrom = "";
                det.selectionTo = "";
                det.exclude = false;
                det.xml = false;
                det.fromCollection = new ObservableCollection<string>(combochoice);
                det.toCollection = new ObservableCollection<string>(combochoice);
                //add newdets to datalist
                datalist.Add(det);

                //for each string in totalduplicates
                foreach (string dets in detsToAdd)
                {
                    //create LineItem type det and add items from total duplicates
                    det = new LineItem();
                    string[] splitdet = dets.Split(new[] { DelimiterWrite }, StringSplitOptions.None);
                    det.searchString = splitdet[0];
                    det.linesAbove = splitdet[1];
                    det.linesBelow = splitdet[2];
                    det.selectionFrom = splitdet[3];
                    det.selectionTo = splitdet[4];
                    if (splitdet[5] == "xmltrue")
                    {
                        det.xml = true;
                    }
                    else
                    {
                        det.xml = false;
                    }
                    if (splitdet[6] == "excludetrue")
                    {
                        det.exclude = true;
                    }
                    else
                    {
                        det.exclude = false;
                    }
                    det.fromCollection = new ObservableCollection<string>(combochoice);
                    det.toCollection = new ObservableCollection<string>(combochoice);
                    //add det to datalist
                    datalist.Add(det);
                }
                //assign LineDataGrids itemssource to datalist
                LineDataGrid.ItemsSource = datalist;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Select Script Functions - which scripts are selected using checkboxes in ListView
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Handles the Checked event of the listcheckboxheader control. Checks all checkboxes.
        /// This checks all checkboxes in the list view.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void listcheckboxheader_Checked(object sender, RoutedEventArgs e)
        {
            //clear listAllChecked
            listAllChecked.Clear();
            //for all cats in GroupDataGrids itemssource
            foreach (GroupItem cats in GroupDataGrid.ItemsSource)
            {
                //set cats gridCheckboxColumn to true
                cats.gridCheckboxColumn = true;
                //add cats to listAllChecked
                listAllChecked.Add(cats);
            }
            //call allCheckedInList
            allCheckedInList();
        }

        /// <summary>
        /// Handles the Unchecked event of the listcheckboxheader control. Unchecks all checkboxes
        /// This unchecks all checkboxes in the list view.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void listcheckboxheader_Unchecked(object sender, RoutedEventArgs e)
        {
            //clear listAllChecked
            listAllChecked.Clear();
            //for all cats in GroupDataGrids itemssource
            foreach (GroupItem cats in GroupDataGrid.ItemsSource)
            {
                //set cats gridCheckboxColumn to false
                cats.gridCheckboxColumn = false;
                //add cats to listAllChecked
                listAllChecked.Add(cats);

            }
            //call allCheckedInList
            allCheckedInList();
        }

        /// <summary>
        /// Resets the list view after items checked to view whether checked or not.
        /// </summary>
        private void allCheckedInList()
        {
            //create list of groups called alistoflists
            List<GroupItem> listOfGroups = new List<GroupItem>();
            //for all groups in listAllChecked
            foreach (GroupItem cats in listAllChecked)
            {
                //create group called agroup
                GroupItem group = new GroupItem();
                //set groups gridNameColumn to cats gridCheckboxColumn
                group.gridNameColumn = cats.gridNameColumn;
                group.gridCheckboxColumn = cats.gridCheckboxColumn;
                //add group to listOfGroups
                listOfGroups.Add(group);
            }
            //set GroupDataGrids itemssource to listOfGroups
            GroupDataGrid.ItemsSource = listOfGroups;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Delete Script Functions
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Handles the Click event of the DeleteScriptsButton control.
        /// Check which scripts are checked and pass them to the delete function.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void DeleteScriptsButton_Click(object sender, RoutedEventArgs e)
        {
            //create list of strings called scriptToDelete
            List<string> scriptsToDelete = new List<string>();
            //for all cats in GroupDataGrid itemssource
            foreach (GroupItem cats in GroupDataGrid.ItemsSource)
            {
                //create string called scriptName equal to group name
                string scriptName = cats.gridNameColumn;
                //if cats checkbox is checked
                if (cats.gridCheckboxColumn == true)
                {
                    //add scriptName to scriptsToDelete
                    scriptsToDelete.Add(scriptName);
                }
            }
            //if no groups selected, show messagebox
            if (scriptsToDelete.Count == 0)
            {
                MessageBox.Show("No Scripts Selected", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                //if one script to delete
                if (scriptsToDelete.Count == 1)
                {
                    //check if user is sure
                    if (MessageBox.Show("Are you sure you wish to delete this script?", "Delete Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //for all groups in GroupDataGrids itemssource
                        foreach (GroupItem cats in GroupDataGrid.ItemsSource)
                        {
                            //create string called scriptName equal to cats gridNameColumn
                            string scriptName = cats.gridNameColumn;
                            //if gridcheckboxcolumn is checked
                            if (cats.gridCheckboxColumn == true)
                            {
                                //call deleteScriptsFunction and pass scriptName to it
                                deleteScriptsFunction(scriptName);
                            }
                            try
                            {
                                //if group selected for editing are to be deleted
                                if (ScriptSelectCombo.SelectedValue.ToString() == scriptName)
                                {
                                    //set scriptSelectCombo to null
                                    ScriptSelectCombo.SelectedValue = null;
                                }
                            }
                            catch (System.NullReferenceException)
                            {
                            }
                        }
                    }
                }
                //if there are more than 1 groups selected - this is the same as for 1 - just message is different
                else
                {
                    //show messagebox to ensure user wishes to delete and if so
                    if (MessageBox.Show("Are you sure you wish to delete these scripts?", "Delete Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //for all cats in GroupDataGrids itemssource
                        foreach (GroupItem cats in GroupDataGrid.ItemsSource)
                        {
                            //create string called scriptName equal to cats gridNameColumn
                            string scriptName = cats.gridNameColumn;
                            //if gridcheckboxcolumn is checked
                            if (cats.gridCheckboxColumn == true)
                            {
                                //call deleteScriptsFunction and pass scriptName to it
                                deleteScriptsFunction(scriptName);
                                try
                                {
                                    //if group selected for editing are to be deleted
                                    if (ScriptSelectCombo.SelectedValue.ToString() == scriptName)
                                    {
                                        //set scriptSelectCombo to null
                                        ScriptSelectCombo.SelectedValue = null;
                                    }
                                }
                                catch (System.NullReferenceException)
                                {
                                }
                            }
                        }
                    }
                }
            }
            //call ComboBox_Reload
            ComboBox_Reload();
        }

        /// <summary>
        /// Deletes the scripts from the scripts textfile.
        /// </summary>
        /// <param name="script">The script.</param>
        private void deleteScriptsFunction(string script)
        {
            //create a string tempfile and assign it to a temp file name 
            string tempfile = System.IO.Path.GetTempFileName();
            //create streamwriter call writer and assign to tempfile
            StreamWriter writer = new StreamWriter(tempfile);
            //create streamreader called reader and assign to categoryFile
            StreamReader reader = new StreamReader(categoryFile);
            //create string line for reading file
            string line;
            //create a regex containing "name :" + the name of the script to be deleted
            Regex regex = new Regex("^name :" + script + "$");
            //while line not at end of group file
            while ((line = reader.ReadLine()) != null)
            {
                //if line equals regex
                if (regex.IsMatch(line))
                {
                    //create string nextline
                    string nextLine;
                    //while next line is not equal to "--"
                    while ((nextLine = reader.ReadLine()) != "--")
                    {
                        //do nothing
                    }
                }
                //otherwise write line to tempfile
                else
                {
                    writer.WriteLine(line);
                }
            }
            //close writer
            writer.Close();
            //close reader
            reader.Close();
            //assign reader to tempfile
            reader = new StreamReader(tempfile);
            //assign writer to category file
            writer = new StreamWriter(categoryFile);
            //while line not at end of temp file
            while ((line = reader.ReadLine()) != null)
            {
                //write everything to category file
                writer.WriteLine(line);
            }
            //close writer
            writer.Close();
            //close reader
            reader.Close();
            //call datareload passing regex to it
            DataReload(regex);
            //call listreload
            listReload();
            //delete temp file
            File.Delete(tempfile);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Import and export scripts via text file.
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Imports scripts from a file.  Checks for duplicates and errors and writes to script text file.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            //create a list of strings called existingScriptsList
            List<string> existingScriptsList = new List<string>();
            //for all strings in scriptSelectCombos itemssource
            foreach (string existingscript in ScriptSelectCombo.ItemsSource)
            {
                //add to existingScriptsList - this will contain a list of all the existing scripts
                existingScriptsList.Add(existingscript);
            }
            //create a dialog to load in the file
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //only show text files
            dlg.Filter = "Text Files (.txt)|*.txt";
            //show dialog
            Nullable<bool> result = dlg.ShowDialog();
            //create a string call newScriptdoc set to null
            string newScriptdoc = null;
            ////if dialog returns result 
            if (result == true)
            {
                //set newScriptdoc to the selected file
                newScriptdoc = dlg.FileName;
                //create long called length and set to the size of the selected file
                long length = new System.IO.FileInfo(newScriptdoc).Length;
                //if length is greater than 3000000
                if (length > 3000000)
                {
                    //show messagebox saying file is too large and return
                    MessageBox.Show("File selected is to large to import!\n\n Please check that you have selected the right file.", "Import File", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                //if length greater than 300000
                else if (length > 300000)
                {
                    //show messagebox and if yes
                    if (MessageBox.Show("Selected File size appers to be to large.\n\nAre you sure you wish to import this file?", "Import File", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //do nothing
                    }
                    //if no
                    else
                    {
                        //return
                        return;
                    }
                }
            }
            //if newScriptdoc is not null
            if (newScriptdoc != null)
            {
                //create string called filecheck and set it to newScriptdocs extension
                string fileCheck = System.IO.Path.GetExtension(newScriptdoc);
                //if filecheck equals ".txt"
                if (fileCheck != ".txt")
                {
                    //show messagebox - invalid file format and return
                    MessageBox.Show("Invalid file format.\nPlease use file types with the extension .txt.", "Invalid file", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    //create var called logFile and set to read all lines of newScriptdoc
                    var logFile = File.ReadAllLines(newScriptdoc);
                    //create list of strings called LogList and assign to logFile
                    List<string> LogList = new List<string>(logFile);
                    //if LogList contains "name :"
                    if (LogList.Any(s => s.Contains("name :")))
                    {
                        //create list of strings called imports
                        List<string> imports = new List<string>();
                        //create streamreader called reader and assign to newScriptdoc
                        StreamReader reader = new StreamReader(newScriptdoc);
                        //create streamreader called reader and assign to category file
                        StreamWriter writer = File.AppendText(categoryFile);
                        //create string called line
                        string line;
                        //while not at end of newScriptdoc
                        while ((line = reader.ReadLine()) != null)
                        {
                            //add line to imports
                            imports.Add(line);
                        }
                        //close reader
                        reader.Close();
                        //for all lines in imports
                        foreach (string lines in imports)
                        {
                            //create string called nameReturned and set to ""
                            string nameReturned = "";
                            //if line contains "name :"
                            if (lines.Contains("name :"))
                            {
                                //create string array called newscriptname holding lines split by ":"
                                string[] newscriptname = lines.Split(':');
                                //if existingScriptsList contains the name of scripts being imported
                                if (existingScriptsList.Contains(newscriptname[1]))
                                {
                                    //create a window of type ImportDuplicateNameChange called dupWin, passing newscriptname and exitstingScriptsList
                                    ImportDuplicateNameChange dupWin = new ImportDuplicateNameChange(newscriptname[1], existingScriptsList);
                                    //if dupWin is false
                                    if (dupWin.ShowDialog() == false)
                                    {
                                        //set nameReturned to dupWins returnfunction
                                        nameReturned = dupWin.returnfunction;
                                        //if name already exists
                                        if (existingScriptsList.Contains(nameReturned))
                                        {
                                            //continue
                                            continue;
                                        }
                                        //if nameReturned is "SCRIPTHASBEENCANCELLED"
                                        else if (nameReturned == "SCRIPTHASBEENCANCELLED")
                                        {
                                            //continue
                                            continue;
                                        }
                                        else
                                        {
                                            //otherwise write "name :" + nameReturned to category file
                                            writer.WriteLine("name :" + nameReturned);
                                        }
                                    }
                                }
                                else
                                {
                                    //otherwise, write lines to category file and set nameReturned to ""
                                    writer.WriteLine(lines);
                                    nameReturned = "";
                                }
                            }
                            //else if nameReturned is "SCRIPTHASBEENCANCELLED" - continue
                            else if (nameReturned == "SCRIPTHASBEENCANCELLED")
                            {
                                continue;
                            }
                            else
                            {
                                //write to category file
                                writer.WriteLine(lines);
                            }
                        }
                        //close writer
                        writer.Close();
                        //call listReload
                        listReload();
                        //call comboBox_Reload
                        ComboBox_Reload();

                    }
                    else
                    {
                        //otherwise, show messagebox - incorrect format and return
                        MessageBox.Show("Imported file does not match format required for this application. ", "Incorrect format", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;

                    }
                }

            }
        }

        /// <summary>
        /// Writes selected scripts to a file which can then be used to import.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            //create a list of strings call exports
            List<string> exports = new List<string>();
            //for all cats it GroupDataGrids itemssource
            foreach (GroupItem cats in GroupDataGrid.ItemsSource)
            {
                //if cats checbox is checked
                if (cats.gridCheckboxColumn == true)
                {
                    //add "name :" + cats name to exports
                    exports.Add("name :" + cats.gridNameColumn);
                }
            }
            //create a string tempfile and assign it to a temp file name 
            string tempfile = System.IO.Path.GetTempFileName();
            //create streamwriter call writer and assign to tempfile
            StreamWriter writer = new StreamWriter(tempfile);
            //create streamreader called reader and assign to categoryFile
            StreamReader reader = new StreamReader(categoryFile);
            //create string line for reading file
            string line;
            //while line not at end of group file
            while ((line = reader.ReadLine()) != null)
            {
                //if line is in exports - ie found one of the groups wanted to write to export file
                if (exports.Contains(line))
                {
                    //create string array called getScriptName and split line on ":"
                    string[] getScriptName = line.Split(':');
                    //write "name :" and getScriptName[1]
                    writer.WriteLine("name :" + getScriptName[1]);
                    //while line not equal to "--" - end of group
                    while ((line = reader.ReadLine()) != "--")
                    {
                        //write to temp file
                        writer.WriteLine(line);
                    }
                    //write "--"
                    writer.WriteLine("--");
                }
            }
            //close writer
            writer.Close();
            //close reader
            reader.Close();
            //assign reader to tempfile
            reader = new StreamReader(tempfile);
            //if no scripts selected
            if (exports.Count == 0)
            {
                //display messagebox and return
                MessageBox.Show("Please select a script to export!", "Script Selection", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            //create a SaveFileDialog dialog called dlg
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //set default filename to be Scripts
            dlg.FileName = "Scripts";
            //set default file extensioin to .txt
            dlg.DefaultExt = ".text";
            //filter files by extension
            dlg.Filter = "Text documents (.txt)|*.txt";
            // Show dlg and create nullable bool result to check for result
            Nullable<bool> result = dlg.ShowDialog();
            //if result is true
            if (result == true)
            {
                //create string called filename set to dlgs filename
                string filename = dlg.FileName;
                //set writer to this filename
                writer = new StreamWriter(filename);
                //while not at the end of the tempfile
                while ((line = reader.ReadLine()) != null)
                {
                    //write line to new file
                    writer.WriteLine(line);
                }
            }
            //close writer
            writer.Close();
            //close reader
            reader.Close();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Delete Line Function
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Check  if there are lines to be deleted, if so these are placed in a list and passed to the delete lines function.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void deleteMultipleLinesButton_Click(object sender, RoutedEventArgs e)
        {
            //if datagrid is empty
            if (LineDataGrid.Items.Count == 0)
            {
                //show messagebox and return
                MessageBox.Show("No Script Selected.", "Deletion Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            else
            {
                //create new list of LineItem called newItemssource
                List<LineItem> newItemssource = new List<LineItem>();
                //create new list of LineItem called deleteLinesList
                List<LineItem> deleteLinesList = new List<LineItem>();
                //get enumerator
                LineDataGrid.ItemsSource.GetEnumerator();
                //for all items in LineDataGrid itemssource
                datalist = new List<LineItem>();
                int count = 0;
                foreach (LineItem dets in LineDataGrid.ItemsSource)
                {
                    count++;
                    //create string called stringCheck and assign to dets searchString
                    string stringCheck = dets.searchString;
                    //if stringCheck is not "Add New Line"
                    if (stringCheck != "Add New Line")
                    {
                        //if dets checkbox is unchecked - ie not to be deleted
                        if (dets.gridCheckbox == false)
                        {
                            //add dets to datalist
                            datalist.Add(dets);
                        }
                    }
                    //else if stringCheck is "Add New Lne"
                    else
                    {
                        //add to datalist
                        datalist.Add(dets);
                    }
                }
                //set LineDataGrids ItemsSource to datalist
                LineDataGrid.ItemsSource = datalist;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Copy and paste functions
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Checks if there are lines selected and copies these to a list until they are pasted.  Enables the paste button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void copyMultipleLinesButton_Click(object sender, RoutedEventArgs e)
        {
            //assign copyLinesList to a new list of LineItem
            copyLinesList = new List<LineItem>();
            //if LineDataGrid count is 0 - ie data not loaded
            if (LineDataGrid.Items.Count == 0)
            {
                //show messagebox and return
                MessageBox.Show("No Script Selected.", "Copy Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            //count number of lines seleted
            int countselected = 0;
            //for all LineItem in LineDataGrids ItemsSource
            foreach (LineItem dets in LineDataGrid.ItemsSource)
            {
                //create string called stringCheck and set to dets searchstring
                string stringCheck = dets.searchString;
                //if dets checkbox is checked
                if (dets.gridCheckbox == true)
                {
                    //if stringCheck is not "Add New Line"
                    if (stringCheck != "Add New Line")
                    {
                        //add 1 to countselected
                        countselected++;
                        //add dets to copyLinesList
                        copyLinesList.Add(dets);
                    }
                }
            }
            //if no lines selected
            if (countselected == 0)
            {
                //show messagebox and return
                MessageBox.Show("No Lines Selected.", "Copy Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            //enable the pastebutton
            pasteButton.IsEnabled = IsEnabled;
        }

        /// <summary>
        /// Pastes copied lines into selected script, passes duplicates to the duplicate choice window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void pasteButton_Click(object sender, RoutedEventArgs e)
        {
            dupStrList = new List<string>();
            //create list of strings called copyStrList
            List<string> copyStrList = new List<string>();
            //for each dets in copyLinesList - propogated in copy function
            foreach (LineItem dets in copyLinesList)
            {
                //if xml is true
                if (dets.xml == true)
                {
                    //if exclude is true
                    if (dets.exclude == true)
                    {
                        //create string called copyStr and add all dets items split by delimeter, replacing exclude and xml for strings
                        string copyStr = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludetrue";
                        //add to copyStrList
                        copyStrList.Add(copyStr);
                    }
                    //if exclude is false
                    else
                    {
                        //create string called copyStr and add all dets items split by delimeter, replacing exclude and xml for strings
                        string copyStr = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludefalse";
                        //add to copyStrList
                        copyStrList.Add(copyStr);
                    }
                }
                //if xml is false
                else
                {
                    //if exclude is true
                    if (dets.exclude == true)
                    {
                        //create string called copyStr and add all dets items split by delimeter, replacing exclude and xml for strings
                        string copyStr = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludetrue";
                        //add to copyStrList
                        copyStrList.Add(copyStr);
                    }
                    //if exclude is false
                    else
                    {
                        //create string called copyStr and add all dets items split by delimeter, replacing exclude and xml for strings
                        string copyStr = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludefalse";
                        //add to copyStrList
                        copyStrList.Add(copyStr);
                    }
                }
            }
            //create list called currentStrList
            List<string> currentStrList = new List<string>();
            //for each dets in datalist - LineDataGrids ItemsSource
            foreach (LineItem dets in datalist)
            {
                //if xml is true
                if (dets.xml == true)
                {
                    //if exclude is true
                    if (dets.exclude == true)
                    {
                        //create string called copyStr and add all dets items split by delimeter, replacing exclude and xml for strings
                        string copyStr = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludetrue";
                        //add to currentStrList
                        currentStrList.Add(copyStr);
                    }
                    //if exclude is false
                    else
                    {
                        //create string called copyStr and add all dets items split by delimeter, replacing exclude and xml for strings
                        string copyStr = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmltrue" + DelimiterWrite + "excludefalse";
                        //add to currentStrList
                        currentStrList.Add(copyStr);
                    }
                }
                //if xml is false
                else
                {
                    //if exclude is true
                    if (dets.exclude == true)
                    {
                        //create string called copyStr and add all dets items split by delimeter, replacing exclude and xml for strings
                        string copyStr = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludetrue";
                        //add to currentStrList
                        currentStrList.Add(copyStr);
                    }
                    //if exclude is false
                    else
                    {
                        //create string called copyStr and add all dets items split by delimeter, replacing exclude and xml for strings
                        string copyStr = dets.searchString + DelimiterWrite + dets.linesAbove + DelimiterWrite + dets.linesBelow + DelimiterWrite + dets.selectionFrom + DelimiterWrite + dets.selectionTo + DelimiterWrite + "xmlfalse" + DelimiterWrite + "excludefalse";
                        //add to currentStrList
                        currentStrList.Add(copyStr);
                    }
                }
            }


            ///ignore any identical duplicates
            //for each currentdet in currentStrList
            foreach (string currentdet in currentStrList)
            {
                //if copyStrList contains currentdat
                if (copyStrList.Contains(currentdet))
                {
                    //remove currentdet from copyStrList as it is already in currentdet and is identical
                    copyStrList.Remove(currentdet);
                }
            }

            //create list of strings called partialDups
            List<string> partialDups = new List<string>();
            //for each currentDet in currentStrList
            foreach (string currentDet in currentStrList)
            {
                //create string array called currentStrDet containing currentDet split by delimeter
                //string[] currentStrDet = currentDet.Split(new[] { DelimiterRead }, StringSplitOptions.None);
                string[] currentStrDet = currentDet.Split(new[] { DelimiterWrite }, StringSplitOptions.None);

                //create string called strDetCurrent set to currentStrDets first entry
                string strDetCurrent = currentStrDet[0];
                //for each copyDet in copyStrList
                foreach (string copyDet in copyStrList)
                {
                    //create string array called copyStrDet containing copyDet split by delimeter
                    string[] copyStrDet = copyDet.Split(new[] { DelimiterWrite }, StringSplitOptions.None);
                    //create string called strDetCopy set to copyStrDets first entry
                    string strDetCopy = copyStrDet[0];
                    //if strDetCurrent equals strDetCopy - ie the searchstring matches
                    if (strDetCurrent == strDetCopy)
                    {
                        //add currentDet to partialDups
                        partialDups.Add(currentDet);
                        //add copyDet to partiaDups
                        partialDups.Add(copyDet);
                        //create a list of strings called partDups
                        List<string> partDups = new List<string>();
                        //add currentDet to partDups
                        partDups.Add(currentDet);
                        //add copyDet to partDups
                        partDups.Add(copyDet);
                        //create string called type containing "copytoexistingscript"
                        string type = "copytoexistingscript";

                        //create DuplicateOptionsWindow called dupWin and pass it type, partDups, and the selected group
                        DuplicateOptionsWindow dupWin = new DuplicateOptionsWindow(type, partDups, ScriptSelectCombo.SelectedItem.ToString(), import_selection);
                        dupWin.Owner = this;
                        //show dupwin
                        dupWin.ShowDialog();
                        //dupWin.Check += value => label.Content = value;
                        //if dupwin result isn't null
                        if (dupWin.DialogResult != null)
                        {
                            //create regex and assign to "name :" + selected script
                            Regex regex = new Regex("^name :" + ScriptSelectCombo.SelectedValue + "$");
                            //call DataReload and pass regex to it
                            DataReload(regex);
                        }
                    }
                }
            }

            //foreach string detStr in partialDups
            foreach (string detStr in partialDups)
            {
                //add partDup to copyStrList
                copyStrList.Remove(detStr);
                currentStrList.Remove(detStr);
            }
            //for each detStr in currentStrList
            foreach (string detStr in currentStrList)
            {
                //if not "Add New Line"
                if (!detStr.Contains("Add New Line"))
                {
                    //add detStr to copyStrList
                    copyStrList.Add(detStr);
                }
            }
            //foreach detStr in dupStrList
            foreach (string detStr in dupStrList)
            {
                //add detStr to copyStrList
                copyStrList.Add(detStr);
            }
            //assign datalist to a new list of LineItem
            datalist = new List<LineItem>();
            //create LineItem newdets
            LineItem newdets = new LineItem();
            //add "add new line" with DATE type etc to newdets
            newdets.searchString = "Add New Line";
            newdets.linesAbove = "DATE";
            newdets.linesBelow = "DATE";
            newdets.selectionFrom = "";
            newdets.selectionTo = "";
            newdets.exclude = false;
            newdets.xml = false;
            newdets.fromCollection = new ObservableCollection<string>(combochoice);
            newdets.toCollection = new ObservableCollection<string>(combochoice);
            //add dets to datalist
            datalist.Add(newdets);

            //for each detStr in copyStrList
            foreach (string detStr in copyStrList)
            {
                //assign new dets to new LineItem
                newdets = new LineItem();
                //create string array called splitdet and assign it to detStr split by {;}
                string[] splitdet = detStr.Split(new[] { DelimiterWrite }, StringSplitOptions.None);
                //assign data from splitdet to newdets
                newdets.searchString = splitdet[0];
                newdets.linesAbove = splitdet[1];
                newdets.linesBelow = splitdet[2];
                newdets.selectionFrom = splitdet[3];
                newdets.selectionTo = splitdet[4];
                //if xmltrue assign xml to true, else to false
                if (splitdet[5] == "xmltrue")
                {
                    newdets.xml = true;
                }
                else
                {
                    newdets.xml = false;
                }
                //if excludetrue assign exclude to true, else to false
                if (splitdet[6] == "excludetrue")
                {
                    newdets.exclude = true;
                }
                else
                {
                    newdets.exclude = false;
                }
                newdets.fromCollection = new ObservableCollection<string>(combochoice);
                newdets.toCollection = new ObservableCollection<string>(combochoice);
                //add newdets to datalist
                datalist.Add(newdets);
            }
            //assign LineDataGrids itemsSource to datalist
            LineDataGrid.ItemsSource = datalist;
            //set copyLinesList to null
            copyLinesList = null;
            //disable the paste button
            pasteButton.IsEnabled = false;
        }

        /// <summary>
        /// Import_selections the specified s. Takes returns from DuplicateOptionsWindow and adds to dupStrList
        /// </summary>
        /// <param name="chosenDup">The s.</param>
        private void import_selection(string chosenDup)
        {
            //add returned string (chosen duplicate) to dupStrList
            dupStrList.Add(chosenDup);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Select data Functions - which lines are selected using checkboxes in DataGrid
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Handles the Checked event of the datacheckboxheader control.  Checks all checkboxes in data grid (on right)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void datacheckboxheader_Checked(object sender, RoutedEventArgs e)
        {
            //clear dataAllChecked;
            dataAllChecked.Clear();
            //for each LineItem tocheck in LineDataGrids itemssource
            foreach (LineItem tocheck in LineDataGrid.ItemsSource)
            {
                //set tochecks checkbox to checked
                tocheck.gridCheckbox = true;
                //add tocheck to dataAllChecked
                dataAllChecked.Add(tocheck);
            }
            //call allCheckedInData function
            allCheckedInData();
        }

        /// <summary>
        /// Handles the Unchecked event of the datacheckboxheader control. Unchecks all checkboxes in data grid (on right)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void datacheckboxheader_Unchecked(object sender, RoutedEventArgs e)
        {
            //clear dataAllChecked;
            dataAllChecked.Clear();
            //for each LineItem tocheck in LineDataGrids itemssource
            foreach (LineItem touncheck in LineDataGrid.ItemsSource)
            {
                //set tochecks checkbox to unchecked
                touncheck.gridCheckbox = false;
                //add tocheck to dataAllChecked
                dataAllChecked.Add(touncheck);
            }
            //call allCheckedInData function
            allCheckedInData();

        }

        /// <summary>
        /// Resets the data grid view after checked/unchecked
        /// </summary>
        private void allCheckedInData()
        {
            //create a list of LineItem called listofDets
            List<LineItem> listofDets = new List<LineItem>();
            //foreach LineItem checkdets in dataAllChecked
            foreach (LineItem checkdets in dataAllChecked)
            {
                //add checkdets to listofDets
                listofDets.Add(checkdets);
            }
            //assign LineDataGridsitemssource to listofDets
            LineDataGrid.ItemsSource = listofDets;
        }

    }

}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////END////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////