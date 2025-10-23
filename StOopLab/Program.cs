using Newtonsoft.Json;

namespace StOopLab
{
    public class Program
    {
        public static List<WaterVendingMachine> database = new List<WaterVendingMachine>();
        public static string jsonFileName = "json.json";
        public static string csvFileName = "csv.csv";

        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Menu (7)");
                Console.WriteLine(@"
1 – Додати об’єкт
2 – Переглянути всі об’єкти
3 – Знайти об’єкт
4 – Продемонструвати поведінку
5 – Видалити об’єкт
6 – Продемонструвати static-методи
7 – Зберегти колекцію об’єктів у файлі
8 – Зчитати колекцію об’єктів з файлу
9 – Очистити колекцію об’єктів
0 – Вийти з програми");
                Console.WriteLine();
                var menuNumber = ReadUserNumber("Enter menu number: ", 0, 9);
                switch (menuNumber)
                {
                    case 1:
                        AddItem();
                        break;
                    case 2:
                        DisplayAllItems();
                        break;
                    case 3:
                        FindItem();
                        break;
                    case 4:
                        DemoFull();
                        break;
                    case 5:
                        Delete();
                        break;
                    case 6:
                        DemoStatic();
                        break;
                    case 7:
                        SaveToFile();
                        break;
                    case 8:
                        ReadFromFile();
                        break;
                    case 9:
                        ClearAll();
                        break;
                    case 0:
                        Console.WriteLine("You selected Option 0. Exiting the program.");
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please enter a number between 0 and 5.");
                        break;
                }
            }
        }

