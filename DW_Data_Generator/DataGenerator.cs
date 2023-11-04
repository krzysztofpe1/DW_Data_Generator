using DW_Data_Generator.CarRepairMasterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW_Data_Generator
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
        #region Private vars
        private List<Car> _carsList = new List<Car>();
        private List<Mechanic> _mechanicList = new List<Mechanic>();
        private List<Part> _partsList = new List<Part>();
        private List<Repair> _repairsList = new List<Repair>();
        #endregion
        public DataGenerator() { }

        public void GenerateData()
        {

        }
    }
}
