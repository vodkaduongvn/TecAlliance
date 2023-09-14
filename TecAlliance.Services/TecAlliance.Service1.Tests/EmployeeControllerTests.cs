using Microsoft.AspNetCore.Mvc;
using TecAlliance.Service1.Controllers;
using TecAlliance.Service1.Models;
using Moq;
using TecAlliance.Service1.Repositories;

namespace TecAlliance.Service1.Tests
{
    [TestClass]
    public class EmployeeControllerTests
    {
        private static readonly int _employeeId = 1;
        private readonly Employee _employee = new() { Id = _employeeId, Name = "John Doe" };

        private readonly EmployeeController _employeeController;
        private readonly Mock<IEmployeeRepository> _employeeRepository = new();

        public EmployeeControllerTests() 
        {
            _employeeRepository.Setup(repo => repo.GetEmployeeById(_employeeId)).Returns(_employee);
            _employeeController = new EmployeeController(_employeeRepository.Object);
        }

        [TestMethod]
        public void GetEmployeeById_ReturnsEmployee()
        {
            // Act
            var result = _employeeController.GetEmployee(_employeeId) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var employee = result.Value as Employee;
            Assert.IsNotNull(employee);
            Assert.AreEqual(_employeeId, employee.Id);
        }

        [TestMethod]
        public void GetEmployeeById_ReturnsAllEmployees()
        {
            //Arrange
            var employees = new List<Employee>() 
            {
                new Employee { Id = 1, Name = "John Doe 1" },
                new Employee { Id = 2, Name = "John Doe 2" }
            };
            _employeeRepository.Setup(repo => repo.GetAllEmployees()).Returns(employees);

            // Act
            var result = _employeeController.GetAllEmployees() as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var actualEmployees = result.Value as List<Employee>;
            Assert.IsNotNull(actualEmployees);
            Assert.IsTrue(actualEmployees.Count == 2);
        }

        [TestMethod]
        public void CreateEmployee_ReturnsCreatedEmployee()
        {
            // Arrange
            _employeeRepository.Setup(repo => repo.AddEmployee(It.IsAny<Employee>()))
                .Callback<Employee>(employee =>
                {
                    Assert.AreEqual(_employee.Name, employee.Name);
                });

            // Act
            var result = _employeeController.CreateEmployee(_employee) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);

            var createdEmployee = result.Value as Employee;
            Assert.IsNotNull(createdEmployee);
            Assert.AreEqual(_employee.Name, createdEmployee.Name);
        }

        [TestMethod]
        public void UpdateEmployee_ReturnsNoContent()
        {
            // Arrange
            _employeeRepository.Setup(repo => repo.UpdateEmployee(It.IsAny<Employee>()))
                .Callback<Employee>((employee) =>
                {
                    Assert.AreEqual(_employee.Name, employee.Name);
                });

            // Act
            var result = _employeeController.UpdateEmployee(_employee) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void DeleteEmployee_ReturnsNoContent()
        {
            // Arrange
            _employeeRepository.Setup(repo => repo.DeleteEmployee(_employeeId))
                         .Callback<int>(id =>
                         {
                             Assert.AreEqual(_employeeId, id);
                         });

            // Act
            var result = _employeeController.DeleteEmployee(_employeeId) as NoContentResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);
        }
    }
}