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
        string Delimiter = "[;]";
        string type;
        private Action<string> callback;
        string choiceStr;
        /// <summary>
        /// Class that stores singlestring, linesabove, linesbelow, xml, selectionone, selectiontwo, and exclude.
        /// </summary>
        public class Details
        {
            /// <summary>
            /// Gets or sets the single string.
            /// </summary>
            /// <value>
            /// The single string.
            /// </value>
            public string singleString { get; set; }
            /// <summary>
            /// Gets or sets the lines above.
            /// </summary>
            /// <value>
            /// The lines above.
            /// </value>
            public string linesAbove { get; set; }
            /// <summary>
            /// Gets or sets the lines below.
            /// </summary>
            /// <value>
            /// The lines below.
            /// </value>
            public string linesBelow { get; set; }
            /// <summary>
            /// Gets or sets the XML.
            /// </summary>
            /// <value>
            /// The XML.
            /// </value>
            public string xml { get; set; }
            /// <summary>
            /// Gets or sets the selection one.
            /// </summary>
            /// <value>
            /// The selection one.
            /// </value>
            public string selectionOne { get; set; }
            /// <summary>
            /// Gets or sets the selection two.
            /// </summary>
            /// <value>
            /// The selection two.
            /// </value>
            public string selectionTwo { get; set; }
            /// <summary>
            /// Gets or sets the exclude.
            /// </summary>
            /// <value>
            /// The exclude.
            /// </value>
            public string exclude { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateOptionsWindow"/> class. Imports type, duplicates and selected script from mainwindow
        /// </summary>
        /// <param name="_type">The _type.</param>
        /// <param name="_duplicates">The _duplicates.</param>
        /// <param name="_selectedScript">The _selected script.</param>
        public DuplicateOptionsWindow(string _type, List<string> _duplicates, string _selectedScript, Action<string> action)
        {
            InitializeComponent();
            duplicates = _duplicates;
            selectedScript = _selectedScript;
            type = _type;

            //Closed += duplicatesWindow_Closing;
            callback = action;

        }


        //public event Action<string> Check;

        /// <summary>
        /// Handles the Closing event of the duplicatesWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void duplicatesWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (selectionmade == false)
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
            callback(choiceStr);

            //Check("apple");
        }

        //checks if selection has been made
        bool selectionmade = false;
        //used to store the details of the chosen item
        Details chosen;

        /// <summary>
        /// Handles the SelectionChanged event of the DupsDataGrid control.  Selection changed means a duplicate has been selected to keep.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void DupsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionmade = true;
            chosen = new Details();
            chosen = null;
            foreach (Details choices in DupsDataGrid.ItemsSource)
            {
                if (DupsDataGrid.SelectedItem == choices)
                {
                    chosen = choices;
                }
            }

            if (chosen.xml == "True")
            {
                if (chosen.exclude == "True")
                {
                    choiceStr = (chosen.singleString + Delimiter + chosen.linesAbove + Delimiter + chosen.linesBelow + Delimiter + chosen.selectionOne + Delimiter + chosen.selectionTwo + Delimiter + "xmltrue" + Delimiter + "excludetrue");
                }
                else
                {
                    choiceStr = (chosen.singleString + Delimiter + chosen.linesAbove + Delimiter + chosen.linesBelow + Delimiter + chosen.selectionOne + Delimiter + chosen.selectionTwo + Delimiter + "xmltrue" + Delimiter + "excludefalse");
                }
            }
            else
            {
                if (chosen.exclude == "True")
                {
                    choiceStr = (chosen.singleString + Delimiter + chosen.linesAbove + Delimiter + chosen.linesBelow + Delimiter + chosen.selectionOne + Delimiter + chosen.selectionTwo + Delimiter + "xmlfalse" + Delimiter + "excludetrue");
                }
                else
                {
                    choiceStr = (chosen.singleString + Delimiter + chosen.linesAbove + Delimiter + chosen.linesBelow + Delimiter + chosen.selectionOne + Delimiter + chosen.selectionTwo + Delimiter + "xmlfalse" + Delimiter + "excludefalse");
                }
            }



            //var scriptFileName = Directory.GetCurrentDirectory() + @"\ListViewScriptsTwo.txt";
            //StreamReader pasteReader = new StreamReader(scriptFileName);
            //string tempfile = System.IO.Path.GetTempFileName();
            //StreamWriter pasteWriter = new StreamWriter(tempfile);
            //string scriptLine;

            //if (type == "copytoexistingscript")
            //{
            //    while ((scriptLine = pasteReader.ReadLine()) != null)
            //    {
            //        //pasteWriter.WriteLine(scriptLine);
            //        if (scriptLine == "name :" + selectedScript)
            //        {
            //            string scriptline2;
            //            while ((scriptline2 = pasteReader.ReadLine()) != "--")
            //            {
            //                if (scriptline2.Contains(chosen.singleString))
            //                {
            //                    if (chosen.xml == "True")
            //                    {
            //                        if (chosen.exclude == "True")
            //                        {
            //                            choiceStr = (chosen.singleString + Delimiter + chosen.linesAbove + Delimiter + chosen.linesBelow + Delimiter + chosen.selectionOne + Delimiter + chosen.selectionTwo + Delimiter + "xmltrue" + Delimiter + "excludetrue");
            //                            pasteWriter.WriteLine(choiceStr);
            //                        }
            //                        else
            //                        {
            //                            choiceStr = (chosen.singleString + Delimiter + chosen.linesAbove + Delimiter + chosen.linesBelow + Delimiter + chosen.selectionOne + Delimiter + chosen.selectionTwo + Delimiter + "xmltrue" + Delimiter + "excludefalse");
            //                            pasteWriter.WriteLine(choiceStr);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (chosen.exclude == "True")
            //                        {
            //                            choiceStr = (chosen.singleString + Delimiter + chosen.linesAbove + Delimiter + chosen.linesBelow + Delimiter + chosen.selectionOne + Delimiter + chosen.selectionTwo + Delimiter + "xmlfalse" + Delimiter + "excludetrue");
            //                            pasteWriter.WriteLine(choiceStr);
            //                        }
            //                        else
            //                        {
            //                            choiceStr = (chosen.singleString + Delimiter + chosen.linesAbove + Delimiter + chosen.linesBelow + Delimiter + chosen.selectionOne + Delimiter + chosen.selectionTwo + Delimiter + "xmlfalse" + Delimiter + "excludefalse");
            //                            pasteWriter.WriteLine(choiceStr);
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    pasteWriter.WriteLine(scriptline2);
            //                }
            //            }
            //            pasteWriter.WriteLine("--");
            //        }
            //    }
            //}
            //else
            //{
            //    while ((scriptLine = pasteReader.ReadLine()) != null)
            //    {
            //        pasteWriter.WriteLine(scriptLine);
            //        if (scriptLine == "name :" + selectedScript)
            //        {
            //            string scriptline2;
            //            while ((scriptline2 = pasteReader.ReadLine()) != "--")
            //            {
            //                pasteWriter.WriteLine(scriptline2);
            //                if (chosen != null)
            //                {
            //                    if (chosen.xml == "True")
            //                    {
            //                        if (chosen.exclude == "True")
            //                        {
            //                            pasteWriter.WriteLine("{0}{7}{1}{7}{2}{7}{3}{7}{4}{7}{5}{7}{6}", chosen.singleString, chosen.linesAbove, chosen.linesBelow, chosen.selectionOne, chosen.selectionTwo, "xmltrue", "excludetrue", Delimiter);
            //                            chosen = null;
            //                        }
            //                        else
            //                        {
            //                            pasteWriter.WriteLine("{0}{7}{1}{7}{2}{7}{3}{7}{4}{7}{5}{7}{6}", chosen.singleString, chosen.linesAbove, chosen.linesBelow, chosen.selectionOne, chosen.selectionTwo, "xmltrue", "excludefalse", Delimiter);
            //                            chosen = null;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (chosen.exclude == "True")
            //                        {
            //                            pasteWriter.WriteLine("{0}{7}{1}{7}{2}{7}{3}{7}{4}{7}{5}{7}{6}", chosen.singleString, chosen.linesAbove, chosen.linesBelow, chosen.selectionOne, chosen.selectionTwo, "xmlfalse", "excludetrue", Delimiter);
            //                            chosen = null;
            //                        }
            //                        else
            //                        {
            //                            pasteWriter.WriteLine("{0}{7}{1}{7}{2}{7}{3}{7}{4}{7}{5}{7}{6}", chosen.singleString, chosen.linesAbove, chosen.linesBelow, chosen.selectionOne, chosen.selectionTwo, "xmlfalse", "excludefalse", Delimiter);
            //                            chosen = null;
            //                        }
            //                    }
            //                }
            //            }
            //            pasteWriter.WriteLine("--");
            //        }
            //    }
            //}
            //pasteReader.Close();
            //pasteWriter.Close();
            //pasteReader = new StreamReader(tempfile);
            //pasteWriter = new StreamWriter(scriptFileName);

            //while ((scriptLine = pasteReader.ReadLine()) != null)
            //{
            //    pasteWriter.WriteLine(scriptLine);

            //}
            //pasteWriter.Close();
            //pasteReader.Close();
            duplicatesWindow.Close();


        }

        /// <summary>
        /// Handles the Loaded event of the DupsDataGrid control.  Places the dup options in the datagrid for selection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DupsDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DupsDataGrid.Columns[0].Visibility = Visibility.Visible;
            DupsDataGrid.Columns[1].Visibility = Visibility.Visible;
            DupsDataGrid.Columns[2].Visibility = Visibility.Visible;
            DupsDataGrid.Columns[3].Visibility = Visibility.Visible;
            DupsDataGrid.Columns[4].Visibility = Visibility.Visible;
            DupsDataGrid.Columns[5].Visibility = Visibility.Visible;
            DupsDataGrid.Columns[6].Visibility = Visibility.Visible;


            List<Details> detsList = new List<Details>();
            foreach (string dups in duplicates)
            {
                Console.WriteLine(dups);
                Details dets = new Details();
                string[] dupsSplit = dups.Split(new[] { Delimiter }, StringSplitOptions.None);
                dets.singleString = dupsSplit[0];
                dets.linesAbove = dupsSplit[1];
                dets.linesBelow = dupsSplit[2];
                dets.selectionOne = dupsSplit[3];
                dets.selectionTwo = dupsSplit[4];
                dets.xml = dupsSplit[5];
                dets.exclude = dupsSplit[6];
                detsList.Add(dets);

            }
            DupsDataGrid.ItemsSource = null;
            DupsDataGrid.ItemsSource = detsList;
        }
    }
}