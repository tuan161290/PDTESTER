using System;
using System.Windows;

namespace PDTESTER.ActionWindow
{
    /// <summary>
    /// Interaction logic for PDAction.xaml
    /// </summary>
    public partial class SIMAction : Window
    {
        public SIMJig ClickedSIM { get; set; } = null;
        public SIMAction(SIMJig jig)
        {
            InitializeComponent();
            ClickedSIM = jig;
            DataContext = ClickedSIM;
            SIMListView.ItemsSource = Auto.Page.SIMs;
            SIMListView.SelectedItem = jig;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ManualTest_MouseClick(object sender, RoutedEventArgs e)
        {
            if (SIMListView.SelectedItems != null)
            {
                foreach (SIMJig SIM in SIMListView.SelectedItems)
                {
                    SIM.Jig.TestResult = TestResult.TEST;
                    SIM.Jig.IsSetInJig = true;
                    //await App.UCTCOM.UCTTestSwitch(ClickedPD.Jig.JigID, 1, 1);
                    SIM.Jig.StatTime = DateTime.Now;
                    SIM.StartTestLoop();
                }
            }
            //if (ClickedLeak != null)
            //{
            //    ManualButton.IsEnabled = false;
            //    AbortButton.IsEnabled = false;
            //    ClickedLeak.Jig.TestResult = TestResult.TEST;
            //    ClickedLeak.Jig.IsSetInJig = true;
            //    await ClickedLeak.PressTask();
            //    ManualButton.IsEnabled = true;
            //    AbortButton.IsEnabled = true;
            //}
        }

        private void Clear_MouseClick(object sender, RoutedEventArgs e)
        {
            if (SIMListView.SelectedItems != null)
            {
                foreach (SIMJig ClickedSIM in SIMListView.SelectedItems)
                {
                    ClickedSIM.Jig.IsSetInJig = false;
                    ClickedSIM.Jig.TestResult = TestResult.READY;
                }
            }

            //if (ClickedLeak != null)
            //{
            //    ClickedLeak.Jig.IsSetInJig = false;
            //    ClickedLeak.Jig.TestResult = TestResult.READY;
            //}
        }

        private void Use_Cliked(object sender, RoutedEventArgs e)
        {
            if (ClickedSIM != null)
            {
                if (ClickedSIM.Jig.IsJigEnable == false)
                {
                    ClickedSIM.Jig.IsJigEnable = true;
                    ClickedSIM.Jig.TestResult = TestResult.READY;
                }
                else if (ClickedSIM.Jig.IsJigEnable == true)
                    ClickedSIM.Jig.IsJigEnable = false;
            }
        }

        private void ClearCounter_Cliked(object sender, RoutedEventArgs e)
        {
            if (SIMListView.SelectedItems != null)
            {
                foreach (LeakJig ClickedLeak in SIMListView.SelectedItems)
                {
                    ClickedSIM.Jig.OKCounter = 0;
                    ClickedSIM.Jig.NGCounter = 0;
                }
            }
            //if (ClickedLeak != null)
            //{
            //    ClickedLeak.Jig.OKCounter = 0;
            //    ClickedLeak.Jig.NGCounter = 0;
            //}
        }

        private void AbortTest_MouseClick(object sender, RoutedEventArgs e)
        {
            if (SIMListView.SelectedItems != null)
            {
                foreach (SIMJig SIM in SIMListView.SelectedItems)
                {
                    SIM.Jig.TestResult = TestResult.ABORTED;
                    SIM.Jig.IsSetInJig = true;
                    SIM.StopTestLoop();
                    //await App.UCTCOM.UCTTestSwitch(ClickedPD.Jig.JigID, 1, 1);
                    //SIM.Release();
                }
            }
            //if (ClickedLeak != null)
            //{
            //    ManualButton.IsEnabled = false;
            //    AbortButton.IsEnabled = false;
            //    ClickedLeak.Jig.TestResult = TestResult.ABORTED;
            //    ClickedLeak.Jig.IsSetInJig = true;
            //    await ClickedLeak.ReleaseTask();
            //    ManualButton.IsEnabled = true;
            //    AbortButton.IsEnabled = true;
            //}
        }

        private void MarkFail_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedSIM != null && ClickedSIM.Jig.IsJigEnable)
            {
                ClickedSIM.Jig.TestResult = TestResult.FAIL;
            }
        }

        private void MarkOK_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedSIM != null && ClickedSIM.Jig.IsJigEnable)
            {
                ClickedSIM.Jig.TestResult = TestResult.PASS;
            }
        }
    }
}
