﻿#pragma checksum "..\..\..\ActionWindow\LeakAction - Copy.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "718BC6B45782887F94A9B9CCEDCA693D12E90E1F"
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
using PDTESTER.ActionWindow;
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


namespace PDTESTER.ActionWindow {
    
    
    /// <summary>
    /// LeakAction
    /// </summary>
    public partial class LeakAction : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\ActionWindow\LeakAction - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ManualButton;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\ActionWindow\LeakAction - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AbortButton;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\ActionWindow\LeakAction - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView LeakListView;
        
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
            System.Uri resourceLocater = new System.Uri("/PDTESTER;component/actionwindow/leakaction%20-%20copy.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ActionWindow\LeakAction - Copy.xaml"
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
            this.ManualButton = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\ActionWindow\LeakAction - Copy.xaml"
            this.ManualButton.Click += new System.Windows.RoutedEventHandler(this.ManualTest_MouseClick);
            
            #line default
            #line hidden
            return;
            case 2:
            this.AbortButton = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\ActionWindow\LeakAction - Copy.xaml"
            this.AbortButton.Click += new System.Windows.RoutedEventHandler(this.AbortTest_MouseClick);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 40 "..\..\..\ActionWindow\LeakAction - Copy.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Clear_MouseClick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 41 "..\..\..\ActionWindow\LeakAction - Copy.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Use_Cliked);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 42 "..\..\..\ActionWindow\LeakAction - Copy.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ClearCounter_Cliked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.LeakListView = ((System.Windows.Controls.ListView)(target));
            return;
            case 7:
            
            #line 65 "..\..\..\ActionWindow\LeakAction - Copy.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

