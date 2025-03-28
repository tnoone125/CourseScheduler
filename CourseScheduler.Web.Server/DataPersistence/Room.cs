using CourseScheduler.Web.Server.DataPersistence;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseScheduler.Web.Server.DataPersistence
{
    [Table("rooms", Schema = "schdl")]
    public class Room
    {
        [StringLength(55)]
        public string Name { get; set; }

        [Required]
        public int StudentCapacity { get; set; }

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ICollection<PreferredDepartmentRoom> PreferredDepartmentRooms { get; set; }
    }
}