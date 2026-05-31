using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalAPI.Data;
using HospitalAPI.DTOs;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly HospitalDbContext _db;

        public DashboardController(HospitalDbContext db) => _db = db;

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = new DashboardStatsDto
            {
                TotalPatients = await _db.Patients.CountAsync(),
                AdmittedPatients = await _db.Patients.CountAsync(p => p.Status == "Admitted"),
                OutPatients = await _db.Patients.CountAsync(p => p.Status == "Outpatient"),
                TotalDoctors = await _db.Doctors.CountAsync(),
                AvailableDoctors = await _db.Doctors.CountAsync(d => d.Status == "Available"),
                TotalDepartments = await _db.Departments.CountAsync(d => d.IsActive),
                TotalBeds = await _db.Departments.SumAsync(d => d.TotalBeds),
                AvailableBeds = await _db.Departments.SumAsync(d => d.AvailableBeds),
                TodayAppointments = await _db.Appointments.CountAsync(a =>
                    a.AppointmentDate.Date == DateTime.Today),
                UpcomingAppointments = await _db.Appointments.CountAsync(a =>
                    a.Status == "Scheduled" && a.AppointmentDate >= DateTime.Now)
            };
            return Ok(stats);
        }

        [HttpGet("recent-patients")]
        public async Task<IActionResult> GetRecentPatients()
        {
            var patients = await _db.Patients
                .Include(p => p.Department)
                .Include(p => p.Doctor)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .Select(p => new {
                    p.Id,
                    p.PatientNo,
                    p.FullName,
                    p.Status,
                    p.BloodGroup,
                    p.Gender,
                    Department = p.Department != null ? p.Department.Name : "",
                    Doctor = p.Doctor != null ? p.Doctor.FullName : "",
                    p.AdmittedDate
                })
                .ToListAsync();
            return Ok(patients);
        }

        [HttpGet("upcoming-appointments")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            var appts = await _db.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Status == "Scheduled" && a.AppointmentDate >= DateTime.Now)
                .OrderBy(a => a.AppointmentDate)
                .Take(5)
                .Select(a => new {
                    a.Id,
                    Patient = a.Patient != null ? a.Patient.FullName : "",
                    Doctor = a.Doctor != null ? a.Doctor.FullName : "",
                    a.AppointmentDate,
                    a.Reason,
                    a.Status
                })
                .ToListAsync();
            return Ok(appts);
        }
    }
}
