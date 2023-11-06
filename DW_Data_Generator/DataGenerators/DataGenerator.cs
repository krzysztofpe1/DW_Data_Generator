using DW_Data_Generator.CarRepairMasterModels;
using DW_Data_Generator.CEOExcelModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DW_Data_Generator.DataGenerators
{
    public class DataGenerator
    {
        #region Public Props
        public DateTime StartDate { get; set; }
        public int RepairsPerDay { get; set; }
        public int RepairsPerDayOscilation { get; set; }
        public DateTime T1 { get; set; }
        public DateTime T2 { get; set; }
        public int LimitRecords { get; set; }
        public int MechanicCount { get; set; }
        public int RegularClients { get; set; }
        public double ChanceForNewClient { get; set; }
        #endregion
        #region Private set Props
        public List<Mechanic> Mechanics { get; private set; }
        public List<Car> Cars { get; private set; }
        public List<Part> Parts { get; private set; }
        public List<Repair> Repairs { get; private set; }
        public List<MechanicTA> MechanicTAs { get; private set; }
        #endregion
        #region Private vars
        private List<Car> _carsList = new List<Car>();
        private List<Mechanic> _mechanicList = new List<Mechanic>();
        private List<Part> _partsList = new List<Part>();
        private List<Repair> _repairsList = new List<Repair>();
        private Random _random = new Random();
        #endregion
        #region Ctor
        public DataGenerator()
        {
            Mechanics = new();
            Cars = new();
            Parts = new();
            Repairs = new();
            MechanicTAs = new();
        }
        #endregion
        #region Public Methods
        public void GenerateData()
        {
            if (LimitRecords == 0)
                LimitRecords = int.MaxValue;
            GenerateMechanics();
            GenerateRegularClients();
            GenerateRecords();
            SortAndIndexData();
        }
        #endregion
        #region Private Methods
        private void GenerateMechanics()
        {
            Mechanics.Clear();
            var names = MiscGenerators.GenerateFirstAndLastNames(MechanicCount);
            for (int i = 0; i < names.Count; i++)
            {
                Mechanics.Add(new Mechanic()
                {
                    Id = i,
                    Name = names[i].Item1,
                    Surname = names[i].Item2
                });
            }
        }
        private void GenerateRegularClients()
        {
            Cars.Clear();
            var names = MiscGenerators.GenerateFirstAndLastNames(RegularClients);
            var registrations = MiscGenerators.GenerateLicensePlates(RegularClients);
            var carInfos = MiscGenerators.GenerateCarInfos(RegularClients);
            for (int i = 0; i < names.Count; i++)
            {
                Cars.Add(new Car()
                {
                    Registration = registrations[i],
                    Brand = carInfos[i].Item1,
                    Model = carInfos[i].Item2,
                    Name = names[i].Item1,
                    Surname = names[i].Item2,
                });
            }
        }
        private Car GenerateSingleClient()
        {
            var name = MiscGenerators.GenerateFirstAndLastNames(1);
            List<string> registration;
            do
            {
                registration = MiscGenerators.GenerateLicensePlates(1);
            } while (Cars.FirstOrDefault(car => car.Registration == registration[0]) != null);
            var carInfo = MiscGenerators.GenerateCarInfos(1);
            Car car = new Car()
            {
                Registration = registration[0],
                Brand = carInfo[0].Item1,
                Model = carInfo[0].Item2,
                Name = name[0].Item1,
                Surname = name[0].Item2,
            };
            Cars.Add(car);
            return car;
        }
        private MechanicTA? GetUnocupiedMechanic(DateTime currentDate)
        {
            MechanicTA? mechanicTA;
            do
            {
                var randomMechanic = Mechanics[_random.Next(Mechanics.Count)];
                mechanicTA = MechanicTAs.FirstOrDefault(item =>
                    item.Date == currentDate && item.Mechanic.Id == randomMechanic.Id
                );
                if (mechanicTA == null)
                {
                    mechanicTA = new MechanicTA()
                    {
                        Mechanic = randomMechanic,
                        Date = currentDate,
                    };
                }
            } while (mechanicTA == null || mechanicTA.HoursAmount >= 8);
            MechanicTAs.Add(mechanicTA);
            return mechanicTA;
        }
        private void GenerateRecords()
        {
            var currentDay = StartDate;
            //Individual Days inside
            while (true)
            {
                var repairsNumber = RepairsPerDay - RepairsPerDayOscilation + _random.Next(RepairsPerDayOscilation * 2 + 1);
                //Individual Repairs inside
                for (int i = 0; i < repairsNumber; i++)
                {

                    Part part = MiscGenerators.GeneratePart();
                    part.Date_order = currentDay.AddDays(-7 + _random.Next(8));
                    part.Date_in_stock = part.Date_order.Value.AddDays(_random.Next(8));
                    if (part.Date_in_stock.Value > currentDay)
                        part.Date_in_stock = currentDay;
                    part.Date_used = currentDay;

                    Car car;
                    if (_random.NextDouble() <= ChanceForNewClient)
                    {
                        //New Client
                        car = GenerateSingleClient();
                    }
                    else
                    {
                        car = Cars[_random.Next(Cars.Count)];
                    }
                    var mechanicTA = GetUnocupiedMechanic(currentDay);
                    Repair repair = new Repair()
                    {
                        Id = Repairs.Count,
                        Repair_date_end = currentDay,
                        Repair_date_start = currentDay.AddDays(-7 + _random.Next(8)),
                        FK_registration = car.Registration,
                        FK_id_mechanic = mechanicTA.Mechanic.Id,
                        Pricing = part.Price.Value + part.LabourCost.Value,
                        Used_car_transporter = _random.NextDouble() <= 0.05,
                        Is_complaint = _random.NextDouble() <= 0.05
                    };
                    Repairs.Add(repair);
                    Parts.Add(part);
                    MechanicTAs[MechanicTAs.IndexOf(mechanicTA)].HoursAmount += part.LabourTime.Value;
                }
                if (currentDay == T2 || Repairs.Count >= LimitRecords)
                {
                    //End of generating data
                    break;
                }
                currentDay = currentDay.AddDays(1);
            }
        }
        private void SortAndIndexData()
        {
            Parts = Parts.OrderBy(item => item.Date_order).ToList();
            for(int i =0;i < Parts.Count; i++)
            {
                Parts[i].Id = i;
            }
        }
        #endregion
    }
}
