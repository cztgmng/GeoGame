using Microsoft.AspNetCore.Mvc;
using System;

namespace GeoGame
{
    public class PointsManager
    {
        private Dictionary<string, float> pointsDictionary;
        private string filePath = "points.txt";

        public PointsManager()
        {
            pointsDictionary = new Dictionary<string, float>();
            LoadPointsFromFile();
        }

        public Dictionary<string, float> GetSortedPoints()
        {
            return pointsDictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        private void LoadPointsFromFile()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        string username = parts[0].Trim();
                        float points;
                        if (float.TryParse(parts[1].Trim(), out points))
                        {
                            pointsDictionary.Add(username, points);
                        }
                    }
                }
            }
        }

        public float GetPoints(string username)
        {
            if (pointsDictionary.ContainsKey(username))
            {
                return pointsDictionary[username];
            }
            else
            {
                return 0;
            }
        }

        public void AddPoints(string username, float pointsToAdd)
        {
            if (pointsDictionary.ContainsKey(username))
            {
                pointsDictionary[username] = pointsDictionary[username] + pointsToAdd;
            }
            else
            {
                pointsDictionary.Add(username, pointsToAdd);
            }

            SavePointsToFile();
        }

        private void SavePointsToFile()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var kvp in pointsDictionary)
                {
                    writer.WriteLine($"{kvp.Key}={kvp.Value}");
                }
            }
        }
    }
}
namespace GeoGame.Pages
{
    public class GeoGameController : Controller
    {
        public class PointsModel
        {
            public float value { get; set; }
        }
        [HttpPost]
        public IActionResult UploadResults([FromBody] PointsModel points)
        {
            new PointsManager().AddPoints(Request.Cookies["UserCookie"], points.value);
            return Ok();
        }
        public IActionResult GetRandom()
        {
            string latitude = string.Empty;
            string longitude = string.Empty;

            Location randomLocation = new Location { };
            switch (HttpContext.Request.Cookies["difficulty"])
            {
                case "easy":
                    randomLocation = LocationHelper.GetRandomEasyLocation();
                    break;

                default:
                    randomLocation = GetRandomLoc();   
                    break;
            }

            latitude = randomLocation.latitude;
            longitude = randomLocation.longitude;
            return Json(new { latitude, longitude });
        }
        static Location GetRandomLoc()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync("https://api.3geonames.org/?randomland=yes").Result;

                response.EnsureSuccessStatusCode();

                string xmlContent = response.Content.ReadAsStringAsync().Result;

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                Location loc;

                loc.latitude = xmlDoc.SelectSingleNode("//geodata/major/latt")?.InnerText;
                loc.longitude = xmlDoc.SelectSingleNode("//geodata/major/longt")?.InnerText;

                return loc;
            }

        }
    }
    
}
