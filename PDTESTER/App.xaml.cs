using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using static GPIOCommunication.GPIOHelper;

namespace PDTESTER
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SWSetting SW { get; set; } = new SWSetting();
        //public static UCTControl UCTCOM = null;
        public static ServoHelper ServoCOM { get; set; } = null;
        public static GPIOSerial GPIOCOM { get; set; } = null;
        public static UCTHelper UCTCOM { get; set; } = null;
        public static SerialHelper PrinterCOM { get; set; } = null;

        public static GPIOBoard GPIOBoardF0 { get; set; } = new GPIOBoard(0xF0)
        {
            OutputPins = new ObservableCollection<OutputPin>()
            {
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO00, GPIODescription = "PD_01_PACK", GPIOLabel = "OUF0_00"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO01, GPIODescription = "PD_02_PACK", GPIOLabel = "OUF0_01"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO02, GPIODescription = "PD_03_PACK", GPIOLabel = "OUF0_02"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO03, GPIODescription = "PD_04_PACK", GPIOLabel = "OUF0_03",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO04, GPIODescription = "PD_05_PACK", GPIOLabel = "OUF0_04",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO05, GPIODescription = "PD_06_PACK", GPIOLabel = "OUF0_05",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO06, GPIODescription = "PD_07_PACK", GPIOLabel = "OUF0_06",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO07, GPIODescription = "PD_08_PACK", GPIOLabel = "OUF0_07",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO08, GPIODescription = "NFC_01_PACK", GPIOLabel = "OUF0_08",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO09, GPIODescription = "NFC_02_PACK", GPIOLabel = "OUF0_09",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO10, GPIODescription = "NFC_03_PACK", GPIOLabel = "OUF0_10",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO11, GPIODescription = "NFC_04_PACK", GPIOLabel = "OUF0_11",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO12, GPIODescription = "LEAK_01_PACK", GPIOLabel = "OUF0_12",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO13, GPIODescription = "LEAK_02_PACK", GPIOLabel = "OUF0_13",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO14, GPIODescription = "LEAK_03_PACK", GPIOLabel = "OUF0_14",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO15, GPIODescription = "LEAK_04_PACK", GPIOLabel = "OUF0_15",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO16, GPIODescription = "LEAK_01_PRESS_SOL", GPIOLabel = "OUF0_16",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO17, GPIODescription = "LEAK_01_TRANS_SOL", GPIOLabel = "OUF0_17",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO18, GPIODescription = "LEAK_02_PRESS_SOL", GPIOLabel = "OUF0_18",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO19, GPIODescription = "LEAK_02_TRANS_SOL", GPIOLabel = "OUF0_19",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO20, GPIODescription = "LEAK_03_PRESS_SOL", GPIOLabel = "OUF0_20",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO21, GPIODescription = "LEAK_03_TRANS_SOL", GPIOLabel = "OUF0_21",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO22, GPIODescription = "LEAK_04_PRESS_SOL", GPIOLabel = "OUF0_22",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO23, GPIODescription = "LEAK_04_TRANS_SOL", GPIOLabel = "OUF0_23",},
            },
            InputPins = new ObservableCollection<InputPin>()
            {
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO00, GPIODescription = "NFC_01_OK", GPIOLabel = "INF0_00"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO01, GPIODescription = "NFC_01_NG", GPIOLabel = "INF0_01"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO02, GPIODescription = "NFC_02_OK", GPIOLabel = "INF0_02"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO03, GPIODescription = "NFC_02_NG", GPIOLabel = "INF0_03"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO04, GPIODescription = "NFC_03_OK", GPIOLabel = "INF0_04"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO05, GPIODescription = "NFC_03_NG", GPIOLabel = "INF0_05"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO06, GPIODescription = "NFC_04_OK", GPIOLabel = "INF0_06"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO07, GPIODescription = "NFC_04_NG", GPIOLabel = "INF0_07"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO08, GPIODescription = "LEAK_01_OK", GPIOLabel = "INF0_08"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO09, GPIODescription = "LEAK_01_NG", GPIOLabel = "INF0_09"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO10, GPIODescription = "LEAK_02_OK", GPIOLabel = "INF0_10"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO11, GPIODescription = "LEAK_02_NG", GPIOLabel = "INF0_11"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO12, GPIODescription = "LEAK_03_OK", GPIOLabel = "INF0_12"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO13, GPIODescription = "LEAK_03_NG", GPIOLabel = "INF0_13"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO14, GPIODescription = "LEAK_04_OK", GPIOLabel = "INF0_14"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO15, GPIODescription = "LEAK_04_NG", GPIOLabel = "INF0_15"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO16, GPIODescription = "LEAK_01_PRESS_UP_SENSOR", GPIOLabel = "INF0_16"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO17, GPIODescription = "LEAK_01_PRESS_DOWN_SENSOR", GPIOLabel = "INF0_17"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO18, GPIODescription = "LEAK_01_FOWARD_SENSOR", GPIOLabel = "INF0_18"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO19, GPIODescription = "LEAK_01_REVERSE_SENSOR", GPIOLabel = "INF0_19"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO20, GPIODescription = "LEAK_02_PRESS_UP_SENSOR", GPIOLabel = "INF0_20"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO21, GPIODescription = "LEAK_02_PRESS_DOWN_SENSOR", GPIOLabel = "INF0_21"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO22, GPIODescription = "LEAK_02_FOWARD_SENSOR", GPIOLabel = "INF0_22"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO23, GPIODescription = "LEAK_02_REVERSE_SENSOR", GPIOLabel = "INF0_23"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO24, GPIODescription = "LEAK_03_PRESS_UP_SENSOR", GPIOLabel = "INF0_24"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO25, GPIODescription = "LEAK_03_PRESS_DOWN_SENSOR", GPIOLabel = "INF0_25"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO26, GPIODescription = "LEAK_03_FOWARD_SENSOR", GPIOLabel = "INF0_26"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO27, GPIODescription = "LEAK_03_REVERSE_SENSOR", GPIOLabel = "INF0_27"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO28, GPIODescription = "LEAK_04_PRESS_UP_SENSOR", GPIOLabel = "INF0_28"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO29, GPIODescription = "LEAK_04_PRESS_DOWN_SENSOR", GPIOLabel = "INF0_29"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO30, GPIODescription = "LEAK_04_FOWARD_SENSOR", GPIOLabel = "INF0_30"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO31, GPIODescription = "LEAK_04_REVERSE_SENSOR", GPIOLabel = "INF0_31"},
            }
        };
        public static GPIOBoard GPIOBoardF1 { get; set; } = new GPIOBoard(0xF1)
        {
            OutputPins = new ObservableCollection<OutputPin>()
            {
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO00, GPIODescription = "TVOC_START", GPIOLabel = "OUF1_00"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO01, GPIODescription = "RED_LIGHT", GPIOLabel = "OUF1_01"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO02, GPIODescription = "ORANGE_LIGHT", GPIOLabel = "OUF1_02"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO03, GPIODescription = "GREEN_LIGHT", GPIOLabel = "OUF1_03"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO04, GPIODescription = "BUZZER", GPIOLabel = "OUF1_04"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO05, GPIODescription = "LOADING_CV_01_RELAY", GPIOLabel = "OUF1_05"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO06, GPIODescription = "TRANSFER_CV_RELAY", GPIOLabel = "OUF1_06"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO07, GPIODescription = "NG_CV_RELAY", GPIOLabel = "OUF1_07"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO08, GPIODescription = "OUTPUT(UNLOADING)_CV_RELAY", GPIOLabel = "OUF1_08"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO09, GPIODescription = "LOADING_01_LIFT_SOL", GPIOLabel = "OUF1_09"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO10, GPIODescription = "LOADING_01_CLAMP_SOL", GPIOLabel = "OUF1_10"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO11, GPIODescription = "LOADING_01_UNCLAMP_SOL", GPIOLabel = "OUF1_11"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO12, GPIODescription = "LOADING_02_LIFT_SOL", GPIOLabel = "OUF1_12"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO13, GPIODescription = "LOADING_02_CLAMP_SOL", GPIOLabel = "OUF1_13"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO14, GPIODescription = "LOADING_02_UNCLAMP_SOL", GPIOLabel = "OUF1_14"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO15, GPIODescription = "LOADING_02_TURN_SOL", GPIOLabel = "OUF1_15"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO16, GPIODescription = "PD_01_POWER_PIN", GPIOLabel = "OUF1_16"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO17, GPIODescription = "PD_02_POWER_PIN", GPIOLabel = "OUF1_17"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO18, GPIODescription = "PD_03_POWER_PIN", GPIOLabel = "OUF1_18"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO19, GPIODescription = "PD_04_POWER_PIN", GPIOLabel = "OUF1_19"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO20, GPIODescription = "PD_05_POWER_PIN", GPIOLabel = "OUF1_20"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO21, GPIODescription = "PD_06_POWER_PIN", GPIOLabel = "OUF1_21"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO22, GPIODescription = "PD_07_POWER_PIN", GPIOLabel = "OUF1_22"},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO23, GPIODescription = "PD_08_POWER_PIN", GPIOLabel = "OUF1_23"},
            },
            InputPins = new ObservableCollection<InputPin>()
            {
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO00, GPIODescription = "TVOC_OK_SIGNAL", GPIOLabel = "INF1_00"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO01, GPIODescription = "TVOC_NG_SIGNAL", GPIOLabel = "INF1_01"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO02, GPIODescription = "TVOC_TEST", GPIOLabel = "INF1_02"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO03, GPIODescription = "OUTPUT_CV_02_SENSOR", GPIOLabel = "INF1_03"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO04, GPIODescription = "OUTPUT_CV_03_SENSOR", GPIOLabel = "INF1_04"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO05, GPIODescription = "LOADING_01_LIFT_UP_SENSOR", GPIOLabel = "INF1_05"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO06, GPIODescription = "LOADING_01_LIFT_DOWN_SENSOR", GPIOLabel = "INF1_06"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO07, GPIODescription = "LOADING_02_UP_SENSOR", GPIOLabel = "INF1_07"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO08, GPIODescription = "LOADING_02_DOWN_SENSOR", GPIOLabel = "INF1_08"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO09, GPIODescription = "LOADING_02_TURN_SENSOR", GPIOLabel = "INF1_09"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO10, GPIODescription = "LOADING_02_RETURN_SENSOR", GPIOLabel = "INF1_10"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO11, GPIODescription = "INPUT_CV_SENSOR_02", GPIOLabel = "INF1_11"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO12, GPIODescription = "TRANSFER_CV_SENSOR_BEGIN", GPIOLabel = "INF1_12"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO13, GPIODescription = "TRANSFER_CV_SENSOR_END", GPIOLabel = "INF1_13"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO14, GPIODescription = "NG_CV_SENSOR_01", GPIOLabel = "INF1_14"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO15, GPIODescription = "NG_CV_SENSOR_02", GPIOLabel = "INF1_15"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO16, GPIODescription = "NG_CV_SENSOR_03", GPIOLabel = "INF1_16"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO17, GPIODescription = "OUTPUT(UNLOADING)_CV_SENSOR", GPIOLabel = "INF1_17"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO18, GPIODescription = "DOOR_SENSOR_01", GPIOLabel = "INF1_18"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO19, GPIODescription = "DOOR_SENSOR_02", GPIOLabel = "INF1_19" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO20, GPIODescription = "START_BUTTON_01", GPIOLabel = "INF1_20" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO21, GPIODescription = "STOP_BUTTON_01", GPIOLabel = "INF1_21" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO22, GPIODescription = "ESTOP_BUTTON_01", GPIOLabel = "INF1_22" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO23, GPIODescription = "START_BUTTON_02", GPIOLabel = "INF1_23" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO24, GPIODescription = "STOP_BUTTON_02", GPIOLabel = "INF1_24" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO25, GPIODescription = "ESTOP_BUTTON_02", GPIOLabel = "INF1_25" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO26, GPIODescription = "INPUT_CV_SENSOR_01", GPIOLabel = "INF1_26" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO27, GPIODescription = "NG_CV_SENSOR_04", GPIOLabel = "INF1_27" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO28, GPIODescription = "LOADING_01_TURN_SENSOR", GPIOLabel = "INF1_28" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO29, GPIODescription = "LOADING_01_RETURN_SENSOR", GPIOLabel = "INF1_29" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO30, GPIODescription = "REVERSED", GPIOLabel = "INF1_30" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO31, GPIODescription = "REVERSED", GPIOLabel = "INF1_31" },
            }
        };
        public static GPIOBoard GPIOBoardF2 { get; set; } = new GPIOBoard(0xF2)
        {
            //GPIOStation = 0xF2,
            OutputPins = new ObservableCollection<OutputPin>()
            {
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO00, GPIODescription = "LOADING_03_LIFT_SOL_01", GPIOLabel = "OUF2_00",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO01, GPIODescription = "LOADING_03_LIFT_SOL_02", GPIOLabel = "OUF2_01",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO02, GPIODescription = "LOADING_03_CLAMP_SOL", GPIOLabel = "OUF2_02",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO03, GPIODescription = "LOADING_03_UNCLAMP_SOL", GPIOLabel = "OUF2_03",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO04, GPIODescription = "LOADING_04_LIFT_SOL_01", GPIOLabel = "OUF2_04",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO05, GPIODescription = "LOADING_04_LIFT_SOL_02", GPIOLabel = "OUF2_05",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO06, GPIODescription = "LOADING_04_CLAMP_SOL", GPIOLabel = "OUF2_06",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO07, GPIODescription = "LOADING_04_UNCLAMP_SOL", GPIOLabel = "OUF2_07",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO08, GPIODescription = "START_BUTTON_01_LED", GPIOLabel = "OUF2_08",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO09, GPIODescription = "STOP_BUTTON_01_LED", GPIOLabel = "OUF2_09",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO10, GPIODescription = "START_BUTTON_02_LED", GPIOLabel = "OUF2_10",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO11, GPIODescription = "STOP_BUTTON_02_LED", GPIOLabel = "OUF2_11",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO12, GPIODescription = "BUFFER_PACK", GPIOLabel = "OUF2_12",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO13, GPIODescription = "LOADING_CV_02_RELAY", GPIOLabel = "OUF2_13",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO14, GPIODescription = "LOADING_01_TURN_SOL", GPIOLabel = "OUF2_14",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO15, GPIODescription = "SIM_01_PACK_SOL", GPIOLabel = "OUF2_15",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO16, GPIODescription = "SIM_01_PRESS_SOL", GPIOLabel = "OUF2_16",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO17, GPIODescription = "SIM_02_PACK_SOL", GPIOLabel = "OUF2_17",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO18, GPIODescription = "SIM_02_PRESS_SOL", GPIOLabel = "OUF2_18",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO19, GPIODescription = "REVERSED", GPIOLabel = "OUF2_19",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO20, GPIODescription = "REVERSED", GPIOLabel = "OUF2_20",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO21, GPIODescription = "REVERSED", GPIOLabel = "OUF2_21",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO22, GPIODescription = "REVERSED", GPIOLabel = "OUF2_22",},
                new OutputPin(){GPIOBitmask = GPIOBitmask.GPIO23, GPIODescription = "REVERSED", GPIOLabel = "OUF2_23",},

            },
            InputPins = new ObservableCollection<InputPin>()
            {
                new InputPin(){ GPIOBitmask = GPIOBitmask.GPIO00, GPIODescription = "LOADING_01_CLAMP_SENSOR", GPIOLabel = "INF2_00" },
                new InputPin(){ GPIOBitmask = GPIOBitmask.GPIO01, GPIODescription = "LOADING_01_UNCLAMP_SENSOR", GPIOLabel = "INF2_01" },
                new InputPin(){ GPIOBitmask = GPIOBitmask.GPIO02, GPIODescription = "LOADING_02_CLAMP_SENSOR", GPIOLabel = "INF2_02" },
                new InputPin(){ GPIOBitmask = GPIOBitmask.GPIO03, GPIODescription = "LOADING_02_UNCLAMP_SENSOR", GPIOLabel = "INF2_03" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO04, GPIODescription = "LOADING_03_CLAMP_SENSOR", GPIOLabel = "INF2_04"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO05, GPIODescription = "LOADING_03_UNCLAMP_SENSOR", GPIOLabel = "INF2_05"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO06, GPIODescription = "LOADING_03_LIFT_UP_SENSOR", GPIOLabel = "INF2_06"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO07, GPIODescription = "LOADING_03_LIFT_DOWN_SENSOR", GPIOLabel = "INF2_07"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO08, GPIODescription = "REVERSED", GPIOLabel = "INF2_08"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO09, GPIODescription = "REVERSED", GPIOLabel = "INF2_09"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO10, GPIODescription = "LOADING_04_CLAMP_SENSOR", GPIOLabel = "INF2_10"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO11, GPIODescription = "LOADING_04_UNCLAMP_SENSOR", GPIOLabel = "INF2_11"},
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO12, GPIODescription = "LOADING_04_LIFT_UP_SENSOR", GPIOLabel = "INF2_12" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO13, GPIODescription = "LOADING_04_LIFT_DOWN_SENSOR", GPIOLabel = "INF2_13" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO14, GPIODescription = "SIM_01_PACK_IN_SENSOR", GPIOLabel = "INF2_14" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO15, GPIODescription = "SIM_01_PACK_OUT_SENSOR", GPIOLabel = "INF2_15" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO16, GPIODescription = "SIM_01_PRESS_SENSOR", GPIOLabel = "INF2_16" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO17, GPIODescription = "SIM_01_RELEASE_SENSOR", GPIOLabel = "INF2_17" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO18, GPIODescription = "SIM_02_PACK_IN_SENSOR", GPIOLabel = "INF2_18" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO19, GPIODescription = "SIM_02_PACK_OUT_SENSOR", GPIOLabel = "INF2_19" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO20, GPIODescription = "SIM_02_PRESS_SENSOR", GPIOLabel = "INF2_20" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO21, GPIODescription = "SIM_02_RELEASE_SENSOR", GPIOLabel = "INF2_21" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO22, GPIODescription = "REVERSED", GPIOLabel = "INF2_22" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO23, GPIODescription = "REVERSED", GPIOLabel = "INF2_23" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO24, GPIODescription = "REVERSED", GPIOLabel = "INF2_24" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO25, GPIODescription = "REVERSED", GPIOLabel = "INF2_25" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO26, GPIODescription = "REVERSED", GPIOLabel = "INF2_26" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO27, GPIODescription = "REVERSED", GPIOLabel = "INF2_27" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO28, GPIODescription = "REVERSED", GPIOLabel = "INF2_28" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO29, GPIODescription = "REVERSED", GPIOLabel = "INF2_29" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO30, GPIODescription = "REVERSED", GPIOLabel = "INF2_30" },
                new InputPin(){GPIOBitmask = GPIOBitmask.GPIO31, GPIODescription = "REVERSED", GPIOLabel = "INF2_31" },
            }
        };

        public static event PropertyChangedEventHandler StaticPropertyChanged;
        private static void NotifyStaticPropertyChanged([CallerMemberName] string name = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
        }

        public App()
        {
            Foo();
            using (SettingContext Db = new SettingContext())
                Db.Database.Migrate();
            foreach (OutputPin OutputPin in GPIOBoardF0.OutputPins)
                OutputPin.Board = GPIOBoardF0;
            foreach (OutputPin OutputPin in GPIOBoardF1.OutputPins)
                OutputPin.Board = GPIOBoardF1;
            foreach (OutputPin OutputPin in GPIOBoardF2.OutputPins)
                OutputPin.Board = GPIOBoardF2;
        }

        private void Foo()
        {
            uint DesiredResolution = 1000;
            bool SetResolution = true;
            uint CurrentResolution = 0;
            NtSetTimerResolution(DesiredResolution, SetResolution, ref CurrentResolution);
        }
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);

    }

    public class IN
    {
        #region DefineINPUT
        //F0----------------------------------------------------------------------------------
        public static InputPin NFC_01_OK = App.GPIOBoardF0.InputPins[0];
        public static InputPin NFC_01_NG = App.GPIOBoardF0.InputPins[1];
        public static InputPin NFC_02_OK = App.GPIOBoardF0.InputPins[2];
        public static InputPin NFC_02_NG = App.GPIOBoardF0.InputPins[3];
        public static InputPin NFC_03_OK = App.GPIOBoardF0.InputPins[4];
        public static InputPin NFC_03_NG = App.GPIOBoardF0.InputPins[5];
        public static InputPin NFC_04_OK = App.GPIOBoardF0.InputPins[6];
        public static InputPin NFC_04_NG = App.GPIOBoardF0.InputPins[7];
        public static InputPin LEAK_01_OK = App.GPIOBoardF0.InputPins[8];
        public static InputPin LEAK_01_NG = App.GPIOBoardF0.InputPins[9];
        public static InputPin LEAK_02_OK = App.GPIOBoardF0.InputPins[10];
        public static InputPin LEAK_02_NG = App.GPIOBoardF0.InputPins[11];
        public static InputPin LEAK_03_OK = App.GPIOBoardF0.InputPins[12];
        public static InputPin LEAK_03_NG = App.GPIOBoardF0.InputPins[13];
        public static InputPin LEAK_04_OK = App.GPIOBoardF0.InputPins[14];
        public static InputPin LEAK_04_NG = App.GPIOBoardF0.InputPins[15];
        public static InputPin LEAK_01_PRESS_UP_SENSOR = App.GPIOBoardF0.InputPins[16];
        public static InputPin LEAK_01_PRESS_DOWN_SENSOR = App.GPIOBoardF0.InputPins[17];
        public static InputPin LEAK_01_FOWARD_SENSOR = App.GPIOBoardF0.InputPins[18];
        public static InputPin LEAK_01_REVERSE_SENSOR = App.GPIOBoardF0.InputPins[19];
        public static InputPin LEAK_02_PRESS_UP_SENSOR = App.GPIOBoardF0.InputPins[20];
        public static InputPin LEAK_02_PRESS_DOWN_SENSOR = App.GPIOBoardF0.InputPins[21];
        public static InputPin LEAK_02_FOWARD_SENSOR = App.GPIOBoardF0.InputPins[22];
        public static InputPin LEAK_02_REVERSE_SENSOR = App.GPIOBoardF0.InputPins[23];
        public static InputPin LEAK_03_PRESS_UP_SENSOR = App.GPIOBoardF0.InputPins[24];
        public static InputPin LEAK_03_PRESS_DOWN_SENSOR = App.GPIOBoardF0.InputPins[25];
        public static InputPin LEAK_03_FOWARD_SENSOR = App.GPIOBoardF0.InputPins[26];
        public static InputPin LEAK_03_REVERSE_SENSOR = App.GPIOBoardF0.InputPins[27];
        public static InputPin LEAK_04_PRESS_UP_SENSOR = App.GPIOBoardF0.InputPins[28];
        public static InputPin LEAK_04_PRESS_DOWN_SENSOR = App.GPIOBoardF0.InputPins[29];
        public static InputPin LEAK_04_FOWARD_SENSOR = App.GPIOBoardF0.InputPins[30];
        public static InputPin LEAK_04_REVERSE_SENSOR = App.GPIOBoardF0.InputPins[31];
        //F1----------------------------------------------------------------------------------
        public static InputPin TVOC_OK_SIGNAL = App.GPIOBoardF1.InputPins[0];
        public static InputPin TVOC_NG_SIGNAL = App.GPIOBoardF1.InputPins[1];
        public static InputPin TVOC_TEST = App.GPIOBoardF1.InputPins[2];
        public static InputPin UNLOADING_CV_SENSOR_02 { get; set; } = App.GPIOBoardF1.InputPins[3];
        public static InputPin UNLOADING_CV_SENSOR_03 { get; set; } = App.GPIOBoardF1.InputPins[4];
        public static InputPin LOADING_01_LIFT_UP_SENSOR { get; set; } = App.GPIOBoardF1.InputPins[5];
        public static InputPin LOADING_01_LIFT_DOWN_SENSOR { get; set; } = App.GPIOBoardF1.InputPins[6];
        public static InputPin LOADING_02_LIFT_UP_SENSOR { get; set; } = App.GPIOBoardF1.InputPins[7];
        public static InputPin LOADING_02_LIFT_DOWN_SENSOR { get; set; } = App.GPIOBoardF1.InputPins[8];
        public static InputPin LOADING_02_TURN_SENSOR { get; set; } = App.GPIOBoardF1.InputPins[9];
        public static InputPin LOADING_02_RETURN_SENSOR { get; set; } = App.GPIOBoardF1.InputPins[10];
        public static InputPin INPUT_CV_SENSOR_02 { get; set; } = App.GPIOBoardF1.InputPins[11];
        public static InputPin TRANSFER_CV_SENSOR_BEGIN { get; set; } = App.GPIOBoardF1.InputPins[12];
        public static InputPin TRANSFER_CV_SENSOR_END { get; set; } = App.GPIOBoardF1.InputPins[13];
        public static InputPin NG_CV_SENSOR_01 { get; set; } = App.GPIOBoardF1.InputPins[14];
        public static InputPin NG_CV_SENSOR_02 { get; set; } = App.GPIOBoardF1.InputPins[15];
        public static InputPin NG_CV_SENSOR_03 { get; set; } = App.GPIOBoardF1.InputPins[16];
        public static InputPin UNLOADING_SET_SENSOR { get; set; } = App.GPIOBoardF1.InputPins[17];
        public static InputPin DOOR1_SENSOR = App.GPIOBoardF1.InputPins[18];
        public static InputPin DOOR2_SENSOR = App.GPIOBoardF1.InputPins[19];
        public static InputPin START_BUTTON_01 = App.GPIOBoardF1.InputPins[20];
        public static InputPin STOP_BUTTON_01 = App.GPIOBoardF1.InputPins[21];
        public static InputPin ESTOP_BUTTON_01 = App.GPIOBoardF1.InputPins[22];
        public static InputPin START_BUTTON_02 = App.GPIOBoardF1.InputPins[23];
        public static InputPin STOP_BUTTON_02 = App.GPIOBoardF1.InputPins[24];
        public static InputPin ESTOP_BUTTON_02 = App.GPIOBoardF1.InputPins[25];
        public static InputPin INPUT_CV_SENSOR_01 = App.GPIOBoardF1.InputPins[26];
        public static InputPin NG_CV_SENSOR_04 = App.GPIOBoardF1.InputPins[27];
        public static InputPin LOADING_01_TURN_SENSOR = App.GPIOBoardF1.InputPins[28];
        public static InputPin LOADING_01_RETURN_SENSOR = App.GPIOBoardF1.InputPins[29];
        //F2-----------------------------------------------------------------------
        public static InputPin LOADING_01_CLAMP_SENSOR { get; set; } = /*new InputPin() { PinValue = PinValue.OFF };*/ App.GPIOBoardF2.InputPins[0];
        public static InputPin LOADING_01_UNCLAMP_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[1];
        public static InputPin LOADING_02_CLAMP_SENSOR { get; set; } = /*new InputPin() { PinValue = PinValue.OFF };*/App.GPIOBoardF2.InputPins[2];
        public static InputPin LOADING_02_UNCLAMP_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[3];
        public static InputPin LOADING_03_CLAMP_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[4];
        public static InputPin LOADING_03_UNCLAMP_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[5];
        public static InputPin LOADING_03_LIFT_UP_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[6];
        public static InputPin LOADING_03_LIFT_DOWN_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[7];
        //public static InputPin LOADING_03_LIFT_02_UP_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[8];
        //public static InputPin LOADING_03_LIFT_02_DOWN_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[9];
        public static InputPin LOADING_04_CLAMP_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[10];
        public static InputPin LOADING_04_UNCLAMP_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[11];
        public static InputPin LOADING_04_LIFT_UP_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[12];
        public static InputPin LOADING_04_LIFT_DOWN_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[13];
        public static InputPin SIM_01_PACK_IN_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[14];
        public static InputPin SIM_01_PACK_OUT_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[15];
        public static InputPin SIM_01_PRESS_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[16];
        public static InputPin SIM_01_RELEASE_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[17];
        public static InputPin SIM_02_PACK_IN_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[18];
        public static InputPin SIM_02_PACK_OUT_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[19];
        public static InputPin SIM_02_PRESS_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[20];
        public static InputPin SIM_02_RELEASE_SENSOR { get; set; } = App.GPIOBoardF2.InputPins[21];

        //new InputPin() { GPIOBitmask = GPIOBitmask.GPIO14, GPIODescription = "SIM_01_PACK_IN_SENSOR", GPIOLabel = "INF2_14" },
        //        new InputPin() { GPIOBitmask = GPIOBitmask.GPIO15, GPIODescription = "SIM_01_PACK_OUT_SENSOR", GPIOLabel = "INF2_15" },
        //        new InputPin() { GPIOBitmask = GPIOBitmask.GPIO16, GPIODescription = "SIM_01_PRESS_SENSOR", GPIOLabel = "INF2_16" },
        //        new InputPin() { GPIOBitmask = GPIOBitmask.GPIO17, GPIODescription = "SIM_01_RELEASE_SENSOR", GPIOLabel = "INF2_17" },
        //        new InputPin() { GPIOBitmask = GPIOBitmask.GPIO18, GPIODescription = "SIM_02_PACK_IN_SENSOR", GPIOLabel = "INF2_18" },
        //        new InputPin() { GPIOBitmask = GPIOBitmask.GPIO19, GPIODescription = "SIM_02_PACK_OUT_SENSOR", GPIOLabel = "INF2_19" },
        //        new InputPin() { GPIOBitmask = GPIOBitmask.GPIO20, GPIODescription = "SIM_02_PRESS_SENSOR", GPIOLabel = "INF2_20" },
        //        new InputPin() { GPIOBitmask = GPIOBitmask.GPIO21, GPIODescription = "SIM_02_RELEASE_SENSOR", GPIOLabel = "INF2_21" },

        #endregion
    }
    public class OUT
    {
        #region define OUTPUT
        ////F0------------------------------------------------------------------------
        public static OutputPin PD_01_PACK = App.GPIOBoardF0.OutputPins[0];
        public static OutputPin PD_02_PACK = App.GPIOBoardF0.OutputPins[1];
        public static OutputPin PD_03_PACK = App.GPIOBoardF0.OutputPins[2];
        public static OutputPin PD_04_PACK = App.GPIOBoardF0.OutputPins[3];
        public static OutputPin PD_05_PACK = App.GPIOBoardF0.OutputPins[4];
        public static OutputPin PD_06_PACK = App.GPIOBoardF0.OutputPins[5];
        public static OutputPin PD_07_PACK = App.GPIOBoardF0.OutputPins[6];
        public static OutputPin PD_08_PACK = App.GPIOBoardF0.OutputPins[7];
        public static OutputPin NFC_01_PACK = App.GPIOBoardF0.OutputPins[8];
        public static OutputPin NFC_02_PACK = App.GPIOBoardF0.OutputPins[9];
        public static OutputPin NFC_03_PACK = App.GPIOBoardF0.OutputPins[10];
        public static OutputPin NFC_04_PACK = App.GPIOBoardF0.OutputPins[11];
        public static OutputPin LEAK_01_PACK = App.GPIOBoardF0.OutputPins[12];
        public static OutputPin LEAK_02_PACK = App.GPIOBoardF0.OutputPins[13];
        public static OutputPin LEAK_03_PACK = App.GPIOBoardF0.OutputPins[14];
        public static OutputPin LEAK_04_PACK = App.GPIOBoardF0.OutputPins[15];
        public static OutputPin LEAK_01_PRESS_SOL = App.GPIOBoardF0.OutputPins[16];
        public static OutputPin LEAK_01_TRANS_SOL = App.GPIOBoardF0.OutputPins[17];
        public static OutputPin LEAK_02_PRESS_SOL = App.GPIOBoardF0.OutputPins[18];
        public static OutputPin LEAK_02_TRANS_SOL = App.GPIOBoardF0.OutputPins[19];
        public static OutputPin LEAK_03_PRESS_SOL = App.GPIOBoardF0.OutputPins[20];
        public static OutputPin LEAK_03_TRANS_SOL = App.GPIOBoardF0.OutputPins[21];
        public static OutputPin LEAK_04_PRESS_SOL = App.GPIOBoardF0.OutputPins[22];
        public static OutputPin LEAK_04_TRANS_SOL = App.GPIOBoardF0.OutputPins[23];
        //F1-------------------------------------------------------------------------------
        public static OutputPin TVOC_START = App.GPIOBoardF1.OutputPins[0];
        public static OutputPin RED_LIGHT = App.GPIOBoardF1.OutputPins[1];
        public static OutputPin ORANGE_LIGHT = App.GPIOBoardF1.OutputPins[2];
        public static OutputPin GREEN_LIGHT = App.GPIOBoardF1.OutputPins[3];
        public static OutputPin BUZZER = App.GPIOBoardF1.OutputPins[4];
        public static OutputPin LOADING_CV_01_RELAY = App.GPIOBoardF1.OutputPins[5];
        public static OutputPin TRANSFER_CV_RELAY = App.GPIOBoardF1.OutputPins[6];
        public static OutputPin NG_CV_RELAY = App.GPIOBoardF1.OutputPins[7];
        public static OutputPin OUTPUT_CV_RELAY = App.GPIOBoardF1.OutputPins[8];
        public static OutputPin LOADING_01_LIFT_SOL = App.GPIOBoardF1.OutputPins[9];
        public static OutputPin LOADING_01_CLAMP_SOL = App.GPIOBoardF1.OutputPins[10];
        public static OutputPin LOADING_01_UNCLAMP_SOL = App.GPIOBoardF1.OutputPins[11];
        public static OutputPin LOADING_02_LIFT_SOL = App.GPIOBoardF1.OutputPins[12];
        public static OutputPin LOADING_02_CLAMP_SOL = App.GPIOBoardF1.OutputPins[13];
        public static OutputPin LOADING_02_UNCLAMP_SOL = App.GPIOBoardF1.OutputPins[14];
        public static OutputPin LOADING_02_TURN_SOL = App.GPIOBoardF1.OutputPins[15];
        public static OutputPin PD_01_POWER_PIN = App.GPIOBoardF1.OutputPins[16];
        public static OutputPin PD_02_POWER_PIN = App.GPIOBoardF1.OutputPins[17];
        public static OutputPin PD_03_POWER_PIN = App.GPIOBoardF1.OutputPins[18];
        public static OutputPin PD_04_POWER_PIN = App.GPIOBoardF1.OutputPins[19];
        public static OutputPin PD_05_POWER_PIN = App.GPIOBoardF1.OutputPins[20];
        public static OutputPin PD_06_POWER_PIN = App.GPIOBoardF1.OutputPins[21];
        public static OutputPin PD_07_POWER_PIN = App.GPIOBoardF1.OutputPins[22];
        public static OutputPin PD_08_POWER_PIN = App.GPIOBoardF1.OutputPins[23];
        //F2---------------------------------------------------------------------------------
        public static OutputPin LOADING_03_LIFT_SOL = App.GPIOBoardF2.OutputPins[0];
        //public static OutputPin LOADING_03_LIFT_SOL_02 = App.GPIOBoardF2.OutputPins[1];
        public static OutputPin LOADING_03_CLAMP_SOL = App.GPIOBoardF2.OutputPins[2];
        public static OutputPin LOADING_03_UNCLAMP_SOL = App.GPIOBoardF2.OutputPins[3];
        public static OutputPin LOADING_04_LIFT_SOL = App.GPIOBoardF2.OutputPins[4];
        //public static OutputPin LOADING_04_LIFT_SOL_02 = App.GPIOBoardF2.OutputPins[5];
        public static OutputPin LOADING_04_CLAMP_SOL = App.GPIOBoardF2.OutputPins[6];
        public static OutputPin LOADING_04_UNCLAMP_SOL = App.GPIOBoardF2.OutputPins[7];
        public static OutputPin START_BUTTON_01_LED = App.GPIOBoardF2.OutputPins[8];
        public static OutputPin STOP_BUTTON_01_LED = App.GPIOBoardF2.OutputPins[9];
        public static OutputPin START_BUTTON_02_LED = App.GPIOBoardF2.OutputPins[10];
        public static OutputPin STOP_BUTTON_02_LED = App.GPIOBoardF2.OutputPins[11];
        public static OutputPin BUFFER_PACK = App.GPIOBoardF2.OutputPins[12];
        public static OutputPin LOADING_CV_02_RELAY = App.GPIOBoardF2.OutputPins[13];
        public static OutputPin LOADING_01_TURN_SOL = App.GPIOBoardF2.OutputPins[14];
        public static OutputPin SIM_01_PACK_SOL = App.GPIOBoardF2.OutputPins[15];
        public static OutputPin SIM_01_PRESS_SOL = App.GPIOBoardF2.OutputPins[16];
        public static OutputPin SIM_02_PACK_SOL = App.GPIOBoardF2.OutputPins[17];
        public static OutputPin SIM_02_PRESS_SOL = App.GPIOBoardF2.OutputPins[18];
        #endregion
    }
}
