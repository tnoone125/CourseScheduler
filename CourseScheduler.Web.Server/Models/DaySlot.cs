namespace CourseScheduler.Web.Server.Models
{
    public enum Day
    {
        MONDAY = 1,
        TUESDAY = 2,
        WEDNESDAY = 3,
        THURSDAY = 4,
        FRIDAY = 5,
        SATURDAY = 6,
        SUNDAY = 7,
    }
    public class DaySlot
    {
        public Day Day { get; set; }
        public List<(TimeOnly start, TimeOnly end)> TimeSlots { get; set; }
    }
}
