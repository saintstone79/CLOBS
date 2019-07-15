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
using CLOBS2.Models;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CLOBS2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ObservationSummaryPage : Page
    {
        public ObservationSummaryPage()
        {
            this.InitializeComponent();
        }

        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            ObservationManager.Instance.m_objInfoData.ObservationNote = ObservationNote.Text;
            //await ObservationManager.Instance.SaveSessionInfoWithStatistics();
            await Windows.System.Launcher.LaunchFolderAsync(ObservationManager.Instance.m_storageFolder);
            Application.Current.Exit();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            ObservationManager objManager = ObservationManager.Instance;

            ObservationStarted.Text = objManager.m_dtInitTime.ToString("HH:mm:ss");
            ObservationEnded.Text = objManager.m_dtEndTime.ToString("HH:mm:ss");
            ObservationNote.Text = objManager.m_objInfoData.ObservationNote;
            ObservationLogLocation.Text = objManager.m_storageFile.Path;
            //ObservationNoteLocation.Text = objManager.m_storageNoteFile.Path;

            TotalLogCount.Text = objManager.GetLogTotalLogCounts().ToString();
            TotalWholeClassCount.Text = objManager.GetClassStructureCount(CLOBSClassStructure.WholeClass).ToString();
            TotalHandsOnCount.Text = objManager.GetClassStructureCount(CLOBSClassStructure.SmallGroupOrHandsOn).ToString();

            ST1Name.Text = objManager.m_objInfoData.Stomper1;
            ST1WholeClassCt.Text = objManager.GetEventST1AudienceCount(CLOBSAudience.WholeClass).ToString();
            ST1SmallGroupCt.Text = objManager.GetEventST1AudienceCount(CLOBSAudience.SmallGroupIndividual).ToString();
            ST1NoneCt.Text = objManager.GetEventST1AudienceCount(CLOBSAudience.None).ToString();
            ST1STEMDisciplinaryCt.Text = objManager.GetEventST1InteractionCount(CLOBSInteraction.STEMDisciplinary).ToString();
            ST1ActivityCt.Text = objManager.GetEventST1InteractionCount(CLOBSInteraction.ProjectActivity).ToString();
            ST1ClassroomManagementCt.Text = objManager.GetEventST1InteractionCount(CLOBSInteraction.ClassroomManagement).ToString();
            ST1HandsOnCt.Text = objManager.GetEventST1InteractionCount(CLOBSInteraction.HandsOnSupport).ToString();
            ST1WalkAroundCt.Text = objManager.GetEventST1InteractionCount(CLOBSInteraction.WalkAround).ToString();
            ST1UnobservableCt.Text = objManager.GetEventST1InteractionCount(CLOBSInteraction.Unobservable).ToString();

            if (objManager.m_objInfoData.StomperNumber > 1)
            {
                PanelSTOMPER2.Visibility = Visibility.Visible;
                ST2Name.Text = objManager.m_objInfoData.Stomper2;
                ST2WholeClassCt.Text = objManager.GetEventST2AudienceCount(CLOBSAudience.WholeClass).ToString();
                ST2SmallGroupCt.Text = objManager.GetEventST2AudienceCount(CLOBSAudience.SmallGroupIndividual).ToString();
                ST2NoneCt.Text = objManager.GetEventST2AudienceCount(CLOBSAudience.None).ToString();

                ST2STEMDisciplinaryCt.Text = objManager.GetEventST2InteractionCount(CLOBSInteraction.STEMDisciplinary).ToString();
                ST2ActivityCt.Text = objManager.GetEventST2InteractionCount(CLOBSInteraction.ProjectActivity).ToString();
                ST2ClassroomManagementCt.Text = objManager.GetEventST2InteractionCount(CLOBSInteraction.ClassroomManagement).ToString();
                ST2HandsOnCt.Text = objManager.GetEventST2InteractionCount(CLOBSInteraction.HandsOnSupport).ToString();
                ST2WalkAroundCt.Text = objManager.GetEventST2InteractionCount(CLOBSInteraction.WalkAround).ToString();
                ST2UnobservableCt.Text = objManager.GetEventST2InteractionCount(CLOBSInteraction.Unobservable).ToString();
            }

            if (objManager.m_objInfoData.StomperNumber > 2)
            {
                PanelSTOMPER3.Visibility = Visibility.Visible;
                ST3Name.Text = objManager.m_objInfoData.Stomper3;
                ST3WholeClassCt.Text = objManager.GetEventST3AudienceCount(CLOBSAudience.WholeClass).ToString();
                ST3SmallGroupCt.Text = objManager.GetEventST3AudienceCount(CLOBSAudience.SmallGroupIndividual).ToString();
                ST3NoneCt.Text = objManager.GetEventST3AudienceCount(CLOBSAudience.None).ToString();

                ST3STEMDisciplinaryCt.Text = objManager.GetEventST3InteractionCount(CLOBSInteraction.STEMDisciplinary).ToString();
                ST3ActivityCt.Text = objManager.GetEventST3InteractionCount(CLOBSInteraction.ProjectActivity).ToString();
                ST3ClassroomManagementCt.Text = objManager.GetEventST3InteractionCount(CLOBSInteraction.ClassroomManagement).ToString();
                ST3HandsOnCt.Text = objManager.GetEventST3InteractionCount(CLOBSInteraction.HandsOnSupport).ToString();
                ST3WalkAroundCt.Text = objManager.GetEventST3InteractionCount(CLOBSInteraction.WalkAround).ToString();
                ST3UnobservableCt.Text = objManager.GetEventST3InteractionCount(CLOBSInteraction.Unobservable).ToString();
            }

            InterventionTradingCardCt.Text = objManager.GetInterventionTradingCount().ToString();
            InterventionVideosBiosCt.Text = objManager.GetInterventionBioVideoCount().ToString();
            InterventionClassWorkExampleCt.Text = objManager.GetInterventionClassroomExcmpleCount().ToString();
            InterventionPersonalCt.Text = objManager.GetInterventionPersonal().ToString();
            
            base.OnNavigatedTo(e);
        }
    }
}
