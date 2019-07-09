using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PDTESTER
{
    /// <summary>
    /// Interaction logic for AlarmWindows.xaml
    /// </summary>
    public partial class AlarmWindow : Window
    {
        public AlarmWindow()
        {
            InitializeComponent();
            DataContext = this;
            ALMWindowDispatcher = Dispatcher;
            AlarmListViewItem.CollectionChanged += AlarmListViewItem_CollectionChanged;
        }

        SemaphoreSlim Lock = new SemaphoreSlim(1, 1);

        private void AlarmListViewItem_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AlarmListView.ScrollIntoView(AlarmListView.Items[AlarmListView.Items.Count - 1]);
            //if (Lock.CurrentCount > 0)
            //{
            //    await Lock.WaitAsync();
            //    for (int i = 0; i < 3; i++)
            //    {
            //        await OUT.BUZZER.SET();
            //        await Task.Delay(200);
            //        await OUT.BUZZER.RST();
            //        await Task.Delay(300);
            //    }
            //    Lock.Release();
            //}
        }

        public static Dispatcher ALMWindowDispatcher { get; set; }
        public static ObservableCollection<string> AlarmListViewItem = new ObservableCollection<string>();
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Collapsed;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            AlarmListViewItem.CollectionChanged -= AlarmListViewItem_CollectionChanged;
            AlarmListViewItem.Clear();
            AlarmListViewItem.CollectionChanged += AlarmListViewItem_CollectionChanged;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
