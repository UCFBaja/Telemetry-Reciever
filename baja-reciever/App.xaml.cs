using System.Windows;

namespace baja_reciever
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        // Parse the data
        // Searches array for the given sensor identification string
        // Returns a string with the data from that sensor
        public static string parseData(string[] data, string sensor)
        {
            for (int i = 0; i < data.Length; i++)
            {
                // Make sure there is actually data in the element, and the data is longer than the sensor identifier
                if (data[i].Length > 0 && data[i].Length > sensor.Length)
                {
                    // Looking for the first 3-4 characters
                    string search = data[i].Substring(0, sensor.Length);

                    if (search.Equals(sensor))
                    {
                        /*Console Debug Statements
                        Console.WriteLine("\"" + data[i] + "\"");
                        Console.WriteLine(data[i].Length);
                        return data[i].Substring(3, data[i].Length-1);
                        Console.WriteLine(data[i].Substring(sensor.Length + 1, data[i].Length - 3));
                        */
                        return data[i].Substring(sensor.Length + 1, data[i].Length - 3);
                    }

                }
            }
            return "";

        }




    }
}
