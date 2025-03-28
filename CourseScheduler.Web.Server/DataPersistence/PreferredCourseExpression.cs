using System.ComponentModel.DataAnnotations;
using CourseScheduler.Web.Server.DataPersistence;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseScheduler.Web.Server.DataPersistence
{
    [Table("preferredCourseExpressions", Schema = "schdl")]
    public class PreferredCourseExpression
    {
        [Key, Column(Order = 0)]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [Key, Column(Order = 1)]
        public int ExpressionId { get; set; }

        [ForeignKey("ExpressionId")]
        public Expression Expression { get; set; }
    }
}