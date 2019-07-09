using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PDTESTER.ActionWindow
{
    /// <summary>
    /// Interaction logic for PDAction.xaml
    /// </summary>
    public partial class TVOCAction : Window
    {
        public TVOCJig ClickedTVOC { get; set; } = null;
        public TVOCAction(TVOCJig jig)
        {
            InitializeComponent();
            ClickedTVOC = jig;
            DataContext = ClickedTVOC;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ManualTest_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedTVOC != null)
            {
                ClickedTVOC.Jig.TestResult = TestResult.TEST;
                ClickedTVOC.Jig.IsSetInJig = true;
                await OUT.TVOC_START.SET();
                await Task.Delay(100);
                await OUT.TVOC_START.RST();
            }
        }

        private void Clear_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedTVOC != null) { 
                ClickedTVOC.Jig.IsSetInJig = false;
                ClickedTVOC.Jig.TestResult = TestResult.READY;
            }
        }

        private void ClearCounter_Cliked(object sender, RoutedEventArgs e)
        {
            if (ClickedTVOC != null)
            {
                ClickedTVOC.Jig.OKCounter = 0;
                ClickedTVOC.Jig.NGCounter = 0;
            }
        }
    }
}
