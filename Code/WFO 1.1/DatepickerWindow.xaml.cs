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

namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        string startDateValue;
        string endDateValue;
        public Window1()
        {
            InitializeComponent();
        }

        private void Date_picker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
            startDateValue = e.NewValue.ToString();
        }

        private void Date_pickertwo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
             endDateValue = e.NewValue.ToString();
        }

        public string startOne
        {
            set { startDateValue = startDateValue; }
            get { return startDateValue; }
            
        }

        public  string endOne
        {
            set { endDateValue = endDateValue; }
            get { return endDateValue; }
           

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Windowpopup.Close();

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Date_picker.IsEnabled = true;
            Date_pickertwo.IsEnabled = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Date_picker.IsEnabled = false;
            Date_pickertwo.IsEnabled = false;

        }

        private void ExcludeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            ExcludeTxtBox.IsEnabled = true;
        }

        private void ExcludeCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            ExcludeTxtBox.IsEnabled = false;
        }

        private void SwapCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            SwapTxtBox.IsEnabled = true;
        }

        private void SwapCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            SwapTxtBox.IsEnabled = false;
        }

    }
}
