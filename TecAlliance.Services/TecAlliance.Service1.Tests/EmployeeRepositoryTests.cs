using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Service1.Models;
using TecAlliance.Service1.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace TecAlliance.Service1.Tests
{
    [TestClass]
    public class EmployeeRepositoryTests
    {
        private EmployeeRepository _employeeRepository;
        private readonly List<Employee> _employees = new()
        {
                new Employee { Id = 1, Name = "John Doe" },
                new Employee { Id = 2, Name = "Jane Smith" },
                new Employee { Id = 3, Name = "Bob Johnson" }
            };
        private readonly string _filePath = "path_to_employee.json";
        private readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        private readonly IMemoryCache _memoryCache = Mock.Of<IMemoryCache>();

        [TestInitialize]
        public void Initialize()
        {
            _configuration.Setup(c => c["EmployeeRepository:FilePath"]).Returns(_filePath);

            var mockMemoryCache = Mock.Get(_memoryCache);
            mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());
            mockMemoryCache.Object.Set("employees", _employees);

            _employeeRepository = new EmployeeRepository(_configuration.Object, MockMemoryCacheService.GetMemoryCache(_employees));
            //    var cachedResponse = memoryCache.Get("employees");
        }

        [TestMethod]
        public void GetEmployeeById_ExistingEmployee_ReturnsEmployee()
        {
            // Arrange
            var employeeId = 1; 
            var expectedEmployee = _employees.FirstOrDefault(e => e.Id == employeeId);

            // Act
            var result = _employeeRepository.GetEmployeeById(employeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployee.Id, result.Id);
            Assert.AreEqual(expectedEmployee.Name, result.Name);
        }

        [TestMethod]
        public void GetEmployeeById_NonExistingEmployee_ReturnsNull()
        {
            // Arrange
            var employeeId = 10;

            // Act
            var result = _employeeRepository.GetEmployeeById(employeeId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllEmployees_ReturnsAllEmployees()
        {
            // Act
            var result = _employeeRepository.GetAllEmployees();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 3);
        }
    }
}
