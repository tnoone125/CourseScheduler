namespace CourseScheduler.Web.Server.Models
{
    public class Course
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Enrollment { get; set; }
        public int NumberOfSections { get; set; }
        public List<string> Departments { get; set; }
    }
}
