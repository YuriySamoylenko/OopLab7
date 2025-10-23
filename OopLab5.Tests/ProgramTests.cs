using Newtonsoft.Json;
using StOopLab;

namespace OopLab5.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Reset global state before each test
            Program.database.Clear();
            WaterVendingMachine.Count = 0;
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Delete temporary files
            if (File.Exists(Program.csvFileName))
            {
                File.Delete(Program.csvFileName);
            }

            if (File.Exists(Program.jsonFileName))
            {
                File.Delete(Program.jsonFileName);
            }
        }

        [TestMethod]
        public void CreateItemManually_ValidInput_AddsToDatabase()
        {
            // Arrange
            var address = "1 floor";
            var name = "Tom";
            var phone = "123456";
            var comp = "Aqua";
            var capacity = 1000;
            var input = new StringReader($"{address}\n{name}\n{phone}\n{comp}\n{capacity}\n");
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            // Act
            Program.CreateItemManually();

            // Assert
            Assert.AreEqual(1, Program.database.Count);
            var machine = Program.database[0];
            Assert.AreEqual(address, machine.Address);
            Assert.AreEqual(name, machine.OperatorName);
            Assert.AreEqual(phone, machine.Phone);
            Assert.AreEqual(comp, machine.CompanyName);
            Assert.AreEqual(capacity, machine.WaterCapacityLiters);
            Assert.AreEqual(1, WaterVendingMachine.Count);
        }

        [TestMethod]
        public void CreateItemManually_InvalidAddress_RetriesUntilValid()
        {
            // Arrange
            var address = "1 floor";
            var input = new StringReader("\n" + // Empty address
                                         "ab\n" + // Too short
                                         $"{address}\n" + // Valid address
                                         "Tom\n123456\nAqua\n1000\n");
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            // Act
            Program.CreateItemManually();

            // Assert
            Assert.AreEqual(1, Program.database.Count);
            var machine = Program.database[0];
            Assert.AreEqual(address, machine.Address);
            StringAssert.Contains(output.ToString(), "Value is empty");
            StringAssert.Contains(output.ToString(), "Value is out of range");
        }

        [TestMethod]
        public void CreateItemManually_InvalidCapacity_RetriesUntilValid()
        {
            // Arrange
            var input = new StringReader("1 floor\nTom\n123456\nAqua\n" +
                                        "abc\n" + // Invalid number
                                        "400\n" + // Out of range
                                        "1000\n"); // Valid capacity
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            // Act
            Program.CreateItemManually();

            // Assert
            Assert.AreEqual(1, Program.database.Count);
            var machine = Program.database[0];
            Assert.AreEqual(1000, machine.WaterCapacityLiters);
            StringAssert.Contains(output.ToString(), "Invalid format");
            StringAssert.Contains(output.ToString(), "Value is out of range");
        }

        [TestMethod]
        public void CreateItemManually_Name_RetriesUntilValid()
        {
            // Arrange
            var input = new StringReader("1 floor" +
                                        "\nxx" + // Out of range
                                        "\nTom" + // Valid name
                                        "\n123456\nAqua\n1000\n");
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            // Act
            Program.CreateItemManually();

            // Assert
            Assert.AreEqual(1, Program.database.Count);
            StringAssert.Contains(output.ToString(), "Operator Name value is out of range");
        }

        [TestMethod]
        public void Delete_NonEmptyDatabase_ValidIndex_RemovesItem()
        {
            // Arrange
            Program.database.Add(new WaterVendingMachine { Address = "1 floor", OperatorName = "Tom", Phone = "123456", CompanyName = "Aqua", WaterCapacityLiters = 1000 });
            Program.database.Add(new WaterVendingMachine { Address = "7 floor", OperatorName = "Joe", Phone = "654321", CompanyName = "Water", WaterCapacityLiters = 1500 });
            WaterVendingMachine.Count = 2;
            var input = new StringReader("1\n"); // Delete index 1
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            // Act
            Program.Delete();

            // Assert
            Assert.AreEqual(1, Program.database.Count);
            Assert.AreEqual("1 floor", Program.database[0].Address);
            Assert.AreEqual(1, WaterVendingMachine.Count);
            var outputString = output.ToString();
            StringAssert.Contains(outputString, "5 – Видалити об’єкт");
            StringAssert.Contains(outputString, "Enter item index to delete: ");
            StringAssert.Contains(outputString, "Element 1 removed");
        }

        [TestMethod]
        public void Delete_EmptyDatabase_PrintsEmptyMessage()
        {
            // Arrange
            var input = new StringReader("");
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            // Act
            Program.Delete();

            // Assert
            Assert.AreEqual(0, Program.database.Count, "Database should remain empty.");
            var outputString = output.ToString();
            StringAssert.Contains(outputString, "5 – Видалити об’єкт");
            StringAssert.Contains(outputString, "Create at least 1 machine");
        }

        [TestMethod]
        public void Delete_InvalidIndexInput_RetriesUntilValid()
        {
            // Arrange
            Program.database.Add(new WaterVendingMachine { Address = "1 floor", OperatorName = "Tom", Phone = "123456", CompanyName = "Aqua", WaterCapacityLiters = 1000 });
            WaterVendingMachine.Count = 1;
            var input = new StringReader("abc\n8\n0\n"); // Invalid, out-of-range, valid
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            // Act
            Program.Delete();

            // Assert
            Assert.AreEqual(0, Program.database.Count);
            Assert.AreEqual(0, WaterVendingMachine.Count);
            var outputString = output.ToString();
            StringAssert.Contains(outputString, "Invalid format");
            StringAssert.Contains(outputString, "Value is out of range");
            StringAssert.Contains(outputString, "Element 0 removed");
            StringAssert.Contains(outputString, "Database is empty.");
        }

        [TestMethod]
        public void SaveToCsvFile_NonEmptyDatabase_WritesCorrectCsv()
        {
            // Arrange
            Program.database.Add(new WaterVendingMachine { WaterCapacityLiters = 1000, Address = "1 floor", OperatorName = "Tom", Phone = "123456", CompanyName = "Aqua" });
            Program.database.Add(new WaterVendingMachine { WaterCapacityLiters = 1500, Address = "7 floor", OperatorName = "Joe", Phone = "654321", CompanyName = "Water" });
            WaterVendingMachine.Count = 2;
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.SaveToCsvFile();

            // Assert
            var csvContent = File.ReadAllText(Program.csvFileName);
            var expectedCsv = $"{Program.database[0].ToString()}{Environment.NewLine}{Program.database[1].ToString()}{Environment.NewLine}";
            Assert.AreEqual(expectedCsv, csvContent);
        }

        [TestMethod]
        public void SaveToCsvFile_EmptyDatabase_WritesEmptyFile()
        {
            // Arrange
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.SaveToCsvFile();

            // Assert
            var csvContent = File.ReadAllText(Program.csvFileName);
            Assert.AreEqual("", csvContent);
        }

        [TestMethod]
        public void SaveToJsonFile_NonEmptyDatabase_WritesCorrectJson()
        {
            // Arrange
            Program.database.Add(new WaterVendingMachine { WaterCapacityLiters = 1000, Address = "1 floor", OperatorName = "Tom", Phone = "123456", CompanyName = "Aqua" });
            WaterVendingMachine.Count = 1;
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.SaveToJsonFile();

            // Assert
            var jsonContent = File.ReadAllText(Program.jsonFileName);
            var expectedJson = JsonConvert.SerializeObject(Program.database);
            Assert.AreEqual(expectedJson, jsonContent);
        }

        [TestMethod]
        public void SaveToJsonFile_EmptyDatabase_WritesEmptyList()
        {
            // Arrange
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.SaveToJsonFile();

            // Assert
            var jsonContent = File.ReadAllText(Program.jsonFileName);
            Assert.AreEqual("[]", jsonContent);
        }

        [TestMethod]
        public void ReadFromCsvFile_ValidCsv_PopulatesDatabase()
        {
            // Arrange
            var csvContent = "1000,1 floor,Tom,123456,Aqua\n1500,7 floor,Joe,654321,Water";
            File.WriteAllText(Program.csvFileName, csvContent);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.ReadFromCsvFile();

            // Assert
            Assert.AreEqual(2, Program.database.Count);
            Assert.AreEqual("1 floor", Program.database[0].Address);
            Assert.AreEqual("7 floor", Program.database[1].Address);
        }

        [TestMethod]
        public void ReadFromCsvFile_InvalidCsvLine_SkipsInvalid()
        {
            // Arrange
            var csvContent = "1000,1 floor,Tom,123456,Aqua\ninvalid,7 floor,Joe,654321,Water";
            File.WriteAllText(Program.csvFileName, csvContent);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.ReadFromCsvFile();

            // Assert
            Assert.AreEqual(1, Program.database.Count);
            Assert.AreEqual("1 floor", Program.database[0].Address);
            Assert.IsFalse(output.ToString().Contains("Error reading csv file"));
        }

        [TestMethod]
        public void ReadFromJsonFile_ValidJson_PopulatesDatabase()
        {
            // Arrange
            var machines = new List<WaterVendingMachine>
            {
                new WaterVendingMachine { WaterCapacityLiters = 1000, Address = "1 floor", OperatorName = "Tom", Phone = "123456", CompanyName = "Aqua" },
                new WaterVendingMachine { WaterCapacityLiters = 1500, Address = "7 floor", OperatorName = "Joe", Phone = "654321", CompanyName = "Water" }
            };
            var json = JsonConvert.SerializeObject(machines);
            File.WriteAllText(Program.jsonFileName, json);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.ReadFromJsonFile();

            // Assert
            Assert.AreEqual(2, Program.database.Count);
            Assert.AreEqual("1 floor", Program.database[0].Address);
            Assert.AreEqual("7 floor", Program.database[1].Address);
        }

        [TestMethod]
        public void ClearAll_NonEmptyDatabase_ClearsAll()
        {
            // Arrange
            Program.database.Add(new WaterVendingMachine { WaterCapacityLiters = 1000, Address = "1 floor", OperatorName = "Tom", Phone = "123456", CompanyName = "Aqua" });
            Program.database.Add(new WaterVendingMachine { WaterCapacityLiters = 1500, Address = "7 floor", OperatorName = "Joe", Phone = "654321", CompanyName = "Water" });
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.ClearAll();

            // Assert
            Assert.AreEqual(0, Program.database.Count);
        }

        [TestMethod]
        public void ClearAll_EmptyDatabase_StaysEmpty()
        {
            // Arrange
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.ClearAll();

            // Assert
            Assert.AreEqual(0, Program.database.Count);
            var outputString = output.ToString();
        }
    }
}