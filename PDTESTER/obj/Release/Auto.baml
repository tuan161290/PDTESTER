   M S B A M L   `   `   `  ���� C  ?PDTESTER, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:PDTESTERPDTESTER      PDTESTER.Auto   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     (localclr-namespace:PDTESTER       N� Title$
  Auto��$
��1024��$	��660�� � 	Resources "   PDTESTER.IsSetInJigConverter   IsSetInJigConverter,   &PDTESTER.PinStateToVisibilityConverter ! PinStateToVisibilityConverter)   #PDTESTER.TestResultToColorConverter  TestResultToColorConverter'   !PDTESTER.TestItemToColorConverter  TestItemToColorConverter(   "PDTESTER.PinValueToOrangeConverter  PinValueToOrangeConverter%   PDTESTER.PinValueToRedConverter  PinValueToRedConverter(   "PDTESTER.PinValueToPurpleConverter  PinValueToPurpleConverter'   !PDTESTER.PinValueToGreenConverter  PinValueToGreenConverter&	    PDTESTER.TextToTimeSpanConverter  TextToTimeSpanConverter'
   !PDTESTER.boolToRoyalBlueConverter 	 boolToRoyalBlueConverter.   (PDTESTER.PackingPinStateToColorConverter #
 PackingPinStateToColorConverter   PDTESTER.App  AppProperty��%�   &	        &	      &	 
     &	      &	      &	      &	      &	 #     &	 (     &		 -     &	
 2     &	 7                     	  
      .��� #��� ��$��10q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� �� $
��Auto ��� $
��Auto ��� $
��0.7* ��� $
��0.5* � � ColumnDefinitions �� $
��0.8* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $
��0.8* ��� $
��0.8* ��� $��2* �.+��� $��13��$��10,20,20,20q�.+��� -   $��TestTextblock���� $�	PD TESTER��$��32(�$
��Bold"�� $��1��$��13��$��1q����� *NFC_LEAK_RUN+ �� 	Converter# [	 .+��� $��NFC-LEAK SIDE��$��20(�$��Center=�$��10q�� $��2��$��0��$��1q����� *
PinValue+   PDTESTER.OUT   TRANSFER_CV_RELAY �� Source# Z # [ .+��� $��TRANCV��$��16(�$��Center=�$	��5,2q�� $��3��$��0��$��6��
����İ�$��1q� �� �� .+��� $��Assert/Note.PNG��$��28��$	��Top=�$��10q�y��� *
PinValue+   PDTESTER.IN   TRANSFER_CV_SENSOR_END# Z # [  J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect  	   	Direction$	 0Q�
   RenderingBias$
 Quality=�   ShadowDepth$ 0Q�   
BlurRadius$ 3Q��� $��Assert/Note.PNG��$��28��$��1��$	��Top=�$��10q�y��� *
PinValue+   TRANSFER_CV_SENSOR_BEGIN# Z # [    $	 0Q�$
 Quality=�$ 0Q�$ 3Q�� $��2��$��1��$��3��$��10��
������� �� $��4* ��� $��* ��� $��4* �.+��� -   $��NFCGridView��$��0q����� .R�A�  A� Columns$ 4������ .R��  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� -   $��0������ *Jig.TestResult+# [ .+��� $��16(�$��Center=�$��2q����� *Jig.JigDescription+� $��1��$��10q�.+��� $��Assert/Note.PNG��$��28��y��� *Jig.IsSetInJig+# [     $	 0Q�$
 Quality=�$ 0Q�$ 3Q��� $��2��.+��� $	��5,0q�$��14(����� *Jig.TestResult+�� $��14(�$��Center=�$��3��$	��5,0q��� ��� *Jig.ElapseTime+�� $��14(�$��Center=�$��4��$	��5,0q��� ��� *Jig.OKCounter+ �� StringFormat$ P: {0}���� $��14(�$��5��$	��5,0q��� 
���  ����� *Jig.FailRate+$ {0:0.0}%��� $��1�� �� $
��Auto ��� $
��Auto �.+�� -   $��NFCBufferGrid1��$��1q����� *Jig.TestResult+# [ .+��� $��BUFFER��$��16(�$��Center=�$��2q�� -   $��NFCBufferGrid2��$��1��
�������.+��  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� $��0��$��10q�.+��� $��Assert/Note.PNG��$��28��y��� *Jig.IsSetInJig+# [     $	 0Q�$
 Quality=�$ 0Q�$ 3Q��� $��1��.+��� $	��5,1q�$��14(����� *Jig.TestResult+�� -   $��LEAKGridView��$��0q�$��2��$��4������ .R�A� $ 4������ .R��  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� -   $��0������ *Jig.TestResult+# [ .+��� $��16(�$��Center=�$��2q����� *Jig.JigDescription+� $��1��$��10q�.+��� $��Assert/Note.PNG��$��28��y��� *Jig.IsSetInJig+# [     $	 0Q�$
 Quality=�$ 0Q�$ 3Q��� $��2��.+��� $��2��$	��5,0q�$��14(����� *Jig.TestResult+�� $��14(�$��Center=�$��3��$	��5,0q��� ��� *Jig.ElapseTime+�� $��14(�$��Center=�$��4��$	��5,0q��� ��� *Jig.OKCounter+$ P: {0}���� $��14(�$��5��$	��5,0q��� 
���  ����� *Jig.FailRate+$ {0:0.0}%��� $��2��$��11��
���2�2�$��1q�.+��� $��OUT_CV��$��16(�$��Center=�$��2q�� $��3��$��11��$��2��$��1,0,1,0q�
����İ� �� $��* ��� $��* �.+��� $��Assert/Note.PNG��$��28��$	��Top=�$
��10,2q�y��� *
PinValue+   UNLOADING_CV_SENSOR_03# Z # [    $	 10Q�$
 Quality=�$ 0Q�$ 3Q��� $��Assert/Note.PNG��$��28��$��Center=�$
��10,2q�$��1��y��� *
PinValue+   UNLOADING_CV_SENSOR_02# Z # [    $	 0Q�$
 Quality=�$ 0Q�$ 3Q�� $��2��$��12��
���  ��$��1q�.+��� $��NG_CV��$��16(�$��Center=�$	��5,2q�� $��3��$��12��$��2��$��1,0,1,-1q�
����İ�.+��� $��Assert/Note.PNG��$��28��y��� *
PinValue+   NG_CV_SENSOR_01# Z # [    $	 0Q�$
 Quality=�$ 0Q�$ 3Q�� $��6��$��12��$��3��$��1,-1,1,1q�
����İ�.+��� $��Assert/Note.PNG��$��28��$��10q�$	��Top=�y��� *
PinValue+   NG_CV_SENSOR_02# Z # [    $	 0Q�$
 Quality=�$ 0Q�$ 3Q��� $��Assert/Note.PNG��$��28��$��10q�y��� *
PinValue+   NG_CV_SENSOR_03# Z # [    $	 0Q�$
 Quality=�$ 0Q�$ 3Q��� $��Assert/Note.PNG��$��28��$��Bottom=�$��10q�y��� *
PinValue+   NG_CV_SENSOR_04# Z # [    $	 0Q�$
 Quality=�$ 0Q�$ 3Q�� $��5��$��13��$��1q����� *PDRUN+# [	  �� Mode$ OneWay=�.+��� $��20(�$��Center=�$��10q��� $�TVOC-PD SIDE��$��20(� �� ��� *	TotalPD+ �� ��� *
RetestRate+�� $��5��$��9��$��4��$��Bottom=�$��Right=����� *PDProcessingStep+�� $��5��$��FF��$��1��$��4��$��Bottom=�$
��Left=�� $��2��$��3��$��6��$��1��.+��� -   $��SIMGridView��$��0q����� .R�A� $ 2������ .R��  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� -	   $��0��$��-1q����� *Jig.TestResult+# [ .+��� $��16(�$��Center=�$��2q����� *Jig.JigDescription+� $��1��$��10q�.+��� $��Assert/Note.PNG��$��28��y��� *Jig.IsSetInJig+# [     $	 0Q�$
 Quality=�$ 0Q�$ 3Q��� $��2��$	��5,0q��� $��14(���� *Jig.TestResult+�� �� $��14(���� *Jig.ElapseTime+�� �� ��� *SIMTest.NVResult+$ SIM: {0}���� �� ��� *SDCardTest.NVResult+$ SD: {0}���� �� ��� *NFCTest.NVResult+$ NFC: {0}���� $��14(�$��Center=�$��4��$	��5,0q��� ��� *Jig.OKCounter+$ P: {0}���� $��14(�$��5��$	��5,0q��� 
���  ����� *Jig.FailRate+$ {0:0.0}%��� -
   $��6��$��3�����	#FF7a96ea$��1q�.+��� $��BUFFER��$��16(�$��Center=�$��2q�� -   $��Axis01BufferGrid��$��7��$��3��$��2��
�������$��1q� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� $��0��$��10q�.+��� $��Assert/Note.PNG��$��28��y��� *
IsSetInJig+# [     $	 0Q�$
 Quality=�$ 0Q�$ 3Q�� $��6��$��4��$��3��$��7��.+��� -   $��
PDGridView��$��0q����� .R�A� $ 8������ .R��  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� -   $��0��$��-1q����� *Jig.TestResult+# [ .+��� $��16(�$��Center=�$��2q����� *Jig.JigDescription+� $��1��$��10q�.+��� $��Assert/Note.PNG��$��28��y��� *Jig.IsSetInJig+# [     $	 0Q�$
 Quality=�$ 0Q�$ 3Q��� $��2��$	��5,0q��� $��14(���� *Jig.JigState+�� �� $��14(���� *Jig.ElapseTime+�� $��14(�$��Center=�$��4��$	��5,0q��� ��� *Jig.OKCounter+$ P: {0}���� $��14(�$��5��$	��5,0q��� 
���  ����� *Jig.FailRate+$ {0:0.0}%��� $��6��$��11��$��1,1,1,0q����� *
PinValue+   LOADING_CV_02_RELAY# Z # [ .+��� $��IN_CV2��$��16(�$��Center=�$��2q�� $��7��$��11��$��2��
����İ�$��1,0,1,1q� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* �.+��� $��Assert/Note.PNG��$��28��$��10q�y��� *
PinValue+   INPUT_CV_SENSOR_02# Z # [    $	 0Q�$
 Quality=�$ 0Q�$ 3Q�� $��1������ *
PinValue+   LOADING_CV_01_RELAY# Z # [ .+��� $��IN_CV1��$��16(�$��Center=�$��2q��� $��Assert/Note.PNG��$��28��$��2��$��Center=�$��10q�y��� *
PinValue+   INPUT_CV_SENSOR_01# Z # [    $	 0Q�$
 Quality=�$ 0Q�$ 3Q�� $��1��$��13��$��8��.+���  �� Orientation$ Vertical=�$	��Top=�.+��� -   $��PauseButton��$��Pause��$��5,0,5,5q�$��60��
��� � �$y�	Collapsed=��� -   $��StartButton��{�.  $��Start��$��5,0,5,5q�$��60��
��� � ��� -   $��
StopButton��$
��Stop��$��5q�$��60��
���  ���� -   $��	ORGButton��$	��ORG��$��5q�$��60��
��� ����� -   $��ResetButton��$��Reset��$��5q�$��60��
����� ��� -   $	��5,1q�.���� -   $��CounterTextblock��$��14(�$��Center=� �� TextAlignment$ Center=�
��� � ��� $�Pass:�� �� ��� *	TotalOK+�� -   $	��5,1q�.���� -   $��OkTextBlock��$��14(�$��Center=�
���  ���� $�Fail: �� �� ��� *	TotalFail+�� $	��5,1q�$��14(�.���� ���� *	CycleTime+$ TT {0:mm\:ss}���� $	��5,1q�$��14(�.���� ���� *AVTime+$ Avg: {0:0.0}���� $	��5,1q�$��14(�.���� $��V.020119��