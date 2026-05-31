using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalAPI.Data;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FacilitiesController : ControllerBase
    {
        private readonly HospitalDbContext _db;

        public FacilitiesController(HospitalDbContext db) => _db = db;

        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var depts = await _db.Departments
                .Where(d => d.IsActive)
                .Select(d => new {
                    d.Id,
                    d.Name,
                    d.Description,
                    d.Floor,
                    d.TotalBeds,
                    d.AvailableBeds,
                    DoctorCount = _db.Doctors.Count(doc => doc.DepartmentId == d.Id)
                })
                .ToListAsync();
            return Ok(depts);
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _db.Doctors
                .Include(d => d.Department)
                .Select(d => new {
                    d.Id,
                    d.FullName,
                    d.Specialization,
                    d.Phone,
                    d.Email,
                    d.Qualification,
                    d.Experience,
                    d.Status,
                    d.JoiningDate,
                    Department = d.Department != null ? d.Department.Name : ""
                })
                .ToListAsync();
            return Ok(doctors);
        }

        [HttpGet("beds")]
        public async Task<IActionResult> GetBedSummary()
        {
            var beds = await _db.Departments
                .Where(d => d.IsActive && d.TotalBeds > 0)
                .Select(d => new {
                    d.Id,
                    d.Name,
                    d.Floor,
                    d.TotalBeds,
                    d.AvailableBeds,
                    OccupiedBeds = d.TotalBeds - d.AvailableBeds,
                    OccupancyRate = d.TotalBeds > 0
                        ? Math.Round((double)(d.TotalBeds - d.AvailableBeds) / d.TotalBeds * 100, 1)
                        : 0.0
                })
                .ToListAsync();
            return Ok(beds);
        }
    }
}
