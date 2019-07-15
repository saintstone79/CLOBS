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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CLOBS2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ObservationItemUpdatePage : Page
    {        
        public ObservationItemUpdatePage()
        {
            this.InitializeComponent();            
        }                      

        private void UpdateObservationItems()
        {            
            ObservationLogData currentLogData = ObservationManager.Instance.GetCurrentLogData();
            WholeClass.IsChecked = false;
            GroupHandsOn.IsChecked = false;
            foreach(CLOBSClassStructure aStructure in currentLogData.classStructures)
            {
                switch (aStructure)
                {
                    case CLOBSClassStructure.WholeClass:
                        WholeClass.IsChecked = true;
                        break;
                    case CLOBSClassStructure.SmallGroupOrHandsOn:
                        GroupHandsOn.IsChecked = true;
                        break;
                }
            }
            ST1AudienceWhole.IsChecked = false;
            ST1AudienceIndividual.IsChecked = false;
            ST1AudienceNone.IsChecked = false;
            foreach (CLOBSAudience audience in currentLogData.ST1Audiences)
            {
                switch (audience)
                {
                    case CLOBSAudience.WholeClass:
                        ST1AudienceWhole.IsChecked = true;
                        break;
                    case CLOBSAudience.SmallGroupIndividual:
                        ST1AudienceIndividual.IsChecked = true;
                        break;
                    case CLOBSAudience.None:
                        ST1AudienceNone.IsChecked = true;
                        break;
                }
            }
            
            ST1DisciplinarySTEM.IsChecked = false;
            ST1Activity.IsChecked = false;
            ST1HandsOn.IsChecked = false;
            ST1ClassroomManagement.IsChecked = false;
            ST1WalkAround.IsChecked = false;            
            ST1UnObservable.IsChecked = false;
            foreach (CLOBSInteraction interaction in currentLogData.ST1Interactions)
            {
                switch (interaction)
                {
                    case CLOBSInteraction.STEMDisciplinary:
                        ST1DisciplinarySTEM.IsChecked = true;
                        break;
                    case CLOBSInteraction.ProjectActivity:
                        ST1Activity.IsChecked = true;
                        break;
                    case CLOBSInteraction.HandsOnSupport:
                        ST1HandsOn.IsChecked = true;
                        break;
                    case CLOBSInteraction.ClassroomManagement:
                        ST1ClassroomManagement.IsChecked = true;
                        break;
                    case CLOBSInteraction.WalkAround:
                        ST1WalkAround.IsChecked = true;
                        break;
                    case CLOBSInteraction.Unobservable:
                        ST1UnObservable.IsChecked = true;
                        break;
                }
            }

            if (Stomper2Panel.Visibility == Visibility.Visible)
            {
                ST2AudienceWhole.IsChecked = false;
                ST2AudienceIndividual.IsChecked = false;
                ST2AudienceNone.IsChecked = false;
                foreach (CLOBSAudience audience in currentLogData.ST2Audiences)
                {
                    switch (audience)
                    {
                        case CLOBSAudience.WholeClass:
                            ST2AudienceWhole.IsChecked = true;
                            break;
                        case CLOBSAudience.SmallGroupIndividual:
                            ST2AudienceIndividual.IsChecked = true;
                            break;
                        case CLOBSAudience.None:
                            ST2AudienceNone.IsChecked = true;
                            break;
                    }
                }

                ST2DisciplinarySTEM.IsChecked = false;
                ST2Activity.IsChecked = false;
                ST2HandsOn.IsChecked = false;
                ST2ClassroomManagement.IsChecked = false;
                ST2WalkAround.IsChecked = false;                
                ST2UnObservable.IsChecked = false;
                foreach (CLOBSInteraction interaction in currentLogData.ST2Interactions)
                {
                    switch (interaction)
                    {
                        case CLOBSInteraction.STEMDisciplinary:
                            ST2DisciplinarySTEM.IsChecked = true;
                            break;
                        case CLOBSInteraction.ProjectActivity:
                            ST2Activity.IsChecked = true;
                            break;
                        case CLOBSInteraction.HandsOnSupport:
                            ST2HandsOn.IsChecked = true;
                            break;
                        case CLOBSInteraction.ClassroomManagement:
                            ST2ClassroomManagement.IsChecked = true;
                            break;
                        case CLOBSInteraction.WalkAround:
                            ST2WalkAround.IsChecked = true;
                            break;
                        case CLOBSInteraction.Unobservable:
                            ST2UnObservable.IsChecked = true;
                            break;
                    }
                }
            }
            if (Stomper3Panel.Visibility == Visibility.Visible)
            {
                ST3AudienceWhole.IsChecked = false;
                ST3AudienceIndividual.IsChecked = false;
                ST3AudienceNone.IsChecked = false;
                foreach (CLOBSAudience audience in currentLogData.ST3Audiences)
                {
                    switch (audience)
                    {
                        case CLOBSAudience.WholeClass:
                            ST3AudienceWhole.IsChecked = true;
                            break;
                        case CLOBSAudience.SmallGroupIndividual:
                            ST3AudienceIndividual.IsChecked = true;
                            break;
                        case CLOBSAudience.None:
                            ST3AudienceNone.IsChecked = true;
                            break;
                    }
                }

                ST3DisciplinarySTEM.IsChecked = false;
                ST3Activity.IsChecked = false;
                ST3HandsOn.IsChecked = false;
                ST3ClassroomManagement.IsChecked = false;
                ST3WalkAround.IsChecked = false;                
                ST3UnObservable.IsChecked = false;
                foreach (CLOBSInteraction interaction in currentLogData.ST3Interactions)
                {
                    switch (interaction)
                    {
                        case CLOBSInteraction.STEMDisciplinary:
                            ST3DisciplinarySTEM.IsChecked = true;
                            break;
                        case CLOBSInteraction.ProjectActivity:
                            ST3Activity.IsChecked = true;
                            break;
                        case CLOBSInteraction.HandsOnSupport:
                            ST3HandsOn.IsChecked = true;
                            break;
                        case CLOBSInteraction.ClassroomManagement:
                            ST3ClassroomManagement.IsChecked = true;
                            break;
                        case CLOBSInteraction.WalkAround:
                            ST3WalkAround.IsChecked = true;
                            break;
                        case CLOBSInteraction.Unobservable:
                            ST3UnObservable.IsChecked = true;
                            break;
                    }
                }
            }

            InterventionTradingCard.IsChecked = currentLogData.InterventionTradingCard;
            InterventionBioVideos.IsChecked = currentLogData.InterventionBioVideo;
            InterventionClassWork.IsChecked = currentLogData.InterventionClassExemple;
            InterventionPersonal.IsChecked = currentLogData.InterventionPersonal;

            TeacherStepskIn.IsChecked = currentLogData.TeacherStepsIn;
            ObservationNote.Text = currentLogData.LogNote;
        }
                                
        private void UpdateSTOMPERInfo()
        {
            ObservationInfoData objInfo = ObservationManager.Instance.m_objInfoData;
            Stomper1.Text = objInfo.Stomper1;

            switch (objInfo.StomperNumber)
            {
                case 1:
                    Stomper2Panel.Visibility = Visibility.Collapsed;
                    Stomper3Panel.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    Stomper2Panel.Visibility = Visibility.Visible;
                    Stomper3Panel.Visibility = Visibility.Collapsed;
                    Stomper2.Text = objInfo.Stomper2;
                    break;
                case 3:
                    Stomper2Panel.Visibility = Visibility.Visible;
                    Stomper2.Text = objInfo.Stomper2;
                    Stomper3Panel.Visibility = Visibility.Visible;
                    Stomper3.Text = objInfo.Stomper3;
                    break;
            }
        }

        private void DisplaySTOMPerPanels()
        {
            Stomper1.Text = ObservationManager.Instance.m_objInfoData.Stomper1;
            int nSTOMPers = ObservationManager.Instance.m_objInfoData.StomperNumber;
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
            DisplaySTOMPerPanels();
            UpdateSTOMPERInfo();
            UpdateObservationItems();
            base.OnNavigatedTo(e);
        }

        public void FlushObservationItemData()
        {
            ObservationLogData currentLogData = ObservationManager.Instance.GetCurrentLogData();

            currentLogData.classStructures.Clear();
            if (WholeClass.IsChecked.Value) currentLogData.classStructures.Add(CLOBSClassStructure.WholeClass);
            if (GroupHandsOn.IsChecked.Value) currentLogData.classStructures.Add(CLOBSClassStructure.SmallGroupOrHandsOn);

            currentLogData.ST1Audiences.Clear();
            if (ST1AudienceWhole.IsChecked.Value) currentLogData.ST1Audiences.Add(CLOBSAudience.WholeClass);
            if (ST1AudienceIndividual.IsChecked.Value) currentLogData.ST1Audiences.Add(CLOBSAudience.SmallGroupIndividual);
            if (ST1AudienceNone.IsChecked.Value) currentLogData.ST1Audiences.Add(CLOBSAudience.None);

            currentLogData.ST1Interactions.Clear();
            if (ST1DisciplinarySTEM.IsChecked.Value) currentLogData.ST1Interactions.Add(CLOBSInteraction.STEMDisciplinary);
            if (ST1Activity.IsChecked.Value) currentLogData.ST1Interactions.Add(CLOBSInteraction.ProjectActivity);
            if (ST1HandsOn.IsChecked.Value) currentLogData.ST1Interactions.Add(CLOBSInteraction.HandsOnSupport);
            if (ST1ClassroomManagement.IsChecked.Value) currentLogData.ST1Interactions.Add(CLOBSInteraction.ClassroomManagement);
            if (ST1WalkAround.IsChecked.Value) currentLogData.ST1Interactions.Add(CLOBSInteraction.WalkAround);
            if (ST1UnObservable.IsChecked.Value) currentLogData.ST1Interactions.Add(CLOBSInteraction.Unobservable);

            if (Stomper2Panel.Visibility == Visibility.Visible)
            {
                currentLogData.ST2Audiences.Clear();
                if (ST2AudienceWhole.IsChecked.Value) currentLogData.ST2Audiences.Add(CLOBSAudience.WholeClass);
                if (ST2AudienceIndividual.IsChecked.Value) currentLogData.ST2Audiences.Add(CLOBSAudience.SmallGroupIndividual);
                if (ST2AudienceNone.IsChecked.Value) currentLogData.ST2Audiences.Add(CLOBSAudience.None);

                currentLogData.ST2Interactions.Clear();
                if (ST2DisciplinarySTEM.IsChecked.Value) currentLogData.ST2Interactions.Add(CLOBSInteraction.STEMDisciplinary);
                if (ST2Activity.IsChecked.Value) currentLogData.ST2Interactions.Add(CLOBSInteraction.ProjectActivity);
                if (ST2HandsOn.IsChecked.Value) currentLogData.ST2Interactions.Add(CLOBSInteraction.HandsOnSupport);
                if (ST2ClassroomManagement.IsChecked.Value) currentLogData.ST2Interactions.Add(CLOBSInteraction.ClassroomManagement);
                if (ST2WalkAround.IsChecked.Value) currentLogData.ST2Interactions.Add(CLOBSInteraction.WalkAround);
                if (ST2UnObservable.IsChecked.Value) currentLogData.ST2Interactions.Add(CLOBSInteraction.Unobservable);
            }

            if (Stomper3Panel.Visibility == Visibility.Visible)
            {
                currentLogData.ST3Audiences.Clear();
                if (ST3AudienceWhole.IsChecked.Value) currentLogData.ST3Audiences.Add(CLOBSAudience.WholeClass);
                if (ST3AudienceIndividual.IsChecked.Value) currentLogData.ST3Audiences.Add(CLOBSAudience.SmallGroupIndividual);
                if (ST3AudienceNone.IsChecked.Value) currentLogData.ST3Audiences.Add(CLOBSAudience.None);

                currentLogData.ST3Interactions.Clear();
                if (ST3DisciplinarySTEM.IsChecked.Value) currentLogData.ST3Interactions.Add(CLOBSInteraction.STEMDisciplinary);
                if (ST3Activity.IsChecked.Value) currentLogData.ST3Interactions.Add(CLOBSInteraction.ProjectActivity);
                if (ST3HandsOn.IsChecked.Value) currentLogData.ST3Interactions.Add(CLOBSInteraction.HandsOnSupport);
                if (ST3ClassroomManagement.IsChecked.Value) currentLogData.ST3Interactions.Add(CLOBSInteraction.ClassroomManagement);
                if (ST3WalkAround.IsChecked.Value) currentLogData.ST3Interactions.Add(CLOBSInteraction.WalkAround);
                if (ST3UnObservable.IsChecked.Value) currentLogData.ST3Interactions.Add(CLOBSInteraction.Unobservable);
            }

            currentLogData.InterventionTradingCard = InterventionTradingCard.IsChecked.Value;
            currentLogData.InterventionBioVideo = InterventionBioVideos.IsChecked.Value;
            currentLogData.InterventionClassExemple = InterventionClassWork.IsChecked.Value;
            currentLogData.InterventionPersonal = InterventionPersonal.IsChecked.Value;
            currentLogData.TeacherStepsIn = TeacherStepskIn.IsChecked.Value;
            currentLogData.LogNote = ObservationNote.Text;

            ObservationManager.Instance.WriteLogsToFile();
            //ObservationManager.Instance.UpdateLastLogInFile(currentLogData);            
        }        

        private void EndUpdate_Click(object sender, RoutedEventArgs e)
        {
            FlushObservationItemData();            
            this.Visibility = Visibility.Collapsed;
            var parentStack = this.Frame.Parent as StackPanel;
            var parentGrid = parentStack.Parent as Grid;
            var parentPage = parentGrid.Parent as GapTimePage;
            parentPage.ShowButtonPanel();            
        }
    }
}
