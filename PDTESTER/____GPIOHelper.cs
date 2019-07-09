using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PDTESTER
{

    public class GPIOBoard : INotifyPropertyChanged
    {
        private ulong _GPIORegister;
        public ulong GPIORegister
        {
            get { return _GPIORegister; }
            set
            {
                if (_GPIORegister != value)
                {
                    _GPIORegister = value;
                    foreach (GPIOPin GP in GPIOPins)
                    {

                        GP.PinValue = (_GPIORegister & GP.GPIOBitmask) > 0 ? PinValue.ON : PinValue.OFF;
                        if (GP.PinValue != GP.PrePinValue)
                        {
                            if (GP.PinValue == PinValue.ON)
                            {
                                NotifyPinValueChanged(GP, Edge.Rise);
                            }
                            else
                            {
                                NotifyPinValueChanged(GP, Edge.Fall);
                            }
                        }
                        GP.PrePinValue = GP.PinValue;
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
        public ObservableCollection<GPIOPin> GPIOPins { get; set; } = new ObservableCollection<GPIOPin>();

        SemaphoreSlim GPIOIOLock = new SemaphoreSlim(1, 1);
        /// <summary>
        /// Reset a GPIOPin
        /// </summary>
        /// <param name="Pin">Pin to be reset</param>
        /// <returns></returns>
        public async Task SetOff(GPIOPin Pin)
        {

            if (App.GPIOCOM != null)
            {
                if ((Pin.GPIOBitmask & GPIORegister) > 0)
                {
                    await GPIOIOLock.WaitAsync();
                    sw.Reset();
                    sw.Start();
                    var outputRegister = await App.GPIOCOM.GPIOWriteAsync(GPIOStation, Flag.GetIOInPut, null);
                    if (outputRegister != null)
                    {
                        GPIORegister = BitConverter.ToUInt64(outputRegister, 3);
                        if ((GPIORegister & Pin.GPIOBitmask) > 0)
                        {
                            GPIORegister &= ~(Pin.GPIOBitmask);
                            await App.GPIOCOM.GPIOWriteAsync(GPIOStation, Flag.SetIOOutput, BitConverter.GetBytes(GPIORegister));
                        }
                    }
                    RespTime = (int)sw.ElapsedMilliseconds;
                    GPIOIOLock.Release();
                }
            }
        }

        /// <summary>
        /// Set On a GPIOPin
        /// </summary>
        /// <param name="Pin">GPIOPin</param>
        /// <returns></returns>
        public async Task SetOn(GPIOPin Pin)
        {

            if (App.GPIOCOM != null)
            {
                if ((Pin.GPIOBitmask & GPIORegister) == 0)
                {
                    await GPIOIOLock.WaitAsync();
                    sw.Reset();
                    sw.Start();
                    var outputRegister = await App.GPIOCOM.GPIOWriteAsync(GPIOStation, Flag.GetIOInPut, null);
                    if (outputRegister != null)
                    {
                        GPIORegister = BitConverter.ToUInt64(outputRegister, 3);
                        if ((GPIORegister & Pin.GPIOBitmask) == 0)
                        {
                            GPIORegister |= Pin.GPIOBitmask;
                            await App.GPIOCOM.GPIOWriteAsync(GPIOStation, Flag.SetIOOutput, BitConverter.GetBytes(GPIORegister));
                        }
                    }
                    RespTime = (int)sw.ElapsedMilliseconds;
                    GPIOIOLock.Release();
                }
            }

        }

        Stopwatch sw = new Stopwatch();

        public async Task GetGPIOsState()
        {
            await GPIOIOLock.WaitAsync();
            sw.Reset();
            sw.Start();
            if (App.GPIOCOM != null)
            {
                var gpioRegister = await App.GPIOCOM.GPIOWriteAsync(GPIOStation, Flag.GetIOInPut, null);
                if (gpioRegister != null)
                {
                    GPIORegister = BitConverter.ToUInt64(gpioRegister, 3);
                }
            }
            RespTime = (int)sw.ElapsedMilliseconds;
            GPIOIOLock.Release();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void NotifyPinValueChanged(GPIOPin Pin, Edge e)
        {
            // Make sure someone is listening to event
            if (OnPinValueChanged != null)
            {
                PinValueChangedEventArgs args = new PinValueChangedEventArgs(Pin, e);
                OnPinValueChanged(this, args);
            }
        }
        public delegate void PinStateChanged(object sender, PinValueChangedEventArgs e);
        public event PinStateChanged OnPinValueChanged;
        public class PinValueChangedEventArgs : EventArgs
        {
            public Edge Edge { get; private set; }
            public GPIOPin Pin { get; private set; }
            public PinValueChangedEventArgs(GPIOPin Pin, Edge e)
            {
                this.Pin = Pin;
                Edge = e;
            }
        }
    }

    public class GPIOPin : INotifyPropertyChanged
    {
        public string GPIODesciption { get; set; }
        public string GPIOLabel { get; set; }
        public ulong GPIOBitmask { get; set; }
        public PinValue PrePinValue { get; set; }
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
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    #region GPIO Communication method
    public class GPIOHelper
    {
        public SerialHelper Serial { get; set; }
        public byte[] ReceivedBytes { get; private set; }
        private int MAXRETRY = 3;

        public GPIOHelper(string ComPort)
        {
            Serial = new SerialHelper(ComPort);
        }

        private byte[] CRC16Generator(byte[] Bytes)
        {
            byte[] CRC = new byte[2];
            ushort CheckSum = 0xFFFF;
            ushort j;
            byte lowCRC;
            byte highCRC;
            for (j = 0; j < Bytes.Count(); j++)
            {
                CheckSum = (ushort)(CheckSum ^ Bytes[j]);
                for (short i = 8; i > 0; i--)
                    if ((CheckSum & 0x0001) == 1)
                        CheckSum = (ushort)((CheckSum >> 1) ^ 0xA001);
                    else
                        CheckSum >>= 1;
            }
            highCRC = (byte)(CheckSum >> 8);
            CheckSum <<= 8;
            lowCRC = (byte)(CheckSum >> 8);
            CRC[0] = lowCRC;
            CRC[1] = highCRC;
            return CRC;
        }
        private bool CRC16ErrorCheck(byte[] Data)
        {
            if (Data.Count() < 2)
                return false;
            try
            {
                int Count = Data.Count();
                byte HighCRC = Data[Count - 1];
                byte LowCRC = Data[Count - 2];
                byte[] data = new byte[Count - 2];
                Array.Copy(Data, 0, data, 0, Count - 2);
                byte[] CalculatedCRC = CRC16Generator(data);
                if (HighCRC == CalculatedCRC[1] && LowCRC == CalculatedCRC[0])
                    return true;
                else return false;
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
                        if (SlaveID == Data[0] && FrameType == Data[1] && Data[2] == BitMask.FrameOK)
                            return true;
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
                if (Serial == null) return null;
                int Retry = MAXRETRY;
                List<byte> Frame = new List<byte> { SlaveID, FrameType };
                if (Data != null)
                    Frame.AddRange(Data);
                Frame.AddRange(CRC16Generator(Frame.ToArray()));
                Frame.AddRange(Tail);
                Frame.InsertRange(0, Header);
                bool ErrorCheck = false;
                while (Retry > 0)
                {
                    await Serial.WriteAsync(Frame.ToArray());
                    byte[] ReceivedBytes;
                    ReceivedBytes = await Serial.ReadAsync();
                    if (ReceivedBytes != null)
                    {
                        ErrorCheck = ParseResponde(SlaveID, FrameType, ReceivedBytes);
                        if (ErrorCheck)
                            return ReceivedBytes;
                    }
                    //await Serial.WriteAsync(Frame.ToArray());
                    await Task.Delay(10);
                    Retry--;
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

        public const ulong GPIO00 = 1UL;
        public const ulong GPIO01 = 1UL << 1;
        public const ulong GPIO02 = 1UL << 2;
        public const ulong GPIO03 = 1UL << 3;
        public const ulong GPIO04 = 1UL << 4;
        public const ulong GPIO05 = 1UL << 5;
        public const ulong GPIO06 = 1UL << 6;
        public const ulong GPIO07 = 1UL << 7;
        public const ulong GPIO08 = 1UL << 8;
        public const ulong GPIO09 = 1UL << 9;
        public const ulong GPIO10 = 1UL << 10;
        public const ulong GPIO11 = 1UL << 11;
        public const ulong GPIO12 = 1UL << 12;
        public const ulong GPIO13 = 1UL << 13;
        public const ulong GPIO14 = 1UL << 14;
        public const ulong GPIO15 = 1UL << 15;
        public const ulong GPIO16 = 1UL << 16;
        public const ulong GPIO17 = 1UL << 17;
        public const ulong GPIO18 = 1UL << 18;
        public const ulong GPIO19 = 1UL << 19;
        public const ulong GPIO20 = 1UL << 20;
        public const ulong GPIO21 = 1UL << 21;
        public const ulong GPIO22 = 1UL << 22;
        public const ulong GPIO23 = 1UL << 23;
        public const ulong GPIO24 = 1UL << 24;
        public const ulong GPIO25 = 1UL << 25;
        public const ulong GPIO26 = 1UL << 26;
        public const ulong GPIO27 = 1UL << 27;
        public const ulong GPIO28 = 1UL << 28;
        public const ulong GPIO29 = 1UL << 29;
        public const ulong GPIO30 = 1UL << 30;
        public const ulong GPIO31 = 1UL << 31;
        public const ulong GPIO32 = 1UL << 32;
        public const ulong GPIO33 = 1UL << 33;
        public const ulong GPIO34 = 1UL << 34;
        public const ulong GPIO35 = 1UL << 35;
        public const ulong GPIO36 = 1UL << 36;
        public const ulong GPIO37 = 1UL << 37;
        public const ulong GPIO38 = 1UL << 38;
        public const ulong GPIO39 = 1UL << 39;
        public const ulong GPIO40 = 1UL << 40;
        public const ulong GPIO41 = 1UL << 41;
        public const ulong GPIO42 = 1UL << 42;
        public const ulong GPIO43 = 1UL << 43;
        public const ulong GPIO44 = 1UL << 44;
        public const ulong GPIO45 = 1UL << 45;
        public const ulong GPIO46 = 1UL << 46;
        public const ulong GPIO47 = 1UL << 47;
        public const ulong GPIO48 = 1UL << 48;
        public const ulong GPIO49 = 1UL << 49;
        public const ulong GPIO50 = 1UL << 50;
        public const ulong GPIO51 = 1UL << 51;
        public const ulong GPIO52 = 1UL << 52;
        public const ulong GPIO53 = 1UL << 53;
        public const ulong GPIO54 = 1UL << 54;
        public const ulong GPIO55 = 1UL << 55;
        public const ulong GPIO56 = 1UL << 56;
        public const ulong GPIO57 = 1UL << 57;
        public const ulong GPIO58 = 1UL << 58;
        public const ulong GPIO59 = 1UL << 59;
        public const ulong GPIO60 = 1UL << 60;
        public const ulong GPIO61 = 1UL << 61;
        public const ulong GPIO62 = 1UL << 62;
        public const ulong GPIO63 = 1UL << 63;
    }

    #endregion
}

