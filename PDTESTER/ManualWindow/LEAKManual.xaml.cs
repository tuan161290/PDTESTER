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
    /// Interaction logic for LEAKManual.xaml
    /// </summary>
    public partial class LEAKManual : UserControl
    {
        public LEAKManual()
        {
            InitializeComponent();
            Loaded += LEAKManual_Loaded;
            //Unloaded += LEAKManual_Unloaded;
        }

        private void LEAKManual_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private async void MovePos_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Axis._04.SON == PinValue.ON &&
                //Axis._04.ORG_OK == PinValue.ON &&
                Axis._04.Motioning == PinValue.OFF &&
                IN.LOADING_04_LIFT_UP_SENSOR.PinValue == PinValue.ON)
            {
                var ClickedButton = sender as Button;
                var LeakJig = ClickedButton.DataContext as LeakJig;
                await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveSingleAxisAbs, DataFrame.MoveAbsIncData(LeakJig.Jig.JigPos, Axis4Velocity));
            }
        }

        private async void Pack_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var ClickedButton = sender as Button;
            ClickedButton.IsEnabled = false;
            var LeakJig = ClickedButton.DataContext as LeakJig;
            if (LeakJig.PackingPin != null)
                if (LeakJig.PackingPin.PinValue == PinValue.OFF)
                    await LeakJig.PackingPin.SET();
                else
                    await LeakJig.PackingPin.RST();
            ClickedButton.IsEnabled = true;
        }

        private void SavePos_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Save Pos?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var ClickedButton = sender as Button;
                var LeakJig = ClickedButton.DataContext as LeakJig;
                LeakJig.Jig.JigPos = Axis._04.CurrentPos;
            }
        }

        private async void SON_Click(object sender, RoutedEventArgs e)
        {
            if (Axis._04.SON == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.StepEnable, new byte[] { 0x00 });
            else
                await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.StepEnable, new byte[] { 0x01 });
        }

        private async void RESET_Click(object sender, RoutedEventArgs e)
        {
            await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.Reset, null);
        }

        private async void STOP_Click(object sender, RoutedEventArgs e)
        {
            await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveStop, null);
        }

        private async void ORG_Click(object sender, RoutedEventArgs e)
        {
            if (Axis._04.SON == PinValue.ON && Axis._04.Motioning == PinValue.OFF &&
                IN.LOADING_04_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveOrigin, null);
            else
                MessageBox.Show("Motor's state is not correct to ORG", "Error", MessageBoxButton.OK);
        }

        private async void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((Axis._04.ALM == PinValue.ON || Axis._04.SON == PinValue.OFF) && Axis._04.ORG_OK == PinValue.ON)
            {
                MessageBox.Show("Motor is not ready or in alarm state!!!", "Error", MessageBoxButton.OK);
                return;
            }
            if (Axis._04.Motioning == PinValue.OFF)
            {
                var ClickedButton = sender as Button;
                if (ClickedButton == LeftJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(500, 0));
                }
                else if (ClickedButton == FLeftJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(2000, 0));
                }
                else if (ClickedButton == RightJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(500, 1));
                }
                else if (ClickedButton == FRightJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(2000, 1));
                }
            }
        }

        private async void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Motor Stop
            await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveStop, null);
        }

        private async void Lift_04_UpDown_Clicked(object sender, RoutedEventArgs e)
        {
            //Lift 01 Cylinder manual            
            if (IN.LOADING_04_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                //Down
                await OUT.LOADING_04_LIFT_SOL.SET();
            else
                //Up
                await OUT.LOADING_04_LIFT_SOL.RST();
        }

        private async void LOADING_04_CLAMP_Clicked(object sender, RoutedEventArgs e)
        {   //Clamp manual
            //CLAMP
            var ClickedButton = sender as Button;
            ClickedButton.IsEnabled = false;
            await OUT.LOADING_04_CLAMP_SOL.SET();
            await OUT.LOADING_04_UNCLAMP_SOL.RST();
            ClickedButton.IsEnabled = true;
        }

        private async void LOADING_04_UNCLAMP_Clicked(object sender, RoutedEventArgs e)
        {
            //Clamp manual
            //Unclamp
            var ClickedButton = sender as Button;
            await OUT.LOADING_04_UNCLAMP_SOL.SET();
            await OUT.LOADING_04_CLAMP_SOL.RST();
        }

        ObservableCollection<LeakJig> LEAKs;
        void Init()
        {
            //using (var Db = new SettingContext())
            //{
            LEAKs = new ObservableCollection<LeakJig>();
            var _AXIS_04_BUFFER = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_04_BUFFER")).FirstOrDefault();
            if (_AXIS_04_BUFFER != null)
            {
                var AXIS_04_BUFFER = new LeakJig() { Jig = _AXIS_04_BUFFER, PackingPin = null };
                LEAKs.Add(AXIS_04_BUFFER);
            }
            var _LEAK_01 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("LEAK_01")).FirstOrDefault();
            if (_LEAK_01 != null)
            {
                LeakJig LEAK01 = new LeakJig()
                {
                    Jig = _LEAK_01,
                    PackingPin = OUT.LEAK_01_PACK,
                    PressSolenoid = OUT.LEAK_01_PRESS_SOL,
                    TransferSolenoid = OUT.LEAK_01_TRANS_SOL,
                    DownSenSor = IN.LEAK_01_PRESS_DOWN_SENSOR,
                    UpSensor = IN.LEAK_01_PRESS_UP_SENSOR,
                    ForwardSensor = IN.LEAK_01_FOWARD_SENSOR,
                    ReverseSensor = IN.LEAK_01_REVERSE_SENSOR,
                };
                LEAKs.Add(LEAK01);
            }
            var _LEAK_02 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("LEAK_02")).FirstOrDefault();
            if (_LEAK_02 != null)
            {
                LeakJig LEAK02 = new LeakJig()
                {
                    Jig = _LEAK_02,
                    PackingPin = OUT.LEAK_02_PACK,
                    PressSolenoid = OUT.LEAK_02_PRESS_SOL,
                    TransferSolenoid = OUT.LEAK_02_TRANS_SOL,
                    DownSenSor = IN.LEAK_02_PRESS_DOWN_SENSOR,
                    UpSensor = IN.LEAK_02_PRESS_UP_SENSOR,
                    ForwardSensor = IN.LEAK_02_FOWARD_SENSOR,
                    ReverseSensor = IN.LEAK_02_REVERSE_SENSOR,
                };
                LEAKs.Add(LEAK02);
            }
            var _LEAK_03 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("LEAK_03")).FirstOrDefault();
            if (_LEAK_03 != null)
            {
                LeakJig LEAK03 = new LeakJig()
                {
                    Jig = _LEAK_03,
                    PackingPin = OUT.LEAK_03_PACK,
                    PressSolenoid = OUT.LEAK_03_PRESS_SOL,
                    TransferSolenoid = OUT.LEAK_03_TRANS_SOL,
                    DownSenSor = IN.LEAK_03_PRESS_DOWN_SENSOR,
                    UpSensor = IN.LEAK_03_PRESS_UP_SENSOR,
                    ForwardSensor = IN.LEAK_03_FOWARD_SENSOR,
                    ReverseSensor = IN.LEAK_03_REVERSE_SENSOR,
                };
                LEAKs.Add(LEAK03);
            }
            var _LEAK_04 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("LEAK_04")).FirstOrDefault();
            if (_LEAK_04 != null)
            {
                LeakJig LEAK04 = new LeakJig()
                {
                    Jig = _LEAK_04,
                    PackingPin = OUT.LEAK_04_PACK,
                    PressSolenoid = OUT.LEAK_04_PRESS_SOL,
                    TransferSolenoid = OUT.LEAK_04_TRANS_SOL,
                    DownSenSor = IN.LEAK_04_PRESS_DOWN_SENSOR,
                    UpSensor = IN.LEAK_04_PRESS_UP_SENSOR,
                    ForwardSensor = IN.LEAK_04_FOWARD_SENSOR,
                    ReverseSensor = IN.LEAK_04_REVERSE_SENSOR,
                };
                LEAKs.Add(LEAK04);
            }
            var _AXIS_04_UNLOADING_CV_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_04_UNLOADING_CV_POS")).FirstOrDefault();
            if (_AXIS_04_UNLOADING_CV_POS != null)
            {
                LeakJig AXIS_04_UNLOADING_CV_POS = new LeakJig()
                {
                    Jig = _AXIS_04_UNLOADING_CV_POS
                };
                LEAKs.Add(AXIS_04_UNLOADING_CV_POS);
            }
            var _AXIS_04_NG_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_04_NG_POS")).FirstOrDefault();
            if (_AXIS_04_NG_POS != null)
            {
                LeakJig AXIS_04_NG_POS = new LeakJig()
                {
                    Jig = _AXIS_04_NG_POS
                };
                LEAKs.Add(AXIS_04_NG_POS);
            }
            using (var Db = new SettingContext())
            {
                var SavedAxis4Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_04_VELOCITY").FirstOrDefault();
                if (SavedAxis4Vel != null)
                {
                    Axis4Velocity = int.Parse(SavedAxis4Vel.Value);
                }
            }
            foreach (LeakJig LK in LEAKs)
            {
                LK.Jig.PropertyChanged -= J_PropertyChanged;
                LK.Jig.PropertyChanged += J_PropertyChanged;
            }
            LEAKGridView.ItemsSource = LEAKs;
            DataContext = this;
            //}
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

        int _Axis4Velocity;
        public int Axis4Velocity
        {
            get { return _Axis4Velocity; }
            set
            {
                if (value < 100000)
                    _Axis4Velocity = value;
                else
                    _Axis4Velocity = 100000;
                NotifyPropertyChanged("Axis4Velocity");
                using (var Db = new SettingContext())
                {
                    var SavedAxis4Velocity = Db.ValueSettings.Where(x => x.Key == "AXIS_04_VELOCITY").FirstOrDefault();
                    if (SavedAxis4Velocity != null)
                    {
                        SavedAxis4Velocity.Value = _Axis4Velocity.ToString();
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


        private void LeftJog_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Press_Release_Clicked(object sender, MouseButtonEventArgs e)
        {
            var ClickedButton = sender as Button;
            var LeakJig = ClickedButton.DataContext as LeakJig;
            ClickedButton.IsEnabled = false;
            if (LeakJig.UpSensor.PinValue == PinValue.ON)
                await LeakJig.PressTask();
            else
                await LeakJig.ReleaseTask();
            ClickedButton.IsEnabled = true;
        }

        private async void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveStop, null);
            }
            else if (Axis._04.Motioning == PinValue.OFF
                )//&& Axis._04.ORG_OK == PinValue.ON)
            {
                if (e.Key == Key.Left)
                    await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(1000, 1));
                else if (e.Key == Key.Right)
                    await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(1000, 0));
                else if (e.Key == Key.Down)
                    await OUT.LOADING_04_LIFT_SOL.SET();
                else if (e.Key == Key.Up)
                    await OUT.LOADING_04_LIFT_SOL.RST();
                else if (e.Key == Key.C)
                {
                    await OUT.LOADING_04_CLAMP_SOL.SET();
                    await OUT.LOADING_04_UNCLAMP_SOL.RST();
                }
                else if (e.Key == Key.V)
                {
                    await OUT.LOADING_04_CLAMP_SOL.RST();
                    await OUT.LOADING_04_UNCLAMP_SOL.SET();
                }
            }
        }

        private async void UserControl_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
                await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveStop, null);
        }
    }
}
