using System;
using System.Collections.Generic;

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
            if(Country == "SE") return new SwedenTaxCalculation();
            if (Country == "FI") return new FinlandTaxCalculation(Yrke);
            if (Country == "NO") return new NorwayTaxCalculation(Yrke,Namn);
            throw new Exception("Fel land");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var p = new Person { Namn="Ole", Country="NO", Salary = 1000, Yrke = Yrke.OilWorker};
            var p2 = new Person { Namn = "Stefan", Country = "SE", Salary = 1000, Yrke = Yrke.Programmer };
            
            Console.WriteLine($"Ole:{p.CalculateTax()}");
            Console.WriteLine($"Stefan:{p2.CalculateTax()}");

        }
    }
}
