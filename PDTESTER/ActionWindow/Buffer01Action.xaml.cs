using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PDTESTER.ActionWindow
{
    /// <summary>
    /// Interaction logic for PDAction.xaml
    /// </summary>
    public partial class Buffer01Action : Window
    {
        public PDJig ClickedJig { get; set; } = null;
        public Buffer01Action(PDJig jig)
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
                ClickedJig.Jig.IsSetInJig = false;
        }

        private async void Unpack_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedJig != null)
            {
                await ClickedJig.PackingPin.RST();
            }
        }

        private async void Pack_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedJig != null)
            {
                ClickedJig.Jig.IsSetInJig = true;
                await ClickedJig.PackingPin.SET();
            }
        }

        private void Use_Clicked(object sender, RoutedEventArgs e)
        {
            if (ClickedJig != null)
            {
                ClickedJig.Jig.IsJigEnable = !ClickedJig.Jig.IsJigEnable;
            }
        }
    }
}
