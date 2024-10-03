
using System;
using System.Globalization;

namespace GeoGame
{
    public struct Location
    {
        public string latitude;
        public string longitude;
    }

    public class LocationHelper
    {
        public static Location GetRandomEasyLocation()
        {
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            List<Location> _locations = new List<Location>();

            foreach (var line in File.ReadAllLines("worldcities.txt"))
            {
                float random1 = (float)(new Random().NextDouble() * (0.00318 * 2) - 0.00318);
                float random2 = (float)(new Random().NextDouble() * (0.00318 * 2) - 0.00318);

                string[] parts = line.Split(',');
                string latitude = (float.Parse(parts[0].Trim(), NumberStyles.Any, ci) + random1).ToString();
                string longitude = (float.Parse(parts[1].Trim(), NumberStyles.Any, ci) + random2).ToString();

                _locations.Add(new Location { latitude = latitude.Replace(",", "."), longitude = longitude.Replace(",", ".") });
            }

            return _locations[new Random().Next(_locations.Count)];
        }
    }
}
