using Microsoft.AspNetCore.Mvc;
using CourseScheduler.Web.Server.Models;
using System.Text.Json;
using CourseScheduler.Web.Server.SchedulingCalc;

namespace CourseScheduler.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulerController : ControllerBase
    {
        private readonly ILogger<SchedulerController> _logger;
        private readonly ScheduleSolver solver;

        public SchedulerController(ILogger<SchedulerController> logger)
        {
            _logger = logger;
            solver = new ScheduleSolver();
        }

        [HttpPost]
        public IActionResult SubmitSettings([FromBody] JsonInput data)
        {
            _logger.LogInformation(JsonSerializer.Serialize(data));

            List<CourseSection> courseSections = CreateCourseSections(data.Courses);
            List<Expression> expressions = CreateExpressionsFromJsonInput(data);

            this.solver.Solve(data.Instructors, expressions, data.Rooms, courseSections);

            return Ok(new { Message = "Success!!!!" });
        }

        [HttpGet]
        public IActionResult TestEndpoint()
        {
            return Ok(new { Message = "Scheduler API is working!" });
        }

        private List<Expression> CreateExpressionsFromJsonInput(JsonInput input)
        {
            List<Expression> expressions = new List<Expression>();
            for (int i = 0; i < input.Timeslots.Count(); i++)
            {
                var possExpression = input.Timeslots[i];
                List<DaySlot> daySlots = new List<DaySlot>();
                foreach (var item in possExpression)
                {
                    var startAndEnds = item.Value;
                    var timesOnly = startAndEnds.Select(s => (TimeOnly.Parse(s["start"]), TimeOnly.Parse(s["end"]))).ToList();

                    var day = item.Key;
                    switch (day)
                    {
                        case "Monday":
                            daySlots.Add(new DaySlot
                            {
                                Day = Day.MONDAY,
                                TimeSlots = timesOnly,
                            });
                            break;
                        case "Tuesday":
                            daySlots.Add(new DaySlot
                            {
                                Day = Day.TUESDAY,
                                TimeSlots = timesOnly,
                            });
                            break;
                        case "Wednesday":
                            daySlots.Add(new DaySlot
                            {
                                Day = Day.WEDNESDAY,
                                TimeSlots = timesOnly,
                            });
                            break;
                        case "Thursday":
                            daySlots.Add(new DaySlot
                            {
                                Day = Day.THURSDAY,
                                TimeSlots = timesOnly,
                            });
                            break;
                        case "Friday":
                            daySlots.Add(new DaySlot
                            {
                                Day = Day.FRIDAY,
                                TimeSlots = timesOnly,
                            });
                            break;
                        case "Saturday":
                            daySlots.Add(new DaySlot
                            {
                                Day = Day.SATURDAY,
                                TimeSlots = timesOnly,
                            });
                            break;
                        case "Sunday":
                            daySlots.Add(new DaySlot
                            {
                                Day = Day.SUNDAY,
                                TimeSlots = timesOnly,
                            });
                            break;
                        default:
                            throw new Exception("Unrecognized day of week");
                    }
                }
                expressions.Add(new Expression { Slots = daySlots });
            }
            return expressions;
        }

        protected List<CourseSection> CreateCourseSections(List<Course> courses)
        {
            return courses.SelectMany(c =>
            {
                var sections = new List<CourseSection>();
                for (int i = 1; i <= c.NumberOfSections; i++)
                {
                    sections.Add(new CourseSection
                    {
                        Name = c.Name,
                        DisplayName = c.DisplayName,
                        StudentEnrollment = c.Enrollment,
                        PreferredTimeslots = c.PreferredTimeslots,
                        SectionNum = i,
                        Department = c.Department,
                    });
                }
                return sections;
            }).ToList();
        }
    }

    public class JsonInput
    {
        public List<Instructor> Instructors { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Course> Courses { get; set; }
        public List<Dictionary<string, List<Dictionary<string, string>>>> Timeslots { get; set; }
    }
}
