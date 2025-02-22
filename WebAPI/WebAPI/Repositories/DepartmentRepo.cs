using WebAPI.Interfaces;
using WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Repositories
{
    public class DepartmentRepo : IDepartmentRepo
    {
        private readonly HRDBContext dBContext;
        public DepartmentRepo(HRDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public List<Department> GetAll()
        {
            return dBContext.Departments.ToList();
        }

        public Department GetByName(string name)
        {
            return dBContext.Departments.SingleOrDefault(d => d.Name == name);
        }

        public Department GetById(int id)
        {
            return dBContext.Departments.Find(id);
        }

        public void Create(Department department)
        {
            dBContext.Departments.Add(department);
            dBContext.SaveChanges();
        }

        public void Update(Department department)
        {
            dBContext.Departments.Update(department);
            dBContext.SaveChanges();
        }

        public void Delete(Department department)
        {
            dBContext.Departments.Remove(department);
            dBContext.SaveChanges();
        }
    }
}
