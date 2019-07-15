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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CLOBS2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ObservationItemsPage : Page
    {
        private DispatcherTimer m_observationlogTimer;
        private DispatcherTimer m_observationLogTimeTick;
        private DateTime m_dtLogStarted;
        private bool m_bGapTimeLog = false;

        public ObservationItemsPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            InitObservationLogTimerWithoutInterval();
            InitObservationLogTimeTickWithInterval();
        }

        private void InitObservationLogTimeTickWithInterval()
        {
            m_observationLogTimeTick = new DispatcherTimer();
            m_observationLogTimeTick.Tick += M_observationLogTimeTick_Tick;
            m_observationLogTimeTick.Interval = new TimeSpan(0, 0, 0, 0, 100);
        }
        private void InitObservationLogTimerWithoutInterval()
        {
            m_observationlogTimer = new DispatcherTimer();
            m_observationlogTimer.Tick += M_observationLogTimer_Tick;
        }

        private void ResetObservationItems()
        {
            ST1AudienceWhole.IsChecked = false;
            ST1AudienceIndividual.IsChecked = false;
            ST1AudienceNone.IsChecked = false;
            ST1DisciplinarySTEM.IsChecked = false;
            ST1Activity.IsChecked = false;
            ST1HandsOn.IsChecked = false;
            ST1ClassroomManagement.IsChecked = false;
            ST1WalkAround.IsChecked = false;            
            ST1UnObservable.IsChecked = false;

            if (Stomper2Panel.Visibility == Visibility.Visible)
            {
                ST2AudienceWhole.IsChecked = false;
                ST2AudienceIndividual.IsChecked = false;
                ST2AudienceNone.IsChecked = false;
                ST2DisciplinarySTEM.IsChecked = false;
                ST2Activity.IsChecked = false;
                ST2HandsOn.IsChecked = false;
                ST2ClassroomManagement.IsChecked = false;
                ST2WalkAround.IsChecked = false;                
                ST2UnObservable.IsChecked = false;
            }
            if (Stomper3Panel.Visibility == Visibility.Visible)
            {
                ST3AudienceWhole.IsChecked = false;
                ST3AudienceIndividual.IsChecked = false;
                ST3AudienceNone.IsChecked = false;
                ST3DisciplinarySTEM.IsChecked = false;
                ST3Activity.IsChecked = false;
                ST3HandsOn.IsChecked = false;
                ST3ClassroomManagement.IsChecked = false;
                ST3WalkAround.IsChecked = false;                
                ST3UnObservable.IsChecked = false;
            }

            InterventionTradingCard.IsChecked = false;
            InterventionBioVideos.IsChecked = false;
            InterventionClassWork.IsChecked = false;
            InterventionPersonal.IsChecked = false;

            TeacherStepskIn.IsChecked = false;
            ObservationNote.Text = "";
        }
        public void FlushObservationItemData()
        {
            List<CLOBSClassStructure> classStructures = new List<CLOBSClassStructure>();
            if (WholeClass.IsChecked.Value) classStructures.Add(CLOBSClassStructure.WholeClass);
            if (GroupHandsOn.IsChecked.Value) classStructures.Add(CLOBSClassStructure.SmallGroupOrHandsOn);

            List<CLOBSAudience> eventST1Audiences = new List<CLOBSAudience>();
            if (ST1AudienceWhole.IsChecked.Value) eventST1Audiences.Add(CLOBSAudience.WholeClass);
            if (ST1AudienceIndividual.IsChecked.Value) eventST1Audiences.Add(CLOBSAudience.SmallGroupIndividual);
            if (ST1AudienceNone.IsChecked.Value) eventST1Audiences.Add(CLOBSAudience.None);

            List<CLOBSInteraction> eventST1Interactions = new List<CLOBSInteraction>();
            if (ST1DisciplinarySTEM.IsChecked.Value) eventST1Interactions.Add(CLOBSInteraction.STEMDisciplinary);
            if (ST1Activity.IsChecked.Value) eventST1Interactions.Add(CLOBSInteraction.ProjectActivity);
            if (ST1HandsOn.IsChecked.Value) eventST1Interactions.Add(CLOBSInteraction.HandsOnSupport);
            if (ST1ClassroomManagement.IsChecked.Value) eventST1Interactions.Add(CLOBSInteraction.ClassroomManagement);
            if (ST1WalkAround.IsChecked.Value) eventST1Interactions.Add(CLOBSInteraction.WalkAround);
            if (ST1UnObservable.IsChecked.Value) eventST1Interactions.Add(CLOBSInteraction.Unobservable);

            List<CLOBSAudience> eventST2Audiences = new List<CLOBSAudience>();
            List<CLOBSInteraction> eventST2Interactions = new List<CLOBSInteraction>();
            if (Stomper2Panel.Visibility == Visibility.Visible)
            {
                if (ST2AudienceWhole.IsChecked.Value) eventST2Audiences.Add(CLOBSAudience.WholeClass);
                if (ST2AudienceIndividual.IsChecked.Value) eventST2Audiences.Add(CLOBSAudience.SmallGroupIndividual);
                if (ST2AudienceNone.IsChecked.Value) eventST2Audiences.Add(CLOBSAudience.None);

                if (ST2DisciplinarySTEM.IsChecked.Value) eventST2Interactions.Add(CLOBSInteraction.STEMDisciplinary);
                if (ST2Activity.IsChecked.Value) eventST2Interactions.Add(CLOBSInteraction.ProjectActivity);
                if (ST2HandsOn.IsChecked.Value) eventST2Interactions.Add(CLOBSInteraction.HandsOnSupport);
                if (ST2ClassroomManagement.IsChecked.Value) eventST2Interactions.Add(CLOBSInteraction.ClassroomManagement);
                if (ST2WalkAround.IsChecked.Value) eventST2Interactions.Add(CLOBSInteraction.WalkAround);
                if (ST2UnObservable.IsChecked.Value) eventST2Interactions.Add(CLOBSInteraction.Unobservable);
            }

            List<CLOBSAudience> eventST3Audiences = new List<CLOBSAudience>();
            List<CLOBSInteraction> eventST3Interactions = new List<CLOBSInteraction>();

            if (Stomper3Panel.Visibility == Visibility.Visible)
            {
                if (ST3AudienceWhole.IsChecked.Value) eventST3Audiences.Add(CLOBSAudience.WholeClass);
                if (ST3AudienceIndividual.IsChecked.Value) eventST3Audiences.Add(CLOBSAudience.SmallGroupIndividual);
                if (ST3AudienceNone.IsChecked.Value) eventST3Audiences.Add(CLOBSAudience.None);

                if (ST3DisciplinarySTEM.IsChecked.Value) eventST3Interactions.Add(CLOBSInteraction.STEMDisciplinary);
                if (ST3Activity.IsChecked.Value) eventST3Interactions.Add(CLOBSInteraction.ProjectActivity);
                if (ST3HandsOn.IsChecked.Value) eventST3Interactions.Add(CLOBSInteraction.HandsOnSupport);
                if (ST3ClassroomManagement.IsChecked.Value) eventST3Interactions.Add(CLOBSInteraction.ClassroomManagement);
                if (ST3WalkAround.IsChecked.Value) eventST3Interactions.Add(CLOBSInteraction.WalkAround);
                if (ST3UnObservable.IsChecked.Value) eventST3Interactions.Add(CLOBSInteraction.Unobservable);
            }

            AddNewLog(classStructures, eventST1Audiences, eventST1Interactions, eventST2Audiences, eventST2Interactions, eventST3Audiences, eventST3Interactions,
                InterventionTradingCard.IsChecked.Value, InterventionBioVideos.IsChecked.Value, InterventionClassWork.IsChecked.Value, InterventionPersonal.IsChecked.Value, TeacherStepskIn.IsChecked.Value, ObservationNote.Text);
        }
        private void M_observationLogTimer_Tick(object sender, object e)
        {
            FlushObservationItemData();
            m_observationlogTimer.Stop();
            m_observationLogTimeTick.Stop();

            // if current observation log is additional log returned from gap time by user
            if (m_bGapTimeLog == true || ObservationManager.Instance.m_objInfoData.NoGapTime == true)
                this.Frame.Navigate(typeof(ObservationItemsPage));
            else
                this.Frame.Navigate(typeof(GapTimePage));
        }

        private void M_observationLogTimeTick_Tick(object sender, object e)
        {
            TimeSpan tsVideoTimeLapse = (DateTime.Now - m_dtLogStarted);
            ObservationLogLapse.Text = String.Format("{0:00}:{1:00}", tsVideoTimeLapse.Minutes, tsVideoTimeLapse.Seconds);
        }
        public void UpdateSTOMPERInfo(int nStomper, string newSTOMPER1, string newSTOMPER2 = null, string newSTOMPER3 = null)
        {
            Stomper1.Text = newSTOMPER1;

            switch (nStomper)
            {
                case 1:
                    Stomper2Panel.Visibility = Visibility.Collapsed;
                    Stomper3Panel.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    Stomper2Panel.Visibility = Visibility.Visible;
                    Stomper3Panel.Visibility = Visibility.Collapsed;
                    Stomper2.Text = newSTOMPER2;
                    break;
                case 3:
                    Stomper2Panel.Visibility = Visibility.Visible;
                    Stomper2.Text = newSTOMPER2;
                    Stomper3Panel.Visibility = Visibility.Visible;
                    Stomper3.Text = newSTOMPER3;
                    break;
            }
        }

        private void DisplaySTOMPerPanels(int nSTOMPers)
        {
            Stomper1.Text = ObservationManager.Instance.m_objInfoData.Stomper1;
            switch (nSTOMPers)
            {
                case 1:
                    Stomper1Panel.MinWidth = 600;
                    Stomper1Panel.MaxWidth = 800;
                    Stomper2Panel.Visibility = Visibility.Collapsed;
                    Stomper3Panel.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    Stomper1Panel.MinWidth = 500;
                    Stomper2Panel.MaxWidth = 800;
                    Stomper2Panel.MinWidth = 500;
                    Stomper2Panel.MaxWidth = 800;
                    Stomper2.Text = ObservationManager.Instance.m_objInfoData.Stomper2;
                    Stomper3Panel.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    Stomper1Panel.MinWidth = 355;
                    Stomper2Panel.MinWidth = 355;
                    Stomper3Panel.MinWidth = 355;
                    Stomper2.Text = ObservationManager.Instance.m_objInfoData.Stomper2;
                    Stomper3.Text = ObservationManager.Instance.m_objInfoData.Stomper3;
                    break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DisplaySTOMPerPanels(ObservationManager.Instance.m_objInfoData.StomperNumber);                        
            TimeSpan tsObjDuration;

            // Return from gap time when videographer wants to add log
            if (e.Parameter is TimeSpan && e.Parameter != null)
            {
                m_bGapTimeLog = true;
                GapTimeLogInfo.Visibility = Visibility.Visible;
                tsObjDuration = (TimeSpan)e.Parameter;
                ObservationLogDuration.Text = String.Format("{0:00}:{1:00}", tsObjDuration.Minutes, tsObjDuration.Seconds);
                m_observationlogTimer.Interval = tsObjDuration;
            }
            else // navigation by regular timer set
            {
                m_bGapTimeLog = false;
                GapTimeLogInfo.Visibility = Visibility.Collapsed;
                tsObjDuration = ObservationManager.Instance.m_objInfoData.ObservationDuration;

                // increase log index here
                ObservationManager.Instance.m_nLogIndex++;
            }
            LogIndex.Text = ObservationManager.Instance.m_nLogIndex.ToString();
            ObservationLogDuration.Text = String.Format("{0:00}:{1:00}", tsObjDuration.Minutes, tsObjDuration.Seconds);
            m_observationlogTimer.Interval = tsObjDuration;

            m_observationlogTimer.Start();
            m_dtLogStarted = DateTime.Now;
            m_observationLogTimeTick.Start();
            ResetObservationItems();

            base.OnNavigatedTo(e);
        }
        private void AddNewLog(List<CLOBSClassStructure> classStructures, List<CLOBSAudience> ST1Audiences, List<CLOBSInteraction> ST1Interactions,
            List<CLOBSAudience> ST2Audiences, List<CLOBSInteraction> ST2Interactions,
            List<CLOBSAudience> ST3Audiences, List<CLOBSInteraction> ST3Interactions,
            bool InterventionTradingCard, bool InterventionBioVideo, bool InterventionClassExemple, bool InterventionPersonal, bool TeacherStepsIn, string strLogNote)
        {
            ObservationLogData newLog = new ObservationLogData(ObservationManager.Instance.m_nLogIndex, m_dtLogStarted - ObservationManager.Instance.m_dtInitTime + ObservationManager.Instance.m_objInfoData.VideoElasped);
            newLog.EventTimeStarted = (DateTime.Now - ObservationManager.Instance.m_dtInitTime) + ObservationManager.Instance.m_objInfoData.VideoElasped;
            newLog.classStructures = classStructures;
            newLog.ST1Audiences = ST1Audiences;
            newLog.ST1Interactions = ST1Interactions;
            newLog.ST2Audiences = ST2Audiences;
            newLog.ST2Interactions = ST2Interactions;
            newLog.ST3Audiences = ST3Audiences;
            newLog.ST3Interactions = ST3Interactions;
            newLog.InterventionTradingCard = InterventionTradingCard;
            newLog.InterventionBioVideo = InterventionBioVideo;
            newLog.InterventionClassExemple = InterventionClassExemple;
            newLog.InterventionPersonal = InterventionPersonal;
            newLog.TeacherStepsIn = TeacherStepsIn;
            newLog.LogNote = strLogNote;
            ObservationManager.Instance.m_lsObLogData.Add(newLog);
            ObservationManager.Instance.WriteLogsToFile();
            //ObservationManager.Instance.AppendNewLogToFile(newLog);
        }        
    }
}
