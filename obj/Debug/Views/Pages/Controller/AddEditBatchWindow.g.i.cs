﻿#pragma checksum "..\..\..\..\..\Views\Pages\Controller\AddEditBatchWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3783F7F1B4176D384DB9BE5C82886069181820F203B96A9FE20AE70D95EE1CBE"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using PrimeTrack.Views.Pages.Controller;
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


namespace PrimeTrack.Views.Pages.Controller {
    
    
    /// <summary>
    /// AddEditBatchWindow
    /// </summary>
    public partial class AddEditBatchWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\..\Views\Pages\Controller\AddEditBatchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BatchCodeTextBox;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\..\Views\Pages\Controller\AddEditBatchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker ProductionDatePicker;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\..\Views\Pages\Controller\AddEditBatchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ProductionTimeTextBox;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\..\Views\Pages\Controller\AddEditBatchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox WarehouseCodeTextBox;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\..\Views\Pages\Controller\AddEditBatchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ClientCodeTextBox;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\..\Views\Pages\Controller\AddEditBatchWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker ShipmentDatePicker;
        
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
            System.Uri resourceLocater = new System.Uri("/PrimeTrack;component/views/pages/controller/addeditbatchwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\Pages\Controller\AddEditBatchWindow.xaml"
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
            this.BatchCodeTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.ProductionDatePicker = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 3:
            this.ProductionTimeTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.WarehouseCodeTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.ClientCodeTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.ShipmentDatePicker = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 7:
            
            #line 30 "..\..\..\..\..\Views\Pages\Controller\AddEditBatchWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 31 "..\..\..\..\..\Views\Pages\Controller\AddEditBatchWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

