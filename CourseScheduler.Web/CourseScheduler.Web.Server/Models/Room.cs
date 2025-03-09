namespace CourseScheduler.Web.Server.Models
{
    public class Room
    {
        public string UniqueSchoolId { get; set; }
        public int SettingId { get; set; }
        public string Name { get; set; }
        public List<string> PermittedDepartments { get; set; }
        public int StudentCapacity { get; set; }
    }
}
