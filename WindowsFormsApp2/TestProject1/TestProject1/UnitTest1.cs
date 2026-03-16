using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using task2;

namespace UnitTestProject
{
    [TestClass]
    public class BigNumberLogicTests_Add
    {
        // Тест 1: Сложение положительных чисел
        [TestMethod]
        public void Add_PositiveNumbers_ReturnsCorrectResult()
        {
            // Arrange
            string num1 = "12345678901234567890";
            string num2 = "98765432109876543210";
            string expected = "111111111011111111100";

            // Act
            string result = BigNumberLogic.Add(num1, num2);

            // Assert
            Assert.AreEqual(expected, result);
        }

        // Тест 2: Сложение с некорректными данными (Негативный тест)
        [TestMethod]
        public void Add_InvalidData_ThrowsException()
        {
            // Arrange
            string num1 = "123abc";
            string num2 = "456";

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => BigNumberLogic.Add(num1, num2));
        }
    }

    [TestClass]
    public class BigNumberLogicTests_Subtract
    {
        // Тест 3: Вычитание положительных чисел
        [TestMethod]
        public void Subtract_PositiveNumbers_ReturnsCorrectResult()
        {
            // Arrange
            string num1 = "12345678901234567890";
            string num2 = "9876543210987654321";
            string expected = "2469135690246913569";

            // Act
            string result = BigNumberLogic.Subtract(num1, num2);

            // Assert
            Assert.AreEqual(expected, result);
        }

        // Тест 4: Вычитание с некорректными данными (Негативный тест)
        [TestMethod]
        public void Subtract_InvalidData_ThrowsException()
        {
            // Arrange
            string num1 = "abc123";
            string num2 = "456";

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => BigNumberLogic.Subtract(num1, num2));
        }
    }

    [TestClass]
    public class BigNumberLogicTests_Increment
    {
        // Тест 5: Увеличение числа на 1
        [TestMethod]
        public void Increment_PositiveNumber_ReturnsCorrectResult()
        {
            // Arrange
            string num = "12345678901234567890";
            string expected = "12345678901234567891";

            // Act
            string result = BigNumberLogic.Increment(num);

            // Assert
            Assert.AreEqual(expected, result);
        }

        // Тест 6: Увеличение с некорректными данными (Негативный тест)
        [TestMethod]
        public void Increment_InvalidData_ThrowsException()
        {
            // Arrange
            string num = "12.34";

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => BigNumberLogic.Increment(num));
        }
    }

    [TestClass]
    public class BigNumberLogicTests_Decrement
    {
        // Тест 7: Уменьшение числа на 1
        [TestMethod]
        public void Decrement_PositiveNumber_ReturnsCorrectResult()
        {
            // Arrange
            string num = "12345678901234567890";
            string expected = "12345678901234567889";

            // Act
            string result = BigNumberLogic.Decrement(num);

            // Assert
            Assert.AreEqual(expected, result);
        }

        // Тест 8: Уменьшение с некорректными данными (Негативный тест)
        [TestMethod]
        public void Decrement_InvalidData_ThrowsException()
        {
            // Arrange
            string num = "1,234";

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => BigNumberLogic.Decrement(num));
        }
    }
}

