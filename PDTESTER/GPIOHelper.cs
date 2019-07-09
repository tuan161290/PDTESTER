using PDTESTER;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//0961041893

namespace GPIOCommunication
{
    public class GPIOHelper
    {
        public class GPIOBoard : INotifyPropertyChanged
        {
            private const PinValue ON = PinValue.ON;
            private const PinValue OFF = PinValue.OFF;
            SemaphoreSlim GPIOIOLock = new SemaphoreSlim(1, 1);
            private uint _OutputRegister;
            public uint OutputRegister
            {
                get { return _OutputRegister; }
                private set
                {
                    if (_OutputRegister != value)
                    {
                        _OutputRegister = value;
                        foreach (GPIOPin GP in OutputPins)
                        {
                            GP.PinValue = (_OutputRegister & GP.GPIOBitmask) > 0 ? PinValue.ON : PinValue.OFF;
                        }
                    }
                }
            }
            private uint _InputRegister;
            public uint InputRegister
            {
                get { return _InputRegister; }
                private set
                {
                    if (_InputRegister != value)
                    {
                        _InputRegister = value;
                        foreach (GPIOPin GP in InputPins)
                        {
                            GP.PinValue = (_InputRegister & GP.GPIOBitmask) > 0 ? ON : OFF;
                        }
                    }
                }
            }
            public int _RespTime;
            public int RespTime
            {
                get { return _RespTime; }
                set
                {
                    _RespTime = value;
                    NotifyPropertyChanged("RespTime");
                }
            }
            public byte GPIOStation { get; set; }
            public ObservableCollection<InputPin> InputPins { get; set; }
            public ObservableCollection<OutputPin> OutputPins { get; set; }

            public GPIOBoard(byte Station)
            {
                GPIOStation = Station;
                //InputPins = new ObservableCollection<InputPin>()
                //{
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO00, GPIOLabel = Station.ToString("X") + "_" + 00, GPIODesciption = "IN0"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO01, GPIOLabel = Station.ToString("X") + "_" + 01, GPIODesciption = "IN1"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO02, GPIOLabel = Station.ToString("X") + "_" + 02, GPIODesciption = "IN2"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO03, GPIOLabel = Station.ToString("X") + "_" + 03, GPIODesciption = "IN3"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO04, GPIOLabel = Station.ToString("X") + "_" + 04, GPIODesciption = "IN4"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO05, GPIOLabel = Station.ToString("X") + "_" + 05, GPIODesciption = "IN5"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO06, GPIOLabel = Station.ToString("X") + "_" + 06, GPIODesciption = "IN6"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO07, GPIOLabel = Station.ToString("X") + "_" + 07, GPIODesciption = "IN7"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO08, GPIOLabel = Station.ToString("X") + "_" + 08, GPIODesciption = "IN8"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO09, GPIOLabel = Station.ToString("X") + "_" + 09, GPIODesciption = "IN9"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO10, GPIOLabel = Station.ToString("X") + "_" + 10, GPIODesciption = "IN10"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO11, GPIOLabel = Station.ToString("X") + "_" + 11, GPIODesciption = "IN11"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO12, GPIOLabel = Station.ToString("X") + "_" + 12, GPIODesciption = "IN12"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO13, GPIOLabel = Station.ToString("X") + "_" + 13, GPIODesciption = "IN13"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO14, GPIOLabel = Station.ToString("X") + "_" + 14, GPIODesciption = "IN14"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO15, GPIOLabel = Station.ToString("X") + "_" + 15, GPIODesciption = "IN15"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO16, GPIOLabel = Station.ToString("X") + "_" + 16, GPIODesciption = "IN16"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO17, GPIOLabel = Station.ToString("X") + "_" + 17, GPIODesciption = "IN17"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO18, GPIOLabel = Station.ToString("X") + "_" + 18, GPIODesciption = "IN18"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO19, GPIOLabel = Station.ToString("X") + "_" + 19, GPIODesciption = "IN19"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO20, GPIOLabel = Station.ToString("X") + "_" + 20, GPIODesciption = "IN20"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO21, GPIOLabel = Station.ToString("X") + "_" + 21, GPIODesciption = "IN21"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO22, GPIOLabel = Station.ToString("X") + "_" + 22, GPIODesciption = "IN22"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO23, GPIOLabel = Station.ToString("X") + "_" + 23, GPIODesciption = "IN23"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO24, GPIOLabel = Station.ToString("X") + "_" + 24, GPIODesciption = "IN24"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO25, GPIOLabel = Station.ToString("X") + "_" + 25, GPIODesciption = "IN25"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO26, GPIOLabel = Station.ToString("X") + "_" + 26, GPIODesciption = "IN26"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO27, GPIOLabel = Station.ToString("X") + "_" + 27, GPIODesciption = "IN27"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO28, GPIOLabel = Station.ToString("X") + "_" + 28, GPIODesciption = "IN28"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO29, GPIOLabel = Station.ToString("X") + "_" + 29, GPIODesciption = "IN29"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO30, GPIOLabel = Station.ToString("X") + "_" + 30, GPIODesciption = "IN30"},
                //    new InputPin() { GPIOBitmask = GPIOBitmask.GPIO31, GPIOLabel = Station.ToString("X") + "_" + 31, GPIODesciption = "IN31"},
                //};
                //OutputPins = new ObservableCollection<OutputPin>()
                //{
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO00, GPIOLabel = Station.ToString("X") + "_" + 00,GPIODesciption = "OUT0", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO01, GPIOLabel = Station.ToString("X") + "_" + 01,GPIODesciption = "OUT1", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO02, GPIOLabel = Station.ToString("X") + "_" + 02,GPIODesciption = "OUT2", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO03, GPIOLabel = Station.ToString("X") + "_" + 03,GPIODesciption = "OUT3", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO04, GPIOLabel = Station.ToString("X") + "_" + 04,GPIODesciption = "OUT4", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO05, GPIOLabel = Station.ToString("X") + "_" + 05,GPIODesciption = "OUT5", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO06, GPIOLabel = Station.ToString("X") + "_" + 06,GPIODesciption = "OUT6", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO07, GPIOLabel = Station.ToString("X") + "_" + 07,GPIODesciption = "OUT7", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO08, GPIOLabel = Station.ToString("X") + "_" + 08,GPIODesciption = "OUT8", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO09, GPIOLabel = Station.ToString("X") + "_" + 09,GPIODesciption = "OUT9", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO10, GPIOLabel = Station.ToString("X") + "_" + 10,GPIODesciption = "OUT10", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO11, GPIOLabel = Station.ToString("X") + "_" + 11,GPIODesciption = "OUT11", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO12, GPIOLabel = Station.ToString("X") + "_" + 12,GPIODesciption = "OUT12", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO13, GPIOLabel = Station.ToString("X") + "_" + 13,GPIODesciption = "OUT13", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO14, GPIOLabel = Station.ToString("X") + "_" + 14,GPIODesciption = "OUT14", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO15, GPIOLabel = Station.ToString("X") + "_" + 15,GPIODesciption = "OUT15", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO16, GPIOLabel = Station.ToString("X") + "_" + 16,GPIODesciption = "OUT16", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO17, GPIOLabel = Station.ToString("X") + "_" + 17,GPIODesciption = "OUT17", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO18, GPIOLabel = Station.ToString("X") + "_" + 18,GPIODesciption = "OUT18", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO19, GPIOLabel = Station.ToString("X") + "_" + 19,GPIODesciption = "OUT19", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO20, GPIOLabel = Station.ToString("X") + "_" + 20,GPIODesciption = "OUT20", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO21, GPIOLabel = Station.ToString("X") + "_" + 21,GPIODesciption = "OUT21", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO22, GPIOLabel = Station.ToString("X") + "_" + 22,GPIODesciption = "OUT22", Board = this},
                //    new OutputPin() { GPIOBitmask = GPIOBitmask.GPIO23, GPIOLabel = Station.ToString("X") + "_" + 23,GPIODesciption = "OUT23", Board = this}
                //};
            }
            public async Task RST(OutputPin Pin)
            {
                if (App.GPIOCOM != null)
                {
                    if ((Pin.GPIOBitmask & OutputRegister) > 0)
                    {
                        await GPIOIOLock.WaitAsync();
                        //sw.Reset();
                        //sw.Start();
                        if ((OutputRegister & Pin.GPIOBitmask) > 0)
                        {
                            OutputRegister &= ~(Pin.GPIOBitmask);
                            await App.GPIOCOM.GPIOWriteAsync(GPIOStation, Flag.SetIOOutput, BitConverter.GetBytes(OutputRegister));
                        }
                        //RespTime = (int)sw.ElapsedMilliseconds;
                        GPIOIOLock.Release();
                    }
                }
            }
            public async Task SET(OutputPin Pin)
            {
                if (App.GPIOCOM != null)
                {
                    if ((Pin.GPIOBitmask & OutputRegister) == 0)
                    {
                        await GPIOIOLock.WaitAsync();
                        //sw.Reset();
                        //sw.Start();
                        OutputRegister |= Pin.GPIOBitmask;
                        await App.GPIOCOM.GPIOWriteAsync(GPIOStation, Flag.SetIOOutput, BitConverter.GetBytes(OutputRegister));
                        GPIOIOLock.Release();
                    }
                }
            }

            Stopwatch sw = new Stopwatch();
            public async Task GetGPIOsState()
            {
                await GPIOIOLock.WaitAsync();
                //sw = new Stopwatch();
                //sw.Start();
                if (App.GPIOCOM != null)
                {
                    var resp = await App.GPIOCOM.GPIOWriteAsync(GPIOStation, Flag.GetIOInPut, null);
                    if (resp != null)
                    {
                        InputRegister = BitConverter.ToUInt32(resp, 3);
                        OutputRegister = BitConverter.ToUInt32(resp, 7);
                    }
                }
                //RespTime = (int)sw.ElapsedMilliseconds;
                GPIOIOLock.Release();
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            //private void NotifyPinValueChanged(GPIOPin Pin, Edge e)
            //{
            //    // Make sure someone is listening to event
            //    if (OnPinValueChanged != null)
            //    {
            //        PinValueChangedEventArgs args = new PinValueChangedEventArgs(Pin, e);
            //        OnPinValueChanged(this, args);
            //    }
            //}
            //public delegate void PinValueChanged(object sender, PinValueChangedEventArgs e);
            //public event PinValueChanged OnPinValueChanged;
            //public class PinValueChangedEventArgs : EventArgs
            //{
            //    public Edge Edge { get; private set; }
            //    public GPIOPin Pin { get; private set; }
            //    public PinValueChangedEventArgs(GPIOPin Pin, Edge e)
            //    {
            //        this.Pin = Pin;
            //        Edge = e;
            //    }
            //}
        }

        public class InputPin : GPIOPin
        {

        }

        public class OutputPin : GPIOPin
        {
            public GPIOBoard Board { get; set; }
            public async Task SET()
            {
                if (PinValue == PinValue.OFF)
                    await Board.SET(this);
            }
            public async Task RST()
            {
                if (PinValue == PinValue.ON)
                    await Board.RST(this);
            }
        }

        public class GPIOPin : INotifyPropertyChanged
        {
            public string GPIODescription { get; set; }
            public string GPIOLabel { get; set; }
            public uint GPIOBitmask { get; set; }

