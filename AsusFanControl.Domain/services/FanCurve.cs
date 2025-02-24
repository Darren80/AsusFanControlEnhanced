using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsusFanControl.Domain.services
{
    public class FanCurve
    {
        // private readonly Dictionary<int, Point> _fanCurvePoints = new Dictionary<int, Point>();

        public Dictionary<int, Point>? convertStringToPointsDictionary(
            string fanCurvePointsString, int temperatureLowerBound, int temperatureUpperBound, int fanSpeedLowerBound, int fanSpeedUpperBound
            )
        {
            if (fanCurvePointsString == null)
            {
                return null;
            }

            // Define the allowed characters
            var allowedChars = new HashSet<char>("0123456789-,");
            // Check if the string contains any invalid characters
            if (fanCurvePointsString.Any(c => !allowedChars.Contains(c)))
            {
                throw new ArgumentException($"The fan curve points string contains invalid characters. Allowed characters: {string.Join("", allowedChars)}");
            }


            // Parse the string
            int pointCount = 1;
            var pointsDictionary = new Dictionary<int, Point>();
            try
            {
                var pointStrings = fanCurvePointsString.Split('-');

                foreach (var pointString in pointStrings)
                {
                    // Split the point string into temperature and fan speed parts
                    string[] parts = pointString.Split(',');

                    // Ensure there are exactly two parts (temperature and fan speed)
                    if (parts.Length != 2)
                    {
                        throw new ArgumentException($"Invalid point format: '{pointString}'. Expected format: 'temperature,fanSpeed'.");
                    }

                    // Parse the temperature and fan speed values
                    int temperature = int.Parse(parts[0]);
                    int fanSpeed = int.Parse(parts[1]);

                    // Validate temperature and fan speed ranges
                    if (temperature < temperatureLowerBound || temperature > temperatureUpperBound)
                    {
                        throw new ArgumentOutOfRangeException(nameof(temperature), $"Temperature value {temperature} is out of range. Valid range: {temperatureLowerBound}-{temperatureUpperBound}.");
                    }

                    if (fanSpeed < fanSpeedLowerBound || fanSpeed > fanSpeedUpperBound)
                    {
                        throw new ArgumentOutOfRangeException(nameof(fanSpeed), $"Fan speed value {fanSpeed} is out of range. Valid range: {fanSpeedLowerBound}-{fanSpeedUpperBound}.");
                    }

                    // Create a new KeyValuePair with an incremented ID and the parsed Point
                    pointsDictionary.Add(pointCount++, new Point(temperature, fanSpeed));
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error occurred while parsing the fan curve points string" +
                    $"\n\n{ex.Message}", ex);
            }

            return pointsDictionary;
        }
    }
}
