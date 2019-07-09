using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PDTESTER
{
    public class UCTHelper
    {
        public SerialHelper Serial { get; set; } = null;
        public byte[] ReceivedBytes;

        public UCTHelper(string SerialPort)
        {
            Serial = new SerialHelper(SerialPort, 40, "UCTHelper");
        }

        SemaphoreSlim WriteSync = new SemaphoreSlim(1, 1);

        public async Task<string> UCTTestSwitch(int JigNo, int channel, int OnOff)
        {
            try
            {
                await WriteSync.WaitAsync();
                char JigID = Convert.ToChar(JigNo.ToString());
                char Channel = Convert.ToChar(channel.ToString());
                List<byte> cmd = new List<byte>() { (byte)':', (byte)'0', (byte)JigID, (byte)'S', (byte)Channel };
                if (OnOff != 0)
                    cmd.AddRange(Command.Start);
                else
                    cmd.AddRange(Command.Stop);
                cmd.Add(CheckSumGenerate(cmd));
                int Retry = 2;
                while (Retry > 0)
                {
                    await Serial.WriteAsync(cmd.ToArray());
                    ReceivedBytes = await Serial.ReadUCTAsync();
                    if (ReceivedBytes != null)
                        if (CheckSum(ReceivedBytes.ToList()))
                        {
                            //Retry = 0;
                            //string s = Encoding.ASCII.GetString(ReceivedBytes);
                            return "OK";
                        }
                    Retry--;
                }
                return "NORESP";
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return s;
            }
            finally
            {
                WriteSync.Release();
            }
        }

        public async Task<UCTStatus> GetTestStatus(int JigNo, int channel)
        {
            try
            {
                await WriteSync.WaitAsync();
                char JigID = Convert.ToChar(JigNo.ToString());
                char Channel = Convert.ToChar(channel.ToString());
                List<byte> cmd = new List<byte>() { (byte)':', (byte)'0', (byte)JigID, (byte)'S', (byte)Channel };
                List<byte> rqt = new List<byte>();
                cmd.AddRange(Command.CheckTestStatus);
                cmd.Add(CheckSumGenerate(cmd));
                string S = Encoding.ASCII.GetString(cmd.ToArray());
                ReceivedBytes = new byte[32];
                int Retry = 2;
                while (Retry > 0 && Serial.IsOpen)
                {
                    await Serial.WriteAsync(cmd.ToArray());
                    //Thread.Sleep(20);
                    ReceivedBytes = await Serial.ReadUCTAsync();
                    if (ReceivedBytes != null)
                    {
                        //S = Encoding.ASCII.GetString(ReceivedBytes);
                        if (CheckSum(ReceivedBytes.ToList()))
                        {
                            string ReceivedString = Encoding.ASCII.GetString(ReceivedBytes);
                            if (ReceivedString[2] == JigID && ReceivedString[4] == Channel)
                            {
                                Retry = 0;
                                ReceivedString = ReceivedString.Substring(5, 4);
                                switch (ReceivedString)
                                {
                                    case "REDY":
                                        return UCTStatus.READY;
                                    case "15MD":
                                        return UCTStatus.MODE15;
                                    case "UD31":
                                        return UCTStatus.UD31;
                                    case "UO31":
                                        return UCTStatus.UO31;
                                    case "UO20":
                                        return UCTStatus.UO20;
                                    case "PDCT":
                                        return UCTStatus.PDC;
                                    case "LOAD":
                                        return UCTStatus.LOAD;
                                    case "VCON":
                                        return UCTStatus.VCON;
                                    case "SBUT":
                                        return UCTStatus.SBU;
                                    case "STOP":
                                        return UCTStatus.STOP;
                                    case "FINI":
                                        return UCTStatus.FINISHED;
                                }
                                return UCTStatus.NORESP;
                            }
                        }
                    }
                    Retry--;
                    await Task.Delay(20);
                    Serial.DiscardInBuffer();
                }
                return UCTStatus.NORESP;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return UCTStatus.NORESP;
            }
            finally
            {
                WriteSync.Release();
            }
        }
        public async Task Reset(int JigNo, int channel)
        {
            try
            {
                await WriteSync.WaitAsync();
                char JigID = Convert.ToChar(JigNo.ToString());
                char Channel = Convert.ToChar(channel.ToString());
                //:01S1RSONa
                List<byte> cmd = new List<byte>() { (byte)':', (byte)'0', (byte)JigID, (byte)'S', (byte)Channel, (byte)'R', (byte)'S', (byte)'O', (byte)'N', };
                cmd.Add(CheckSumGenerate(cmd));
                //string S = Encoding.ASCII.GetString(cmd.ToArray());
                int Retry = 2;
                while (Retry > 0)
                {
                    await Serial.WriteAsync(cmd.ToArray());
                    ReceivedBytes = await Serial.ReadUCTAsync();
                    if (ReceivedBytes != null)
                        if (CheckSum(ReceivedBytes.ToList()))
                        {
                            //Retry = 0;
                            //string s = Encoding.ASCII.GetString(ReceivedBytes);
                            return;
                        }
                    Retry--;
                }
                return;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return;
            }
            finally
            {
                WriteSync.Release();
            }
        }
        public async Task GetTestItemResults(int JigNo, int channel, ObservableCollection<TestItem> TestItems)
        {
            try
            {
                await WriteSync.WaitAsync();
                char JigID = Convert.ToChar(JigNo.ToString());
                char Channel = Convert.ToChar(channel.ToString());
                List<byte> cmd = new List<byte>() { (byte)':', (byte)'0', (byte)JigID, (byte)'S', (byte)Channel, (byte)'A', (byte)'L', (byte)'L', (byte)'?' };
                cmd.Add(CheckSumGenerate(cmd));
                //string S = Encoding.ASCII.GetString(cmd.ToArray());
                ReceivedBytes = new byte[32];
                int Retry = 3;
                while (Retry > 0)
                {
                    await Serial.WriteAsync(cmd.ToArray());
                    ReceivedBytes = await Serial.ReadUCTAsync();
                    if (ReceivedBytes != null)
                    {
                        if (CheckSum(ReceivedBytes.ToList()))
                        {
                            Retry = 0;
                            //string ReceivedString = Encoding.ASCII.GetString(ReceivedBytes);
                            int ItemIndex = 0;
                            for (int index = 6; index < 14; index++)
                            {
                                if (index != 7)
                                {
                                    switch (ReceivedBytes[index])
                                    {

                                        case (byte)'P':
                                            TestItems[ItemIndex].TestItemStatus = TestItemStatus.PASS;
                                            break;
                                        case (byte)'N':
                                            TestItems[ItemIndex].TestItemStatus = TestItemStatus.NT;
                                            break;
                                        case (byte)'F':
                                            TestItems[ItemIndex].TestItemStatus = TestItemStatus.FAIL;
                                            break;
                                        case (byte)'T':
                                            TestItems[ItemIndex].TestItemStatus = TestItemStatus.TEST;
                                            break;
                                    }
                                    ItemIndex++;
                                }
                            }
                            //:01T1APNPPPPPP
                        }
                    }
                    Retry--;
                }
                //return null;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                //return null;
            }
            finally
            {
                WriteSync.Release();
            }
        }

        private byte CheckSumGenerate(List<byte> Bytes)
        {
            int i;
            byte CSUM = 0;
            for (i = 0; i < Bytes.Count(); i++)
            {
                CSUM += Bytes[i];
            }
            return CSUM;
        }
        private bool CheckSum(List<byte> Bytes)
        {
            try
            {
                if (Bytes == null) return false;
                Bytes.RemoveRange(Bytes.Count - 2, 2);
                byte CSUM = 0;
                for (int i = 0; i < Bytes.Count - 1; i++)
                {
                    CSUM += Bytes[i];
                }
                return (CSUM == Bytes.Last());
            }
            catch
            {
                return false;
            }
        }

        public void CloseDevice()
        {
            if (Serial != null)
                Serial.CloseDevice();
        }
    }
}

