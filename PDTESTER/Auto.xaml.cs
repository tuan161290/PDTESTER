using Newtonsoft.Json;
using PD_lib;
using PDTESTER.ActionWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using static GPIOCommunication.GPIOHelper;
using static GPIOCommunication.GPIOHelper.GPIOPin;

namespace PDTESTER
{
    /// <summary>
    /// Interaction logic for Auto.xaml
    /// </summary>
    public partial class Auto : Page, INotifyPropertyChanged
    {
        public static Auto Page;
        MainWindow MainPage;

        public static void CancelLoopTask()
        {
            if (MainLoopCancelationTokenSource != null)
            {
                if (!MainLoopCancelationTokenSource.IsCancellationRequested)
                {
                    MainLoopCancelationTokenSource.Cancel();
                }
            }
            if (ReadUCTCancelationTokenSource != null)
            {
                if (!ReadUCTCancelationTokenSource.IsCancellationRequested)
                {
                    ReadUCTCancelationTokenSource.Cancel();
                }
            }
        }

        //Stopwatch sw = new Stopwatch();
        //private async Task ServoTask(CancellationToken token)
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            token.ThrowIfCancellationRequested();
        //            sw.Reset();
        //            sw.Start();
        //            var axis1Status = await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.GetAxisStatus, null);
        //            if (axis1Status != null)
        //            {
        //                Axis._01.Status = BitConverter.ToInt32(axis1Status, 3);//3 = Index of High Byte INT                                                                               
        //                if ((Axis._01.Status & BitMask.Motioning) > 0) //GET AXIS#1 Motioning
        //                {

        //                }
        //                if ((Axis._01.Status & BitMask.ERRORALL) > 0)
        //                {

        //                }
        //            }
        //            var axis1currentpos = await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.GetActualPosition, null);
        //            if (axis1currentpos != null)
        //                Axis._01.CurrentPos = BitConverter.ToInt32(axis1currentpos, 3);
        //            //--------------------------------------------------
        //            var axis2Status = await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.GetAxisStatus, null);
        //            if (axis2Status != null)
        //            {
        //                Axis._02.Status = BitConverter.ToInt32(axis2Status, 3);//3 = Index of High Byte INT                                                                               
        //                if ((Axis._02.Status & BitMask.Motioning) > 0) //GET AXIS#1 Motioning
        //                {

        //                }
        //                if ((Axis._02.Status & BitMask.ERRORALL) > 0)
        //                {

        //                }
        //            }
        //            var axis2currentpos = await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.GetActualPosition, null);
        //            if (axis2currentpos != null)
        //                Axis._02.CurrentPos = BitConverter.ToInt32(axis2currentpos, 3);
        //            //--------------------------------------------------
        //            var axis3Status = await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.GetAxisStatus, null);
        //            if (axis3Status != null)
        //            {
        //                Axis._03.Status = BitConverter.ToInt32(axis3Status, 3);//3 = Index of High Byte INT                                                                               
        //                if ((Axis._03.Status & BitMask.Motioning) > 0) //GET AXIS#1 Motioning
        //                {

        //                }
        //                if ((Axis._03.Status & BitMask.ERRORALL) > 0)
        //                {

        //                }
        //            }
        //            var axis3currentpos = await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.GetActualPosition, null);
        //            if (axis3currentpos != null)
        //                Axis._03.CurrentPos = BitConverter.ToInt32(axis3currentpos, 3);
        //            //--------------------------------------------------
        //            var axis4Status = await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.GetAxisStatus, null);
        //            if (axis4Status != null)
        //            {
        //                Axis._04.Status = BitConverter.ToInt32(axis4Status, 3);//3 = Index of High Byte INT                                                                               
        //                if ((Axis._04.Status & BitMask.Motioning) > 0) //GET AXIS#1 Motioning
        //                {

        //                }
        //                if ((Axis._04.Status & BitMask.ERRORALL) > 0)
        //                {

        //                }
        //            }
        //            var axis4currentpos = await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.GetActualPosition, null);
        //            if (axis4currentpos != null)
        //                Axis._04.CurrentPos = BitConverter.ToInt32(axis4currentpos, 3);
        //            Axis._01.RespondTime = (int)sw.ElapsedMilliseconds;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string s = ex.Message;
        //    }
        //}
        //private void ServoLoop()
        //{
        //    ServoLoopCancelationTokenSource = new CancellationTokenSource();
        //    Task.Run(async () => await ServoTask(ServoLoopCancelationTokenSource.Token));
        //}

