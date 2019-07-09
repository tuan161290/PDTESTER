using DGS;
using NFCCommunicationClass;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PDTESTER
{
    public class SerialHelper : SerialPort
    {
        MainWindow MainPage = MainWindow.Root;
        //public SerialPort SerialPort { get; set; }
        private int timeOut;
        public SerialHelper(string Port, int TimeOut, string InstanceName)
        {
            try
            {//SerialPort = new SerialPort(Port);
                PortName = Port;
                BaudRate = 115200;
                Parity = Parity.None;
                DataBits = 8;
                StopBits = StopBits.One;
                Handshake = Handshake.None;
                // Set the read/write timeouts
                ReadTimeout = 1;
                WriteTimeout = 10;
                Open();
                timeOut = TimeOut;
                MainPage.WriteToLog(InstanceName + " connected to " + Port);
            }
            catch (Exception ex)
            {
                MainPage.WriteToLog(ex.Message + " " + InstanceName + "\r\nConnect to " + Port + " FAILED");
            }
        }

        public SerialHelper(string Port, int baudRate, int TimeOut, string InstanceName)
        {
            try
            {
                //SerialPort = new SerialPort(Port);
                PortName = Port;
                BaudRate = baudRate;
                Parity = Parity.None;
                DataBits = 8;
                StopBits = StopBits.One;
                Handshake = Handshake.None;
                // Set the read/write timeouts
                ReadTimeout = 1;
                WriteTimeout = 1;
                Open();
                timeOut = TimeOut;
                MainPage.WriteToLog(InstanceName + " connected to " + Port);
            }
            catch (Exception ex)
            {
                MainPage.WriteToLog(ex.Message + "\r\nConnect to " + Port + " FAILED");

            }
        }

        public SerialHelper(string Port, int baudRate, int WriteTimeOut, int ReadTimeOut, string InstanceName)
        {
            //SerialPort = new SerialPort(Port);
            try
            {
                PortName = Port;
                BaudRate = baudRate;
                Parity = Parity.None;
                DataBits = 8;
                StopBits = StopBits.One;
                Handshake = Handshake.None;
                // Set the read/write timeouts
                ReadTimeout = WriteTimeOut;
                WriteTimeout = ReadTimeOut;
                Open();
                MainPage.WriteToLog(InstanceName + " connected to " + Port);
            }
            catch (Exception ex)
            {
                MainPage.WriteToLog(ex.Message + " " + InstanceName + "\r\nConnect to " + Port + " FAILED");

            }
        }

        public async Task<byte[]> ReadUCTAsync()
        {
            try
            {
                if (IsOpen)
                {
                    //if (SerialPort.IsOpen)
                    int TimeOut = 100;
                    var receiveTask = BaseStream.ReadAsync(buffer, 0, buffer.Length);
                    var isReceived = await Task.WhenAny(receiveTask, Task.Delay(TimeOut)) == receiveTask;
                    if (isReceived)
                    {
                        int Count = receiveTask.Result;
                        if (Count > 4)
                        {
                            ReceivedByte = new byte[Count];
                            Array.Copy(buffer, 0, ReceivedByte, 0, Count);
                            return ReceivedByte;
                        }
                    }
                }
                return null;
            }
            catch (OperationCanceledException ex)
            {
                string s = ex.Message;
                return null;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }
        }

        SemaphoreSlim ReadLock = new SemaphoreSlim(1, 1);
        byte[] buffer = new byte[64];
        byte[] ReceivedByte = new byte[64];
        int TimeOut = 50;
        public async Task<byte[]> ReadAsync()
        {
            try
            {
                await ReadLock.WaitAsync();
                if (IsOpen)
                {
                    var receiveTask = BaseStream.ReadAsync(buffer, 0, 64);
                    if (await Task.WhenAny(receiveTask, Task.Delay(TimeOut)) == receiveTask)
                    {
                        int Count = receiveTask.Result;
                        if (Count > 4)
                        {
                            ReceivedByte = new byte[Count - 4];
                            Array.Copy(buffer, 2, ReceivedByte, 0, Count - 4);
                            return ReceivedByte;
                        }
                    }
                }
                return null;
            }
            catch (IOException error)
            {
                string s = error.Message;
                return null;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }
            finally
            {
                ReadLock.Release();
            }
        }

        public async Task WriteAsync(byte[] Bytes)
        {
            try
            {

                if (IsOpen)
                {
                    await BaseStream.WriteAsync(Bytes, 0, Bytes.Length);
                    BaseStream.Flush();
                    Thread.Sleep(timeOut);

                }
            }
            catch (IOException error)
            {
                string s = error.Message;
                return;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return;
            }
        }

        public void CloseDevice()
        {
            Close();
            Dispose();
        }

        private static ushort[] TABLE_CRCVALUE = new ushort[]
                                            {
                                            0X0000, 0XC0C1, 0XC181, 0X0140, 0XC301, 0X03C0, 0X0280, 0XC241,
                                            0XC601, 0X06C0, 0X0780, 0XC741, 0X0500, 0XC5C1, 0XC481, 0X0440,
                                            0XCC01, 0X0CC0, 0X0D80, 0XCD41, 0X0F00, 0XCFC1, 0XCE81, 0X0E40,
                                            0X0A00, 0XCAC1, 0XCB81, 0X0B40, 0XC901, 0X09C0, 0X0880, 0XC841,
                                            0XD801, 0X18C0, 0X1980, 0XD941, 0X1B00, 0XDBC1, 0XDA81, 0X1A40,
                                            0X1E00, 0XDEC1, 0XDF81, 0X1F40, 0XDD01, 0X1DC0, 0X1C80, 0XDC41,
                                            0X1400, 0XD4C1, 0XD581, 0X1540, 0XD701, 0X17C0, 0X1680, 0XD641,
                                            0XD201, 0X12C0, 0X1380, 0XD341, 0X1100, 0XD1C1, 0XD081, 0X1040,
                                            0XF001, 0X30C0, 0X3180, 0XF141, 0X3300, 0XF3C1, 0XF281, 0X3240,
                                            0X3600, 0XF6C1, 0XF781, 0X3740, 0XF501, 0X35C0, 0X3480, 0XF441,
                                            0X3C00, 0XFCC1, 0XFD81, 0X3D40, 0XFF01, 0X3FC0, 0X3E80, 0XFE41,
                                            0XFA01, 0X3AC0, 0X3B80, 0XFB41, 0X3900, 0XF9C1, 0XF881, 0X3840,
                                            0X2800, 0XE8C1, 0XE981, 0X2940, 0XEB01, 0X2BC0, 0X2A80, 0XEA41,
                                            0XEE01, 0X2EC0, 0X2F80, 0XEF41, 0X2D00, 0XEDC1, 0XEC81, 0X2C40,
                                            0XE401, 0X24C0, 0X2580, 0XE541, 0X2700, 0XE7C1, 0XE681, 0X2640,
                                            0X2200, 0XE2C1, 0XE381, 0X2340, 0XE101, 0X21C0, 0X2080, 0XE041,
                                            0XA001, 0X60C0, 0X6180, 0XA141, 0X6300, 0XA3C1, 0XA281, 0X6240,
                                            0X6600, 0XA6C1, 0XA781, 0X6740, 0XA501, 0X65C0, 0X6480, 0XA441,
                                            0X6C00, 0XACC1, 0XAD81, 0X6D40, 0XAF01, 0X6FC0, 0X6E80, 0XAE41,
                                            0XAA01, 0X6AC0, 0X6B80, 0XAB41, 0X6900, 0XA9C1, 0XA881, 0X6840,
                                            0X7800, 0XB8C1, 0XB981, 0X7940, 0XBB01, 0X7BC0, 0X7A80, 0XBA41,
                                            0XBE01, 0X7EC0, 0X7F80, 0XBF41, 0X7D00, 0XBDC1, 0XBC81, 0X7C40,
                                            0XB401, 0X74C0, 0X7580, 0XB541, 0X7700, 0XB7C1, 0XB681, 0X7640,
                                            0X7200, 0XB2C1, 0XB381, 0X7340, 0XB101, 0X71C0, 0X7080, 0XB041,
                                            0X5000, 0X90C1, 0X9181, 0X5140, 0X9301, 0X53C0, 0X5280, 0X9241,
                                            0X9601, 0X56C0, 0X5780, 0X9741, 0X5500, 0X95C1, 0X9481, 0X5440,
                                            0X9C01, 0X5CC0, 0X5D80, 0X9D41, 0X5F00, 0X9FC1, 0X9E81, 0X5E40,
                                            0X5A00, 0X9AC1, 0X9B81, 0X5B40, 0X9901, 0X59C0, 0X5880, 0X9841,
                                            0X8801, 0X48C0, 0X4980, 0X8941, 0X4B00, 0X8BC1, 0X8A81, 0X4A40,
                                            0X4E00, 0X8EC1, 0X8F81, 0X4F40, 0X8D01, 0X4DC0, 0X4C80, 0X8C41,
                                            0X4400, 0X84C1, 0X8581, 0X4540, 0X8701, 0X47C0, 0X4680, 0X8641,
                                            0X8201, 0X42C0, 0X4380, 0X8341, 0X4100, 0X81C1, 0X8081, 0X4040
                                            };
        public static byte[] CalcCRC(byte[] Buffer)
        {
            byte nTemp;
            ushort wCRCWord = 0xFFFF;
            for (int i = 0; i < Buffer.Length; i++)
            {
                nTemp = (byte)(wCRCWord ^ (Buffer[i]));
                wCRCWord >>= 8;
                wCRCWord ^= TABLE_CRCVALUE[nTemp];
            }
            return new byte[] { (byte)wCRCWord, (byte)(wCRCWord >> 8) };
        }

    }

    public class Printer
    {
        static SemaphoreSlim PrintLock = new SemaphoreSlim(1, 1);
        public static async void Print(string UN, string Jig)
        {
            try
            {
                await PrintLock.WaitAsync();
                string Time = DateTime.Now.ToString("HH:mm:ss dd/MM/yy");
                string s = string.Format("FT \"Swiss 721 BT\", 6 : PP 70, 150 : PT \"UN: {0}\" : PP 70, 120 : PT \"{1}: {2}\" : PF\r", UN, Time, Jig);
                var Bytes = Encoding.ASCII.GetBytes(s);
                await App.PrinterCOM?.WriteAsync(Bytes);
                var Resp = await App.PrinterCOM?.ReadUCTAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                PrintLock.Release();
            }
            //FT "Swiss 721 BT", 6 : PP 70, 150 : PT "17.03.18 13:34: PD_01" : PP 70, 100 : PT "17.03.18 13:34: PD_01" : PF<CR>
        }
    }

    public class NFCDevice : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public static NFCCommunication PD_OK_COM { get; set; }
        public static NFCCommunication NFC_BUFFER_COM { get; set; }
        public static NFCCommunication LEAK_OK_COM { get; set; }
        public static NFCCommunication LEAK_NG_COM { get; set; }
        public static NFCCommunication PD_NG_COM { get; set; }

        private string uN;
        public string UN
        {
            get { return uN.Length > 8 ? uN.Substring(uN.Length - 4, 4) : "Fail"; }
            set { if (uN != value) uN = value; NotifyPropertyChanged("UN"); }
        }

        public HeadInfoData ReadHeaderInfo(NFCCommunication NFCCommunication)
        {
            if (NFCCommunication != null)
            {
                NFCCommunication.NfcOnpenCommand();
                Thread.Sleep(1000);
                var HeaderInfo = new HeadInfoData();
                HeaderInfo = NFCCommunication.NEW_HEAD_INFO();
                UN = HeaderInfo.UniqueNo;
                NFCCommunication.NfcCloseCommand();
                return HeaderInfo;
            }
            return null;
        }

        public static void Log(JigModel Jig, HeadInfoData SetInfo, TestResult RESULT)
        {

            int JigNo = int.Parse(Jig.JigDescription.Substring(Jig.JigDescription.Length - 2));
            var _LOG = new DGSv1(
                SetInfo != null ? SetInfo.ModelName : "", // Model kiểm tra
                Jig.JigDescription,   // Công đoạn kiểm tra
                "TOPD00",   // Mã công đoạn, nếu không có mã thì để là TOPD00
                JigNo           // Thứ tự jig kiểm tra
                );

            _LOG.InitializeSession();
            // Thêm thông tin log header
            _LOG.AddHeaderInfo("MODEL", SetInfo != null ? SetInfo.ModelName : ""); // Model value được lấy từ điện thoại qua NFC
            _LOG.AddHeaderInfo("P/N", SetInfo != null ? SetInfo.UniqueNo : "");  // Giá trị PN chính là UN của Set lấy qua NFC
            _LOG.AddHeaderInfo("DATE", DateTime.Now.ToString("yyyy/MM/dd"));
            _LOG.AddHeaderInfo("TIME", DateTime.Now.ToString("HH:mm:ss"));
            _LOG.AddHeaderInfo("TESTCODE", _LOG.TopCode);
            _LOG.AddHeaderInfo("JIG", _LOG.Jig);
            _LOG.AddHeaderInfo("PROGRAM", "1.23.04.18");
            _LOG.AddHeaderInfo("INIFILE", "0");
            // Thêm nội dung kiểm tra, các hạng mục kiểm tra, mỗi hạng mục add một lần
            //_LOG.AddBodyInfo(
            //    "Test item",
            //    "Test value",
            //    "spec dưới",
            //    "spec trên",
            //    true,   // true : PASS, false : FAIL
            //    "giây",
            //    "",
            //    "",
            //    "");
            //_LOG.AddBodyInfo(
            //    "Test item",
            //    "Test value",
            //    "spec dưới",
            //    "spec trên",
            //    true,   // true : PASS, false : FAIL
            //    "giây",
            //    "",
            //    "",
            //    "");

            // Thêm kết quả kiểm tra công đoạn
            _LOG.AddResultInfo("RESULT", RESULT.ToString());  // PASS or FAIL
            TimeSpan TestTime = TimeSpan.ParseExact(Jig.ElapseTime, "mm\\:ss", CultureInfo.InvariantCulture);
            if (RESULT == TestResult.FAIL)
            {
                MainWindow.Root.WriteToLog(string.Format("{{1} - {2} FAIL", DateTime.Now.ToString("HH:mm:ss yyyy/MM/dd"), SetInfo != null ? SetInfo.UniqueNo : "", Jig.JigDescription));
                Printer.Print(SetInfo != null ? SetInfo.UniqueNo : "", Jig.JigDescription);
            }
            _LOG.AddResultInfo("TEST-TIME", TestTime.TotalSeconds);  // Thời gian test
                                                                     // Write log sau khi kết thúc quá trình trên
            _LOG.WriteDataToFile();
        }

        public void LogFile(NFCCommunication NFCCommunication, JigModel Jig, TestResult RESULT, ref int CV)
        {
            CV = 0;
            var HeaderInfo = ReadHeaderInfo(NFCCommunication);
            Log(Jig, HeaderInfo, RESULT);
            CV = 200;
        }

    }

    internal class HiPerfTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);

        private long startTime, stopTime;
        private long freq;

        // Constructor
        public HiPerfTimer()
        {
            startTime = 0;
            stopTime = 0;

            if (QueryPerformanceFrequency(out freq) == false)
            {
                // high-performance counter not supported
                throw new Win32Exception();
            }
        }

        // Start the timer
        public void Start()
        {
            // lets do the waiting threads there work
            //Thread.Sleep(0);
            QueryPerformanceCounter(out startTime);
        }

        // Stop the timer
        public void Stop()
        {
            QueryPerformanceCounter(out stopTime);
        }

        // Returns the duration of the timer (in seconds)
        public double Duration => (double)((stopTime - startTime) * 1000) / freq;
    }
}
