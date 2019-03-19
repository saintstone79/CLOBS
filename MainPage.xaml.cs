using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using CLOBS2.Models;
using Windows.UI.Core.Preview;
using System.Globalization;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CLOBS2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>    
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        DispatcherTimer m_sessionTick;
        private DateTime dtStartTime;

        public MainPage()
        {
            this.InitializeComponent();
            Current = this;

            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void M_sessionTick_Tick(object sender, object e)
        {
            TimeSpan tsSessionTimeLapse = (DateTime.Now - dtStartTime) + ObservationManager.Instance.m_objInfoData.VideoElasped;
            SessionLapse.Text = String.Format("{0:00}:{1:00}:{2:00}", tsSessionTimeLapse.Hours, tsSessionTimeLapse.Minutes, tsSessionTimeLapse.Seconds);
        }

        public void StartObservation()
        {
            m_sessionTick = new DispatcherTimer();
            m_sessionTick.Tick += M_sessionTick_Tick;
            m_sessionTick.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dtStartTime = DateTime.Now;
            m_sessionTick.Start();
            ObservationItemFrame.Navigate(typeof(SampledObservationItemsPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ObservationInfoData && e.Parameter != null)
            {
                ObservationInfoData objInfoData = (ObservationInfoData)e.Parameter;
                ObserverName.Text = objInfoData.ObserverName;
                SchoolName.Text = objInfoData.SchoolName;
                TeacherName.Text = objInfoData.TeacherName;
                ObservationDate.Text = objInfoData.ObservationDate.ToString("D", DateTimeFormatInfo.InvariantInfo);


                if (ObservationManager.Instance.m_bStarted == false)
                {
                    ObservationManager.Instance.StartObservation(objInfoData);
                    StartObservation();
                }
                else
                {
                    ObservationManager.Instance.UpdateObservationInfo(objInfoData);
                    if (ObservationItemFrame.Content is SampledObservationItemsPage)
                    {
                        var childPage = (SampledObservationItemsPage)ObservationItemFrame.Content;
                        childPage.UpdateSTOMPERInfo(objInfoData.StomperNumber, objInfoData.Stomper1, objInfoData.Stomper2, objInfoData.Stomper3);
                    }
                }
            }
            else
            {
                this.Frame.Navigate(typeof(ObservInfoPage));
            }
            base.OnNavigatedTo(e);
        }

        private void UpdateObservationInfoData_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ObservInfoPage));
        }

        private async void EndObservation_Click(object sender, RoutedEventArgs e)
        {
            var yesCommand = new UICommand("Yes");
            var noCommand = new UICommand("No");

            var dialog = new MessageDialog("Are you sure finishing this observation?", "End Observation");
            dialog.Commands.Add(yesCommand);
            dialog.Commands.Add(noCommand);
            var command = await dialog.ShowAsync();

            if (command == yesCommand)
            {
                // handle yes command
                if (ObservationItemFrame.Content is GapTimePage)
                {
                    //objManager.SaveLogFile();
                    var childPage = (GapTimePage)ObservationItemFrame.Content;
                    childPage.FlushObservationItemData();
                    ObservationManager.Instance.m_dtEndTime = DateTime.Now;
                    this.Frame.Navigate(typeof(SampledObservationSummaryPage));
                }
                else
                {
                    var childPage = (SampledObservationItemsPage)ObservationItemFrame.Content;
                    childPage.FlushObservationItemData();                    
                    ObservationManager.Instance.m_dtEndTime = DateTime.Now;
                    this.Frame.Navigate(typeof(SampledObservationSummaryPage));
                }
            }
            else if (command == noCommand)
            {
                // handle no command
                return;
            }
        }
    }
}
