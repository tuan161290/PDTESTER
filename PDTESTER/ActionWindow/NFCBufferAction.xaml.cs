using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PDTESTER.ActionWindow
{
    /// <summary>
    /// Interaction logic for PDAction.xaml
    /// </summary>
    public partial class NFCBufferAction : Window
    {
        public JigModel ClickedJig { get; set; } = null;
        public NFCBufferAction(JigModel jig)
        {
            InitializeComponent();
            ClickedJig = jig;
            DataContext = ClickedJig;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Clear_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedJig != null)
                ClickedJig.IsSetInJig = false;
        }

        private void InsertOK_Click(object sender, RoutedEventArgs e)
        {
            if (ClickedJig != null)
            {
                ClickedJig.IsSetInJig = true;
                ClickedJig.TestResult = TestResult.PASS;
            }
        }

        private void InsertNG_Click(object sender, RoutedEventArgs e)
        {
            if (ClickedJig != null)
            {
                ClickedJig.IsSetInJig = true;
                ClickedJig.TestResult = TestResult.FAIL;
            }
        }
    }
}
