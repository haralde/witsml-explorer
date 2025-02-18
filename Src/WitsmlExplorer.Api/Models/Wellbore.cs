using System;
using WitsmlExplorer.Api.Models.Measure;

namespace WitsmlExplorer.Api.Models
{
    public class Wellbore
    {
        public string Uid { get; set; }
        public string Name { get; set; }
        public string WellUid { get; set; }
        public string WellName { get; set; }
        public string WellborePurpose { get; set; }
        public string WellboreParentUid { get; set; }
        public string WellboreParentName { get; set; }
        public string WellStatus { get; set; }
        public string WellType { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateTimeCreation { get; set; }
        public DateTime? DateTimeLastChange { get; set; }
        public string ItemState { get; set; }
        public string Number { get; set; }
        public string SuffixAPI { get; set; }
        public string NumGovt { get; set; }
        public string Shape { get; set; }
        public DateTime? DTimeKickoff { get; set; }
        public LengthMeasure Md { get; set; }
        public LengthMeasure Tvd { get; set; }
        public LengthMeasure MdKickoff { get; set; }
        public LengthMeasure TvdKickoff { get; set; }
        public LengthMeasure MdPlanned { get; set; }
        public LengthMeasure TvdPlanned { get; set; }
        public LengthMeasure MdSubSeaPlanned { get; set; }
        public LengthMeasure TvdSubSeaPlanned { get; set; }
        public DayMeasure DayTarget { get; set; }
    }
}
