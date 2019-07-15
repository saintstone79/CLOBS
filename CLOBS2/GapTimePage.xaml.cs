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
using System.Diagnostics;
using CLOBS2.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CLOBS2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GapTimePage : Page
    {
        private DispatcherTimer m_TimeTick;
        private DispatcherTimer m_logGapTimer;
        private DateTime m_dtGapStartTime;

        public GapTimePage()
        {
            this.InitializeComponent();
            //this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            m_TimeTick = new DispatcherTimer();
            m_TimeTick.Tick += M_TimeTick_Tick;
            m_TimeTick.Interval = new TimeSpan(0, 0, 0, 0, 100);

            m_logGapTimer = new DispatcherTimer();
            m_logGapTimer.Tick += M_logGapTimer_Tick;
        }

        private void M_logGapTimer_Tick(object sender, object e)
        {
            FlushObservationItemData();
            m_logGapTimer.Stop();
            m_TimeTick.Stop();

            this.Frame.Navigate(typeof(ObservationItemsPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            m_logGapTimer.Interval = ObservationManager.Instance.m_objInfoData.GapDuration;
            m_dtGapStartTime = DateTime.Now;
            m_logGapTimer.Start();
            m_TimeTick.Start();
            GapTimeNote.Text = "";
            base.OnNavigatedTo(e);
        }

        private void M_TimeTick_Tick(object sender, object e)
        {
            TimeSpan tsWaitTime = ObservationManager.Instance.m_objInfoData.GapDuration - (DateTime.Now - m_dtGapStartTime) + TimeSpan.FromSeconds(1);
            GapTime.Text = String.Format("{0:00}:{1:00}", tsWaitTime.Minutes, tsWaitTime.Seconds);
        }

        private void btnShowNote_Click(object sender, RoutedEventArgs e)
        {
            if (btnShowNote.IsChecked.Value)
                GapTimeNote.Visibility = Visibility.Visible;
            else
                GapTimeNote.Visibility = Visibility.Collapsed;
        }

        private void btnReturnLogSheet_Click(object sender, RoutedEventArgs e)
        {
            GapButtonPanel.Visibility = Visibility.Collapsed;
            btnShowNote.IsChecked = false;
            GapTimeNote.Visibility = Visibility.Collapsed;
            LogUpdateFrame.Visibility = Visibility.Visible;
            LogUpdateFrame.Navigate(typeof(ObservationItemUpdatePage));            
        }

        public void ShowButtonPanel()
        {
            GapButtonPanel.Visibility = Visibility.Visible;            
        }

        public void FlushObservationItemData()
        {
            if (string.IsNullOrEmpty(GapTimeNote.Text) == false)
            {
                ObservationLogData newLog = new ObservationLogData(ObservationManager.Instance.m_nLogIndex, m_dtGapStartTime - ObservationManager.Instance.m_dtInitTime + ObservationManager.Instance.m_objInfoData.VideoElasped);
                newLog.LogNote = "[GapTimeNote]"  + GapTimeNote.Text;
                ObservationManager.Instance.m_lsObLogData.Add(newLog);
                ObservationManager.Instance.WriteLogsToFile();
                //ObservationManager.Instance.AppendNewNoteToFile(m_dtGapStartTime - ObservationManager.Instance.m_dtInitTime + ObservationManager.Instance.m_objInfoData.VideoElasped, GapTimeNote.Text);
            }                
        }
    }
}