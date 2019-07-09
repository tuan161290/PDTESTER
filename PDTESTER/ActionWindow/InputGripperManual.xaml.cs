using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static GPIOCommunication.GPIOHelper;

namespace PDTESTER.ActionWindow
{
    /// <summary>
    /// Interaction logic for InputGripperManual.xaml
    /// </summary>
    public partial class InputGripperManual : Window
    {
        public InputGripperManual()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Turn_Clicked(object sender, RoutedEventArgs e)
        {
            //if (App.LOADING_03_UP_SENSOR.PinValue == PinValue.ON)
            //    await App.GPIOBoardF0.SetOn(App.LOADING_03_TURN_SOL);
        }

        private void Return_Clicked(object sender, RoutedEventArgs e)
        {
            //if (App.LOADING_03_UP_SENSOR.PinValue == PinValue.ON)
            //    await App.GPIOBoardF0.SetOff(App.LOADING_03_TURN_SOL);
        }

        private void Trans_Right(object sender, RoutedEventArgs e)
        {
            //if (App.LOADING_03_UP_SENSOR.PinValue == PinValue.ON)
            //{
            //    await App.GPIOBoardF0.SetOff(App.TRANS_CYLINDER_LEFT_SOL);
            //    await App.GPIOBoardF0.SetOn(App.TRANS_CYLINDER_RIGHT_SOL);
            //}
        }

        private void Trans_Left(object sender, RoutedEventArgs e)
        {
            if (IN.LOADING_03_LIFT_UP_SENSOR.PinValue == PinValue.ON)
            {
                //await App.GPIOBoardF0.SetOn(App.TRANS_CYLINDER_LEFT_SOL);
                //await App.GPIOBoardF0.SetOff(App.TRANS_CYLINDER_RIGHT_SOL);
            }
        }

        private void Loading_03_Up_Down(object sender, RoutedEventArgs e)
        {
            //if (App.LOADING_03_UP_SENSOR.PinValue == PinValue.ON)
            //    await App.GPIOBoardF0.SetOn(App.LOADING_03_UP_DOWN_SOL);
            //else
            //    await App.GPIOBoardF0.SetOff(App.LOADING_03_UP_DOWN_SOL);
        }

        private void Loading_03_Unlamp(object sender, RoutedEventArgs e)
        {
            //await App.GPIOBoardF0.SetOn(App.LOADING_03_UNCLAMP_SOL);
            //await App.GPIOBoardF0.SetOff(App.LOADING_03_CLAMP_SOL);
        }
        private void Loading_03_Clamp(object sender, RoutedEventArgs e)
        {
            //await App.GPIOBoardF0.SetOff(App.LOADING_03_UNCLAMP_SOL);
            //await App.GPIOBoardF0.SetOn(App.LOADING_03_CLAMP_SOL);
        }
    }
}
