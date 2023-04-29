using System.Diagnostics;
using System.IO.Ports;
using System.Windows;

namespace baja_reciever
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : Window
    {
        readonly Stopwatch Stopwatch = Stopwatch.StartNew();

        public static string sensor = "";

        public GraphView(SerialPort port)
        {
            try
            { 
                if (!port.IsOpen)
                {
                    port.Open();
                }
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            }
            // Make sure program doesn't crash if it can't open up the serial port
            catch (System.IO.IOException)
            {
                MessageBox.Show("Serial Error: Please ensure your settings are correct");
            }
            InitializeComponent();
        }

        public static void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Turn recieved data to string
            string recievedData = MainWindow.port.ReadExisting();

            // Split recieved data into array of strings
            // (each element will have a single sensor's output)
            string[] data = recievedData.Split('|');

            updateGraph(data, GraphView.sensor);

        }

        public static void updateGraph(string[] data, string sensor)
        {
            double value = double.Parse(App.parseData(data, sensor));


        }

        public void sensorSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            GraphView.sensor = sensorSelectionBox.SelectionBoxItem.ToString();
        }
    }


}
