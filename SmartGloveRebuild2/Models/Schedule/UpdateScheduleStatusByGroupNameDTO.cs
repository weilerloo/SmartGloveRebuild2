namespace SmartGloveRebuild2.Models.Schedule
{
    public class UpdateScheduleStatusByGroupNameDTO
    {
        public string DayMonthYear
        {
            get => $"{ScheduleDate.ToUniversalTime}";
        }
        public string EmployeeNumber { get; set; }
        public string GroupName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public bool Status { get; set; }
    }
}