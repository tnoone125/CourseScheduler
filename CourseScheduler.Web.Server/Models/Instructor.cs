namespace CourseScheduler.Web.Server.Models
{
    public class Instructor
    {
        public string UniqueSchoolId { get; set; }
        public int SettingId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public int? CourseMin { get; set; }
        public int? CourseMax { get; set; }
    }
}
