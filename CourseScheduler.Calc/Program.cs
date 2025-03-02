using CourseScheduler;

List<Course> courses = new List<Course>()
{
    new Course
    {
        Department = "Mathematics",
        Name = "Algebra 1",
        EnrollmentCount = 20,
        Uid = "MAT100",
    },
    new Course
    {
        Department = "Mathematics",
        Name = "Algebra 2 / Trig",
        EnrollmentCount = 25,
        Uid = "MAT300",
    },
    new Course
    { 
        Department = "English",
        Name = "AP Language and Composition",
        EnrollmentCount = 29,
        Uid = "ENG320AP",
    },
    new Course
    {
        Department = "Science",
        Name = "Biology",
        EnrollmentCount = 30,
        Uid = "BIO100",
    },
    new Course
    { 
        Department = "Science",
        Name = "Chemistry",
        EnrollmentCount = 22,
        Uid = "SCI200"
    },
    new Course
    {
        Department = "Science",
        Name = "Chemistry H",
        EnrollmentCount = 18,
        Uid = "SCI210H"
    },
    new Course
    { 
        Department = "English",
        Name = "English 3",
        EnrollmentCount = 28,
        Uid = "ENG300",
    },
    new Course
    { 
        Department = "Mathematics",
        Name = "Calculus Honors",
        EnrollmentCount = 24,
        Uid = "MAT410H"
    },
    new Course
    {
        Department = "Science",
        Name = "Physics",
        EnrollmentCount = 23,
        Uid = "SCI300",
    }
};

List<Room> rooms = new List<Room>()
{
    new Room
    { 
        DisplayName = "234",
        Capacity = 27
    },
    new Room
    { 
        DisplayName = "242",
        Capacity = 18,
    },
    new Room
    {
        DisplayName = "353",
        Capacity = 30
    },
    new Room
    { 
        DisplayName = "Smart Classroom",
        Capacity = 22,
    },
    new Room
    {
        DisplayName = "217",
        Capacity = 30,
    },
    new Room
    { 
        DisplayName = "216",
        Capacity = 22,
    },
    new Room
    { 
        DisplayName = "341",
        Capacity = 35,
    },
    new Room
    { 
        DisplayName = "Art Room",
        Capacity = 27,
    }
};

List<Instructor> instructors = new List<Instructor>()
{
    new Instructor
    {
        Name = "Mr. Noone",
        CourseLimit = 4,
        Department = "Mathematics",
    },
    new Instructor
    { 
        Name = "Ms. Zefran",
        CourseLimit = 4,
        Department = "Mathematics",
    },
    new Instructor
    {
        Name = "Mr. Reidy",
        CourseLimit = 5,
        Department = "English",
    },
    new Instructor
    { 
        Name = "Mr. Distefano",
        CourseLimit = 4,
        Department = "Science",
    },
    new Instructor
    { 
        Name = "Mr. Sullivan",
        CourseLimit = 4,
        Department = "Science"
    },
    new Instructor
    { 
        Name = "Ms. Lala",
        CourseLimit = 5,
        Department = "English",
    }
};

List<Timeslot> timeslots = new List<Timeslot>()
{
    new Timeslot
    { 
        DisplayName = "Monday 9:00-11:00am",
        RelativeOrdering = 1,
    },
    new Timeslot
    { 
        DisplayName = "Tuesday 9:00-11:00am",
        RelativeOrdering = 2,
    },
    new Timeslot
    { 
        DisplayName = "Tuesday 12:00pm-2:00pm",
        RelativeOrdering = 3,
    },
    new Timeslot
    {
        DisplayName = "Wednesday 12:00pm-2:00pm",
        RelativeOrdering = 5,
    },
    new Timeslot
    { 
        DisplayName = "Thursday 9:00am-11:00am",
        RelativeOrdering = 6,
    }
};

ScheduleSolver solver = new ScheduleSolver();
solver.Solve(instructors, timeslots, rooms, courses);