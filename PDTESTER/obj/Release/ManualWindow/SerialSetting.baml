   M S B A M L   `   `   `  ���� C  ?PDTESTER, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PDTESTER.ManualWindowPDTESTER.ManualWindow  )    #PDTESTER.ManualWindow.SerialSetting   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     3com#clr-namespace:PDTESTER.ManualWindow     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     $	��640��$	��480��.���   � ColumnDefinitions  �� $
��Auto ��� $��* ��� $��* �.+��� .+��� $��Setting��$��10q�$��24(�$��SemiBold"���  �� Orientation$ 
Horizontal=�.+��� $��10q�.+��� $��
SW Setting��$��16(�$��SemiBold"��� -   $��USB 3.0 Device Test�� d� 	IsChecked �� *
SW.UD3Test+ �� Mode$ TwoWay=��� -   $��USB 3.0 OTG Test�� �� *
SW.UO3Test+$ TwoWay=��� -   $��USB 2.0 OTG Test�� �� *
SW.UO2Test+$ TwoWay=��� -   $��PDC Test�� �� *
SW.PDCTest+$ TwoWay=��� -   $��	LOAD Test�� �� *SW.LOADTest+$ TwoWay=��� -   $��
VCONN Test�� �� *SW.VCONNTest+$ TwoWay=��� -   $��SBU Test�� �� *
SW.SBUTest+$ TwoWay=��� $��Work Setting��$��16(�$��SemiBold"�d� $��	PD Bypass�� �� *
PDBypass+$ TwoWay=�d� $��
SIM Bypass�� �� *
TVOCBypass+$ TwoWay=�d� $��
NFC Bypass�� �� *	NFCBypass+$ TwoWay=�d� $��Leak Bypass�� �� *
LEAKBypass+$ TwoWay=�d� $��
8UCTs Mode�� �� *
UCT8Mode+$ TwoWay=��� $��TVOC Setting��$��16(�$��SemiBold"�$��0,5,0,0q�d� $
��Turn�� �� *
TVOCTurn+$ TwoWay=��� $��Test TimeOut��$��16(�$��SemiBold"�$��0,5,0,0q��� ���� *TimeOutText+$ TwoWay=��� $��FreeMode��$��16(�$��SemiBold"�$��0,5,0,0q��� -   $��FreeMode��� $��1��$��10q�.+��� -	   $��ListCOMDevice������ .R�A� $��10q� A� Columns$ 4��.+��� $��Center=����� *Device+�� $��Center=����� %   PDTESTER.ManualWindow.COMDevice   	PortNames �� Source# Z $ OneWay=�{��� *
IsClosed+ �� SelectedItem �� *SelectedItem+$ TwoWay=��� -
   $	��Set��{��� *
IsClosed+� $��2��$��10q� � RowDefinitions �� $��* ��� $
��Auto �.+�&   System.Windows.Controls.DataGrid  -   $��ListNVValues��	   AutoGenerateColumns	 . �� -   $��1��$
��Save��