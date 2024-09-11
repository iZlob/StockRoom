using System;

namespace StockRoom.Objects
{
    public class Box
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Lenght { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float Volume { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Pallet_id { get; set; }

        public Box()
        {
            Id = 0;
            Name = "Box_";
            Lenght = 0;
            Width = 0;
            Height = 0;
            Weight = 0;
            Volume = 0;
            DateStart = DateTime.MaxValue;
            DateEnd = DateTime.MaxValue;
            Pallet_id = 0;
        }
    }
}
