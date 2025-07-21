# Supabase Auth Integration with ASP.NET Core

This project demonstrates how to implement **user authentication (signup and login)** using **Supabase** as the backend and **ASP.NET Core MVC** as the frontend. It is a practical project for IoT-related systems where Supabase can act as a lightweight, serverless backend.

---

## 🚀 Features

- User Signup via Supabase Authentication
- User Login and JWT token retrieval
- Secure token storage using ASP.NET Core `Session`
- Inserting user data into a Supabase PostgreSQL table (`users`)
- REST-based integration without using Entity Framework

---

## 🧰 Technologies Used

- ASP.NET Core MVC (.NET 6/7)
- Supabase (Auth + Database + REST API)
- HttpClient
- System.Text.Json
- Session-based state management

---

## 📦 Project Structure

/Controllers
AccountController.cs
/Models
RegisterViewModel.cs
LoginViewModel.cs
/Services
SupabaseService.cs
/Views
/Account
Register.cshtml
Login.cshtml
Program.cs
appsettings.json


---

## ⚙️ Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/your-username/your-repo-name.git
cd your-repo-name

### 2. Configure Supabase settings
Update appsettings.json:

"Supabase": {
  "Url": "https://your-project-id.supabase.co",
  "ApiKey": "your-anon-public-api-key"
}

### 3. Run the application

Use the .NET CLI or Visual Studio:

```bash
dotnet run
