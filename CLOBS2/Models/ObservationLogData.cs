using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLOBS2
{
    public enum CLOBSClassStructure
    {
        WholeClass,
        SmallGroupOrHandsOn
    }
    
    public enum CLOBSInteraction
    {
        STEMDisciplinary,
        ProjectActivity,
        HandsOnSupport,
        ClassroomManagement,
        WalkAround,
        Unobservable
    }
            
    public enum CLOBSAudience
    {
        WholeClass,
        SmallGroupIndividual,
        None
    }

    public class ObservationLogData
    {        
        public int LogIndex { get; set; }
        public TimeSpan LogStarted { get; set; }        
        public TimeSpan EventTimeStarted { get; set; }
        public TimeSpan EventTimeEnded { get; set; }
        public List<CLOBSClassStructure> classStructures { get; set; }
        public List<CLOBSAudience> ST1Audiences { get; set; }
        public List<CLOBSInteraction> ST1Interactions { get; set; }
        public List<CLOBSAudience> ST2Audiences { get; set; }
        public List<CLOBSInteraction> ST2Interactions { get; set; }
        public List<CLOBSAudience> ST3Audiences { get; set; }
        public List<CLOBSInteraction> ST3Interactions { get; set; }
        public bool InterventionTradingCard { get; set; }
        public bool InterventionBioVideo { get; set; }
        public bool InterventionClassExemple { get; set; }
        public bool InterventionPersonal { get; set; }
        public bool TeacherStepsIn { get; set; }        
        public string LogNote { get; set; }

        public ObservationLogData(int nIndex, TimeSpan tsStart)
        {
            LogIndex = nIndex;
            LogStarted = tsStart;            
        }
    }
}