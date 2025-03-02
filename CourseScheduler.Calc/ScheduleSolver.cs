using Google.OrTools.Sat;

namespace CourseScheduler
{
    class ScheduleSolver
    {
        public void Solve(List<Instructor> instructors, List<Timeslot> timeslots, List<Room> rooms, List<Course> courses)
        {
            if (!IsValid(instructors, timeslots, rooms, courses))
            {
                Console.WriteLine("Cannot create schedule with the provided parameters.");
                return;
            }
            CpModel model = new CpModel();

            // Define the variable x[c, i, r, t] (binary decision variable)
            // s[c, i, r, t] = 1 if course c is assigned to timeslot t with instructor i and room r
            Dictionary<(int c, int i, int r, int t), BoolVar> assignments = new Dictionary<(int, int, int, int), BoolVar>();

            for (int c = 0; c < courses.Count; c++)
            {
                for (int i = 0; i < instructors.Count; i++)
                {
                    for (int r = 0; r < rooms.Count; r++)
                    {
                        for (int t = 0; t < timeslots.Count; t++)
                        {
                            assignments[(c, i, r, t)] = model.NewBoolVar($"s_{c}_{i}_{r}_{t}");
                        }
                    }
                }
            }

            // Constraint: Each course is assigned exactly one instructor, room, and timeslot
            for (int c = 0; c < courses.Count; c++)
            {
                LinearExpr sum = LinearExpr.Constant(0);
                for (int i = 0; i < instructors.Count; i++)
                {
                    for (int r = 0; r < rooms.Count; r++)
                    {
                        for (int t = 0; t < timeslots.Count; t++)
                        {
                            sum += assignments[(c, i, r, t)];
                        }
                    }
                }

                // Add the constraint: Each course must be assigned exactly once
                model.Add(sum == 1);
            }

            // Constraint: no course should be assigned to a room with not enough capacity.
            for (int c = 0; c < courses.Count; c++)
            {
                LinearExpr doNotAssign = assignments.Where(kv => kv.Key.c == c && rooms[kv.Key.r].Capacity < courses[c].EnrollmentCount)
                                                    .Select(kv => kv.Value)
                                                    .Aggregate(LinearExpr.Constant(0), (acc, v) => acc + v);

                model.Add(doNotAssign == 0);
            }

            // Constraint: No instructor is assigned more than their allotted limit
            for (int i = 0; i < instructors.Count; i++)
            {
                int limit = instructors[i].CourseLimit;
                LinearExpr assigns = assignments.Where(kv => kv.Key.i == i)
                                                .Select(kv => kv.Value)
                                                .Aggregate(LinearExpr.Constant(0), (acc, v) => acc + v);
                model.Add(assigns <= limit);
            }

            // Constraint: No instructor is assigned under their allotted minimum
            for (int i = 0; i < instructors.Count; i++)
            {
                int minimum = instructors[i].CourseMinimum;
                LinearExpr assigns = assignments.Where(kv => kv.Key.i == i)
                                                .Select(kv => kv.Value)
                                                .Aggregate(LinearExpr.Constant(0), (acc, v) => acc + v);
                model.Add(assigns >= minimum);
            }

            // Constraint: at each timeslot and room pair, only one course is assigned.
            for (int t = 0; t < timeslots.Count; t++)
            {
                for (int r = 0; r < rooms.Count; r++)
                {
                    LinearExpr vs = assignments.Where(kv => kv.Key.r == r && kv.Key.t == t)
                                                  .Select(kv => kv.Value)
                                                  .Aggregate(LinearExpr.Constant(0), (acc, v) => acc + v);
                    
                    model.Add(vs <= 1);
                }
            }

            // Constraint: No instructor should be assigned to multiple courses at one time.
            for (int i = 0; i < instructors.Count; i++)
            {
                for (int t = 0; t < timeslots.Count; t++)
                {
                    LinearExpr vs = assignments.Where(kv => kv.Key.i == i && kv.Key.t == t)
                                                  .Select(kv => kv.Value)
                                                  .Aggregate(LinearExpr.Constant(0), (acc, v) => acc + v);

                    model.Add(vs <= 1);
                }
            }

            // LinearExpr objectiveFunction = CreateObjectiveFunction(assignments, instructors, timeslots, rooms, courses);
            // model.Minimize(objectiveFunction);

            // Create the solver and solve
            CpSolver solver = new CpSolver();
            CpSolverStatus status = solver.Solve(model);

            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                Console.WriteLine("Solution found:");
                for (int c = 0; c < courses.Count; c++)
                {
                    for (int i = 0; i < instructors.Count; i++)
                    {
                        for (int r = 0; r < rooms.Count; r++)
                        {
                            for (int t = 0; t < timeslots.Count; t++)
                            {
                                if (solver.BooleanValue(assignments[(c, i, r, t)]))
                                {
                                    Console.WriteLine($"Course {courses[c].Name} is assigned to Instructor {instructors[i].Name}, Room {rooms[r].DisplayName}, Timeslot {timeslots[t].DisplayName}");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No feasible solution found.");
            }
        }

        protected LinearExpr CreateObjectiveFunction(Dictionary<(int c, int i, int r, int t), BoolVar> assignments,
            List<Instructor> instructors,
            List<Timeslot> timeslots,
            List<Room> rooms,
            List<Course> courses)
        {
            var instructorClassCounts = new List<LinearExpr>(new LinearExpr[instructors.Count]);
            // Penalize disparities in class assignments
            LinearExpr obj = LinearExpr.Constant(0);

            // Initialize each entry with LinearExpr.Constant(0)
            for (int i = 0; i < instructors.Count; i++)
            {
                instructorClassCounts[i] = LinearExpr.Constant(0);
            }

            // Count the assignments per instructor
            foreach (var ((c, i, r, t), varVar) in assignments)
            {
                if (i >= 0 && i < instructors.Count)
                {
                    obj += LinearExpr.Term(LinearExpr.Sum(new[] { instructors[i].CourseLimit - instructorClassCounts[i], varVar }), 3);
                }
            }

            return obj;
        }


        // Returns true if initial validity checks over the inputs seem to show valid constraints
        // (primarily enough rooms, timeslots, and instructors for the given courses)
        protected bool IsValid(List<Instructor> instructors, List<Timeslot> timeslots, List<Room> rooms, List<Course> courses)
        {
            // Check that the total number of rooms and timeslots are sufficient
            var roomSlotCombos = rooms.Count * timeslots.Count;
            if (courses.Count > roomSlotCombos)
            {
                return false;
            }

            // Find if there is any course that exceeds the capacity of all the rooms
            var cannotFitCourse = courses.Any(c => rooms.All(r => r.Capacity < c.EnrollmentCount));
            if (cannotFitCourse)
            {
                return false;
            }

            var totalInstructorSlots = instructors.Select(i => i.CourseLimit).Sum();
            if (totalInstructorSlots < courses.Count)
            {
                return false;
            }

            var minimumInstructorSlots = instructors.Select(i => i.CourseMinimum).Sum();
            if (courses.Count < minimumInstructorSlots)
            {
                return false;
            }

            return true;
        }
    }
}
