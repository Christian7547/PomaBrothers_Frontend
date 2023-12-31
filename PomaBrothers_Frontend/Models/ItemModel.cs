﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PomaBrothers_Frontend.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string Marker { get; set; }
        public int CapacityOrSize { get; set; }
        public string MeasurementUnit { get; set; }
    }
}
