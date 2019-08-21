using PD_lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;
using static GPIOCommunication.GPIOHelper;

namespace PDTESTER
{
    public enum TestResult { FAIL, PASS, TEST, READY, NOT_USE, RETEST, ABORTED, BLOCKED, REPACK };
    public enum UCTStatus { STOP, NORESP, READY, MODE15, UD31, UO31, UO20, PDC, LOAD, VCON, SBU, FINISHED, NOT_USE };

    public class JigModel : INotifyPropertyChanged
    {
        public int JigModelID { get; set; }
        public string JigDescription { get; set; }
        public int JigID { get; set; }
        public int Channel { get; set; }
        private TestResult _TestResult = TestResult.READY;
        public TestResult TestResult
        {
            get
            {
                return _TestResult;
            }
            set
            {
                if (_TestResult != value)
                {
                    _TestResult = value;
                    NotifyPropertyChanged("TestResult");
                }
            }
        }
        //--------------------------------------------------------
        private UCTStatus _JigState = UCTStatus.NORESP;
        [NotMapped]
        public UCTStatus JigState
        {
            get { return _JigState; }
            set
            {
                if (_JigState != value)
                    _JigState = value; NotifyPropertyChanged("JigState");
            }
        }
        [NotMapped]
        public bool IsRetested { get; set; }
        [NotMapped]
        public UCTStatus PreJigState { get; set; }

        private float _FailRate = 0;
        public float FailRate
        {
            get { return _FailRate; }
            set { _FailRate = value; NotifyPropertyChanged("FailRate"); }
        }
        private int _OKCounter;
        public int OKCounter
        {
            get { return _OKCounter; }
            set
            {
                if (_OKCounter != value)
                {
                    _OKCounter = value;
                    if (NGCounter + OKCounter != 0)
                        FailRate = (float)NGCounter * 100 / (NGCounter + OKCounter);
                    else FailRate = 0;
                    NotifyPropertyChanged("OKCounter");
                }
            }
        }
        private int _NGCounter;
        public int NGCounter
        {
            get { return _NGCounter; }
            set
            {
                if (value != _NGCounter)
                {
                    _NGCounter = value;
                    if (NGCounter + OKCounter != 0)
                        FailRate = (NGCounter * 100) / (NGCounter + OKCounter);
                    else FailRate = 0;
                    NotifyPropertyChanged("NGCounter");
                }
            }
        }

        private bool isJigEnable = true;
        public bool IsJigEnable
        {
            get
            {
                return isJigEnable;
            }
            set
            {
                if (value != isJigEnable)
                {
                    isJigEnable = value;
                    NotifyPropertyChanged("IsJigEnable");
                }
            }
        }
        private bool isSetInJig = false;
        public bool IsSetInJig
        {
            get
            {
                return isSetInJig;
            }
            set
            {
                if (value != isSetInJig)
                {
                    isSetInJig = value;
                    NotifyPropertyChanged("IsSetInJig");
                }
            }
        }
        //--------------------------------------------------------
        private int _JigPos;
        public int JigPos { get { return _JigPos; } set { _JigPos = value; NotifyPropertyChanged("JigPos"); } }
        private string _ElapseTime = "00:00";

        [NotMapped]
        public string ElapseTime
        {
            get { return _ElapseTime; }
            set
            {
                if (_ElapseTime != value)
                {
                    _ElapseTime = value; NotifyPropertyChanged("ElapseTime");
                }
            }
        }

        [NotMapped]
        public DateTime StatTime { get; set; } = DateTime.Now;

        //[NotMapped]
        //public TimeSpan AvgTestTime { get; set; }

        [NotMapped]
        public int FailCounter { get; set; }

