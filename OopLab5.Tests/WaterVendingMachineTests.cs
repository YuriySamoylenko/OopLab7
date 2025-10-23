using StOopLab;

namespace OopLab5.Tests
{
    [TestClass]
    public class WaterVendingMachineTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Reset static fields before each test
            WaterVendingMachine.Count = 0;
            WaterVendingMachine.WaterPrice = 1;
        }

        [TestMethod]
        public void CalculateWaterByPrice_ValidInput_ReturnsCorrectVolume()
        {
            // Arrange
            WaterVendingMachine.WaterPrice = 2;
            decimal cash = 10;

            // Act
            float result = WaterVendingMachine.CalculateWaterByPrice(cash);

            // Assert
            Assert.AreEqual(5.0f, result);
        }

        [TestMethod]
        public void Parse_ValidInput_ReturnsCorrectInstance()
        {
            // Arrange
            string input = "1000,Main Street,Tom,123456,Aqua";

            // Act
            var machine = WaterVendingMachine.Parse(input);

            // Assert
            Assert.AreEqual(1000, machine.WaterCapacityLiters);
            Assert.AreEqual("Main Street", machine.Address);
            Assert.AreEqual("Tom", machine.OperatorName);
            Assert.AreEqual("123456", machine.Phone);
            Assert.AreEqual("Aqua", machine.CompanyName);
            Assert.AreEqual(1, WaterVendingMachine.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_EmptyInput_ThrowsArgumentException()
        {
            // Act
            WaterVendingMachine.Parse("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_InsufficientParameters_ThrowsArgumentException()
        {
            // Arrange
            var input = "1000,Main Street";

            // Act
            WaterVendingMachine.Parse(input);
        }

        [TestMethod]
        public void Parse_InvalidCapacityFormat_ThrowsArgumentException()
        {
            // Arrange
            var input = "invalid,Main Street,Tom,123456,Aqua";

            // Act/Assert
            Assert.ThrowsException<ArgumentException>(() => WaterVendingMachine.Parse(input));
        }

        [TestMethod]
        public void TryParse_ValidInput_ReturnsTrueAndInstance()
        {
            // Arrange
            string input = "1000,Main Street,Tom,123456,Aqua";

            // Act
            bool result = WaterVendingMachine.TryParse(input, out var machine);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(machine);
            Assert.AreEqual(1000, machine.WaterCapacityLiters);
            Assert.AreEqual("Main Street", machine.Address);
        }

        [TestMethod]
        public void TryParse_InvalidInput_ReturnsFalseAndNull()
        {
            // Arrange
            string input = "invalid,Main Street,Tom,123456,Aqua";

            // Act
            bool result = WaterVendingMachine.TryParse(input, out var machine);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(machine);
        }

        [TestMethod]
        public void WaterCapacityLiters_SetValidValue_WorksCorrectly()
        {
            // Arrange
            var machine = new WaterVendingMachine();

            // Act
            machine.WaterCapacityLiters = 1000;

            // Assert
            Assert.AreEqual(1000, machine.WaterCapacityLiters);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WaterCapacityLiters_SetBelowRange_ThrowsArgumentException()
        {
            // Arrange
            var machine = new WaterVendingMachine();

            // Act
            machine.WaterCapacityLiters = 499;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WaterCapacityLiters_SetAboveRange_ThrowsArgumentException()
        {
            // Arrange
            var machine = new WaterVendingMachine();

            // Act
            machine.WaterCapacityLiters = 2001;
        }

        [TestMethod]
        public void Address_SetValidValue_WorksCorrectly()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            var value = "New Address";

            // Act
            machine.Address = value;

            // Assert
            Assert.AreEqual(value, machine.Address);
        }

        [TestMethod]
        public void OperatorName_SetValidValue_WorksCorrectly()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            var value = "John";

            // Act
            machine.OperatorName = value;

            // Assert
            Assert.AreEqual(value, machine.OperatorName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OperatorName_SetEmpty_ThrowsArgumentException()
        {
            // Arrange
            var machine = new WaterVendingMachine();

            // Act
            machine.OperatorName = "";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OperatorName_SetTooShort_ThrowsArgumentException()
        {
            // Arrange
            var machine = new WaterVendingMachine();

            // Act
            machine.OperatorName = "Jo";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OperatorName_SetContainsDelimiter_ThrowsArgumentException()
        {
            // Arrange
            var machine = new WaterVendingMachine();

            // Act
            machine.OperatorName = $"John{WaterVendingMachine.Delimiter}Smith";
        }

        [TestMethod]
        public void Phone_SetValidValue_WorksCorrectly()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            var value = "123456";

            // Act
            machine.Phone = value;

            // Assert
            Assert.AreEqual(value, machine.Phone);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Phone_SetInvalidFormat_ThrowsArgumentException()
        {
            // Arrange
            var machine = new WaterVendingMachine();

            // Act
            machine.Phone = "abc123";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Phone_SetWrongLength_ThrowsArgumentException()
        {
            // Arrange
            var machine = new WaterVendingMachine();

            // Act
            machine.Phone = "12345";
        }

        [TestMethod]
        public void CompanyName_SetValidValue_WorksCorrectly()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            var value = "Aqua";

            // Act
            machine.CompanyName = value;

            // Assert
            Assert.AreEqual(value, machine.CompanyName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompanyName_SetEmpty_ThrowsArgumentException()
        {
            // Arrange
            var machine = new WaterVendingMachine();

            // Act
            machine.CompanyName = "";
        }

        [TestMethod]
        public void PutMoney_ValidInput_UpdatesCashAndReturnsMessage()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            machine.State = MachineState.Active;

            // Act
            var result = machine.PutMoney(10);

            // Assert
            Assert.AreEqual("You put 10 $", result);
            Assert.AreEqual(10m, machine.GetMoneyCapacity() - 990); // Indirectly checking cashAmount
        }

        [TestMethod]
        public void PutMoney_RequiresMoneyWithdraw_ReturnsErrorMessage()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            machine.State = MachineState.RequiresMoneyWithraw;

            // Act
            var result = machine.PutMoney(10);

            // Assert
            Assert.AreEqual("Machine can't take money", result);
        }

        [TestMethod]
        public void PutMoney_ExceedsCapacity_SetsRequiresMoneyWithdraw()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            machine.State = MachineState.Active;

            // Act
            machine.PutMoney(1000);

            // Assert
            Assert.AreEqual(MachineState.RequiresMoneyWithraw, machine.State);
        }

        [TestMethod]
        public void TakeWater_NoCashDeposit_ReturnsZero()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            machine.State = MachineState.Active;

            // Act
            var result = machine.TakeWater();

            // Assert
            Assert.AreEqual(0f, result);
        }

        [TestMethod]
        public void TakeWater_ValidCashDeposit_ReturnsCorrectVolume()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            machine.State = MachineState.Active;
            machine.WaterCapacityLiters = 1000;
            machine.Refill(100);
            machine.PutMoney(10);
            WaterVendingMachine.WaterPrice = 2;

            // Act
            var result = machine.TakeWater();

            // Assert
            Assert.AreEqual(5f, result);
            Assert.AreEqual(95f, machine.WaterLeftLiters);
        }

        [TestMethod]
        public void TakeWater_InsufficientWater_SetsRequiresRefill()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            machine.State = MachineState.Active;
            machine.WaterCapacityLiters = 1000;
            machine.Refill(2);
            machine.PutMoney(20);
            WaterVendingMachine.WaterPrice = 5;

            // Act
            var result = machine.TakeWater();

            // Assert
            Assert.AreEqual(2f, result);
            Assert.AreEqual(MachineState.RequiresRefill, machine.State);
        }

        [TestMethod]
        public void Refill_WithLiters_ValidInput_UpdatesStateAndReturnsMessage()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            machine.WaterCapacityLiters = 1000;

            // Act
            var result = machine.Refill(500);

            // Assert
            Assert.AreEqual("Machine refilled with 500 liters", result);
            Assert.AreEqual(500f, machine.WaterLeftLiters);
            Assert.AreEqual(MachineState.Active, machine.State);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Refill_WithLiters_ExceedsCapacity_ThrowsArgumentException()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            machine.WaterCapacityLiters = 1000;

            // Act
            machine.Refill(1100);
        }

        [TestMethod]
        public void Refill_NoParameters_FillsToCapacity()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            machine.WaterCapacityLiters = 1000;

            // Act
            var result = machine.Refill();

            // Assert
            Assert.AreEqual("Machine refilled with 1000 liters", result);
            Assert.AreEqual(1000f, machine.WaterLeftLiters);
            Assert.AreEqual(MachineState.Active, machine.State);
        }

        [TestMethod]
        public void WithdrawCash_ReturnsAndResetsCashAmount()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            var value = 100m;
            machine.PutMoney(value);

            // Act
            var result = machine.WithdrawCash();

            // Assert
            Assert.AreEqual(value, result);
            Assert.AreEqual(0m, machine.GetMoneyCapacity() - 1000); // Indirectly checking cashAmount
            Assert.AreEqual(MachineState.Active, machine.State);
        }

        [TestMethod]
        public void Move_SetsNewAddressAndReturnsMessage()
        {
            // Arrange
            var machine = new WaterVendingMachine();
            var value = "New Street";

            // Act
            var result = machine.Move(value);

            // Assert
            Assert.AreEqual("New address - New Street", result);
            Assert.AreEqual(value, machine.Address);
        }

        [TestMethod]
        public void GetMoneyCapacity_ReturnsCorrectCapacity()
        {
            // Arrange
            var machine = new WaterVendingMachine();

            // Act
            var result = machine.GetMoneyCapacity();

            // Assert
            Assert.AreEqual(1000m, result);
        }

        [TestMethod]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var machine = new WaterVendingMachine(1000, "1 floor", "Tom", "123456", "Aqua");

            // Act
            var result = machine.ToString();

            // Assert
            Assert.AreEqual($"1000{WaterVendingMachine.Delimiter}1 floor{WaterVendingMachine.Delimiter}Tom{WaterVendingMachine.Delimiter}123456{WaterVendingMachine.Delimiter}Aqua", result);
        }
    }
}
