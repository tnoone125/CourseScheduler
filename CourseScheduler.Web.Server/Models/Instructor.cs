using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseScheduler.Web.Server.Models
{
    [Table("instructors", Schema = "schdl")]
    public class Instructor
    {
        [Key]
        [StringLength(55)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(55)]
        public string Department { get; set; }
        
        public int? CourseMin { get; set; }
        
        public int? CourseMax { get; set; }
    }
}