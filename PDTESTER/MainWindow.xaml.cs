using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static GPIOCommunication.GPIOHelper;

namespace PDTESTER
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static MainWindow Root;
        public SWSetting SW = new SWSetting() { SWSettingID = 1 };
        public static Button serialButton;
        public static Button autoButton;
        public static Button manualButton;
        public MainWindow()
        {
            InitializeComponent();
            Root = this;
            serialButton = SerialSettingButton;
            autoButton = AutoButton;
            manualButton = ManualButton;
            List<byte> Bytes = new List<byte>();
            DataContext = this;
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
            DispatcherTimer T = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            T.Start();
            T.Tick += T_Tick;
            LogListView.ItemsSource = TextLogs;
        }


        public static List<JigModel> JigModels;

        private void T_Tick(object sender, EventArgs e)
        {
            Now.Text = DateTime.Now.ToString("HH:mm:ss \ndd/MM/yyyy");
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //s.SendAsync()
            //UdpClient udpClient = new UdpClient(11000);
            using (var Db = new SettingContext())
            {
                #region GetSWSetting
                //var SavedSWSetting = Db.SWSettings.Where(x => x.SWSettingID == 1).FirstOrDefault(); //Search for SWSetting if not found create a new one.
                //if (SavedSWSetting == null)
                //{
                //    var sw = new SWSetting();
                //    Db.Add(sw);
                //    Db.SaveChanges();
                //}
                //else //If Saved setting is found load the saved setting
                //{
                //    SW = SavedSWSetting;
                //    App.SW = SavedSWSetting;
                //}
                string SWITCH_SETTING = Database.GetDbValue("SWITCH_SETTING");
                if (SWITCH_SETTING != null)
                {
                    SW = JsonConvert.DeserializeObject<SWSetting>(SWITCH_SETTING);
                    App.SW = SW;
                }
                else App.SW = SW;
                #endregion
                #region Get SavedPD,SavedTVOC JigSetting
                //TestTimeout
                var TestTimeout = Db.ValueSettings.Where(x => x.Key == "TestTimeout").FirstOrDefault();
                if (TestTimeout == null)
                {
                    Db.Add(new ValueSetting() { Key = "TestTimeout", Value = "180" });
                    await Db.SaveChangesAsync();
                }
                var TVOCBypass = Db.ValueSettings.Where(x => x.Key == "TVOCBypass").FirstOrDefault();
                if (TVOCBypass == null)
                {
                    Db.Add(new ValueSetting() { Key = "TVOCBypass", Value = "0" });
                    await Db.SaveChangesAsync();
                }
                var NFCBypass = Db.ValueSettings.Where(x => x.Key == "NFCBypass").FirstOrDefault();
                if (NFCBypass == null)
                {
                    Db.Add(new ValueSetting() { Key = "NFCBypass", Value = "0" });
                    await Db.SaveChangesAsync();
                }
                var LEAKBypass = Db.ValueSettings.Where(x => x.Key == "LEAKBypass").FirstOrDefault();
                if (LEAKBypass == null)
                {
                    Db.Add(new ValueSetting() { Key = "LEAKBypass", Value = "0" });
                    await Db.SaveChangesAsync();
                }
                var PDBypass = Db.ValueSettings.Where(x => x.Key == "PDBypass").FirstOrDefault();
                if (PDBypass == null)
                {
                    Db.Add(new ValueSetting() { Key = "PDBypass", Value = "0" });
                    await Db.SaveChangesAsync();
                }
                var IsTurnEnable = Db.ValueSettings.Where(x => x.Key == "IsTurnEnable").FirstOrDefault();
                if (IsTurnEnable == null)
                {
                    Db.Add(new ValueSetting() { Key = "IsTurnEnable", Value = "0" });
                    await Db.SaveChangesAsync();
                }
                //AXIS_01 Positions
                var SavedPD01Jig = Db.JigModels.Where(x => x.JigDescription == "PD_01").FirstOrDefault();
                if (SavedPD01Jig == null)
                {
                    var PD01 = new JigModel() { JigDescription = "PD_01", Channel = 1, JigID = 1 };
                    Db.Add(PD01);
                    await Db.SaveChangesAsync();
                }
                var SavedPD02Jig = Db.JigModels.Where(x => x.JigDescription == "PD_02").FirstOrDefault();
                if (SavedPD02Jig == null)
                {
                    var PD02 = new JigModel() { JigDescription = "PD_02", Channel = 2, JigID = 1 };
                    Db.Add(PD02);
                    await Db.SaveChangesAsync();
                }
                var SavedPD03Jig = Db.JigModels.Where(x => x.JigDescription == "PD_03").FirstOrDefault();
                if (SavedPD03Jig == null)
                {
                    var PD03 = new JigModel() { JigDescription = "PD_03", Channel = 1, JigID = 2 };
                    Db.Add(PD03);
                    await Db.SaveChangesAsync();
                }
                var SavedPD04Jig = Db.JigModels.Where(x => x.JigDescription == "PD_04").FirstOrDefault();
                if (SavedPD04Jig == null)
                {
                    var PD04 = new JigModel() { JigDescription = "PD_04", Channel = 2, JigID = 2 };
                    Db.Add(PD04);
                    await Db.SaveChangesAsync();
                }
                var SavedPD05Jig = Db.JigModels.Where(x => x.JigDescription == "PD_05").FirstOrDefault();
                if (SavedPD05Jig == null)
                {
                    var PD05 = new JigModel() { JigDescription = "PD_05", Channel = 1, JigID = 3 };
                    Db.Add(PD05);
                    await Db.SaveChangesAsync();
                }
                var SavedPD06Jig = Db.JigModels.Where(x => x.JigDescription == "PD_06").FirstOrDefault();
                if (SavedPD06Jig == null)
                {
                    var PD06 = new JigModel() { JigDescription = "PD_06", Channel = 2, JigID = 3 };
                    Db.Add(PD06);
                    await Db.SaveChangesAsync();
                }
                var SavedPD07Jig = Db.JigModels.Where(x => x.JigDescription == "PD_07").FirstOrDefault();
                if (SavedPD07Jig == null)
                {
                    var PD07 = new JigModel() { JigDescription = "PD_07", Channel = 1, JigID = 4 };
                    Db.Add(PD07);
                    await Db.SaveChangesAsync();
                }
                var SavedPD08Jig = Db.JigModels.Where(x => x.JigDescription == "PD_08").FirstOrDefault();
                if (SavedPD08Jig == null)
                {
                    var PD08 = new JigModel() { JigDescription = "PD_08", Channel = 2, JigID = 4 };
                    Db.Add(PD08);
                    await Db.SaveChangesAsync();
                }
                var SavedAXIS_01_NG_POS = Db.JigModels.Where(x => x.JigDescription == "AXIS_01_NG_POS").FirstOrDefault();
                if (SavedAXIS_01_NG_POS == null)
                {
                    var AXIS_01_NG_POS = new JigModel() { JigDescription = "AXIS_01_NG_POS", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_01_NG_POS);
                    await Db.SaveChangesAsync();
                }
                var SavedLOADING_CV_POS = Db.JigModels.Where(x => x.JigDescription == "AXIS_01_LOADING_CV_POS").FirstOrDefault();
                if (SavedLOADING_CV_POS == null)
                {
                    var LOADING_CV_POS = new JigModel() { JigDescription = "AXIS_01_LOADING_CV_POS", Channel = 0, JigID = 0 };
                    Db.Add(LOADING_CV_POS);
                    await Db.SaveChangesAsync();
                }
                var SavedAXIS_01_TVOC = Db.JigModels.Where(x => x.JigDescription == "AXIS_01_TVOC").FirstOrDefault();
                if (SavedAXIS_01_TVOC == null)
                {
                    var AXIS_01_TVOC = new JigModel() { JigDescription = "AXIS_01_TVOC", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_01_TVOC);
                    await Db.SaveChangesAsync();
                }
                var SavedAxis01Buffer = Db.JigModels.Where(x => x.JigDescription == "AXIS_01_BUFFER").FirstOrDefault();
                if (SavedAxis01Buffer == null)
                {
                    var Axis01Buffer = new JigModel() { JigDescription = "AXIS_01_BUFFER", Channel = 0, JigID = 0 };
                    Db.Add(Axis01Buffer);
                    await Db.SaveChangesAsync();
                }
                var SavedAXIS_01_SIM_01 = Db.JigModels.Where(x => x.JigDescription == "AXIS_01_SIM_01").FirstOrDefault();
                if (SavedAXIS_01_SIM_01 == null)
                {
                    var AXIS_01_SIM_01 = new JigModel() { JigDescription = "AXIS_01_SIM_01", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_01_SIM_01);
                    await Db.SaveChangesAsync();
                }
                var SavedAXIS_01_SIM_02 = Db.JigModels.Where(x => x.JigDescription == "AXIS_01_SIM_02").FirstOrDefault();
                if (SavedAXIS_01_SIM_02 == null)
                {
                    var AXIS_01_SIM_02 = new JigModel() { JigDescription = "AXIS_01_SIM_02", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_01_SIM_02);
                    await Db.SaveChangesAsync();
                }
                //AXIS_02_POS
                var SavedTRANSFER_CV_POS = Db.JigModels.Where(x => x.JigDescription == "AXIS_02_TRANSFER_CV_POS").FirstOrDefault();
                if (SavedTRANSFER_CV_POS == null)
                {
                    var TRANSFER_CV_POS = new JigModel() { JigDescription = "AXIS_02_TRANSFER_CV_POS", Channel = 0, JigID = 0 };
                    Db.Add(TRANSFER_CV_POS);
                    await Db.SaveChangesAsync();
                }
                var SavedAXIS_02_SIM_01 = Db.JigModels.Where(x => x.JigDescription == "SIM_01").FirstOrDefault();
                if (SavedAXIS_02_SIM_01 == null)
                {
                    var AXIS_02_SIM_01 = new JigModel() { JigDescription = "SIM_01", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_02_SIM_01);
                    await Db.SaveChangesAsync();
                }
                var SavedAXIS_02_SIM_02 = Db.JigModels.Where(x => x.JigDescription == "SIM_02").FirstOrDefault();
                if (SavedAXIS_02_SIM_02 == null)
                {
                    var AXIS_02_SIM_02 = new JigModel() { JigDescription = "SIM_02", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_02_SIM_02);
                    await Db.SaveChangesAsync();
                }
                var SavedAXIS_02_Buffer = Db.JigModels.Where(x => x.JigDescription == "AXIS_02_BUFFER").FirstOrDefault();
                if (SavedAXIS_02_Buffer == null)
                {
                    var AXIS_02_BUFFER = new JigModel() { JigDescription = "AXIS_02_BUFFER", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_02_BUFFER);
                    await Db.SaveChangesAsync();
                }
                //--------------------------------------------------------------------------------------
                var SavedNFC11 = Db.JigModels.Where(x => x.JigDescription == "NFC_01").FirstOrDefault();
                if (SavedNFC11 == null)
                {
                    var NFC11 = new JigModel() { JigDescription = "NFC_01", Channel = 1, JigID = 1 };
                    Db.Add(NFC11);
                    await Db.SaveChangesAsync();
                }
                var SavedNFC12 = Db.JigModels.Where(x => x.JigDescription == "NFC_02").FirstOrDefault();
                if (SavedNFC12 == null)
                {
                    var NFC12 = new JigModel() { JigDescription = "NFC_02", Channel = 2, JigID = 1 };
                    Db.Add(NFC12);
                    await Db.SaveChangesAsync();
                }
                var SavedNFC21 = Db.JigModels.Where(x => x.JigDescription == "NFC_03").FirstOrDefault();
                if (SavedNFC21 == null)
                {
                    var NFC21 = new JigModel() { JigDescription = "NFC_03", Channel = 1, JigID = 2 };
                    Db.Add(NFC21);
                    await Db.SaveChangesAsync();
                }
                var SavedNFC22 = Db.JigModels.Where(x => x.JigDescription == "NFC_04").FirstOrDefault();
                if (SavedNFC22 == null)
                {
                    var NFC22 = new JigModel() { JigDescription = "NFC_04", Channel = 2, JigID = 2 };
                    Db.Add(NFC22);
                    await Db.SaveChangesAsync();
                }
                var SavedAXIS_03_LOADING_CV_POS = Db.JigModels.Where(x => x.JigDescription == "AXIS_03_LOADING_CV_POS").FirstOrDefault();
                if (SavedAXIS_03_LOADING_CV_POS == null)
                {
                    var AXIS_03_LOADING_CV_POS = new JigModel() { JigDescription = "AXIS_03_LOADING_CV_POS", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_03_LOADING_CV_POS);
                    await Db.SaveChangesAsync();
                }
                var SavedAXIS_03_BUFFER = Db.JigModels.Where(x => x.JigDescription == "AXIS_03_BUFFER").FirstOrDefault();
                if (SavedAXIS_03_BUFFER == null)
                {
                    var AXIS_03_BUFFER = new JigModel() { JigDescription = "AXIS_03_BUFFER", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_03_BUFFER);
                    await Db.SaveChangesAsync();
                }
                //---------------------------------------------------------------------------------------
                var SavedAXIS_04_BUFFER = Db.JigModels.Where(x => x.JigDescription == "AXIS_04_BUFFER").FirstOrDefault();
                if (SavedAXIS_04_BUFFER == null)
                {
                    var AXIS_04_BUFFER = new JigModel() { JigDescription = "AXIS_04_BUFFER", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_04_BUFFER);
                    await Db.SaveChangesAsync();
                }
                var SavedLeak11 = Db.JigModels.Where(x => x.JigDescription == "LEAK_01").FirstOrDefault();
                if (SavedLeak11 == null)
                {
                    var Leak11 = new JigModel() { JigDescription = "LEAK_01", Channel = 1, JigID = 1 };
                    Db.Add(Leak11);
                    await Db.SaveChangesAsync();
                }
                var SavedLEAK12 = Db.JigModels.Where(x => x.JigDescription == "LEAK_02").FirstOrDefault();
                if (SavedLEAK12 == null)
                {
                    var LEAK12 = new JigModel() { JigDescription = "LEAK_02", Channel = 2, JigID = 1 };
                    Db.Add(LEAK12);
                    await Db.SaveChangesAsync();
                }
                var SavedLEAK21 = Db.JigModels.Where(x => x.JigDescription == "LEAK_03").FirstOrDefault();
                if (SavedLEAK21 == null)
                {
                    var LEAK21 = new JigModel() { JigDescription = "LEAK_03", Channel = 1, JigID = 2 };
                    Db.Add(LEAK21);
                    await Db.SaveChangesAsync();
                }
                var SavedLEAK22 = Db.JigModels.Where(x => x.JigDescription == "LEAK_04").FirstOrDefault();
                if (SavedLEAK22 == null)
                {
                    var LEAK22 = new JigModel() { JigDescription = "LEAK_04", Channel = 2, JigID = 2 };
                    Db.Add(LEAK22);
                    await Db.SaveChangesAsync();
                }
                var SavedAXIS_04_UNLOADING_CV_POS = Db.JigModels.Where(x => x.JigDescription == "AXIS_04_UNLOADING_CV_POS").FirstOrDefault();
                if (SavedAXIS_04_UNLOADING_CV_POS == null)
                {
                    var AXIS_04_UNLOADING_CV_POS = new JigModel() { JigDescription = "AXIS_04_UNLOADING_CV_POS", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_04_UNLOADING_CV_POS);
                    await Db.SaveChangesAsync();
                }
                var SavedAXIS_04_NG_POS = Db.JigModels.Where(x => x.JigDescription == "AXIS_04_NG_POS").FirstOrDefault();
                if (SavedAXIS_04_NG_POS == null)
                {
                    var AXIS_04_NG_POS = new JigModel() { JigDescription = "AXIS_04_NG_POS", Channel = 0, JigID = 0 };
                    Db.Add(AXIS_04_NG_POS);
                    await Db.SaveChangesAsync();
                }
                //------------------------------------------------------------
                var SavedAxis1Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_01_VELOCITY").FirstOrDefault();
                if (SavedAxis1Vel == null)
                {
                    var Axis1Vel = new ValueSetting() { Key = "AXIS_01_VELOCITY", Value = "30000" };
                    Db.Add(Axis1Vel);
                    await Db.SaveChangesAsync();
                }
                var SavedAxis2Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_02_VELOCITY").FirstOrDefault();
                if (SavedAxis2Vel == null)
                {
                    var Axis2Vel = new ValueSetting() { Key = "AXIS_02_VELOCITY", Value = "30000" };
                    Db.Add(Axis2Vel);
                    await Db.SaveChangesAsync();
                }
                var SavedAxis3Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_03_VELOCITY").FirstOrDefault();
                if (SavedAxis2Vel == null)
                {
                    var Axis3Vel = new ValueSetting() { Key = "AXIS_03_VELOCITY", Value = "30000" };
                    Db.Add(Axis3Vel);
                    await Db.SaveChangesAsync();
                }
                var SavedAxis4Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_04_VELOCITY").FirstOrDefault();
                if (SavedAxis2Vel == null)
                {
                    var Axis4Vel = new ValueSetting() { Key = "AXIS_04_VELOCITY", Value = "30000" };
                    Db.Add(Axis4Vel);
                    await Db.SaveChangesAsync();
                }
                //------------------------------------------------------------
                var UCTPortID = Db.ValueSettings.Where(x => x.Key == "UCTPortID").FirstOrDefault();
                if (UCTPortID == null)
                {
                    Db.ValueSettings.Add(new ValueSetting() { Key = "UCTPortID", Value = "COM1" });
                    Db.SaveChanges();
                }
                var ServoPortID = Db.ValueSettings.Where(x => x.Key == "ServoPortID").FirstOrDefault();
                if (ServoPortID == null)
                {
                    Db.ValueSettings.Add(new ValueSetting() { Key = "ServoPortID", Value = "COM2" });
                    Db.SaveChanges();
                }
                var GPIOPortID = Db.ValueSettings.Where(x => x.Key == "GPIOPortID").FirstOrDefault();
                if (GPIOPortID == null)
                {
                    Db.ValueSettings.Add(new ValueSetting() { Key = "GPIOPortID", Value = "COM3" });
                    Db.SaveChanges();
                }
                var PrinterPortID = Db.ValueSettings.Where(x => x.Key == "PrinterPortID").FirstOrDefault();
                if (PrinterPortID == null)
                {
                    Db.ValueSettings.Add(new ValueSetting() { Key = "PrinterPortID", Value = "COM4" });
                    Db.SaveChanges();
                }
                var TVOCNFCPortID = Db.ValueSettings.Where(x => x.Key == "TVOCNFC_PortID").FirstOrDefault();
                if (TVOCNFCPortID == null)
                {
                    Db.Add(new ValueSetting() { Key = "TVOCNFC_PortID", Value = "COM6" });
                    await Db.SaveChangesAsync();
                }
                var SIM01_NFC_PortID = Db.ValueSettings.Where(x => x.Key == "SIM01_NFC_PortID").FirstOrDefault();
                if (SIM01_NFC_PortID == null)
                {
                    Db.Add(new ValueSetting() { Key = "SIM01_NFC_PortID", Value = "COM4" });
                    await Db.SaveChangesAsync();
                }
                var SIM02_NFC_PortID = Db.ValueSettings.Where(x => x.Key == "SIM02_NFC_PortID").FirstOrDefault();
                if (SIM02_NFC_PortID == null)
                {
                    Db.Add(new ValueSetting() { Key = "SIM02_NFC_PortID", Value = "COM8" });
                    await Db.SaveChangesAsync();
                }
                //--------------NFC Communication----------------------------------------------------------
                var PD_OK_NFC_COM = Db.ValueSettings.Where(x => x.Key == "PD_OK_COM").FirstOrDefault();
                if (PD_OK_NFC_COM == null)
                {
                    Db.ValueSettings.Add(new ValueSetting() { Key = "PD_OK_COM", Value = "COM8" });
                    Db.SaveChanges();
                }
                var NFC_BUFFER_COM = Db.ValueSettings.Where(x => x.Key == "NFC_BUFFER_COM").FirstOrDefault();
                if (NFC_BUFFER_COM == null)
                {
                    Db.ValueSettings.Add(new ValueSetting() { Key = "NFC_BUFFER_COM", Value = "COM9" });
                    Db.SaveChanges();
                }
                var LEAK_OK_COM = Db.ValueSettings.Where(x => x.Key == "LEAK_OK_COM").FirstOrDefault();
                if (LEAK_OK_COM == null)
                {
                    Db.ValueSettings.Add(new ValueSetting() { Key = "LEAK_OK_COM", Value = "COM10" });
                    Db.SaveChanges();
                }
                var LEAK_NG_COM = Db.ValueSettings.Where(x => x.Key == "LEAK_NG_COM").FirstOrDefault();
                if (LEAK_NG_COM == null)
                {
                    Db.ValueSettings.Add(new ValueSetting() { Key = "LEAK_NG_COM", Value = "COM11" });
                    Db.SaveChanges();
                }
                var PD_NG_COM = Db.ValueSettings.Where(x => x.Key == "PD_NG_COM").FirstOrDefault();
                if (PD_NG_COM == null)
                {
                    Db.ValueSettings.Add(new ValueSetting() { Key = "PD_NG_COM", Value = "COM12" });
                    Db.SaveChanges();
                }
                //-------------------------------------------------------------------------------------------
                JigModels = Db.JigModels.ToList();
                #endregion
                try
                {
                    App.UCTCOM = new UCTHelper(UCTPortID.Value);
                    App.GPIOCOM = new GPIOSerial(GPIOPortID.Value);
                    App.ServoCOM = new ServoHelper(ServoPortID.Value);
                    App.PrinterCOM = new SerialHelper(PrinterPortID.Value, 9600, 20, 20, "PrinterHelper");

                    if (App.UCTCOM != null && App.ServoCOM != null && App.GPIOCOM != null)
                    {
                        autoPage = new Auto();
                        Auto.Page.UnpackJig();
                        MainFrame.Navigate(autoPage);
                    }
                }
                catch
                {
                    //serialSetting.ShowDialog();
                    //if (serialSetting.DialogResult == true)
                    //{
                    //    MainFrame.Navigate(autoPage);
                    //}
                }
            }
        }

        private async void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Auto.CancelLoopTask();
            await Task.Delay(200);
            await OUT.LOADING_CV_01_RELAY.RST();
            await OUT.LOADING_CV_02_RELAY.RST();
            await OUT.TRANSFER_CV_RELAY.RST();
            await OUT.OUTPUT_CV_RELAY.RST();
            await OUT.NG_CV_RELAY.RST();
            Application.Current.Shutdown();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Auto autoPage;
        Manual manualPage = new Manual();
        //SerialSetting serialSetting = new SerialSetting();
        //IOMonitor ioPage = new IOMonitor();
        private void Top_Button_Click(object sender, RoutedEventArgs e)
        {
            var ClickedButton = (Button)sender;
            if (ClickedButton == AutoButton)
            {
                //autoPage = new Auto();
                if (autoPage != null)
                    MainFrame.Navigate(autoPage);
            }
            if (ClickedButton == ManualButton)
            {
                MainFrame.Navigate(manualPage);
            }
            if (ClickedButton == IOMonitorButton)
            {
                IOMonitor ioPage = new IOMonitor();
                ioPage.ShowDialog();
            }
            //if (ClickedButton == SerialSettingButton)
            //{
            //    serialSetting = new SerialSetting();
            //    serialSetting.ShowDialog();
            //}
        }
        private object LogLockObject = new object();
        private ObservableCollection<TextLog> TextLogs = new ObservableCollection<TextLog>();
        public async void WriteToLog(string Text)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                lock (LogLockObject)
                {
                    if (TextLogs.Count > 200)
                    {
                        TextLogs.Remove(TextLogs.First());
                    }
                    TextLogs.Add(new TextLog() { Text = String.Format("<{0}> :: {1}", DateTime.Now.ToString("hh:mm:ss dd-MMM-yy"), Text) });
                }
                if (TextLogs.Count > 1) LogListView.ScrollIntoView(TextLogs.Last());
            });
        }

        public async void WriteToLog(TextLog TextLog)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                lock (LogLockObject)
                {
                    if (TextLogs.Count > 200)
                    {
                        TextLogs.Remove(TextLogs.First());
                    }
                    TextLogs.Add(TextLog);
                }
                if (TextLogs.Count > 1) LogListView.ScrollIntoView(TextLogs.Last());
            });
        }
    }
}
