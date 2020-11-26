using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace CmsStrategyDemo
{
    interface IRepository
    {
        List<Person> GetAll();
    }


    class DBRepo : IRepository
    {
        public DBRepo(string conn)
        {
            
        }
        public List<Person> GetAll()
        {
            throw new NotImplementedException();
        }
    }


    class FileRepo : IRepository
    {
        public FileRepo(string path)
        {

        }

        public List<Person> GetAll()
        {
            throw new NotImplementedException();
        }
    }

    interface ITaxCalculationStrategy
    {
        decimal CalculateTax(int Salary);
    }


    class SwedenTaxCalculation : ITaxCalculationStrategy
    {
        public decimal CalculateTax(int Salary)
        {
            if (Salary > 50000)
                return Salary * 0.6m;
            if (Salary > 40000)
                return Salary * 0.5m;
            if (Salary > 30000)
                return Salary * 0.2m;
            return Salary * 0.1m;
        }
    }

    class NorwayTaxCalculation : ITaxCalculationStrategy
    {
        private readonly Yrke _yrke;
        private readonly string _namn;

        public NorwayTaxCalculation(Yrke yrke, string namn)
        {
            _yrke = yrke;
            _namn = namn;
        }    
        public decimal CalculateTax(int Salary)
        {
            if(_namn == "Ole")
                return Salary * 0.01m;

            if (_yrke == Yrke.OilWorker)
                return Salary * 0.08m;

            if (Salary > 50000)
                return Salary * 0.4m;
            if (Salary > 40000)
                return Salary * 0.3m;
            if (Salary > 30000)
                return Salary * 0.2m;
            return Salary * 0.1m;
        }
    }

    class FinlandTaxCalculation : ITaxCalculationStrategy
    {
        private readonly Yrke _yrke;

        public FinlandTaxCalculation(Yrke yrke)
        {
            _yrke = yrke;
        }
        public decimal CalculateTax(int Salary)
        {
            if (_yrke == Yrke.Hockeyplayer)
                return Salary * 0.1m;
            return Salary * 0.2m;
        }
    }



    enum Yrke
    {
        Unknown,
        OilWorker,
        Hockeyplayer,
        Programmer,
        Other
    }


    class Person
    {
        public Yrke Yrke { get; set; }
        public string Namn { get; set; }
        public void ChangeName(string n)
        {
            Namn = n;
        }
        public void MarryTo(Person p)
        {

        }

        public string Country { get; set; }
        public int Salary { get; set; }
        public decimal CalculateTax()
        {
            ITaxCalculationStrategy taxCalculationToUse = GetTaxCalculationToUseForThisPerson();
            return taxCalculationToUse.CalculateTax(Salary);
        }

        private ITaxCalculationStrategy GetTaxCalculationToUseForThisPerson()
        {
            TaxCalculationFactory factory = new TaxCalculationFactory();
            return factory.GetTaxCalculationToUseForThisPerson(Country, Yrke, Namn);
        }
    }

    class TaxCalculationFactory
    {
        public ITaxCalculationStrategy GetTaxCalculationToUseForThisPerson(string Country, Yrke yrke, string Namn)
        {
            if (Country == "SE") return new SwedenTaxCalculation();
            if (Country == "FI") return new FinlandTaxCalculation(yrke);
            if (Country == "NO") return new NorwayTaxCalculation(yrke, Namn);
            throw new Exception("Fel land");
        }
    }


    /*
     *
        ct
      public SalesOrder(string customerName, string customerPhoneNumber, string customerEmailAddress,
                string customerShippingAddress, List<string> lineItems, bool wrapAsGift
                )
            {
                CustomerName = customerName;
                CustomerPhoneNumber = customerPhoneNumber;
                CustomerEmailAddress = customerEmailAddress;
                CustomerShippingAddress = customerShippingAddress;
                LineItems = lineItems;
                WrapAsGift = wrapAsGift;
            }
     *
     */


    class SalesOrder
    {
        public string CustomerName{ get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmailAddress { get; set; }
        public string CustomerShippingAddress { get; set; }
        public List<string> LineItems { get; set; }
        public bool WrapAsGift { get; set; }
    }


    class SalesOrderBuilder
    {
        public SalesOrderBuilder CustomerInfo(string customerName, string customerPhoneNumber, string customerEmailAddress)
        {
            CustomerName = customerName;
            CustomerPhoneNumber = customerPhoneNumber;
            CustomerEmailAddress = customerEmailAddress;
            return this;
        }
        public SalesOrderBuilder WrapAsGift()
        {
            wrapAsGift = true;
            return this;
        }

        string CustomerName { get; set; }
        string CustomerPhoneNumber { get; set; }
        string CustomerEmailAddress { get; set; }
        string CustomerShippingAddress { get; set; }
        List<string> LineItems { get; set; }
        bool wrapAsGift { get; set; }

        public SalesOrder Build()
        {
            var salesOrder = new SalesOrder
            {
                CustomerName = CustomerName,
                CustomerPhoneNumber = CustomerPhoneNumber,
                CustomerEmailAddress = CustomerEmailAddress,
                CustomerShippingAddress = CustomerShippingAddress,
                LineItems = LineItems,
                WrapAsGift = wrapAsGift
            };
            return salesOrder;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var salesOrder2 = new SalesOrderBuilder()
                .CustomerInfo("Mats Sundin", "555-12121212", "m@m.se")
                //.SetLines()
                .WrapAsGift()
                .Build();

            var salesOrder = new SalesOrder
            {
                CustomerName = "Mats Sundin",
                CustomerPhoneNumber = "555 - 222 111",
                CustomerEmailAddress = "mats@toronto.ca",
                CustomerShippingAddress = "Maple Leafs Garden 123, CA 11122 Toronto",
                LineItems = new List<string> { "Sticks"  },
                WrapAsGift = true
            };










            var p = new Person { Namn="Ole", Country="NO", Salary = 1000, Yrke = Yrke.OilWorker};
            var p2 = new Person { Namn = "Stefan", Country = "SE", Salary = 1000, Yrke = Yrke.Programmer };
            
            Console.WriteLine($"Ole:{p.CalculateTax()}");
            Console.WriteLine($"Stefan:{p2.CalculateTax()}");

        }
    }
}
