using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? HeadDoctorId { get; set; }

        [MaxLength(50)]
        public string? Floor { get; set; }

        public int TotalBeds { get; set; } = 0;

        public int AvailableBeds { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Doctor? HeadDoctor { get; set; }

        public ICollection<Doctor>? Doctors { get; set; }
    }
}
