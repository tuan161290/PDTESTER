using System.Windows;
using System.Windows.Input;

namespace PDTESTER.ActionWindow
{
    /// <summary>
    /// Interaction logic for PDAction.xaml
    /// </summary>
    public partial class NFCAction : Window
    {
        public NFCJig ClickedNFC { get; set; } = null;
        public NFCAction(NFCJig jig)
        {
            InitializeComponent();
            ClickedNFC = jig;
            DataContext = ClickedNFC;
            NFCListView.SelectedItem = jig;
            NFCListView.ItemsSource = Auto.Page.NFCs;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private async void ManualTest_Click(object sender, RoutedEventArgs e)
        {
            if (NFCListView.SelectedItems != null)
            {
                foreach (NFCJig NFC in NFCListView.SelectedItems)
                {
                    NFC.Jig.TestResult = TestResult.TEST;
                    NFC.Jig.IsSetInJig = true;
                    //await App.UCTCOM.UCTTestSwitch(ClickedPD.Jig.JigID, 1, 1);
                    await NFC.PackingPin.SET();
                }
            }
            //if (ClickedNFC != null)
            //{
            //    ClickedNFC.Jig.TestResult = TestResult.TEST;
            //    ClickedNFC.Jig.IsSetInJig = true;
            //    await ClickedNFC.PackingPin.SET();
            //}
        }

        private void Clear_Clicked(object sender, RoutedEventArgs e)
        {
            if (NFCListView.SelectedItems != null)
            {
                foreach (NFCJig NFC in NFCListView.SelectedItems)
                {
                    NFC.Jig.TestResult = TestResult.TEST;
                    NFC.Jig.IsSetInJig = false;
                    NFC.Jig.TestResult = TestResult.READY;
                    //await App.UCTCOM.UCTTestSwitch(ClickedPD.Jig.JigID, 1, 1);
                    //await NFC.PackingPin.SET();
                }
            }
            //if (ClickedNFC != null)
            //{
            //    ClickedNFC.Jig.IsSetInJig = false;
            //    ClickedNFC.Jig.TestResult = TestResult.READY;
            //}
        }

        private void Use_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedNFC != null)
            {
                if (ClickedNFC.Jig.IsJigEnable == false)
                {
                    ClickedNFC.Jig.IsJigEnable = true;
                    ClickedNFC.Jig.TestResult = TestResult.READY;
                }
                else if (ClickedNFC.Jig.IsJigEnable == true)
                    ClickedNFC.Jig.IsJigEnable = false;
            }
        }

        private void ClearCounter_Clicked(object sender, RoutedEventArgs e)
        {
            if (NFCListView.SelectedItems != null)
            {
                foreach (NFCJig ClickedNFC in NFCListView.SelectedItems)
                {
                    ClickedNFC.Jig.OKCounter = 0;
                    ClickedNFC.Jig.NGCounter = 0;
                }
            }
            //if (ClickedNFC != null)
            //{
            //    ClickedNFC.Jig.OKCounter = 0;
            //    ClickedNFC.Jig.NGCounter = 0;
            //}
        }

        private async void AbortTest_Click(object sender, RoutedEventArgs e)
        {
            if (NFCListView.SelectedItems != null)
            {
                foreach (NFCJig ClickedNFC in NFCListView.SelectedItems)
                {
                    ClickedNFC.Jig.TestResult = TestResult.ABORTED;
                    ClickedNFC.Jig.IsSetInJig = true;
                    await ClickedNFC.PackingPin.RST();
                }
            }
            //if (ClickedNFC != null)
            //{
            //    ClickedNFC.Jig.TestResult = TestResult.ABORTED;
            //    ClickedNFC.Jig.IsSetInJig = true;
            //    await ClickedNFC.PackingPin.RST();
            //}
        }
    }
}
