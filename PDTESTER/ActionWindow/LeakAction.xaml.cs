using System;
using System.Windows;

namespace PDTESTER.ActionWindow
{
    /// <summary>
    /// Interaction logic for PDAction.xaml
    /// </summary>
    public partial class LeakAction : Window
    {
        public LeakJig ClickedLeak { get; set; } = null;
        public LeakAction(LeakJig jig)
        {
            InitializeComponent();
            ClickedLeak = jig;
            DataContext = ClickedLeak;
            LeakListView.ItemsSource = Auto.Page.LEAKs;
            LeakListView.SelectedItem = jig;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ManualTest_MouseClick(object sender, RoutedEventArgs e)
        {
            if (LeakListView.SelectedItems != null)
            {
                foreach (LeakJig Leak in LeakListView.SelectedItems)
                {
                    Leak.Jig.TestResult = TestResult.TEST;
                    Leak.Jig.IsSetInJig = true;
                    Leak.Jig.StatTime = DateTime.Now;
                    Leak.Press();
                }
            }
        }

        private void Clear_MouseClick(object sender, RoutedEventArgs e)
        {
            if (LeakListView.SelectedItems != null)
            {
                foreach (LeakJig ClickedLeak in LeakListView.SelectedItems)
                {
                    ClickedLeak.Jig.IsSetInJig = false;
                    ClickedLeak.Jig.TestResult = TestResult.READY;
                }
            }
        }

        private void Use_Cliked(object sender, RoutedEventArgs e)
        {
            if (ClickedLeak != null)
            {
                if (ClickedLeak.Jig.IsJigEnable == false)
                {
                    ClickedLeak.Jig.IsJigEnable = true;
                    ClickedLeak.Jig.TestResult = TestResult.READY;
                }
                else if (ClickedLeak.Jig.IsJigEnable == true)
                    ClickedLeak.Jig.IsJigEnable = false;
            }
        }

        private void ClearCounter_Cliked(object sender, RoutedEventArgs e)
        {
            if (LeakListView.SelectedItems != null)
            {
                foreach (LeakJig ClickedLeak in LeakListView.SelectedItems)
                {
                    ClickedLeak.Jig.OKCounter = 0;
                    ClickedLeak.Jig.NGCounter = 0;
                }
            }
        }

        private void AbortTest_MouseClick(object sender, RoutedEventArgs e)
        {

            if (LeakListView.SelectedItems != null)
            {
                foreach (LeakJig Leak in LeakListView.SelectedItems)
                {
                    Leak.Jig.TestResult = TestResult.ABORTED;
                    Leak.Jig.IsSetInJig = true;
                    Leak.Release();
                }
            }
        }
    }
}
