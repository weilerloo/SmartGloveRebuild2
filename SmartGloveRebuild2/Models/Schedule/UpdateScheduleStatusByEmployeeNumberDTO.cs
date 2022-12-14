namespace SmartGloveRebuild2.Models.Schedule
{
    public class UpdateScheduleStatusByEmployeeNumberDTO
    {
        public string ScheduleID
        {
            get => $"{ScheduleDate.ToUniversalTime}";
        }
        public string EmployeeNumber { get; set; }
        public DateTime ScheduleDate { get; set; }
        public int Paxs { get; set; }
        public double Hours { get; set; }
        public bool Status { get; set; }
    }
}