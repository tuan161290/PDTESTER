   M S B A M L   `   `   `  ����C  ?PDTESTER, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:PDTESTER.ActionWindowPDTESTER.ActionWindow  5   	   #clr-namespace:PDTESTERPDTESTER  5   	   *    $PDTESTER.ActionWindow.Buffer01Action   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:PDTESTER.ActionWindow     5   	   &appclr-namespace:PDTESTER     5   	     � Title$  TVOC Action��5	   	   $	��300��6   $	��600��6*    � ShowInTaskbar .   � 
ResizeMode$ NoResize=�6L    � WindowStartupLocation$ CenterScreen=�6b    � 	Resources 5
      #   PDTESTER.IsJigEnableConverter   IsSetEnableConverter  IsJigEnableConverter��%:   &	        &	        5   
   6#     5   
   6#   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� .+�5      �� $��Buffer Action Window��6   $��18(�64   $
��Bold"�6B   5      �� 5   
   $��1��6   ��5      �� 5      $��Buffer Action��6   .��5      �� $��Center=�6    �� Orientation$ 
Horizontal=�67   $��5q�6P   .+�5      �� -   5        $
��Pack��5      $��Stretch=�6=   $	��100��6Y   $��10q�6e   �� 5       -   5        $��Unpack��5       $��Stretch=�6A   $	��100��6]   $��10q�6i   5!      5"      �� 5#   
   $��2��6   ��5$      �� 5%      $��
Jig Action��6   .��5'      �� $��Center=�6   $ 
Horizontal=�67   $��5q�6P   .+�5(      �� -   5        $��	Clear Set��5(      $��Stretch=�6C   $	��100��6_   $��10q�6k   �� 5)      -   5        $	��100��5*   -   $��5q�69   ��5)      �� *Jig.IsJigEnable+ �� 	Converter# [ 5*   9   5+      5,      �� 5-   
   $��3��6   $��Right=�6"   .+�5.      �� -   5        $��Close��5.      $��Stretch=�6:   $	��100��6V   $��5q�6b   5/      50      51      