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

        public GraphView(SerialPort port)
        {
            if(!port.IsOpen)
            {
                port.Open();    
            }
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            InitializeComponent();
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
