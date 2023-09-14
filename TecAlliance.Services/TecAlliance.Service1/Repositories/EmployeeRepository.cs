using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using TecAlliance.Service1.Models;

namespace TecAlliance.Service1.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployeeById(int id);
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);
    }

    public class EmployeeRepository:IEmployeeRepository
    {
        private List<Employee> _employees;
        private readonly string _filePath;
        private readonly IMemoryCache _memoryCache;

        public EmployeeRepository(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _filePath = configuration["EmployeeRepository:FilePath"];
            _memoryCache = memoryCache;
            LoadData();
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employees;
        }

        public Employee GetEmployeeById(int id)
        {
            return _employees.FirstOrDefault(e => e.Id == id);
        }

        public void AddEmployee(Employee employee)
        {
            employee.Id = GenerateNewEmployeeId();
            _employees.Add(employee);
            SaveData();
        }

        public void UpdateEmployee(Employee employee)
        {
            var existingEmployee = _employees.FirstOrDefault(e => e.Id == employee.Id);

            // Update the properties of the existing employee, We can apply AutoMapper
            existingEmployee.Name = employee.Name;
            existingEmployee.Position = employee.Position;
            existingEmployee.HiringDate = employee.HiringDate;
            existingEmployee.Salary = employee.Salary;

            SaveData();
        }

        public void DeleteEmployee(int id)
        {
            var employeeToRemove = _employees.FirstOrDefault(e => e.Id == id);
            if (employeeToRemove != null)
            {
                _employees.Remove(employeeToRemove);
                SaveData();
            }
        }

        #region private methods

        private void LoadData()
        {
            if (!_memoryCache.TryGetValue("employees", out _employees))
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    _employees = JsonConvert.DeserializeObject<List<Employee>>(json);

                    _memoryCache.Set("employees", _employees, TimeSpan.FromMinutes(15));
                }
                else
                {
                    _employees = new List<Employee>();
                }
            }
        }

        private int GenerateNewEmployeeId()
        {
            return _employees.Count > 0 ? _employees.Max(e => e.Id) + 1 : 1;
        }

        private void SaveData()
        {
            // Serialize the employees list and save it to the JSON file
            string json = JsonConvert.SerializeObject(_employees, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        #endregion
    }
}
