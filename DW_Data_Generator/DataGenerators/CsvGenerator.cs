﻿using DW_Data_Generator.CarRepairMasterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace DW_Data_Generator.DataGenerators
{
    public class CsvGenerator
    {
        private DataGenerator _dataGenerator;
        private string _systemDirectory = "CarRepairMaster";
        private string _excelDirectory = "CEOExcel";
        public CsvGenerator(DataGenerator dataGenerator)
        {
            _dataGenerator = dataGenerator;
        }
        public void ExportData()
        {
            CreateDirectories();
            SaveFile(_dataGenerator.Cars.Cast<ModelInterface>().ToList(), _systemDirectory + "\\Cars.csv");
            SaveFile(_dataGenerator.Mechanics.Cast<ModelInterface>().ToList(), _systemDirectory + "\\Mechanics.csv");
            SaveFile(_dataGenerator.Parts.Cast<ModelInterface>().ToList(), _systemDirectory + "\\Parts.csv");
            SaveFile(_dataGenerator.Repairs.Cast<ModelInterface>().ToList(), _systemDirectory + "\\Repairs.csv");

            var mechanics = _dataGenerator.Mechanics;
            var mechanicTAs = _dataGenerator.MechanicTAs;
            List<int> years = new();
            mechanicTAs.ForEach(item =>
            {
                var tempYear = item.Date.Year;
                if (!years.Contains(tempYear))
                    years.Add(tempYear);
            });
            foreach (var mechanic in mechanics)
            {
                years.ForEach(year =>
                {
                    CreateDirectory(_excelDirectory + "\\" + year);
                    var mechanicTA = mechanicTAs.Where(item => item.Mechanic.Id == mechanic.Id).Cast<ModelInterface>().ToList();
                    SaveFile(mechanicTA, _excelDirectory + '\\' + year + "\\" + mechanic.Name + mechanic.Surname + ".csv");
                });
            }
        }
        #region directories
        private void CreateDirectories()
        {
            CreateDirectory(_systemDirectory);
            CreateDirectory(_excelDirectory);
        }
        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        #endregion
        private void SaveFile(List<ModelInterface> objects, string path)
        {
            if (objects.Count == 0)
                return;
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(objects[0].GenerateCsvHeader());
                    foreach (var obj in objects)
                    {
                        sw.WriteLine(obj.ToCsv());
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"Wystąpił błąd podczas Zapisywania danych: {ex.Message}"); }
        }
    }
}
