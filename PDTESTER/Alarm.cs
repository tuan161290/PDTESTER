using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GPIOCommunication.GPIOHelper;

namespace PDTESTER
{
    public class Alarm : INotifyPropertyChanged
    {
        public int AlarmTimer { get; set; }
        public string AlarmMessage { get; set; }
        PinValue _AlarmState;
        public PinValue AlarmState
        {
            get { return _AlarmState; }
            set
            {
                if (_AlarmState != value)
                {
                    _AlarmState = value;
                    NotifyPropertyChanged("AlarmState");
                }
            }
        }
        public static async void Buzzing()
        {
            for (int i = 0; i < 3; i++)
            {
                await OUT.BUZZER.SET();
                Thread.Sleep(500);
                await OUT.BUZZER.RST();
                Thread.Sleep(500);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }

    public class Alarms
    {
        public static Alarm Lift_01_Alarm = new Alarm() { AlarmMessage = string.Format("LIFT#1 CYLINDER SENSOR ERROR ({0}/{1})", IN.LOADING_01_LIFT_DOWN_SENSOR.GPIOLabel, IN.LOADING_01_LIFT_UP_SENSOR.GPIOLabel) };
        public static Alarm Lift_02_Alarm = new Alarm() { AlarmMessage = string.Format("LIFT#2 CYLINDER SENSOR ERROR ({0}/{1})", IN.LOADING_02_LIFT_DOWN_SENSOR.GPIOLabel, IN.LOADING_02_LIFT_UP_SENSOR.GPIOLabel) };
        public static Alarm Lift_03_Alarm = new Alarm() { AlarmMessage = string.Format("LIFT#3 CYLINDER SENSOR ERROR ({0}/{1})", IN.LOADING_03_LIFT_DOWN_SENSOR.GPIOLabel, IN.LOADING_03_LIFT_UP_SENSOR.GPIOLabel) };
        public static Alarm Lift_04_Alarm = new Alarm() { AlarmMessage = string.Format("LIFT#4 CYLINDER SENSOR ERROR ({0}/{1})", IN.LOADING_04_LIFT_DOWN_SENSOR.GPIOLabel, IN.LOADING_04_LIFT_UP_SENSOR.GPIOLabel) };
        public static Alarm Clamp_01_Alarm = new Alarm() { AlarmMessage = string.Format("CLAMP#1 CYLINDER SENSOR ERROR ({0}/{1})", IN.LOADING_01_CLAMP_SENSOR.GPIOLabel, IN.LOADING_01_UNCLAMP_SENSOR.GPIOLabel) };
        public static Alarm Clamp_02_Alarm = new Alarm() { AlarmMessage = string.Format("CLAMP#2 CYLINDER SENSOR ERROR ({0}/{1})", IN.LOADING_02_CLAMP_SENSOR.GPIOLabel, IN.LOADING_02_UNCLAMP_SENSOR.GPIOLabel) };
        public static Alarm Clamp_03_Alarm = new Alarm() { AlarmMessage = string.Format("CLAMP#3 CYLINDER SENSOR ERROR ({0}/{1})", IN.LOADING_03_CLAMP_SENSOR.GPIOLabel, IN.LOADING_03_UNCLAMP_SENSOR.GPIOLabel) };
        public static Alarm Clamp_04_Alarm = new Alarm() { AlarmMessage = string.Format("CLAMP#4 CYLINDER SENSOR ERROR ({0}/{1})", IN.LOADING_04_CLAMP_SENSOR.GPIOLabel, IN.LOADING_04_UNCLAMP_SENSOR.GPIOLabel) };
        public static Alarm Leak01PressError = new Alarm() { AlarmMessage = string.Format("LEAK#1 PRESS CYLINDER SENSOR ERROR ({0}/{1})", IN.LEAK_01_PRESS_UP_SENSOR.GPIOLabel, IN.LEAK_01_PRESS_DOWN_SENSOR.GPIOLabel) };
        public static Alarm Leak02PressError = new Alarm() { AlarmMessage = string.Format("LEAK#2 PRESS CYLINDER SENSOR ERROR ({0}/{1})", IN.LEAK_02_PRESS_UP_SENSOR.GPIOLabel, IN.LEAK_02_PRESS_DOWN_SENSOR.GPIOLabel) };
        public static Alarm Leak03PressError = new Alarm() { AlarmMessage = string.Format("LEAK#3 PRESS CYLINDER SENSOR ERROR ({0}/{1})", IN.LEAK_03_PRESS_UP_SENSOR.GPIOLabel, IN.LEAK_03_PRESS_DOWN_SENSOR.GPIOLabel) };
        public static Alarm Leak04PressError = new Alarm() { AlarmMessage = string.Format("LEAK#4 PRESS CYLINDER SENSOR ERROR ({0}/{1})", IN.LEAK_04_PRESS_UP_SENSOR.GPIOLabel, IN.LEAK_04_PRESS_DOWN_SENSOR.GPIOLabel) };
        public static Alarm Leak01TransError = new Alarm() { AlarmMessage = string.Format("LEAK#1 TRANS CYLINDER SENSOR ERROR ({0}/{1})", IN.LEAK_01_FOWARD_SENSOR.GPIOLabel, IN.LEAK_01_REVERSE_SENSOR.GPIOLabel) };
        public static Alarm Leak02TransError = new Alarm() { AlarmMessage = string.Format("LEAK#2 TRANS CYLINDER SENSOR ERROR ({0}/{1})", IN.LEAK_02_FOWARD_SENSOR.GPIOLabel, IN.LEAK_02_REVERSE_SENSOR.GPIOLabel) };
        public static Alarm Leak03TransError = new Alarm() { AlarmMessage = string.Format("LEAK#3 TRANS CYLINDER SENSOR ERROR ({0}/{1})", IN.LEAK_03_FOWARD_SENSOR.GPIOLabel, IN.LEAK_03_REVERSE_SENSOR.GPIOLabel) };
        public static Alarm Leak04TransError = new Alarm() { AlarmMessage = string.Format("LEAK#4 TRANS CYLINDER SENSOR ERROR ({0}/{1})", IN.LEAK_04_FOWARD_SENSOR.GPIOLabel, IN.LEAK_04_REVERSE_SENSOR.GPIOLabel) };
        public static Alarm Axis01TurnError = new Alarm() { AlarmMessage = string.Format("AXIS#1 TURN CYLINDER ERROR ({0}/{1})", IN.LOADING_01_TURN_SENSOR.GPIOLabel, IN.LOADING_01_RETURN_SENSOR.GPIOLabel) };
        public static Alarm Axis02TurnError = new Alarm() { AlarmMessage = string.Format("Axis#2 TURN CYLINDER ERROR ({0}/{1})", IN.LOADING_02_TURN_SENSOR.GPIOLabel, IN.LOADING_02_TURN_SENSOR.GPIOLabel) };

        public static Alarm SIM01PressError = new Alarm() { AlarmMessage = string.Format("SIM#1 PRESS CYLINDER SENSOR ERROR ({0}/{1})", IN.SIM_01_PRESS_SENSOR.GPIOLabel, IN.SIM_01_RELEASE_SENSOR.GPIOLabel) };
        public static Alarm SIM02PressError = new Alarm() { AlarmMessage = string.Format("SIM#2 PRESS CYLINDER SENSOR ERROR ({0}/{1})", IN.SIM_02_PRESS_SENSOR.GPIOLabel, IN.SIM_02_RELEASE_SENSOR.GPIOLabel) };
        public static Alarm SIM01PackError = new Alarm() { AlarmMessage = string.Format("SIM#1 PACK CYLINDER SENSOR ERROR ({0}/{1})", IN.SIM_01_PACK_IN_SENSOR.GPIOLabel, IN.SIM_01_PACK_OUT_SENSOR.GPIOLabel) };
        public static Alarm SIM02PackError = new Alarm() { AlarmMessage = string.Format("SIM#2 PACK CYLINDER SENSOR ERROR ({0}/{1})", IN.SIM_02_PACK_IN_SENSOR.GPIOLabel, IN.SIM_02_PACK_OUT_SENSOR.GPIOLabel) };

        //public static Alarm Axis01Alarm = new Alarm() { AlarmMessage = string.Format("Axis 01 Error") };
        //public static Alarm Axis02Alarm = new Alarm() { AlarmMessage = string.Format("Axis 02 Error") };
        //public static Alarm Axis03Alarm = new Alarm() { AlarmMessage = string.Format("Axis 03 Error") };
        //public static Alarm Axis04Alarm = new Alarm() { AlarmMessage = string.Format("Axis 04 Error") };
    }
}
