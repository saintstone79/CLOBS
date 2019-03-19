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
        private DateTime m_tsGapStartTime;

        public GapTimePage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            m_TimeTick = new DispatcherTimer();
            m_TimeTick.Tick += M_TimeTick_Tick;
            m_TimeTick.Interval = new TimeSpan(0, 0, 0, 0, 100);

            m_logGapTimer = new DispatcherTimer();
            m_logGapTimer.Tick += M_logGapTimer_Tick;
        }

        private void M_logGapTimer_Tick(object sender, object e)
        {
            if (string.IsNullOrEmpty(GapTimeNote.Text) == false)                            
                ObservationManager.Instance.AppendNewNoteToFile(m_tsGapStartTime - ObservationManager.Instance.m_dtInitTime + ObservationManager.Instance.m_objInfoData.VideoElasped
                    , GapTimeNote.Text);            

            m_logGapTimer.Stop();
            m_TimeTick.Stop();

            this.Frame.Navigate(typeof(SampledObservationItemsPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            m_logGapTimer.Interval = ObservationManager.Instance.m_objInfoData.GapDuration;
            m_tsGapStartTime = DateTime.Now;
            m_logGapTimer.Start();
            m_TimeTick.Start();
            GapTimeNote.Text = "";
            base.OnNavigatedTo(e);
        }

        private void M_TimeTick_Tick(object sender, object e)
        {
            TimeSpan tsWaitTime = ObservationManager.Instance.m_objInfoData.GapDuration - (DateTime.Now - m_tsGapStartTime) + TimeSpan.FromSeconds(1);
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
            if (string.IsNullOrEmpty(GapTimeNote.Text) == false)
                ObservationManager.Instance.AppendNewNoteToFile(m_tsGapStartTime - ObservationManager.Instance.m_dtInitTime + ObservationManager.Instance.m_objInfoData.VideoElasped, GapTimeNote.Text);

            m_logGapTimer.Stop();
            m_TimeTick.Stop();

            this.Frame.Navigate(typeof(SampledObservationItemsPage), (TimeSpan)ObservationManager.Instance.m_objInfoData.GapDuration - (DateTime.Now - m_tsGapStartTime));
        }

        public void FlushObservationItemData()
        {
            if (string.IsNullOrEmpty(GapTimeNote.Text) == false)
                ObservationManager.Instance.AppendNewNoteToFile(m_tsGapStartTime - ObservationManager.Instance.m_dtInitTime + ObservationManager.Instance.m_objInfoData.VideoElasped, GapTimeNote.Text);
        }
    }
}