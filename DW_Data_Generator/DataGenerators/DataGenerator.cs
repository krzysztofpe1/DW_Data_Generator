using DW_Data_Generator.CarRepairMasterModels;
using DW_Data_Generator.CEOExcelModels;
using DW_Data_Generator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private Random _random = new Random();
        private int _repair_no = 1;
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
                    Type = carInfos[i].Item3,
                    YearOfProduction = carInfos[i].Item4,
                    Name = names[i].Item1,
                    Surname = names[i].Item2,
                    IntroductionDate = StartDate

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
                Type = carInfo[0].Item3,
                YearOfProduction = carInfo[0].Item4,
                Name = name[0].Item1,
                Surname = name[0].Item2,
            };
            Cars.Add(car);
            return car;
        }
        private MechanicTA GetUnocupiedMechanic(DateTime currentDate)
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
                    if (_random.NextDouble() <= 0.05 && Repairs.Count > 20)
                    {
                        //Complaint
                        Repair originalRepair;
                        do
                        {
                            originalRepair = Repairs[Repairs.Count - _random.Next(repairsNumber, 20)];
                        } while (originalRepair.Is_complaint == true);
                        var complaintRepair = new Repair()
                        {
                            Id = Repairs.Count,
                            Repair_date_end = currentDay,
                            Repair_date_start = currentDay.AddDays(-3 + _random.Next(4)),
                            FK_registration = originalRepair.FK_registration,
                            //maybe will need to change for the cost of repair
                            Pricing = 0,
                            Used_car_transporter = _random.NextDouble() <= 0.05,
                            Is_complaint = true,
                            RepairNo = originalRepair.RepairNo
                        };
                        var partsFromOriginalRepair = Parts.Where(item => item.FK_id_repair == originalRepair.Id).ToList();
                        var partsForComplaintRepair = partsFromOriginalRepair.PickRandom(_random.Next(partsFromOriginalRepair.Count));
                        for(int j=0;j< partsForComplaintRepair.Count; j++)
                        {
                            partsForComplaintRepair[j].FK_id_repair = complaintRepair.Id;
                            partsForComplaintRepair[j].Date_used = currentDay;
                            partsForComplaintRepair[j].Date_order = currentDay.AddDays(-3+_random.Next(4));
                            partsForComplaintRepair[j].Date_in_stock = partsForComplaintRepair[j].Date_order.Value.AddDays(_random.Next(4));
                            if (partsForComplaintRepair[j].Date_in_stock > currentDay)
                                partsForComplaintRepair[j].Date_in_stock = currentDay;
                        }
                        Parts.AddRange(partsForComplaintRepair);
                        var mechTA = GetUnocupiedMechanic(currentDay);
                        MechanicTAs[MechanicTAs.IndexOf(mechTA)].HoursAmount += partsForComplaintRepair.Sum(item => item.LabourTime.Value);
                        complaintRepair.FK_id_mechanic = mechTA.Mechanic.Id;
                        Repairs.Add(complaintRepair);
                        

                        continue;
                    }
                    var listOfPartsInRepair = new List<Part>();
                    for (int j = 0; j < _random.Next(4); j++)
                    {
                        Part part = MiscGenerators.GeneratePart();
                        part.Date_order = currentDay.AddDays(-7 + _random.Next(8));
                        part.Date_in_stock = part.Date_order.Value.AddDays(_random.Next(8));
                        if (part.Date_in_stock.Value > currentDay)
                            part.Date_in_stock = currentDay;
                        part.Date_used = currentDay;
                        listOfPartsInRepair.Add(part);
                    }
                    Car car;
                    if (_random.NextDouble() <= ChanceForNewClient)
                    {
                        //New Client
                        car = GenerateSingleClient();
                        car.IntroductionDate = currentDay;
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
                        Pricing = listOfPartsInRepair.Sum(item => item.LabourCost.Value),
                        Used_car_transporter = _random.NextDouble() <= 0.05,
                        Is_complaint = false,
                        RepairNo = _repair_no++
                    };
                    Repairs.Add(repair);
                    for (int j = 0; j < listOfPartsInRepair.Count; j++)
                    {
                        listOfPartsInRepair[j].FK_id_repair = repair.Id;
                    }
                    Parts.AddRange(listOfPartsInRepair);
                    MechanicTAs[MechanicTAs.IndexOf(mechanicTA)].HoursAmount += listOfPartsInRepair.Sum(item => item.LabourTime.Value);
                    if (Repairs.Count >= LimitRecords)
                        break;
                }
                if (currentDay == T2)
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
            for (int i = 0; i < Parts.Count; i++)
            {
                Parts[i].Id = i;
            }
        }
        #endregion
        
    }
    public static class ListHelper
    {
        private static Random _random = new Random();
        public static List<T> PickRandom<T>(this List<T> list, int number)
        {
            if (list == null)
                return new List<T>();
            if (number == 0)
                return new List<T>();
            if (list.Count < number)
                throw new DWException("Tried to pick more items than there are in list");
            var res = new List<T>();
            var temp = new List<T>();
            temp.AddRange(list);
            do
            {
                var item = temp[_random.Next(temp.Count)];
                res.Add(item);
                temp.Remove(item);
            } while (res.Count < number);
            return res;
        }
    }
}
