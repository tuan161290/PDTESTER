﻿#pragma checksum "..\..\..\ManualWindow\FreeMode.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3876D408C0C787D039762D3309942B1A65338B30C64D25327EBE9FD89B4EF5DF"
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
using PDTESTER.ManualWindow;
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
    /// FreeMode
    /// </summary>
    public partial class FreeMode : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\..\ManualWindow\FreeMode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PDFreeRunButton;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\ManualWindow\FreeMode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NFCFreeRunButton;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\ManualWindow\FreeMode.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseButton;
        
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
            System.Uri resourceLocater = new System.Uri("/PDTESTER;component/manualwindow/freemode.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ManualWindow\FreeMode.xaml"
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
            this.PDFreeRunButton = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\..\ManualWindow\FreeMode.xaml"
            this.PDFreeRunButton.Click += new System.Windows.RoutedEventHandler(this.PDFreeRun_Clicked);
            
            #line default
            #line hidden
            return;
            case 2:
            this.NFCFreeRunButton = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\..\ManualWindow\FreeMode.xaml"
            this.NFCFreeRunButton.Click += new System.Windows.RoutedEventHandler(this.NFCFreeRun_Clicked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.CloseButton = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\..\ManualWindow\FreeMode.xaml"
            this.CloseButton.Click += new System.Windows.RoutedEventHandler(this.Close_Clicked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

