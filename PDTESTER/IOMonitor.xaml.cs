using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using static GPIOCommunication.GPIOHelper;

namespace PDTESTER
{
    /// <summary>
    /// Interaction logic for IOMonitor.xaml
    /// </summary>
    public partial class IOMonitor : Window
    {
        public IOMonitor()
        {
            InitializeComponent();
            //Closing += Auto_Closing;
        }

        //private void Auto_Closing(object sender, CancelEventArgs e)
        //{
        //    e.Cancel = false;
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Set_Click(object sender, RoutedEventArgs e)
        {
            var ClickedButton = sender as Button;
            var Pin = ClickedButton.DataContext as OutputPin;
            await Pin.SET();
        }

        private async void Reset_Click(object sender, RoutedEventArgs e)
        {
            var ClickedButton = sender as Button;
            var Pin = ClickedButton.DataContext as OutputPin;
            await Pin.RST();
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
