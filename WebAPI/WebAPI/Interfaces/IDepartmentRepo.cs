using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IDepartmentRepo
    {
        List<Department> GetAll();
        Department GetByName(string name);
        Department GetById(int id);
        void Create(Department department);
        void Update(Department department);
        void Delete(Department department);
    }
}
