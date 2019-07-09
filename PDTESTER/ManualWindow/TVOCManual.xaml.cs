using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static GPIOCommunication.GPIOHelper;

namespace PDTESTER.ManualWindow
{
    /// <summary>
    /// Interaction logic for TVOCManual.xaml
    /// </summary>
    public partial class TVOCManual : UserControl
    {
        public TVOCManual()
        {
            InitializeComponent();
            Loaded += TVOCManual_Loaded;
            Unloaded += TVOCManual_Unloaded;
        }

        private void TVOCManual_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (JigModel J in Jigs)
            {
                J.PropertyChanged -= J_PropertyChanged;
            }
        }

        private void TVOCManual_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        ObservableCollection<JigModel> Jigs;
        void Init()
        {

            Jigs = new ObservableCollection<JigModel>();
            var AXIS_02_BUFFER = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_02_BUFFER")).FirstOrDefault();
            if (AXIS_02_BUFFER != null)
            {
                Jigs.Add(AXIS_02_BUFFER);
            }
            var AXIS_02_TRANSFER_CV_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_02_TRANSFER_CV_POS")).FirstOrDefault();
            if (AXIS_02_TRANSFER_CV_POS != null)
            {
                Jigs.Add(AXIS_02_TRANSFER_CV_POS);
            }
            var AXIS_02_SIM_01 = MainWindow.JigModels.Where(x => x.JigDescription == "SIM_01").FirstOrDefault();
            if (AXIS_02_SIM_01 != null)
            {
                Jigs.Add(AXIS_02_SIM_01);
            }
            var AXIS_02_SIM_02 = MainWindow.JigModels.Where(x => x.JigDescription == "SIM_02").FirstOrDefault();
            if (AXIS_02_SIM_02 != null)
            {
                Jigs.Add(AXIS_02_SIM_02);
            }
            using (var Db = new SettingContext())
            {
                var SavedAxis2Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_02_VELOCITY").FirstOrDefault();
                if (SavedAxis2Vel != null)
                {
                    Axis2Velocity = int.Parse(SavedAxis2Vel.Value);
                }
            }
            foreach (JigModel J in Jigs)
            {
                J.PropertyChanged -= J_PropertyChanged;
                J.PropertyChanged += J_PropertyChanged;
            }
            TVOCGridView.ItemsSource = Jigs;
            DataContext = this;
        }

        private async void J_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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

