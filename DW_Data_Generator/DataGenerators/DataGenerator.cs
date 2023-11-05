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
            GenerateMechanics();
            GenerateRegularClients();
            GenerateRecords();
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
        private void GenerateRecords()
        {

        }
        #endregion
    }
}
