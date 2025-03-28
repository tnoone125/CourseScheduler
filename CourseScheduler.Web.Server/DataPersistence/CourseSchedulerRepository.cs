using CourseScheduler.Web.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseScheduler.Web.Server.DataPersistence
{
    public class CourseSchedulerRepository
    {
        private readonly CourseSchedulerContext _context;

        public CourseSchedulerRepository(CourseSchedulerContext context)
        {
            _context = context;
        }

        public async Task UpsertInstructorsAsync(IEnumerable<Instructor> instructors)
        {
            foreach (var instructor in instructors)
            {
                var existingInstructor = await _context.Instructors
                    .FirstOrDefaultAsync(i => i.Name == instructor.Name);

                if (existingInstructor == null)
                {
                    _context.Instructors.Add(instructor);
                }
                else
                {
                    _context.Entry(existingInstructor).CurrentValues.SetValues(instructor);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Room>> UpsertRoomsAsync(List<CourseScheduler.Web.Server.Models.Room> rooms)
        {
            var insertedRooms = new List<Room>();
            foreach (var room in rooms)
            {
                var existingRoom = await _context.Rooms
                    .Include(r => r.PreferredDepartmentRooms)
                    .FirstOrDefaultAsync(r => r.Name == room.Name);

                if (existingRoom == null)
                {
                    var roomToInsert = new Room
                    {
                        Name = room.Name,
                        StudentCapacity = room.StudentCapacity,
                        PreferredDepartmentRooms = room.PermittedDepartments.Select(d =>
                            new PreferredDepartmentRoom
                            {
                                Department = d,
                            }
                        ).ToList()
                    };
                    _context.Rooms.Add(roomToInsert);
                    insertedRooms.Add(roomToInsert);
                }
                else
                {
                    await UpdatePreferredDepartments(existingRoom, room.PermittedDepartments.Select(d => new PreferredDepartmentRoom
                    {
                        Department = d,
                        Room = existingRoom,
                        RoomId = existingRoom.Id,
                    }));
                    insertedRooms.Add(existingRoom);
                }
            }

            await _context.SaveChangesAsync();

            foreach (var room in insertedRooms)
            {
                await UpsertPreferredDepartmentsAsync(room.Id, room.PreferredDepartmentRooms);
            }

            return insertedRooms;
        }

        private async Task UpdatePreferredDepartments(Room existingRoom, IEnumerable<PreferredDepartmentRoom> newDepartments)
        {
            // Remove departments that are no longer associated
            foreach (var existingDept in existingRoom.PreferredDepartmentRooms.ToList())
            {
                if (!newDepartments.Any(d => d.Department == existingDept.Department))
                {
                    _context.PreferredDepartmentRooms.Remove(existingDept);
                }
            }

            foreach (var newDept in newDepartments)
            {
                var existingDept = existingRoom.PreferredDepartmentRooms
                    .FirstOrDefault(d => d.Department == newDept.Department);

                if (existingDept == null)
                {
                    existingRoom.PreferredDepartmentRooms.Add(newDept);
                }
                else
                {
                    _context.Entry(existingDept).CurrentValues.SetValues(newDept);
                }
            }
            await _context.SaveChangesAsync();
        }

        // Upsert PreferredDepartments for a specific Room ID
        private async Task UpsertPreferredDepartmentsAsync(int roomId, IEnumerable<PreferredDepartmentRoom> departments)
        {
            foreach (var dept in departments)
            {
                dept.RoomId = roomId;

                var existingDept = await _context.PreferredDepartmentRooms
                    .FirstOrDefaultAsync(d => d.Department == dept.Department);

                if (existingDept == null)
                {
                    _context.PreferredDepartmentRooms.Add(dept);
                }
                else
                {
                    _context.Entry(existingDept).CurrentValues.SetValues(dept);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
