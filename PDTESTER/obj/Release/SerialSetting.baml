   M S B A M L   `   `   `  ���� C  ?PDTESTER, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:PDTESTERPDTESTER      PDTESTER.SerialSetting   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:PDTESTER     -     � Title$  Serial Setting��$	��600��$	��450�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � WindowStyle$ 
ToolWindow=�.��� .+��� .+��� $��Setting��$��10q�$��24(�$��SemiBold"���  �� Orientation$ 
Horizontal=�.+��� $��10q�.+��� $��
SW Setting��$��16(�$��SemiBold"��� $��USB 3.0 Device Test�� d� 	IsChecked �� *
SW.UD3Test+ �� Mode$ TwoWay=��� $��USB 3.0 OTG Test�� �� *
SW.UO3Test+$ TwoWay=��� $��USB 2.0 OTG Test�� �� *
SW.UO2Test+$ TwoWay=��� $��PDC Test�� �� *
SW.PDCTest+$ TwoWay=��� $��	LOAD Test�� �� *SW.LOADTest+$ TwoWay=��� $��
VCONN Test�� �� *SW.VCONNTest+$ TwoWay=��� $��SBU Test�� �� *
SW.SBUTest+$ TwoWay=��� $��Work Setting��$��16(�$��SemiBold"�d� $��	PD Bypass�� �� *
PDBypass+$ TwoWay=�d� $��TVOC Bypass�� �� *
TVOCBypass+$ TwoWay=�d� $��
NFC Bypass�� �� *	NFCBypass+$ TwoWay=�d� $��Leak Bypass�� �� *
LEAKBypass+$ TwoWay=��� $��TVOC Setting��$��16(�$��SemiBold"�$��0,5,0,0q�d� $
��Turn�� �� *
TVOCTurn+$ TwoWay=��� $��Test TimeOut��$��16(�$��SemiBold"�$��0,5,0,0q��� ���� *TimeOutText+$ TwoWay=��� $��10q�$	��250��.+��� $��Select COM port��$��16(�$��SemiBold"��� $��Center=�.+��� $
��Auto��$��1��
�������$��0,5,0,5q��� $��UCT Communication���� -   $��UCTListComport��$��Stretch=����� .R��� ���� �� $ 
Horizontal=�.+��� -   $��	UCTButton��$��Connect��$	��100��$��0,5,5,5q��� -   $��	UCTStatus��$��
UCT Status��$��Center=��� $
��Auto��$��1��
�������$��0,5,0,5q��� $��Servo Communication���� -   $��ServoListCOMPort��$��Stretch=����� .R��� ���� �� $ 
Horizontal=�.+��� -   $��ServoButton��$��Connect��$	��100��$��0,5,5,5q��� -   $��ServoStatus��$��Motor��$��Center=��� $
��Auto��$��1��
�������$��0,5,0,5q��� $��GPIO Communication���� -   $��GPIOListCOMPort��$��Stretch=����� .R��� ���� �� $ 
Horizontal=�.+��� -	   $��
GPIOButton��$��Connect��$	��100��$��0,5,5,5q�$��Center=��� -
   $��
GPIOStatus��$
��GPIO��$��Center=��� $��GPIO Communication���� -   $��PrinterListCOMPort��$��Stretch=����� .R��� ���� �� $ 
Horizontal=�.+��� -   $��PrinterButton��$��Connect��$	��100��$��0,5,5,5q�$��Center=��� -   $��PrinterStatus��$��Printer��$��Center=��� $ 
Horizontal=�.+��� -   $��OK��$	��200��$��10q��� -   $��Close��$	��200��$��10q