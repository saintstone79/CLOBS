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
        public StorageFolder m_storageFolder;
        public StorageFile m_storageNoteFile;
        public bool m_bStarted = false;
        public int m_nClassroomActivity = 0;
        public int m_nLogIndex = 0;

        public ObservationManager()
        {
            m_lsObLogData = new List<ObservationLogData>();
            m_objInfoData = new ObservationInfoData();
            m_storageFolder = ApplicationData.Current.LocalFolder;
        }

        public async void StartObservation(ObservationInfoData objInfo)
        {
            m_objInfoData = objInfo;
            m_dtInitTime = DateTime.Now;
            m_bStarted = true;
            //await SaveSessionInfo();
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

        /*
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
        */

        private string ExtractStringFromClassStructureList(List<CLOBSClassStructure> aStructureList)
        {
            string strExtractor = "";
            if (aStructureList == null)
                return "";
            foreach (var content in aStructureList)
            {
                if (string.IsNullOrEmpty(strExtractor))
                    strExtractor = content.ToString();
                else
                    strExtractor += ";" + content.ToString();
            }
            return strExtractor;
        }

        private string ExtractStringFromAudienceList(List<CLOBSAudience> anAudienceList)
        {
            string strExtractor = "";
            if (anAudienceList == null)
                return "";
            foreach (var content in anAudienceList)
            {
                if (string.IsNullOrEmpty(strExtractor))
                    strExtractor = content.ToString();
                else
                    strExtractor += ";" + content.ToString();
            }
            return strExtractor;
        }

        private string ExtractStringFromInteractionList(List<CLOBSInteraction> anInteractionList)
        {
            string strExtractor = "";
            if (anInteractionList == null)
                return "";
            foreach (var content in anInteractionList)
            {
                if (string.IsNullOrEmpty(strExtractor))
                    strExtractor = content.ToString();
                else
                    strExtractor += ";" + content.ToString();
            }
            return strExtractor;
        }

        public void AddSessionInfo(CsvFileWriter csvFile)
        {
            if (csvFile == null) return;

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
            WriteRowToFile(csvFile, "");
        }

        public void AddLogHeaderToFile(CsvFileWriter csvFile)
        {
            if (csvFile == null) return;

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

        public void AddLogsToFile(CsvFileWriter csvFile)
        {
            if (csvFile == null) return;

            foreach (ObservationLogData aLog in m_lsObLogData)
            {
                CsvRow aRow = new CsvRow();
                aRow.Add(aLog.LogIndex.ToString());
                aRow.Add(aLog.LogStarted.ToString(@"hh\:mm\:ss"));
                aRow.Add(ExtractStringFromClassStructureList(aLog.classStructures));
                aRow.Add(ExtractStringFromAudienceList(aLog.ST1Audiences));
                aRow.Add(ExtractStringFromInteractionList(aLog.ST1Interactions));

                //if (m_objInfoData.StomperNumber > 1)
                {
                    aRow.Add(ExtractStringFromAudienceList(aLog.ST2Audiences));
                    aRow.Add(ExtractStringFromInteractionList(aLog.ST2Interactions));
                }

                //if (m_objInfoData.StomperNumber > 2)
                {
                    aRow.Add(ExtractStringFromAudienceList(aLog.ST3Audiences));
                    aRow.Add(ExtractStringFromInteractionList(aLog.ST3Interactions));
                }

                aRow.Add(ConvertBooleanString(aLog.InterventionTradingCard));
                aRow.Add(ConvertBooleanString(aLog.InterventionBioVideo));
                aRow.Add(ConvertBooleanString(aLog.InterventionClassExemple));
                aRow.Add(ConvertBooleanString(aLog.InterventionPersonal));
                aRow.Add(ConvertBooleanString(aLog.TeacherStepsIn));
                aRow.Add(aLog.LogNote);
                csvFile.WriteRow(aRow);
            }
        }

        public async void WriteLogsToFile()
        {
            if (m_storageFile == null)
            {
                var dialog = new MessageDialog(string.Format("Failed to write log, file stream has not been correctly created "));
                await dialog.ShowAsync();
                return;
            }
            try
            {
                Stream fs = null;
                fs = await m_storageFile.OpenStreamForWriteAsync();
                using (CsvFileWriter csvFileWriter = new CsvFileWriter(fs))
                {                    
                    AddSessionInfo(csvFileWriter);                    
                    AddLogHeaderToFile(csvFileWriter);                    
                    AddLogsToFile(csvFileWriter);
                }
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("Failed to access log file, {0}", ex.Message));
                await dialog.ShowAsync();
            }
        }
        /*
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
                    aRow.Add(ExtractStringFromClassStructureList(newLog.classStructures));                    
                    aRow.Add(ExtractStringFromAudienceList(newLog.ST1Audiences));
                    aRow.Add(ExtractStringFromInteractionList(newLog.ST1Interactions));
                                        
                    if (m_objInfoData.StomperNumber > 1)
                    {
                        aRow.Add(ExtractStringFromAudienceList(newLog.ST2Audiences));
                        aRow.Add(ExtractStringFromInteractionList(newLog.ST2Interactions));                    
                    }                    
                    
                    if (m_objInfoData.StomperNumber > 2)
                    {
                        aRow.Add(ExtractStringFromAudienceList(newLog.ST3Audiences));
                        aRow.Add(ExtractStringFromInteractionList(newLog.ST3Interactions));                        
                    }                    

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
        public async void UpdateLastLogInFile(ObservationLogData newLog)
        {
            Stream fs = null;
            Stream fsRead = null;
            try
            {                
                fsRead = await m_storageFile.OpenStreamForReadAsync();
                fsRead.Seek(0, SeekOrigin.Begin);
                fsRead.Position = 0;
                long searchPos = 0;
                using (var streamReader = new StreamReader(fsRead))
                {
                    // find last log line
                    while (streamReader.Peek() >= 0)
                    {                                                
                        string aLine = streamReader.ReadLine();
                        if (aLine[0].ToString() == m_nLogIndex.ToString())
                            break;
                        searchPos += aLine.Length;
                    }
                    streamReader.Dispose();
                }
                fsRead.Dispose();
                
                fs = await m_storageFile.OpenStreamForWriteAsync();
                fs.Seek(searchPos+3, SeekOrigin.Begin); // move to the log line, +3 for \r\n and next pos                
                using (var csvFile = new CsvFileWriter(fs))
                {
                    CsvRow aRow = new CsvRow();
                    aRow.Add(newLog.LogIndex.ToString());
                    aRow.Add(newLog.LogStarted.ToString(@"hh\:mm\:ss"));
                    aRow.Add(ExtractStringFromClassStructureList(newLog.classStructures));
                    aRow.Add(ExtractStringFromAudienceList(newLog.ST1Audiences));
                    aRow.Add(ExtractStringFromInteractionList(newLog.ST1Interactions));

                    if (m_objInfoData.StomperNumber > 1)
                    {
                        aRow.Add(ExtractStringFromAudienceList(newLog.ST2Audiences));
                        aRow.Add(ExtractStringFromInteractionList(newLog.ST2Interactions));
                    }

                    if (m_objInfoData.StomperNumber > 2)
                    {
                        aRow.Add(ExtractStringFromAudienceList(newLog.ST3Audiences));
                        aRow.Add(ExtractStringFromInteractionList(newLog.ST3Interactions));
                    }

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
            } finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }
        */

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
                //m_storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(strFileName, CreationCollisionOption.GenerateUniqueName);
                m_storageFile = await m_storageFolder.CreateFileAsync(strFileName, CreationCollisionOption.GenerateUniqueName);

                /*
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
                */
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("Failed to access log file, {0}", ex.Message));
                await dialog.ShowAsync();
            }
        }

        /*
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
        */
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
            // Set filename as CLOBS + Observer name + School name + STOMPer 1 name
            string strNoteFilename = "CLOBS_" + m_objInfoData.ObserverName.Substring(0, 2).ToUpper() +
               m_objInfoData.SchoolName.Substring(0, 2).ToUpper() + m_objInfoData.Stomper1.Substring(0, 2).ToUpper() + DateTime.Now.ToString("MMddyyyy") + "_Note.csv";

            Stream fs = null;
            try
            {
                if (m_storageNoteFile == null)
                {
                    m_storageNoteFile = await m_storageFolder.CreateFileAsync(strNoteFilename, CreationCollisionOption.GenerateUniqueName);
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

        public ObservationLogData GetCurrentLogData()
        {
            // [WARNING] Gap log shares the log index with privious log. But it the gap log is recorded later than the observation log.
            // If extend this function, please consider above limitation.
            foreach(ObservationLogData aLog in m_lsObLogData)
            {
                if (aLog.LogIndex == m_nLogIndex)
                    return aLog;
            }            
            return null;
        }

        public int GetLogTotalLogCounts()
        {
            return m_lsObLogData.Count;
            /*
            if (m_lsObLogData.Count > 0)
                return m_lsObLogData[m_lsObLogData.Count - 1].LogIndex;
            else
                return 0;
                */
        }

        public int GetClassStructureCount(CLOBSClassStructure structure)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
            {
                if (aLog.classStructures != null)
                    foreach (CLOBSClassStructure aStructure in aLog.classStructures)
                        if (aStructure == structure)
                            nCount++;
            }
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
            {
                if (aLog.ST1Audiences != null)
                    foreach (CLOBSAudience aAudience in aLog.ST1Audiences)
                        if (aAudience == eventAudience)
                            nCount++;
            }
                
            return nCount;
        }

        public int GetEventST1InteractionCount(CLOBSInteraction eventInteraction)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
            {
                if (aLog.ST1Interactions != null)
                    foreach (CLOBSInteraction anInteraction in aLog.ST1Interactions)
                        if (anInteraction == eventInteraction)
                            nCount++;
            }
                
            return nCount;
        }

        public int GetEventST2AudienceCount(CLOBSAudience eventAudience)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
            {
                if (aLog.ST2Audiences != null)
                    foreach (CLOBSAudience aAudience in aLog.ST2Audiences)
                        if (aAudience == eventAudience)
                            nCount++;
            }
                
            return nCount;
        }
        public int GetEventST2InteractionCount(CLOBSInteraction eventInteraction)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
            {
                if (aLog.ST2Interactions != null)
                    foreach (CLOBSInteraction anInteraction in aLog.ST2Interactions)
                        if (anInteraction == eventInteraction)
                            nCount++;
            }
                
            return nCount;
        }

        public int GetEventST3AudienceCount(CLOBSAudience eventAudience)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
            {
                if (aLog.ST3Audiences != null)
                    foreach (CLOBSAudience aAudience in aLog.ST3Audiences)
                        if (aAudience == eventAudience)
                            nCount++;
            }
            return nCount;
        }
        public int GetEventST3InteractionCount(CLOBSInteraction eventInteraction)
        {
            int nCount = 0;
            foreach (ObservationLogData aLog in m_lsObLogData)
            {
                if (aLog.ST3Interactions != null)
                    foreach (CLOBSInteraction anInteraction in aLog.ST3Interactions)
                        if (anInteraction == eventInteraction)
                            nCount++;
            }
            return nCount;
        }
    }
}
