using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    [Table("Patients")]
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string PatientNo { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }

        public string? Address { get; set; }

        [MaxLength(5)]
        public string? BloodGroup { get; set; }

        [MaxLength(150)]
        public string? EmergencyContact { get; set; }

        [MaxLength(20)]
        public string? EmergencyPhone { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Outpatient";

        public DateTime? AdmittedDate { get; set; }

        public DateTime? DischargedDate { get; set; }

        public int? DepartmentId { get; set; }

        public int? DoctorId { get; set; }

        [MaxLength(10)]
        public string? BedNo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor? Doctor { get; set; }
    }
}