        Stopwatch sw2 = new Stopwatch();
        bool PDAlarm, NFCAlarm;
        private async Task MainTask(CancellationToken token)
        {
            try
            {
                NGCVRun = 100;
                TransCVRun = 100;
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(30);
                    //sw2.Reset();
                    //sw2.Start();
                    await App.GPIOBoardF0.GetGPIOsState();
                    await App.GPIOBoardF1.GetGPIOsState();
                    await App.GPIOBoardF2.GetGPIOsState();
                    //await OUT.LOADING_CV_02_RELAY.SET();
                    //Axis._02.RespondTime = (int)sw2.ElapsedMilliseconds;
                    if (pDRUN && nFC_LEAK_RUN)
                    {
                        if (IN.INPUT_CV_SENSOR_01.PinValue == PinValue.OFF || IN.INPUT_CV_SENSOR_02.PinValue == PinValue.OFF)
                        {
                            InputCVRun1 = 1200;
                        }
                        if (InputCVRun1 > 0)
                        {
                            if (IN.INPUT_CV_SENSOR_02.PinValue == PinValue.ON || IN.INPUT_CV_SENSOR_01.PinValue == PinValue.ON)
                                await OUT.LOADING_CV_01_RELAY.SET();
                            else
                                await OUT.LOADING_CV_01_RELAY.RST();

                            if (IN.INPUT_CV_SENSOR_02.PinValue == PinValue.ON)
                            {
                                Input_CV_Delay = 5;
                                await OUT.LOADING_CV_02_RELAY.SET();
                            }
                            else if (Input_CV_Delay > 0)
                            {
                                Input_CV_Delay--;
                            }
                            if (Input_CV_Delay == 0)
                            {
                                await OUT.LOADING_CV_02_RELAY.RST();
                            }
                        }
                        else
                        {
                            Input_CV_Delay = 5;
                            await OUT.LOADING_CV_02_RELAY.RST();
                            await OUT.LOADING_CV_01_RELAY.RST();
                        }

                        if (IN.TRANSFER_CV_SENSOR_END.PinValue == PinValue.ON && TransCVRun > 0)
                            await OUT.TRANSFER_CV_RELAY.SET();
                        else if (Trans_CV_Debounce == 0 || TransCVRun == 0)
                            await OUT.TRANSFER_CV_RELAY.RST();

                        if (IN.TRANSFER_CV_SENSOR_END.PinValue == PinValue.ON)
                            Trans_CV_Debounce = 5;
                        if (Trans_CV_Debounce > 0)
                            Trans_CV_Debounce--;

                        if (IN.NG_CV_SENSOR_04.PinValue == PinValue.ON && NGCVRun > 0)
                            await OUT.NG_CV_RELAY.SET();
                        else
                            await OUT.NG_CV_RELAY.RST();

                        if (IN.UNLOADING_CV_SENSOR_03.PinValue == PinValue.ON && OutputCVRun > 0)// || IN.UNLOADING_CV_SENSOR_03.PinValue == PinValue.ON)
                            await OUT.OUTPUT_CV_RELAY.SET();
                        else
                            await OUT.OUTPUT_CV_RELAY.RST();
                        if (IN.TRANSFER_CV_SENSOR_BEGIN.PinValue == PinValue.OFF) TransCVRun = 300;
                    }
                    else if (PDBypass && NFCBypass && TVOCBypass && LEAKBypass && !nFCLoopExcuting && !pDLoopExcuting)
                    {
                        if (IN.UNLOADING_CV_SENSOR_03.PinValue == PinValue.ON &&
                            IN.ESTOP_BUTTON_01.PinValue == PinValue.ON &&
                            IN.ESTOP_BUTTON_02.PinValue == PinValue.ON)
                        {
                            await OUT.LOADING_CV_01_RELAY.SET();
                            await OUT.LOADING_CV_02_RELAY.SET();
                            await OUT.OUTPUT_CV_RELAY.SET();
                        }
                        else
                        {
                            await OUT.LOADING_CV_01_RELAY.RST();
                            await OUT.LOADING_CV_02_RELAY.RST();
                            await OUT.OUTPUT_CV_RELAY.RST();
                        }
                    }
                    else if (PDLoopExcuting && NFCLoopExcuting && (!NFC_LEAK_RUN || !PDRUN))
                    {
                        await OUT.LOADING_CV_01_RELAY.RST();
                        await OUT.LOADING_CV_02_RELAY.RST();
                        await OUT.NG_CV_RELAY.RST();
                        await OUT.TRANSFER_CV_RELAY.RST();
                        await OUT.OUTPUT_CV_RELAY.RST();
                    }
                    //---------------------------------------------------------------------------
                    if (IN.ESTOP_BUTTON_01.PinValue == PinValue.ON &&
                        IN.ESTOP_BUTTON_02.PinValue == PinValue.ON &&
                        IN.DOOR1_SENSOR.PinValue == PinValue.ON &&
                        Axis._01.SON == PinValue.ON &&
                        Axis._02.SON == PinValue.ON && !PDAlarm)
                        PDReady = true;
                    else
                    {
                        PDReady = false;
                        PDRUN = false;//PD Loop is stopped if NFC_LEAK_RUN = false;
                    }

                    if (IN.ESTOP_BUTTON_01.PinValue == PinValue.ON &&
                        IN.ESTOP_BUTTON_02.PinValue == PinValue.ON &&
                        IN.DOOR2_SENSOR.PinValue == PinValue.ON &&
                        Axis._03.SON == PinValue.ON &&
                        Axis._04.SON == PinValue.ON && !NFCAlarm)
                        NFCReady = true;
                    else
                    {
                        NFCReady = false;
                        NFC_LEAK_RUN = false;//NFC Loop is stopped if NFC_LEAK_RUN = false;
                    }

                    //---------------------------------------------------------------------------
                    if (IN.ESTOP_BUTTON_01.PinValue == PinValue.OFF || IN.ESTOP_BUTTON_02.PinValue == PinValue.OFF)
                    {
                        await OUT.RED_LIGHT.SET();
                        await OUT.GREEN_LIGHT.RST();
                        await OUT.ORANGE_LIGHT.RST();
                    }
                    else if (PDRUN && NFC_LEAK_RUN)
                    {
                        await OUT.RED_LIGHT.RST();
                        await OUT.GREEN_LIGHT.SET();
                        await OUT.ORANGE_LIGHT.RST();
                        await OUT.START_BUTTON_01_LED.SET();
                        await OUT.START_BUTTON_02_LED.SET();
                        await OUT.STOP_BUTTON_01_LED.RST();
                        await OUT.STOP_BUTTON_02_LED.RST();

                    }
                    else if (PDRUN == false || NFC_LEAK_RUN == false)
                    {
                        await OUT.RED_LIGHT.RST();
                        await OUT.GREEN_LIGHT.RST();
                        await OUT.ORANGE_LIGHT.SET();
                        await OUT.START_BUTTON_01_LED.RST();
                        await OUT.START_BUTTON_02_LED.RST();
                        await OUT.STOP_BUTTON_01_LED.SET();
                        await OUT.STOP_BUTTON_02_LED.SET();
                    }
                    //---------------------------------------------------------------------------
                    if (NFCLoopExcuting)
                    {
                        foreach (NFCJig NFC in NFCs)
                        {
                            if (NFC.Jig.TestResult == TestResult.TEST ||
                                NFC.Jig.TestResult == TestResult.RETEST)
                            {
                                NFC.Jig.ElapseTime = (DateTime.Now - NFC.Jig.StatTime).ToString("mm\\:ss");
                                if ((DateTime.Now - NFC.Jig.StatTime) >= TimeSpan.FromMinutes(1))
                                {
                                    NFC.Jig.TestResult = TestResult.FAIL;
                                    NFC.TestCount = 0;
                                    await NFC.PackingPin.RST();
                                }
                            }
                            else NFC.Jig.StatTime = DateTime.Now;
                        }
                        foreach (LeakJig LEAK in LEAKs)
                        {
                            if (LEAK.Jig.TestResult == TestResult.TEST ||
                                LEAK.Jig.TestResult == TestResult.RETEST)
                            {
                                LEAK.Jig.ElapseTime = (DateTime.Now - LEAK.Jig.StatTime).ToString("mm\\:ss");
                                if ((DateTime.Now - LEAK.Jig.StatTime) >= TimeSpan.FromMinutes(1))
                                {
                                    LEAK.TestCount = 0;
                                    LEAK.Jig.TestResult = TestResult.FAIL;
                                    LEAK.Release();
                                }
                            }
                            else LEAK.Jig.StatTime = DateTime.Now;
                        }

                    }
                    if (pDLoopExcuting)
                        foreach (SIMJig SIM in SIMs)
                        {
                            if (SIM.Jig.TestResult == TestResult.TEST ||
                                SIM.Jig.TestResult == TestResult.RETEST)
                            {
                                SIM.Jig.ElapseTime = (DateTime.Now - SIM.Jig.StatTime).ToString("mm\\:ss");
                            }
                            else SIM.Jig.StatTime = DateTime.Now;
                            if (!SIM.Jig.IsJigEnable) SIM.Jig.TestResult = TestResult.NOT_USE;
                        }
                    if (TotalOK > 0)
                        CycleTime = DateTime.Now - PreCycleTime;
                    foreach (NFCJig NFC in NFCs)
                    {
                        if (!NFC.Jig.IsJigEnable) NFC.Jig.TestResult = TestResult.NOT_USE;
                    }
                    foreach (LeakJig Leak in LEAKs)
                    {
                        if (!Leak.Jig.IsJigEnable) Leak.Jig.TestResult = TestResult.NOT_USE;
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        private void MainLoop()
        {
            MainLoopCancelationTokenSource = new CancellationTokenSource();
            Task.Run(async () => await MainTask(MainLoopCancelationTokenSource.Token));
        }

        private async Task ReadUCTTask(CancellationToken token)
        {
            try
            {
                while (App.UCTCOM != null)
                {
                    Thread.Sleep(100);
                    foreach (UCT100 UCT in UCT100s)
                    {
                        foreach (PDJig PD in UCT.PDs)
                        {
                            token.ThrowIfCancellationRequested();
                            if (!PD.Jig.IsJigEnable)
                            {
                                if (PD.Jig.FailCounter >= 5)
                                {
                                    PD.Jig.TestResult = TestResult.BLOCKED;
                                    PD.Jig.JigState = UCTStatus.NOT_USE;
                                }
                                else
                                {
                                    PD.Jig.TestResult = TestResult.NOT_USE;
                                    PD.Jig.JigState = UCTStatus.NOT_USE;
                                }
                                goto Finish;
                            }
                            PD.Jig.JigState = await App.UCTCOM.GetTestStatus(PD.SWID, PD.Channel);
                            UCTStatus CurrentJigState = PD.Jig.JigState;
                            if (CurrentJigState != PD.Jig.PreJigState && PD.Resetting == false)
                            {
                                if (CurrentJigState != UCTStatus.NORESP)
                                    await App.UCTCOM.GetTestItemResults(PD.SWID, PD.Channel, PD.TestItems);

                                if (CurrentJigState == UCTStatus.FINISHED && PD.Jig.IsSetInJig &&
                                    PD.Jig.TestResult != TestResult.PASS && PD.Jig.TestResult != TestResult.FAIL)
                                {
                                    foreach (TestItem T in PD.TestItems)
                                    {
                                        if ((T.TestItemStatus == TestItemStatus.FAIL || T.TestItemStatus == TestItemStatus.NT || T.TestItemStatus == TestItemStatus.TEST) && T.TestEnable)
                                        {
                                            PD.TestCount = 0;
                                            PD.Mode15Count = 0;
                                            PD.Jig.TestResult = TestResult.FAIL;
                                            UCT.ResetFlag = true;
                                            PD.Jig.FailCounter++;
                                            if (PD.Jig.FailCounter >= 5)
                                            {
                                                PD.Jig.FailCounter = 0;
                                                PD.Jig.IsJigEnable = false;
                                            }
                                            goto Finish;
                                        }
                                    }
                                    {
                                        PD.TestCount = 0;
                                        PD.Mode15Count = 0;
                                        PD.Jig.TestResult = TestResult.PASS;
                                        PD.Jig.FailCounter = 0;
                                    }
                                }
                                else if (CurrentJigState == UCTStatus.READY)
                                {
                                    foreach (TestItem T in PD.TestItems)
                                    {
                                        T.TestItemStatus = TestItemStatus.NT;
                                    }
                                }
                                else if (CurrentJigState == UCTStatus.MODE15 && PD.Jig.IsSetInJig)
                                {
                                    PD.Mode15Count++;
                                    if (PD.Jig.TestResult != TestResult.FAIL)
                                        if (PD.Mode15Count >= 2)
                                        {
                                            PD.Mode15Count = 0;
                                            if (PD.TestCount < 2)
                                            {
                                                PD.TestCount++;
                                                Repack(PD);
                                            }
                                            else
                                            {
                                                PD.Jig.TestResult = TestResult.FAIL;
                                                UCT.ResetFlag = true;
                                                PD.TestCount = 0;
                                            }
                                        }
                                }
                                PD.Jig.PreJigState = CurrentJigState;
                            }
                        Finish:
                            if (PD.Jig.TestResult == TestResult.TEST || PD.Jig.TestResult == TestResult.RETEST)
                            {
                                if ((DateTime.Now - PD.Jig.StatTime) >= TestTimeout)
                                {
                                    PD.Jig.TestResult = TestResult.FAIL;
                                    UCT.ResetFlag = true;
                                }
                                PD.Jig.ElapseTime = (DateTime.Now - PD.Jig.StatTime).ToString("mm\\:ss");
                            }
                            else if (PD.Jig.IsJigEnable == true)
                                PD.Jig.StatTime = DateTime.Now;
                        }
                    }
                }
            }
            catch (OperationCanceledException opc)
            {
                string s = opc.Message;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        private async void Repack(PDJig PD)
        {
            MainPage.WriteToLog("Repack " + PD.Jig.JigDescription);
            await App.UCTCOM.UCTTestSwitch(PD.SWID, PD.Channel, 0);
            Thread.Sleep(100);
            await PD.PackingPin.RST();
            await App.UCTCOM.UCTTestSwitch(PD.SWID, PD.Channel, 1);
            Thread.Sleep(1000);
            await PD.PackingPin.SET();
        }

        private void ReadUCTLoop()
        {
            ReadUCTCancelationTokenSource = new CancellationTokenSource();
            Task.Run(async () => await ReadUCTTask(ReadUCTCancelationTokenSource.Token));
        }

        private async Task ORGExecution(CancellationToken token)
        {
            try
            {
                ORGStep = 0;
                //TVOCStep = 0;
                //PD_TVOC_STEP = 0; PDStep = 0; PD_TVOC_STEP = 0; BUFFER_TVOC_STEP = 0; PD_Buf_Step = 0; BufTVOC_Step = 0; TVOCTrans_Step = 0;
                //NFC_BUFF_STEP = 0; NFC_Buf_Step = 0; Buf_Leak_Step = 0; CV_NFC_Step = 0; BUFF_LEAK_STEP = 0; LeakFinalStep = 0;
                //; NFCSTEP = 0; LEAK_STEP = 0; NFC_LEAK_STEP = 0;    
                while (IN.ESTOP_BUTTON_01.PinValue == ON && IN.ESTOP_BUTTON_02.PinValue == ON)
                {
                    Thread.Sleep(33);
                    token.ThrowIfCancellationRequested();
                    var axis1Status = await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.GetAxisStatus, null);
                    if (axis1Status != null)
                    {
                        Axis._01.Status = BitConverter.ToInt32(axis1Status, 3);//3 = Index of High Byte INT                                                                               
                    }
                    var axis2Status = await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.GetAxisStatus, null);
                    if (axis2Status != null)
                    {
                        Axis._02.Status = BitConverter.ToInt32(axis2Status, 3);//3 = Index of High Byte INT                                                                               
                    }
                    var axis3Status = await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.GetAxisStatus, null);
                    if (axis3Status != null)
                    {
                        Axis._03.Status = BitConverter.ToInt32(axis3Status, 3);//3 = Index of High Byte INT                                                                               
                    }
                    var axis4Status = await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.GetAxisStatus, null);
                    if (axis4Status != null)
                    {
                        Axis._04.Status = BitConverter.ToInt32(axis4Status, 3);//3 = Index of High Byte INT                                                                               
                    }
                    var axis1CurrentPos = await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.GetActualPosition, null);
                    if (axis1CurrentPos != null)
                    {
                        Axis._01.CurrentPos = BitConverter.ToInt32(axis1CurrentPos, 3);//3 = Index of High Byte INT                                                                               
                    }
                    var axis2CurrentPos = await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.GetActualPosition, null);
                    if (axis2CurrentPos != null)
                    {
                        Axis._02.CurrentPos = BitConverter.ToInt32(axis2CurrentPos, 3);//3 = Index of High Byte INT                                                                               
                    }
                    var axis3CurrentPos = await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.GetActualPosition, null);
                    if (axis3CurrentPos != null)
                    {
                        Axis._03.CurrentPos = BitConverter.ToInt32(axis3CurrentPos, 3);//3 = Index of High Byte INT                                                                               
                    }
                    var axis4CurrentPos = await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.GetActualPosition, null);
                    if (axis4CurrentPos != null)
                    {
                        Axis._04.CurrentPos = BitConverter.ToInt32(axis4CurrentPos, 3);//3 = Index of High Byte INT                                                                               
                    }
                    if (Axis._01.SON != PinValue.ON || Axis._02.SON != PinValue.ON || Axis._03.SON != PinValue.ON || Axis._04.SON != PinValue.ON)
                    {
                        await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.StepEnable, new List<byte>() { 0x01 });
                        await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.StepEnable, new List<byte>() { 0x01 });
                        await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.StepEnable, new List<byte>() { 0x01 });
                        await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.StepEnable, new List<byte>() { 0x01 });
                    }
                    if (ORGStep == 0)
                    {
                        ORGStep = 1;
                    }
                    if (ORGStep == 1)
                    {

                        await OUT.BUFFER_PACK.RST();
                        ORGStep = 2;
                    }
                    if (ORGStep == 2)
                    {
                        await OUT.LOADING_01_LIFT_SOL.RST();
                        await OUT.LOADING_02_LIFT_SOL.RST();
                        await OUT.LOADING_03_LIFT_SOL.RST();
                        await OUT.LOADING_04_LIFT_SOL.RST();
                        if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON &&
                            IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON &&
                            IN.LOADING_03_LIFT_UP_SENSOR.PinValue == PinValue.ON &&
                            IN.LOADING_04_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                        {
                            await OUT.LOADING_01_TURN_SOL.RST();
                            ORGStep = 3;
                        }
                    }
                    if (ORGStep == 3)
                    {
                        ORGStep = 4;
                    }
                    if (ORGStep == 4)
                    {
                        ORGStep = 5;
                    }
                    if (ORGStep == 5)
                    {
                        if (Axis._01.ORG_OK != PinValue.ON)
                            await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveOrigin, null);
                        if (Axis._02.ORG_OK != PinValue.ON)
                            await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveOrigin, null);
                        if (Axis._03.ORG_OK != PinValue.ON)
                            await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveOrigin, null);
                        if (Axis._04.ORG_OK != PinValue.ON)
                            await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveOrigin, null);
                        ORGStep = 6;
                        continue;
                    }
                    if (ORGStep == 6)
                    {
                        if (/*Axis._01.ORG == PinValue.ON &&*/ Axis._01.ORG_OK == PinValue.ON &&
                           /*Axis._02.ORG == PinValue.ON &&*/ Axis._02.ORG_OK == PinValue.ON &&
                           /*Axis._03.ORG == PinValue.ON &&*/ Axis._03.ORG_OK == PinValue.ON &&
                           /*Axis._04.ORG == PinValue.ON)// &&*/ Axis._04.ORG_OK == PinValue.ON)
                        {
                            ORGStep = 7;
                            //IsOrigin = true;
                            Origin = true;
                            //PD_TVOC_STEP = 0; PDStep = 0; PD_Buf_Step = 0; BUFFER_TVOC_STEP = 0;  BufTVOC_Step = 0; TVOCTrans_Step = 0;
                            //NFC_BUFF_STEP = 0; NFC_Buf_Step = 0; Buf_Leak_Step = 0; CV_NFC_Step = 0; BUFF_LEAK_STEP = 0; LeakFinalStep = 0;
                            //PD PP
                            PDStep = 0; PD_Buf_Step = 0;
                            Buff_SIM_Step = 0; SIM_TRANS_CV_STEP = 0; SIM_NG_Step = 0;
                            NFC_Buf_Step = 0; Buf_Leak_Step = 0; CV_NFC_Step = 0; LeakFinalStep = 0;
                            Dispatcher.Invoke(() => StartButton.IsEnabled = true);
                            return;
                        }
                    }
                    if (IN.ESTOP_BUTTON_01.PinValue == PinValue.OFF || IN.ESTOP_BUTTON_02.PinValue == PinValue.OFF)
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveStop, null);
                await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveStop, null);
                await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveStop, null);
                await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveStop, null);
                string s = ex.Message;
            }
        }

        string _PDProcessingStep;

        public string PDProcessingStep
        {
            get { return _PDProcessingStep; }
            set
            {
                if (_PDProcessingStep != value)
                {
                    _PDProcessingStep = value; NotifyPropertyChanged("PDProcessingStep");
                }
            }
        }
        private async Task PDLoopExecution(CancellationToken token)
        {
            try
            {
                PDLoopExcuting = true;
                while (true)
                {
                    Thread.Sleep(33);
                    token.ThrowIfCancellationRequested();
                    if (PDRUN)
                    {
                        #region INPUT CV TO PD JIG
                        //**************************INPUT_CV TO PD_JIG STEP**************************//
                        if (AXIS_01_STEP == 0)
                        {
                            PDProcessingStep = $"CV_PD STEP: {PDStep} - WAIT";
                            if (PDStep == 0 && PDBypass == false)
                            {
                                ReadyPD = null;
                                foreach (UCT100 UCT in UCT100s)
                                {
                                    foreach (PDJig PD in UCT.PDs)
                                    {
                                        if (PD.Jig.IsJigEnable == true && !PD.Jig.IsSetInJig &&
                                            PD.PackingPin.PinValue != PinValue.ON && !UCT.ResetFlag &&
                                            PD.Jig.TestResult == TestResult.READY && PD.Jig.JigState != UCTStatus.NORESP)
                                        {
                                            ReadyPD = PD;
                                            break;
                                        }
                                    }
                                    if (ReadyPD != null) break;
                                }
                                if (ReadyPD != null)
                                    PDStep = 1;
                            }
                            if (PDStep == 1)
                            {
                                PDProcessingStep = $"CV_PD STEP: {PDStep} - WAIT FOR INPUT CV";

                                if ((Input_CV_Delay == 0 || InputCVTimer > 0) && IN.LOADING_01_UNCLAMP_SENSOR.PinValue == ON)//IN.INPUT_CV_SENSOR_02.PinValue == PinValue.OFF)
                                {
                                    PDStep = 2;
                                }
                                else if ((IN.LOADING_01_CLAMP_SENSOR.PinValue == OFF && IN.LOADING_01_UNCLAMP_SENSOR.PinValue == OFF) && TestedPD != null)
                                    PDStep = 5;
                                else if (IN.LOADING_01_CLAMP_SENSOR.PinValue == ON)
                                {
                                    PDProcessingStep = $"CV_PD STEP: {PDStep} - UNCLAMP {IN.LOADING_01_CLAMP_SENSOR.GPIOLabel} OFF";
                                    await OUT.LOADING_01_CLAMP_SOL.RST();
                                    await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                    await Task.Delay(500);
                                }
                            }
                            if (PDStep == 0 || PDStep == 1) { PDStep = 0; AXIS_01_STEP = 1; }
                            //Move to Loading Pos
                            if (PDStep == 2)
                            {
                                PDProcessingStep = $"CV_PD STEP: {PDStep} - GO TO INPUT CV {Axis._01.CurrentPos}/{AXIS_01_LOADING_POS.JigPos}";
                                if (await MoveAbs(Axis._01, AXIS_01_LOADING_POS.JigPos, Axis01Velocity))
                                {
                                    PDStep = 3;
                                    if (Input_CV_Delay > 0)
                                    {
                                        Axis01Delay = 50;
                                    }
                                }
                            }
                            //Lift 01 Down
                            if (PDStep == 3)
                            {
                                PDProcessingStep = $"CV_PD STEP: {PDStep} - LIFT#1 DOWN ({IN.LOADING_01_LIFT_DOWN_SENSOR.GPIOLabel} ON)";
                                if (IN.LOADING_01_RETURN_SENSOR.PinValue == ON && Input_CV_Delay == 0)
                                {
                                    await OUT.LOADING_01_LIFT_SOL.SET();

                                    Axis01Delay = 0;
                                }
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    PDStep = 4;
                                }
                                if (Input_CV_Delay > 0 && Axis01Delay == 0)
                                {
                                    PDStep = 0;
                                }
                            }
                            //Clamp
                            if (PDStep == 4 && Axis01Delay == 0)
                            {
                                PDProcessingStep = $"CV_PD STEP: {PDStep} - CLAMP#1 ON ({IN.LOADING_01_UNCLAMP_SENSOR.GPIOLabel} OFF)";
                                await OUT.LOADING_01_CLAMP_SOL.SET();
                                await OUT.LOADING_01_UNCLAMP_SOL.RST();
                                if (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    PDStep = 5;
                                }
                            }
                            if (PDStep == 5)
                            {
                                PDProcessingStep = $"CV_PD STEP: {PDStep} - LIFT#1 UP ({IN.LOADING_01_LIFT_UP_SENSOR.GPIOLabel} ON)";
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    InputCVTimer = 0;
                                    Input_CV_Delay = 5;
                                    if (IN.LOADING_01_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        if (ReadyPD == null)
                                        {
                                            foreach (UCT100 UCT in UCT100s)
                                            {
                                                foreach (PDJig PD in UCT.PDs)
                                                {
                                                    if (PD.Jig.IsJigEnable == true && !PD.Jig.IsSetInJig &&
                                                        PD.PackingPin.PinValue != PinValue.ON && !UCT.ResetFlag &&
                                                        PD.Jig.TestResult == TestResult.READY && PD.Jig.JigState != UCTStatus.NORESP)
                                                    {
                                                        ReadyPD = PD;
                                                        break;
                                                    }
                                                }
                                                if (ReadyPD != null) break;
                                            }
                                        }
                                        else
                                            PDStep = 6;
                                    }
                                    else//If CLAMP 03 SENSOR IS ON OR UNCLAMP_SENSOR IS ON
                                    {
                                        PDProcessingStep = $"CV_PD STEP: {PD_Buf_Step} - CLAMP ERROR {IN.LOADING_01_CLAMP_SENSOR.GPIOLabel} OFF";
                                        if (MessageBox.Show("LOADING 01 CLAMP ERROR, Reset?", "Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                        {
                                            await OUT.LOADING_01_CLAMP_SOL.RST();
                                            await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                            PDStep = 0;
                                        }
                                        else PDRUN = false;
                                    }
                                }
                            }
                            if (PDStep == 6)
                            {
                                PDProcessingStep = $"CV_PD STEP: {PDStep} - GO TO READY PD {ReadyPD.Jig.JigDescription} {Axis._01.CurrentPos}/{ReadyPD.Jig.JigPos}";
                                if (ReadyPD != null)
                                    if (await MoveAbs(Axis._01, ReadyPD.Jig.JigPos, Axis01Velocity))
                                        PDStep = 7;
                            }
                            if (PDStep == 7)
                            {
                                //Lift Down
                                PDProcessingStep = $"CV_PD STEP: {PDStep} - LIFT#1 DOWN {IN.LOADING_01_LIFT_DOWN_SENSOR.GPIOLabel} ON";
                                await OUT.LOADING_01_LIFT_SOL.SET();
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    PDStep = 8;
                                }
                            }
                            if (PDStep == 8)
                            {
                                //Unclamp
                                PDProcessingStep = $"CV_PD STEP: {PDStep} - UNCLAMP {IN.LOADING_01_UNCLAMP_SENSOR.GPIOLabel} ON";
                                await OUT.LOADING_01_CLAMP_SOL.RST();
                                await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                if (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                {
                                    //Set in Jig
                                    ReadyPD.Jig.IsSetInJig = true;
                                    PDStep = 9;
                                }
                            }
                            if (PDStep == 9)
                            {   //Lift Up
                                PDProcessingStep = $"CV_PD STEP: {PDStep} - LIFT#1 UP {IN.LOADING_01_LIFT_UP_SENSOR.GPIOLabel} ON";
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                    PDStep = 10;
                            }
                            if (PDStep == 10)
                            {
                                ReadyPD.Jig.TestResult = TestResult.TEST;
                                TotalPD++;
                                await ReadyPD.PackingPin.SET();
                                PDStep = 0;
                                AXIS_01_STEP = 1;
                            }
                        }
                        #endregion

                        #region PD TO BUFFER(OK) / TO NGCV(NG) STEP
                        if (AXIS_01_STEP == 1)
                        {
                            //Loop for TestJig
                            if (PD_Buf_Step == 0)
                                // && AXIS_01_BUFFER.IsSetInJig == false && Axis._02.CurrentPos <= AXIS_02_TVOC.JigPos)
                                if (PDBypass == false && IN.LOADING_01_UNCLAMP_SENSOR.PinValue == ON)
                                {
                                    SIMJig ReadySIM = null;
                                    foreach (SIMJig SIM in SIMs)
                                    {
                                        if (!SIM.Jig.IsSetInJig && SIM.Jig.TestResult == TestResult.READY && SIM.Jig.IsJigEnable)
                                        {
                                            ReadySIM = SIM;
                                            break;
                                        }
                                    }
                                    TestedPDResult = TestResult.READY;
                                    TestedPD = null;
                                    foreach (PDJig PD in PDs)
                                    {
                                        if ((PD.Jig.TestResult == TestResult.PASS && (!AXIS_01_BUFFER.IsSetInJig || ReadySIM != null)) ||
                                            (PD.Jig.TestResult == TestResult.FAIL && PD.Jig.IsRetested == true))
                                        {
                                            TestedPD = PD;
                                            TestedPDResult = PD.Jig.TestResult;
                                            break;
                                        }
                                    }
                                    if (TestedPD != null)
                                        PD_Buf_Step = 1;
                                }
                                else if (PDBypass == true && IN.LOADING_01_UNCLAMP_SENSOR.PinValue == ON)
                                {
                                    if (Input_CV_Delay == 0)
                                    {
                                        TestedPD = new PDJig() { Jig = AXIS_01_LOADING_POS };
                                        //TestedPD.Jig.TestResult = TestResult.PASS;
                                        TestedPDResult = TestResult.PASS;
                                        PD_Buf_Step = 1;
                                    }
                                }
                                else if (IN.LOADING_01_CLAMP_SENSOR.PinValue == OFF && IN.LOADING_01_UNCLAMP_SENSOR.PinValue == OFF
                                         && TestedPD != null)
                                {
                                    PD_Buf_Step = 5;
                                }
                                else if (IN.LOADING_01_CLAMP_SENSOR.PinValue == ON || TestedPD == null)
                                {
                                    await OUT.LOADING_01_CLAMP_SOL.RST();
                                    await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                    await Task.Delay(500);
                                    AXIS_01_STEP = 0;
                                }
                            if (PD_Buf_Step == 1)
                            {
                                //PD_TVOCMove = false;
                                //if (TestedPD.Jig.TestResult == TestResult.PASS && !TVOC.Jig.IsSetInJig && !AXIS_01_BUFFER.IsSetInJig
                                //    && (Axis._02.CurrentPos <= 0 || Axis._02.CurrentPos == AXIS_02_TRANSFER_CV_POS.JigPos))
                                //{
                                //    PD_TVOCMove = true;
                                //    PD_Buf_Step = 2;
                                //}
                                //else 
                                if (TestedPDResult == TestResult.PASS ||
                                   (TestedPDResult == TestResult.FAIL && TestedPD.Jig.IsRetested == true)                                                                                                                                                                                                                                                                                                                   /*&& (ReadyPD == null || Input_CV_Debounce > 1) && !AXIS_01_BUFFER.IsSetInJig
                                                                                                                                                                                                                                                                                                                                 && Axis._02.CurrentPos <= AXIS_02_TVOC.JigPos)
                                    //{
                                    //    if (!AXIS_01_BUFFER.IsSetInJig /*|| TVOCBypass*/) //&& Axis._02.CurrentPos <= AXIS_02_TVOC.JigPos)
                                    //    {
                                    //        PD_Buf_Step = 2;
                                    //    }
                                    //    else if (ReadyPD == null || Input_CV_Debounce > 0)
                                    //    {
                                    //        PD_Buf_Step = 2;
                                    //    }
                                    //}
                                    PD_Buf_Step = 2;
                            }
                            if (PD_Buf_Step == 0 || PD_Buf_Step == 1)
                            {
                                PD_Buf_Step = 0;
                                AXIS_01_STEP = 2;
                            }
                            //RUN-------------------------------------------------------------
                            if (PD_Buf_Step == 2)
                            {//Move to Tested PD jig
                                PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - GO TO PD {TestedPD.Jig.JigDescription} {Axis._01.CurrentPos}/{TestedPD.Jig.JigPos}";
                                if (await MoveAbs(Axis._01, TestedPD.Jig.JigPos, Axis01Velocity))
                                    PD_Buf_Step = 3;
                            }
                            if (PD_Buf_Step == 3)
                            {//Lift 01 Down
                                PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - LIFT#1 DOWN {IN.LOADING_01_LIFT_DOWN_SENSOR.GPIOLabel} ON";
                                await OUT.LOADING_01_LIFT_SOL.SET();
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (TestedPD.PackingPin != null)
                                    {
                                        await TestedPD.PackingPin.RST();
                                        Axis01Delay = 5;
                                    }
                                    PD_Buf_Step = 4;
                                }
                            }
                            if (PD_Buf_Step == 4 && Axis01Delay == 0)
                            {//Clamp
                                PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - CLAMP#1 ON {IN.LOADING_01_UNCLAMP_SENSOR.GPIOLabel} OFF";
                                await OUT.LOADING_01_CLAMP_SOL.SET();
                                await OUT.LOADING_01_UNCLAMP_SOL.RST();
                                if (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    PD_Buf_Step = 5;
                                }
                            }
                            if (PD_Buf_Step == 5)
                            {
                                PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - LIFT#1 UP {IN.LOADING_01_LIFT_UP_SENSOR.GPIOLabel} ON";
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (IN.LOADING_01_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        PD_Buf_Step = 6;
                                        TestedPD.Jig.IsSetInJig = false;
                                        TestedPD.Jig.TestResult = TestResult.READY;
                                        if (!UCT8Mode)
                                            ResetPower(TestedPD);
                                        else if (true && TestedPDResult == TestResult.FAIL)//Uncomment to reset power when test fail
                                            ResetPower(TestedPD);
                                        //TestedPD.Jig.TestResult = TestResult.READY;
                                    }
                                    else//If CLAMP 03 SENSOR IS ON OR UNCLAMP_SENSOR IS ON
                                    {
                                        PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - CLAMP ERROR {IN.LOADING_01_CLAMP_SENSOR.GPIOLabel} OFF";
                                        if (MessageBox.Show("LOADING 01 CLAMP ERROR, Reset?", "Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                        {
                                            await OUT.LOADING_01_CLAMP_SOL.RST();
                                            await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                            TestedPD.Jig.TestResult = TestResult.READY;
                                            PD_Buf_Step = 0;
                                        }
                                        else PDRUN = false;
                                    }
                                }
                            }
                            if (PD_Buf_Step == 6)
                            {
                                if (TestedPDResult == TestResult.PASS)
                                {
                                    if (AXIS_01_BUFFER.IsSetInJig) //|| Axis._02.CurrentPos >= AXIS_02_TVOC.JigPos)
                                    {
                                        PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - GO TO PD7 {Axis._01.CurrentPos}/{TestedPD.Jig.JigPos}";
                                        await MoveAbs(Axis._01, PDs[6].Jig.JigPos, Axis01Velocity);
                                    }
                                    else
                                    if (Axis._02.CurrentPos <= SIMs[0].Jig.JigPos || (Buff_SIM_Step >= 6 && Axis._02.Motioning == PinValue.ON))
                                    {
                                        PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - GO TO BUFF {Axis._01.CurrentPos}/{TestedPD.Jig.JigPos}";
                                        if (await MoveAbs(Axis._01, AXIS_01_BUFFER.JigPos, Axis01Velocity))
                                        {
                                            PD_Buf_Step = 7;
                                            TestedPD.Jig.OKCounter++;
                                        }//Increase OK Counter
                                    }
                                }
                                else if (TestedPDResult == TestResult.FAIL)
                                {
                                    PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - GO TO NG_CV {Axis._01.CurrentPos}/{TestedPD.Jig.JigPos}";
                                    if (await MoveAbs(Axis._01, AXIS_01_NG_POS.JigPos, Axis01Velocity))
                                    {
                                        TestedPD.Jig.NGCounter++; //Increase NG Counter
                                        PD_Buf_Step = 7;
                                    }
                                }
                            }
                            if (PD_Buf_Step == 7)
                            {
                                //Lift Down
                                if (TestedPDResult == TestResult.PASS)
                                {
                                    PDProcessingStep =
                                        $"PD_BUFF STEP: {PD_Buf_Step} - LIFT#1 DOWN {IN.LOADING_01_LIFT_DOWN_SENSOR.GPIOLabel} ON";
                                    //if (AXIS_01_BUFFER.IsJigEnable)
                                    await OUT.LOADING_01_LIFT_SOL.SET();
                                    //else
                                    //if (IN.LOADING_01_TURN_SENSOR.PinValue == PinValue.ON)
                                    //    await OUT.LOADING_01_LIFT_SOL.SET();
                                }
                                else if (TestedPDResult == TestResult.FAIL)
                                {
                                    if (IN.NG_CV_SENSOR_02.PinValue == PinValue.OFF || IN.NG_CV_SENSOR_03.PinValue == PinValue.OFF)
                                        Axis01Delay = 10;
                                    if (IN.NG_CV_SENSOR_02.PinValue == PinValue.ON && IN.NG_CV_SENSOR_03.PinValue == PinValue.ON && Axis01Delay == 0)
                                        await OUT.LOADING_01_LIFT_SOL.SET();
                                }
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (TestedPDResult == TestResult.PASS)
                                    {
                                        AXIS_01_BUFFER.IsSetInJig = true;
                                        await OUT.BUFFER_PACK.SET();
                                        PD_Buf_Step = 8;
                                    }
                                    else if (TestedPDResult == TestResult.FAIL)
                                    {//Write To log
                                     //WriteToLog(DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy: ") + TestedPD.Jig.JigDescription);
                                     //Axis01Delay = 0;
                                     //PDNG.LogFile(App.PD_NG_COM, TestedPD.Jig, TestResult.FAIL);
                                        Printer.Print("", TestedPD.Jig.JigDescription);
                                        MainWindow.Root.WriteToLog(string.Format(": {0} - {1} FAIL", "", TestedPD.Jig.JigDescription));
                                        TotalFail++;
                                        PD_Buf_Step = 8;
                                    }
                                }
                            }
                            if (PD_Buf_Step == 8)
                            {
                                //Unclamp
                                PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - LIFT#1 UNCLAMP {IN.LOADING_01_UNCLAMP_SENSOR.GPIOLabel} ON";
                                await OUT.LOADING_01_CLAMP_SOL.RST();
                                await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                if (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                {
                                    PD_Buf_Step = 9;
                                }
                            }
                            if (PD_Buf_Step == 9)
                            {
                                //Lift Up
                                PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - LIFT#1 GO UP {IN.LOADING_01_LIFT_UP_SENSOR.GPIOLabel} ON";
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    PD_Buf_Step = 10;
                                    //if (TestedPDResult == TestResult.FAIL)
                                    //    NGCVRun = 200;
                                    TestedPD.Jig.TestResult = TestResult.READY;
                                    TestedPD.Jig.IsRetested = false;
                                }
                            }
                            if (PD_Buf_Step == 10)
                            {
                                MovedPDJig = null;
                                MovedPDJig = movedPDJig();
                                PD_Buf_Step = 11;
                            }
                            if (PD_Buf_Step == 11)
                            {
                                //await OUT.LOADING_01_TURN_SOL.RST();
                                //if ((TestedPDResult == TestResult.PASS || TestedPDResult == TestResult.FAIL) && (Input_CV_Debounce == 0))
                                //{
                                //    if (await MoveAbs(Axis._01, AXIS_01_LOADING_POS.JigPos, Axis01Velocity))
                                //        PD_Buf_Step = 12;
                                //}
                                //else 
                                if (TestedPDResult == TestResult.FAIL)
                                {
                                    NGCVRun = 200;
                                    PD_Buf_Step = 0;
                                    AXIS_01_STEP = 1;
                                }
                                else if (MovedPDJig != null)
                                {
                                    PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - GO TO {MovedPDJig.Jig.JigDescription} {Axis._01.CurrentPos}/{TestedPD.Jig.JigPos}";
                                    if (await MoveAbs(Axis._01, MovedPDJig.Jig.JigPos, Axis01Velocity))
                                        PD_Buf_Step = 12;
                                }
                                else
                                {
                                    PDProcessingStep = $"PD_BUFF STEP: {PD_Buf_Step} - GO TO {PDs[6].Jig.JigDescription} {Axis._01.CurrentPos}/{TestedPD.Jig.JigPos}";
                                    if (await MoveAbs(Axis._01, PDs[6].Jig.JigPos, Axis01Velocity))
                                        PD_Buf_Step = 12;
                                }
                                //if (Axis._01.CurrentPos <= AXIS_01_BUFFER.JigPos && !AXIS_01_BUFFER.IsJigEnable)
                                //    if (TestedPDResult == TestResult.PASS)
                                //        await TVOCStart();
                            }
                            if (PD_Buf_Step == 12)
                            {
                                PD_Buf_Step = 0;
                                if (TestedPDResult == TestResult.PASS)
                                    AXIS_01_STEP = 3;
                                else AXIS_01_STEP = 0;
                            }
                        }
                        #endregion

                        #region PD RETEST STEP
                        if (AXIS_01_STEP == 2)
                        {
                            if (PDRetestStep == 0)
                            {
                                ReadyPD = null;
                                foreach (UCT100 UCT in UCT100s)
                                {
                                    foreach (PDJig PD in UCT.PDs)
                                    {
                                        if (PD.Jig.IsJigEnable == true && !PD.Jig.IsSetInJig &&
                                            PD.PackingPin.PinValue != PinValue.ON && !UCT.ResetFlag &&
                                            PD.Jig.TestResult == TestResult.READY && PD.Jig.JigState != UCTStatus.NORESP)
                                        {
                                            ReadyPD = PD;
                                            break;
                                        }
                                    }
                                    if (ReadyPD != null) break;
                                }
                                if (ReadyPD != null)
                                    PDRetestStep = 1;
                                else
                                if (ReadyPD == null)
                                {
                                    PDJig FailedPD = null;
                                    foreach (PDJig PD in PDs)
                                    {
                                        if (PD.Jig.TestResult == TestResult.FAIL && PD.Jig.IsRetested == true)
                                        {
                                            FailedPD = PD;
                                            break;
                                        }
                                    }
                                    if (FailedPD == null)
                                    {
                                        foreach (PDJig PD in PDs)
                                        {
                                            if (PD.Jig.TestResult == TestResult.FAIL && PD.Jig.IsRetested == false)
                                            {
                                                //FailPD = PD;
                                                PD.Jig.IsRetested = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (PDRetestStep == 1 && IN.LOADING_01_UNCLAMP_SENSOR.PinValue == ON)
                            {
                                {
                                    TestedPD = null;
                                    foreach (PDJig PD in PDs) //Loop for FAIL Set to Retest
                                    {
                                        if (PD.Jig.TestResult == TestResult.FAIL && PD.Jig.IsRetested == false && ReadyPD != null)
                                        {
                                            TestedPD = PD;
                                            break;
                                        }
                                    }
                                    if (TestedPD != null)
                                    {
                                        //PDRetest = true;
                                        PDRetestStep = 2;
                                    }
                                }
                            }
                            else if (PDRetestStep == 1 && IN.LOADING_01_CLAMP_SENSOR.PinValue == OFF && IN.LOADING_01_UNCLAMP_SENSOR.PinValue == OFF)
                            {
                                PDRetestStep = 6;
                            }
                            else if (PDRetestStep == 1 && IN.LOADING_01_CLAMP_SENSOR.PinValue == ON)
                            {
                                await OUT.LOADING_01_CLAMP_SOL.RST();
                                await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                await Task.Delay(500);
                                AXIS_01_STEP = 0;
                            }

                            if (PDRetestStep == 0 || PDRetestStep == 1) { PDRetestStep = 0; AXIS_01_STEP = 3; }
                            if (PDRetestStep == 2)
                            {
                                if (await MoveAbs(Axis._01, TestedPD.Jig.JigPos, Axis01Velocity))
                                {
                                    PDRetestStep = 3;
                                }
                            }
                            if (PDRetestStep == 3)
                            {
                                //await OUT.LOADING_01_LIFT_SOL.SET();
                                //if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    PDRetestStep = 4;
                                }
                            }
                            if (PDRetestStep == 4)
                            {
                                await OUT.LOADING_01_LIFT_SOL.SET();
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    PDRetestStep = 5;
                                    await TestedPD.PackingPin.RST();
                                    Axis01Delay = 5;
                                }
                            }
                            if (PDRetestStep == 5 && Axis01Delay == 0)
                            {
                                await OUT.LOADING_01_CLAMP_SOL.SET();
                                await OUT.LOADING_01_UNCLAMP_SOL.RST();
                                if (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    PDRetestStep = 6;
                                }
                            }
                            if (PDRetestStep == 6)
                            {
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (IN.LOADING_01_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        TestedPD.Jig.IsSetInJig = false;
                                        ResetPower(TestedPD);
                                        PDRetestStep = 7;
                                    }
                                    else//If CLAMP 03 SENSOR IS ON OR UNCLAMP_SENSOR IS ON
                                    {
                                        if (MessageBox.Show("LOADING 01 CLAMP ERROR, Reset?", "Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                        {
                                            await OUT.LOADING_01_CLAMP_SOL.RST();
                                            await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                            PDRetestStep = 0;
                                            AXIS_01_STEP = 0;
                                            TestedPD.Jig.TestResult = TestResult.READY;
                                        }
                                        else PDRUN = false;
                                    }
                                }
                            }
                            if (PDRetestStep == 7)
                            {
                                if (await MoveAbs(Axis._01, ReadyPD.Jig.JigPos, Axis01Velocity))
                                    PDRetestStep = 8;
                            }
                            if (PDRetestStep == 8)
                            {
                                //Lift Down
                                await OUT.LOADING_01_LIFT_SOL.SET();
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    PDRetestStep = 9;
                                }
                            }
                            if (PDRetestStep == 9)
                            {
                                //Unclamp
                                await OUT.LOADING_01_CLAMP_SOL.RST();
                                await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                if (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                {
                                    //Set in Jig
                                    ReadyPD.Jig.IsSetInJig = true;
                                    PDRetestStep = 10;
                                }
                            }
                            if (PDRetestStep == 10)
                            {   //Lift Up
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    PDRetestStep = 11;
                                }
                            }
                            if (PDRetestStep == 11)
                            {
                                ReadyPD.Jig.IsRetested = true;
                                TestedPD.Jig.TestResult = TestResult.READY;
                                ReadyPD.Jig.TestResult = TestResult.RETEST;
                                await ReadyPD.PackingPin.SET();
                                PDRetestStep = 12;
                            }
                            if (PDRetestStep == 12)
                            {
                                MovedPDJig = null;
                                MovedPDJig = movedPDJig();
                                PDRetestStep = 13;
                            }
                            if (PDRetestStep == 13)
                            {
                                //if (Input_CV_Debounce == 0)
                                //{
                                //    if (await MoveAbs(Axis._01, AXIS_01_LOADING_POS.JigPos, Axis01Velocity))
                                //        PDRetestStep = 14;
                                //}
                                //else 
                                if (MovedPDJig != null)
                                {
                                    if (await MoveAbs(Axis._01, MovedPDJig.Jig.JigPos, Axis01Velocity))
                                    {
                                        PDRetestStep = 14;
                                    }
                                }
                                else PDRetestStep = 14;
                            }
                            if (PDRetestStep == 14)
                            {
                                PDRetestStep = 0;
                                AXIS_01_STEP = 0;
                            }
                        }
                        #endregion

                        #region SIM NG PROCESSING
                        if (AXIS_01_STEP == 3)
                        {
                            if (SIM_NG_Step == 0 && SIM_TRANS_CV_STEP == 0 && Buff_SIM_Step == 0)
                            {
                                TestedSIM = null;
                                foreach (SIMJig SIM in SIMs)
                                {
                                    if (SIM.Jig.TestResult == TestResult.FAIL && SIM.Jig.IsSetInJig/*&& Axis._02.CurrentPos <= AXIS_02_TRANSFER_CV_POS.JigPos*/)
                                    {
                                        SIMTestedResult = SIM.Jig.TestResult;
                                        TestedSIM = SIM;
                                        SIM_NG_Step = 1;
                                        break;
                                    }
                                }
                            }
                            if (SIM_NG_Step == 1 && IN.LOADING_01_UNCLAMP_SENSOR.PinValue == ON)
                            {
                                //PDRetest = true;
                                SIM_NG_Step = 2;
                            }
                            else if (SIM_NG_Step == 1 && IN.LOADING_01_CLAMP_SENSOR.PinValue == OFF && IN.LOADING_01_UNCLAMP_SENSOR.PinValue == OFF
                                     && TestedSIM != null)
                            {
                                SIM_NG_Step = 6;
                            }
                            else if (SIM_NG_Step == 1 && IN.LOADING_01_CLAMP_SENSOR.PinValue == ON)
                            {
                                await OUT.LOADING_01_CLAMP_SOL.RST();
                                await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                await Task.Delay(500);
                                AXIS_01_STEP = 0;
                            }

                            if (SIM_NG_Step == 0 || SIM_NG_Step == 1) { SIM_NG_Step = 0; AXIS_01_STEP = 0; }
                            if (SIM_NG_Step == 2)
                            {
                                //await OUT.LOADING_01_TURN_SOL.SET();
                                bool a = await MoveAbs(Axis._01, TestedSIM.Axis01SIM.JigPos, Axis01Velocity);
                                bool b = await MoveAbs(Axis._02, AXIS_02_TRANSFER_CV_POS.JigPos, Axis02Velocity);
                                if (a && b)
                                {
                                    SIM_NG_Step = 3;
                                }
                            }
                            if (SIM_NG_Step == 3)
                            {
                                //await OUT.LOADING_01_LIFT_SOL.SET();
                                //if (IN.LOADING_01_TURN_SENSOR.PinValue == PinValue.ON)
                                {
                                    TestedSIM.StopTestLoop();
                                    SIM_NG_Step = 4;
                                }
                            }
                            if (SIM_NG_Step == 4)
                            {
                                if (TestedSIM.ReleaseSensor.PinValue == PinValue.ON && TestedSIM.PackOutSensor.PinValue == PinValue.ON)
                                    await OUT.LOADING_01_LIFT_SOL.SET();
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    SIM_NG_Step = 5;
                                }
                            }
                            if (SIM_NG_Step == 5)
                            {
                                await OUT.LOADING_01_CLAMP_SOL.SET();
                                await OUT.LOADING_01_UNCLAMP_SOL.RST();
                                if (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    SIM_NG_Step = 6;
                                }
                            }
                            if (SIM_NG_Step == 6)
                            {
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (IN.LOADING_01_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        TestedSIM.Jig.IsSetInJig = false;
                                        TestedSIM.Jig.TestResult = TestResult.READY;
                                        SIM_NG_Step = 7;
                                    }
                                    else//If CLAMP 03 SENSOR IS ON OR UNCLAMP_SENSOR IS ON
                                    {
                                        if (MessageBox.Show("LOADING 01 CLAMP ERROR, Reset?", "Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                        {
                                            await OUT.LOADING_01_CLAMP_SOL.RST();
                                            await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                            SIM_NG_Step = 0;
                                            AXIS_01_STEP = 0;
                                            TestedSIM.Jig.TestResult = TestResult.READY;
                                        }
                                        else PDRUN = false;
                                    }
                                }
                            }
                            if (SIM_NG_Step == 7)
                            {
                                //await OUT.LOADING_01_TURN_SOL.RST();
                                if (await MoveAbs(Axis._01, AXIS_01_NG_POS.JigPos, Axis01Velocity))
                                    SIM_NG_Step = 8;
                            }
                            if (SIM_NG_Step == 8)
                            {
                                //Lift Down
                                if (IN.NG_CV_SENSOR_02.PinValue == PinValue.OFF || IN.NG_CV_SENSOR_03.PinValue == PinValue.OFF)
                                    Axis01Delay = 10;
                                if (IN.NG_CV_SENSOR_02.PinValue == PinValue.ON && IN.NG_CV_SENSOR_03.PinValue == PinValue.ON && Axis01Delay == 0)
                                    if (IN.LOADING_01_RETURN_SENSOR.PinValue == PinValue.ON)
                                        await OUT.LOADING_01_LIFT_SOL.SET();
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    SIM_NG_Step = 9;
                                }
                            }
                            if (SIM_NG_Step == 9)
                            {
                                //Unclamp
                                await OUT.LOADING_01_CLAMP_SOL.RST();
                                await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                if (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                {
                                    SIM_NG_Step = 10;
                                }
                            }
                            if (SIM_NG_Step == 10)
                            {   //Lift Up
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    SIM_NG_Step = 11;
                                    NGCVRun = 30;
                                }
                            }
                            if (SIM_NG_Step == 11)
                            {
                                TestedSIM.Jig.TestResult = TestResult.READY;
                                SIM_NG_Step = 12;
                            }
                            if (SIM_NG_Step == 12)
                            {
                                SIM_NG_Step = 13;
                            }
                            if (SIM_NG_Step == 13)
                            {
                                SIM_NG_Step = 14;
                            }
                            if (SIM_NG_Step == 14)
                            {
                                SIM_NG_Step = 0;
                                AXIS_01_STEP = 0;
                            }
                        }
                        #endregion

                        #region BUFFER TO SIM STEP
                        if (AXIS_02_STEP == 0)
                        {
                            if (Buff_SIM_Step == 0)
                            {
                                if (!TVOCBypass)
                                {
                                    ReadySIM = null;
                                    foreach (SIMJig SIM in SIMs)
                                    {
                                        if (SIM.Jig.TestResult == TestResult.READY && SIM.Jig.IsJigEnable && !SIM.Jig.IsSetInJig &&
                                            SIM.PackOutSensor.PinValue == PinValue.ON && SIM.ReleaseSensor.PinValue == PinValue.ON)
                                        {
                                            ReadySIM = SIM;

                                            break;
                                        }
                                    }
                                }
                                else if (TVOCBypass)
                                {
                                    if (AXIS_01_BUFFER.IsSetInJig &&
                                    ((Axis._01.CurrentPos <= PDs[7].Jig.JigPos && SIM_NG_Step == 0) ||
                                    ((PD_Buf_Step >= 11 && Axis._01.Motioning == PinValue.ON) || SIM_NG_Step >= 8))
                                    && IN.LOADING_02_UNCLAMP_SENSOR.PinValue == ON)
                                        Buff_SIM_Step = 1;
                                }
                                if (AXIS_01_BUFFER.IsSetInJig &&
                                    ((Axis._01.CurrentPos <= PDs[7].Jig.JigPos && SIM_NG_Step == 0) ||
                                    ((PD_Buf_Step >= 11 && Axis._01.Motioning == PinValue.ON) || SIM_NG_Step >= 8))
                                    && IN.LOADING_02_UNCLAMP_SENSOR.PinValue == ON)
                                {
                                    if (ReadySIM != null)
                                        Buff_SIM_Step = 1;
                                }
                                else if (IN.LOADING_02_CLAMP_SENSOR.PinValue == OFF && IN.LOADING_02_UNCLAMP_SENSOR.PinValue == OFF &&
                                         ReadySIM != null &&
                                         Axis._01.CurrentPos <= PDs[7].Jig.JigPos)
                                {
                                    Buff_SIM_Step = 4;
                                }
                                else if (IN.LOADING_02_CLAMP_SENSOR.PinValue == ON)
                                {
                                    Buff_SIM_Step = 0;
                                    await OUT.LOADING_02_CLAMP_SOL.RST();
                                    await OUT.LOADING_02_UNCLAMP_SOL.SET();
                                    await Task.Delay(500);
                                }
                            }
                            if (Buff_SIM_Step == 0) AXIS_02_STEP = 1;
                            //RUN
                            if (Buff_SIM_Step == 1)
                            {
                                await OUT.LOADING_02_TURN_SOL.RST();
                                if (await MoveAbs(Axis._02, AXIS_02_BUFFER.JigPos, Axis02Velocity))
                                {
                                    Buff_SIM_Step = 2;
                                }
                            }
                            if (Buff_SIM_Step == 2)
                            {
                                if (IN.LOADING_02_RETURN_SENSOR.PinValue == PinValue.ON)
                                    await OUT.LOADING_02_LIFT_SOL.SET();
                                if (IN.LOADING_02_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    Buff_SIM_Step = 3;
                                    await OUT.BUFFER_PACK.RST();
                                    Axis02Delay = 2;
                                }
                            }
                            //Clamp
                            if (Buff_SIM_Step == 3 && Axis02Delay == 0)
                            {
                                await OUT.LOADING_02_CLAMP_SOL.SET();
                                await OUT.LOADING_02_UNCLAMP_SOL.RST();
                                if (IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    Buff_SIM_Step = 4;
                                }
                            }
                            if (Buff_SIM_Step == 4)
                            {   //Lift 02 Up
                                await OUT.LOADING_02_LIFT_SOL.RST();
                                if (IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (IN.LOADING_02_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        Buff_SIM_Step = 5;
                                    }
                                    else//If CLAMP 02 SENSOR IS ON OR UNCLAMP_SENSOR IS ON
                                    {
                                        if (MessageBox.Show("LOADING 02 CLAMP ERROR, Reset?", "Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                        {
                                            await OUT.LOADING_02_CLAMP_SOL.RST();
                                            await OUT.LOADING_02_UNCLAMP_SOL.SET();
                                        }
                                        else PDRUN = false;
                                        if (await MoveAbs(Axis._02, AXIS_02_TRANSFER_CV_POS.JigPos, Axis02Velocity))
                                        {
                                            Buff_SIM_Step = 0;
                                            AXIS_01_BUFFER.IsSetInJig = false;
                                        }
                                    }
                                }
                            }
                            if (Buff_SIM_Step == 5)
                            {
                                if (IN.LOADING_02_RETURN_SENSOR.PinValue == PinValue.ON)
                                    Buff_SIM_Step = 6;
                            }
                            if (Buff_SIM_Step == 6)
                            {   //Move to TVOC
                                if (TVOCBypass)
                                {
                                    if (await MoveAbs(Axis._02, AXIS_02_TRANSFER_CV_POS.JigPos, Axis02Velocity))
                                    {
                                        AXIS_02_STEP = 1;
                                        SIM_TRANS_CV_STEP = 7;
                                        Buff_SIM_Step = 0;
                                        AXIS_01_BUFFER.IsSetInJig = false;
                                    }
                                }
                                else
                                {
                                    if (await MoveAbs(Axis._02, ReadySIM.Jig.JigPos, Axis02Velocity))
                                    {
                                        Buff_SIM_Step = 7;
                                        AXIS_01_BUFFER.IsSetInJig = false;
                                    }
                                }
                            }
                            if (Buff_SIM_Step == 7)
                            {  //Put down to TVOC
                                await OUT.LOADING_02_LIFT_SOL.SET();
                                if (IN.LOADING_02_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                    Buff_SIM_Step = 8;
                            }
                            if (Buff_SIM_Step == 8)
                            {//Unclamp
                                await OUT.LOADING_02_CLAMP_SOL.RST();
                                await OUT.LOADING_02_UNCLAMP_SOL.SET();
                                if (IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                {
                                    Buff_SIM_Step = 9;
                                    ReadySIM.Jig.IsSetInJig = true;
                                }
                            }
                            if (Buff_SIM_Step == 9)
                            {//Lift 02 Up
                                await OUT.LOADING_02_LIFT_SOL.RST();
                                if (IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                    Buff_SIM_Step = 10;
                            }
                            if (Buff_SIM_Step == 10)
                            {
                                ReadySIM.StartTestLoop();
                                Buff_SIM_Step = 11;
                            }
                            if (Buff_SIM_Step == 11)
                            {
                                ReadySIM.Jig.TestResult = TestResult.TEST;
                                Buff_SIM_Step = 12;
                            }
                            if (Buff_SIM_Step == 12)
                            {
                                if (ReadySIM == SIMs[0])
                                {
                                    if (await MoveAbs(Axis._02, SIMs[1].Jig.JigPos, Axis02Velocity))
                                    {
                                        Buff_SIM_Step = 13;
                                    }
                                }
                                else
                                    Buff_SIM_Step = 13;
                            }
                            if (Buff_SIM_Step == 13)
                            {
                                Buff_SIM_Step = 0;
                                AXIS_02_STEP = 1;
                            }
                        }
                        #endregion

                        #region SIM TO TRANS CV STEP
                        if (AXIS_02_STEP == 1)
                        {
                            if (SIM_TRANS_CV_STEP == 0)
                            {
                                if (Axis._01.CurrentPos <= AXIS_01_BUFFER.JigPos &&
                                    IN.LOADING_02_UNCLAMP_SENSOR.PinValue == ON && (SIM_NG_Step == 0 || SIM_NG_Step >= 7))
                                {
                                    foreach (SIMJig SIM in SIMs)
                                    {
                                        if (SIM.Jig.TestResult == TestResult.PASS && SIM.Jig.IsSetInJig == true /*&&
                                            SIM.ReleaseSensor.PinValue == PinValue.ON && SIM.PackOutSensor.PinValue == PinValue.ON*/)
                                        {
                                            TestedSIM = SIM;
                                            SIM_TRANS_CV_STEP = 1;
                                            break;
                                        }
                                    }
                                }
                                else if (IN.LOADING_02_CLAMP_SENSOR.PinValue == OFF &&
                                    IN.LOADING_02_UNCLAMP_SENSOR.PinValue == OFF
                                    && TestedSIM != null &&
                                    Axis._01.CurrentPos <= AXIS_01_BUFFER.JigPos)
                                {
                                    SIM_TRANS_CV_STEP = 4;
                                }
                                else if (IN.LOADING_02_CLAMP_SENSOR.PinValue == ON || TestedSIM == null)
                                {
                                    Buff_SIM_Step = 0;
                                    await OUT.LOADING_02_CLAMP_SOL.RST();
                                    await OUT.LOADING_02_UNCLAMP_SOL.SET();
                                    await Task.Delay(500);
                                }
                            }
                            if (SIM_TRANS_CV_STEP == 0) AXIS_02_STEP = 0;
                            //RUN
                            if (SIM_TRANS_CV_STEP == 1)
                            {
                                //if (TVOC.IsTurnEnable)
                                //    await OUT.LOADING_02_TURN_SOL.SET();
                                SIM_TRANS_CV_STEP = 2;
                            }
                            if (SIM_TRANS_CV_STEP == 2)
                            {//Move To TVOC
                                if (await MoveAbs(Axis._02, TestedSIM.Jig.JigPos, Axis02Velocity))
                                //if (IN.LOADING_02_TURN_SENSOR.PinValue == PinValue.ON)
                                {
                                    TestedSIM.StopTestLoop();
                                    SIM_TRANS_CV_STEP = 3;
                                }
                            }
                            if (SIM_TRANS_CV_STEP == 3)
                            {//Lift 02 DOWN
                                if (TestedSIM.ReleaseSensor.PinValue == PinValue.ON && TestedSIM.ReleaseSensor.PinValue == PinValue.ON)
                                    await OUT.LOADING_02_LIFT_SOL.SET();
                                if (IN.LOADING_02_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    SIM_TRANS_CV_STEP = 4;
                                }
                            }
                            if (SIM_TRANS_CV_STEP == 4)
                            {//Clamp
                                await OUT.LOADING_02_CLAMP_SOL.SET();
                                await OUT.LOADING_02_UNCLAMP_SOL.RST();
                                if (IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    SIM_TRANS_CV_STEP = 5;
                                }
                            }
                            if (SIM_TRANS_CV_STEP == 5)
                            {//Lift 02 DOWN
                                await OUT.LOADING_02_LIFT_SOL.RST();
                                if (IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (IN.LOADING_02_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        SIM_TRANS_CV_STEP = 6;
                                        TestedSIM.Jig.TestResult = TestResult.READY;
                                        TestedSIM.Jig.IsSetInJig = false;
                                    }
                                    else//If CLAMP 02 SENSOR IS ON OR UNCLAMP_SENSOR IS ON
                                    {
                                        if (MessageBox.Show("LOADING 02 CLAMP ERROR, Reset?", "Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                        {
                                            await OUT.LOADING_02_CLAMP_SOL.RST();
                                            await OUT.LOADING_02_UNCLAMP_SOL.SET();
                                            //await MoveAbs(Axis._02, AXIS_02_TRANSFER_CV_POS.JigPos, Axis02Velocity);
                                            TestedSIM.Jig.TestResult = TestResult.READY;
                                            SIM_TRANS_CV_STEP = 0;
                                        }
                                        else PDRUN = false;
                                    }
                                }
                            }
                            if (SIM_TRANS_CV_STEP == 6)
                            {
                                //await OUT.LOADING_02_TURN_SOL.RST();
                                if (await MoveAbs(Axis._02, AXIS_02_TRANSFER_CV_POS.JigPos, Axis02Velocity))
                                    SIM_TRANS_CV_STEP = 7;
                            }
                            if (SIM_TRANS_CV_STEP == 7)
                            {//RETURN
                                //if (IN.LOADING_02_RETURN_SENSOR.PinValue == PinValue.ON)
                                {
                                    SIM_TRANS_CV_STEP = 8;
                                }
                            }
                            if (SIM_TRANS_CV_STEP == 8)
                            {//Wait for TRANS CV SENSOR
                                if (IN.TRANSFER_CV_SENSOR_BEGIN.PinValue != PinValue.ON)
                                    Axis02Delay = 5;
                                if (IN.TRANSFER_CV_SENSOR_BEGIN.PinValue == PinValue.ON && Axis02Delay == 0)
                                    await OUT.LOADING_02_LIFT_SOL.SET();
                                if (IN.LOADING_02_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    Axis02Delay = 0;
                                    SIM_TRANS_CV_STEP = 9;
                                    TransCVRun = 300;
                                }
                            }
                            if (SIM_TRANS_CV_STEP == 9)
                            {
                                await OUT.LOADING_02_UNCLAMP_SOL.SET();
                                await OUT.LOADING_02_CLAMP_SOL.RST();
                                if (IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                    SIM_TRANS_CV_STEP = 10;
                            }
                            if (SIM_TRANS_CV_STEP == 10)
                            {
                                await OUT.LOADING_02_LIFT_SOL.RST();
                                if (IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    SIM_TRANS_CV_STEP = 0;
                                    AXIS_02_STEP = 0;
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveStop, null);
                        await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveStop, null);
                    }
                }
            }
            catch
            {
                await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.MoveStop, null);
                await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.MoveStop, null);
                PDLoopExcuting = false;
            }
        }

        private async void ResetPower(PDJig pDJig)
        {
            if (!UCT8Mode)
            {
                bool CanReset = true;
                UCT100 ResetUCT = null;
                foreach (UCT100 UCT in UCT100s)
                {
                    if (UCT.ResetFlag)
                    {
                        foreach (PDJig PD in UCT.PDs)
                        {
                            if (pDJig == PD)
                            {
                                ResetUCT = UCT;
                                break;
                            }
                        }
                        if (ResetUCT != null)
                            break;
                    }
                }
                if (ResetUCT != null)
                {
                    foreach (PDJig PD in ResetUCT.PDs)
                    {
                        if ((PD.Jig.TestResult == TestResult.TEST || PD.Jig.TestResult == TestResult.RETEST) ||
                            ((PD.Jig.TestResult == TestResult.PASS || PD.Jig.TestResult == TestResult.FAIL) && PD.Jig.IsSetInJig))
                        {
                            CanReset = false;
                        }
                    }
                    if (CanReset)
                    {
                        ResetUCT.ResetFlag = false;
                        await ResetUCT.PowerPin.SET();
                        await Task.Delay(3000);
                        await ResetUCT.PowerPin.RST();
                    }
                }
            }
            else
            {
                await pDJig.PowerPin.SET();
                await Task.Delay(3000);
                await pDJig.PowerPin.RST();
            }
        }

        private void PDLoop()
        {
            PDLoopCancelationTokenSource = new CancellationTokenSource();
            Task.Run(async () => await PDLoopExecution(PDLoopCancelationTokenSource.Token));
        }
        PDJig ReadyPD = null, MovedPDJig;
        PDJig TestedPD = null;
        TestResult TestedPDResult = TestResult.TEST;
        NFCJig ReadyNFC, TestedNFC;
        NFCJig MoveNFC;
        TestResult NFCTestedResult = TestResult.READY;
        LeakJig ReadyLeak, TestedLeak;
        LeakJig MoveLeak;
        TestResult LeakTestedResult = TestResult.READY;
        SIMJig ReadySIM, TestedSIM;

        TestResult SIMTestedResult = TestResult.READY;
        List<double> Times = new List<double>();
        private async Task NFCLoopExecution(CancellationToken token)
        {
            try
            {
                NFCLoopExcuting = true;
                while (true)
                {
                    Thread.Sleep(50);
                    token.ThrowIfCancellationRequested();
                    if (NFC_LEAK_RUN)
                    {
                        if (AXIS_03_STEP == 0)
                        {
                            if (CV_NFC_Step == 0)
                            {
                                if (!NFCBypass)
                                {
                                    ReadyNFC = null;
                                    foreach (NFCJig NFC in NFCs)
                                    {
                                        if (NFC.Jig.IsJigEnable == true && !NFC.Jig.IsSetInJig &&
                                            NFC.PackingPin.PinValue == PinValue.OFF &&
                                            NFC.Jig.TestResult == TestResult.READY)
                                        {
                                            ReadyNFC = NFC;
                                            break;
                                        }
                                    }
                                    if (IN.LOADING_03_UNCLAMP_SENSOR.PinValue == ON)
                                    {
                                        if (ReadyNFC != null && Trans_CV_Debounce == 0)
                                            CV_NFC_Step = 1;
                                    }
                                    else if (IN.LOADING_03_UNCLAMP_SENSOR.PinValue == OFF &&
                                             IN.LOADING_03_CLAMP_SENSOR.PinValue == OFF)
                                    {
                                        CV_NFC_Step = 4;
                                    }
                                    else if (IN.LOADING_03_CLAMP_SENSOR.PinValue == ON)
                                    {
                                        await OUT.LOADING_03_CLAMP_SOL.RST();
                                        await OUT.LOADING_03_UNCLAMP_SOL.SET();
                                        await Task.Delay(500);
                                        CV_NFC_Step = 0;
                                    }
                                }
                            }
                            if (CV_NFC_Step == 0) AXIS_03_STEP = 1;
                            if (CV_NFC_Step == 1)
                            {
                                if (await MoveAbs(Axis._03, AXIS_03_LOADING_CV_POS.JigPos, Axis03Velocity))
                                    CV_NFC_Step = 2;
                            }
                            if (CV_NFC_Step == 2)
                            {
                                await OUT.LOADING_03_LIFT_SOL.SET();
                                if (IN.LOADING_03_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    CV_NFC_Step = 3;
                                }
                            }
                            if (CV_NFC_Step == 3)
                            {//Clamp
                                await OUT.LOADING_03_CLAMP_SOL.SET();
                                await OUT.LOADING_03_UNCLAMP_SOL.RST();
                                if (IN.LOADING_03_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    CV_NFC_Step = 4;
                                }
                            }
                            if (CV_NFC_Step == 4)
                            {
                                await OUT.LOADING_03_LIFT_SOL.RST();
                                if (IN.LOADING_03_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (IN.LOADING_03_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_03_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        Trans_CV_Debounce = 5;
                                        CV_NFC_Step = 5;
                                    }
                                    else//If CLAMP 03 SENSOR IS ON OR UNCLAMP_SENSOR IS ON
                                    {
                                        if (MessageBox.Show("LOADING 03 CLAMP ERROR, Reset?", "Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                        {
                                            await OUT.LOADING_03_CLAMP_SOL.RST();
                                            await OUT.LOADING_03_UNCLAMP_SOL.SET();
                                            CV_NFC_Step = 0;
                                        }
                                        else NFC_LEAK_RUN = false;
                                    }
                                }
                            }
                            if (CV_NFC_Step == 5)
                            {
                                if (await MoveAbs(Axis._03, ReadyNFC.Jig.JigPos, Axis03Velocity))
                                    CV_NFC_Step = 6;
                            }
                            if (CV_NFC_Step == 6)
                            {
                                //Lift Down
                                await OUT.LOADING_03_LIFT_SOL.SET();
                                if (IN.LOADING_03_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    CV_NFC_Step = 7;
                                }
                            }
                            if (CV_NFC_Step == 7)
                            {
                                //Unclamp
                                await OUT.LOADING_03_CLAMP_SOL.RST();
                                await OUT.LOADING_03_UNCLAMP_SOL.SET();
                                if (IN.LOADING_03_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                {
                                    //Set in Jig
                                    ReadyNFC.Jig.IsSetInJig = true;
                                    CV_NFC_Step = 8;
                                }
                            }
                            if (CV_NFC_Step == 8)
                            {   //Lift Up
                                await OUT.LOADING_03_LIFT_SOL.RST();
                                if (IN.LOADING_03_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    CV_NFC_Step = 9;
                                }
                            }
                            if (CV_NFC_Step == 9)
                            {
                                ReadyNFC.Jig.TestResult = TestResult.TEST;
                                await ReadyNFC.PackingPin.SET();
                                CV_NFC_Step = 10;
                            }
                            if (CV_NFC_Step == 10)
                            {
                                CV_NFC_Step = 0;
                                AXIS_03_STEP = 1;
                            }
                        }
                        if (AXIS_03_STEP == 1)
                        {
                            if (NFC_Buf_Step == 0)
                                if (!NFCBypass && IN.LOADING_03_UNCLAMP_SENSOR.PinValue == ON)
                                {
                                    TestedNFC = null;
                                    NFCTestedResult = TestResult.READY;
                                    foreach (NFCJig NFC in NFCs)
                                    {
                                        if ((NFC.Jig.TestResult == TestResult.PASS || NFC.Jig.TestResult == TestResult.FAIL) && NFC.Jig.IsSetInJig)
                                        {
                                            TestedNFC = NFC;
                                            NFCTestedResult = TestedNFC.Jig.TestResult;
                                            break;
                                        }
                                    }
                                    if (TestedNFC != null)
                                        NFC_Buf_Step = 1;
                                }
                                else
                                if (IN.LOADING_03_UNCLAMP_SENSOR.PinValue == ON && NFCBypass)
                                {
                                    if (Trans_CV_Debounce == 0)
                                    {
                                        TestedNFC = new NFCJig() { Jig = AXIS_03_LOADING_CV_POS };
                                        TestedNFC.Jig.TestResult = TestResult.PASS;
                                        NFCTestedResult = TestedNFC.Jig.TestResult;
                                        NFC_Buf_Step = 1;
                                    }
                                }
                                else if (IN.LOADING_03_UNCLAMP_SENSOR.PinValue == OFF &&
                                         IN.LOADING_03_CLAMP_SENSOR.PinValue == OFF && TestedNFC != null)
                                {
                                    if (NFCBypass)
                                    {
                                        TestedNFC = new NFCJig() { Jig = AXIS_03_LOADING_CV_POS };
                                        TestedNFC.Jig.TestResult = TestResult.PASS;
                                        NFCTestedResult = TestedNFC.Jig.TestResult;
                                    }
                                    NFC_Buf_Step = 5;
                                }
                                else if (IN.LOADING_03_CLAMP_SENSOR.PinValue == ON)
                                {
                                    await OUT.LOADING_03_CLAMP_SOL.RST();
                                    await OUT.LOADING_03_UNCLAMP_SOL.SET();
                                    await Task.Delay(500);
                                    NFC_Buf_Step = 0;
                                }
                            if (NFC_Buf_Step == 1)
                            {
                                if (!AXIS_03_BUFFER.Jig.IsSetInJig || ReadyNFC == null || NFCBypass)
                                {
                                    NFC_Buf_Step = 2;
                                }
                            }
                            if (NFC_Buf_Step == 0 || NFC_Buf_Step == 1) AXIS_03_STEP = 0;

                            if (NFC_Buf_Step == 2)
                            {
                                if (await MoveAbs(Axis._03, TestedNFC.Jig.JigPos, Axis03Velocity))
                                    NFC_Buf_Step = 3;
                            }
                            if (NFC_Buf_Step == 3)
                            {//Lift 01 Down
                                await OUT.LOADING_03_LIFT_SOL.SET();
                                if (TestedNFC.PackingPin != null)
                                    await TestedNFC.PackingPin.RST();
                                if (IN.LOADING_03_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    NFC_Buf_Step = 4;
                                }
                            }
                            if (NFC_Buf_Step == 4)
                            {//Clamp
                                await OUT.LOADING_03_CLAMP_SOL.SET();
                                await OUT.LOADING_03_UNCLAMP_SOL.RST();
                                if (IN.LOADING_03_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    NFC_Buf_Step = 5;
                                }
                            }
                            if (NFC_Buf_Step == 5)
                            {
                                await OUT.LOADING_03_LIFT_SOL.RST();
                                if (IN.LOADING_03_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (IN.LOADING_03_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_03_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        NFC_Buf_Step = 6;
                                        TestedNFC.Jig.IsSetInJig = false;
                                        if (NFCTestedResult == TestResult.PASS)
                                            TestedNFC.Jig.OKCounter++;
                                        else
                                            TestedNFC.Jig.NGCounter++;
                                    }
                                    else//If CLAMP 03 SENSOR IS ON OR UNCLAMP_SENSOR IS ON
                                    {
                                        if (MessageBox.Show("LOADING 03 CLAMP ERROR, Reset?", "Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                        {
                                            await OUT.LOADING_03_CLAMP_SOL.RST();
                                            await OUT.LOADING_03_UNCLAMP_SOL.SET();
                                            TestedNFC.Jig.TestResult = TestResult.READY;
                                            NFC_Buf_Step = 0;
                                        }
                                        else
                                            NFC_LEAK_RUN = false;
                                    }
                                }
                            }
                            if (NFC_Buf_Step == 6)
                            {
                                if (!AXIS_03_BUFFER.Jig.IsSetInJig && (Axis._04.CurrentPos <= LEAKs[0].Jig.JigPos))
                                {
                                    if (await MoveAbs(Axis._03, AXIS_03_BUFFER.Jig.JigPos, Axis03Velocity))
                                        NFC_Buf_Step = 7;
                                }
                                else
                                    await MoveAbs(Axis._03, NFCs[2].Jig.JigPos, Axis03Velocity);
                            }
                            if (NFC_Buf_Step == 7)
                            {
                                await OUT.LOADING_03_LIFT_SOL.SET();
                                if (IN.LOADING_03_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    AXIS_03_BUFFER.Jig.TestResult = NFCTestedResult;
                                    AXIS_03_BUFFER.Jig.JigDescription = TestedNFC.Jig.JigDescription;
                                    AXIS_03_BUFFER.Jig.IsSetInJig = true;
                                    TestedNFC.Jig.TestResult = TestResult.READY;
                                    NFC_Buf_Step = 8;
                                }
                            }
                            if (NFC_Buf_Step == 8)
                            {
                                //Unclamp
                                await OUT.LOADING_03_CLAMP_SOL.RST();
                                await OUT.LOADING_03_UNCLAMP_SOL.SET();
                                if (IN.LOADING_03_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                {
                                    //Set in Jig
                                    //ReadyPD.Jig.IsSetInJig = true;
                                    NFC_Buf_Step = 9;
                                }
                            }
                            if (NFC_Buf_Step == 9)
                            {
                                //Lift Up
                                await OUT.LOADING_03_LIFT_SOL.RST();
                                if (IN.LOADING_03_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    NFC_Buf_Step = 10;
                                }
                            }
                            if (NFC_Buf_Step == 10)
                            {
                                MoveNFC = null;
                                ReadyNFC = null;
                                foreach (NFCJig NFC in NFCs)
                                {
                                    if (NFC.Jig.IsJigEnable == true && !NFC.Jig.IsSetInJig &&
                                        NFC.PackingPin.PinValue == PinValue.OFF &&
                                        NFC.Jig.TestResult == TestResult.READY)
                                    {
                                        ReadyNFC = NFC;
                                        break;
                                    }
                                }
                                if (ReadyNFC != null && Trans_CV_Debounce == 0)
                                {
                                    MoveNFC = null;
                                    goto Skip;
                                }
                                else
                                {
                                    foreach (NFCJig NFC in NFCs.Reverse())
                                    {
                                        if ((NFC.Jig.TestResult == TestResult.PASS || NFC.Jig.TestResult == TestResult.FAIL) && NFC.Jig.IsSetInJig)
                                        {
                                            MoveNFC = NFC;
                                            break;
                                        }
                                    }
                                    if (MoveNFC == null)
                                    {
                                        TimeSpan TestedTime = DateTime.Now - NFCs[0].Jig.StatTime;
                                        foreach (NFCJig NFC in NFCs.Reverse())
                                        {
                                            TimeSpan CurrentNFCTestedTime = DateTime.Now - NFC.Jig.StatTime;
                                            if (CurrentNFCTestedTime > TestedTime && NFC.Jig.IsSetInJig && NFC.Jig.IsJigEnable)
                                            {
                                                TestedTime = CurrentNFCTestedTime;
                                                MoveNFC = NFC;
                                            }
                                        }
                                    }
                                }
                            Skip:
                                NFC_Buf_Step = 11;

                            }
                            if (NFC_Buf_Step == 11)
                            {
                                if (MoveNFC != null)
                                {
                                    if (await MoveAbs(Axis._03, MoveNFC.Jig.JigPos, Axis03Velocity))
                                        NFC_Buf_Step = 12;
                                }
                                else
                                {
                                    if (await MoveAbs(Axis._03, AXIS_03_LOADING_CV_POS.JigPos, Axis03Velocity))
                                        NFC_Buf_Step = 12;
                                }
                            }
                            if (NFC_Buf_Step == 12)
                            {
                                NFC_Buf_Step = 0;
                                AXIS_03_STEP = 0;
                            }
                        }
                        if (AXIS_04_STEP == 0)
                        {
                            if (Buf_Leak_Step == 0)
                                if (!LEAKBypass && IN.LOADING_04_UNCLAMP_SENSOR.PinValue == ON)
                                {
                                    if ((Axis._03.CurrentPos <= NFCs[3].Jig.JigPos || NFC_Buf_Step >= 11) && AXIS_03_BUFFER.Jig.IsSetInJig == true)
                                    {
                                        ReadyLeak = null;
                                        if (AXIS_03_BUFFER.Jig.TestResult == TestResult.PASS)
                                            foreach (LeakJig Leak in LEAKs)
                                            {
                                                if (Leak.Jig.IsJigEnable == true && !Leak.Jig.IsSetInJig &&
                                                    Leak.Jig.TestResult == TestResult.READY)
                                                {
                                                    ReadyLeak = Leak;
                                                    break;
                                                }
                                            }
                                        else if (AXIS_04_BUFFER.TestResult == TestResult.FAIL)
                                            Buf_Leak_Step = 1;

                                        if (ReadyLeak != null)
                                            Buf_Leak_Step = 1;
                                        AXIS_04_BUFFER.TestResult = AXIS_03_BUFFER.Jig.TestResult;
                                        AXIS_04_BUFFER.JigDescription = AXIS_03_BUFFER.Jig.JigDescription;
                                    }
                                }
                                else if (IN.LOADING_04_UNCLAMP_SENSOR.PinValue == OFF &&
                                         IN.LOADING_04_CLAMP_SENSOR.PinValue == OFF)
                                {
                                    ReadyLeak = null;
                                    foreach (LeakJig Leak in LEAKs)
                                    {
                                        if (Leak.Jig.IsJigEnable == true && !Leak.Jig.IsSetInJig &&
                                            Leak.Jig.TestResult == TestResult.READY)
                                        {
                                            ReadyLeak = Leak;
                                            break;
                                        }
                                    }
                                    if (ReadyLeak != null)
                                        Buf_Leak_Step = 4;
                                }
                                else if (IN.LOADING_04_CLAMP_SENSOR.PinValue == ON)
                                {
                                    await OUT.LOADING_04_CLAMP_SOL.RST();
                                    await OUT.LOADING_04_UNCLAMP_SOL.SET();
                                    await Task.Delay(500);
                                    Buf_Leak_Step = 0;
                                }

                            if (Buf_Leak_Step == 0) AXIS_04_STEP = 1;

                            //RUN---------------------------------------------
                            if (Buf_Leak_Step == 1)
                            {
                                if (await MoveAbs(Axis._04, AXIS_04_BUFFER.JigPos, Axis04Velocity))
                                    Buf_Leak_Step = 2;
                            }
                            if (Buf_Leak_Step == 2)
                            {
                                await OUT.LOADING_04_LIFT_SOL.SET();
                                if (IN.LOADING_04_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    Buf_Leak_Step = 3;
                                }
                            }
                            if (Buf_Leak_Step == 3)
                            {//Clamp
                                await OUT.LOADING_04_CLAMP_SOL.SET();
                                await OUT.LOADING_04_UNCLAMP_SOL.RST();
                                if (IN.LOADING_04_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    Buf_Leak_Step = 4;
                                }
                            }
                            if (Buf_Leak_Step == 4)
                            {
                                await OUT.LOADING_04_LIFT_SOL.RST();
                                if (IN.LOADING_04_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (IN.LOADING_04_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_04_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        AXIS_03_BUFFER.Jig.IsSetInJig = false;
                                        Buf_Leak_Step = 5;
                                    }
                                    else//If CLAMP 03 SENSOR IS ON OR UNCLAMP_SENSOR IS ON
                                    {
                                        if (MessageBox.Show("LOADING 04 CLAMP ERROR, Reset? " + Buf_Leak_Step, "Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                        {
                                            await OUT.LOADING_04_CLAMP_SOL.RST();
                                            await OUT.LOADING_04_UNCLAMP_SOL.SET();
                                            AXIS_03_BUFFER.Jig.IsSetInJig = false;
                                            //await MoveAbs(Axis._04, LEAKs[0].Jig.JigPos, Axis04Velocity);
                                            Buf_Leak_Step = 0;
                                        }
                                        else NFC_LEAK_RUN = false;
                                    }
                                }
                            }
                            if (Buf_Leak_Step == 5)
                            {
                                if (AXIS_04_BUFFER.TestResult == TestResult.FAIL)
                                {
                                    if (await MoveAbs(Axis._04, AXIS_04_NG_POS.JigPos, Axis04Velocity))
                                        Buf_Leak_Step = 6;
                                }
                                else
                                if (ReadyLeak != null)
                                    if (await MoveAbs(Axis._04, ReadyLeak.Jig.JigPos, Axis04Velocity))
                                        Buf_Leak_Step = 6;
                            }
                            if (Buf_Leak_Step == 6)
                            {
                                //Lift Down
                                if (AXIS_04_BUFFER.TestResult == TestResult.FAIL)
                                {
                                    if (IN.NG_CV_SENSOR_01.PinValue == PinValue.ON)
                                    {
                                        await OUT.LOADING_04_LIFT_SOL.SET();
                                        NGCVRun = 0;
                                    }
                                }
                                else
                                    await OUT.LOADING_04_LIFT_SOL.SET();

                                if (IN.LOADING_04_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (AXIS_04_BUFFER.TestResult == TestResult.FAIL)
                                    {
                                        TotalFail++;
                                        Printer.Print("", AXIS_04_BUFFER.JigDescription);
                                        MainWindow.Root.WriteToLog(string.Format("{1} - {2} FAIL", DateTime.Now.ToString("HH:mm:ss yyyy/MM/dd"), "", AXIS_04_BUFFER.JigDescription));
                                        //LEAKNG.LogFile(App.LEAK_NG_COM, AXIS_04_BUFFER, TestResult.FAIL);
                                        //Axis04Delay = 10;
                                        //WriteToLog(DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy: ") + AXIS_04_BUFFER.JigDescription);
                                    }
                                    Buf_Leak_Step = 7;
                                }
                            }
                            if (Buf_Leak_Step == 7)
                            {
                                //Unclamp
                                await OUT.LOADING_04_CLAMP_SOL.RST();
                                await OUT.LOADING_04_UNCLAMP_SOL.SET();
                                if (IN.LOADING_04_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                {
                                    //Set in Jig
                                    if (AXIS_04_BUFFER.TestResult == TestResult.PASS)
                                        ReadyLeak.Jig.IsSetInJig = true;
                                    Buf_Leak_Step = 8;
                                }
                            }
                            if (Buf_Leak_Step == 8)
                            {   //Lift Up
                                await OUT.LOADING_04_LIFT_SOL.RST();
                                if (IN.LOADING_04_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    Buf_Leak_Step = 9;
                                }
                            }
                            if (Buf_Leak_Step == 9)
                            {
                                if (AXIS_04_BUFFER.TestResult == TestResult.PASS)
                                {
                                    ReadyLeak.Jig.TestResult = TestResult.TEST;
                                    ReadyLeak.Press();
                                }
                                else if (AXIS_04_BUFFER.TestResult == TestResult.FAIL)
                                    NGCVRun = 200;
                                Buf_Leak_Step = 10;
                            }
                            if (Buf_Leak_Step == 10)
                            {
                                Buf_Leak_Step = 0;
                                AXIS_04_STEP = 1;
                            }
                        }
                        if (AXIS_04_STEP == 1)
                        {
                            if (LeakFinalStep == 0)
                                if (IN.LOADING_04_UNCLAMP_SENSOR.PinValue == ON && !LEAKBypass)
                                {
                                    TestedLeak = null;
                                    LeakTestedResult = TestResult.READY;
                                    foreach (LeakJig Leak in LEAKs)
                                    {
                                        if (((Leak.Jig.TestResult == TestResult.FAIL && IN.NG_CV_SENSOR_01.PinValue == PinValue.ON) ||
                                            (Leak.Jig.TestResult == TestResult.PASS && IN.UNLOADING_CV_SENSOR_02.PinValue == PinValue.ON)) &&
                                            Leak.Jig.IsSetInJig && Leak.ReverseSensor.PinValue == PinValue.ON)
                                        {
                                            LeakTestedResult = Leak.Jig.TestResult;
                                            TestedLeak = Leak;
                                            break;
                                        }
                                    }
                                    if (TestedLeak != null)
                                        LeakFinalStep = 1;
                                }
                                else if (IN.LOADING_04_UNCLAMP_SENSOR.PinValue == ON && LEAKBypass)
                                {
                                    if ((Axis._03.CurrentPos <= NFCs[3].Jig.JigPos || NFC_Buf_Step == 11) && AXIS_03_BUFFER.Jig.IsSetInJig == true)
                                    {
                                        TestedLeak = new LeakJig() { Jig = AXIS_04_BUFFER, ReverseSensor = new InputPin() { PinValue = PinValue.ON } };
                                        LeakTestedResult = AXIS_03_BUFFER.Jig.TestResult;
                                        LeakFinalStep = 1;
                                    }
                                }
                                else if (IN.LOADING_04_UNCLAMP_SENSOR.PinValue == OFF &&
                                         IN.LOADING_04_CLAMP_SENSOR.PinValue == OFF && TestedLeak != null)
                                {
                                    if (LEAKBypass)
                                    {
                                        TestedLeak = new LeakJig() { Jig = AXIS_04_BUFFER, ReverseSensor = new InputPin() { PinValue = PinValue.ON } };
                                        LeakTestedResult = TestedLeak.Jig.TestResult;
                                        LeakFinalStep = 1;
                                    }
                                    LeakFinalStep = 5;
                                }
                                else if (IN.LOADING_04_CLAMP_SENSOR.PinValue == ON)
                                {
                                    await OUT.LOADING_04_CLAMP_SOL.RST();
                                    await OUT.LOADING_04_UNCLAMP_SOL.SET();
                                    await Task.Delay(500);
                                    LeakFinalStep = 0;
                                    AXIS_04_STEP = 0;
                                }

                            if (LeakFinalStep == 1)
                            {
                                //if (TestedLeak.ReverseSensor.PinValue == PinValue.ON)
                                LeakFinalStep = 2;
                            }
                            if (LeakFinalStep == 0 || LeakFinalStep == 1) AXIS_04_STEP = 0;
                            if (LeakFinalStep == 2)
                            {
                                if (await MoveAbs(Axis._04, TestedLeak.Jig.JigPos, Axis04Velocity))
                                    LeakFinalStep = 3;
                            }
                            if (LeakFinalStep == 3)
                            {
                                if (TestedLeak.ReverseSensor.PinValue == PinValue.ON)
                                    await OUT.LOADING_04_LIFT_SOL.SET();
                                else await TestedLeak.ReleaseTask();
                                if (IN.LOADING_04_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                    LeakFinalStep = 4;
                            }
                            if (LeakFinalStep == 4)
                            {//Clamp
                                await OUT.LOADING_04_CLAMP_SOL.SET();
                                await OUT.LOADING_04_UNCLAMP_SOL.RST();
                                if (IN.LOADING_04_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    LeakFinalStep = 5;
                            }
                            if (LeakFinalStep == 5)
                            {
                                await OUT.LOADING_04_LIFT_SOL.RST();
                                if (IN.LOADING_04_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (IN.LOADING_04_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_04_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        LeakFinalStep = 6;
                                        TestedLeak.Jig.IsSetInJig = false;
                                        if (LEAKBypass)
                                            AXIS_03_BUFFER.Jig.IsSetInJig = false;
                                        //TestedPD.Jig.TestResult = TestResult.READY;
                                    }
                                    else//If CLAMP 04 SENSOR IS ON OR UNCLAMP_SENSOR IS ON
                                    {
                                        if (MessageBox.Show(MainPage, "LOADING 04 CLAMP ERROR, Reset?", "Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                        {
                                            await OUT.LOADING_04_CLAMP_SOL.RST();
                                            await OUT.LOADING_04_UNCLAMP_SOL.SET();
                                            TestedLeak.Jig.TestResult = TestResult.READY;
                                            LeakFinalStep = 0;
                                        }
                                        else NFC_LEAK_RUN = false;
                                    }
                                }
                            }
                            if (LeakFinalStep == 6)
                            {
                                if (LeakTestedResult == TestResult.PASS)
                                {
                                    if (await MoveAbs(Axis._04, AXIS_04_UNLOADING_CV_POS.JigPos, Axis04Velocity))
                                    {
                                        LeakFinalStep = 7;
                                        TestedLeak.Jig.OKCounter++;
                                        //OutputCVRun = 300;
                                    }
                                }
                                else if (LeakTestedResult == TestResult.FAIL)
                                {
                                    if (await MoveAbs(Axis._04, AXIS_04_NG_POS.JigPos, Axis04Velocity))
                                    {
                                        TestedLeak.Jig.NGCounter++;
                                        LeakFinalStep = 7;
                                        //NGCVRun = 200;
                                    }
                                }
                            }
                            if (LeakFinalStep == 7)
                            {


                                if ((LeakTestedResult == TestResult.PASS && IN.UNLOADING_CV_SENSOR_02.PinValue != PinValue.ON) ||
                                    (LeakTestedResult == TestResult.FAIL && IN.NG_CV_SENSOR_01.PinValue != PinValue.ON))
                                    Axis04Delay = 10;
                                if (Axis04Delay == 0)
                                    await OUT.LOADING_04_LIFT_SOL.SET();

                                if (IN.LOADING_04_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (LeakTestedResult == TestResult.PASS)
                                    {
                                        //LEAKOK.LogFile(App.LEAK_OK_COM, TestedLeak.Jig, TestResult.PASS);
                                        Times.Add(CycleTime.TotalSeconds);
                                        if (Times.Count > 20)
                                        {
                                            Times.Remove(Times.First());
                                        }
                                        double SUM = 0;
                                        if (Times.Count > 0)
                                        {
                                            foreach (double d in Times)
                                            {
                                                SUM += d;
                                            }
                                            AVTime = SUM / Times.Count;
                                        }
                                        TotalOK++;
                                        PreCycleTime = DateTime.Now;
                                    }
                                    else
                                    {
                                        //LEAKNG.LogFile(App.LEAK_NG_COM, TestedLeak.Jig, TestResult.FAIL);
                                        Printer.Print("", TestedLeak.Jig.JigDescription);
                                        MainWindow.Root.WriteToLog(string.Format("{1} - {2} FAIL", DateTime.Now.ToString("HH:mm:ss yyyy/MM/dd"), "", TestedLeak.Jig.JigDescription));
                                        NGCVRun = 0;
                                        TotalFail++;
                                    }
                                    LeakFinalStep = 8;
                                }
                            }
                            if (LeakFinalStep == 8)
                            {
                                //Unclamp
                                await OUT.LOADING_04_CLAMP_SOL.RST();
                                await OUT.LOADING_04_UNCLAMP_SOL.SET();
                                if (IN.LOADING_04_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                {
                                    //Set in Jig
                                    //ReadyPD.Jig.IsSetInJig = true;
                                    //if (TestedLeak.Jig.TestResult == TestResult.PASS)
                                    //OutputCVRun = 200;
                                    //else if (TestedLeak.Jig.TestResult == TestResult.FAIL)
                                    //    NGCVRun = 200;
                                    LeakFinalStep = 9;
                                }
                            }
                            if (LeakFinalStep == 9)
                            {
                                //Lift Up
                                await OUT.LOADING_04_LIFT_SOL.RST();
                                if (IN.LOADING_04_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (LeakTestedResult == TestResult.PASS)
                                        OutputCVRun = 200;
                                    else
                                    if (LeakTestedResult == TestResult.FAIL)
                                        NGCVRun = 200;
                                    TestedLeak.Jig.TestResult = TestResult.READY;
                                    //TestedLeak.Jig.IsRetested = false;
                                    LeakFinalStep = 10;
                                }
                            }
                            if (LeakFinalStep == 10)
                            {
                                MoveLeak = null;
                                ReadyLeak = null;
                                foreach (LeakJig Leak in LEAKs.Reverse())
                                {
                                    if (Leak.Jig.IsJigEnable == true && !Leak.Jig.IsSetInJig &&
                                        Leak.PackingPin.PinValue == PinValue.OFF && Leak.ReverseSensor.PinValue == PinValue.ON &&
                                        Leak.Jig.TestResult == TestResult.READY)
                                    {
                                        ReadyLeak = Leak;
                                        break;
                                    }
                                }
                                if (ReadyLeak != null && (AXIS_03_BUFFER.Jig.IsSetInJig || NFC_Buf_Step >= 11))
                                {
                                    MoveNFC = null;
                                    LeakFinalStep = 11;
                                }
                                else
                                {
                                    foreach (LeakJig Leak in LEAKs)
                                    {
                                        if ((Leak.Jig.TestResult == TestResult.PASS || Leak.Jig.TestResult == TestResult.FAIL) && Leak.Jig.IsSetInJig)
                                        {
                                            MoveLeak = Leak;
                                            break;
                                        }
                                    }
                                    if (MoveLeak == null)
                                    {
                                        TimeSpan TestedTime = DateTime.Now - LEAKs[3].Jig.StatTime;
                                        foreach (LeakJig Leak in LEAKs.Reverse())
                                        {
                                            TimeSpan CurrentLeakestedTime = DateTime.Now - Leak.Jig.StatTime;
                                            if (CurrentLeakestedTime > TestedTime && Leak.Jig.IsSetInJig && Leak.Jig.IsJigEnable)
                                            {
                                                TestedTime = CurrentLeakestedTime;
                                                MoveLeak = Leak;
                                            }
                                        }
                                    }
                                }
                                LeakFinalStep = 11;
                            }
                            if (LeakFinalStep == 11)
                            {
                                if (MoveLeak != null)
                                {
                                    if (await MoveAbs(Axis._04, MoveLeak.Jig.JigPos, Axis04Velocity))
                                        LeakFinalStep = 12;
                                }
                                else if (AXIS_03_BUFFER.Jig.IsSetInJig && (Axis._03.CurrentPos <= NFCs[3].Jig.JigPos ||
                                                                          (NFC_Buf_Step >= 11 && Axis._03.Motioning == PinValue.ON)))
                                {
                                    if (await MoveAbs(Axis._04, AXIS_04_BUFFER.JigPos, Axis04Velocity))
                                        LeakFinalStep = 12;
                                }
                                else
                                {
                                    if (await MoveAbs(Axis._04, LEAKs[0].Jig.JigPos, Axis04Velocity))
                                        LeakFinalStep = 12;
                                }
                            }
                            if (LeakFinalStep == 12)
                            {
                                LeakFinalStep = 0;
                                AXIS_04_STEP = 0;
                            }
                        }
                    }
                    else
                    {
                        await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveStop, null);
                        await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveStop, null);
                    }
                }
            }
            catch (Exception ex)
            {
                await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.MoveStop, null);
                await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.MoveStop, null);
                MainWindow.Root.WriteToLog(string.Format("{1} - {2}", DateTime.Now.ToString("HH:mm:ss yyyy/MM/dd"), "", ex.Message));
                NFCLoopExcuting = false;
            }
        }
        private void NFCLoop()
        {
            NFCLoopCancelationTokenSource = new CancellationTokenSource();
            Task.Run(async () => await NFCLoopExecution(NFCLoopCancelationTokenSource.Token));
        }

        private void BeginExecution()
        {
            // The previous Extended Execution must be closed before a new one can10 be requested.
            // This code is redundant here because the sample doesn't allow a new extended
            // execution to begin until the previous one ends, but we leave it here for illustration.
            CancelExecution();
            //------------------------AutoLoop-----------------------------------            
            //ReadUCTLoop();
            PDLoop();
            NFCLoop();
        }

        public Auto()
        {
            InitializeComponent();
            Page = this;
            MainPage = MainWindow.Root;
            Loaded += Auto_Loaded;
            Unloaded += Auto_Unloaded;
            AlarmWindow.AlarmListViewItem.CollectionChanged += AlarmListViewItem_CollectionChanged;
            //Printer.Print("xYXABB", "PD_01");
        }

        private void AlarmListViewItem_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ShowAlarmDialog();
        }

        private async void ShowAlarmDialog()
        {
            await Dispatcher.InvokeAsync(() =>
            {
                if (ALMWindows.Visibility != Visibility.Visible)
                    ALMWindows.ShowDialog();
            });
        }

        bool OnStart = true;
        public async void UnpackJig()
        {
            foreach (UCT100 UCT in UCT100s)
            {
                foreach (PDJig PD in UCT.PDs)
                    await PD.PackingPin.RST();
                await UCT.PowerPin.RST();
            }
            foreach (NFCJig NFC in NFCs)
            {
                await NFC.PackingPin.RST();
            }
            foreach (LeakJig Leak in LEAKs)
            {
                await Leak.ReleaseTask();
            }
        }

        private void Start_Stop_OnPinValueChanged(object sender, PinValueChangedEventArgs e)
        {
            var Pin = sender as InputPin;
            if ((Pin == IN.START_BUTTON_01 || Pin == IN.START_BUTTON_02) && e.Edge == Edge.Rise)
            {
                Dispatcher.Invoke(() =>
                {
                    RUN();
                    if (ALMWindows.Visibility == Visibility.Visible)
                        ALMWindows.Hide();
                });
            }
            if ((Pin == IN.STOP_BUTTON_01 || Pin == IN.STOP_BUTTON_02) && e.Edge == Edge.Rise)
            {
                Dispatcher.Invoke(() =>
                {
                    PAUSE();
                });
            }
        }


        DispatcherTimer T1 = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100), IsEnabled = true };
        private async void Auto_Loaded(object sender, RoutedEventArgs e)
        {
            //Printer.Print("", "ABC");
            CancelLoopTask();
            CancelExecution();
            await Init_Db();
            await Task.Delay(100);
            MainLoop();
            if (!PDBypass)
                ReadUCTLoop();
            T1.Tick -= T1_Tick;
            T1.Tick += T1_Tick;
            T1.Start();
        }
        private void Auto_Unloaded(object sender, RoutedEventArgs e)
        {
            DeinitDB();
        }

        uint Axis01Delay = 0;
        uint Axis02Delay = 0;
        uint Axis03Delay = 0;
        uint Axis04Delay = 0;
        object TimerLockObject = new object();
        string Now;
        private void T1_Tick(object sender, EventArgs e)
        {
            if (Axis01Delay > 0) Axis01Delay--;
            if (Axis02Delay > 0) Axis02Delay--;
            if (Axis03Delay > 0) Axis03Delay--;
            if (Axis04Delay > 0) Axis04Delay--;
            if (InputCVRun1 > 0) InputCVRun1--;
            if (InputCVRun2 > 0) InputCVRun2--;
            if (InputCVTimer > 0) InputCVTimer--;
            if (NGCVRun > 0 && IN.NG_CV_SENSOR_04.PinValue == PinValue.ON && PDRUN && NFC_LEAK_RUN) NGCVRun--;
            if (TransCVRun > 0 && IN.TRANSFER_CV_SENSOR_END.PinValue == PinValue.ON && PDRUN && NFC_LEAK_RUN) TransCVRun--;
            if (OutputCVRun > 0 && IN.UNLOADING_CV_SENSOR_03.PinValue == PinValue.ON && PDRUN && NFC_LEAK_RUN) OutputCVRun--;
            Now = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            if (pDLoopExcuting && !PDAlarm)
            {
                if (IN.INPUT_CV_SENSOR_01.PinValue == PinValue.OFF || IN.INPUT_CV_SENSOR_02.PinValue == PinValue.OFF)
                {
                    InputCVTimer = 50;
                }
                //LIFT_01 ALARM
                if ((IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.OFF && OUT.LOADING_01_LIFT_SOL.PinValue == PinValue.ON) ||
                    (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.OFF && OUT.LOADING_01_LIFT_SOL.PinValue == PinValue.OFF))
                {
                    Alarms.Lift_01_Alarm.AlarmTimer++;
                    if (Alarms.Lift_01_Alarm.AlarmTimer >= 50)
                    {
                        PDAlarm = true;
                        AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Lift_01_Alarm.AlarmMessage}");
                        //Alarms.Lift_01_Alarm.AlarmState = PinValue.ON;
                    }
                }
                else
                {
                    Alarms.Lift_01_Alarm.AlarmTimer = 0;
                }
                ////LIFT_02 ALARM
                if ((IN.LOADING_02_LIFT_DOWN_SENSOR.PinValue == PinValue.OFF && OUT.LOADING_02_LIFT_SOL.PinValue == PinValue.ON) ||
                    (IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.OFF && OUT.LOADING_02_LIFT_SOL.PinValue == PinValue.OFF))
                {
                    Alarms.Lift_02_Alarm.AlarmTimer++;
                    if (Alarms.Lift_02_Alarm.AlarmTimer >= 50)
                    {
                        AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Lift_02_Alarm.AlarmMessage}");
                        PDAlarm = true;
                    }
                }
                else
                {
                    Alarms.Lift_02_Alarm.AlarmTimer = 0;
                }
                //CLAMP 1 ALARM
                if ((OUT.LOADING_01_CLAMP_SOL.PinValue == PinValue.ON && (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.ON || IN.LOADING_01_CLAMP_SENSOR.PinValue == PinValue.ON)) ||
                    (OUT.LOADING_01_UNCLAMP_SOL.PinValue == PinValue.ON && (IN.LOADING_01_CLAMP_SENSOR.PinValue == PinValue.ON || IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)))
                {
                    Alarms.Clamp_01_Alarm.AlarmTimer++;
                    if (Alarms.Clamp_01_Alarm.AlarmTimer >= 50)
                    {
                        AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Clamp_01_Alarm.AlarmMessage}");
                        PDAlarm = true;
                    }
                }
                else
                    Alarms.Clamp_01_Alarm.AlarmTimer = 0;
                //CLAMP 2 ALARM
                if ((OUT.LOADING_02_CLAMP_SOL.PinValue == PinValue.ON && (IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.ON || IN.LOADING_02_CLAMP_SENSOR.PinValue == PinValue.ON)) ||
                    (OUT.LOADING_02_UNCLAMP_SOL.PinValue == PinValue.ON && (IN.LOADING_02_CLAMP_SENSOR.PinValue == PinValue.ON || IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.OFF)))
                {
                    Alarms.Clamp_02_Alarm.AlarmTimer++;
                    if (Alarms.Clamp_02_Alarm.AlarmTimer >= 50)
                    {
                        AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Clamp_02_Alarm.AlarmMessage}");
                        PDAlarm = true;
                    }
                }
                else
                    Alarms.Clamp_02_Alarm.AlarmTimer = 0;
                //LOADING #1 TURN ALARM
                if ((OUT.LOADING_01_TURN_SOL.PinValue == PinValue.ON && IN.LOADING_01_TURN_SENSOR.PinValue == PinValue.OFF) ||
                    (OUT.LOADING_01_TURN_SOL.PinValue == PinValue.OFF && IN.LOADING_01_RETURN_SENSOR.PinValue == PinValue.OFF))
                {
                    Alarms.Axis01TurnError.AlarmTimer++;
                    if (Alarms.Axis01TurnError.AlarmTimer >= 50)
                    {
                        AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Axis01TurnError.AlarmMessage}");
                        PDAlarm = true;
                    }
                }
                else
                    Alarms.Axis01TurnError.AlarmTimer = 0;

                //LOADING #2 TURN ALARM
                if ((OUT.LOADING_02_TURN_SOL.PinValue == PinValue.ON && IN.LOADING_02_TURN_SENSOR.PinValue == PinValue.OFF) ||
                    (OUT.LOADING_02_TURN_SOL.PinValue == PinValue.OFF && IN.LOADING_02_RETURN_SENSOR.PinValue == PinValue.OFF))
                {
                    Alarms.Axis02TurnError.AlarmTimer++;
                    if (Alarms.Axis02TurnError.AlarmTimer >= 50)
                    {
                        AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Axis02TurnError.AlarmMessage}");
                        PDAlarm = true;
                    }
                }
                else
                    Alarms.Axis02TurnError.AlarmTimer = 0;

                if (IN.DOOR1_SENSOR.PinValue == PinValue.OFF)
                {
                    PDAlarm = true;
                    AlarmWindow.AlarmListViewItem.Add($"<{Now}>: PD DOOR SENSOR ERROR ({IN.DOOR1_SENSOR.GPIOLabel})");
                }
                //SIM #1 ALARM
                if (SIMs[0].Jig.IsJigEnable)
                {
                    if ((OUT.SIM_01_PRESS_SOL.PinValue == PinValue.ON && IN.SIM_01_PRESS_SENSOR.PinValue == PinValue.OFF) ||
                        (OUT.SIM_01_PRESS_SOL.PinValue == PinValue.OFF && IN.SIM_01_RELEASE_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.SIM01PressError.AlarmTimer++;
                        if (Alarms.SIM01PressError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.SIM01PressError.AlarmMessage}");
                            PDAlarm = true;
                        }
                    }
                    else
                        Alarms.SIM01PressError.AlarmTimer = 0;

                    if ((OUT.SIM_01_PACK_SOL.PinValue == PinValue.ON && IN.SIM_01_PACK_IN_SENSOR.PinValue == PinValue.OFF) ||
                        (OUT.SIM_01_PACK_SOL.PinValue == PinValue.OFF && IN.SIM_01_PACK_OUT_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.SIM01PackError.AlarmTimer++;
                        if (Alarms.SIM01PackError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.SIM01PackError.AlarmMessage}");
                            PDAlarm = true;
                        }
                    }
                    else
                        Alarms.SIM01PackError.AlarmTimer = 0;
                }
                //SIM #2 ALARM
                if (SIMs[1].Jig.IsJigEnable)
                {
                    if ((OUT.SIM_02_PRESS_SOL.PinValue == PinValue.ON && IN.SIM_02_PRESS_SENSOR.PinValue == PinValue.OFF) ||
                        (OUT.SIM_02_PRESS_SOL.PinValue == PinValue.OFF && IN.SIM_02_RELEASE_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.SIM02PressError.AlarmTimer++;
                        if (Alarms.SIM02PressError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.SIM02PressError.AlarmMessage}");
                            PDAlarm = true;
                        }
                    }
                    else
                        Alarms.SIM02PressError.AlarmTimer = 0;

                    if ((OUT.SIM_02_PACK_SOL.PinValue == PinValue.ON && IN.SIM_02_PACK_IN_SENSOR.PinValue == PinValue.OFF) ||
                        (OUT.SIM_02_PACK_SOL.PinValue == PinValue.OFF && IN.SIM_02_PACK_OUT_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.SIM02PackError.AlarmTimer++;
                        if (Alarms.SIM02PackError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.SIM02PackError.AlarmMessage}");
                            PDAlarm = true;
                        }
                    }
                    else
                        Alarms.SIM02PackError.AlarmTimer = 0;
                }
            }
            if (nFCLoopExcuting && !NFCAlarm)
            {
                //LIFT_03 ALARM
                if ((IN.LOADING_03_LIFT_DOWN_SENSOR.PinValue == PinValue.OFF && OUT.LOADING_03_LIFT_SOL.PinValue == PinValue.ON) ||
                    (IN.LOADING_03_LIFT_UP_SENSOR.PinValue == PinValue.OFF && OUT.LOADING_03_LIFT_SOL.PinValue == PinValue.OFF))
                {
                    Alarms.Lift_03_Alarm.AlarmTimer++;
                    if (Alarms.Lift_03_Alarm.AlarmTimer >= 50)
                    {
                        AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Lift_03_Alarm.AlarmMessage}");
                        NFCAlarm = true;
                    }
                }
                else
                    Alarms.Lift_03_Alarm.AlarmTimer = 0;
                ////LIFT_04 ALARM
                if ((IN.LOADING_04_LIFT_DOWN_SENSOR.PinValue == PinValue.OFF && OUT.LOADING_04_LIFT_SOL.PinValue == PinValue.ON) ||
                    (IN.LOADING_04_LIFT_UP_SENSOR.PinValue == PinValue.OFF && OUT.LOADING_04_LIFT_SOL.PinValue == PinValue.OFF))
                {
                    Alarms.Lift_04_Alarm.AlarmTimer++;
                    if (Alarms.Lift_04_Alarm.AlarmTimer >= 50)
                    {
                        AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Lift_04_Alarm.AlarmMessage}");
                        NFCAlarm = true;
                    }
                }
                else Alarms.Lift_04_Alarm.AlarmTimer = 0;
                //CLAMP 3 ALARM
                if ((OUT.LOADING_03_CLAMP_SOL.PinValue == PinValue.ON && (IN.LOADING_03_UNCLAMP_SENSOR.PinValue == PinValue.ON || IN.LOADING_03_CLAMP_SENSOR.PinValue == PinValue.ON)) ||
                   (OUT.LOADING_03_UNCLAMP_SOL.PinValue == PinValue.ON && (IN.LOADING_03_CLAMP_SENSOR.PinValue == PinValue.ON || IN.LOADING_03_UNCLAMP_SENSOR.PinValue == PinValue.OFF)))
                {
                    Alarms.Clamp_03_Alarm.AlarmTimer++;
                    if (Alarms.Clamp_03_Alarm.AlarmTimer >= 50)
                    {
                        AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Clamp_03_Alarm.AlarmMessage}");
                        NFCAlarm = true;
                    }
                }
                else
                    Alarms.Clamp_03_Alarm.AlarmTimer = 0;
                //CLAMP 4 ALARM
                if ((OUT.LOADING_04_CLAMP_SOL.PinValue == PinValue.ON && (IN.LOADING_04_UNCLAMP_SENSOR.PinValue == PinValue.ON || IN.LOADING_04_CLAMP_SENSOR.PinValue == PinValue.ON)) ||
                    (OUT.LOADING_04_UNCLAMP_SOL.PinValue == PinValue.ON && (IN.LOADING_04_CLAMP_SENSOR.PinValue == PinValue.ON || IN.LOADING_04_UNCLAMP_SENSOR.PinValue == PinValue.OFF)))
                {
                    Alarms.Clamp_04_Alarm.AlarmTimer++;
                    if (Alarms.Clamp_04_Alarm.AlarmTimer >= 50)
                    {
                        AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Clamp_04_Alarm.AlarmMessage}");
                        NFCAlarm = true;
                    }
                }
                else
                    Alarms.Clamp_04_Alarm.AlarmTimer = 0;
                //DOOR_SENSOR_2 ALARM
                if (IN.DOOR2_SENSOR.PinValue == PinValue.OFF)
                {
                    NFCAlarm = true;
                    AlarmWindow.AlarmListViewItem.Add($"<{Now}>: NFC/LEAK DOOR SENSOR ERROR ({IN.DOOR2_SENSOR.GPIOLabel})");
                }

                //LEAK PRESS/TRANS #1 ALARM
                if (LEAKs[0].Jig.IsJigEnable)
                {
                    if ((OUT.LEAK_01_PRESS_SOL.PinValue == PinValue.ON && IN.LEAK_01_PRESS_DOWN_SENSOR.PinValue == PinValue.OFF) ||
                              (OUT.LEAK_01_PRESS_SOL.PinValue == PinValue.OFF && IN.LEAK_01_PRESS_UP_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.Leak01PressError.AlarmTimer++;
                        if (Alarms.Leak01PressError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Leak01PressError.AlarmMessage}");
                            NFCAlarm = true;
                        }
                    }
                    else
                        Alarms.Leak01PressError.AlarmTimer = 0;
                    if ((OUT.LEAK_01_TRANS_SOL.PinValue == PinValue.ON && IN.LEAK_01_FOWARD_SENSOR.PinValue == PinValue.OFF) ||
                       (OUT.LEAK_01_TRANS_SOL.PinValue == PinValue.OFF && IN.LEAK_01_REVERSE_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.Leak01TransError.AlarmTimer++;
                        if (Alarms.Leak01TransError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Leak01TransError.AlarmMessage}");
                            NFCAlarm = true;
                        }
                    }
                    else
                        Alarms.Leak01TransError.AlarmTimer = 0;
                }
                //LEAK PRESS/TRANS #2 ALARM
                if (LEAKs[1].Jig.IsJigEnable)
                {
                    if ((OUT.LEAK_02_PRESS_SOL.PinValue == PinValue.ON && IN.LEAK_02_PRESS_DOWN_SENSOR.PinValue == PinValue.OFF) ||
                        (OUT.LEAK_02_PRESS_SOL.PinValue == PinValue.OFF && IN.LEAK_02_PRESS_UP_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.Leak02PressError.AlarmTimer++;
                        if (Alarms.Leak02PressError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Leak02PressError.AlarmMessage}");
                            NFCAlarm = true;
                        }
                    }
                    else
                        Alarms.Leak02PressError.AlarmTimer = 0;
                    if ((OUT.LEAK_02_TRANS_SOL.PinValue == PinValue.ON && IN.LEAK_02_FOWARD_SENSOR.PinValue == PinValue.OFF) ||
                       (OUT.LEAK_02_TRANS_SOL.PinValue == PinValue.OFF && IN.LEAK_02_REVERSE_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.Leak02TransError.AlarmTimer++;
                        if (Alarms.Leak02TransError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Leak02TransError.AlarmMessage}");
                            NFCAlarm = true;
                        }
                    }
                    else
                        Alarms.Leak02TransError.AlarmTimer = 0;
                }
                //LEAK PRESS/TRANS #3 ALARM
                if (LEAKs[2].Jig.IsJigEnable)
                {
                    if ((OUT.LEAK_03_PRESS_SOL.PinValue == PinValue.ON && IN.LEAK_03_PRESS_DOWN_SENSOR.PinValue == PinValue.OFF) ||
                    (OUT.LEAK_03_PRESS_SOL.PinValue == PinValue.OFF && IN.LEAK_03_PRESS_UP_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.Leak03PressError.AlarmTimer++;
                        if (Alarms.Leak03PressError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Leak03PressError.AlarmMessage}");
                            NFCAlarm = true;
                        }
                    }
                    else
                        Alarms.Leak03PressError.AlarmTimer = 0;
                    if ((OUT.LEAK_03_TRANS_SOL.PinValue == PinValue.ON && IN.LEAK_03_FOWARD_SENSOR.PinValue == PinValue.OFF) ||
                       (OUT.LEAK_03_TRANS_SOL.PinValue == PinValue.OFF && IN.LEAK_03_REVERSE_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.Leak03TransError.AlarmTimer++;
                        if (Alarms.Leak03TransError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Leak03TransError.AlarmMessage}");
                            NFCAlarm = true;
                        }
                    }
                    else
                        Alarms.Leak03TransError.AlarmTimer = 0;
                }
                //LEAK PRESS/TRANS #4 ALARM
                if (LEAKs[3].Jig.IsJigEnable)
                {
                    if ((OUT.LEAK_04_PRESS_SOL.PinValue == PinValue.ON && IN.LEAK_04_PRESS_DOWN_SENSOR.PinValue == PinValue.OFF) ||
                        (OUT.LEAK_04_PRESS_SOL.PinValue == PinValue.OFF && IN.LEAK_04_PRESS_UP_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.Leak04PressError.AlarmTimer++;
                        if (Alarms.Leak04PressError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Leak04PressError.AlarmMessage}");
                            NFCAlarm = true;
                        }
                    }
                    else
                        Alarms.Leak04PressError.AlarmTimer = 0;
                    if ((OUT.LEAK_04_TRANS_SOL.PinValue == PinValue.ON && IN.LEAK_04_FOWARD_SENSOR.PinValue == PinValue.OFF) ||
                       (OUT.LEAK_04_TRANS_SOL.PinValue == PinValue.OFF && IN.LEAK_04_REVERSE_SENSOR.PinValue == PinValue.OFF))
                    {
                        Alarms.Leak04TransError.AlarmTimer++;
                        if (Alarms.Leak04TransError.AlarmTimer >= 50)
                        {
                            AlarmWindow.AlarmListViewItem.Add($"<{Now}>: {Alarms.Leak04TransError.AlarmMessage}");
                            NFCAlarm = true;
                        }
                    }
                    else
                        Alarms.Leak04TransError.AlarmTimer = 0;
                }
            }
        }

        private void SIMJig_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ClickedObject = sender as Grid;
            var SIMAction = new SIMAction(ClickedObject.DataContext as SIMJig);
            SIMAction.ShowDialog();
        }

        private void NFCGridView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ClickedObject = sender as Grid;
            var NFCAction = new NFCAction(ClickedObject.DataContext as NFCJig);
            NFCAction.ShowDialog();
        }

        private void LEAK_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clickedObject = sender as Grid;
            LeakAction LeakActionWindows = new LeakAction(clickedObject.DataContext as LeakJig);
            LeakActionWindows.ShowDialog();
        }

        private void TVOC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TVOCAction TVOCActionWindows = new TVOCAction(TVOC);
            TVOCActionWindows.ShowDialog();
        }

        private void NFCBuffer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var NFCBufferAction = new NFCBufferAction(AXIS_03_BUFFER.Jig);
            NFCBufferAction.ShowDialog();
        }


        private void Axis01Buffer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var _AXIS_01_BUFFER = new PDJig() { PackingPin = OUT.BUFFER_PACK, Jig = AXIS_01_BUFFER };
            var Buffer01Action = new Buffer01Action(_AXIS_01_BUFFER);
            Buffer01Action.ShowDialog();
        }

        private void ORG_Clicked(object sender, RoutedEventArgs e)
        {
            var ClickedButton = sender as Button;
            ClickedButton.IsEnabled = false;
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            try
            {
                if (ORGLoopCancelationTokenSource != null)
                {
                    if (!ORGLoopCancelationTokenSource.IsCancellationRequested)
                    {
                        ORGLoopCancelationTokenSource.Cancel();
                    }
                }
                ORGLoopCancelationTokenSource = new CancellationTokenSource();
                Task.Run(async () => await ORGExecution(ORGLoopCancelationTokenSource.Token));
            }
            catch (Exception)
            {

            }
            finally
            {
                ClickedButton.IsEnabled = true;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StopButton.IsEnabled = false;
            StartButton.IsEnabled = false;
            StartButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Collapsed;
            Origin = false;
            PDRUN = false;
            NFC_LEAK_RUN = false;
            CancelExecution();
            ResetButton.IsEnabled = true;
            ORGButton.IsEnabled = true;
            MainWindow.autoButton.IsEnabled = true;
            MainWindow.serialButton.IsEnabled = true;
            MainWindow.manualButton.IsEnabled = true;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (PauseButton.Content.ToString() == "Pause")
                PAUSE();
            else if (PauseButton.Content.ToString() == "Run")
                RUN();
        }

        private void PAUSE()
        {
            if (Origin)
            {
                PauseButton.Content = "Run";
                PDRUN = false;
                NFC_LEAK_RUN = false;
                NFCAlarm = false;
                PDAlarm = false;
            }
        }

        private void RUN()
        {
            if (Origin && (PDLoopExcuting || NFCLoopExcuting))
            {
                PDAlarm = false;
                if (PDReady)
                {
                    PDRUN = true;
                }
                NFCAlarm = false;
                if (NFCReady)
                {
                    NFC_LEAK_RUN = true;
                }
                if (PDReady && NFCReady)
                    PauseButton.Content = "Pause";
            }
        }

        public void CancelExecution()
        {
            if (PDLoopCancelationTokenSource != null)
                if (!PDLoopCancelationTokenSource.IsCancellationRequested)
                    PDLoopCancelationTokenSource.Cancel();
            if (ORGLoopCancelationTokenSource != null)
                if (!ORGLoopCancelationTokenSource.IsCancellationRequested)
                    ORGLoopCancelationTokenSource.Cancel();
            if (NFCLoopCancelationTokenSource != null)
                if (!NFCLoopCancelationTokenSource.IsCancellationRequested)
                    NFCLoopCancelationTokenSource.Cancel();
        }

        object PDsLockObject = new object();
        public async Task Init_Db()
        {
            await App.GPIOBoardF0.GetGPIOsState();
            await App.GPIOBoardF1.GetGPIOsState();
            await App.GPIOBoardF2.GetGPIOsState();
            using (SettingContext Db = new SettingContext())
            {
                //var _TVOC = Db.JigModels.Where(x => x.JigDescription.Contains("TVOC")).FirstOrDefault();
                //if (_TVOC != null)
                //{
                //    if (TVOC == null)
                //    {
                //        TVOC = new TVOCJig() { Jig = _TVOC, TESTPin = IN.TVOC_TEST, OKPin = IN.TVOC_OK_SIGNAL, NGPin = IN.TVOC_NG_SIGNAL, InTesting = IN.TVOC_TEST };
                //        //TVOCGrid2.DataContext = TVOC;
                //        //TVOCGrid.DataContext = TVOC;
                //        TVOC.Jig.PropertyChanged -= Jig_PropertyChanged;
                //        TVOC.Jig.PropertyChanged += Jig_PropertyChanged;
                //    }

                //    if (String.IsNullOrEmpty(Database.GetDbValue("TVOCDelay")))
                //        TVOC.Delay = 500;
                //    else
                //        TVOC.Delay = int.Parse(Database.GetDbValue("TVOCDelay"));
                //    var DbIsTurnEnable = Db.ValueSettings.Where(x => x.Key == "IsTurnEnable").FirstOrDefault();
                //    if (DbIsTurnEnable != null)
                //    {
                //        if (DbIsTurnEnable.Value == "1")
                //            TVOC.IsTurnEnable = true;
                //        else TVOC.IsTurnEnable = false;
                //        //TurnButton.DataContext = TVOC;
                //    }
                //}
                string SWITCH_SETTING = Database.GetDbValue("SWITCH_SETTING");
                if (SWITCH_SETTING != null)
                {
                    App.SW = JsonConvert.DeserializeObject<SWSetting>(SWITCH_SETTING);
                }
                ////Init TimeoutSetting
                var DbTestTimeout = Db.ValueSettings.Where(x => x.Key.Contains("TestTimeout")).FirstOrDefault();
                if (DbTestTimeout != null)
                {
                    int time = int.Parse(DbTestTimeout.Value);
                    TestTimeout = TimeSpan.FromSeconds(time);
                }
                //Init Bypass Setting
                var DbTVOCBypass = Db.ValueSettings.Where(x => x.Key.Contains("TVOCBypass")).FirstOrDefault();
                if (DbTVOCBypass != null)
                {
                    if (DbTVOCBypass.Value == "1") TVOCBypass = true;
                    else TVOCBypass = false;
                }
                var DbPDBypass = Db.ValueSettings.Where(x => x.Key.Contains("PDBypass")).FirstOrDefault();
                if (DbPDBypass != null)
                {
                    if (DbPDBypass.Value == "1") PDBypass = true;
                    else PDBypass = false;
                }
                var DbNFCBypass = Db.ValueSettings.Where(x => x.Key.Contains("NFCBypass")).FirstOrDefault();
                if (DbNFCBypass != null)
                {
                    if (DbNFCBypass.Value == "1") NFCBypass = true;
                    else NFCBypass = false;
                }
                var DbLEAKBypass = Db.ValueSettings.Where(x => x.Key.Contains("LEAKBypass")).FirstOrDefault();
                if (DbLEAKBypass != null)
                {
                    if (DbLEAKBypass.Value == "1") LEAKBypass = true;
                    else LEAKBypass = false;
                }
                var SavedAxis01Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_01_VELOCITY").FirstOrDefault();
                if (SavedAxis01Vel != null)
                {
                    Axis01Velocity = int.Parse(SavedAxis01Vel.Value);
                }
                var SavedAxis02Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_02_VELOCITY").FirstOrDefault();
                if (SavedAxis02Vel != null)
                {
                    Axis02Velocity = int.Parse(SavedAxis02Vel.Value);
                }
                var SavedAxis03Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_03_VELOCITY").FirstOrDefault();
                if (SavedAxis03Vel != null)
                {
                    Axis03Velocity = int.Parse(SavedAxis03Vel.Value);
                }
                var SavedAxis04Vel = Db.ValueSettings.Where(x => x.Key == "AXIS_04_VELOCITY").FirstOrDefault();
                if (SavedAxis04Vel != null)
                {
                    Axis04Velocity = int.Parse(SavedAxis04Vel.Value);
                }
                //Notify User
                TVOCNFC_PortID = Database.GetDbValue("TVOCNFC_PortID");
                MainPage.WriteToLog("TVOC NFC Set to " + TVOCNFC_PortID);
                SIM01_NFC_PortID = Database.GetDbValue("SIM01_NFC_PortID");
                MainPage.WriteToLog("SIM_01 NFC Set to " + SIM01_NFC_PortID);
                SIM02_NFC_PortID = Database.GetDbValue("SIM02_NFC_PortID");
                MainPage.WriteToLog("SIM_02 NFC Set to " + SIM02_NFC_PortID);
            }
            //Init PD VM --------------------------------------------------------------------------------------------------
            lock (PDsLockObject)
            {
                UCT8Mode = Database.GetDbValue("UCT8Mode") == "1" ? true : false;
                PDs = new ObservableCollection<PDJig>();
                UCT100s = new List<UCT100>();
                UCT100 UCT1 = new UCT100();
                var _PD01 = MainWindow.JigModels.Where(x => x.JigDescription == ("PD_01")).FirstOrDefault();
                if (_PD01 != null)
                {
                    PDJig PD01;
                    if (!UCT8Mode)
                        PD01 = new PDJig() { Jig = _PD01, PackingPin = OUT.PD_01_PACK, PowerPin = OUT.PD_01_POWER_PIN, SWID = 1, Channel = 1 };
                    else
                        PD01 = new PDJig() { Jig = _PD01, PackingPin = OUT.PD_01_PACK, PowerPin = OUT.PD_01_POWER_PIN, SWID = 1, Channel = 1 };
                    PD01.Jig.OKCounter = _PD01.OKCounter;
                    PDs.Add(PD01);
                    UCT1.PowerPin = OUT.PD_01_POWER_PIN;
                    UCT1.PDs.Add(PD01);
                }
                var _PD02 = MainWindow.JigModels.Where(x => x.JigDescription == ("PD_02")).FirstOrDefault();
                if (_PD02 != null)
                {
                    PDJig PD02;
                    if (!UCT8Mode)
                        PD02 = new PDJig() { Jig = _PD02, PackingPin = OUT.PD_02_PACK, PowerPin = OUT.PD_01_POWER_PIN, SWID = 1, Channel = 2 };
                    else
                        PD02 = new PDJig() { Jig = _PD02, PackingPin = OUT.PD_02_PACK, PowerPin = OUT.PD_02_POWER_PIN, SWID = 2, Channel = 1 };
                    PD02.Jig.OKCounter = _PD02.OKCounter;
                    PDs.Add(PD02);
                    UCT1.PDs.Add(PD02);
                    UCT100s.Add(UCT1);
                }
                UCT100 UCT2 = new UCT100();
                var _PD03 = MainWindow.JigModels.Where(x => x.JigDescription == "PD_03").FirstOrDefault();
                if (_PD03 != null)
                {
                    PDJig PD03;
                    if (!UCT8Mode)
                        PD03 = new PDJig() { Jig = _PD03, PackingPin = OUT.PD_03_PACK, PowerPin = OUT.PD_02_POWER_PIN, SWID = 2, Channel = 1 };
                    else
                        PD03 = new PDJig() { Jig = _PD03, PackingPin = OUT.PD_03_PACK, PowerPin = OUT.PD_03_POWER_PIN, SWID = 3, Channel = 1 };
                    PD03.Jig.OKCounter = _PD03.OKCounter;
                    PDs.Add(PD03);
                    UCT2.PowerPin = OUT.PD_02_POWER_PIN;
                    UCT2.PDs.Add(PD03);
                }
                var _PD04 = MainWindow.JigModels.Where(x => x.JigDescription == "PD_04").FirstOrDefault();
                if (_PD04 != null)
                {
                    PDJig PD04;
                    if (!UCT8Mode)
                        PD04 = new PDJig() { Jig = _PD04, PackingPin = OUT.PD_04_PACK, PowerPin = OUT.PD_02_POWER_PIN, SWID = 2, Channel = 2 };
                    else
                        PD04 = new PDJig() { Jig = _PD04, PackingPin = OUT.PD_04_PACK, PowerPin = OUT.PD_04_POWER_PIN, SWID = 4, Channel = 1 };
                    PD04.Jig.OKCounter = _PD04.OKCounter;
                    PDs.Add(PD04);
                    UCT2.PDs.Add(PD04);
                    UCT100s.Add(UCT2);
                }

                UCT100 UCT3 = new UCT100();
                var _PD05 = MainWindow.JigModels.Where(x => x.JigDescription == "PD_05").FirstOrDefault();
                if (_PD05 != null)
                {
                    PDJig PD05;
                    if (!UCT8Mode)
                        PD05 = new PDJig() { Jig = _PD05, PackingPin = OUT.PD_05_PACK, PowerPin = OUT.PD_03_POWER_PIN, SWID = 3, Channel = 1 };
                    else
                        PD05 = new PDJig() { Jig = _PD05, PackingPin = OUT.PD_05_PACK, PowerPin = OUT.PD_05_POWER_PIN, SWID = 5, Channel = 1 };
                    PD05.Jig.OKCounter = _PD05.OKCounter;
                    PDs.Add(PD05);
                    UCT3.PowerPin = OUT.PD_03_POWER_PIN;
                    UCT3.PDs.Add(PD05);
                }
                var _PD06 = MainWindow.JigModels.Where(x => x.JigDescription == "PD_06").FirstOrDefault();
                if (_PD06 != null)
                {
                    PDJig PD06;
                    if (!UCT8Mode)
                        PD06 = new PDJig() { Jig = _PD06, PackingPin = OUT.PD_06_PACK, PowerPin = OUT.PD_03_POWER_PIN, SWID = 3, Channel = 2 };
                    else
                        PD06 = new PDJig() { Jig = _PD06, PackingPin = OUT.PD_06_PACK, PowerPin = OUT.PD_06_POWER_PIN, SWID = 6, Channel = 1 };
                    PD06.Jig.OKCounter = _PD06.OKCounter;
                    PDs.Add(PD06);
                    UCT3.PDs.Add(PD06);
                    UCT100s.Add(UCT3);
                }

                UCT100 UCT4 = new UCT100();
                var _PD07 = MainWindow.JigModels.Where(x => x.JigDescription == "PD_07").FirstOrDefault();
                if (_PD07 != null)
                {
                    PDJig PD07;
                    if (!UCT8Mode)
                        PD07 = new PDJig() { Jig = _PD07, PackingPin = OUT.PD_07_PACK, PowerPin = OUT.PD_04_POWER_PIN, SWID = 4, Channel = 1 };
                    else
                        PD07 = new PDJig() { Jig = _PD07, PackingPin = OUT.PD_07_PACK, PowerPin = OUT.PD_07_POWER_PIN, SWID = 7, Channel = 1 };
                    PD07.Jig.OKCounter = _PD07.OKCounter;
                    PDs.Add(PD07);
                    UCT4.PowerPin = OUT.PD_04_POWER_PIN;
                    UCT4.PDs.Add(PD07);
                }
                var _PD08 = MainWindow.JigModels.Where(x => x.JigDescription == "PD_08").FirstOrDefault();
                if (_PD08 != null)
                {
                    PDJig PD08;
                    if (!UCT8Mode)
                        PD08 = new PDJig() { Jig = _PD08, PackingPin = OUT.PD_08_PACK, PowerPin = OUT.PD_04_POWER_PIN, SWID = 4, Channel = 2 };
                    else
                        PD08 = new PDJig() { Jig = _PD08, PackingPin = OUT.PD_08_PACK, PowerPin = OUT.PD_08_POWER_PIN, SWID = 8, Channel = 1 };
                    PD08.Jig.OKCounter = _PD08.OKCounter;
                    PDs.Add(PD08);
                    UCT4.PDs.Add(PD08);
                    UCT100s.Add(UCT4);
                }

                foreach (PDJig PD in PDs)
                {
                    PD.Jig.PropertyChanged -= Jig_PropertyChanged;
                    PD.Jig.PropertyChanged += Jig_PropertyChanged;
                }
                PDGridView.ItemsSource = PDs.Reverse();

                //--------------------------------------------------------------------------------------------------
                SIMs = new ObservableCollection<SIMJig>();
                var _SIM01 = MainWindow.JigModels.Where(x => x.JigDescription == "SIM_01").FirstOrDefault();
                if (_SIM01 != null)
                {
                    var _AXIS_01_SIM_01 = MainWindow.JigModels.Where(x => x.JigDescription == "AXIS_01_SIM_01").FirstOrDefault();
                    string SIM_01_COM = Database.GetDbValue("SIM01_NFC_PortID");
                    if (SIM_01_NFC == null)
                    {
                        SIM_01_NFC = new NFC(SIM_01_COM);
                    }
                    var SIM_01 = new SIMJig()
                    {
                        NFC = SIM_01_NFC,
                        Jig = _SIM01,
                        PackingPin = OUT.SIM_01_PACK_SOL,
                        PressPin = OUT.SIM_01_PRESS_SOL,
                        PackInSensor = IN.SIM_01_PACK_IN_SENSOR,
                        PackOutSensor = IN.SIM_01_PACK_OUT_SENSOR,
                        PressSensor = IN.SIM_01_PRESS_SENSOR,
                        ReleaseSensor = IN.SIM_01_RELEASE_SENSOR,
                        Axis01SIM = _AXIS_01_SIM_01
                    };
                    //SIM_01.StartTestLoop();
                    SIMs.Add(SIM_01);
                }
                var _SIM02 = MainWindow.JigModels.Where(x => x.JigDescription == "SIM_02").FirstOrDefault();
                if (_SIM02 != null)
                {
                    var _AXIS_01_SIM_02 = MainWindow.JigModels.Where(x => x.JigDescription == "AXIS_01_SIM_02").FirstOrDefault();
                    string SIM_02_COM = Database.GetDbValue("SIM02_NFC_PortID");
                    if (SIM_02_NFC == null)
                    {
                        SIM_02_NFC = new NFC(SIM_02_COM);
                    }
                    var SIM_02 = new SIMJig()
                    {
                        NFC = SIM_02_NFC,
                        Jig = _SIM02,
                        PackingPin = OUT.SIM_02_PACK_SOL,
                        PressPin = OUT.SIM_02_PRESS_SOL,
                        PackInSensor = IN.SIM_02_PACK_IN_SENSOR,
                        PackOutSensor = IN.SIM_02_PACK_OUT_SENSOR,
                        PressSensor = IN.SIM_02_PRESS_SENSOR,
                        ReleaseSensor = IN.SIM_02_RELEASE_SENSOR,
                        Axis01SIM = _AXIS_01_SIM_02
                    };
                    SIMs.Add(SIM_02);
                }
                SIMGridView.ItemsSource = SIMs.Reverse();
                //--------------------------------------------------------------------------------------------------
                LEAKs = new ObservableCollection<LeakJig>();
                var _LEAK_01 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("LEAK_01")).FirstOrDefault();
                if (_LEAK_01 != null)
                {
                    LeakJig LEAK01 = new LeakJig()
                    {
                        OKPIN = IN.LEAK_01_OK,
                        NGPIN = IN.LEAK_01_NG,
                        Jig = _LEAK_01,
                        PackingPin = OUT.LEAK_01_PACK,
                        PressSolenoid = OUT.LEAK_01_PRESS_SOL,
                        TransferSolenoid = OUT.LEAK_01_TRANS_SOL,
                        DownSenSor = IN.LEAK_01_PRESS_DOWN_SENSOR,
                        UpSensor = IN.LEAK_01_PRESS_UP_SENSOR,
                        ForwardSensor = IN.LEAK_01_FOWARD_SENSOR,
                        ReverseSensor = IN.LEAK_01_REVERSE_SENSOR,
                    };
                    LEAKs.Add(LEAK01);
                }
                var _LEAK_02 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("LEAK_02")).FirstOrDefault();
                if (_LEAK_02 != null)
                {
                    LeakJig LEAK02 = new LeakJig()
                    {
                        OKPIN = IN.LEAK_02_OK,
                        NGPIN = IN.LEAK_02_NG,
                        Jig = _LEAK_02,
                        PackingPin = OUT.LEAK_02_PACK,
                        PressSolenoid = OUT.LEAK_02_PRESS_SOL,
                        TransferSolenoid = OUT.LEAK_02_TRANS_SOL,
                        DownSenSor = IN.LEAK_02_PRESS_DOWN_SENSOR,
                        UpSensor = IN.LEAK_02_PRESS_UP_SENSOR,
                        ForwardSensor = IN.LEAK_02_FOWARD_SENSOR,
                        ReverseSensor = IN.LEAK_02_REVERSE_SENSOR,
                    };
                    LEAKs.Add(LEAK02);
                }
                var _LEAK_03 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("LEAK_03")).FirstOrDefault();
                if (_LEAK_03 != null)
                {
                    LeakJig LEAK03 = new LeakJig()
                    {
                        OKPIN = IN.LEAK_03_OK,
                        NGPIN = IN.LEAK_03_NG,
                        Jig = _LEAK_03,
                        PackingPin = OUT.LEAK_03_PACK,
                        PressSolenoid = OUT.LEAK_03_PRESS_SOL,
                        TransferSolenoid = OUT.LEAK_03_TRANS_SOL,
                        DownSenSor = IN.LEAK_03_PRESS_DOWN_SENSOR,
                        UpSensor = IN.LEAK_03_PRESS_UP_SENSOR,
                        ForwardSensor = IN.LEAK_03_FOWARD_SENSOR,
                        ReverseSensor = IN.LEAK_03_REVERSE_SENSOR,
                    };
                    LEAKs.Add(LEAK03);
                }
                var _LEAK_04 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("LEAK_04")).FirstOrDefault();
                if (_LEAK_04 != null)
                {
                    LeakJig LEAK04 = new LeakJig()
                    {
                        OKPIN = IN.LEAK_04_OK,
                        NGPIN = IN.LEAK_04_NG,
                        Jig = _LEAK_04,
                        PackingPin = OUT.LEAK_04_PACK,
                        PressSolenoid = OUT.LEAK_04_PRESS_SOL,
                        TransferSolenoid = OUT.LEAK_04_TRANS_SOL,
                        DownSenSor = IN.LEAK_04_PRESS_DOWN_SENSOR,
                        UpSensor = IN.LEAK_04_PRESS_UP_SENSOR,
                        ForwardSensor = IN.LEAK_04_FOWARD_SENSOR,
                        ReverseSensor = IN.LEAK_04_REVERSE_SENSOR,
                    };
                    LEAKs.Add(LEAK04);
                }
                foreach (LeakJig LK in LEAKs)
                {
                    LK.Jig.PropertyChanged -= Jig_PropertyChanged;
                    LK.Jig.PropertyChanged += Jig_PropertyChanged;
                }
                LEAKGridView.ItemsSource = LEAKs;
                ////--------------------------------------------------------------------------------------------------
                NFCs = new ObservableCollection<NFCJig>();
                var _NFC_01 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("NFC_01")).FirstOrDefault();
                if (_NFC_01 != null)
                {
                    NFCJig NFC01 = new NFCJig()
                    {
                        Jig = _NFC_01,
                        OKPIN = IN.NFC_01_OK,
                        NGPIN = IN.NFC_01_NG,
                        PackingPin = OUT.NFC_01_PACK
                    };
                    NFCs.Add(NFC01);
                }
                var _NFC_02 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("NFC_02")).FirstOrDefault();
                if (_NFC_02 != null)
                {
                    NFCJig NFC02 = new NFCJig()
                    {
                        OKPIN = IN.NFC_02_OK,
                        NGPIN = IN.NFC_02_NG,
                        Jig = _NFC_02,
                        PackingPin = OUT.NFC_02_PACK
                    };
                    NFCs.Add(NFC02);
                }
                var _NFC_03 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("NFC_03")).FirstOrDefault();
                if (_NFC_03 != null)
                {
                    NFCJig NFC03 = new NFCJig()
                    {
                        OKPIN = IN.NFC_03_OK,
                        NGPIN = IN.NFC_03_NG,
                        Jig = _NFC_03,
                        PackingPin = OUT.NFC_03_PACK
                    };
                    NFCs.Add(NFC03);
                }
                var _NFC_04 = MainWindow.JigModels.Where(x => x.JigDescription.Contains("NFC_04")).FirstOrDefault();
                if (_NFC_04 != null)
                {
                    NFCJig NFC04 = new NFCJig()
                    {
                        OKPIN = IN.NFC_04_OK,
                        NGPIN = IN.NFC_04_NG,
                        Jig = _NFC_04,
                        PackingPin = OUT.NFC_04_PACK
                    };
                    NFCs.Add(NFC04);
                }
                NFCGridView.ItemsSource = NFCs;
                foreach (NFCJig NFC in NFCs)
                {
                    NFC.Jig.PropertyChanged -= Jig_PropertyChanged;
                    NFC.Jig.PropertyChanged += Jig_PropertyChanged;
                }
                ////--------------------------------------------------------------------------------------------------
                var _AXIS_01_LOADING_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_01_LOADING_CV_POS")).FirstOrDefault();
                if (_AXIS_01_LOADING_POS != null)
                {
                    AXIS_01_LOADING_POS = _AXIS_01_LOADING_POS;
                }
                var _AXIS_01_NG_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_01_NG_POS")).FirstOrDefault();
                if (_AXIS_01_NG_POS != null)
                {
                    AXIS_01_NG_POS = _AXIS_01_NG_POS;
                }
                var _AXIS_01_BUFFER = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_01_BUFFER")).FirstOrDefault();
                if (_AXIS_01_BUFFER != null)
                {
                    AXIS_01_BUFFER = _AXIS_01_BUFFER;
                    Axis01BufferGrid.DataContext = AXIS_01_BUFFER;
                    AXIS_01_BUFFER.PropertyChanged -= Jig_PropertyChanged;
                    AXIS_01_BUFFER.PropertyChanged += Jig_PropertyChanged;
                }
                var _AXIS_01_TVOC = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_01_TVOC")).FirstOrDefault();
                if (_AXIS_01_TVOC != null)
                {
                    AXIS_01_TVOC = _AXIS_01_TVOC;
                    //AXIS_01_TVOC.PropertyChanged -= Jig_PropertyChanged;
                    //AXIS_01_TVOC.PropertyChanged += Jig_PropertyChanged;
                }
                var _AXIS_02_BUFFER = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_02_BUFFER")).FirstOrDefault();
                if (_AXIS_02_BUFFER != null)
                {
                    AXIS_02_BUFFER = _AXIS_02_BUFFER;
                }
                var _AXIS_02_TRANSFER_CV_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_02_TRANSFER_CV_POS")).FirstOrDefault();
                if (_AXIS_02_TRANSFER_CV_POS != null)
                {
                    AXIS_02_TRANSFER_CV_POS = _AXIS_02_TRANSFER_CV_POS;
                }
                var _AXIS_02_TVOC = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_02_TVOC")).FirstOrDefault();
                if (_AXIS_02_TVOC != null)
                {
                    AXIS_02_TVOC = _AXIS_02_TVOC;
                }
                var _AXIS_03_LOADING_CV_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_03_LOADING_CV_POS")).FirstOrDefault();
                if (_AXIS_03_LOADING_CV_POS != null)
                {
                    AXIS_03_LOADING_CV_POS = _AXIS_03_LOADING_CV_POS;
                }
                var _AXIS_03_BUFFER = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_03_BUFFER")).FirstOrDefault();
                if (_AXIS_03_BUFFER != null)
                {
                    AXIS_03_BUFFER = new NFCJig() { Jig = _AXIS_03_BUFFER, OKPIN = new InputPin { PinValue = PinValue.OFF }, NGPIN = new InputPin { PinValue = PinValue.OFF } };
                    _AXIS_03_BUFFER.PropertyChanged += Jig_PropertyChanged;
                    NFCBufferGrid1.DataContext = AXIS_03_BUFFER;
                    NFCBufferGrid2.DataContext = AXIS_03_BUFFER;
                }
                var _AXIS_04_BUFFER = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_04_BUFFER")).FirstOrDefault();
                if (_AXIS_04_BUFFER != null)
                {
                    AXIS_04_BUFFER = _AXIS_04_BUFFER;
                }
                var _AXIS_04_UNLOADING_CV_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_04_UNLOADING_CV_POS")).FirstOrDefault();
                if (_AXIS_04_UNLOADING_CV_POS != null)
                {
                    AXIS_04_UNLOADING_CV_POS = _AXIS_04_UNLOADING_CV_POS;
                }
                var _AXIS_04_NG_POS = MainWindow.JigModels.Where(x => x.JigDescription.Contains("AXIS_04_NG_POS")).FirstOrDefault();
                if (_AXIS_04_NG_POS != null)
                {
                    AXIS_04_NG_POS = _AXIS_04_NG_POS;
                }
                //TVOC Event
                //TVOC.OKPin.OnPinValueChanged += TVOCPin_PinValueChanged;
                //TVOC.NGPin.OnPinValueChanged += TVOCPin_PinValueChanged;
                //TVOC.TESTPin.OnPinValueChanged += TVOCPin_PinValueChanged;
                //Get NFC Test Result from PinValue
                foreach (NFCJig NFC in NFCs)
                {
                    NFC.OKPIN.OnPinValueChanged += NFCPINs_OnPinValueChanged;
                    NFC.NGPIN.OnPinValueChanged += NFCPINs_OnPinValueChanged;
                }
                //Get Leak Test Result from PinValue
                foreach (LeakJig Leak in LEAKs)
                {
                    Leak.OKPIN.OnPinValueChanged += LEAKPINs_OnPinValueChanged;
                    Leak.NGPIN.OnPinValueChanged += LEAKPINs_OnPinValueChanged;
                }

                string Str_SavedNVValue = Database.GetDbValue("NVValues");
                if (!string.IsNullOrEmpty(Str_SavedNVValue))
                {
                    var SavedNVValues = JsonConvert.DeserializeObject<ObservableCollection<NVValue>>(Str_SavedNVValue);
                    if (SavedNVValues != null)
                        NVValues = SavedNVValues;
                }
                else
                    NVValues = new ObservableCollection<NVValue>();
                DataContext = this;
                IN.START_BUTTON_01.OnPinValueChanged += Start_Stop_OnPinValueChanged;
                IN.START_BUTTON_02.OnPinValueChanged += Start_Stop_OnPinValueChanged;
                IN.STOP_BUTTON_01.OnPinValueChanged += Start_Stop_OnPinValueChanged;
                IN.STOP_BUTTON_02.OnPinValueChanged += Start_Stop_OnPinValueChanged;
                IN.ESTOP_BUTTON_01.OnPinValueChanged += ESTOP_BUTTON_OnPinValueChanged;
                IN.ESTOP_BUTTON_02.OnPinValueChanged += ESTOP_BUTTON_OnPinValueChanged;
            }
            if (OnStart)
            {
                OnStart = false;
                UnpackJig();
            }
            if (PDBypass && TVOCBypass && NFCBypass && LEAKBypass)
            {
                await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.StepEnable, new byte[] { 0x00 });
                await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.StepEnable, new byte[] { 0x00 });
                await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.StepEnable, new byte[] { 0x00 });
                await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.StepEnable, new byte[] { 0x00 });
            }
            if (PDBypass)
                foreach (PDJig PD in PDs)
                {
                    await PD.PowerPin.SET();
                }

            else
                foreach (PDJig PD in PDs)
                {
                    await PD.PowerPin.RST();
                }
        }

        private void ESTOP_BUTTON_OnPinValueChanged(object sender, PinValueChangedEventArgs e)
        {
            if (e.Edge == Edge.Fall)
                Dispatcher.Invoke(() =>
                {
                    AlarmWindow.AlarmListViewItem.Add($"<{Now}>: ESTOP BUTTON PRESSED ({((GPIOPin)sender).GPIOLabel})");
                });
        }

        private void DeinitDB()
        {
            try
            {
                foreach (NFCJig NFC in NFCs)
                {
                    NFC.OKPIN.OnPinValueChanged -= NFCPINs_OnPinValueChanged;
                    NFC.NGPIN.OnPinValueChanged -= NFCPINs_OnPinValueChanged;
                }
                //Get Leak Test Result from PinValue
                foreach (LeakJig Leak in LEAKs)
                {
                    Leak.OKPIN.OnPinValueChanged -= LEAKPINs_OnPinValueChanged;
                    Leak.NGPIN.OnPinValueChanged -= LEAKPINs_OnPinValueChanged;
                }
                AXIS_03_BUFFER.Jig.JigDescription = "AXIS_03_BUFFER";
                AXIS_04_BUFFER.JigDescription = "AXIS_04_BUFFER";
            }
            catch
            {

            }
        }

        private async Task Repack(LeakJig Leak)
        {
            await Leak.PackingPin.RST();
            await Task.Delay(2000);
            await Leak.PackingPin.SET();
        }
        private async Task Repack(NFCJig NFC)
        {
            await NFC.PackingPin.RST();
            await Task.Delay(2000);
            await NFC.PackingPin.SET();
        }

        private async void LEAKPINs_OnPinValueChanged(object sender, PinValueChangedEventArgs e)
        {
            if (e.Edge == Edge.Rise /*&& NFCLoopExcuting*/)
                foreach (LeakJig Leak in LEAKs)
                {
                    if (Leak.Jig.TestResult != TestResult.NOT_USE &&
                        Leak.Jig.TestResult != TestResult.ABORTED &&
                        Leak.Jig.TestResult == TestResult.TEST &&
                        Leak.Jig.IsSetInJig && Leak.ForwardSensor.PinValue == PinValue.ON)
                        if ((InputPin)sender == Leak.OKPIN)
                        {
                            Leak.TestCount = 0;
                            await Leak.ReleaseTask();
                            Leak.Jig.TestResult = TestResult.PASS;
                            break;
                        }
                        else if ((InputPin)sender == Leak.NGPIN)
                        {
                            if (Leak.Jig.TestResult != TestResult.FAIL)
                            {
                                Leak.TestCount++;
                                if (Leak.TestCount >= 2)
                                {
                                    Leak.TestCount = 0;
                                    await Leak.ReleaseTask();
                                    Leak.Jig.TestResult = TestResult.FAIL;
                                }
                                else
                                {
                                    if (NFCLoopExcuting)
                                    {
                                        Leak.Jig.TestResult = TestResult.REPACK;
                                        await Leak.ReleaseTask();
                                        await Task.Delay(500);
                                        await Leak.PressTask();
                                        Leak.Jig.StatTime = DateTime.Now;
                                        Leak.Jig.TestResult = TestResult.TEST;
                                    }
                                    else if (!NFCLoopExcuting)
                                    {
                                        Leak.Jig.TestResult = TestResult.FAIL;
                                    }
                                }
                            }
                            break;
                        };
                }
        }

        private async void NFCPINs_OnPinValueChanged(object sender, PinValueChangedEventArgs e)
        {
            if (e.Edge == Edge.Rise /*&& NFCLoopExcuting*/)
                foreach (NFCJig NFC in NFCs)
                {
                    if (NFC.Jig.TestResult != TestResult.NOT_USE &&
                        NFC.Jig.TestResult != TestResult.ABORTED &&
                        NFC.Jig.IsSetInJig && NFC.Jig.TestResult == TestResult.TEST)
                        if ((InputPin)sender == NFC.OKPIN)
                        {
                            NFC.TestCount = 0;
                            await NFC.PackingPin.RST();
                            await Task.Delay(500);
                            NFC.Jig.TestResult = TestResult.PASS;
                            break;
                        }
                        else if ((InputPin)sender == NFC.NGPIN)
                        {
                            if (NFC.Jig.TestResult != TestResult.FAIL)
                            {

                                NFC.TestCount++;
                                if (NFC.TestCount >= 2)
                                {
                                    NFC.TestCount = 0;
                                    await NFC.PackingPin.RST();
                                    await Task.Delay(500);
                                    NFC.Jig.TestResult = TestResult.FAIL;
                                }
                                else
                                {
                                    NFC.Jig.TestResult = TestResult.REPACK;
                                    await Repack(NFC);
                                    NFC.Jig.StatTime = DateTime.Now;
                                    NFC.Jig.TestResult = TestResult.TEST;
                                }
                            }
                            break;
                        };
                }
        }

        //SemaphoreSlim TVOCLockObject = new SemaphoreSlim(1, 1);
        ////int TVOCMatchingCount = 5;
        //private async void TVOCPin_PinValueChanged(object sender, PinValueChangedEventArgs e)
        //{
        //    await TVOCLockObject.WaitAsync();
        //    await Task.Run(async () =>
        //    {
        //        if (e.Edge == Edge.Rise && TVOC.Jig.TestResult == TestResult.TEST)
        //        {
        //            try//lock (TVOCLockObject)
        //            {
        //                if ((InputPin)sender == TVOC.OKPin)
        //                {
        //                    TVOCNFC = new NFC(TVOCNFC_PortID);
        //                    if (!TVOCNFC.Set_NV(253, "P")) MainPage.WriteToLog("TVOC matching error");
        //                    await Task.Delay(TVOC.Delay);
        //                    TVOC.Jig.TestResult = TestResult.PASS;
        //                }
        //                else if ((InputPin)sender == TVOC.TESTPin)
        //                {
        //                    if (e.Edge == Edge.Rise)
        //                    {
        //                        TVOCNFC = new NFC(TVOCNFC_PortID);
        //                        if (!TVOCNFC.Set_NV(253, "P")) MainPage.WriteToLog("TVOC matching error");
        //                    }
        //                }
        //                else if ((InputPin)sender == TVOC.NGPin)
        //                {
        //                    if (e.Edge == Edge.Rise)
        //                    {
        //                        if (TVOCNFC != null)
        //                        {
        //                            TVOCNFC = new NFC(TVOCNFC_PortID);
        //                            if (!TVOCNFC.Set_NV(253, "F")) MainPage.WriteToLog("TVOC matching error");
        //                            await Task.Delay(TVOC.Delay);
        //                            TVOC.Jig.TestResult = TestResult.FAIL;
        //                        }
        //                    }
        //                }
        //            }
        //            catch
        //            {

        //            }
        //            finally
        //            {
        //                TVOCNFC.NFC_Closeport();
        //            }
        //        }
        //    });
        //    TVOCLockObject.Release();
        //}

        private void Jig_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsJigEnable")
            {
                var Jig = sender as JigModel;
                using (var Db = new SettingContext())
                {
                    var SavedJig = Db.JigModels.Where(x => x.JigModelID == Jig.JigModelID).FirstOrDefault();
                    SavedJig.IsJigEnable = Jig.IsJigEnable;
                    Db.SaveChanges();
                }
            }
        }

        private async Task<bool> MoveAbs(AxisStatus Axis, int Pos, int Vel)
        {
            var motioning = await IsAxisMotioning(Axis);
            var axiscurrentpos = await App.ServoCOM.StepGetdata(Axis.SlaveID, Flag.GetActualPosition, null);
            if (axiscurrentpos != null)
                Axis.CurrentPos = BitConverter.ToInt32(axiscurrentpos, 3);
            if (!motioning && (Pos != Axis.CurrentPos) && Axis.ALM == PinValue.OFF)
                await App.ServoCOM.StepGetdata(Axis.SlaveID, Flag.MoveSingleAxisAbs, DataFrame.MoveAbsIncData(Pos, Vel));
            return ((Pos == Axis.CurrentPos /*|| (Pos >= Axis.CurrentPos - 100 && Pos <= Axis.CurrentPos + 100)*/) && (!motioning/*Axis.Motioning == PinValue.OFF*/));
        }

        private async Task<bool> IsAxisMotioning(AxisStatus axis)
        {
            var motioning = await App.ServoCOM.StepGetdata(axis.SlaveID, Flag.GetAxisStatus, null);
            if (motioning != null)
            {
                axis.Status = BitConverter.ToInt32(motioning, 3);
                return (axis.Motioning == PinValue.ON);
            }
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        TimeSpan _TestTimeout = TimeSpan.FromSeconds(180);
        public TimeSpan TestTimeout
        {
            get { return _TestTimeout; }
            set
            {
                _TestTimeout = value;
                using (var Db = new SettingContext())
                {
                    var SaveTimeout = Db.ValueSettings.Where(x => x.Key.Contains("TestTimeout")).FirstOrDefault();
                    if (SaveTimeout != null)
                    {
                        SaveTimeout.Value = TestTimeout.TotalSeconds.ToString();
                        Db.SaveChanges();
                    }
                }
            }
        }

        bool nFCLoopExcuting = false;
        public bool NFCLoopExcuting
        {
            get { return nFCLoopExcuting; }
            set { nFCLoopExcuting = value; NotifyPropertyChanged("NFCLoopExcuting"); }
        }

        bool pDLoopExcuting = false;
        public bool PDLoopExcuting { get { return pDLoopExcuting; } set { pDLoopExcuting = value; NotifyPropertyChanged("PDLoopExcuting"); } }

        public static CancellationTokenSource PDLoopCancelationTokenSource = null;
        public static CancellationTokenSource ReadUCTCancelationTokenSource = null;
        public static CancellationTokenSource NFCLoopCancelationTokenSource = null;
        public static CancellationTokenSource ORGLoopCancelationTokenSource = null;
        public static CancellationTokenSource MainLoopCancelationTokenSource = null;
        public static CancellationTokenSource ServoLoopCancelationTokenSource = null;
        //Work POS---------------------------------------------------------------------------------------
        JigModel AXIS_01_LOADING_POS, AXIS_01_NG_POS, AXIS_01_BUFFER, AXIS_01_TVOC;
        JigModel AXIS_02_BUFFER, AXIS_02_TRANSFER_CV_POS, AXIS_02_TVOC;
        JigModel AXIS_03_LOADING_CV_POS;
        JigModel AXIS_04_BUFFER, AXIS_04_UNLOADING_CV_POS, AXIS_04_NG_POS;
        NFCJig AXIS_03_BUFFER;
        public static bool UCT8Mode { get; set; }

        //PD ViewModel-----------------------------------------------------------------------------------
        public ObservableCollection<PDJig> PDs { get; set; } = new ObservableCollection<PDJig>();
        List<UCT100> UCT100s = new List<UCT100>();
        //SIMModel
        public ObservableCollection<SIMJig> SIMs { get; set; } = new ObservableCollection<SIMJig>();
        //LeakViewModel----------------------------------------------------------------------------------
        public ObservableCollection<LeakJig> LEAKs { get; set; } = new ObservableCollection<LeakJig>();
        //NFC ViewModel----------------------------------------------------------------------------------
        public ObservableCollection<NFCJig> NFCs { get; set; } = new ObservableCollection<NFCJig>();

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ClickedObject = sender as Grid;
            ClickedObject.IsEnabled = false;
            var ClickedPD = ClickedObject.DataContext as PDJig;
            var PDAction = new PDAction(ClickedPD) { UCTs = UCT100s };
            PDAction.ShowDialog();
            ClickedObject.IsEnabled = true;
        }

        private PDJig movedPDJig()
        {
            UCTStatus Status = UCTStatus.STOP;
            PDJig PDJig = null;
            PDJig ReadyPD = null;
            //PD bypass
            if (PDBypass) return new PDJig() { Jig = AXIS_01_LOADING_POS };
            foreach (UCT100 UCT in UCT100s)
            {
                foreach (PDJig PD in UCT.PDs)
                {
                    if (PD.Jig.IsJigEnable == true && !PD.Jig.IsSetInJig &&
                        PD.PackingPin.PinValue != PinValue.ON && !UCT.ResetFlag &&
                        PD.Jig.TestResult == TestResult.READY && PD.Jig.JigState != UCTStatus.NORESP)
                    {
                        ReadyPD = PD;
                        break;
                    }
                }
            }
            //if (ReadyPD != null)
            //    return PDJig;
            //else
            //if (AXIS_01_STEP == 0)
            //{
            //    foreach (PDJig PD in PDs)
            //    {
            //        if (PD.Jig.IsSetInJig && /*(PD != PDs[7] || !AXIS_01_BUFFER.IsJigEnable) &&*/ PD.Jig.TestResult == TestResult.PASS)
            //        {
            //            return PD;
            //        }
            //    }
            //    foreach (PDJig PD in PDs)
            //    {
            //        if (PD.Jig.IsSetInJig && /*(PD != PDs[7] || !AXIS_01_BUFFER.IsJigEnable) &&*/ PD.Jig.IsJigEnable && PD.Jig.TestResult == TestResult.FAIL)
            //        {
            //            return PD;
            //        }
            //    }
            //    if (ReadyPD != null && (Input_CV_Debounce == 0 || InputCVTimer > 0))
            //        return new PDJig() { Jig = AXIS_01_LOADING_POS };
            //}
            //else
            if ((AXIS_01_STEP == 1 || AXIS_01_STEP == 2))
            {
                if (ReadyPD != null && (Input_CV_Delay == 0 || InputCVTimer > 0))
                    return new PDJig() { Jig = AXIS_01_LOADING_POS };
                foreach (PDJig PD in PDs)
                {
                    if (PD.Jig.IsSetInJig && /*(PD != PDs[7] || !AXIS_01_BUFFER.IsJigEnable) && */PD.Jig.TestResult == TestResult.PASS)
                    {
                        return PD;
                    }
                }
                foreach (PDJig PD in PDs)
                {
                    if (PD.Jig.IsSetInJig /*&& (PD != PDs[7] || !AXIS_01_BUFFER.IsJigEnable)*/ && PD.Jig.IsJigEnable && PD.Jig.TestResult == TestResult.FAIL)
                    {
                        return PD;
                    }
                }
            }
            foreach (PDJig PD in PDs)
            {
                if (Status < PD.Jig.JigState && PD.Jig.IsSetInJig &&
                    PD.Jig.IsJigEnable /*&& (PD != PDs[7] || !AXIS_01_BUFFER.IsJigEnable)*/)
                {
                    Status = PD.Jig.JigState;
                    PDJig = PD;
                }
            }
            return PDJig;
        }
        ////-----------------------------------------------------------------------------------------------
        public TVOCJig TVOC { get; set; }
        int Axis01Velocity, Axis02Velocity, Axis03Velocity, Axis04Velocity;
        public static int AXIS_01_STEP { get; set; } = 0;
        public static int AXIS_02_STEP { get; set; } = 0;
        public int Buff_SIM_Step { get; set; } = 0;
        public int SIM_TRANS_CV_STEP { get; set; } = 0;
        int PD_Buf_Step = 0, SIM_NG_Step = 0;
        int PDStep = 0, PDRetestStep = 0, LeakFinalStep, ORGStep = 0;
        private int AXIS_03_STEP, Buf_Leak_Step, CV_NFC_Step, AXIS_04_STEP;
        AlarmWindow ALMWindows = new AlarmWindow();
        private async void ResetButton_Clicked(object sender, RoutedEventArgs e)
        {
            //if (ALMWindows.Visibility != Visibility.Visible)
            //    ALMWindows.Show();
            //else
            //    ALMWindows.Hide();
            //return;
            PDAlarm = false;
            NFCAlarm = false;
            if (Axis._01.ALM == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.Reset, null);
            if (Axis._02.ALM == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.Reset, null);
            if (Axis._03.ALM == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.Reset, null);
            if (Axis._04.ALM == PinValue.ON)
                await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.Reset, null);
            if (Axis._01.SON == PinValue.OFF)
                await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.StepEnable, new byte[] { 0x01 });
            if (Axis._02.SON == PinValue.OFF)
                await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.StepEnable, new byte[] { 0x01 });
            if (Axis._03.SON == PinValue.OFF)
                await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.StepEnable, new byte[] { 0x01 });
            if (Axis._04.SON == PinValue.OFF)
                await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.StepEnable, new byte[] { 0x01 });
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (IN.ESTOP_BUTTON_01.PinValue == PinValue.OFF || IN.ESTOP_BUTTON_02.PinValue == PinValue.OFF)
            {
                MessageBox.Show("CHECK E_STOP BUTTON");
                return;
            }
            if (IN.DOOR1_SENSOR.PinValue == PinValue.OFF || IN.DOOR2_SENSOR.PinValue == PinValue.OFF)
            {
                MessageBox.Show("CHECK DOOR SENSOR");
                return;
            }
            else if (NFCReady && PDReady)
            {
                StartButton.Visibility = Visibility.Collapsed;
                PauseButton.Visibility = Visibility.Visible;
                StopButton.IsEnabled = true;
                MainWindow.autoButton.IsEnabled = false;
                MainWindow.serialButton.IsEnabled = false;
                MainWindow.manualButton.IsEnabled = false;
                ResetButton.IsEnabled = false;
                ORGButton.IsEnabled = false;
                PDRUN = true;
                NFC_LEAK_RUN = true;
                BeginExecution();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lock (Times)
            {
                Times.Clear();
                TotalOK = 0;
                TotalFail = 0;
                TotalPD = 0;
                TotalReTest = 0;
            }
        }

        int Input_CV_Delay = 5, Trans_CV_Debounce = 5;
        private bool PDReady = false, NFCReady = false;

        public static bool Origin;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private async Task TVOCStart()
        {
            if (TVOC.Jig.TestResult == TestResult.READY)
            {
                await OUT.TVOC_START.SET();
                await Task.Delay(50);
                await OUT.TVOC_START.RST();
                TVOC.Jig.TestResult = TestResult.TEST;
                if (TVOC.IsTurnEnable && IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.ON
                    && IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                    await OUT.LOADING_02_TURN_SOL.SET();
            }
        }

        private bool pDRUN;
        public bool PDRUN { get { return pDRUN; } set { pDRUN = value; NotifyPropertyChanged("PDRUN"); } }
        bool IsReversed;
        private void Reverse(object sender, RoutedEventArgs e)
        {
            if (!IsReversed)
            {
                IsReversed = true;
                LEAKGridView.ItemsSource = NFCs.Reverse();
                NFCGridView.ItemsSource = LEAKs.Reverse();
            }
            else
            {
                IsReversed = false;
                LEAKGridView.ItemsSource = LEAKs;
                NFCGridView.ItemsSource = NFCs;
            }
        }

        private bool nFC_LEAK_RUN;
        public bool NFC_LEAK_RUN { get { return nFC_LEAK_RUN; } set { nFC_LEAK_RUN = value; NotifyPropertyChanged("NFC_LEAK_RUN"); } }
        bool _NFCBypass;
        public bool NFCBypass
        {
            get
            {
                return _NFCBypass;
            }
            set
            {
                if (_NFCBypass != value)
                {
                    _NFCBypass = value;
                }
            }
        }
        bool _LEAKBypass;
        public bool LEAKBypass
        {
            get
            {
                return _LEAKBypass;
            }
            set
            {
                if (_LEAKBypass != value)
                {
                    _LEAKBypass = value;
                }
            }
        }


        bool _TVOCBypass;
        public bool TVOCBypass
        {
            get
            {
                return _TVOCBypass;
            }
            set
            {
                if (_TVOCBypass != value)
                {
                    _TVOCBypass = value;
                }
            }
        }
        bool _PDBypass;
        private int NFC_Buf_Step;
        public bool PDBypass
        {
            get
            {
                return _PDBypass;
            }
            set
            {
                if (_PDBypass != value)
                {
                    _PDBypass = value;
                }
            }
        }
        private DateTime PreCycleTime = DateTime.Now;
        private TimeSpan _CycleTime;
        public TimeSpan CycleTime { get { return _CycleTime; } set { _CycleTime = value; NotifyPropertyChanged("CycleTime"); } }
        private int _TotalOK;
        public int TotalOK
        {
            get
            {
                return _TotalOK;
            }
            set
            {
                if (_TotalOK != value)
                {
                    _TotalOK = value;
                    NotifyPropertyChanged("TotalOK");
                }
            }
        }
        private int _TotalFail;
        public int TotalFail
        {
            get
            {
                return _TotalFail;
            }
            set
            {
                if (_TotalFail != value)
                {
                    _TotalFail = value;
                    NotifyPropertyChanged("TotalFail");
                }
            }
        }
        private double _AVTime;
        public double AVTime
        {
            get
            {
                return _AVTime;
            }
            set
            {
                if (_AVTime != value)
                {
                    _AVTime = value;
                    NotifyPropertyChanged("AVTime");
                }
            }
        }
        int InputCVRun1 = 0, InputCVRun2 = 0, NGCVRun = 0, OutputCVRun = 30, TransCVRun = 0;
        int InputCVTimer = 0;
        private int _TotalPD;
        public int TotalPD
        {
            get
            {
                return _TotalPD;
            }
            set
            {
                if (_TotalPD != value)
                {
                    _TotalPD = value;
                    if (_TotalPD > 0)
                        RetestRate = ((float)(_TotalReTest * 1000 / _TotalPD)).ToString("0.0") + "%";
                    NotifyPropertyChanged("TotalPD");
                }
            }
        }
        private int _TotalReTest;
        public int TotalReTest
        {
            get
            {
                return _TotalReTest;
            }
            set
            {
                if (_TotalReTest != value)
                {
                    _TotalReTest = value;
                    NotifyPropertyChanged("TotalReTest");
                }
            }
        }

        string _RetestRate;
        public string RetestRate
        {
            get
            {
                return _RetestRate;
            }
            set
            {
                if (_RetestRate != value)
                {
                    _RetestRate = value;
                    NotifyPropertyChanged("RetestRate");
                }
            }
        }
        public static NFC TVOCNFC;
        string TVOCNFC_PortID, SIM01_NFC_PortID, SIM02_NFC_PortID;
        NFC SIM_01_NFC, SIM_02_NFC;
        private ObservableCollection<NVValue> NVValues;
        private const PinValue OFF = PinValue.OFF;
        private const PinValue ON = PinValue.ON;
    }

    public class TextLog : INotifyPropertyChanged
    {
        private string _Text;
        public string Text { get { return _Text; } set { _Text = value; NotifyPropertyChanged("Text"); } }
        //private SolidColorBrush _Color = new SolidColorBrush(Colors.Black);
        //public SolidColorBrush Color { get { return _Color; } set { _Color = value; NotifyPropertyChanged("Color"); } }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
