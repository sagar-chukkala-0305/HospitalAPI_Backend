using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    [Table("Doctors")]
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Specialization { get; set; }

        public int? DepartmentId { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Qualification { get; set; }

        public int? Experience { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Available";

        public DateTime? JoiningDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
    }
}
