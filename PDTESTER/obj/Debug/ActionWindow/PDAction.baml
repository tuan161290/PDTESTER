   M S B A M L   `   `   `  ����C  ?PDTESTER, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:PDTESTERPDTESTER  5   	   $    PDTESTER.ActionWindow.PDAction   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &appclr-namespace:PDTESTER     5   	   -   5          � Title$  PDAction��5   	   $	��400��6   $	��720��6'    � ShowInTaskbar .  � 
ResizeMode$ NoResize=�5	   	    � WindowStartupLocation$ CenterScreen=�6    � 	Resources 5      #   PDTESTER.IsJigEnableConverter   IsJigEnableConverter��%   &	          5   
   6#   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� .+�5      �� $��18(�6   $
��Bold"�6&   �� 5      �6   �� *Jig.JigDescription+ �� 5      $� Action Window��6   5      5      �� 5   
   $��1��6   ��5      �� 5       $��Test Action��6   .��5"      �� $��Center=�6    �� Orientation$ 
Horizontal=�67   $��5q�6P   .+�5#      �� -   5        $��Reset Power��5#      $	��100��6E   $��5q�6Q   �� 5$      -   5        $��ResetUCT��5$      $	��100��6C   $��5q�6O   �� 5%      -   5        $
��TSON��5%      $	��100��6=   $��5q�6I   �� 5&      -   5        $
��TSOF��5&      $	��100��6=   $��5q�6I   �� 5'      -   5        $��Manual Test��5'      $	��100��6M   $��5q�6Y   �� 5(      -   5        $��
Abort Test��5(      $	��100��6G   $��5q�6S   5)      5*      �� 5+   
   $��2��6   ��5,      �� 5-      $��
Jig Action��6   .��5/      �� $��Center=�6   $ 
Horizontal=�67   $��5q�6P   .+�50      �� -   5        $��	Clear Set��50      $��Stretch=�6A   $	��100��6]   $��5q�6i   �� 51      -	   5        $��Clear Counter��51      $	��100��6N   $��5q�6Z   �� 52      -
   5        $	��100��53   -   $��5q�69   ��52      �� *Jig.IsJigEnable+ �� 	Converter# [  53   9   �� 54      -   5        $��	Mark FAIL��54      $	��100��55   2   $��5q�6>   �� 56      -   5        $��Mark OK��56      $	��100��57   0   $��5q�6<   58      59      �� 5:   
   $��3��6   ��5;      �� 5<      $��Selected Jig��6   .��5>      �� -   5        $��JigListView��5>       �� SelectionMode$ Multiple=�6*     ActionListViewStyle#��[ 6C   ��5?      �� 5@      .R�5A      A� 	 A� Columns$	 8��6&   5B      ��5D      �� 5E      .R�5F      � $��5q�6   .+�5G      �� $��SemiBold"�6(   $��16(�6>   $��Center=�6p   $��Center=�6�   ��6L   �� *Jig.JigDescription+6�   5H      5I      5K      5L      �� 5M   
   $��4��6   $��Right=�6"   .+�5N      �� -   5        $��Close��5N      $��Stretch=�6:   $	��100��6V   $��5q�6b   5O      5P      5Q      