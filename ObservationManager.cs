using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLOBS2.Models;
using System.IO;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.Storage;

namespace CLOBS2
{
    public enum ObservationMode
    {
        Sampled,
        Continuous
    }
    public class ObservationManager
    {
        // Singleton pattern
        private static ObservationManager _instance = null;
        public static ObservationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ObservationManager();
                }
                return _instance;
            }
        }

        public List<ObservationLogData> m_lsObLogData;
        public ObservationInfoData m_objInfoData;

        public string m_strDate;
        public DateTime m_dtInitTime;
        public DateTime m_dtEndTime;
        public ObservationLogData m_dataActiveLog;

        public StorageFile m_storageFile;
        public StorageFile m_storageNoteFile;
        public bool m_bStarted = false;
        public int m_nClassroomActivity = 0;
        public int m_nLogIndex = 0;

        public ObservationManager()
        {
            m_lsObLogData = new List<ObservationLogData>();
            m_objInfoData = new ObservationInfoData();
        }

        public async void StartObservation(ObservationInfoData objInfo)
        {
            m_objInfoData = objInfo;
            m_dtInitTime = DateTime.Now;
            m_bStarted = true;
            await SaveSessionInfo();
            await CreateLogStorageFile();
        }
        public void EndObservation()
        {
            m_dtEndTime = DateTime.Now;
        }

        public void UpdateObservationInfo(ObservationInfoData objInfoData)
        {
            m_objInfoData = objInfoData;
        }

        public async void AppendNewNoteToFile(TimeSpan logStarted, string strNote)
        {
            Stream fs = null;
            try
            {
                fs = await m_storageFile.OpenStreamForWriteAsync();
                fs.Seek(0, SeekOrigin.End);
                using (var csvFile = new CsvFileWriter(fs))
                {
                    CsvRow aRow = new CsvRow();
                    aRow.Add("Gap time note");                    
                    aRow.Add(logStarted.ToString(@"hh\:mm\:ss"));
                    for (int i = 0; i < 12; i++)
                        aRow.Add("");
                    aRow.Add(strNote);                    
                    csvFile.WriteRow(aRow);
                }
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("Failed to create log file {0}", ex.Message));
                await dialog.ShowAsync();
            }
        }

        public async void AppendNewLogToFile(ObservationLogData newLog)
        {
            Stream fs = null;
            try
            {
                fs = await m_storageFile.OpenStreamForWriteAsync();
                fs.Seek(0, SeekOrigin.End);
                using (var csvFile = new CsvFileWriter(fs))
                {
                    CsvRow aRow = new CsvRow();
                    aRow.Add(newLog.LogIndex.ToString());
                    aRow.Add(newLog.LogStarted.ToString(@"hh\:mm\:ss"));
                    string strClassStructure = "";

                    foreach (CLOBSClassStructure aStructure in newLog.classStructures)
                        if (string.IsNullOrEmpty(strClassStructure))
                            strClassStructure = aStructure.ToString();
                        else
                            strClassStructure += "; " + aStructure.ToString();
                    aRow.Add(strClassStructure);

                    string strST1Audience = "";
                    foreach (CLOBSAudience anAudience in newLog.ST1Audiences)
                        if (string.IsNullOrEmpty(strST1Audience))
                            strST1Audience = anAudience.ToString();
                        else
                            strST1Audience += "; " + anAudience.ToString();
                    aRow.Add(strST1Audience);

                    string strST1Interaction = "";
                    foreach (CLOBSInteraction anInteraction in newLog.ST1Interactions)
                        if (string.IsNullOrEmpty(strST1Interaction))
                            strST1Interaction = anInteraction.ToString();
                        else
                            strST1Interaction += "; " + anInteraction.ToString();
                    aRow.Add(strST1Interaction);

                    string strST2Audience = "";
                    string strST2Interaction = "";
                    if (m_objInfoData.StomperNumber > 1)
                    {
                        foreach (CLOBSAudience anAudience in newLog.ST2Audiences)
                            if (string.IsNullOrEmpty(strST2Audience))
                                strST2Audience = anAudience.ToString();
                            else
                                strST2Audience += "; " + anAudience.ToString();

                        foreach (CLOBSInteraction anInteraction in newLog.ST2Interactions)
                            if (string.IsNullOrEmpty(strST2Interaction))
                                strST2Interaction = anInteraction.ToString();
                            else
                                strST2Interaction += "; " + anInteraction.ToString();
                    }
                    aRow.Add(strST2Audience);
                    aRow.Add(strST2Interaction);

                    string strST3Audience = "";
                    string strST3Interaction = "";
                    if (m_objInfoData.StomperNumber > 2)
                    {
                        foreach (CLOBSAudience anAudience in newLog.ST3Audiences)
                            if (string.IsNullOrEmpty(strST3Audience))
                                strST3Audience = anAudience.ToString();
                            else
                                strST3Audience += "; " + anAudience.ToString();

                        foreach (CLOBSInteraction anInteraction in newLog.ST3Interactions)
                            if (string.IsNullOrEmpty(strST3Interaction))
                                strST3Interaction = anInteraction.ToString();
                            else
                                strST3Interaction += "; " + anInteraction.ToString();
                    }
                    aRow.Add(strST3Audience);
                    aRow.Add(strST3Interaction);

                    aRow.Add(ConvertBooleanString(newLog.InterventionTradingCard));
                    aRow.Add(ConvertBooleanString(newLog.InterventionBioVideo));
                    aRow.Add(ConvertBooleanString(newLog.InterventionClassExemple));
                    aRow.Add(ConvertBooleanString(newLog.InterventionPersonal));
                    aRow.Add(ConvertBooleanString(newLog.TeacherStepsIn));
                    aRow.Add(newLog.LogNote);

                    csvFile.WriteRow(aRow);
                }

            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("Failed to create log file {0}", ex.Message));
                await dialog.ShowAsync();
            }
        }


        public string ConvertBooleanString(bool bTrue)
        {
            if (bTrue == true)
                return "X";
            else
                return "";
        }

        private async Task CreateLogStorageFile()
        {
            string strFileName = "CLOBS_" + m_objInfoData.ObserverName.Substring(0, 2).ToUpper() +
              m_objInfoData.SchoolName.Substring(0, 2).ToUpper() + m_objInfoData.Stomper1.Substring(0, 2).ToUpper() + DateTime.Now.ToString("MMddyyyy") + ".csv";
            try
            {
                //m_storageFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.CreateFileAsync(strFileName, CreationCollisionOption.GenerateUniqueName);
                m_storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(strFileName, CreationCollisionOption.GenerateUniqueName);

                Stream fs = null;
                fs = await m_storageFile.OpenStreamForWriteAsync();
                using (var csvFile = new CsvFileWriter(fs))
                {
                    WriteRowToFile(csvFile, "OBSERVATION LOGS:");
                    string strST2AudienceHeader = "";
                    string strST2InteractionHeader = "";
                    if (m_objInfoData.StomperNumber > 1)
                    {
                        strST2AudienceHeader = m_objInfoData.Stomper2 + " Audiences";
                        strST2InteractionHeader = m_objInfoData.Stomper2 + " Interactions";
                    }
                    else
                    {
                        strST2AudienceHeader = "No STOMPer2 Audiences";
                        strST2InteractionHeader = "No STOMPer2 Interactions";
                    }
                    string strST3AudienceHeader = "";
                    string strST3InteractionHeader = "";
                    if (m_objInfoData.StomperNumber > 2)
                    {
                        strST3AudienceHeader = m_objInfoData.Stomper3 + " Audiences";
                        strST3InteractionHeader = m_objInfoData.Stomper3 + " Interactions";
                    }
                    else
                    {
                        strST3AudienceHeader = "No STOMPer3 Audiences";
                        strST3InteractionHeader = "No STOMPer3 Interactions";
                    }

                    WriteRowToFile(csvFile, "Log Index", "Log Started", "Class Structure", m_objInfoData.Stomper1 + " Audiences", m_objInfoData.Stomper1 + " Interactions",
                        strST2AudienceHeader, strST2InteractionHeader, strST3AudienceHeader, strST3InteractionHeader,
                        "InterventionTradingCard", "InterventionBioVideo", "InterventionClassExemple", "InterventionPersonal",
                        "TeacherStepsIn", "LogNote");
                }
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("Failed to access log file, {0}", ex.Message));
                await dialog.ShowAsync();
            }
        }

        public async Task SaveSessionInfo()
        {
            string strNoteFilename = "CLOBS_" + m_objInfoData.ObserverName.Substring(0, 2).ToUpper() +
               m_objInfoData.SchoolName.Substring(0, 2).ToUpper() + m_objInfoData.Stomper1.Substring(0, 2).ToUpper() + DateTime.Now.ToString("MMddyyyy") + "_Note.csv";

            Stream fs = null;
            try
            {
                if (m_storageNoteFile == null)
                {
                    //m_storageNoteFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.CreateFileAsync(strNoteFilename, CreationCollisionOption.GenerateUniqueName);
                    m_storageNoteFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(strNoteFilename, CreationCollisionOption.GenerateUniqueName);
                }


                fs = await m_storageNoteFile.OpenStreamForWriteAsync();
                using (var csvFile = new CsvFileWriter(fs))
                {
                    WriteRowToFile(csvFile, "Observation Summary");
                    WriteRowToFile(csvFile, "Date", m_objInfoData.ObservationDate.ToString("MM/dd/yyyy"));
                    WriteRowToFile(csvFile, "Session Started", m_dtInitTime.ToString("HH:mm:ss"));
                    WriteRowToFile(csvFile, "Session Ended", m_dtEndTime.ToString("HH:mm:ss"));
                    WriteRowToFile(csvFile, "Observer", m_objInfoData.ObserverName);
                    WriteRowToFile(csvFile, "School", m_objInfoData.SchoolName);
                    WriteRowToFile(csvFile, "Teacher", m_objInfoData.TeacherName);
                    WriteRowToFile(csvFile, "STOMPER1", m_objInfoData.Stomper1);
                    WriteRowToFile(csvFile, "STOMPER2", m_objInfoData.Stomper2);
                    WriteRowToFile(csvFile, "STOMPER3", m_objInfoData.Stomper3);
                    WriteRowToFile(csvFile, "Note", m_objInfoData.ObservationNote);
                }
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("Failed to save note file {0}", ex.Message));
                await dialog.ShowAsync();
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }
        private void WriteRowToFile(CsvFileWriter csvFile, params string[] rowItems)
        {
            if (csvFile == null)
                return;
            if (rowItems == null)
                return;
            CsvRow aRow = new CsvRow();
            foreach (string item in rowItems)
            {
                aRow.Add(item);
            }
            csvFile.WriteRow(aRow);
        }

        public async Task SaveSessionInfoWithStatistics()
        {
            string strNoteFilename = "CLOBS_" + m_objInfoData.ObserverName.Substring(0, 2).ToUpper() +
               m_objInfoData.SchoolName.Substring(0, 2).ToUpper() + m_objInfoData.Stomper1.Substring(0, 2).ToUpper() + DateTime.Now.ToString("MMddyyyy") + "_Note.csv";

            Stream fs = null;
            try
            {
                if (m_storageNoteFile == null)
                {
                    //m_storageNoteFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.CreateFileAsync(strNoteFilename, CreationCollisionOption.GenerateUniqueName);
                    m_storageNoteFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(strNoteFilename, CreationCollisionOption.GenerateUniqueName);
                }

                fs = await m_storageNoteFile.OpenStreamForWriteAsync();
                using (var csvFile = new CsvFileWriter(fs))
                {
                    WriteRowToFile(csvFile, "Observation Summary");
                    WriteRowToFile(csvFile, "Date", m_objInfoData.ObservationDate.ToString("MM/dd/yyyy"));
                    WriteRowToFile(csvFile, "Session Started", m_dtInitTime.ToString("HH:mm:ss"));
                    WriteRowToFile(csvFile, "Session Ended", m_dtEndTime.ToString("HH:mm:ss"));
                    WriteRowToFile(csvFile, "Observer", m_objInfoData.ObserverName);
                    WriteRowToFile(csvFile, "School", m_objInfoData.SchoolName);
                    WriteRowToFile(csvFile, "Teacher", m_objInfoData.TeacherName);
                    WriteRowToFile(csvFile, "STOMPER1", m_objInfoData.Stomper1);
                    WriteRowToFile(csvFile, "STOMPER2", m_objInfoData.Stomper2);
                    WriteRowToFile(csvFile, "STOMPER3", m_objInfoData.Stomper3);
                    WriteRowToFile(csvFile, "Note", m_objInfoData.ObservationNote);

                    // statistics
                    WriteRowToFile(csvFile, "");

                    /*
                    // classroom activity counts
                    if (m_objInfoData.ObjMode == ObservationMode.Continuous)
                    {
                        WriteRowToFile(csvFile, "Classroom phase");
                        WriteRowToFile(csvFile, ClassroomPhaseString.ClassroomManagement, GetClassroomActivityCount(ClassroomPhaseString.ClassroomManagement).ToString());
                        WriteRowToFile(csvFile, ClassroomPhaseString.IntroDesignTask, GetClassroomActivityCount(ClassroomPhaseString.IntroDesignTask).ToString());
                        WriteRowToFile(csvFile, ClassroomPhaseString.EngineeringSTEMContent, GetClassroomActivityCount(ClassroomPhaseString.EngineeringSTEMContent).ToString());
                        WriteRowToFile(csvFile, ClassroomPhaseString.EngineeringDesign, GetClassroomActivityCount(ClassroomPhaseString.EngineeringDesign).ToString());
                        WriteRowToFile(csvFile, ClassroomPhaseString.Wrapup, GetClassroomActivityCount(ClassroomPhaseString.Wrapup).ToString());
                        WriteRowToFile(csvFile, "");
                    } */

                    /*
                    // STOMPER1 counts
                    WriteRowToFile(csvFile, "STOMPER1", GetEventCount(CLOBSEventType.STOMPER1).ToString());
                    WriteRowToFile(csvFile, "Talk to whole class", GetEventDetailCount(CLOBSEventType.STOMPER1, CLOBSEventDetail.).ToString());
                    WriteRowToFile(csvFile, "Talk to small group", GetEventDetailCount(CLOBSEventType.STOMPER1, CLOBSEventDetail.TalkingToIndividual).ToString());
                    WriteRowToFile(csvFile, "Demo to class", GetEventDetailCount(CLOBSEventType.STOMPER1, CLOBSEventDetail.ConductingDemosToClass).ToString());
                    WriteRowToFile(csvFile, "Demo to individual", GetEventDetailCount(CLOBSEventType.STOMPER1, CLOBSEventDetail.ConductingDemosToIndividual).ToString());
                    WriteRowToFile(csvFile, "Observing", GetEventDetailCount(CLOBSEventType.STOMPER1, CLOBSEventDetail.Observing).ToString());
                    WriteRowToFile(csvFile, "Handing out materials", GetEventDetailCount(CLOBSEventType.STOMPER1, CLOBSEventDetail.HandingOutMaterial).ToString());                    
                    WriteRowToFile(csvFile, "");
                    // STOMPER2 counts
                    WriteRowToFile(csvFile, "STOMPER2", GetEventCount(CLOBSEventType.STOMPER2).ToString());
                    WriteRowToFile(csvFile, "Talk to class", GetEventDetailCount(CLOBSEventType.STOMPER2, CLOBSEventDetail.TalkingToClass).ToString());
                    WriteRowToFile(csvFile, "Talk to individual", GetEventDetailCount(CLOBSEventType.STOMPER2, CLOBSEventDetail.TalkingToIndividual).ToString());
                    WriteRowToFile(csvFile, "Demo to class", GetEventDetailCount(CLOBSEventType.STOMPER2, CLOBSEventDetail.ConductingDemosToClass).ToString());
                    WriteRowToFile(csvFile, "Demo to individual", GetEventDetailCount(CLOBSEventType.STOMPER2, CLOBSEventDetail.ConductingDemosToIndividual).ToString());
                    WriteRowToFile(csvFile, "Observing", GetEventDetailCount(CLOBSEventType.STOMPER2, CLOBSEventDetail.Observing).ToString());
                    WriteRowToFile(csvFile, "Handing out materials", GetEventDetailCount(CLOBSEventType.STOMPER2, CLOBSEventDetail.HandingOutMaterial).ToString());                    
                    WriteRowToFile(csvFile, "");

                    // Intervention counts
                    WriteRowToFile(csvFile, "Interventions", GetEventCount(CLOBSEventType.Intervention).ToString());
                    WriteRowToFile(csvFile, "Trading card", GetEventDetailCount(CLOBSEventType.Intervention, CLOBSEventDetail.TradingCard).ToString());
                    WriteRowToFile(csvFile, "Videos (Bios about STOMPers)", GetEventDetailCount(CLOBSEventType.Intervention, CLOBSEventDetail.ShareBioVideo).ToString());
                    WriteRowToFile(csvFile, "Engineering-related article/video", GetEventDetailCount(CLOBSEventType.Intervention, CLOBSEventDetail.EngineeringRelatedArticles).ToString());
                    WriteRowToFile(csvFile, "Class work example", GetEventDetailCount(CLOBSEventType.Intervention, CLOBSEventDetail.ClassWorkExamples).ToString());
                    WriteRowToFile(csvFile, "");

                    // Interaction counts
                    WriteRowToFile(csvFile, "Interaction", GetEventCount(CLOBSEventType.Interaction).ToString());
                    WriteRowToFile(csvFile, "Engineering connection", GetEventDetailCount(CLOBSEventType.Interaction, CLOBSEventDetail.EngineeringConnection).ToString());
                    WriteRowToFile(csvFile, "Real-world connection", GetEventDetailCount(CLOBSEventType.Interaction, CLOBSEventDetail.RealWorldConnection).ToString());
                    WriteRowToFile(csvFile, "Personal engineering experience", GetEventDetailCount(CLOBSEventType.Interaction, CLOBSEventDetail.PersonalEngineeringExperience).ToString());
                    WriteRowToFile(csvFile, "Engineering passions", GetEventDetailCount(CLOBSEventType.Interaction, CLOBSEventDetail.EngineeringPassion).ToString());
                    WriteRowToFile(csvFile, "Personal connection", GetEventDetailCount(CLOBSEventType.Interaction, CLOBSEventDetail.PersonalConnection).ToString());
                    */
                }
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("Failed to save note file {0}", ex.Message));
                await dialog.ShowAsync();
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }
        /*
        public int GetClassroomActivityCount(string strClassroomActivity)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
            {
                if (aLog.EventType == CLOBSEventType.ClassroomActivityChanged && aLog.ClassroomActivity.Equals(strClassroomActivity) == true)
                    nCount++;
            }
            return nCount;
        }
        */

        public int GetLogTotalLogCounts()
        {
            if (m_lsObLogData.Count > 0)
                return m_lsObLogData[m_lsObLogData.Count - 1].LogIndex;
            else
                return 0;
        }

        public int GetClassStructureCount(CLOBSClassStructure structure)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
                foreach (CLOBSClassStructure aStructure in aLog.classStructures)
                    if (aStructure == structure)
                        nCount++;
            return nCount;
        }

        public int GetInterventionTradingCount()
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
                if (aLog.InterventionTradingCard)
                    nCount++;
            return nCount;
        }
        public int GetInterventionBioVideoCount()
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
                if (aLog.InterventionBioVideo)
                    nCount++;
            return nCount;
        }
        public int GetInterventionClassroomExcmpleCount()
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
                if (aLog.InterventionClassExemple)
                    nCount++;
            return nCount;
        }
        public int GetInterventionPersonal()
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
                if (aLog.InterventionPersonal)
                    nCount++;
            return nCount;
        }

        public int GetEventST1AudienceCount(CLOBSAudience eventAudience)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
                foreach (CLOBSAudience aAudience in aLog.ST1Audiences)
                    if (aAudience == eventAudience)
                        nCount++;
            return nCount;
        }

        public int GetEventST1InteractionCount(CLOBSInteraction eventInteraction)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
                foreach (CLOBSInteraction anInteraction in aLog.ST1Interactions)
                    if (anInteraction == eventInteraction)
                        nCount++;
            return nCount;
        }

        public int GetEventST2AudienceCount(CLOBSAudience eventAudience)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
                foreach (CLOBSAudience aAudience in aLog.ST2Audiences)
                    if (aAudience == eventAudience)
                        nCount++;
            return nCount;
        }
        public int GetEventST2InteractionCount(CLOBSInteraction eventInteraction)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
                foreach (CLOBSInteraction anInteraction in aLog.ST2Interactions)
                    if (anInteraction == eventInteraction)
                        nCount++;
            return nCount;
        }

        public int GetEventST3AudienceCount(CLOBSAudience eventAudience)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
                foreach (CLOBSAudience aAudience in aLog.ST3Audiences)
                    if (aAudience == eventAudience)
                        nCount++;
            return nCount;
        }
        public int GetEventST3InteractionCount(CLOBSInteraction eventInteraction)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
                foreach (CLOBSInteraction anInteraction in aLog.ST3Interactions)
                    if (anInteraction == eventInteraction)
                        nCount++;
            return nCount;
        }
    }
}
