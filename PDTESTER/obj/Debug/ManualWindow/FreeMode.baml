   M S B A M L   `   `   `  ����C  ?PDTESTER, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PDTESTER.ManualWindowPDTESTER.ManualWindow  5   	   #clr-namespace:PDTESTERPDTESTER  5   
   $    PDTESTER.ManualWindow.FreeMode   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:PDTESTER.ManualWindow     5   	   'axisclr-namespace:PDTESTER     5   
     � Title$  FreeMode��5	   	   $	��360��6   $	��640��6'   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� � 5   
   $��0��6   $��2q�6   .+�5      �� $��1q�6   $��18(�6#   $��SemiBold"�61   $��Center=�6G   �� 5      $�PD Free Run��6   $��18(�6)   �� 5      �� 5      $�Axis#1 Pos:��6    �� 5      �6   ��  �� Path$ 
CurrentPos�   PDTESTER.Axis
   _01 �� Source# Z �� 5      �� 5      $�Axis#2 Pos:��6    �� 5      �6   �� $ 
CurrentPos�
   _02# Z 5      5      � 5    
   $��0��6   $��1��6   $��2q�6,   .+�5!      �� $��1q�6   $��18(�6#   $��SemiBold"�61   $��Center=�6G   �� 5"      $�NFC Free Run��6   $��18(�6*   �� 5#      �� 5$      $�Axis#3 Pos:��6    �� 5%      �6   �� $ 
CurrentPos�
   _03# Z �� 5&      �� 5'      $�Axis#4 Pos:��6    �� 5(      �6   �� $ 
CurrentPos�
   _04# Z 5)      5*      �� 5+   
   -   5        $��PDFreeRunButton��5+      $	��Run��6(   $��30q�66   $��1��6C   6P   �� 5,   
   -   5        $��NFCFreeRunButton��5,      $	��Run��6)   $��30q�68   $��1��6_   $��1��6l   �� 5-   
   -   5        $��CloseButton��5-      $��Close��6$   $��2��64   $��30q�6A   $��1��6M   $��60��6]   6i   5.      5/      