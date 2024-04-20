using System;
using ShitTests.ValueTypes;
using Xunit;

namespace ShitTests.Tests
{
    public class MySimpleServiceTest
    {
        private const decimal AnyNumber = 999;

        private readonly MySimpleService _simpleService = new();

        [Fact]
        public void GetCommunismSalary_Proletariat_Returns42PlusAllowance()
        {
            // Arrange
            var employeeClass = EmployeeClass.Proletariat;

            // Act
            var result = _simpleService.GetCommunismMoney(employeeClass, 100);

            // Assert;
            Assert.Equal(142, result);
        }


        [Fact]
        public void GetCommunismMoney_Bourgeoisie_Returns0()
        {
            // arrange
            var @class = EmployeeClass.Bourgeoisie;

            // act
            var salary = _simpleService.GetCommunismMoney(@class, AnyNumber);

            // assert
            Assert.Equal(0, salary);
        }

        [Fact]
        public void GetCommunismSalary_UnexpectedClass_ThrowsExceptionForInvalidInput()
        {
            // Arrange
            var invalidEmployeeClass = (EmployeeClass)999; // Invalid enum

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _simpleService.GetCommunismMoney(invalidEmployeeClass, AnyNumber));
        }
    }
}