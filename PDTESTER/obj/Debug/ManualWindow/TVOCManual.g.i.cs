﻿#pragma checksum "..\..\..\ManualWindow\TVOCManual.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "BC1A830DE255C9D08E9F9BA6743DC35F8A1DB4E7954ED94A7DA8729D92A47C85"
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
    /// TVOCManual
    /// </summary>
    public partial class TVOCManual : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 35 "..\..\..\ManualWindow\TVOCManual.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView TVOCGridView;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\ManualWindow\TVOCManual.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LeftJog;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\ManualWindow\TVOCManual.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button FLeftJog;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\ManualWindow\TVOCManual.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RightJog;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\ManualWindow\TVOCManual.xaml"
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
            System.Uri resourceLocater = new System.Uri("/PDTESTER;component/manualwindow/tvocmanual.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ManualWindow\TVOCManual.xaml"
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
            
            #line 8 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((PDTESTER.ManualWindow.TVOCManual)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.UserControl_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((PDTESTER.ManualWindow.TVOCManual)(target)).PreviewKeyUp += new System.Windows.Input.KeyEventHandler(this.UserControl_PreviewKeyUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TVOCGridView = ((System.Windows.Controls.ListView)(target));
            return;
            case 7:
            this.LeftJog = ((System.Windows.Controls.Button)(target));
            
            #line 98 "..\..\..\ManualWindow\TVOCManual.xaml"
            this.LeftJog.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseDown);
            
            #line default
            #line hidden
            
            #line 98 "..\..\..\ManualWindow\TVOCManual.xaml"
            this.LeftJog.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseUp);
            
            #line default
            #line hidden
            return;
            case 8:
            this.FLeftJog = ((System.Windows.Controls.Button)(target));
            
            #line 100 "..\..\..\ManualWindow\TVOCManual.xaml"
            this.FLeftJog.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseDown);
            
            #line default
            #line hidden
            
            #line 100 "..\..\..\ManualWindow\TVOCManual.xaml"
            this.FLeftJog.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseUp);
            
            #line default
            #line hidden
            return;
            case 9:
            this.RightJog = ((System.Windows.Controls.Button)(target));
            
            #line 102 "..\..\..\ManualWindow\TVOCManual.xaml"
            this.RightJog.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseDown);
            
            #line default
            #line hidden
            
            #line 102 "..\..\..\ManualWindow\TVOCManual.xaml"
            this.RightJog.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseUp);
            
            #line default
            #line hidden
            return;
            case 10:
            this.FRightJog = ((System.Windows.Controls.Button)(target));
            
            #line 104 "..\..\..\ManualWindow\TVOCManual.xaml"
            this.FRightJog.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseDown);
            
            #line default
            #line hidden
            
            #line 104 "..\..\..\ManualWindow\TVOCManual.xaml"
            this.FRightJog.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Button_MouseUp);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 109 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SON_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 113 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ORG_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 117 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.STOP_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 121 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RESET_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 143 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Lift_02_UpDown_Clicked);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 148 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Lift_02_UpDown_Clicked);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 153 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LOADING_02_CLAMP_Clicked);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 158 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LOADING_02_UNCLAMP_Clicked);
            
            #line default
            #line hidden
            return;
            case 19:
            
            #line 163 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LOADING_02_TURN_Clicked);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 168 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LOADING_01_TURN_Clicked);
            
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
            
            #line 60 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.Pack_MouseDoubleClick);
            
            #line default
            #line hidden
            break;
            case 4:
            
            #line 63 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.Pack_MouseDoubleClick);
            
            #line default
            #line hidden
            break;
            case 5:
            
            #line 65 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.MovePos_DoubleClick);
            
            #line default
            #line hidden
            break;
            case 6:
            
            #line 66 "..\..\..\ManualWindow\TVOCManual.xaml"
            ((System.Windows.Controls.Button)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.SavePos_DoubleClick);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

