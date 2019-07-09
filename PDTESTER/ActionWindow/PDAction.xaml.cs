using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PDTESTER.ActionWindow
{
    /// <summary>
    /// Interaction logic for PDAction.xaml
    /// </summary>
    public partial class PDAction : Window
    {
        public PDJig ClickedPD;
        public List<UCT100> UCTs;
        public PDAction(PDJig jig)
        {
            InitializeComponent();
            ClickedPD = jig;
            DataContext = ClickedPD;
            JigListView.ItemsSource = Auto.Page.PDs.Reverse();
            JigListView.SelectedItem = jig;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void ManualTest_MouseClick(object sender, RoutedEventArgs e)
        {
            if (JigListView.SelectedItems != null)
            {
                foreach (PDJig Pd in JigListView.SelectedItems)
                {
                    Pd.Jig.TestResult = TestResult.TEST;
                    Pd.Jig.IsSetInJig = true;
                    //await App.UCTCOM.UCTTestSwitch(ClickedPD.Jig.JigID, 1, 1);
                    await Pd.PackingPin.SET();
                }
            }
        }

        private void Use_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedPD != null)
            {
                if (ClickedPD.Jig.IsJigEnable == false)
                {
                    ClickedPD.Jig.TestResult = TestResult.READY;
                    ClickedPD.Jig.IsJigEnable = true;
                }
                else if (ClickedPD.Jig.IsJigEnable == true)
                {
                    ClickedPD.Jig.IsJigEnable = false;
                    //ClickedPD.Jig.TestResult = TestResult.NOT_USE;
                }
            }
        }

        private void ClearCounter_Clicked(object sender, RoutedEventArgs e)
        {
            if (JigListView.SelectedItems != null)
            {
                foreach (PDJig ClickedPD in JigListView.SelectedItems)
                {
                    ClickedPD.Jig.OKCounter = 0;
                    ClickedPD.Jig.NGCounter = 0;
                }
            }
        }

        private async void Abort_MouseClick(object sender, RoutedEventArgs e)
        {
            if (JigListView.SelectedItems != null)
            {
                foreach (PDJig Pd in JigListView.SelectedItems)
                {
                    Pd.Jig.TestResult = TestResult.ABORTED;
                    Pd.Jig.IsSetInJig = true;
                    //await App.UCTCOM.UCTTestSwitch(ClickedPD.Jig.JigID, 1, 1);
                    await Pd.PackingPin.RST();
                }
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (JigListView.SelectedItems != null)
            {
                foreach (PDJig ClickedPD in JigListView.SelectedItems)
                {
                    foreach (UCT100 UCT in UCTs)
                    {
                        if (UCT.PDs.Contains(ClickedPD)) { UCT.ResetFlag = false; }
                    }
                    ClickedPD.Jig.IsSetInJig = false;
                    ClickedPD.Jig.FailCounter = 0;
                    ClickedPD.Jig.TestResult = TestResult.READY;
                    ClickedPD.TestCount = 0;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private async void TSON_Clicked(object sender, RoutedEventArgs e)
        {
            await App.UCTCOM.UCTTestSwitch(ClickedPD.SWID, ClickedPD.Channel, 1);
        }

        private async void TSOF_Clicked(object sender, RoutedEventArgs e)
        {
            await App.UCTCOM.UCTTestSwitch(ClickedPD.SWID, ClickedPD.Channel, 0);
        }

        private async void RSTUCT_Clicked(object sender, RoutedEventArgs e)
        {
            var ClickedButton = sender as Button;
            ClickedButton.IsEnabled = false;
            ClickedPD.Resetting = true;
            await App.UCTCOM.Reset(ClickedPD.SWID, ClickedPD.Channel);
            //while (ClickedPD.Jig.JigState != UCTStatus.FINISHED) ;
            await Task.Delay(3000);
            ClickedPD.Resetting = false;
            ClickedButton.IsEnabled = true;
        }

        private async void RSTPW_Clicked(object sender, RoutedEventArgs e)
        {
            var ClickedButton = sender as Button;
            ClickedButton.IsEnabled = false;
            ClickedPD.Resetting = true;
            await ClickedPD.PowerPin.SET();
            await Task.Delay(3000);
            await ClickedPD.PowerPin.RST();
            ClickedPD.Resetting = false;
            ClickedButton.IsEnabled = true;
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                Close();
        }

        private void MarkFail_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedPD != null && ClickedPD.Jig.IsJigEnable)
            {
                ClickedPD.Jig.TestResult = TestResult.FAIL;
            }
        }

        private void MarkOK_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedPD != null && ClickedPD.Jig.IsJigEnable)
            {
                ClickedPD.Jig.TestResult = TestResult.PASS;
            }
        }
    }
}
