using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebAPI.Constants;
using WebAPI.Models;
using static WebAPI.Constants.Permissions;

namespace WebAPI.Seeds
{
    public class GeneralSeeders
    {
        public static async Task<bool> Seed(HRDBContext context)
        {
            bool is_success;
            is_success = await SeedDepartments(context);
            is_success = await SeedEmployees(context);

            if (!is_success) return false;
            else return true;
        }

        public static async Task<bool> SeedDepartments(HRDBContext context)
        {
            if (!context.Departments.Any())
            {
                var departments = new List<Department>
                {
                    new Department { Name = "Human Resources" },
                    new Department { Name = "Finance" },
                    new Department { Name = "IT" }
                };
                //reset the autoincrment id counter
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Departments', RESEED, 0)");
                await context.Departments.AddRangeAsync(departments);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> SeedEmployees(HRDBContext context)
        {
            var department = await context.Departments.FirstOrDefaultAsync(d => d.Id == 1);
            if (department == null) return await SeedDepartments(context);

            if (!context.Employees.Any())
            {
                var employees = new List<Employee>
                {
                    new Employee { SSN = 123456789, FullName = "John Doe", Address = "8 Ashutosh", PhoneNumber = "9879143752", Gender = Gender.Male, Nationality = "Indian", BirthDate = new DateOnly(2004, 4, 20), ContractDate = new DateOnly(2023, 1, 15), BaseSalary = 50000, Arrival = new TimeOnly(9, 0, 0), Departure = new TimeOnly(17, 0, 0), DeptId = 1 },
                    new Employee { SSN = 987654321, FullName = "Jane Smith", Address = "12 Green Street", PhoneNumber = "9812345678", Gender = Gender.Female, Nationality = "American", BirthDate = new DateOnly(1995, 7, 10), ContractDate = new DateOnly(2022, 5, 20), BaseSalary = 60000, Arrival = new TimeOnly(8, 30, 0), Departure = new TimeOnly(16, 30, 0), DeptId = 1 },
                    new Employee { SSN = 456123789, FullName = "Alice Brown", Address = "25 Blue Avenue", PhoneNumber = "9785641230", Gender = Gender.Female, Nationality = "British", BirthDate = new DateOnly(1990, 3, 15), ContractDate = new DateOnly(2021, 9, 5), BaseSalary = 70000, Arrival = new TimeOnly(10, 0, 0), Departure = new TimeOnly(18, 0, 0), DeptId = 1 },
                    new Employee { SSN = 741852963, FullName = "Michael Johnson", Address = "78 Ocean Drive", PhoneNumber = "9998877665", Gender = Gender.Male, Nationality = "Canadian", BirthDate = new DateOnly(1988, 11, 30), ContractDate = new DateOnly(2020, 12, 10), BaseSalary = 80000, Arrival = new TimeOnly(7, 30, 0), Departure = new TimeOnly(15, 30, 0), DeptId = 1 },
                    new Employee { SSN = 369258147, FullName = "Emily Davis", Address = "99 Sunset Blvd", PhoneNumber = "9876543201", Gender = Gender.Female, Nationality = "Australian", BirthDate = new DateOnly(1992, 6, 25), ContractDate = new DateOnly(2023, 3, 12), BaseSalary = 55000, Arrival = new TimeOnly(9, 15, 0), Departure = new TimeOnly(17, 15, 0), DeptId = 1 },
                    new Employee { SSN = 852147963, FullName = "Robert Wilson", Address = "45 King Street", PhoneNumber = "9870001112", Gender = Gender.Male, Nationality = "Indian", BirthDate = new DateOnly(1998, 2, 5), ContractDate = new DateOnly(2022, 7, 18), BaseSalary = 62000, Arrival = new TimeOnly(8, 45, 0), Departure = new TimeOnly(16, 45, 0), DeptId = 1 },
                    new Employee { SSN = 159357486, FullName = "Sophia Martinez", Address = "30 Liberty Lane", PhoneNumber = "9873216540", Gender = Gender.Female, Nationality = "Mexican", BirthDate = new DateOnly(1997, 12, 12), ContractDate = new DateOnly(2021, 8, 25), BaseSalary = 65000, Arrival = new TimeOnly(9, 0, 0), Departure = new TimeOnly(17, 0, 0), DeptId = 1 },
                    new Employee { SSN = 951753468, FullName = "James Anderson", Address = "7 River Road", PhoneNumber = "9876547777", Gender = Gender.Male, Nationality = "British", BirthDate = new DateOnly(1985, 10, 8), ContractDate = new DateOnly(2020, 5, 30), BaseSalary = 90000, Arrival = new TimeOnly(7, 0, 0), Departure = new TimeOnly(15, 0, 0), DeptId = 1 },
                    new Employee { SSN = 357159284, FullName = "Olivia Harris", Address = "14 Palm Street", PhoneNumber = "9871112223", Gender = Gender.Female, Nationality = "French", BirthDate = new DateOnly(1993, 9, 18), ContractDate = new DateOnly(2023, 6, 1), BaseSalary = 58000, Arrival = new TimeOnly(9, 30, 0), Departure = new TimeOnly(17, 30, 0), DeptId = 1 },
                    new Employee { SSN = 789654123, FullName = "David Wilson", Address = "65 Maple Drive", PhoneNumber = "9875556667", Gender = Gender.Male, Nationality = "American", BirthDate = new DateOnly(1990, 4, 22), ContractDate = new DateOnly(2021, 4, 10), BaseSalary = 73000, Arrival = new TimeOnly(8, 0, 0), Departure = new TimeOnly(16, 0, 0), DeptId = 1 }
                };
                //reset the autoincrment id counter
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Employees', RESEED, 0)");
                await context.Employees.AddRangeAsync(employees);
                await context.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