        private void SavePos_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Save Pos?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var ClickedButton = sender as Button;
                var Jig = ClickedButton.DataContext as JigModel;
                Jig.JigPos = Axis._02.CurrentPos;
            }
        }

        private async void MovePos_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Axis._02.SON == PinValue.ON && Axis._02.ORG_OK == PinValue.ON &&
                Axis._02.Motioning == PinValue.OFF &&
                IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
            {
                var ClickedButton = sender as Button;
                var Jig = ClickedButton.DataContext as JigModel;
                await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveSingleAxisAbs, DataFrame.MoveAbsIncData(Jig.JigPos, Axis2Velocity));
            }
            else MessageBox.Show("Motor is not ready!", "Error", MessageBoxButton.OK);
        }

        private async void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Left Right Manual
            if ((Axis._02.ALM == PinValue.ON || Axis._02.SON == PinValue.OFF) && Axis._02.ORG_OK == PinValue.ON)
            {
                MessageBox.Show("Motor is not ready or in alarm state!!!", "Error", MessageBoxButton.OK);
                return;
            }
            var ClickedButton = sender as Button;
            if (Axis._02.Motioning == PinValue.OFF)
            {
                if (ClickedButton == LeftJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(500, 0));
                }
                else if (ClickedButton == FLeftJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(2000, 0));
                }
                else if (ClickedButton == RightJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(500, 1));
                }
                else if (ClickedButton == FRightJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(2000, 1));
                }
            }
        }

        private async void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Motor Stop
            await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveStop, null);
        }

        private async void SON_Click(object sender, RoutedEventArgs e)
        {
            //SON
            if (Axis._02.SON == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.StepEnable, new byte[] { 0x00 });
            else
                await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.StepEnable, new byte[] { 0x01 });
        }

        private async void ORG_Click(object sender, RoutedEventArgs e)
        {
            if (Axis._02.SON == PinValue.ON && Axis._02.Motioning == PinValue.OFF
                && IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveOrigin, null);
            else
                MessageBox.Show("Motor's state is not correct to ORG", "Error", MessageBoxButton.OK);
        }

        private async void STOP_Click(object sender, RoutedEventArgs e)
        {
            await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveStop, null);
        }

        private async void RESET_Click(object sender, RoutedEventArgs e)
        {
            await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.Reset, null);
        }

        int _Axis2Velocity;
        public int Axis2Velocity
        {
            get { return _Axis2Velocity; }
            set
            {
                if (value < 100000)
                    _Axis2Velocity = value;
                else
                    _Axis2Velocity = 100000;
                NotifyPropertyChanged("Axis2Velocity");
                using (var Db = new SettingContext())
                {
                    var SavedAxis2Velocity = Db.ValueSettings.Where(x => x.Key == "AXIS_02_VELOCITY").FirstOrDefault();
                    if (SavedAxis2Velocity != null)
                    {
                        SavedAxis2Velocity.Value = _Axis2Velocity.ToString();
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

        private async void Lift_02_UpDown_Clicked(object sender, RoutedEventArgs e)
        {
            //Lift Cylinder Manual
            if (IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                //Down
                await OUT.LOADING_02_LIFT_SOL.SET();
            else
                //Up
                await OUT.LOADING_02_LIFT_SOL.RST();
        }

        private async void LOADING_02_UNCLAMP_Clicked(object sender, RoutedEventArgs e)
        {
            //Clamp manual
            //CLAMP
            await OUT.LOADING_02_UNCLAMP_SOL.SET();
            await OUT.LOADING_02_CLAMP_SOL.RST();
        }

        private async void LOADING_02_CLAMP_Clicked(object sender, RoutedEventArgs e)
        {
            //Clamp manual
            //UNCLAMP
            await OUT.LOADING_02_UNCLAMP_SOL.RST();
            await OUT.LOADING_02_CLAMP_SOL.SET();
        }

        private async void LOADING_02_TURN_Clicked(object sender, RoutedEventArgs e)
        {
            ///Rotary Cylinder manual
            //TURN
            await OUT.LOADING_02_TURN_SOL.SET();
        }

        private async void LOADING_01_TURN_Clicked(object sender, RoutedEventArgs e)
        {
            ///Rotary Cylinder manual
            //RETURN
            await OUT.LOADING_02_TURN_SOL.RST();
        }

        private async void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveStop, null);
            }
            else if (Axis._02.Motioning == PinValue.OFF && Axis._02.ORG_OK == PinValue.ON)
            {
                if (e.Key == Key.Left)
                    await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(1000, 0));
                else if (e.Key == Key.Right)
                    await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(1000, 1));
                else if (e.Key == Key.Down)
                    await OUT.LOADING_02_LIFT_SOL.SET();
                else if (e.Key == Key.Up)
                    await OUT.LOADING_02_LIFT_SOL.RST();
                else if (e.Key == Key.PageUp)
                    await OUT.LOADING_02_TURN_SOL.SET();
                else if (e.Key == Key.PageDown)
                    await OUT.LOADING_02_TURN_SOL.RST();
                else if (e.Key == Key.C)
                {
                    await OUT.LOADING_02_CLAMP_SOL.SET();
                    await OUT.LOADING_02_UNCLAMP_SOL.RST();
                }
                else if (e.Key == Key.V)
                {
                    await OUT.LOADING_02_CLAMP_SOL.RST();
                    await OUT.LOADING_02_UNCLAMP_SOL.SET();
                }
            }
        }

        private async void UserControl_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
                await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveStop, null);
        }

        private void Pack_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
