using EmployeeDirectory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeDirectory.Controllers;
using EmployeeDirectory.Models;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace EmployeeDirectoryTest
{
    public class UnitTest1
    {
        public class TestBase : IAsyncLifetime
        {
            protected readonly Mock<IRepository> repositoryStub = new();
            protected Random rand = new();

            protected Employee[] expectedData;
           // protected string ProcessedCommand;
            protected List<Employee> EmployeeList;
            protected Employee Employee;
            protected Employee ProcessedEmployee;


            public TestBase()
            {
                repositoryStub = new Mock<IRepository>();
            }

            public async Task InitializeAsync()
                => await Run();

            public Task DisposeAsync()
                => Task.CompletedTask;

            protected virtual async Task Run()
                => await Task.CompletedTask;
        }

        public class WhenOneEmployeeIsRequired : TestBase
        {
            protected readonly Mock<IRepository> repositoryStub = new();
            private readonly Mock<ILogger<ValuesController>> loggerStub = new();
            protected Random rand = new();
            public WhenOneEmployeeIsRequired()
            {
                var expectedItem =  CreateEmployee();
                repositoryStub.Setup(e =>e.GetEmployeeAsync(It.IsAny<int>()))
                     .ReturnsAsync(expectedItem);
                //var controller = new ValuesController(repositoryStub.Object, loggerStub.Object);
                //controller.GetEmployeeAsync(rand.Next());
            }

            protected override async Task Run()
            {
                var ProcessedCommand = await new ValuesController(repositoryStub.Object, loggerStub.Object).GetEmployeeAsync(rand.Next());

            }

            [Fact]
            public void EmployeeApiIsCalled()
                => repositoryStub.Verify(e => e.GetEmployeeAsync(rand.Next()), Times.Once);
            
            public Employee CreateEmployee()
            {
                return new()
                {
                    EmployeeId = rand.Next(100),
                    FirstName = Guid.NewGuid().ToString(),
                    LastName = Guid.NewGuid().ToString(),
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString() + "@gamil.com"
                };
            }
        }
        public class WhenOneEmployeesAreRequired : TestBase
        {
            protected readonly Mock<IRepository> repositoryStub = new();
            protected Random rand = new();
            private readonly Mock<ILogger<ValuesController>> loggerStub = new();
          
            public WhenOneEmployeesAreRequired()
            {
                expectedData = new [] {CreateEmployee(), CreateEmployee(), CreateEmployee()};
                //EmployeeList = new List<Employee>(expectedData);
                repositoryStub.Setup(e => e.GetEmployees())
                     .Returns(expectedData);
                var controller= new ValuesController(repositoryStub.Object, loggerStub.Object);
                controller.GetEmployees();
            }

            [Fact]
            public void EmployeeApiIsCalled()
                => repositoryStub.Verify(e => e.GetEmployees(), Times.Once);

            [Fact]
            public void EmployeeApiReturnData()
            {
                //var processedString = EmployeeList.Aggregate("", (current, employee)
                //=> current + ($"employeID: {employee.EmployeeId}\r\n" + $"firstName: {employee.FirstName}\r\n" + $"lastName: {employee.LastName}\r\n" + $"email: {employee.Email}\r\n" + $"department: {employee.Department}\r\n\r\n"));
                repositoryStub.Should().BeEquivalentTo(expectedData);
            }
            public Employee CreateEmployee()
            {
                return new()
                {
                    EmployeeId = rand.Next(100),
                    FirstName = Guid.NewGuid().ToString(),
                    LastName = Guid.NewGuid().ToString(),
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString() + "@gamil.com"
                };
            }
        }
        public class WhenEmployeeIsDeleted : TestBase
        {
            protected readonly Mock<IRepository> repositoryStub = new();
            protected Random rand = new();
            private readonly Mock<ILogger<ValuesController>> loggerStub = new();

            public WhenEmployeeIsDeleted()
            {
                var existingItem = CreateEmployee();
                repositoryStub.Setup(e => e.GetEmployeeAsync(It.IsAny<int>()))
                    .ReturnsAsync(existingItem);
                var controller = new ValuesController(repositoryStub.Object, loggerStub.Object);
                controller.DeleteEmployee(existingItem.EmployeeId);
            }

            [Fact]
            public void EmployeeApiIsCalled()
                => repositoryStub.Verify(e => e.DeleteEmployee(rand.Next()), Times.Once);

            public Employee CreateEmployee()
            {
                return new()
                {
                    EmployeeId = rand.Next(100),
                    FirstName = Guid.NewGuid().ToString(),
                    LastName = Guid.NewGuid().ToString(),
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString() + "@gamil.com"
                };
            }
        }
        public class WhenEmployeeIsAdded : TestBase
        {
            protected readonly Mock<IRepository> repositoryStub = new();
            protected Random rand = new();
            protected Employee Employee;
            private readonly Mock<ILogger<ValuesController>> loggerStub = new();

            public WhenEmployeeIsAdded()
            {
                var itemToCreate = new Employee()
                {
                    FirstName = Guid.NewGuid().ToString(),
                    LastName = Guid.NewGuid().ToString(),
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString() + "@gamil.com"
                };
               
                repositoryStub.Setup(e => e.AddEmployee(Employee)).Callback((Employee e) => ProcessedEmployee = e);
                var controller = new ValuesController(repositoryStub.Object, loggerStub.Object);
                controller.AddEmployee(itemToCreate);
            }

            [Fact]
            public void EmployeeApiIsCalled()
                => repositoryStub.Verify(e => e.AddEmployee(Employee), Times.Once);

        }


    }
}
