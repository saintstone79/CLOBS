using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLOBS2.Models
{
    public class ObservationInfoData
    {
        public ObservationMode ObjMode { get; set; }
        public DateTime ObservationDate { get; set; }
        public string ObserverName { get; set; }        
        public string SchoolName { get; set; }
        public string TeacherName { get; set; }
        public int StomperNumber { get; set; }
        public string Stomper1 { get; set; }
        public string Stomper2 { get; set; }
        public string Stomper3 { get; set; }
        public string ObservationNote { get; set; }
        public TimeSpan ObservationDuration { get; set; }
        public TimeSpan GapDuration { get; set; }
        public TimeSpan VideoElasped { get; set; }        
        public bool NoGapTime { get; set; }
    }
}
