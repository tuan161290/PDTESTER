   M S B A M L   `   `   `  ����C  ?PDTESTER, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PDTESTER.ActionWindowPDTESTER.ActionWindow  5   	   #clr-namespace:PDTESTERPDTESTER  5   	   a ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes 5   
   .    (PDTESTER.ActionWindow.InputGripperManual   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:PDTESTER.ActionWindow     5   	   &appclr-namespace:PDTESTER     5   	   ]ThemesJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero     5   
     � Title$  InputGripperManual��5
   	   $	��300��6$   $	��600��61    � WindowStyle$ SingleBorderWindow=�6=    � 
ResizeMode$ NoResize=�6^    � WindowStartupLocation$ CenterScreen=�6t    � ShowInTaskbar .   � 	Resources 5       �� 
TargetType �� Property �� Value	 �� 
TargetType+  %Microsoft.Windows.Themes.ButtonChrome
  Name   BorderBrush   
Background   RenderPressed �� IsDefaulted   RenderDefaulted J� SnapsToDevicePixels �� ContentStringFormat �� ContentStringFormat �� Triggers P� Property P� Value �� 
TargetName   ButtonBaseControlTemplate1 J� IsMouseOver   RenderMouseOver$   PDTESTER.StateToColorConverter  StateToColorConverter��%�  '��       &	  d    &	 �    �� 5   
   " ��6   .�5      ��  �@��IsPressAndHoldEnabled .  6=   �� 5       � �� 5      �� 5      "	 ��6&   .R�5        $
 Chrome��6.   # z��6>   # z��5   .   # z��5   .   # z 5   .    . ._�5      �� #��z��6/   #��z��6c   # z 6�   #��z��6�   #��z��6  ��. # z 6D  #��z��6�  5       5      P� 5       �@d�	IsChecked$
 TrueY�6H   . �5   "   �� $ Chrome��6B    �@ RenderPressed . 6V   5      P� 5       � {� .  . �5   "   ��  � �� �	#FFADADAD6?   5      5      5!      5"      �� 5#   
   "	 ��6=   .R�5$        $
 Chrome��6"   # z��62   # z��6^   # z 6�   # z��6�   # z 6�    . ._�5%      �� #��z��6#   #��z��6W   # z 6{   #��z��6�   #��z��6�   ��. # z 68  #��z��6t  5&       5'      P� 5(       �@J�IsKeyboardFocused . . �5)      �� $ Chrome��68    �@ RenderDefaulted . 6L   5*      P� 5+       �@d�	IsChecked$
 TrueY�6<   . �5,      �� $ Chrome��66    �@ RenderPressed . 6J   5-      P� 5.       � {� .  . �5/      ��  � �� �	#FFADADAD63   50      52        53   
   6$   54      .��55      � $��10q�6    � RowDefinitions 56   
   �� 57      $
��Auto �6   �� 58      $��* �6   �� 59      $��* �6   �� 5:      $
��Auto �6   .+�5<   
   �� .+�5=      �� $!��Input Gripper Manual Window��6   $��18(�6;   $
��Bold"�6I   5>      �� 5?   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   .+�5@      �� ��5A      �� 5B      $��Up/Down Manual��6    .��5D      �� $��Center=�6   $ 
Horizontal=�6;   $��5q�6T   .+�5E      �� -   5        $
��Down��5E      $��Stretch=�5H      $	��100��69   $��10q�6E   ��5G      ��  �� Path$% LOADING_03_DOWN_SENSOR.PinValue� �� Current �� Source# Z  �� 	Converter# [ 5H   E   �� 5I      -   5        $��Up��5I      $��Stretch=�5L      $	��100��69   $��10q�6E   ��5K      �� $# LOADING_03_UP_SENSOR.PinValue�# Z # [ 5L   E   5N      5O      �� 5P      ��5Q      �� 5R      $��Go Left/Right Manual��6    .��5T      �� $��Center=�6   $ 
Horizontal=�6;   $��5q�6T   .+�5U      �� -   5        $
��Left��5U      $��Stretch=�5W      $	��100��61   $��10q�6=   ��5V      �� $) #TRANS_CYLINDER_LEFT_SENSOR.PinValue�# Z # [ 5W   =   �� 5X      -   5        $��Right��5X      $��Stretch=�5Z   )   $	��100��6E   $��10q�6Q   ��5Y      �� $* $TRANS_CYLINDER_RIGHT_SENSOR.PinValue�# Z # [ 5Z   Q   5[      5\      5]      �� 5^   
   $��2��6   $ 
Horizontal=�6"   $��Center=�6;   .+�5_      �� ��5`      �� 5a      $��Clamp Manual��6    .��5c      �� $��Center=�6   $ 
Horizontal=�6;   $��5q�6T   .+�5d      �� -   5        $��Unclamp��5d      $��Stretch=�5g      $	��100��69   $��10q�6E   ��5f      �� $( "LOADING_03_UNCLAMP_SENSOR.PinValue�# Z # [ 5g   E   �� 5i      -   5        $��Clamp��5i      $��Stretch=�5l      $	��100��69   $��10q�6E   ��5k      �� $&  LOADING_03_CLAMP_SENSOR.PinValue�# Z # [ 5l   E   5m      5n      �� 5o      ��5p      �� 5q      $��Turn Manual��6    .��5s      �� $��Center=�6   $ 
Horizontal=�6;   $��5q�6T   .+�5t      �� -   5        $
��Turn��5t      $��Stretch=�5v      $	��100��69   $��10q�6E   ��5u      �� $' !LOADING_03_TURNED_SENSOR.PinValue�# Z # [ 5v   E   �� 5w      -   5        $��Return��5w      $��Stretch=�5y   4   $	��100��6P   $��10q�6\   ��5x      �� $) #LOADING_03_RETURNED_SENSOR.PinValue�# Z # [ 5y   \   5z      5{      5|      �� 5}   
   $��3��6   $��Right=�6"   .+�5~      �� -	   5        $��Close��5~      $��Stretch=�69   $	��100��6U   $��5q�6a   5      5�      5�      