            private PinValue _PinValue;
            public PinValue PinValue
            {
                get
                {
                    return _PinValue;
                }
                set
                {
                    if (_PinValue != value)
                    {
                        _PinValue = value;
                        NotifyPropertyChanged("PinValue");
                        if (value == PinValue.ON)
                            NotifyPinValueChanged(Edge.Rise);
                        else
                            NotifyPinValueChanged(Edge.Fall);
                    }
                }
            }
            //public byte GPIOStation { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;
            PropertyChangedEventArgs PropertyChangedEventArg = new PropertyChangedEventArgs("PinValue");
            private void NotifyPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, PropertyChangedEventArg);
            }

            private void NotifyPinValueChanged(Edge e)
            {
                // Make sure someone is listening to event
                onPinValueChanged?.Invoke(this, new PinValueChangedEventArgs(e));
            }
            private object _eventLock = new object();
            public delegate void PinValueChanged(object sender, PinValueChangedEventArgs e);
            public event PinValueChanged OnPinValueChanged
            {
                add
                {
                    lock (_eventLock)
                    {
                        onPinValueChanged -= value;
                        onPinValueChanged += value;
                    }
                }
                remove
                {
                    lock (_eventLock)
                        onPinValueChanged -= value;
                }
            }
            private event PinValueChanged onPinValueChanged;
            public class PinValueChangedEventArgs : EventArgs
            {
                public Edge Edge { get; private set; }
                public PinValueChangedEventArgs(Edge e)
                {
                    Edge = e;
                }
            }
        }

        #region GPIO Communication method
        public class GPIOSerial
        {
            public SerialHelper Serial { get; set; }
            private int MAXRETRY = 3;
            public GPIOSerial(string ComPort)
            {
                Serial = new SerialHelper(ComPort, 6, "GPIOHelper");
            }

            private bool CRC16ErrorCheck(byte[] Data)
            {
                if (Data.Count() < 2)
                    return false;
                try
                {
                    int Count = Data.Length;
                    byte HighCRC = Data[Count - 1];
                    byte LowCRC = Data[Count - 2];
                    byte[] data = new byte[Count - 2];
                    Array.Copy(Data, 0, data, 0, Count - 2);
                    var CalculatedCRC = SerialHelper.CalcCRC(data);
                    return (HighCRC == CalculatedCRC[1] && LowCRC == CalculatedCRC[0]);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            private bool ParseResponde(byte SlaveID, byte FrameType, byte[] Data)
            {
                try
                {
                    int Count = Data.Count();
                    if (Count > 2)
                    {
                        if (CRC16ErrorCheck(Data))
                        {
                            return (SlaveID == Data[0] && FrameType == Data[1] && Data[2] == 0x00);
                        }
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            private SemaphoreSlim Writelock = new SemaphoreSlim(1, 1);
            private static readonly List<byte> Header = new List<byte>() { 0xAA, 0xCC };
            private static readonly List<byte> Tail = new List<byte>() { 0xAA, 0xEE };
            /// <summary>
            /// Write data to GPIOBoard
            /// </summary>
            /// <param name="SlaveID">GPIOBoard Station ID</param>
            /// <param name="FrameType">Frame type</param>
            /// <param name="Data">Transmitted data</param>
            /// <returns></returns>
            public async Task<byte[]> GPIOWriteAsync(byte SlaveID, byte FrameType, byte[] Data)
            {
                try
                {
                    await Writelock.WaitAsync();
                    if (Serial == null)
                        return null;
                    int Retry = MAXRETRY;
                    List<byte> Frame = new List<byte> { SlaveID, FrameType };
                    if (Data != null)
                        Frame.AddRange(Data);
                    //var CRC1 = CRC16Generator(Frame.ToArray());
                    //var CRC2 = ;
                    Frame.AddRange(SerialHelper.CalcCRC(Frame.ToArray()));
                    Frame.AddRange(Tail);
                    Frame.InsertRange(0, Header);
                    while (Retry > 0)
                    {
                        await Serial.WriteAsync(Frame.ToArray());
                        byte[] ReceivedBytes;
                        ReceivedBytes = await Serial.ReadAsync();
                        if (ReceivedBytes != null)
                        {
                            if (ParseResponde(SlaveID, FrameType, ReceivedBytes))
                                return ReceivedBytes;
                        }
                        //await Serial.WriteAsync(Frame.ToArray());
                        await Task.Delay(20);
                        Serial.DiscardInBuffer();
                        Retry--;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                    //throw;
                    return null;
                }
                finally
                {
                    Writelock.Release();
                }
            }

            public void CloseDevice()
            {
                if (Serial != null)
                    Serial.CloseDevice();
            }
        }
        #endregion
        #region Define GPIOBitmask
        public class GPIOBitmask
        {

            public const uint GPIO00 = 1;
            public const uint GPIO01 = 1 << 1;
            public const uint GPIO02 = 1 << 2;
            public const uint GPIO03 = 1 << 3;
            public const uint GPIO04 = 1 << 4;
            public const uint GPIO05 = 1 << 5;
            public const uint GPIO06 = 1 << 6;
            public const uint GPIO07 = 1 << 7;
            public const uint GPIO08 = 1 << 8;
            public const uint GPIO09 = 1 << 9;
            public const uint GPIO10 = 1 << 10;
            public const uint GPIO11 = 1 << 11;
            public const uint GPIO12 = 1 << 12;
            public const uint GPIO13 = 1 << 13;
            public const uint GPIO14 = 1 << 14;
            public const uint GPIO15 = 1 << 15;
            public const uint GPIO16 = 1 << 16;
            public const uint GPIO17 = 1 << 17;
            public const uint GPIO18 = 1 << 18;
            public const uint GPIO19 = 1 << 19;
            public const uint GPIO20 = 1 << 20;
            public const uint GPIO21 = 1 << 21;
            public const uint GPIO22 = 1 << 22;
            public const uint GPIO23 = 1 << 23;
            public const uint GPIO24 = 1 << 24;
            public const uint GPIO25 = 1 << 25;
            public const uint GPIO26 = 1 << 26;
            public const uint GPIO27 = 1 << 27;
            public const uint GPIO28 = 1 << 28;
            public const uint GPIO29 = 1 << 29;
            public const uint GPIO30 = 1 << 30;
            public const uint GPIO31 = (uint)1 << 31;
        }
        public enum PinValue { OFF, ON }
        public enum Edge { Rise, Fall }
        public class Flag
        {
            public const byte SetIOOutput = 0x20;
            public const byte GetIOInPut = 0x22;
        }
        #endregion
    }
}
