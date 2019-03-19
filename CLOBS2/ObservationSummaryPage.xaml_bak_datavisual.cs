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
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using CLOBS2.Models;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CLOBS2
{
    public class ChartRecord
    {
        public string EventName { get; set; }
        public int Count { get; set; }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ObservationSummaryPage : Page
    {
        private ObservationManager objManager;
        public ObservationSummaryPage()
        {
            this.InitializeComponent();
        }

        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            objManager.m_objInfoData.ObservationNote = ObservationNote.Text;
            await objManager.SaveSessionInfoWithStatistics();
            await Windows.System.Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
            Application.Current.Exit();
        }

        private void LoadClassroomChartContents()
        {
            List<ChartRecord> records = new List<ChartRecord>();
            ChartRecord ca1 = new ChartRecord()
            {
                EventName = ClassroomActivityString.ClassroomManagement,
                Count= 0
            };
            ChartRecord ca2 = new ChartRecord()
            {
                EventName = ClassroomActivityString.IntroDesignTask,
                Count = 0
            };
            ChartRecord ca3 = new ChartRecord()
            {
                EventName = ClassroomActivityString.EngineeringSTEMContent,
                Count = 0
            };
            ChartRecord ca4 = new ChartRecord()
            {
                EventName = ClassroomActivityString.HandsOnActivity,
                Count = 0
            };
            ChartRecord ca5 = new ChartRecord()
            {
                EventName = ClassroomActivityString.GroupDiscussion,
                Count = 0
            };
            ChartRecord ca6 = new ChartRecord()
            {
                EventName = ClassroomActivityString.Direction,
                Count = 0
            };
            
            foreach (ObservationLogData aLog in objManager.m_lsObLogData)
            {
                if (aLog.EventType == CLOBSEventType.ClassroomActivityChanged)
                {
                    if (aLog.ClassroomActivity.Equals(ClassroomActivityString.ClassroomManagement))
                        //records[]
                        ca1.Count++;
                    else if (aLog.ClassroomActivity.Equals(ClassroomActivityString.IntroDesignTask))
                        ca2.Count++;
                    else if (aLog.ClassroomActivity.Equals(ClassroomActivityString.EngineeringSTEMContent))
                        ca3.Count++;
                    else if (aLog.ClassroomActivity.Equals(ClassroomActivityString.HandsOnActivity))
                        ca4.Count++;
                    else if (aLog.ClassroomActivity.Equals(ClassroomActivityString.GroupDiscussion))
                        ca5.Count++;
                    else if (aLog.ClassroomActivity.Equals(ClassroomActivityString.Direction))
                        ca6.Count++;
                }
            }
            records.Add(ca1);
            records.Add(ca2);
            records.Add(ca3);
            records.Add(ca4);
            records.Add(ca5);
            records.Add(ca6);
            (PieChart.Series[0] as PieSeries).ItemsSource = records;
        }

        private void LoadSTOMPER1ChartContent()
        {
            List<ChartRecord> records = new List<ChartRecord>();
            ChartRecord ca1 = new ChartRecord()
            {
                EventName = CLOBSEventDetail.TalkingToClass.ToString(),
                Count = 0
            };
            ChartRecord ca2 = new ChartRecord()
            {
                EventName = CLOBSEventDetail.TalkingToIndividual.ToString(),
                Count = 0
            };
            ChartRecord ca3 = new ChartRecord()
            {
                EventName = CLOBSEventDetail.Observing.ToString(),
                Count = 0
            };
            ChartRecord ca4 = new ChartRecord()
            {
                EventName = CLOBSEventDetail.HandingOutMaterial.ToString(),
                Count = 0
            };
            ChartRecord ca5 = new ChartRecord()
            {
                EventName = CLOBSEventDetail.AsweringQuestion.ToString(),
                Count = 0
            };            

            foreach (ObservationLogData aLog in objManager.m_lsObLogData)
            {
                if (aLog.EventType == CLOBSEventType.STOMPER1)
                {
                    switch (aLog.EventDetail)
                    {
                        case CLOBSEventDetail.TalkingToClass:
                            ca1.Count++;
                            break;
                        case CLOBSEventDetail.TalkingToIndividual:
                            ca2.Count++;
                            break;
                        case CLOBSEventDetail.Observing:
                            ca3.Count++;
                            break;
                        case CLOBSEventDetail.HandingOutMaterial:
                            ca4.Count++;
                            break;
                        case CLOBSEventDetail.AsweringQuestion:
                            ca5.Count++;
                            break;
                    }                                                
                }
            }
            records.Add(ca1);
            records.Add(ca2);
            records.Add(ca3);
            records.Add(ca4);
            records.Add(ca5);            
            (ST1BarChart.Series[0] as BarSeries).ItemsSource = records;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ObservationManager && e.Parameter != null)
            {
                objManager = (ObservationManager)e.Parameter;
                ObservationStarted.Text = objManager.m_dtInitTime.ToString("HH:mm:ss");
                ObservationEnded.Text = objManager.m_dtEndTime.ToString("HH:mm:ss");
                ST1Name.Text = objManager.m_objInfoData.Stomper1;
                ST2Name.Text = objManager.m_objInfoData.Stomper2;
                ObservationNote.Text = objManager.m_objInfoData.ObservationNote;
                ObservationLogLocation.Text = objManager.m_storageFile.Path;
                ObservationNoteLocation.Text = objManager.m_storageNoteFile.Path;
                                
                TotalLogCount.Text = objManager.GetLogTotalLogCounts().ToString();
                TotalEventCount.Text = objManager.GetTotalEventCounts().ToString();

                LoadClassroomChartContents();
                /*
                ClassroomManagementCt.Text = objManager.GetClassroomActivityCount(ClassroomActivityString.ClassroomManagement).ToString();
                IntroDesignTaskCt.Text = objManager.GetClassroomActivityCount(ClassroomActivityString.IntroDesignTask).ToString();
                EngineeringSTEMContentCt.Text = objManager.GetClassroomActivityCount(ClassroomActivityString.EngineeringSTEMContent).ToString();
                HandsOnActivityCt.Text = objManager.GetClassroomActivityCount(ClassroomActivityString.HandsOnActivity).ToString();
                GroupDisscussionCt.Text = objManager.GetClassroomActivityCount(ClassroomActivityString.GroupDiscussion).ToString();
                DirectionCt.Text = objManager.GetClassroomActivityCount(ClassroomActivityString.Direction).ToString();*/

                ST1EventCt.Text = objManager.GetEventCount(CLOBSEventType.STOMPER1).ToString();
                //LoadSTOMPER1ChartContent();                
                ST1TalkToClassCt.Text = objManager.GetEventDetailCount(CLOBSEventType.STOMPER1, CLOBSEventDetail.TalkingToClass).ToString();
                ST1TalkIndividualCt.Text = objManager.GetEventDetailCount(CLOBSEventType.STOMPER1, CLOBSEventDetail.TalkingToIndividual).ToString();
                ST1ObservingCt.Text = objManager.GetEventDetailCount(CLOBSEventType.STOMPER1, CLOBSEventDetail.Observing).ToString();
                ST1HandingOutCt.Text = objManager.GetEventDetailCount(CLOBSEventType.STOMPER1, CLOBSEventDetail.HandingOutMaterial).ToString();
                ST1AnsweringQuestionCt.Text = objManager.GetEventDetailCount(CLOBSEventType.STOMPER1, CLOBSEventDetail.AsweringQuestion).ToString();                

                ST2EventCt.Text = objManager.GetEventCount(CLOBSEventType.STOMPER2).ToString();
                ST2TalkToClassCt.Text = objManager.GetEventDetailCount(CLOBSEventType.STOMPER2, CLOBSEventDetail.TalkingToClass).ToString();
                ST2TalkIndividualCt.Text = objManager.GetEventDetailCount(CLOBSEventType.STOMPER2, CLOBSEventDetail.TalkingToIndividual).ToString();
                ST2ObservingCt.Text = objManager.GetEventDetailCount(CLOBSEventType.STOMPER2, CLOBSEventDetail.Observing).ToString();
                ST2HandingOutCt.Text = objManager.GetEventDetailCount(CLOBSEventType.STOMPER2, CLOBSEventDetail.HandingOutMaterial).ToString();
                ST2AnsweringQuestionCt.Text = objManager.GetEventDetailCount(CLOBSEventType.STOMPER2, CLOBSEventDetail.AsweringQuestion).ToString();

                InterventionCt.Text = objManager.GetEventCount(CLOBSEventType.Intervention).ToString();
                InterventionTradingCardCt.Text = objManager.GetEventDetailCount(CLOBSEventType.Intervention, CLOBSEventDetail.TradingCard).ToString();
                InterventionVideosBiosCt.Text = objManager.GetEventDetailCount(CLOBSEventType.Intervention, CLOBSEventDetail.ShareBioVideo).ToString();
                InterventionEngineeringRelatedArticleCt.Text = objManager.GetEventDetailCount(CLOBSEventType.Intervention, CLOBSEventDetail.EngineeringRelatedArticles).ToString();
                InterventionClassWorkExampleCt.Text = objManager.GetEventDetailCount(CLOBSEventType.Intervention, CLOBSEventDetail.ClassWorkExamples).ToString();

                InteractionCt.Text = objManager.GetEventCount(CLOBSEventType.Interaction).ToString();
                InteractionEngConnectionCt.Text = objManager.GetEventDetailCount(CLOBSEventType.Interaction, CLOBSEventDetail.EngineeringConnection).ToString();
                InteractionRealConnectionCt.Text = objManager.GetEventDetailCount(CLOBSEventType.Interaction, CLOBSEventDetail.RealWorldConnection).ToString();
                InteractionEngExperienceCt.Text = objManager.GetEventDetailCount(CLOBSEventType.Interaction, CLOBSEventDetail.PersonalEngineeringExperience).ToString();
                InteractionEngPassionCt.Text = objManager.GetEventDetailCount(CLOBSEventType.Interaction, CLOBSEventDetail.EngineeringPassion).ToString();
                InteractionPersonalConnectionCt.Text = objManager.GetEventDetailCount(CLOBSEventType.Interaction, CLOBSEventDetail.PersonalConnection).ToString();
            }

            base.OnNavigatedTo(e);
        }

        private void OpenFileFolder_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            Windows.System.Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }
    }
}
