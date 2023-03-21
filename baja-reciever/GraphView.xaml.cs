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

        public GraphView()
        {
            InitializeComponent();
        }

        public void serialSetup()
        {
            if (!(MainWindow.port.IsOpen))
            {
                MainWindow.port.Open();
            }

            MainWindow.port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        public static void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Turn recieved data to string
            string recievedData = MainWindow.port.ReadExisting();

            // Split recieved data into array of strings
            // (each element will have a single sensor's output)
            string[] data = recievedData.Split('|');

        }

        public void updateGraph(string data)
        {

        }
    }


}
