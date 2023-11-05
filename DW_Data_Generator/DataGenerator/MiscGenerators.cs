using DW_Data_Generator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW_Data_Generator.DataGenerator
{
    public static class MiscGenerators
    {
        private static Random _random = new Random();
        private static List<string> _names = new List<string>();
        private static string _namesFilePath = "FullNames.txt";
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
        #region Names
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
        public static (List<string>, List<string>) GenerateFirstAndLastNames(int amount)
        {
            if(_names.Count == 0)
                PopulateNamesList();
            if (_names.Count == 0)
                throw new DWException($"File: {_namesFilePath} doesn't exist. Can't generate names.");

            if (amount > _names.Count)
                throw new DWException($"File: {_namesFilePath} has too few names to pick from.");

            var res = new List<string>();
            var count = _names.Count;
            string name=string.Empty;
            for (int i = 0;i < amount;i++)
            {
                do
                {
                    var index = _random.Next(count);
                    name = _names[index];
                } while (res.Contains(name));
                res.Add(name);
            }
            var firstnames = new List<string>();
            var lastnames = new List<string>();
            res.ForEach(item =>
            {
                var parts = item.Split(' ');
                firstnames.Add(parts[0]);
                lastnames.Add(parts[1]);
            });
            return (firstnames, lastnames);
        }
        #endregion
    }
}
