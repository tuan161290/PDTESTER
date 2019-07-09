using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PDTESTER
{
    /// <summary>
    /// Interaction logic for Manual.xaml
    /// </summary>
    public partial class Manual : Page
    {
        public Manual()
        {
            InitializeComponent();
            Loaded += Manual_Loaded;
            Unloaded += Manual_Unloaded;
        }

        private void Manual_Unloaded(object sender, RoutedEventArgs e)
        {
            CancelLoopTask();
        }

        private void Manual_Loaded(object sender, RoutedEventArgs e)
        {
            //CancelLoopTask();
            ServoLoop();
        }

        public static void ServoLoop()
        {
            CancelLoopTask();
            ServoLoopCancelationTokenSource = new CancellationTokenSource();
            Task.Run(async () => await ServoTask(ServoLoopCancelationTokenSource.Token));
        }

        static Stopwatch sw = new Stopwatch();
        private static async Task ServoTask(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    //await Task.Delay(15);
                    sw.Reset();
                    sw.Start();
                    var axis1Status = await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.GetAxisStatus, null);
                    if (axis1Status != null)
                    {
                        Axis._01.Status = BitConverter.ToInt32(axis1Status, 3);//3 = Index of High Byte INT                                                                               
                        //if ((Axis._01.Status & BitMask.Motioning) > 0) //GET AXIS#1 Motioning
                        //{

                        //}
                        //if ((Axis._01.Status & BitMask.ERRORALL) > 0)
                        //{

                        //}
                    }
                    //await Task.Delay(10);                   
                    var axis1currentpos = await App.ServoCOM.StepGetdata(Axis._01.SlaveID, Flag.GetActualPosition, null);
                    if (axis1currentpos != null) { 
                        Axis._01.CurrentPos = BitConverter.ToInt32(axis1currentpos, 3);
                     
                    }
                    //--------------------------------------------------
                    var axis2Status = await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.GetAxisStatus, null);
                    if (axis2Status != null)
                    {
                        Axis._02.Status = BitConverter.ToInt32(axis2Status, 3);//3 = Index of High Byte INT                                                                               
                        //if ((Axis._02.Status & BitMask.Motioning) > 0) //GET AXIS#1 Motioning
                        //{

                        //}
                        //if ((Axis._02.Status & BitMask.ERRORALL) > 0)
                        //{

                        //}
                    }
                    //await Task.Delay(10);                   
                    var axis2currentpos = await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.GetActualPosition, null);
                    if (axis2currentpos != null)
                        Axis._02.CurrentPos = BitConverter.ToInt32(axis2currentpos, 3);
                    //--------------------------------------------------
                    var axis3Status = await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.GetAxisStatus, null);
                    if (axis3Status != null)
                    {
                        Axis._03.Status = BitConverter.ToInt32(axis3Status, 3);//3 = Index of High Byte INT                                                                               
                        //if ((Axis._03.Status & BitMask.Motioning) > 0) //GET AXIS#1 Motioning
                        //{

                        //}
                        //if ((Axis._03.Status & BitMask.ERRORALL) > 0)
                        //{

                        //}
                    }
                    //await Task.Delay(10);                   
                    var axis3currentpos = await App.ServoCOM.StepGetdata(Axis._03.SlaveID, Flag.GetActualPosition, null);
                    if (axis3currentpos != null)
                        Axis._03.CurrentPos = BitConverter.ToInt32(axis3currentpos, 3);
                    //--------------------------------------------------
                    var axis4Status = await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.GetAxisStatus, null);
                    if (axis4Status != null)
                    {
                        Axis._04.Status = BitConverter.ToInt32(axis4Status, 3);//3 = Index of High Byte INT                                                                               
                        //if ((Axis._04.Status & BitMask.Motioning) > 0) //GET AXIS#1 Motioning
                        //{

                        //}
                        //if ((Axis._04.Status & BitMask.ERRORALL) > 0)
                        //{

                        //}
                    }
                    //await Task.Delay(10);                   
                    var axis4currentpos = await App.ServoCOM.StepGetdata(Axis._04.SlaveID, Flag.GetActualPosition, null);
                    if (axis4currentpos != null)
                        Axis._04.CurrentPos = BitConverter.ToInt32(axis4currentpos, 3);
                    Axis._01.RespondTime = (int)sw.ElapsedMilliseconds;
                    //var axis2Status = await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.GetAxisStatus, null);
                    //if (axis2Status != null)
                    //{
                    //    Axis._02.Status = BitConverter.ToInt32(axis2Status, 3);//3 = Index of High Byte INT                                                                               
                    //    if ((Axis._02.Status & BitMask.Motioning) > 0) //GET AXIS#1 Motioning
                    //    {

                    //    }
                    //    if ((Axis._02.Status & BitMask.ERRORALL) > 0)
                    //    {

                    //    }
                    //}
                    //var axis2currentpos = await App.ServoCOM.StepGetdata(Axis._02.SlaveID, Flag.GetActualPosition, null);
                    //if (axis2currentpos != null)
                    //    Axis._02.CurrentPos = BitConverter.ToInt32(axis2currentpos, 3);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        public static CancellationTokenSource ServoLoopCancelationTokenSource = null;
        public static void CancelLoopTask()
        {
            if (ServoLoopCancelationTokenSource != null)
            {
                if (!ServoLoopCancelationTokenSource.IsCancellationRequested)
                {
                    ServoLoopCancelationTokenSource.Cancel();
                }
            }
        }
    }
}
