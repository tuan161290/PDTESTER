using System;
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
    /// Interaction logic for PDManual.xaml
    /// </summary>
    public partial class PDManual : UserControl//, //INotifyPropertyChanged
    {
        public PDManual()
        {
            InitializeComponent();
            Loaded += PDManual_Loaded;
        }

        private void PDManual_Loaded(object sender, RoutedEventArgs e)
        {
            InitDb();
        }
        ObservableCollection<PDJig> PDs;
        private void InitDb()
        {
            try
            {
                PDs = new ObservableCollection<PDJig>();
                var _PD01 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("PD_01")).FirstOrDefault();
                if (_PD01 != null)
                {
                    var PD01 = new PDJig() { Jig = _PD01, PackingPin = OUT.PD_01_PACK/*, PowerPin = OUT.PD_01_POWER_PIN*/ };
                    PDs.Add(PD01);
                }
                var _PD02 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("PD_02")).FirstOrDefault();
                if (_PD02 != null)
                {
                    var PD02 = new PDJig() { PackingPin = OUT.PD_02_PACK, Jig = _PD02/*, PowerPin = OUT.PD_02_POWER_PIN*/ };
                    PDs.Add(PD02);
                }
                var _PD03 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("PD_03")).FirstOrDefault();
                if (_PD03 != null)
                {
                    var PD03 = new PDJig() { PackingPin = OUT.PD_03_PACK, Jig = _PD03/*, PowerPin = OUT.PD_03_POWER_PIN*/ };
                    PDs.Add(PD03);
                }
                var _PD04 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("PD_04")).FirstOrDefault();
                if (_PD04 != null)
                {
                    var PD04 = new PDJig() { PackingPin = OUT.PD_04_PACK, Jig = _PD04/*, PowerPin = OUT.PD_04_POWER_PIN */};
                    PDs.Add(PD04);
                }
                var _PD05 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("PD_05")).FirstOrDefault();
                if (_PD05 != null)
                {
                    var PD05 = new PDJig() { PackingPin = OUT.PD_05_PACK, Jig = _PD05/*, PowerPin = OUT.PD_05_POWER_PIN */};
                    PDs.Add(PD05);
                }
                var _PD06 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("PD_06")).FirstOrDefault();
                if (_PD06 != null)
                {
                    var PD06 = new PDJig() { PackingPin = OUT.PD_06_PACK, Jig = _PD06/*, PowerPin = OUT.PD_06_POWER_PIN */};
                    PDs.Add(PD06);
                }
                var _PD07 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("PD_07")).FirstOrDefault();
                if (_PD07 != null)
                {
                    var PD07 = new PDJig() { PackingPin = OUT.PD_07_PACK, Jig = _PD07/*, PowerPin = OUT.PD_07_POWER_PIN */};
                    PDs.Add(PD07);
                }
                var _PD08 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("PD_08")).FirstOrDefault();
                if (_PD08 != null)
                {
                    var PD08 = new PDJig() { PackingPin = OUT.PD_08_PACK, Jig = _PD08/*, PowerPin = OUT.PD_08_POWER_PIN */};
                    PDs.Add(PD08);
                }
                var _AXIS_01_LOADING_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_01_LOADING_CV_POS")).FirstOrDefault();
                if (_AXIS_01_LOADING_POS != null)
                {
                    var AXIS_01_LOADING_POS = new PDJig() { Jig = _AXIS_01_LOADING_POS };
                    PDs.Add(AXIS_01_LOADING_POS);
                }
                var _AXIS_01_TVOC = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_01_TVOC")).FirstOrDefault();
                if (_AXIS_01_TVOC != null)
                {
                    var AXIS_01_TVOC = new PDJig() { Jig = _AXIS_01_TVOC };
                    PDs.Add(AXIS_01_TVOC);
                }
                var _AXIS_01_NG_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_01_NG_POS")).FirstOrDefault();
                if (_AXIS_01_NG_POS != null)
                {
                    var AXIS_01_NG_POS = new PDJig() { Jig = _AXIS_01_NG_POS };
                    PDs.Add(AXIS_01_NG_POS);
                }
                var _AXIS_01_BUFFER = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_01_BUFFER")).FirstOrDefault();
                if (_AXIS_01_BUFFER != null)
                {
                    var AXIS_01_BUFFER = new PDJig() { PackingPin = OUT.BUFFER_PACK, Jig = _AXIS_01_BUFFER };
                    PDs.Add(AXIS_01_BUFFER);
                }
                var _AXIS_01_SIM_01 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_01_SIM_01")).FirstOrDefault();
                if (_AXIS_01_SIM_01 != null)
                {
                    var AXIS_01_SIM_01 = new PDJig() { Jig = _AXIS_01_SIM_01 };
                    PDs.Add(AXIS_01_SIM_01);
                }
                var _AXIS_01_SIM_02 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_01_SIM_02")).FirstOrDefault();
                if (_AXIS_01_SIM_02 != null)
                {
                    var AXIS_01_SIM_02 = new PDJig() { Jig = _AXIS_01_SIM_02 };
                    PDs.Add(AXIS_01_SIM_02);
                }
                PDsGridView.ItemsSource = PDs;
                foreach (PDJig PD in PDs)
                {
                    PD.Jig.PropertyChanged -= Jig_PropertyChanged;
                    PD.Jig.PropertyChanged += Jig_PropertyChanged;
                }
                using (var Db = new SettingContext())
                {
                    var SavedAxis1Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_01_VELOCITY").FirstOrDefault();
                    if (SavedAxis1Vel != null)
                    {
                        Axis1Velocity = int.Parse(SavedAxis1Vel.Value);
                    }
                    DataContext = this;
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }


        private async void Jig_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

        int _Axis1Velocity;
        public int Axis1Velocity
        {
            get { return _Axis1Velocity; }
            set
            {
                if (value < 100000)
                    _Axis1Velocity = value;
                else
                    _Axis1Velocity = 100000;
                //NotifyPropertyChanged("Axis1Velocity");
                using (var Db = new SettingContext())
                {
                    var SavedAxis1Velocity = Db.ValueSettings.Where(x => x.Key == "AXIS_01_VELOCITY").FirstOrDefault();
                    if (SavedAxis1Velocity != null)
                    {
                        SavedAxis1Velocity.Value = _Axis1Velocity.ToString();
                        Db.SaveChanges();
                    }
                }
            }
        }
        //public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        private async void MovePos_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Axis._01.SON == PinValue.ON && Axis._01.ORG_OK == PinValue.ON &&
                Axis._01.Motioning == PinValue.OFF &&
                IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
            {
                var ClickedButton = sender as Button;
                var PDJig = ClickedButton.DataContext as PDJig;
                await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveSingleAxisAbs, DataFrame.MoveAbsIncData(PDJig.Jig.JigPos, Axis1Velocity));
            }
            else MessageBox.Show("Motor is not ready!", "Error", MessageBoxButton.OK);
        }

        private async void Pack_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var ClickedButton = sender as Button;
            ClickedButton.IsEnabled = false;
            var PDJig = ClickedButton.DataContext as PDJig;
            if (PDJig.PackingPin != null)
                if (PDJig.PackingPin.PinValue == PinValue.OFF)
                    await PDJig.PackingPin.SET();
                else
                    await PDJig.PackingPin.RST();
            ClickedButton.IsEnabled = true;
        }

        private void SavePos_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Save Pos?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var ClickedButton = sender as Button;
                var PDJig = ClickedButton.DataContext as PDJig;
                PDJig.Jig.JigPos = Axis._01.CurrentPos;
            }
        }

        private async void SON_Click(object sender, RoutedEventArgs e)
        {
            if (Axis._01.SON == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.StepEnable, new byte[] { 0x00 });
            else
                await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.StepEnable, new byte[] { 0x01 });
        }

        private async void RESET_Click(object sender, RoutedEventArgs e)
        {
            await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.Reset, null);
        }

        private async void STOP_Click(object sender, RoutedEventArgs e)
        {
            await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveStop, null);
        }

        private async void ORG_Click(object sender, RoutedEventArgs e)
        {
            if (Axis._01.SON == PinValue.ON && Axis._01.Motioning == PinValue.OFF
                && IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveOrigin, null);
            else
                MessageBox.Show("Motor's state is not correct to ORG", "Error", MessageBoxButton.OK);
        }

        private async void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Axis._01.ALM == PinValue.ON || Axis._01.SON == PinValue.OFF || Axis._01.ORG_OK != PinValue.ON)
            {
                MessageBox.Show("Motor is not ready or in alarm state!!!", "Error", MessageBoxButton.OK);
                return;
            }
            if (Axis._01.Motioning == PinValue.OFF)
            {
                var ClickedButton = sender as Button;
                if (ClickedButton == LeftJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(500, 1));
                }
                else if (ClickedButton == FLeftJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(2000, 1));
                }
                else if (ClickedButton == RightJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(500, 0));
                }
                else if (ClickedButton == FRightJog)
                {
                    await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(2000, 0));
                }
            }
        }

        private async void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Motor Stop
            await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveStop, null);
        }

        private async void Lift_01_UpDown_Clicked(object sender, RoutedEventArgs e)
        {
            //Lift 01 Cylinder manual            
            if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                //Down
                await OUT.LOADING_01_LIFT_SOL.SET();
            else
                //Up
                await OUT.LOADING_01_LIFT_SOL.RST();
        }

        private async void LOADING_01_CLAMP_Clicked(object sender, RoutedEventArgs e)
        {   //Clamp manual
            //CLAMP
            var ClickedButton = sender as Button;
            ClickedButton.IsEnabled = false;
            await OUT.LOADING_01_CLAMP_SOL.SET();
            await OUT.LOADING_01_UNCLAMP_SOL.RST();
            ClickedButton.IsEnabled = true;
        }

        private async void LOADING_01_UNCLAMP_Clicked(object sender, RoutedEventArgs e)
        {
            //Clamp manual
            //Unclamp
            Auto.AXIS_01_STEP = 0;
            var ClickedButton = sender as Button;
            await OUT.LOADING_01_UNCLAMP_SOL.SET();
            await OUT.LOADING_01_CLAMP_SOL.RST();
        }

        private async void LOADING_01_TURN_Clicked(object sender, RoutedEventArgs e)
        {
            //Cylinder manual
            //TURN 
            await OUT.LOADING_01_TURN_SOL.SET();
        }

        private async void LOADING_01_RETURN_Clicked(object sender, RoutedEventArgs e)
        {
            ///Rotary Cylinder manual
            //RETURN
            await OUT.LOADING_01_TURN_SOL.RST();
        }

        private async void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveStop, null);
            }
            else if (Axis._01.Motioning == PinValue.OFF && Axis._01.ORG_OK == PinValue.ON)
            {
                if (e.Key == Key.Left)
                    await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(1000, 1));
                else if (e.Key == Key.Right)
                    await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveVelocity, DataFrame.MoveVelocityData(1000, 0));
                else if (e.Key == Key.Down)
                    await OUT.LOADING_01_LIFT_SOL.SET();
                else if (e.Key == Key.Up)
                    await OUT.LOADING_01_LIFT_SOL.RST();
                else if (e.Key == Key.PageUp)
                    await OUT.LOADING_01_TURN_SOL.SET();
                else if (e.Key == Key.PageDown)
                    await OUT.LOADING_01_TURN_SOL.RST();
                else if (e.Key == Key.C)
                {
                    await OUT.LOADING_01_CLAMP_SOL.SET();
                    await OUT.LOADING_01_UNCLAMP_SOL.RST();
                }
                else if (e.Key == Key.V)
                {
                    await OUT.LOADING_01_CLAMP_SOL.RST();
                    await OUT.LOADING_01_UNCLAMP_SOL.SET();
                }
            }
        }

        private async void UserControl_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
                await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveStop, null);
        }

        private void ReadNFC_Clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
