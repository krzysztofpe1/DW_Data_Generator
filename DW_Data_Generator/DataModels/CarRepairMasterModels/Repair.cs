﻿using System;
using System.Text;

namespace DW_Data_Generator.CarRepairMasterModels
{
    public class Repair : ModelInterface
    {
        public int Id { get; set; }
        public DateTime Repair_date_start { get; set; }
        public DateTime Repair_date_end { get; set; }
        public string? FK_registration { get; set; }
        public int FK_id_mechanic { get; set; }
        public double Pricing { get; set; }
        public bool Used_car_transporter { get; set; }
        public bool Is_complaint { get; set; }
        public int RepairNo { get; set; }

        public string GenerateCsvHeader()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';',
                "id",
                "repair_date_start",
                "repair_date_end",
                "FK_registration",
                "FK_id_mechanic",
                "pricing",
                "used_car_transporter",
                "is_complaint",
                "repair_no");
            return sb.ToString();
        }

        public string ToCsv()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(';',
                Id.ToString(),
                Repair_date_start.ToString("dd-MM-yyyy"),
                Repair_date_end.ToString("dd-MM-yyyy"),
                FK_registration,
                FK_id_mechanic,
                Pricing,
                Used_car_transporter.ToString(),
                Is_complaint.ToString(),
                RepairNo.ToString());
            return sb.ToString();
        }
    }
}
