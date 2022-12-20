namespace SmartGloveRebuild2.Models.Schedule
{
    public class UpdateScheduleStatusByGroupNameDTO
    {
        public string DayMonthYear
        {
            get => $"{ScheduleDate.ToUniversalTime}";
        }
        public DateTime ScheduleDate { get; set; }
        public string GroupName { get; set; }
        public double Hours { get; set; } = 0;
        public int Paxs { get; set; } = 0;
        public bool Status { get; set; }
    }
}