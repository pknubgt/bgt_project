﻿#pragma checksum "D:\Project\Project_bgt\bgt_project\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E8849A56C58BD289FBE72817B9F6842F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BGTviewer
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_InkToolbar_TargetInkCanvas(global::Windows.UI.Xaml.Controls.InkToolbar obj, global::Windows.UI.Xaml.Controls.InkCanvas value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Windows.UI.Xaml.Controls.InkCanvas) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Windows.UI.Xaml.Controls.InkCanvas), targetNullValue);
                }
                obj.TargetInkCanvas = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_SymbolIcon_Symbol(global::Windows.UI.Xaml.Controls.SymbolIcon obj, global::Windows.UI.Xaml.Controls.Symbol value)
            {
                obj.Symbol = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class MainPage_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IDataTemplateComponent,
            global::Windows.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IMainPage_Bindings
        {
            private global::BGTviewer.MainPage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.InkToolbar obj41;
            private global::Windows.UI.Xaml.Controls.SymbolIcon obj48;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj41TargetInkCanvasDisabled = false;
            private static bool isobj48SymbolDisabled = false;

            public MainPage_obj1_Bindings()
            {
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 33 && columnNumber == 46)
                {
                    isobj41TargetInkCanvasDisabled = true;
                }
                else if (lineNumber == 35 && columnNumber == 37)
                {
                    isobj48SymbolDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 41: // MainPage.xaml line 33
                        this.obj41 = (global::Windows.UI.Xaml.Controls.InkToolbar)target;
                        break;
                    case 48: // MainPage.xaml line 35
                        this.obj48 = (global::Windows.UI.Xaml.Controls.SymbolIcon)target;
                        break;
                    default:
                        break;
                }
            }

            // IDataTemplateComponent

            public void ProcessBindings(global::System.Object item, int itemIndex, int phase, out int nextPhase)
            {
                throw new global::System.NotImplementedException();
            }

            public void Recycle()
            {
                throw new global::System.NotImplementedException();
            }

            // IMainPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                if (newDataRoot != null)
                {
                    this.dataRoot = (global::BGTviewer.MainPage)newDataRoot;
                    return true;
                }
                return false;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::BGTviewer.MainPage obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_inkCanvas(obj.inkCanvas, phase);
                        this.Update_LassoSelect(obj.LassoSelect, phase);
                    }
                }
            }
            private void Update_inkCanvas(global::Windows.UI.Xaml.Controls.InkCanvas obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // MainPage.xaml line 33
                    if (!isobj41TargetInkCanvasDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_InkToolbar_TargetInkCanvas(this.obj41, obj, null);
                    }
                }
            }
            private void Update_LassoSelect(global::Windows.UI.Xaml.Controls.Symbol obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // MainPage.xaml line 35
                    if (!isobj48SymbolDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_SymbolIcon_Symbol(this.obj48, obj);
                    }
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // MainPage.xaml line 1
                {
                    this.Page = (global::Windows.UI.Xaml.Controls.Page)(target);
                }
                break;
            case 2: // MainPage.xaml line 21
                {
                    this.titlePanel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 3: // MainPage.xaml line 73
                {
                    this.instruction = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4: // MainPage.xaml line 75
                {
                    this.selectionCanvas = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 5: // MainPage.xaml line 78
                {
                    this.inkCanvas = (global::Windows.UI.Xaml.Controls.InkCanvas)(target);
                }
                break;
            case 6: // MainPage.xaml line 112
                {
                    this.fA = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.fA).Click += this.Bt_fA;
                }
                break;
            case 7: // MainPage.xaml line 113
                {
                    this.f1 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.f1).Click += this.Bt_f1;
                }
                break;
            case 8: // MainPage.xaml line 114
                {
                    this.f2 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.f2).Click += this.Bt_f2;
                }
                break;
            case 9: // MainPage.xaml line 115
                {
                    this.f3 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.f3).Click += this.Bt_f3;
                }
                break;
            case 10: // MainPage.xaml line 116
                {
                    this.f4 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.f4).Click += this.Bt_f4;
                }
                break;
            case 11: // MainPage.xaml line 117
                {
                    this.f5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.f5).Click += this.Bt_f5;
                }
                break;
            case 12: // MainPage.xaml line 118
                {
                    this.f6 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.f6).Click += this.Bt_f6;
                }
                break;
            case 13: // MainPage.xaml line 119
                {
                    this.f7 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.f7).Click += this.Bt_f7;
                }
                break;
            case 14: // MainPage.xaml line 120
                {
                    this.f8 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.f8).Click += this.Bt_f8;
                }
                break;
            case 15: // MainPage.xaml line 121
                {
                    this.scale_up = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.scale_up).Click += this.Bt_UP;
                }
                break;
            case 16: // MainPage.xaml line 122
                {
                    this.scale_down = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.scale_down).Click += this.Bt_DOWN;
                }
                break;
            case 17: // MainPage.xaml line 127
                {
                    this.징후1 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 18: // MainPage.xaml line 128
                {
                    this.점수1 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 19: // MainPage.xaml line 129
                {
                    this.징후2 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 20: // MainPage.xaml line 130
                {
                    this.점수2 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 21: // MainPage.xaml line 133
                {
                    this.도형A의위치 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.도형A의위치).Click += this.Bt_PA;
                }
                break;
            case 22: // MainPage.xaml line 134
                {
                    this.PA = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 23: // MainPage.xaml line 137
                {
                    this.공간사용 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.공간사용).Click += this.Bt_US;
                }
                break;
            case 24: // MainPage.xaml line 138
                {
                    this.US = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 25: // MainPage.xaml line 140
                {
                    this.중첩 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.중첩).Click += this.Bt_RR;
                }
                break;
            case 26: // MainPage.xaml line 141
                {
                    this.RR = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 27: // MainPage.xaml line 147
                {
                    this.교차곤란 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.교차곤란).Click += this.Bt_CS;
                }
                break;
            case 28: // MainPage.xaml line 148
                {
                    this.CS = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 29: // MainPage.xaml line 153
                {
                    this.지각적회전 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.지각적회전).Click += this.Bt_RT;
                }
                break;
            case 30: // MainPage.xaml line 154
                {
                    this.RT = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 31: // MainPage.xaml line 156
                {
                    this.퇴영 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.퇴영).Click += this.Bt_RG;
                }
                break;
            case 32: // MainPage.xaml line 157
                {
                    this.RG = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 33: // MainPage.xaml line 164
                {
                    this.중복곤란 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.중복곤란).Click += this.Bt_OD;
                }
                break;
            case 34: // MainPage.xaml line 165
                {
                    this.OD = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 35: // MainPage.xaml line 181
                {
                    this.LineChart = (global::WinRTXamlToolkit.Controls.DataVisualization.Charting.Chart)(target);
                }
                break;
            case 36: // MainPage.xaml line 196
                {
                    this.report = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 37: // MainPage.xaml line 182
                {
                    this.Pressure = (global::WinRTXamlToolkit.Controls.DataVisualization.Charting.LineSeries)(target);
                }
                break;
            case 38: // MainPage.xaml line 188
                {
                    this.Selected = (global::WinRTXamlToolkit.Controls.DataVisualization.Charting.LineSeries)(target);
                }
                break;
            case 39: // MainPage.xaml line 76
                {
                    this.img = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 40: // MainPage.xaml line 30
                {
                    global::Windows.UI.Xaml.Controls.Image element40 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)element40).Tapped += this.Bt_ImageTappedLoad;
                }
                break;
            case 41: // MainPage.xaml line 33
                {
                    this.toolbar = (global::Windows.UI.Xaml.Controls.InkToolbar)(target);
                }
                break;
            case 42: // MainPage.xaml line 39
                {
                    global::Windows.UI.Xaml.Controls.Image element42 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)element42).Tapped += this.Bt_PartPressure;
                }
                break;
            case 43: // MainPage.xaml line 48
                {
                    this.clear = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.clear).Click += this.Bt_ClearDrawnBoundingRect;
                }
                break;
            case 44: // MainPage.xaml line 43
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element44 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element44).Click += this.Bt_useStroke;
                }
                break;
            case 45: // MainPage.xaml line 44
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element45 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element45).Click += this.Bt_useAuto;
                }
                break;
            case 46: // MainPage.xaml line 45
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element46 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element46).Click += this.Bt_InitAngleButton;
                }
                break;
            case 47: // MainPage.xaml line 34
                {
                    this.toolButtonLasso = (global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton)(target);
                    ((global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton)this.toolButtonLasso).Click += this.Bt_ToolButtonLasso;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1: // MainPage.xaml line 1
                {                    
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)target;
                    MainPage_obj1_Bindings bindings = new MainPage_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                    global::Windows.UI.Xaml.Markup.XamlBindingHelper.SetDataTemplateComponent(element1, bindings);
                }
                break;
            }
            return returnValue;
        }
    }
}

