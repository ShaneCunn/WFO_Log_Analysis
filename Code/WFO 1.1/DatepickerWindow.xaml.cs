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
        string Hex;
        string Exclude;
        string[] startdatetime;
        string[] enddatetime;
        int monthInDigit;
        public Window1()
        {
            InitializeComponent();
        }

        private void Date_picker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
            startDateValue = e.NewValue.ToString();
            try
            {
                 string starttime = startDateValue.Split('-')[1];
                 starttime = starttime.Split('-')[0];
                 monthInDigit = DateTime.ParseExact(starttime, "MMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                 startDateValue = startDateValue.Replace("-" + starttime, "-" + monthInDigit.ToString());                
                 starttime = startDateValue.Split(' ')[0];
                 startdatetime = starttime.Split('-');
                 starttime = startDateValue.Split(' ')[1];
                 if (startdatetime[0].Length == 4)
                 {
                     startDateValue = startdatetime[1] + "/" + startdatetime[2] + "/" + startdatetime[0] + " " + starttime;
                 }
                 else
                 {
                     startDateValue = startdatetime[1] + "/" + startdatetime[0] + "/" + startdatetime[2] + " " + starttime;  
                 }

            }
            catch (Exception)
            {
                return;
            }
        }

        private void Date_pickertwo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
             endDateValue = e.NewValue.ToString();

             try
             {                 
                 string endtime = endDateValue.Split('-')[1];
                 endtime = endtime.Split('-')[0];
                 monthInDigit = DateTime.ParseExact(endtime, "MMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                 endDateValue = endDateValue.Replace(endtime, monthInDigit.ToString());
                 endtime = endDateValue.Split(' ')[0];
                 enddatetime = endtime.Split('-');
                 endtime = endDateValue.Split(' ')[1];
                 if (enddatetime[0].Length == 4)
                 {
                     endDateValue = enddatetime[1] + "/" + enddatetime[2] + "/" + enddatetime[0] + " " + endtime;
                 }
                 else
                 {
                     endDateValue = enddatetime[1] + "/" + enddatetime[0] + "/" + enddatetime[2] + " " + endtime;
                 }
             }
             catch (Exception)
             {
                 return;                 
             }
        }     


        
            
        private void Button_Click(object sender, RoutedEventArgs e)
        {


            if (endDateValue != null && startDateValue != null)
            {
                startdatetime = startDateValue.Split(' ');
                string[] startdate = startdatetime[0].Split('/');
                Console.WriteLine(startdatetime);
                string[] starttime = startdatetime[1].Split(':');

            DateTime date1 = new DateTime(Convert.ToInt32(startdate[2]), Convert.ToInt32(startdate[0]), Convert.ToInt32(startdate[1]), Convert.ToInt32(starttime[0]), Convert.ToInt32(starttime[1]), Convert.ToInt32(starttime[2]));



                enddatetime = endDateValue.Split(' ');
                string[] enddate = enddatetime[0].Split('/');
                string[] endtime = enddatetime[1].Split(':');


            DateTime date2 = new DateTime(Convert.ToInt32(enddate[2]), Convert.ToInt32(enddate[0]), Convert.ToInt32(enddate[1]), Convert.ToInt32(endtime[0]), Convert.ToInt32(endtime[1]), Convert.ToInt32(endtime[2]));

            int comparedates = DateTime.Compare(date1, date2);


                if (comparedates < 0)
                    Windowpopup.Close();
                else if (comparedates == 0)
                {
                    MessageBox.Show("startdate is the same as end date");
                    return;
                }
                else
                {
                    MessageBox.Show("startdate is later than end date");
                    return;
                }

            }
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

        private void ExcludeTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            Exclude = textBox.Text.ToString();            
        }

        private void SwapTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            Hex = textBox.Text.ToString(); 
        }

        public string startOne
        {
            set {}
            get { return startDateValue; }

        }

        public string endOne
        {
            set {}
            get { return endDateValue; }
        }

        public string excludeString
        {
            set {}
            get { return Exclude; }

        }

        public string hexString
        {
            set {}
            get { return Hex; }
        }

        private void Windowpopup_Activated(object sender, EventArgs e)
        {
            startDateValue = null;
            endDateValue = null;
            Hex = null;
            Exclude = null;
        }

    }
}

