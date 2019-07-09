using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GPIOCommunication.GPIOHelper;

namespace PDTESTER
{
    public class FreeMode
    {
        public FreeMode()
        {

        }

        Random N = new Random();
        PDJig ReadyPD = null, MovedPDJig;
        PDJig TestedPD = null;
        TestResult TestedPDResult = TestResult.TEST;
        private async Task PDLoopExecution(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(50);
                    if (IN.ESTOP_BUTTON_01.PinValue == PinValue.ON &&
                        IN.ESTOP_BUTTON_02.PinValue == PinValue.ON &&
                        IN.DOOR1_SENSOR.PinValue == PinValue.ON &&
                        Axis._01.SON == PinValue.ON &&
                        Axis._02.SON == PinValue.ON)
                        PDReady = true;
                    else
                    {
                        PDReady = false;
                        PDRUN = false;//PD Loop is stopped if NFC_LEAK_RUN = false;
                    }
                    token.ThrowIfCancellationRequested();
                    if (PDRUN)
                    {
                        //**************************INPUT_CV TO PD_JIG STEP**************************//
                        //if (true)
                        //{
                        if (PD_TVOC_STEP == 0)
                        {
                            if (PDStep == 0)
                            {
                                ReadyPD = PDs[N.Next(0, 4)];
                                PDStep = 1;
                            }
                            if (PDStep == 1)
                            {
                                PDStep = 2;
                            }
                            if (PDStep == 0 || PDStep == 1) { PDStep = 0; PD_TVOC_STEP = 1; }
                            //Move to Loading Pos
                            if (PDStep == 2)
                            {
                                if (await MoveAbs(Axis._01, AXIS_01_LOADING_POS.JigPos, Axis01Velocity))
                                    PDStep = 3;
                            }
                            //Lift 01 Down
                            if (PDStep == 3)
                            {
                                await OUT.LOADING_01_LIFT_SOL.SET();
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    PDStep = 4;
                                }
                            }
                            //Clamp
                            if (PDStep == 4)
                            {
                                await OUT.LOADING_01_CLAMP_SOL.SET();
                                await OUT.LOADING_01_UNCLAMP_SOL.RST();
                                if (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    PDStep = 5;
                                }
                            }
                            if (PDStep == 5)
                            {
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (IN.LOADING_01_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        PDStep = 6;
                                    }
                                }
                            }
                            if (PDStep == 6)
                            {
                                if (await MoveAbs(Axis._01, ReadyPD.Jig.JigPos, Axis01Velocity))
                                    PDStep = 7;
                            }
                            if (PDStep == 7)
                            {
                                //Lift Down
                                await OUT.LOADING_01_LIFT_SOL.SET();
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    PDStep = 8;
                                }
                            }
                            if (PDStep == 8)
                            {
                                //Unclamp
                                await OUT.LOADING_01_CLAMP_SOL.RST();
                                await OUT.LOADING_01_UNCLAMP_SOL.SET();
                                if (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                {
                                    PDStep = 9;
                                }
                            }
                            if (PDStep == 9)
                            {   //Lift Up
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    PDStep = 10;
                                }
                            }
                            if (PDStep == 10)
                            {
                                await ReadyPD.PackingPin.SET();
                                PDStep = 11;
                            }
                            if (PDStep == 11)
                            {
                                PDStep = 12;
                            }
                            if (PDStep == 12)
                            {
                                if (await MoveAbs(Axis._01, AXIS_01_LOADING_POS.JigPos, Axis01Velocity))
                                    PDStep = 13;
                            }
                            if (PDStep == 13)
                            {
                                PDStep = 0;
                                PD_TVOC_STEP = 1;
                            }
                        }
                        if (PD_TVOC_STEP == 1)
                        {   //Loop for TestJig
                            if (PD_Buf_Step == 0)
                            {
                                TestedPD = ReadyPD;
                                if (TestedPD != null)
                                    PD_Buf_Step = 1;
                                if (N.Next(0, 100) < 80)
                                    TestedPDResult = TestResult.PASS;
                                else
                                    TestedPDResult = TestResult.FAIL;
                            }
                            if (PD_Buf_Step == 1)
                            {
                                if (TestedPDResult == TestResult.PASS)
                                {
                                    if (!AXIS_01_BUFFER.IsSetInJig)
                                    {
                                        PD_Buf_Step = 2;
                                    }
                                }
                                else
                                if (TestedPDResult == TestResult.FAIL)
                                    PD_Buf_Step = 2;
                            }
                            if (PD_Buf_Step == 0 || PD_Buf_Step == 1)
                            {
                                PD_Buf_Step = 0;
                                PD_TVOC_STEP = 0;
                            }
                            //RUN-------------------------------------------------------------
                            if (PD_Buf_Step == 2)
                            {//Move to Tested PD jig
                                if (await MoveAbs(Axis._01, TestedPD.Jig.JigPos, Axis01Velocity))
                                    PD_Buf_Step = 3;
                            }
                            if (PD_Buf_Step == 3)
                            {//Lift 01 Down
                                await OUT.LOADING_01_LIFT_SOL.SET();
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (TestedPD.PackingPin != null)
                                    {
                                        await TestedPD.PackingPin.RST();
                                    }
                                    PD_Buf_Step = 4;
                                }
                            }
                            if (PD_Buf_Step == 4)
                            {//Clamp
                                await OUT.LOADING_01_CLAMP_SOL.SET();
                                await OUT.LOADING_01_UNCLAMP_SOL.RST();
                                if (IN.LOADING_01_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    PD_Buf_Step = 5;
                                }
                            }
                            if (PD_Buf_Step == 5)
                            {
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    PD_Buf_Step = 6;
                                    TestedPD.Jig.IsSetInJig = false;
                                }
                            }
                            if (PD_Buf_Step == 6)
                            {
                                if (TestedPDResult == TestResult.PASS)
                                {
                                    if (Axis._02.CurrentPos <= AXIS_02_TVOC.JigPos || (BufTVOC_Step >= 6 && Axis._02.Motioning == PinValue.ON))
                                    {
                                        if (await MoveAbs(Axis._01, AXIS_01_BUFFER.JigPos, Axis01Velocity))
                                        {
                                            PD_Buf_Step = 7;
                                        }
                                    }
                                }
                                else if (TestedPDResult == TestResult.FAIL)
                                {
                                    if (await MoveAbs(Axis._01, AXIS_01_NG_POS.JigPos, Axis01Velocity))
                                    {
                                        PD_Buf_Step = 7;
                                    }
                                }
                            }
                            if (PD_Buf_Step == 7)
                            {
                                //Lift Down
                                if (TestedPDResult == TestResult.PASS)
                                {
                                    await OUT.LOADING_01_LIFT_SOL.SET();
                                    AXIS_01_BUFFER.IsSetInJig = true;
                                }
                                else if (TestedPDResult == TestResult.FAIL)
                                {
                                    await OUT.LOADING_01_LIFT_SOL.SET();
                                }
                                if (IN.LOADING_01_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    PD_Buf_Step = 8;
                                }
                            }
                            if (PD_Buf_Step == 8)
                            {
                                //Unclamp
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
                                await OUT.LOADING_01_LIFT_SOL.RST();
                                if (IN.LOADING_01_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    PD_Buf_Step = 10;
                                    TestedPD.Jig.TestResult = TestResult.READY;
                                }
                            }
                            if (PD_Buf_Step == 10)
                            {
                                PD_Buf_Step = 11;
                            }
                            if (PD_Buf_Step == 11)
                            {
                                if (await MoveAbs(Axis._01, AXIS_01_LOADING_POS.JigPos, Axis01Velocity))
                                    PD_Buf_Step = 12;
                            }
                            if (PD_Buf_Step == 12)
                            {
                                PD_Buf_Step = 0;
                                PD_TVOC_STEP = 0;
                            }
                        }
                        if (BUFFER_TVOC_STEP == 0)
                        {
                            if (BufTVOC_Step == 0 && AXIS_01_BUFFER.IsSetInJig &&
                                (Axis._01.CurrentPos <= PDs[7].Jig.JigPos || (PD_Buf_Step >= 11 && Axis._01.Motioning == PinValue.ON))
                                && !TVOC.Jig.IsSetInJig)
                                BufTVOC_Step = 1;
                            else
                            if (BufTVOC_Step == 0) BUFFER_TVOC_STEP = 1;
                            //RUN
                            if (BufTVOC_Step == 1)
                            {
                                await OUT.LOADING_02_TURN_SOL.RST();
                                if (await MoveAbs(Axis._02, AXIS_02_BUFFER.JigPos, Axis02Velocity))
                                {
                                    BufTVOC_Step = 2;
                                }
                            }
                            if (BufTVOC_Step == 2)
                            {
                                if (IN.LOADING_02_RETURN_SENSOR.PinValue == PinValue.ON)
                                    await OUT.LOADING_02_LIFT_SOL.SET();
                                if (IN.LOADING_02_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    BufTVOC_Step = 3;
                                    //await OUT.BUFFER_PACK.RST();
                                    //await Task.Delay(200);
                                }
                            }
                            //Clamp
                            if (BufTVOC_Step == 3)
                            {
                                await OUT.LOADING_02_CLAMP_SOL.SET();
                                await OUT.LOADING_02_UNCLAMP_SOL.RST();
                                if (IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    BufTVOC_Step = 4;
                                }
                            }
                            if (BufTVOC_Step == 4)
                            {   //Lift 02 Up
                                await OUT.LOADING_02_LIFT_SOL.RST();
                                if (IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    BufTVOC_Step = 5;
                                }
                            }
                            if (BufTVOC_Step == 5)
                            {   //TURN
                                if (TVOC.IsTurnEnable)
                                {
                                    if (Axis._01.CurrentPos <= PDs[6].Jig.JigPos)
                                    {
                                        if (!(ReadyPD == PDs[7] && PDStep == 6))
                                            BufTVOC_Step = 6;
                                        //await OUT.LOADING_02_TURN_SOL.SET();
                                    }
                                    //if (IN.LOADING_02_TURN_SENSOR.PinValue == PinValue.ON)
                                }
                                else if (IN.LOADING_02_RETURN_SENSOR.PinValue == PinValue.ON)
                                    BufTVOC_Step = 6;
                            }
                            if (BufTVOC_Step == 6)
                            {   //Move to TVOC                               
                                if (TVOC.IsTurnEnable)
                                {
                                    //if (Axis._01.CurrentPos <= PDs[6].Jig.JigPos)                                        
                                    //if (!(ReadyPD == PDs[7] && (PDStep == 6 || PDRetest)))
                                    await OUT.LOADING_02_TURN_SOL.SET();
                                }
                                if (await MoveAbs(Axis._02, AXIS_02_TVOC.JigPos, Axis02Velocity))
                                {
                                    if (TVOC.IsTurnEnable)
                                    {
                                        if (IN.LOADING_02_TURN_SENSOR.PinValue == PinValue.ON)
                                            BufTVOC_Step = 7;
                                    }
                                    else BufTVOC_Step = 7;
                                    AXIS_01_BUFFER.IsSetInJig = false;
                                }
                            }
                            if (BufTVOC_Step == 7)
                            {  //Put down to TVOC
                                await OUT.LOADING_02_LIFT_SOL.SET();
                                if (IN.LOADING_02_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                    BufTVOC_Step = 8;
                            }
                            if (BufTVOC_Step == 8)
                            {//Unclamp
                                await OUT.LOADING_02_CLAMP_SOL.RST();
                                await OUT.LOADING_02_UNCLAMP_SOL.SET();
                                if (IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                {
                                    BufTVOC_Step = 9;
                                    TVOC.Jig.IsSetInJig = true;
                                }
                            }
                            if (BufTVOC_Step == 9)
                            {//Lift 02 Up
                                await OUT.LOADING_02_LIFT_SOL.RST();
                                if (IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                    BufTVOC_Step = 10;
                            }
                            if (BufTVOC_Step == 10)
                            {//Move to TRANSFER_CV
                                if (await MoveAbs(Axis._02, AXIS_02_TRANSFER_CV_POS.JigPos, Axis02Velocity))
                                    BufTVOC_Step = 11;
                            }
                            if (BufTVOC_Step == 11)
                            {
                                //if (TVOC.Jig.IsSetInJig)
                                //{
                                //    await OUT.TVOC_START.SET();
                                //    Thread.Sleep(50);
                                //    await OUT.TVOC_START.RST();
                                //}
                                TVOC.Jig.TestResult = TestResult.PASS;
                                BufTVOC_Step = 0;
                                BUFFER_TVOC_STEP = 1;
                            }
                        }
                        if (BUFFER_TVOC_STEP == 1)
                        {
                            if (TVOC.Jig.TestResult == TestResult.PASS &&
                                TVOCTrans_Step == 0 &&
                                TVOC.Jig.IsSetInJig == true && Axis._01.CurrentPos <= AXIS_01_BUFFER.JigPos)
                            {
                                TVOCTrans_Step = 1;
                            }
                            else if (TVOCTrans_Step == 0) BUFFER_TVOC_STEP = 0;
                            //RUN
                            if (TVOCTrans_Step == 1)
                            {
                                if (TVOC.IsTurnEnable)
                                    await OUT.LOADING_02_TURN_SOL.SET();
                                TVOCTrans_Step = 2;
                            }
                            if (TVOCTrans_Step == 2)
                            {//Move To TVOC
                                if (await MoveAbs(Axis._02, AXIS_02_TVOC.JigPos, Axis02Velocity))
                                    if (IN.LOADING_02_TURN_SENSOR.PinValue == PinValue.ON)
                                        TVOCTrans_Step = 3;
                            }
                            if (TVOCTrans_Step == 3)
                            {//Lift 02 DOWN
                                await OUT.LOADING_02_LIFT_SOL.SET();
                                if (IN.LOADING_02_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    TVOCTrans_Step = 4;
                                }
                            }
                            if (TVOCTrans_Step == 4)
                            {//Clamp
                                await OUT.LOADING_02_CLAMP_SOL.SET();
                                await OUT.LOADING_02_UNCLAMP_SOL.RST();
                                if (IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                {
                                    TVOCTrans_Step = 5;
                                }
                            }
                            if (TVOCTrans_Step == 5)
                            {//Lift 02 DOWN
                                await OUT.LOADING_02_LIFT_SOL.RST();
                                if (IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    if (IN.LOADING_02_CLAMP_SENSOR.PinValue == PinValue.OFF &&
                                        IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.OFF)
                                    {
                                        TVOCTrans_Step = 6;
                                        TVOC.Jig.TestResult = TestResult.READY;
                                        TVOC.Jig.IsSetInJig = false;
                                    }
                                }
                            }
                            if (TVOCTrans_Step == 6)
                            {
                                await OUT.LOADING_02_TURN_SOL.RST();
                                if (await MoveAbs(Axis._02, AXIS_02_TRANSFER_CV_POS.JigPos, Axis02Velocity))
                                    TVOCTrans_Step = 7;
                            }
                            if (TVOCTrans_Step == 7)
                            {//RETURN
                                if (IN.LOADING_02_RETURN_SENSOR.PinValue == PinValue.ON)
                                {
                                    TVOCTrans_Step = 8;
                                }
                            }
                            if (TVOCTrans_Step == 8)
                            {//Wait for TRANS CV SENSOR
                                if (IN.TRANSFER_CV_SENSOR_BEGIN.PinValue == PinValue.ON)
                                    await OUT.LOADING_02_LIFT_SOL.SET();
                                if (IN.LOADING_02_LIFT_DOWN_SENSOR.PinValue == PinValue.ON)
                                {
                                    TVOCTrans_Step = 9;                                   
                                    TVOC.Jig.OKCounter++;
                                }
                            }
                            if (TVOCTrans_Step == 9)
                            {
                                await OUT.LOADING_02_UNCLAMP_SOL.SET();
                                await OUT.LOADING_02_CLAMP_SOL.RST();
                                if (IN.LOADING_02_UNCLAMP_SENSOR.PinValue == PinValue.ON)
                                    TVOCTrans_Step = 10;
                            }
                            if (TVOCTrans_Step == 10)
                            {
                                await OUT.LOADING_02_LIFT_SOL.RST();
                                if (IN.LOADING_02_LIFT_UP_SENSOR.PinValue == PinValue.ON)
                                {
                                    TVOCTrans_Step = 0;
                                    BUFFER_TVOC_STEP = 0;
                                }
                            }
                        }
                        //}
                    }
                    else if (Axis._01.Motioning == PinValue.ON || Axis._02.Motioning == PinValue.ON)
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
            }
        }
        CancellationTokenSource PDLoopCancelationTokenSource = null;
        private void PDLoop()
        {
            PDLoopCancelationTokenSource = new CancellationTokenSource();
            Task.Run(async () => await PDLoopExecution(PDLoopCancelationTokenSource.Token));
        }

        private async Task<bool> MoveAbs(AxisStatus Axis, int Pos, int Vel)
        {
            var motioning = await App.ServoCOM.StepGetdata(Axis.SlaveID, Flag.GetAxisStatus, null);
            if (motioning != null)
            {
                Axis.Status = BitConverter.ToInt32(motioning, 3);
            }
            var axiscurrentpos = await App.ServoCOM.StepGetdata(Axis.SlaveID, Flag.GetActualPosition, null);
            if (axiscurrentpos != null)
                Axis.CurrentPos = BitConverter.ToInt32(axiscurrentpos, 3);

            if (Axis.Motioning == PinValue.OFF && (Pos != Axis.CurrentPos) && Axis.ALM == PinValue.OFF)
                await App.ServoCOM.StepGetdata(Axis.SlaveID, Flag.MoveSingleAxisAbs, DataFrame.MoveAbsIncData(Pos, Vel));
            return ((Pos == Axis.CurrentPos /*|| (Pos >= Axis.CurrentPos - 100 && Pos <= Axis.CurrentPos + 100)*/) && (Axis.Motioning == PinValue.OFF)/*Axis.Motioning == PinValue.OFF*/);
        }

        public void Init()
        {
            using (var Db = new SettingContext())
            {
                Jigs = Db.JigModels.ToList();

                //--------------------------------------------------------------------------
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
                var _TVOC = Jigs.Where(x => x.JigDescription.Contains("TVOC")).FirstOrDefault();
                if (_TVOC != null)
                {
                    TVOC = new TVOCJig() { Jig = _TVOC, OKPin = IN.TVOC_OK_SIGNAL, NGPin = IN.TVOC_NG_SIGNAL, InTesting = IN.TVOC_TEST };
                    var DbIsTurnEnable = Db.ValueSettings.Where(x => x.Key == "IsTurnEnable").FirstOrDefault();
                    if (DbIsTurnEnable != null)
                    {
                        if (DbIsTurnEnable.Value == "1")
                            TVOC.IsTurnEnable = true;
                        else TVOC.IsTurnEnable = false;
                        //TurnButton.DataContext = TVOC;
                    }
                }
            }
            var _PD01 = Jigs.Where(x => x.JigDescription.Contains("PD_01")).FirstOrDefault();
            if (_PD01 != null)
            {
                var PD01 = new PDJig() { Jig = _PD01, PackingPin = OUT.PD_01_PACK, PowerPin = OUT.PD_01_POWER_PIN };
                PD01.Jig.OKCounter = _PD01.OKCounter;
                PDs.Add(PD01);
            }
            var _PD02 = Jigs.Where(x => x.JigDescription.Contains("PD_02")).FirstOrDefault();
            if (_PD02 != null)
            {
                var PD02 = new PDJig() { PackingPin = OUT.PD_02_PACK, Jig = _PD02, PowerPin = OUT.PD_01_POWER_PIN };
                PD02.Jig.OKCounter = _PD02.OKCounter;
                PDs.Add(PD02);
            }
            var _PD03 = Jigs.Where(x => x.JigDescription.Contains("PD_03")).FirstOrDefault();
            if (_PD03 != null)
            {
                var PD03 = new PDJig() { PackingPin = OUT.PD_03_PACK, Jig = _PD03, PowerPin = OUT.PD_02_POWER_PIN };
                PD03.Jig.OKCounter = _PD03.OKCounter;
                PDs.Add(PD03);
            }
            var _PD04 = Jigs.Where(x => x.JigDescription.Contains("PD_04")).FirstOrDefault();
            if (_PD04 != null)
            {
                var PD04 = new PDJig() { PackingPin = OUT.PD_04_PACK, Jig = _PD04, PowerPin = OUT.PD_02_POWER_PIN };
                PD04.Jig.OKCounter = _PD04.OKCounter;
                PDs.Add(PD04);
            }
            var _PD05 = Jigs.Where(x => x.JigDescription.Contains("PD_05")).FirstOrDefault();
            if (_PD05 != null)
            {
                var PD05 = new PDJig() { PackingPin = OUT.PD_05_PACK, Jig = _PD05, PowerPin = OUT.PD_03_POWER_PIN };
                PD05.Jig.OKCounter = _PD05.OKCounter;
                PDs.Add(PD05);
            }
            var _PD06 = Jigs.Where(x => x.JigDescription.Contains("PD_06")).FirstOrDefault();
            if (_PD06 != null)
            {
                var PD06 = new PDJig() { PackingPin = OUT.PD_06_PACK, Jig = _PD06, PowerPin = OUT.PD_03_POWER_PIN };
                PD06.Jig.OKCounter = _PD06.OKCounter;
                PDs.Add(PD06);
            }
            var _PD07 = Jigs.Where(x => x.JigDescription.Contains("PD_07")).FirstOrDefault();
            if (_PD07 != null)
            {
                var PD07 = new PDJig() { PackingPin = OUT.PD_07_PACK, Jig = _PD07, PowerPin = OUT.PD_04_POWER_PIN };
                PD07.Jig.OKCounter = _PD07.OKCounter;
                PDs.Add(PD07);
            }
            var _PD08 = Jigs.Where(x => x.JigDescription.Contains("PD_08")).FirstOrDefault();
            if (_PD08 != null)
            {
                var PD08 = new PDJig() { PackingPin = OUT.PD_08_PACK, Jig = _PD08, PowerPin = OUT.PD_04_POWER_PIN };
                PD08.Jig.OKCounter = _PD08.OKCounter;
                PDs.Add(PD08);
            }

            LEAKs = new List<LeakJig>();
            var _LEAK_01 = Jigs.Where(x => x.JigDescription.Contains("LEAK_01")).FirstOrDefault();
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
            var _LEAK_02 = Jigs.Where(x => x.JigDescription.Contains("LEAK_02")).FirstOrDefault();
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
            var _LEAK_03 = Jigs.Where(x => x.JigDescription.Contains("LEAK_03")).FirstOrDefault();
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
            var _LEAK_04 = Jigs.Where(x => x.JigDescription.Contains("LEAK_04")).FirstOrDefault();
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

            NFCs = new List<NFCJig>();
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

            var _AXIS_01_LOADING_POS = Jigs.Where(x => x.JigDescription.Contains("AXIS_01_LOADING_CV_POS")).FirstOrDefault();
            if (_AXIS_01_LOADING_POS != null)
            {
                AXIS_01_LOADING_POS = _AXIS_01_LOADING_POS;
            }
            var _AXIS_01_NG_POS = Jigs.Where(x => x.JigDescription.Contains("AXIS_01_NG_POS")).FirstOrDefault();
            if (_AXIS_01_NG_POS != null)
            {
                AXIS_01_NG_POS = _AXIS_01_NG_POS;
            }
            var _AXIS_01_BUFFER = Jigs.Where(x => x.JigDescription.Contains("AXIS_01_BUFFER")).FirstOrDefault();
            if (_AXIS_01_BUFFER != null)
            {
                AXIS_01_BUFFER = _AXIS_01_BUFFER;
            }
            var _AXIS_01_TVOC = Jigs.Where(x => x.JigDescription.Contains("AXIS_01_TVOC")).FirstOrDefault();
            if (_AXIS_01_TVOC != null)
            {
                AXIS_01_TVOC = _AXIS_01_TVOC;
            }
            var _AXIS_02_BUFFER = Jigs.Where(x => x.JigDescription.Contains("AXIS_02_BUFFER")).FirstOrDefault();
            if (_AXIS_02_BUFFER != null)
            {
                AXIS_02_BUFFER = _AXIS_02_BUFFER;
            }
            var _AXIS_02_TRANSFER_CV_POS = Jigs.Where(x => x.JigDescription.Contains("AXIS_02_TRANSFER_CV_POS")).FirstOrDefault();
            if (_AXIS_02_TRANSFER_CV_POS != null)
            {
                AXIS_02_TRANSFER_CV_POS = _AXIS_02_TRANSFER_CV_POS;
            }
            var _AXIS_02_TVOC = Jigs.Where(x => x.JigDescription.Contains("AXIS_02_TVOC")).FirstOrDefault();
            if (_AXIS_02_TVOC != null)
            {
                AXIS_02_TVOC = _AXIS_02_TVOC;
            }
            var _AXIS_03_LOADING_CV_POS = Jigs.Where(x => x.JigDescription.Contains("AXIS_03_LOADING_CV_POS")).FirstOrDefault();
            if (_AXIS_03_LOADING_CV_POS != null)
            {
                AXIS_03_LOADING_CV_POS = _AXIS_03_LOADING_CV_POS;
            }
            var _AXIS_03_BUFFER = Jigs.Where(x => x.JigDescription.Contains("AXIS_03_BUFFER")).FirstOrDefault();
            if (_AXIS_03_BUFFER != null)
            {
                AXIS_03_BUFFER = _AXIS_03_BUFFER;
            }
            var _AXIS_04_BUFFER = Jigs.Where(x => x.JigDescription.Contains("AXIS_04_BUFFER")).FirstOrDefault();
            if (_AXIS_04_BUFFER != null)
            {
                AXIS_04_BUFFER = _AXIS_04_BUFFER;
            }
            var _AXIS_04_UNLOADING_CV_POS = Jigs.Where(x => x.JigDescription.Contains("AXIS_04_UNLOADING_CV_POS")).FirstOrDefault();
            if (_AXIS_04_UNLOADING_CV_POS != null)
            {
                AXIS_04_UNLOADING_CV_POS = _AXIS_04_UNLOADING_CV_POS;
            }
            var _AXIS_04_NG_POS = Jigs.Where(x => x.JigDescription.Contains("AXIS_04_NG_POS")).FirstOrDefault();
            if (_AXIS_04_NG_POS != null)
            {
                AXIS_04_NG_POS = _AXIS_04_NG_POS;
            }


        }
        int PD_TVOC_STEP = 0, BUFFER_TVOC_STEP, PD_Buf_Step = 0, BufTVOC_Step = 0, TVOCTrans_Step = 0;
        int PDStep = 0, LeakFinalStep, ORGStep = 0;
        private int NFC_BUFF_STEP, Buf_Leak_Step, CV_NFC_Step, BUFF_LEAK_STEP;
        int Axis01Velocity, Axis02Velocity, Axis03Velocity, Axis04Velocity;
        List<PDJig> PDs = new List<PDJig>();
        List<LeakJig> LEAKs = new List<LeakJig>();
        List<NFCJig> NFCs = new List<NFCJig>();
        List<JigModel> Jigs;

        JigModel AXIS_01_LOADING_POS, AXIS_01_NG_POS, AXIS_01_BUFFER, AXIS_01_TVOC;
        JigModel AXIS_02_BUFFER, AXIS_02_TRANSFER_CV_POS, AXIS_02_TVOC;
        JigModel AXIS_03_LOADING_CV_POS;
        JigModel AXIS_04_BUFFER, AXIS_04_UNLOADING_CV_POS, AXIS_04_NG_POS;
        JigModel AXIS_03_BUFFER;
        public TVOCJig TVOC;
        private bool PDReady;
        private bool PDRUN;
    }
}

