﻿#pragma checksum "..\..\..\ManualWindow\NFCManual.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5159FC9C7BAF0D0539F671A8DE074D6BBF265643EE9E8327A7DF664B48B2EBF0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PDTESTER;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace PDTESTER.ManualWindow {
    
    
    /// <summary>
    /// NFCManual
    /// </summary>
    public partial class NFCManual : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 36 "..\..\..\ManualWindow\NFCManual.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView NFCGridView;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\ManualWindow\NFCManual.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LeftJog;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\ManualWindow\NFCManual.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button FLeftJog;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\ManualWindow\NFCManual.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RightJog;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\ManualWindow\NFCManual.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button FRightJog;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PDTESTER;component/manualwindow/nfcmanual.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ManualWindow\NFCManual.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\ManualWindow\NFCManual.xaml"
            ((PDTESTER.ManualWindow.NFCManual)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.UserControl_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 9 "..\..\..\ManualWindow\NFCManual.xaml"
            ((PDTESTER.ManualWindow.NFCManual)(target)).PreviewKeyUp += new System.Windows.Input.KeyEventHandler(this.UserControl_PreviewKeyUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.NFCGridView = ((System.Windows.Controls.ListView)(target));
            return;
            case 6:
            this.LeftJog = ((System.Windows.Controls.Button)(target));
            
            #line 94 "..\..\..\ManualWindow\NFCManual.xaml"
            this.LeftJog.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseDown);
            
            #line default
            #line hidden
            
            #line 94 "..\..\..\ManualWindow\NFCManual.xaml"
            this.LeftJog.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.FLeftJog = ((System.Windows.Controls.Button)(target));
            
            #line 96 "..\..\..\ManualWindow\NFCManual.xaml"
            this.FLeftJog.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseDown);
            
            #line default
            #line hidden
            
            #line 96 "..\..\..\ManualWindow\NFCManual.xaml"
            this.FLeftJog.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseUp);
            
            #line default
            #line hidden
            return;
            case 8:
            this.RightJog = ((System.Windows.Controls.Button)(target));
            
            #line 98 "..\..\..\ManualWindow\NFCManual.xaml"
            this.RightJog.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseDown);
            
            #line default
            #line hidden
            
            #line 98 "..\..\..\ManualWindow\NFCManual.xaml"
            this.RightJog.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseUp);
            
            #line default
            #line hidden
            return;
            case 9:
            this.FRightJog = ((System.Windows.Controls.Button)(target));
            
            #line 100 "..\..\..\ManualWindow\NFCManual.xaml"
            this.FRightJog.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseDown);
            
            #line default
            #line hidden
            
            #line 100 "..\..\..\ManualWindow\NFCManual.xaml"
            this.FRightJog.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseUp);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 104 "..\..\..\ManualWindow\NFCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SON_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 108 "..\..\..\ManualWindow\NFCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ORG_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 112 "..\..\..\ManualWindow\NFCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.STOP_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 116 "..\..\..\ManualWindow\NFCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RESET_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 138 "..\..\..\ManualWindow\NFCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Lift_03_UpDown_Clicked);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 143 "..\..\..\ManualWindow\NFCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Lift_03_UpDown_Clicked);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 148 "..\..\..\ManualWindow\NFCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LOADING_03_CLAMP_Clicked);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 153 "..\..\..\ManualWindow\NFCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LOADING_03_UNCLAMP_Clicked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 3:
            
            #line 58 "..\..\..\ManualWindow\NFCManual.xaml"
            ((System.Windows.Controls.Button)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.Pack_MouseDoubleClick);
            
            #line default
            #line hidden
            break;
            case 4:
            
            #line 61 "..\..\..\ManualWindow\NFCManual.xaml"
            ((System.Windows.Controls.Button)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.MovePos_DoubleClick);
            
            #line default
            #line hidden
            break;
            case 5:
            
            #line 62 "..\..\..\ManualWindow\NFCManual.xaml"
            ((System.Windows.Controls.Button)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.SavePos_DoubleClick);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

