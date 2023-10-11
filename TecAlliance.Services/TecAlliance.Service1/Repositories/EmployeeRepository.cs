using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using TecAlliance.Service1.Models;

namespace TecAlliance.Service1.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployeeById(int id);
        Task AddEmployee(Employee employee);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(int id);
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

        public async Task AddEmployee(Employee employee)
        {
            employee.Id = GenerateNewEmployeeId();
            _employees.Add(employee);
            await SaveData();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var existingEmployee = _employees.FirstOrDefault(e => e.Id == employee.Id);

            // Update the properties of the existing employee, We can apply AutoMapper
            existingEmployee.Name = employee.Name;
            existingEmployee.Position = employee.Position;
            existingEmployee.HiringDate = employee.HiringDate;
            existingEmployee.Salary = employee.Salary;

            await SaveData();
        }

        public async Task DeleteEmployee(int id)
        {
            var employeeToRemove = _employees.FirstOrDefault(e => e.Id == id);
            if (employeeToRemove != null)
            {
                _employees.Remove(employeeToRemove);
                await SaveData();
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

        private async Task SaveData()
        {
            // Serialize the employees list and save it to the JSON file
            string json = JsonConvert.SerializeObject(_employees, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }

        #endregion
    }
}
