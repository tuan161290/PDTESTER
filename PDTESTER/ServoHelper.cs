using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GPIOCommunication.GPIOHelper;

namespace PDTESTER
{
    public class ServoHelper : INotifyPropertyChanged
    {
        public SerialHelper Serial { get; set; } = null;
        private const int MAXRETRY = 3;
        public ServoHelper(string ComPort)
        {
            Serial = new SerialHelper(ComPort, 115200, 5, "EziServoHelper");
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

        private bool CRC16ErrorCheck(byte[] Data)
        {
            //if (Data.Count() < 2)
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
                return false;
            }
        }

        SemaphoreSlim Writelock = new SemaphoreSlim(1, 1);
        private static readonly List<byte> Header = new List<byte>() { 0xAA, 0xCC };
        private static readonly List<byte> Tail = new List<byte>() { 0xAA, 0xEE };
        static byte[] ReceivedBytes;
        public async Task<byte[]> StepGetdata(byte SlaveID, byte FrameType, IEnumerable<byte> Data)
        {
            try
            {
                await Writelock.WaitAsync();
                int Retry = MAXRETRY;
                List<byte> Frame = new List<byte> { SlaveID, FrameType };
                if (Data != null)
                    Frame.AddRange(Data);
                Frame.AddRange(SerialHelper.CalcCRC(Frame.ToArray()));
                string str_SentData = "";
                Frame.ForEach(x => str_SentData += x.ToString("X2") + " ");
                if (str_SentData.Contains("AA"))
                {
                    str_SentData = str_SentData.TrimEnd(' ');
                    str_SentData = str_SentData.Replace("AA", "AA AA");
                    Frame = new List<byte>();
                    str_SentData.Split(' ').ToList().ForEach(x =>
                    {
                        Frame.Add(Convert.ToByte(x, 16));
                    });
                }
                Frame.AddRange(Tail);
                Frame.InsertRange(0, Header);
                while (Retry > 0)
                {
                    await Serial.WriteAsync(Frame.ToArray());
                    ReceivedBytes = await Serial.ReadAsync();
                    if (ReceivedBytes != null)
                    {
                        string str_Data = "";
                        ReceivedBytes.ToList().ForEach(x => str_Data += x.ToString("X2") + " ");
                        if (str_Data.Contains("AA AA"))
                        {
                            var List_RcvData = new List<byte>();
                            str_Data.TrimEnd(' ');
                            str_Data = str_Data.Replace("AA AA", "AA");
                            str_Data.Split(' ').ToList().ForEach(x =>
                            {
                                if (x != "") List_RcvData.Add(Convert.ToByte(x, 16));
                            });
                            if (ParseResponde(SlaveID, FrameType, List_RcvData))
                            {
                                return List_RcvData.ToArray();
                            }
                        }
                        else
                        {
                            if (ParseResponde(SlaveID, FrameType, ReceivedBytes.ToList()))
                            {
                                return ReceivedBytes;
                            }
                        }


                    }
                    Retry--;
                    await Task.Delay(20);
                    Serial.DiscardInBuffer();
                }
                return null;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                throw;
            }
            finally
            {
                Writelock.Release();
            }
        }

        private bool ParseResponde(byte SlaveID, byte FrameType, List<byte> Data)
        {
            try
            {
                int Count = Data.Count;
                if (Count >= 4)
                {

                    if (CRC16ErrorCheck(Data.ToArray()))
                    {
                        return (SlaveID == Data[0] && FrameType == Data[1] && Data[2] == BitMask.FrameOK);
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void CloseDevice()
        {
            if (Serial != null)
                Serial.CloseDevice();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }


    public class AxisStatus : INotifyPropertyChanged
    {
        public byte SlaveID { get; set; }
        private int _CurrentPos;
        public int CurrentPos
        {
            get { return _CurrentPos; }
            set
            {
                if (_CurrentPos != value)
                {
                    _CurrentPos = value;
                    NotifyPropertyChanged("CurrentPos");
                }
            }
        }
        private int _Status;
        public int Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    ORG = (value & BitMask.ORGSS) != 0 ? PinValue.ON : PinValue.OFF;
                    SON = (value & BitMask.SERVOON) != 0 ? PinValue.ON : PinValue.OFF;
                    ALM = (value & BitMask.ERRORALL) != 0 ? PinValue.ON : PinValue.OFF;
                    Motioning = (value & BitMask.Motioning) != 0 ? PinValue.ON : PinValue.OFF;
                    ORG_OK = (value & BitMask.ORG_RETURN_OK) != 0 ? PinValue.ON : PinValue.OFF;
                }
            }
        }
        private int _RespondTime;
        public int RespondTime
        {
            get { return _RespondTime; }
            set
            {
                if (_RespondTime != value)
                {
                    _RespondTime = value;
                    NotifyPropertyChanged("RespondTime");
                }

            }
        }
        private PinValue _ORG;
        public PinValue ORG
        {
            get
            {
                return _ORG;
            }
            set
            {
                if (_ORG != value)
                {
                    _ORG = value;
                    NotifyPropertyChanged("ORG");
                }
            }
        }
        private PinValue _ALM;
        public PinValue ALM
        {
            get
            {
                return _ALM;
            }
            set
            {
                if (_ALM != value)
                {
                    _ALM = value;
                    NotifyPropertyChanged("ALM");
                    if (_ALM == PinValue.ON)
                    {
                        AlarmWindow.ALMWindowDispatcher.Invoke(() =>
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy")}>: AXIS #{SlaveID} ERROR, CHECK SERVO DRIVER");
                        });
                    }
                }
            }
        }
        private PinValue _SON;
        public PinValue SON
        {
            get
            {
                return _SON;
            }
            set
            {
                if (_SON != value)
                {
                    _SON = value;
                    NotifyPropertyChanged("SON");
                }
            }
        }
        private PinValue _Motioning;
        public PinValue Motioning
        {
            get
            {
                return _Motioning;
            }
            set
            {
                if (_Motioning != value)
                {
                    _Motioning = value;
                    NotifyPropertyChanged("Motioning");
                }
            }
        }
        private PinValue _ORG_OK;
        public PinValue ORG_OK
        {
            get
            {
                return _ORG_OK;
            }
            set
            {
                if (_ORG_OK != value)
                {
                    _ORG_OK = value;
                    NotifyPropertyChanged("ORG_OK");
                }
            }
        }
        public PinValue AxisOrgOK { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Axis
    {
        public static AxisStatus _01 = new AxisStatus() { SlaveID = 0x01 };
        public static AxisStatus _02 = new AxisStatus() { SlaveID = 0x02 };
        public static AxisStatus _03 = new AxisStatus() { SlaveID = 0x03 };
        public static AxisStatus _04 = new AxisStatus() { SlaveID = 0x04 };
    }
}
