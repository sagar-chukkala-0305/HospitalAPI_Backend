-- ============================================================
-- HOSPITAL MANAGEMENT SYSTEM - MySQL Database Schema
-- ============================================================

CREATE DATABASE IF NOT EXISTS HospitalDB;
USE HospitalDB;

-- ============================================================
-- USERS TABLE (Login)
-- ============================================================
CREATE TABLE IF NOT EXISTS Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Role ENUM('Admin','Doctor','Nurse','Receptionist') NOT NULL DEFAULT 'Receptionist',
    FullName VARCHAR(150) NOT NULL,
    Email VARCHAR(150),
    IsActive TINYINT(1) DEFAULT 1,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Default admin user: admin / Admin@123
INSERT INTO Users (Username, PasswordHash, Role, FullName, Email) VALUES
('admin', '$2a$11$Kq5Z5Z5Z5Z5Z5Z5Z5Z5Z5OexampleHashForAdmin123', 'Admin', 'System Administrator', 'admin@hospital.com'),
('doctor1', '$2a$11$Kq5Z5Z5Z5Z5Z5Z5Z5Z5Z5OexampleHashForDoctor', 'Doctor', 'Dr. John Smith', 'jsmith@hospital.com');

-- ============================================================
-- DEPARTMENTS TABLE
-- ============================================================
CREATE TABLE IF NOT EXISTS Departments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT,
    HeadDoctorId INT,
    Floor VARCHAR(20),
    TotalBeds INT DEFAULT 0,
    AvailableBeds INT DEFAULT 0,
    IsActive TINYINT(1) DEFAULT 1,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO Departments (Name, Description, Floor, TotalBeds, AvailableBeds) VALUES
('Emergency', 'Emergency and trauma care unit', 'Ground Floor', 20, 8),
('Cardiology', 'Heart and cardiovascular diseases', '2nd Floor', 30, 12),
('Neurology', 'Brain and nervous system disorders', '3rd Floor', 25, 10),
('Orthopedics', 'Bone, joint and muscle care', '2nd Floor', 20, 7),
('Pediatrics', 'Child health and care', '4th Floor', 15, 5),
('Radiology', 'Imaging and diagnostics', '1st Floor', 0, 0),
('ICU', 'Intensive Care Unit', '1st Floor', 10, 3),
('General Medicine', 'General outpatient and inpatient care', '3rd Floor', 40, 18);

-- ============================================================
-- DOCTORS TABLE
-- ============================================================
CREATE TABLE IF NOT EXISTS Doctors (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(150) NOT NULL,
    Specialization VARCHAR(100),
    DepartmentId INT,
    Phone VARCHAR(20),
    Email VARCHAR(150),
    Qualification VARCHAR(200),
    Experience INT COMMENT 'Years of experience',
    Status ENUM('Available','On Leave','Off Duty') DEFAULT 'Available',
    JoiningDate DATE,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
);

INSERT INTO Doctors (FullName, Specialization, DepartmentId, Phone, Email, Qualification, Experience, Status, JoiningDate) VALUES
('Dr. Sarah Johnson', 'Cardiologist', 2, '555-0101', 'sjohnson@hospital.com', 'MBBS, MD Cardiology', 12, 'Available', '2015-03-01'),
('Dr. Mark Williams', 'Neurologist', 3, '555-0102', 'mwilliams@hospital.com', 'MBBS, DM Neurology', 8, 'Available', '2018-06-15'),
('Dr. Emily Davis', 'Pediatrician', 5, '555-0103', 'edavis@hospital.com', 'MBBS, MD Pediatrics', 6, 'Available', '2020-01-10'),
('Dr. Robert Brown', 'Orthopedic Surgeon', 4, '555-0104', 'rbrown@hospital.com', 'MBBS, MS Orthopedics', 15, 'On Leave', '2012-09-20'),
('Dr. Lisa Martinez', 'Emergency Specialist', 1, '555-0105', 'lmartinez@hospital.com', 'MBBS, Emergency Medicine', 10, 'Available', '2016-07-01'),
('Dr. James Wilson', 'General Physician', 8, '555-0106', 'jwilson@hospital.com', 'MBBS, MD General Medicine', 5, 'Available', '2021-02-15');

