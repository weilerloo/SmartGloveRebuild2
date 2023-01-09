namespace SmartGloveRebuild2.Models.Schedule
{
    public class UpdateScheduleStatusByGroupNameDTO
    {
        public string DayMonthYear { get; set; }
        public string GroupName { get; set; }
        public double Hours { get; set; }
        public string Remarks { get; set; }
        public int Paxs { get; set; }
        public bool Status { get; set; }
    }
}