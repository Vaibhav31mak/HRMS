# HR Management System (HRMS)

## 🚀 Project Overview
The **HR Management System (HRMS)** is a versatile solution designed to streamline HR processes across various industries. The system offers robust features for managing employees, automating payroll, handling attendance via Excel sheets, and managing dynamic roles with claims-based authorization. The dynamic role management through claims in MSSQL ensures flexible authorization and authentication.

---

## ✨ Key Features
### 🔹 **Employee Management**
- Create, Read, Update, Delete (CRUD) operations for employee data
- Team management with role-based access
- Individual employee profiles and detailed views

### 🔹 **HR Payroll Management**
- Bulk salary updates
- Optimized payroll processing using **Stored Procedures**
- Automated attendance processing through Excel sheet uploads
- Salary calculations considering **weekdays**, **overtime**, and **days off**
- Comprehensive tax management

### 🔹 **Role Management**
- Dynamic roles added via **Claims** in MSSQL
- Default roles include **Superadmin** and **Admin**
- Fine-grained control over CRUD operations through claims

---

## 🛠️ Tech Stack
### **Frontend:**
- **Angular** for responsive UI

### **Backend:**
- **.NET 8 Web API** with Entity Framework Core 8.0
- **SQL Server** for data storage and management
- **Stored Procedures** for optimized database operations

### **Other Tools & Technologies:**
- **Email Notifications** via SMTP
- **Azure Deployment** (Student account from GitHub Student)

---

## 🚦 Getting Started
### 1. **Clone the repository:**
```sh
git clone https://github.com/Vaibhav31mak/HRMS.git
cd HRMS
```

### 2. **Setup Backend (.NET 8 Web API):**
- Ensure **.NET 8 SDK** is installed
- Run database migrations:
```sh
dotnet ef database update
```
- Start the API server:
```sh
dotnet run
```

### 3. **Setup Frontend (Angular):**
- Navigate to the Angular project directory:
```sh
cd HRMS-Frontend
npm install
ng serve
```

### 4. **Access the Application:**
- Open `http://localhost:4200` in your browser

---

## 📦 Deployment
The project is deployed on **Azure** using the GitHub Student account benefits. Ensure that the necessary environment variables and database connections are configured properly.

---

## 🤝 Contributing
Contributions are welcome! Please open issues or submit pull requests to help improve this project.

---

Thank you for checking out the HR Management System! Your feedback and suggestions are highly appreciated!

