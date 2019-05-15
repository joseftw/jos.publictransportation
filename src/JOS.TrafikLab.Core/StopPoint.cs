using System;

namespace JOS.TrafikLab.Core
{
    public class StopPoint
    {
        public StopPoint(
            string name,
            Location location,
            int number,
            StopAreaType type,
            DateTime openingDate)
        {
            Name = name;
            Location = location;
            Number = number;
            Type = type;
            OpeningDate = openingDate;
        }

        public string Name { get; }
        public Location Location { get; }
        public int Number { get; }
        public DateTime OpeningDate { get; }
        public StopAreaType Type { get; }
    }
}
