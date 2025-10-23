using System.Globalization;

namespace StOopLab
{
    public class WaterVendingMachine
    {
        private static int count;

        public static int Count
        {
            get => count;
            set => count = value;
        }

        private static decimal waterPrice = 1;

        public static decimal WaterPrice
        {
            get => waterPrice;
            set => waterPrice = value;
        }

        public static string Delimiter = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        private readonly decimal cashCapacity = 1000;
        private decimal cashAmount;
        private decimal cashDeposit;
        private int waterCapacityLiters;
        private DateTime refillDate;
        private MachineState state = MachineState.RequiresRefill;
        private float waterLeftLiters;
        private string? operatorName;
        private string? phone;
        private string? companyName;

        public static float CalculateWaterByPrice(decimal cache)
        {
            return (float)(cache / waterPrice);
        }

        public static WaterVendingMachine Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentException("Input string is empty");
            }

            var props = s.Split(Delimiter);
            if (props.Length < 5)
            {
                throw new ArgumentException("Insufficient number of parameters");
            }

            var capacityStr = props[0];
            if (!int.TryParse(capacityStr, out int capacity))
            {
                throw new ArgumentException($"WaterCapacityLiters value is not in valid format");
            }

            try
            {
                var result = new WaterVendingMachine
                {
                    WaterCapacityLiters = capacity,
                    Address = props[1],
                    OperatorName = props[2],
                    Phone = props[3],
                    CompanyName = props[4],
                };

                return result;
            }
            catch (Exception e)
            {
                count--;
                throw;
            }
        }

        public static bool TryParse(string s, out WaterVendingMachine? machine)
        {
            try
            {
                machine = Parse(s);
                return true;
            }
            catch
            {
                machine = null;
                return false;
            }
        }

        public WaterVendingMachine()
        {
            // increase number of valid instances
            count++;
        }

        public WaterVendingMachine(int waterCapacity) : this(waterCapacity, "1 floor", "Tom", "000000", "water")
        {
        }

        public WaterVendingMachine(int waterCapacity, string address, string operatorName, string phone, string companyName) : this()
        {
            this.waterCapacityLiters = waterCapacity;
            this.Address = address;
            this.operatorName = operatorName;
            this.phone = phone;
            this.companyName = companyName;
        }

        public int WaterCapacityLiters
        {
            get => waterCapacityLiters;
            set
            {
                if (value < 500 || value > 2000)
                {
                    throw new ArgumentException("Water capacity Value is out of range.");
                }

                waterCapacityLiters = value;
            }
        }

        public DateTime RefillDate
        {
            get => refillDate;
            private set => refillDate = value;
        }

        public MachineState State
        {
            get => state;
            set => state = value;
        }

        public float WaterLeftLiters
        {
            get => waterLeftLiters;
            private set => waterLeftLiters = value;
        }

        public float WaterSoldLiters => this.waterCapacityLiters - this.waterLeftLiters;

        public string? Address { get; set; }

        public string? OperatorName
        {
            get => operatorName;
            set
            {
                ValidateValue(value, 3, 20, "Operator Name");
                operatorName = value;
            }
        }

        public string? Phone
        {
            get => phone;
            set
            {
                ValidateValue(value, 6, 6, "Phone");
                if (!int.TryParse(value, out _))
                {
                    throw new ArgumentException("Phone value has invalid format");
                }

                phone = value;
            }
        }

        public string? CompanyName
        {
            get => companyName;
            set
            {
                ValidateValue(value, 3, 20, "Company Name");
                companyName = value;
            }
        }

        public string PutMoney(decimal cash)
        {
            if (this.state == MachineState.RequiresMoneyWithraw)
            {
                return "Machine can't take money";
            }

            this.cashDeposit += cash;
            this.cashAmount += cash;
            if (this.cashAmount >= this.cashCapacity)
            {
                this.state = MachineState.RequiresMoneyWithraw;
            }

            return $"You put {cash} $";
        }

        public float TakeWater()
        {
            if (this.cashDeposit <= 0)
            {
                return 0;
            }

            var volume = CalculateWaterByPrice(this.cashDeposit);
            if (volume >= this.waterLeftLiters)
            {
                state = MachineState.RequiresRefill;
                return waterLeftLiters;
            }

            waterLeftLiters -= volume;
            return volume;
        }

        public string Refill(float liters)
        {
            if (liters + waterLeftLiters > waterCapacityLiters)
            {
                throw new ArgumentException("Too much water");
            }

            waterLeftLiters += liters;
            state = MachineState.Active;
            refillDate = DateTime.Now;
            return $"Machine refilled with {liters} liters";
        }

        public string Refill()
        {
            return this.Refill(waterCapacityLiters - waterLeftLiters);
        }

        public decimal WithdrawCash()
        {
            var cash = cashAmount;
            cashAmount = 0;
            state = MachineState.Active;
            return cash;
        }

        public string Move(string newAddress)
        {
            Address = newAddress;
            return $"New address - {newAddress}";
        }

        public decimal GetMoneyCapacity()
        {
            return cashCapacity;
        }

        public override string ToString()
        {
            return $"{this.WaterCapacityLiters}{Delimiter}{this.Address}{Delimiter}{this.OperatorName}{Delimiter}{this.Phone}{Delimiter}{this.CompanyName}";
        }

        private void ValidateValue(string? value, int from, int to, string propName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"{propName} value is empty");
            }
            else if (value.Length < from || value.Length > to)
            {
                throw new ArgumentException($"{propName} value is out of range");
            }
            else if (value.Contains(Delimiter))
            {
                throw new ArgumentException($"{propName} must not contain {Delimiter}");
            }
        }
    }
}
