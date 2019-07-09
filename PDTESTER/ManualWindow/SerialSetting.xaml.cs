using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PDTESTER.ManualWindow
{
    /// <summary>
    /// Interaction logic for SerialSetting.xaml
    /// </summary>
    public partial class SerialSetting : UserControl
    {
        public SWSetting SW { get; set; } = new SWSetting();


        public SerialSetting()
        {
            InitializeComponent();
            Loaded += SerialSetting_Loaded;
            string SWITCH_SETTING = Database.GetDbValue("SWITCH_SETTING");
            if (SWITCH_SETTING != null)
            {
                SW = JsonConvert.DeserializeObject<SWSetting>(SWITCH_SETTING);
                App.SW = SW;
            }
            else App.SW = SW;
        }

        private void NVValues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (object o in e.NewItems)
                {
                    ((NVValue)o).NVResult = "N";
                    ((NVValue)o).IsEnable = true;
                }
        }

        public ObservableCollection<NVValue> NVValues { get; set; }

        private void SerialSetting_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<COMDevice> COMDevices = new ObservableCollection<COMDevice>()
            {
                new COMDevice(){ Device = "UCTPortID",
                                 SelectedItem = Database.GetDbValue("UCTPortID")},
                new COMDevice(){ Device = "ServoPortID",
                                 SelectedItem = Database.GetDbValue("ServoPortID")},
                new COMDevice(){ Device = "GPIOPortID",
                                 SelectedItem = Database.GetDbValue("GPIOPortID")},
                new COMDevice(){ Device = "PrinterPortID",
                                 SelectedItem = Database.GetDbValue("PrinterPortID")},
                //new COMDevice(){ Device = "TVOCNFC_PortID",
                //                 SelectedItem = Database.GetDbValue("TVOCNFC_PortID")},
                new COMDevice(){ Device = "SIM01_NFC_PortID",
                                 SelectedItem = Database.GetDbValue("SIM01_NFC_PortID")},
                new COMDevice(){ Device = "SIM02_NFC_PortID",
                                 SelectedItem = Database.GetDbValue("SIM02_NFC_PortID")},
                //new COMDevice(){ Device = "SkipCheckNFC_PortID",
                //                 SelectedItem = Database.GetDbValue("SkipCheckNFC_PortID")},
            };
            ListCOMDevice.ItemsSource = COMDevices;
            string Str_SavedNVValue = Database.GetDbValue("NVValues");
            if (!string.IsNullOrEmpty(Str_SavedNVValue))
            {
                var SavedNVValues = JsonConvert.DeserializeObject<ObservableCollection<NVValue>>(Str_SavedNVValue);
                if (SavedNVValues != null)
                    NVValues = SavedNVValues;
            }
            else
                NVValues = new ObservableCollection<NVValue>();
            NVValues.CollectionChanged += NVValues_CollectionChanged;
            ListNVValues.ItemsSource = NVValues;
            DataContext = this;
        }

        private string _TimeOutText;
        public string TimeOutText
        {
            get
            {
                using (var Db = new SettingContext())
                {
                    var DbTimeOut = Db.ValueSettings.Where(x => x.Key == "TestTimeout").FirstOrDefault();
                    if (DbTimeOut != null)
                    {
                        _TimeOutText = DbTimeOut.Value;
                    }
                    else _TimeOutText = "180";
                }
                return _TimeOutText;
            }
            set
            {
                _TimeOutText = value;
                using (var Db = new SettingContext())
                {
                    var SaveTimeout = Db.ValueSettings.Where(x => x.Key.Contains("TestTimeout")).FirstOrDefault();
                    if (SaveTimeout != null)
                    {
                        SaveTimeout.Value = _TimeOutText;
                        Db.SaveChanges();
                    }
                }
            }
        }
        bool _NFCBypass;
        public bool NFCBypass
        {
            get
            {
                _NFCBypass = GetBoolValue("NFCBypass");
                return _NFCBypass;
            }
            set
            {
                if (_NFCBypass != value)
                {
                    _NFCBypass = value;
                    SetBoolValue("NFCBypass", _NFCBypass);
                }
            }
        }
        bool _LEAKBypass;
        public bool LEAKBypass
        {
            get
            {
                _LEAKBypass = GetBoolValue("LEAKBypass");
                return _LEAKBypass;
            }
            set
            {
                if (_LEAKBypass != value)
                {
                    _LEAKBypass = value;
                    SetBoolValue("LEAKBypass", _LEAKBypass);
                }
            }

        }
        bool _TVOCBypass;
        public bool TVOCBypass
        {
            get
            {
                _TVOCBypass = GetBoolValue("TVOCBypass");
                return _TVOCBypass;
            }
            set
            {
                if (_TVOCBypass != value)
                {
                    _TVOCBypass = value;
                    SetBoolValue("TVOCBypass", _TVOCBypass);
                }
            }
        }
        bool _PDBypass;
        public bool PDBypass
        {
            get
            {
                _PDBypass = GetBoolValue("PDBypass");
                return _PDBypass;
            }
            set
            {
                if (_PDBypass != value)
                {
                    _PDBypass = value;
                    SetBoolValue("PDBypass", value);
                }
            }
        }
        bool _TVOCTurn;
        public bool TVOCTurn
        {
            get
            {
                _TVOCTurn = GetBoolValue("IsTurnEnable");
                return _TVOCTurn;
            }
            set
            {
                if (_TVOCTurn != value)
                {
                    _TVOCTurn = value;
                    SetBoolValue("IsTurnEnable", value);
                }
            }
        }
        bool _UCT8Mode;
        public bool UCT8Mode
        {
            get
            {
                _UCT8Mode = GetBoolValue("UCT8Mode");
                return _UCT8Mode;
            }
            set
            {
                if (_UCT8Mode != value)
                {
                    _UCT8Mode = value;
                    SetBoolValue("UCT8Mode", _UCT8Mode);
                }
            }
        }

        public static bool GetBoolValue(string Key)
        {
            using (var Db = new SettingContext())
            {
                var DbValue = Db.ValueSettings.Where(x => x.Key == Key).FirstOrDefault();
                if (DbValue != null)
                {
                    return (DbValue.Value == "1");
                }
                return false;
            }
        }
        public static void SetBoolValue(string Key, bool Bool)
        {
            using (var Db = new SettingContext())
            {
                var DbValue = Db.ValueSettings.Where(x => x.Key == Key).FirstOrDefault();
                if (DbValue != null)
                {
                    if (Bool)
                        DbValue.Value = "1";
                    else
                        DbValue.Value = "0";
                    Db.SaveChanges();
                }
                else
                {
                    Db.Add(new ValueSetting() { Key = Key, Value = "0" });
                    Db.SaveChanges();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        object SerialPortLockObject = new object();
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            var COMDevice = ((Button)sender).DataContext as COMDevice;
            if (COMDevice != null)
            {
                try
                {
                    Database.SetDbValue(COMDevice.Device, COMDevice.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void FreeMode_Clicked(object sender, RoutedEventArgs e)
        {
            Manual.CancelLoopTask();
            FreeMode FreeMode = new FreeMode();
            FreeMode.ShowDialog();
            Auto.Origin = false;
            Manual.ServoLoop();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Database.SetDbValue("NVValues", JsonConvert.SerializeObject(NVValues));
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            string s = JsonConvert.SerializeObject(SW);
            Database.SetDbValue("SWITCH_SETTING", s);
            App.SW = SW;
        }
    }

    public class COMDevice : INotifyPropertyChanged
    {
        public static ObservableCollection<string> PortNames { get; set; } = new ObservableCollection<string>(SerialPort.GetPortNames().ToList());
        public string Device { get; set; }
        public string SelectedCOM { get; set; }
        public string SelectedItem { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChange(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
