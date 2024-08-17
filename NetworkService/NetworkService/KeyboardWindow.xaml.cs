using NetworkService.Helpers;
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

namespace NetworkService
{
    /// <summary>
    /// Interaction logic for KeyboardWindow.xaml
    /// </summary>
    public partial class KeyboardWindow : Window
    {
        
        public string Ret
        {
            get { return ShowTB.Text; }
        }
        public KeyboardWindow(string txt)
        {
            InitializeComponent();
            ShowTB.Text = txt;
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if (b.Content.ToString() == "BS")
            {
                if (ShowTB.Text.Length > 0)
                {
                    ShowTB.Text = ShowTB.Text.Remove(ShowTB.Text.Length - 1);
                }
            }
            else if (b.Content.ToString() == "SPACE")
            {
                
                ShowTB.Text += " ";
            }
            else
            {
                ShowTB.Text += b.Content.ToString();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //this.DialogResult = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
