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

namespace WFO_PROJECT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartString_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            // ... Change Window Title.            
            string grepStartValue = textBox.Text;

            this.Title = grepStartValue;

        }

        private void EndString_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            var textBoxEnd = sender as TextBox;
            string grepEndValue = textBoxEnd.Text;
            this.Title = grepEndValue;

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            var textBoxLineVariable = sender as TextBox;
            string afterValue = textBoxLineVariable.Text;
            this.Title = afterValue;

        }

        private void TextBox_TextChanged_3(object sender, TextChangedEventArgs e)
        {
            var textBoxLine = sender as TextBox;
            string beforeValue = textBoxLine.Text;
            this.Title = beforeValue;
        }

       
    }
}
