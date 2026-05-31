# HospitalAPI — ASP.NET Web API Backend
## Open & Run in Visual Studio Community

---

## ✅ Prerequisites
- Visual Studio Community 2022 (with ASP.NET workload)
- .NET 8 SDK
- MySQL Server 8.x running locally
- MySQL Workbench (optional, for DB setup)

---

## 🗄️ STEP 1 — Setup MySQL Database

1. Open MySQL Workbench or MySQL command line
2. Run the database script:
```
mysql -u root -p < hospital_db.sql
```
Or paste contents of `hospital_db.sql` into MySQL Workbench and execute.

---

## 🔑 STEP 2 — Generate BCrypt Password Hash

The admin user needs a hashed password. Run this once in any .NET console:

```csharp
using BCrypt.Net;
string hash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
Console.WriteLine(hash);
```

Then run this SQL to update:
```sql
USE HospitalDB;
UPDATE Users SET PasswordHash = '<paste_hash_here>' WHERE Username = 'admin';
```

---

## ⚙️ STEP 3 — Configure Connection String

Open `appsettings.json` and update your MySQL password:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=HospitalDB;User=root;Password=YOUR_PASSWORD;"
}
```

---

## ▶️ STEP 4 — Run in Visual Studio Community

1. Open `HospitalAPI.csproj` in Visual Studio Community
2. Wait for NuGet packages to restore automatically
3. Press **F5** or click **▶ Run**
4. API starts at: `https://localhost:7xxx` or `http://localhost:5000`
5. Swagger UI opens automatically at `/swagger`

---

## 🌐 API Endpoints

| Method | URL                                    | Auth | Description              |
|--------|----------------------------------------|------|--------------------------|
| POST   | /api/auth/login                        | No   | Login → returns JWT      |
| GET    | /api/dashboard/stats                   | Yes  | Dashboard stats          |
| GET    | /api/dashboard/recent-patients         | Yes  | Recent 5 patients        |
| GET    | /api/dashboard/upcoming-appointments   | Yes  | Upcoming appointments    |
| GET    | /api/facilities/departments            | Yes  | All departments          |
| GET    | /api/facilities/doctors                | Yes  | All doctors              |
| GET    | /api/facilities/beds                   | Yes  | Bed summary              |
| GET    | /api/patients                          | Yes  | All patients             |
| POST   | /api/patients                          | Yes  | Create patient           |
| PUT    | /api/patients/{id}                     | Yes  | Update patient           |
| DELETE | /api/patients/{id}                     | Yes  | Delete patient           |

---

## 🔐 Test Login
```json
POST /api/auth/login
{ "username": "admin", "password": "Admin@123" }
```
