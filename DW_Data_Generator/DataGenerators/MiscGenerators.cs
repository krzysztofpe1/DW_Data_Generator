using DW_Data_Generator.CarRepairMasterModels;
using DW_Data_Generator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW_Data_Generator.DataGenerators
{
    public static class MiscGenerators
    {
        #region Private vars
        private static Random _random = new Random();
        private static List<string> _names = new List<string>();
        private static string _namesFilePath = "FullNames.txt";
        private static List<string> _carInfos = new List<string>();
        private static string _carInfosFilePath = "CarInfo.txt";
        private static List<Part> _parts = new List<Part>();
        private static string _partsFilePath = "Parts.txt";
        #endregion
        #region License Plate
        public static List<string> GenerateLicensePlates(int amount)
        {
            var res = new List<string>();
            for (int i = 0; i < amount; i++)
            {
                string plate = string.Empty;
                do
                {
                    plate = GenerateLicensePlate();
                } while (res.Contains(plate));
                res.Add(plate);
            }
            return res;
        }
        private static string GenerateLicensePlate()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder plate = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                int index = _random.Next(validChars.Length);
                plate.Append(validChars[index]);
            }

            return "GD" + plate.ToString();
        }
        #endregion
        #region Car Info (Brand, Model)
        private static void PopulateCarInfosList()
        {
            var file = File.ReadAllLines(_carInfosFilePath);
            _carInfos = new List<string>(file);
        }
        public static List<(string, string)> GenerateCarInfos(int amount)
        {
            if (_carInfos.Count == 0)
                PopulateCarInfosList();
            if (_carInfos.Count == 0)
                throw new DWException($"File: {_carInfosFilePath} doesn't exist. Can't generate names.");

            if (amount > _carInfos.Count)
                throw new DWException($"File: {_carInfosFilePath} has too few car infos to pick from.");

            var midRes = new List<string>();
            var count = _carInfos.Count;
            string car = string.Empty;
            for(int i = 0; i < amount; i++)
            {
                do
                {
                    var index = _random.Next(count);
                    car = _carInfos[index];
                }while(midRes.Contains(car));
                midRes.Add(car);
            }
            var res = new List<(string, string)>();
            midRes.ForEach(item =>
            {
                var parts = item.Split(';');
                res.Add((parts[0], parts[1]));
            });
            return res;
        }
        #endregion
        #region Names (Firstname, Surname)
        private static void PopulateNamesList()
        {
            var file = File.ReadAllLines(_namesFilePath);
            _names = new List<string>(file);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>Tuple in which first List are First names and second List are Surnames</returns>
        /// <exception cref="DWException"></exception>
        public static List<(string, string)> GenerateFirstAndLastNames(int amount)
        {
            if (_names.Count == 0)
                PopulateNamesList();
            if (_names.Count == 0)
                throw new DWException($"File: {_namesFilePath} doesn't exist. Can't generate names.");

            if (amount > _names.Count)
                throw new DWException($"File: {_namesFilePath} has too few names to pick from.");

            var midRes = new List<string>();
            var count = _names.Count;
            string name = string.Empty;
            for (int i = 0; i < amount; i++)
            {
                do
                {
                    var index = _random.Next(count);
                    name = _names[index];
                } while (midRes.Contains(name));
                midRes.Add(name);
            }
            var res = new List<(string, string)>();
            midRes.ForEach(item =>
            {
                var parts = item.Split(' ');
                res.Add((parts[0], parts[1]));
            });
            return res;
        }
        #endregion
        #region Parts
        private static void PopulatePartsList()
        {
            _parts.Clear();
            var file = File.ReadAllLines(_partsFilePath);
            var listOfParts = new List<string>(file);
            listOfParts.ForEach(item =>
            {
                var parts = item.Split(';');
                _parts.Add(new Part()
                {
                    Part_type = parts[0],
                    Producer = parts[1],
                    Price = double.Parse(parts[2]),
                    LabourCost = double.Parse(parts[3]),
                    LabourTime = double.Parse(parts[4])
                });
            });
        }
        public static Part GeneratePart()
        {
            if(_parts.Count == 0)
                PopulatePartsList();
            if (_parts.Count == 0)
                throw new DWException($"File: {_partsFilePath} doesn't exist. Can't generate names.");

            var  part = _parts[_random.Next(_parts.Count)];
            return new Part()
            {
                Part_type = part.Part_type,
                Producer = part.Producer,
                Price = part.Price,
                LabourCost = part.LabourCost
            };
        }
        #endregion
    }
}
