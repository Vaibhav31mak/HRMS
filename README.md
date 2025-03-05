# HR Management System (HRMS)

## 🚀 Project Overview

The HR Management System (HRMS) is a versatile solution designed to streamline HR processes across various industries. The system offers robust features for managing employees, automating payroll, handling attendance via Excel sheets, and managing dynamic roles with claims-based authorization. The dynamic role management through claims in MSSQL ensures flexible authorization and authentication.

Payroll calculation is optimized using stored procedures, significantly reducing processing time. The stored procedure approach delivers payroll outputs for numerous employees within seconds, compared to the ORM-based approach, which takes minutes.

Both the backend and frontend are deployed on **Azure**, while the frontend is also hosted on **Vercel** for enhanced accessibility.

## ✨ Key Features

### 🔹 Employee Management

- Create, Read, Update, Delete (CRUD) operations for employee data
- Team management with role-based access
- Individual employee profiles and detailed views

### 🔹 HR Payroll Management

- Bulk salary updates
- Optimized payroll processing using **Stored Procedures**
- Automated attendance processing through Excel sheet uploads
- Salary calculations considering weekdays, overtime, and days off
- Comprehensive tax management

### 🔹 Attendance Management

- HR uploads attendance data via **Excel sheet**
- System provides automated templates for accurate logging
- Integrated with payroll for real-time updates

### 🔹 Role Management

- Dynamic roles added via **Claims in MSSQL**
- Default roles include **Superadmin and Admin**
- Fine-grained control over **CRUD operations** through claims

## 🛠️ Tech Stack

### Frontend:

- **Angular** for responsive UI

### Backend:

- **.NET 8 Web API** with **Entity Framework Core 8.0**
- **SQL Server** for data storage and management
- **Stored Procedures** for optimized database operations
- **LINQ** for efficient querying and data manipulation

### Other Tools & Technologies:

- **Email Notifications** via SMTP
- **Azure Deployment** (Student account from GitHub Student)
- **Vercel** for additional frontend hosting

## 🚦 Getting Started

1. Clone the repository:

   ```sh
   git clone https://github.com/Vaibhav31mak/HRMS.git
   cd HRMS
   ```

2. Setup Backend (.NET 8 Web API):

   - Ensure **.NET 8 SDK** is installed
   - Run database migrations:
     ```sh
     dotnet ef database update
     ```
   - Start the API server:
     ```sh
     dotnet run
     ```

3. Setup Frontend (Angular):

   - Navigate to the Angular project directory:
     ```sh
     cd AngularUI
     ```
   - Install dependencies:
     ```sh
     npm install
     ```
   - Start the development server:
     ```sh
     ng serve
     ```

4. Access the Application:

   - Open **[http://localhost:4200](http://localhost:4200)** in your browser

## 📦 Deployment

- The backend is deployed on **Azure** using **GitHub Student** account benefits:\
  🔗 [Backend API - Swagger Documentation](https://hrms-app-service-g0frh0djd3bug6gh.centralindia-01.azurewebsites.net/swagger/index.html)
- The frontend is hosted on **Azure** and also deployed on **Vercel** for increased reliability:\
  🔗 [Frontend Application](https://hrms-client-eyejb3adcth0f7ae.centralindia-01.azurewebsites.net)
- Ensure that all necessary **environment variables** and **database connections** are properly configured for deployment.

## 🤝 Contributing

Contributions are welcome! Please **open issues** or **submit pull requests** to help improve this project.

Thank you for checking out the HR Management System! Your feedback and suggestions are highly appreciated! 🚀