        public static void AddItem()
        {
            Console.WriteLine("1 – Додати об’єкт");
            Console.WriteLine();
            bool find = true;
            while (find)
            {
                Console.WriteLine("Menu");
                Console.WriteLine(@"
1 – Create default
2 – Manually enter data
3 – Manually enter data partly
4 - Enter entity string
0 – Exit to main menu");
                Console.WriteLine();
                var findNumber = ReadUserNumber("Enter menu number: ", 0, 4);
                switch (findNumber)
                {
                    case 1:
                        // Usage of constructor
                        var machineDefault = new WaterVendingMachine(1000, "1 floor", "Tom", "123456", "Aqua");
                        database.Add(machineDefault);
                        Display(database);
                        break;
                    case 2:
                        CreateItemManually();
                        break;
                    case 3:
                        var capacity = ReadUserNumber("Enter water capacity in liters (number 500 - 2000): ", 500, 2000);
                        // Usage of overloaded constructor
                        var machine1 = new WaterVendingMachine(capacity);
                        database.Add(machine1);
                        Display(database);
                        break;
                    case 4:
                        int maxStringLength = 104;
                        int minStringLength = 20;
                        var instanceString = ReadUserString("Enter instance string in format\n{WaterCapacityLiters}|{Address}|{OperatorName}|{Phone}|{CompanyName}\n: ", minStringLength, maxStringLength);
                        if (WaterVendingMachine.TryParse(instanceString, out var parsedInstance))
                        {
                            database.Add(parsedInstance);
                            Display(database);
                        }
                        else
                        {
                            Console.WriteLine("Incorrect input string");
                        }

                        break;
                    case 0:
                        Console.WriteLine("You selected Option 0. Exiting to main menu.");
                        find = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please enter a number between 0 and 2.");
                        break;
                }
            }
        }

        public static void CreateItemManually()
        {
            var address = ReadUserString("Enter machine address (string length 3 - 20): ", 3, 50);
            // Usage of initializator (with default constructor)
            var machine = new WaterVendingMachine { Address = address };
            SetStringProperty("Enter operator name (string length 3 - 20): ", (string val) => machine.OperatorName = val);
            SetStringProperty("Enter operator phone number (digits length 6): ", (string val) => machine.Phone = val);
            SetStringProperty("Enter operating company name (string length 3 - 20): ", (string val) => machine.CompanyName = val);
            SetNumberProperty("Enter water capacity in liters (number 500 - 2000): ", (int val) => machine.WaterCapacityLiters = val);
            database.Add(machine);
            Display(database);
        }

        public static void DisplayAllItems()
        {
            Console.WriteLine("2 – Переглянути всі об’єкти");
            Display(database);
        }

        public static void FindItem()
        {
            Console.WriteLine("3 – Знайти об’єкт");
            bool find = true;
            while (find)
            {
                Console.WriteLine("Menu");
                Console.WriteLine(@"
1 – Find by company name
2 – Find by operator name
0 – Exit to main menu");
                Console.WriteLine();
                var findNumber = ReadUserNumber("Enter menu number: ", 0, 2);
                switch (findNumber)
                {
                    case 1:
                        var companyName = ReadUserString("Enter operating company name (string length 3 - 20): ", 3, 20);
                        var itemsByName = database.FindAll(i => i.CompanyName == companyName);
                        Display(itemsByName);
                        break;
                    case 2:
                        var operatorName = ReadUserString("Enter operator name (string length 3 - 20): ", 3, 20);
                        var itemsByCompany = database.FindAll(i => i.OperatorName == operatorName);
                        Display(itemsByCompany);
                        break;
                    case 0:
                        Console.WriteLine("You selected Option 0. Exiting to main menu.");
                        find = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please enter a number between 0 and 2.");
                        break;
                }
            }
        }

        public static void DemoFull()
        {
            Console.WriteLine("4 – Продемонструвати поведінку");
            if (database.Count == 0)
            {
                Console.WriteLine("Create at least 1 machine");
                return;
            }

            var idx = ReadUserNumber("Enter machine index: ", 0, database.Count - 1);

            var machine = database[idx];
            bool work = true;
            while (work)
            {
                Console.WriteLine("Menu");
                Console.WriteLine(@"
1 – Refill machine
2 – Put money
3 - Take water
4 - Withdraw cache
5 - Move
6 - How many water sold (calculated property example)
7 - Refill overload
0 – Exit to main menu");
                Console.WriteLine();
                var findNumber = ReadUserNumber("Enter menu number: ", 0, 7);
                try
                {
                    switch (findNumber)
                    {
                        case 1:
                            if (machine.WaterLeftLiters == machine.WaterCapacityLiters)
                            {
                                Console.WriteLine("Machine is full");
                            }
                            else
                            {
                                Console.WriteLine(machine.Refill());
                            }
                            break;
                        case 2:
                            if (machine.State == MachineState.RequiresMoneyWithraw)
                            {
                                Console.WriteLine("Macine can't take cash");
                            }
                            else if (machine.State == MachineState.RequiresRefill)
                            {
                                Console.WriteLine("No water");
                            }
                            else
                            {
                                var money = ReadUserDecimal("Enter cash: ", 1, machine.GetMoneyCapacity());
                                Console.WriteLine(machine.PutMoney(money));
                            }
                            break;
                        case 3:
                            if (machine.State == MachineState.RequiresRefill)
                            {
                                Console.WriteLine("No water");
                            }
                            else
                            {
                                var waterGot = machine.TakeWater();
                                Console.WriteLine($"You got {waterGot} liters of water");
                            }
                            break;
                        case 4:
                            var cash = machine.WithdrawCash();
                            Console.WriteLine($"We earned {cash} $");
                            break;
                        case 5:
                            var address = ReadUserString("Enter new address: ", 3, 50);
                            var moveResult = machine.Move(address);
                            Console.WriteLine(moveResult);
                            break;
                        case 6:
                            Console.WriteLine(machine.WaterSoldLiters);
                            break;
                        case 7:
                            if (machine.WaterLeftLiters == machine.WaterCapacityLiters)
                            {
                                Console.WriteLine("Machine is full");
                            }
                            else
                            {
                                var water = ReadUserFloat("Enter water amount: ", 1, machine.WaterSoldLiters);
                                Console.WriteLine(machine.Refill(water));
                            }

                            break;
                        case 0:
                            Console.WriteLine("You selected Option 0. Exiting to main menu.");
                            work = false;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please enter a number between 0 and 2.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public static void Delete()
        {
            Console.WriteLine("5 – Видалити об’єкт");
            if (database.Count == 0)
            {
                Console.WriteLine("Create at least 1 machine");
                return;
            }

            var index = ReadUserNumber("Enter item index to delete: ", 0, database.Count - 1);
            database.RemoveAt(index);
            WaterVendingMachine.Count--;
            Console.WriteLine($"Element {index} removed");
            Display(database);
        }

        public static void Display(List<WaterVendingMachine> items)
        {
            if (items.Count == 0)
            {
                Console.WriteLine("Database is empty.");
                return;
            }
            var headers = new List<string> { "Index", "CompanyName", "OperatorName", "Phone", "Address" };
            var lines = new List<List<string>>();
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                lines.Add(new List<string> { i.ToString(), item.CompanyName, item.OperatorName, item.Phone, item.Address });
            }

            var widths = headers.Select(h => h.Length).ToList();
            for (int i = 0; i < widths.Count; i++)
            {
                var itemColumnWidth = lines.Max(line => line[i].Length);
                if (itemColumnWidth > widths[i])
                {
                    widths[i] = itemColumnWidth;
                }
            }

            var headerLine = string.Join(" | ", headers.Select((h, i) => h.PadRight(widths[i])));
            var separator = new string('-', headerLine.Length);

            Console.WriteLine(separator);
            Console.WriteLine(headerLine);
            Console.WriteLine(separator);

            foreach (var line in lines)
            {
                var row = string.Join(" | ", line.Select((i, idx) => i.PadRight(widths[idx])));
                Console.WriteLine(row);
            }

            Console.WriteLine(separator);
            Console.WriteLine("Total count of created items: " + WaterVendingMachine.Count);
            Console.WriteLine(separator);
        }

        public static void DemoStatic()
        {
            Console.WriteLine("6 – Продемонструвати static-методи");
            bool work = true;
            while (work)
            {
                Console.WriteLine("Menu");
                Console.WriteLine(@"
1 – Calculate amount of water by cash and price
2 – Parse instance string
0 – Exit to main menu");
                Console.WriteLine();
                var findNumber = ReadUserNumber("Enter menu number: ", 0, 2);
                try
                {
                    switch (findNumber)
                    {
                        case 1:
                            Console.WriteLine("Current price: " + WaterVendingMachine.WaterPrice);
                            var price = ReadUserDecimal("Enter new price or 0 to keep previos price (0, 500): ", 0, 500);
                            if (price > 0)
                            {
                                WaterVendingMachine.WaterPrice = price;
                            }

                            var cash = ReadUserDecimal("Enter cash (1, 100): ", 1, 1000);
                            var water = WaterVendingMachine.CalculateWaterByPrice(cash);
                            Console.WriteLine($"For {cash} $ you can get {water} liters of water");
                            break;
                        case 2:
                            while (true)
                            {
                                try
                                {
                                    int maxStringLength = 104;
                                    var instanceString = ReadUserString("Enter instance string in format\n{WaterCapacityLiters}|{Address}|{OperatorName}|{Phone}|{CompanyName}\n: ", 1, maxStringLength);
                                    if (instanceString == "0")
                                    {
                                        break;

                                    }
                                    var item = WaterVendingMachine.Parse(instanceString);
                                    database.Add(item);
                                    Console.WriteLine("Instance created: " + item);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            break;
                        case 0:
                            Console.WriteLine("You selected Option 0. Exiting to main menu.");
                            work = false;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please enter a number between 0 and 2.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public static void SaveToFile()
        {
            Console.WriteLine("7 – Зберегти колекцію об’єктів у файлі");
            bool work = true;
            while (work)
            {
                Console.WriteLine("Menu");
                Console.WriteLine(@"
1 – зберегти у файл *.csv
2 – зберегти у файл *.json
0 – Exit to main menu");
                Console.WriteLine();
                var findNumber = ReadUserNumber("Enter menu number: ", 0, 2);
                try
                {
                    switch (findNumber)
                    {
                        case 1:
                            SaveToCsvFile();
                            break;
                        case 2:
                            SaveToJsonFile();
                            break;
                        case 0:
                            Console.WriteLine("You selected Option 0. Exiting to main menu.");
                            work = false;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please enter a number between 0 and 2.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public static void SaveToCsvFile()
        {
            try
            {
                var textToSave = "";
                foreach (var item in database)
                {
                    textToSave += item + Environment.NewLine;
                }

                File.WriteAllText(csvFileName, textToSave);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving file to csv: " + e.Message);
            }
        }

        public static void SaveToJsonFile()
        {
            try
            {
                var json = JsonConvert.SerializeObject(database);
                File.WriteAllText(jsonFileName, json);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving file to json: " + e.Message);
            }
        }

        public static void ReadFromFile()
        {
            Console.WriteLine("8 – Зчитати колекцію об’єктів з файлу");
            bool work = true;
            while (work)
            {
                Console.WriteLine("Menu");
                Console.WriteLine(@"
1 – зчитати з файлу *.csv
2 – зчитати з файлу *.json
0 – Exit to main menu");
                Console.WriteLine();
                var findNumber = ReadUserNumber("Enter menu number: ", 0, 2);
                try
                {
                    switch (findNumber)
                    {
                        case 1:
                            ReadFromCsvFile();
                            break;
                        case 2:
                            ReadFromJsonFile();
                            break;
                        case 0:
                            Console.WriteLine("You selected Option 0. Exiting to main menu.");
                            work = false;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please enter a number between 0 and 2.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public static void ReadFromCsvFile()
        {
            try
            {
                var lines = File.ReadAllLines(csvFileName);
                foreach (var line in lines)
                {
                    if (WaterVendingMachine.TryParse(line, out var machine))
                    {
                        database.Add(machine);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading csv file: " + e.Message);
            }
        }

        public static void ReadFromJsonFile()
        {
            try
            {
                var json = File.ReadAllText(jsonFileName);
                var list = JsonConvert.DeserializeObject<List<WaterVendingMachine>>(json);
                if (list != null)
                {
                    database.AddRange(list);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading json file: " + e.Message);
            }
        }

        public static void ClearAll()
        {
            Console.WriteLine("9 – Очистити колекцію об’єктів");
            var count = database.Count;
            database.Clear();
            Console.WriteLine($"Removed {count} elements");
        }

        public static string ReadUserDigits(string message, int from, int to)
        {
            while (true)
            {
                Console.Write(message);
                var value = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine("Value is empty");
                }
                else if (value.Length < from || value.Length > to)
                {
                    Console.WriteLine("Value is out of range");
                }
                else if (int.TryParse(value, out int number))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine("Invalid format");
                }
            }
        }

        public static int ReadUserNumber(string message, int from, int to)
        {
            while (true)
            {
                Console.Write(message);
                var value = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine("Value is empty");
                }
                else if (int.TryParse(value, out int number))
                {
                    if (number < from || number > to)
                    {
                        Console.WriteLine("Value is out of range");
                    }
                    else
                    {
                        return number;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid format");
                }
            }
        }

        public static decimal ReadUserDecimal(string message, decimal from, decimal to)
        {
            while (true)
            {
                Console.Write(message);
                var value = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine("Value is empty");
                }
                else if (decimal.TryParse(value, out decimal number))
                {
                    if (number < from || number > to)
                    {
                        Console.WriteLine("Value is out of range");
                    }
                    else
                    {
                        return number;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid format");
                }
            }
        }

        public static float ReadUserFloat(string message, float from, float to)
        {
            while (true)
            {
                Console.Write(message);
                var value = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine("Value is empty");
                }
                else if (float.TryParse(value, out float number))
                {
                    if (number < from || number > to)
                    {
                        Console.WriteLine("Value is out of range");
                    }
                    else
                    {
                        return number;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid format");
                }
            }
        }

        public static string ReadUserString(string message, int from, int to)
        {
            while (true)
            {
                Console.Write(message);
                var value = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine("Value is empty");
                }
                else if (value.Length < from || value.Length > to)
                {
                    Console.WriteLine("Value is out of range");
                }
                else
                {
                    return value;
                }
            }
        }

        public static void SetStringProperty(string message, Action<string> seter)
        {
            while (true)
            {
                try
                {
                    Console.Write(message);
                    var value = Console.ReadLine();
                    seter(value);
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void SetNumberProperty(string message, Action<int> seter)
        {
            while (true)
            {
                try
                {
                    Console.Write(message);
                    var value = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        Console.WriteLine("Value is empty");
                    }
                    else if (int.TryParse(value, out int number))
                    {
                        seter(number);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid format");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}