        private event PropertyChangedEventHandler propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class NVValue : INotifyPropertyChanged
    {
        public string NVNumber { get; set; }
        public string _NVResult = "N";
        public string NVResult
        {
            get { return _NVResult; }
            set
            {
                if (_NVResult != value)
                {
                    _NVResult = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NVResult)));
                }
            }
        }
        public bool IsEnable { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class UCT100
    {
        public List<PDJig> PDs { get; set; } = new List<PDJig>();
        public OutputPin PowerPin { get; set; }
        bool resetFlag;
        public bool ResetFlag
        {
            get { return resetFlag; }
            set
            {
                if (!Auto.UCT8Mode) resetFlag = value;
            }
        }
    }

    public class PDJig
    {
        public OutputPin PackingPin { get; set; }
        public OutputPin PowerPin { get; set; }
        public JigModel Jig { get; set; }
        public ObservableCollection<TestItem> TestItems { get; set; } = new ObservableCollection<TestItem>() {
            new TestItem { Item = "USB3.1", TestItemStatus = TestItemStatus.NT , TestEnable = App.SW.UD3Test},
            new TestItem { Item = "OTG3.1", TestItemStatus = TestItemStatus.NT, TestEnable = App.SW.UO3Test },
            new TestItem { Item = "OTG2.0", TestItemStatus = TestItemStatus.NT , TestEnable = App.SW.UO2Test},
            new TestItem { Item = "PD", TestItemStatus = TestItemStatus.NT, TestEnable = App.SW.PDCTest },
            new TestItem { Item = "LOAD", TestItemStatus = TestItemStatus.NT, TestEnable = App.SW.LOADTest },
            new TestItem { Item = "VCONN", TestItemStatus = TestItemStatus.NT, TestEnable = App.SW.VCONNTest},
            new TestItem { Item = "SBU", TestItemStatus = TestItemStatus.NT, TestEnable = App.SW.SBUTest },
        };
        public int Channel { get; set; }
        public int SWID { get; set; }
        public bool Resetting { get; set; }
        public int TestCount { get; set; } = 0;
        public int Mode15Count { get; set; }
    }
    public enum TestItemStatus { PASS, FAIL, TEST, NT, SKIP }
    public class TestItem : INotifyPropertyChanged
    {
        private string _Item;
        public string Item
        {
            get { return _Item; }
            set
            {
                if (_Item != value)
                {
                    _Item = value;
                    //NotifyPropertyChanged("Item");
                }
            }
        }
        public bool TestEnable { get; set; }
        private TestItemStatus _TestItemStatus;
        public TestItemStatus TestItemStatus
        {
            get { return _TestItemStatus; }
            set
            {
                if (_TestItemStatus != value)
                {
                    _TestItemStatus = value;
                    //NotifyPropertyChanged("TestItemStatus");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class TVOCJig
    {
        public InputPin OKPin { get; set; } = null;
        public InputPin NGPin { get; set; } = null;
        public InputPin TESTPin { get; set; } = null;
        public JigModel Jig { get; set; } = null;
        public InputPin InTesting { get; set; }
        int _Delay;
        public int Delay
        {
            get { return _Delay; }
            set
            {
                if (value != _Delay)
                {
                    _Delay = value;
                    NotifyPropertyChanged("Delay");
                    Database.SetDbValue("TVOCDelay", value.ToString());
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsTurnEnable { get; set; }
    }

    public class SIMJig
    {
        public int MaxRepack = 1;
        public static bool IsCOMOpened { get; set; }
        public string COM { get; set; }
        public JigModel Jig { get; set; }
        public OutputPin PackingPin { get; set; }
        public OutputPin PressPin { get; set; }
        public InputPin PackInSensor { get; set; }
        public InputPin PackOutSensor { get; set; }
        public InputPin PressSensor { get; set; }
        public InputPin ReleaseSensor { get; set; }
        public JigModel Axis01SIM { get; set; }
        public NVValue SDCardTest { get; set; } = new NVValue() { NVNumber = "24" };
        public NVValue SIMTest { get; set; } = new NVValue() { NVNumber = "37" };
        public NVValue NFCTest { get; set; } = new NVValue() { NVNumber = "145" };
        public TestResult TestResult { get; set; }
        public CancellationTokenSource TestCancellationTokenSource { get; private set; }
        public NFC NFC { get; set; }
        public SIMJig()
        {

        }
        object lockObject = new object();
        public void StopTestLoop()
        {
            Task.Run(async () =>
            {
                lock (lockObject)
                    if (TestCancellationTokenSource != null)
                        if (!TestCancellationTokenSource.Token.IsCancellationRequested)
                            TestCancellationTokenSource.Cancel();
                await SimLockObject.WaitAsync();
                TestCancellationTokenSource = new CancellationTokenSource();
                try
                {
                    StopTest_Step = 0;
                    TestCancellationTokenSource.CancelAfter(15000);
                    while (true)
                    {
                        await Task.Delay(50);
                        TestCancellationTokenSource.Token.ThrowIfCancellationRequested();
                        if (StopTest_Step == 0)
                        {
                            await PackingPin.RST();
                            if (PackOutSensor.PinValue == PinValue.ON)
                                StopTest_Step = 1;
                        }
                        else if (StopTest_Step == 1)
                        {
                            await PressPin.RST();
                            if (ReleaseSensor.PinValue == PinValue.ON)
                            {
                                StopTest_Step = 2;
                            }
                        }
                        else if (StopTest_Step == 2)
                        {
                            return;
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    SimLockObject.Release();
                }
            });
        }
        SemaphoreSlim SimLockObject = new SemaphoreSlim(1, 1);
        int Test_Step = 0, StopTest_Step = 0, RepackCount = 0;
        public void StartTestLoop()
        {
            //return;
            Task.Run(async () =>
            {
                if (SimLockObject.CurrentCount == 0)
                    return;
                lock (lockObject)
                    if (TestCancellationTokenSource != null)
                        if (!TestCancellationTokenSource.Token.IsCancellationRequested)
                            TestCancellationTokenSource.Cancel();
                await SimLockObject.WaitAsync();
                TestCancellationTokenSource = new CancellationTokenSource();
                TestCancellationTokenSource.CancelAfter(30000);
                try
                {
                    //NFCTest.NVResult = "T";
                    //SDCardTest.NVResult = "T";
                    SIMTest.NVResult = "T";
                    Test_Step = 0;
                    RepackCount = 0;
                    NFC.NFC_OpenComand();
                    await Task.Delay(200);
                    //SDCardTest.NVResult = NFC.Get_NV(SDCardTest.NVNumber);
                    SIMTest.NVResult = NFC.Get_NV(SIMTest.NVNumber);
                    NFCTest.NVResult = NFC.Get_NV(NFCTest.NVNumber);
                    if (/*SDCardTest.NVResult == "P" && */SIMTest.NVResult == "P" && NFCTest.NVResult == "P")
                    {
                        Jig.TestResult = TestResult.PASS;
                        Jig.OKCounter++;
                        return;
                    }
                    NFC.GO_MAIN_SCREN();
                    bool IsRepack = false;
                    while (true)
                    {
                        await Task.Delay(50);
                        TestCancellationTokenSource.Token.ThrowIfCancellationRequested();
                        //if (Auto.Page.PDRUN)
                        if (Test_Step == 0)
                        {
                            if (ReleaseSensor.PinValue == PinValue.ON)
                                await PressPin.SET();
                            if (PressSensor.PinValue == PinValue.ON)
                            {
                                Test_Step = 1;
                                NFC.NFC_CloseComand();
                            }
                        }
                        else if (Test_Step == 1)
                        {
                            if (PackOutSensor.PinValue == PinValue.ON)
                                await PackingPin.SET();
                            if (PackInSensor.PinValue == PinValue.ON && PackingPin.PinValue == PinValue.ON)
                            {
                                Test_Step = 2;
                            }
                        }
                        else if (Test_Step == 2)
                        {
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    await Task.Delay(7000, TestCancellationTokenSource.Token);
                                    if (NFC.NFC_OpenComand())
                                    {
                                        await Task.Delay(200);
                                        break;
                                    }
                                }
                                Test_Step = 3;
                            }
                        }
                        else if (Test_Step == 3)
                        {
                            NFC.GO_15MODE();
                            Test_Step = 4;
                        }
                        else if (Test_Step == 4)
                        {
                            await Task.Delay(1000);
                            // NFC = new NFC(COM);
                            // Thread.Sleep(500);
                            //SDCardTest.NVResult = NFC.Get_NV(SDCardTest.NVNumber);
                            SIMTest.NVResult = NFC.Get_NV(SIMTest.NVNumber);
                            NFCTest.NVResult = NFC.Get_NV(NFCTest.NVNumber);
                            Test_Step = 5;
                        }
                        else if (Test_Step == 5)
                        {
                            if (/*SDCardTest.NVResult == "P" &&*/ SIMTest.NVResult == "P" && NFCTest.NVResult == "P")
                            {
                                Jig.TestResult = TestResult.PASS;
                                await PackingPin.RST();
                                Jig.OKCounter++;
                                return;
                            }
                            else
                            {
                                IsRepack = true;
                                goto Restest;
                            }
                            //Test_Step = 6;
                        }
                    //else if (Test_Step == 6)
                    //{
                    //    //await PackingPin.RST();
                    //    //if (PackOutSensor.PinValue == PinValue.ON)
                    //    Test_Step = 7;
                    //}
                    //else if (Test_Step == 7)
                    //{
                    //    //await PressPin.RST();
                    //    //if (PressSensor.PinValue == PinValue.ON)
                    //    Test_Step = 8;
                    //}
                    //else if (Test_Step == 8)
                    //{
                    //    return;
                    //}
                    Restest:
                        if (IsRepack)
                        {
                            IsRepack = false;
                            if (RepackCount < MaxRepack)
                            {
                                RepackCount++;
                                await PackingPin.RST();
                                NFC.GO_MAIN_SCREN();
                                Test_Step = 1;
                                continue;
                            }
                            else
                            {
                                Jig.NGCounter++;
                                Jig.TestResult = TestResult.FAIL;
                                await PackingPin.RST();
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
                finally
                {
                    NFC.NFC_CloseComand();
                    SimLockObject.Release();
                }
            });
        }
    }


    //--------------------------------------------
    public class NFCJig
    {
        public int TestCount { get; set; } = 0;
        public OutputPin PackingPin { get; set; } = null;
        public InputPin OKPIN { get; set; } = null;
        public InputPin NGPIN { get; set; } = null;
        public JigModel Jig { get; set; } = null;

    }

    //--------------------------------------------
    public class LeakJig
    {
        public OutputPin PackingPin { get; set; }
        public JigModel Jig { get; set; } = null;
        public OutputPin PressSolenoid { get; set; }
        public OutputPin TransferSolenoid { get; set; }
        public InputPin UpSensor { get; set; }
        public InputPin DownSenSor { get; set; }
        public InputPin ForwardSensor { get; set; }
        public InputPin ReverseSensor { get; set; }
        public InputPin OKPIN { get; set; }
        public InputPin NGPIN { get; set; }
        public int TestCount { get; set; }

        CancellationTokenSource WorkCancellationSource = null;
        //bool Pressing = false;
        public SemaphoreSlim LockObject = new SemaphoreSlim(1, 1);
        public async Task<bool> PressTask()
        {
            try
            {

                if (WorkCancellationSource != null)
                {
                    if (!WorkCancellationSource.IsCancellationRequested)
                        WorkCancellationSource.Cancel();
                }
                await LockObject.WaitAsync();
                WorkCancellationSource = new CancellationTokenSource();
                WorkCancellationSource.CancelAfter(10000);
                int STEP = 0;
                while (true)
                {
                    WorkCancellationSource.Token.ThrowIfCancellationRequested();
                    if (STEP == 0 && UpSensor.PinValue == PinValue.ON)
                    {
                        await TransferSolenoid.SET();
                        if (ForwardSensor.PinValue == PinValue.ON)
                            STEP = 1;
                    }
                    if (STEP == 1)
                    {
                        await PressSolenoid.SET();
                        if (DownSenSor.PinValue == PinValue.ON)
                        {
                            await PackingPin.SET();
                            //WorkCancellationSource.Dispose();
                            return true;
                        }
                    }
                    await Task.Delay(100);
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                LockObject.Release();
            }
        }
        public async void Press()
        {
            await PressTask();
        }

        //bool Releasing = false;
        //CancellationTokenSource ReleaseCancellationSource = null;
        public async Task<bool> ReleaseTask()
        {
            try
            {
                if (WorkCancellationSource != null)
                {
                    if (!WorkCancellationSource.IsCancellationRequested)
                        WorkCancellationSource.Cancel();
                }
                await LockObject.WaitAsync();
                WorkCancellationSource = new CancellationTokenSource();
                WorkCancellationSource.CancelAfter(10000);
                int STEP = 0;
                await PackingPin.RST();
                await Task.Delay(500);
                while (true)
                {
                    WorkCancellationSource.Token.ThrowIfCancellationRequested();
                    if (STEP == 0)
                    {
                        await PressSolenoid.RST();
                        if (UpSensor.PinValue == PinValue.ON)
                            STEP = 1;
                    }
                    if (STEP == 1)
                    {
                        await TransferSolenoid.RST();
                        if (ReverseSensor.PinValue == PinValue.ON)
                        {
                            return true;
                        }
                    }
                    await Task.Delay(100);
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                LockObject.Release();
            }
        }
        public async void Release()
        {
            await ReleaseTask();
        }
    }
}
//--------------------------------------------





