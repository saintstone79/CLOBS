﻿#pragma checksum "C:\Users\Euisuk\Source\Repos\CLOBS2\CLOBS2\ObservationFramePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "659E8DE600C95029628408B6A8E260CB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CLOBS2
{
    partial class ObservationFramePage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // ObservationFramePage.xaml line 62
                {
                    this.ObservationItemFrame = (global::Windows.UI.Xaml.Controls.Frame)(target);
                }
                break;
            case 3: // ObservationFramePage.xaml line 63
                {
                    this.BottomButtonPanel = (global::Windows.UI.Xaml.Controls.RelativePanel)(target);
                }
                break;
            case 4: // ObservationFramePage.xaml line 64
                {
                    this.UpdateObservationInfoData = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.UpdateObservationInfoData).Click += this.UpdateObservationInfoData_Click;
                }
                break;
            case 5: // ObservationFramePage.xaml line 75
                {
                    this.EditPrevLog = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.EditPrevLog).Click += this.EditPrevLog_Click;
                }
                break;
            case 6: // ObservationFramePage.xaml line 86
                {
                    this.EndObservation = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.EndObservation).Click += this.EndObservation_Click;
                }
                break;
            case 7: // ObservationFramePage.xaml line 17
                {
                    this.OBSERVER_TXT = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8: // ObservationFramePage.xaml line 22
                {
                    this.ObserverName = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9: // ObservationFramePage.xaml line 27
                {
                    this.SCHOOL_TXT = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10: // ObservationFramePage.xaml line 33
                {
                    this.SchoolName = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11: // ObservationFramePage.xaml line 38
                {
                    this.TEACHER_TXT = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 12: // ObservationFramePage.xaml line 43
                {
                    this.TeacherName = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13: // ObservationFramePage.xaml line 47
                {
                    this.ObservationDate = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 14: // ObservationFramePage.xaml line 56
                {
                    this.SessionLapse = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
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
            return returnValue;
        }
    }
}

