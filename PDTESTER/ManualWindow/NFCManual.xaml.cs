using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static GPIOCommunication.GPIOHelper;

namespace PDTESTER.ManualWindow
{
    /// <summary>
    /// Interaction logic for NFCManual.xaml
    /// </summary>
    public partial class NFCManual : UserControl
    {
        public NFCManual()
        {
            InitializeComponent();
            Loaded += NFCManual_Loaded;
            Unloaded += NFCManual_Unloaded;
        }

        private void NFCManual_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (NFCJig NFC in NFCs)
            {
                NFC.Jig.PropertyChanged -= J_PropertyChanged;
            }
        }

        private void NFCManual_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private async void MovePos_DoubleClick(object sender, MouseButtonEventArgs e)
        {   
            if (Axis._03.SON == PinValue.ON && Axis._03.ORG_OK == PinValue.ON &&
               Axis._03.Motioning == PinValue.OFF &&
               IN.LOADING_03_LIFT_UP_SENSOR.PinValue == PinValue.ON)
            {
                var ClickedButton = sender as Button;
                var NFCJig = ClickedButton.DataContext as NFCJig;
                await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveSingleAxisAbs, DataFrame.MoveAbsIncData(NFCJig.Jig.JigPos, Axis3Velocity));
            }
        }

        private async void Pack_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var ClickedButton = sender as Button;
            ClickedButton.IsEnabled = false;
            var NFCJig = ClickedButton.DataContext as NFCJig;
            if (NFCJig.PackingPin != null)
                if (NFCJig.PackingPin.PinValue == PinValue.OFF)
                    await NFCJig.PackingPin.SET();
                else
                    await NFCJig.PackingPin.RST();
            ClickedButton.IsEnabled = true;
        }

        private void SavePos_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Save Pos?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var ClickedButton = sender as Button;
                var NFCJig = ClickedButton.DataContext as NFCJig;
                NFCJig.Jig.JigPos = Axis._03.CurrentPos;
            }
        }

        private async void SON_Click(object sender, RoutedEventArgs e)
        {
            if (Axis._03.SON == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.StepEnable, new byte[] { 0x00 });
            else
                await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.StepEnable, new byte[] { 0x01 });
        }

        private async void RESET_Click(object sender, RoutedEventArgs e)
        {
            await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.Reset, null);
        }

        private async void STOP_Click(object sender, RoutedEventArgs e)
        {
            await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveStop, null);
        }

        private async void ORG_Click(object sender, RoutedEventArgs e)
        {
            if (Axis._03.SON == PinValue.ON && Axis._03.Motioning == PinValue.OFF 
                && IN.LOADING_03_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveOrigin, null);
            else
                MessageBox.Show("Motor's state is not correct to ORG", "Error", MessageBoxButton.OK);
        }

        private async void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((Axis._03.ALM == PinValue.ON || Axis._03.SON == PinValue.OFF) && Axis._01.ORG_OK == PinValue.ON)
            {
                MessageBox.Show("Motor is not ready or in alarm state!!!", "Error", MessageBoxButton.OK);
                return;
            }
            if (Axis._03.Motioning == PinValue.OFF)
            {
                var ClickedButton = sender as Button;
                if (ClickedButton == LeftJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(500, 1));
                }
                else if (ClickedButton == FLeftJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(2000, 1));
                }
                else if (ClickedButton == RightJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(500, 0));
                }
                else if (ClickedButton == FRightJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(2000, 0));
                }
            }
        }

        private async void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Motor Stop
            await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveStop, null);
        }

        private async void Lift_03_UpDown_Clicked(object sender, RoutedEventArgs e)
        {
            //Lift 01 Cylinder manual            
            if (IN.LOADING_03_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                //Down
                await OUT.LOADING_03_LIFT_SOL.SET();
            else
                //Up
                await OUT.LOADING_03_LIFT_SOL.RST();
        }

        private async void LOADING_03_CLAMP_Clicked(object sender, RoutedEventArgs e)
        {   //Clamp manual
            //CLAMP
            var ClickedButton = sender as Button;
            ClickedButton.IsEnabled = false;
            await OUT.LOADING_03_CLAMP_SOL.SET();
            await OUT.LOADING_03_UNCLAMP_SOL.RST();
            ClickedButton.IsEnabled = true;
        }

        private async void LOADING_03_UNCLAMP_Clicked(object sender, RoutedEventArgs e)
        {
            //Clamp manual
            //Unclamp
            var ClickedButton = sender as Button;
            await OUT.LOADING_03_UNCLAMP_SOL.SET();
            await OUT.LOADING_03_CLAMP_SOL.RST();
        }

        ObservableCollection<NFCJig> NFCs;
        void Init()
        {

            NFCs = new ObservableCollection<NFCJig>();
            var _AXIS_03_LOADING_CV_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_03_LOADING_CV_POS")).FirstOrDefault();
            if (_AXIS_03_LOADING_CV_POS != null)
            {
                var AXIS_03_LOADING_CV_POS = new NFCJig() { Jig = _AXIS_03_LOADING_CV_POS, PackingPin = null };
                NFCs.Add(AXIS_03_LOADING_CV_POS);
            }
            var _NFC_01 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("NFC_01")).FirstOrDefault();
            if (_NFC_01 != null)
            {
                var NFC_01 = new NFCJig() { PackingPin = OUT.NFC_01_PACK, Jig = _NFC_01 };
                NFCs.Add(NFC_01);
            }
            var _NFC_02 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("NFC_02")).FirstOrDefault();
            if (_NFC_02 != null)
            {
                var NFC_02 = new NFCJig() { PackingPin = OUT.NFC_02_PACK, Jig = _NFC_02 };
                NFCs.Add(NFC_02);
            }
            var _NFC_03 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("NFC_03")).FirstOrDefault();
            if (_NFC_03 != null)
            {
                var NFC_03 = new NFCJig() { PackingPin = OUT.NFC_03_PACK, Jig = _NFC_03 };
                NFCs.Add(NFC_03);
            }
            var _NFC_04 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("NFC_04")).FirstOrDefault();
            if (_NFC_04 != null)
            {
                var NFC_04 = new NFCJig() { PackingPin = OUT.NFC_04_PACK, Jig = _NFC_04 };
                NFCs.Add(NFC_04);
            }
            var _AXIS_03_BUFFER = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_03_BUFFER")).FirstOrDefault();
            if (_AXIS_03_BUFFER != null)
            {
                var AXIS_03_BUFFER = new NFCJig() { Jig = _AXIS_03_BUFFER, PackingPin = null };
                NFCs.Add(AXIS_03_BUFFER);
            }
            using (var Db = new SettingContext())
            {
                var SavedAxis3Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_03_VELOCITY").FirstOrDefault();
                if (SavedAxis3Vel != null)
                {
                    Axis3Velocity = int.Parse(SavedAxis3Vel.Value);
                }
            }
            foreach (NFCJig NFC in NFCs)
            {
                NFC.Jig.PropertyChanged -= J_PropertyChanged;
                NFC.Jig.PropertyChanged += J_PropertyChanged;
            }
            NFCGridView.ItemsSource = NFCs;
            DataContext = this;
        }


        private async void J_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "JigPos")
            {
                JigModel Sender = sender as JigModel;
                using (SettingContext Db = new SettingContext())
                {
                    var SavedJigModel = Db.JigModels.Where(x => x.JigModelID == Sender.JigModelID).FirstOrDefault();
                    if (SavedJigModel != null)
                    {
                        SavedJigModel.JigPos = Sender.JigPos;
                        await Db.SaveChangesAsync();
                    }
                }
            }
        }

        int _Axis3Velocity;
        public int Axis3Velocity
        {
            get { return _Axis3Velocity; }
            set
            {
                if (value < 100000)
                    _Axis3Velocity = value;
                else
                    _Axis3Velocity = 100000;
                NotifyPropertyChanged("Axis3Velocity");
                using (var Db = new SettingContext())
                {
                    var SavedAxis3Velocity = Db.ValueSettings.Where(x => x.Key == "AXIS_03_VELOCITY").FirstOrDefault();
                    if (SavedAxis3Velocity != null)
                    {
                        SavedAxis3Velocity.Value = _Axis3Velocity.ToString();
                        Db.SaveChanges();
                    }
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveStop, null);
            }
            else if (Axis._03.Motioning == PinValue.OFF && Axis._03.ORG_OK == PinValue.ON)
            {
                if (e.Key == Key.Left)
                    await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(1000, 1));
                else if (e.Key == Key.Right)
                    await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(1000, 0));
                else if (e.Key == Key.Down)
                    await OUT.LOADING_03_LIFT_SOL.SET();
                else if (e.Key == Key.Up)
                    await OUT.LOADING_03_LIFT_SOL.RST();
                else if (e.Key == Key.C)
                {
                    await OUT.LOADING_03_CLAMP_SOL.SET();
                    await OUT.LOADING_03_UNCLAMP_SOL.RST();
                }
                else if (e.Key == Key.V)
                {
                    await OUT.LOADING_03_CLAMP_SOL.RST();
                    await OUT.LOADING_03_UNCLAMP_SOL.SET();
                }
            }
        }

        private async void UserControl_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
                await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveStop, null);
        }
    }
}
