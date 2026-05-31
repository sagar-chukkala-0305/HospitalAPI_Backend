using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalAPI.Data;
using HospitalAPI.Models;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly HospitalDbContext _db;

        public PatientsController(HospitalDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _db.Patients
                .Include(p => p.Department)
                .Include(p => p.Doctor)
                .Select(p => new {
                    p.Id,
                    p.PatientNo,
                    p.FullName,
                    p.Gender,
                    p.Phone,
                    p.BloodGroup,
                    p.Status,
                    p.BedNo,
                    p.AdmittedDate,
                    p.DateOfBirth,
                    Department = p.Department != null ? p.Department.Name : "",
                    Doctor = p.Doctor != null ? p.Doctor.FullName : ""
                })
                .ToListAsync();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var patient = await _db.Patients
                .Include(p => p.Department)
                .Include(p => p.Doctor)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Patient patient)
        {
            var count = await _db.Patients.CountAsync();
            patient.PatientNo = $"P-{(count + 1):D3}";
            patient.CreatedAt = DateTime.UtcNow;
            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Patient patient)
        {
            if (id != patient.Id) return BadRequest();
            _db.Entry(patient).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _db.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            _db.Patients.Remove(patient);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
