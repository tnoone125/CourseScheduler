
namespace CourseScheduler
{
    class Instructor
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public int CourseMinimum { get; set; } = 1;
        public int CourseLimit { get; set; }
    }
}
