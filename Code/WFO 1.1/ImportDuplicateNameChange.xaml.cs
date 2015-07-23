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
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for ImportDuplicateNameChange.xaml
    /// </summary>
    public partial class ImportDuplicateNameChange : Window
    {
        string dupName;
        List<string> existingScripts = new List<string>();
        public ImportDuplicateNameChange(string _dupName, List<string> _existingScripts)
        {
            InitializeComponent();
            dupName = _dupName;
            existingScripts = _existingScripts;
        }

       
        private void duplicateScriptRenameTextbox_Loaded(object sender, RoutedEventArgs e)
        {
            duplicateScriptRenameTextbox.Text = dupName;
            duplicateScriptRenameTextbox.Focus();
        }

        private void saveNewScriptnameButton_Click(object sender, RoutedEventArgs e)
        {
            if (existingScripts.Contains(duplicateScriptRenameTextbox.Text))
            {
                MessageBox.Show("A script with this name already exists.", "Invalid Script Name", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {

                //string newName = duplicateScriptRenameTextbox.Text;
                //RenameReturn = newName;
                ImportDuplicateNameChangeWindow.Close();
            }
        }

        public string returnfunction
        {
            get { return duplicateScriptRenameTextbox.Text; }
        }

        private void CancelImportButton_Click(object sender, RoutedEventArgs e)
        {
            ImportDuplicateNameChangeWindow.Close();
        }
    }
}