-- ============================================================
-- PATIENTS TABLE
-- ============================================================
CREATE TABLE IF NOT EXISTS Patients (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PatientNo VARCHAR(20) NOT NULL UNIQUE,
    FullName VARCHAR(150) NOT NULL,
    DateOfBirth DATE,
    Gender ENUM('Male','Female','Other'),
    Phone VARCHAR(20),
    Email VARCHAR(150),
    Address TEXT,
    BloodGroup VARCHAR(5),
    EmergencyContact VARCHAR(150),
    EmergencyPhone VARCHAR(20),
    Status ENUM('Admitted','Outpatient','Discharged') DEFAULT 'Outpatient',
    AdmittedDate DATETIME,
    DischargedDate DATETIME,
    DepartmentId INT,
    DoctorId INT,
    BedNo VARCHAR(10),
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id),
    FOREIGN KEY (DoctorId) REFERENCES Doctors(Id)
);

INSERT INTO Patients (PatientNo, FullName, DateOfBirth, Gender, Phone, BloodGroup, Status, AdmittedDate, DepartmentId, DoctorId, BedNo) VALUES
('P-001', 'Alice Thompson', '1985-04-12', 'Female', '555-1001', 'A+', 'Admitted', NOW(), 2, 1, 'C-101'),
('P-002', 'Bob Carter', '1990-08-22', 'Male', '555-1002', 'O+', 'Admitted', NOW(), 3, 2, 'N-201'),
('P-003', 'Carol Lewis', '2010-11-05', 'Female', '555-1003', 'B+', 'Admitted', NOW(), 5, 3, 'P-401'),
('P-004', 'David Harris', '1975-02-18', 'Male', '555-1004', 'AB-', 'Outpatient', NULL, 8, 6, NULL),
('P-005', 'Eva White', '1968-07-30', 'Female', '555-1005', 'A-', 'Admitted', NOW(), 1, 5, 'E-001');

-- ============================================================
-- APPOINTMENTS TABLE
-- ============================================================
CREATE TABLE IF NOT EXISTS Appointments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    DepartmentId INT,
    AppointmentDate DATETIME NOT NULL,
    Reason TEXT,
    Status ENUM('Scheduled','Completed','Cancelled','No Show') DEFAULT 'Scheduled',
    Notes TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id),
    FOREIGN KEY (DoctorId) REFERENCES Doctors(Id),
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
);

INSERT INTO Appointments (PatientId, DoctorId, DepartmentId, AppointmentDate, Reason, Status) VALUES
(1, 1, 2, DATE_ADD(NOW(), INTERVAL 1 DAY), 'Follow-up cardiac checkup', 'Scheduled'),
(2, 2, 3, DATE_ADD(NOW(), INTERVAL 2 DAY), 'MRI review', 'Scheduled'),
(4, 6, 8, NOW(), 'General checkup', 'Completed'),
(5, 5, 1, DATE_ADD(NOW(), INTERVAL 3 DAY), 'Emergency follow-up', 'Scheduled');

-- ============================================================
-- BEDS TABLE
-- ============================================================
CREATE TABLE IF NOT EXISTS Beds (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    BedNo VARCHAR(10) NOT NULL UNIQUE,
    DepartmentId INT NOT NULL,
    Type ENUM('General','Semi-Private','Private','ICU') DEFAULT 'General',
    Status ENUM('Available','Occupied','Maintenance') DEFAULT 'Available',
    PatientId INT,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id),
    FOREIGN KEY (PatientId) REFERENCES Patients(Id)
);

-- ============================================================
-- DASHBOARD STATS VIEW
-- ============================================================
CREATE OR REPLACE VIEW vw_DashboardStats AS
SELECT
    (SELECT COUNT(*) FROM Patients) AS TotalPatients,
    (SELECT COUNT(*) FROM Patients WHERE Status = 'Admitted') AS AdmittedPatients,
    (SELECT COUNT(*) FROM Patients WHERE Status = 'Outpatient') AS OutPatients,
    (SELECT COUNT(*) FROM Doctors WHERE Status = 'Available') AS AvailableDoctors,
    (SELECT COUNT(*) FROM Doctors) AS TotalDoctors,
    (SELECT COUNT(*) FROM Departments WHERE IsActive = 1) AS TotalDepartments,
    (SELECT SUM(TotalBeds) FROM Departments) AS TotalBeds,
    (SELECT SUM(AvailableBeds) FROM Departments) AS AvailableBeds,
    (SELECT COUNT(*) FROM Appointments WHERE DATE(AppointmentDate) = CURDATE()) AS TodayAppointments,
    (SELECT COUNT(*) FROM Appointments WHERE Status = 'Scheduled' AND AppointmentDate >= NOW()) AS UpcomingAppointments;

-- ============================================================
-- AUDIT LOG TABLE
-- ============================================================
CREATE TABLE IF NOT EXISTS AuditLogs (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT,
    Action VARCHAR(200),
    TableName VARCHAR(50),
    RecordId INT,
    LoggedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
