   M S B A M L   `   `   `  ���� C  ?PDTESTER, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:PDTESTERPDTESTER      PDTESTER.IOMonitor   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:PDTESTER       � Title$  
IO Monitor��$	��768��$
��1280�� � 
ResizeMode$ CanResizeWithGrip=� � WindowStartupLocation$ CenterScreen=� � ShowInTaskbar .  � Topmost .  � 	Resources $   PDTESTER.StateToColorConverter   StateToColorConverter'   !PDTESTER.PinValueToGreenConverter  PinValueToGreenConverter*   $PDTESTER.StateToEffectColorConverter  StateToEffectColorConverter��%-   &	        &	      &	 
           .��� $��5q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto ��� $��* ��� $
��Auto ��� $
��Auto � � ColumnDefinitions �� $��* ��� $��* ��� $��* �.+�� $��3��.+��� $��0,0,0,1q�
���   �._��� $��IO Monitor Window��$��24(�$
��Bold"��� $��1�����	#FF7a96ea$��1q�._��� $��INF0_00 - INF0_31��$��2q�$��Center=�$��16(�$��SemiBold"��� $��2������  �� Path$ GPIOBoardF0.InputPins�	 �� Current
 �� Source#
 Z	 ���� .R��� 
�������$��0,0,0,1q�._���  �� Orientation$ 
Horizontal=�$��7q�.+�A� $��18��$��18��$��0,0,20,0q�$��Center=����� *
PinValue+ �� 	Converter# [ �� $��Center=�$��14(�$��SemiBold"��� $�::�� �� ��� *	GPIOLabel+ �� $�::�� �� ��� *GPIODescription+�� $��3�����	#FF7a96ea$��0,5,0,0q�$��1q�._��� $��OUF0_00 - OUF0_23��$��2q�$��Center=�$��16(�$��SemiBold"��� $��4������ $ GPIOBoardF0.OutputPins�#
 Z	 ���� .R��� 
�������$��0,0,0,1q�._��� $ 
Horizontal=�$��7q�.+�A� $��18��$��18��$��0,0,20,0q�$��Center=����� *
PinValue+# [ �� $��Center=�$��14(�$��SemiBold"��� $�::�� �� ��� *	GPIOLabel+ �� $�::�� �� ��� *GPIODescription+�� -   $��ON��$��80���� -   $	��OFF��$��80���� $��1��$��1�����	#FF7a96ea$��1q�._��� $��INF1_00 - INF1_31��$��2q�$��Center=�$��16(�$��SemiBold"��� $��2��$��1������ $ GPIOBoardF1.InputPins�#
 Z	 ���� .R��� 
�������$��0,0,0,1q�._��� $ 
Horizontal=�$��7q�.+�A� $��18��$��18��$��0,0,20,0q�$��Center=����� *
PinValue+# [ �� $��Center=�$��14(�$��SemiBold"��� $�::�� �� ��� *	GPIOLabel+ �� $�::�� �� ��� *GPIODescription+�� $��3��$��1�����	#FF7a96ea$��0,5,0,0q�$��1q�._��� $��OUF1_00 - OUF1_23��$��2q�$��Center=�$��16(�$��SemiBold"��� $��4��$��1������ $ GPIOBoardF1.OutputPins�#
 Z	 ���� .R��� 
�������$��0,0,0,1q�._��� $ 
Horizontal=�$��7q�.+�A� $��18��$��18��$��0,0,20,0q�$��Center=����� *
PinValue+# [ �� $��Center=�$��14(�$��SemiBold"��� $�::�� �� ��� *	GPIOLabel+ �� $�::�� �� ��� *GPIODescription+�� -   $��ON��$��80���� -   $	��OFF��$��80���� $��1��$��2�����	#FF7a96ea$��1q�._��� $��INF2_00 - INF2_31��$��2q�$��Center=�$��16(�$��SemiBold"��� $��2��$��2������ $ GPIOBoardF2.InputPins�#
 Z	 ���� .R��� 
�������$��0,0,0,1q�._��� $ 
Horizontal=�$��7q�.+�A� $��18��$��18��$��0,0,20,0q�$��Center=����� *
PinValue+# [ �� $��Center=�$��14(�$��SemiBold"��� $�::�� �� ��� *	GPIOLabel+ �� $�::�� �� ��� *GPIODescription+�� $��3��$��2�����	#FF7a96ea$��0,5,0,0q�$��1q�._��� $��OUF2_00 - OUF2_23��$��2q�$��Center=�$��16(�$��SemiBold"��� $��4��$��2������ $ GPIOBoardF2.OutputPins�#
 Z	 ���� .R��� 
�������$��0,0,0,1q�._��� $ 
Horizontal=�$��7q�.+�A� $��18��$��18��$��0,0,20,0q�$��Center=����� *
PinValue+# [ �� $��Center=�$��14(�$��SemiBold"��� $�::�� �� ��� *	GPIOLabel+ �� $�::�� �� ��� *GPIODescription+�� -   $��ON��$��80���� -   $	��OFF��$��80��� $��5��$��3��.+��� -   $��Close��$��40��$	��200��$��10q�$��Right=