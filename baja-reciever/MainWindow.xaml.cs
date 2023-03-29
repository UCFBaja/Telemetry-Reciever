using System;
using System.IO.Ports;
using System.Windows;

namespace baja_reciever
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SerialPort port = new SerialPort();
        public Boolean started;

        public MainWindow()
        {
            // Don't need to worry about having the user set these
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;

            // Start window components
            InitializeComponent();
            Status.Text = "Inactive"; // Initialize the status message

        }

        // Event handler for serial data
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Update status
            this.Dispatcher.Invoke(() =>
            {
                Status.Text = "Recieving Data";
            });

            try
            {
                if (port.IsOpen)
                {
                    // Turn recieved data to string
                    string recievedData = port.ReadLine();

                    this.Dispatcher.Invoke(() =>
                    {
                        // Just need to display the total recieved data for now
                        // Won't need this in the future
                        Output.Text = recievedData;
                    });

                    // Split recieved data into array of strings
                    // (each element will have a single sensor's output)
                    string[] data = recievedData.Split('|');

                    // Update the displays with the given data
                    updateDisplays(data);
                }
            }

            catch (System.IO.IOException)
            {
                return;
            }

            catch (System.InvalidOperationException)
            {
                return;
            }

            // NOT NEEDED: This is an inefficient way to do this, but may be required when using the ESP32
            /*
            if(recievedData != "\n")
            {
                this.Dispatcher.Invoke(() =>
                {
                    // Just need to display the total recieved data for now
                    // Won't need this in the future
                    Output.Text = Output.Text + recievedData;
                });

            }

            else if(recievedData == "\n")
            {
                this.Dispatcher.Invoke(() =>
                {
                    Output.Text = "";
                });
            }
            */

        }

        // Update the displays
        // TODO: Add other sensors
        public void updateDisplays(string[] data)
        {
            // Doing this all in a single dispatcher for code cleanliness
            // (May not be the most runtime-efficient solution)
            this.Dispatcher.Invoke(() =>
            {
                // Could try and move all of these into a single try-catch, but whatever
                try
                {
                    priRPM.Text = (parseData(data, " PRPM") + " RPM");
                }

                catch (FormatException)
                {
                    priRPM.Text = "0 RPM";
                }
                //AFLDisplay.Text = parseData(data, "AFL");
                try
                {
                    FuelLevel.Value = double.Parse(parseData(data, "FU"));
                }

                catch (FormatException)
                {
                    FuelLevel.Value = 0.0;
                }

                // Doing entire Suspension Travel display in one try-catch
                try
                {
                    FLTravel.Text = (parseData(data, " SFL") + " \"");
                    FRTravel.Text = (parseData(data, " SFR") + " \"");
                    RLTravel.Text = (parseData(data, " SRL") + " \"");
                    RRTravel.Text = (parseData(data, " SRR") + " \"");
                }

                catch (FormatException)
                {
                    FLTravel.Text = "0 \"";
                    FRTravel.Text = "0 \"";
                    RLTravel.Text = "0 \"";
                    RRTravel.Text = "0 \"";
                }
            });
        }


        // Parse the data
        // Searches array for the given sensor identification string
        // Returns a string with the data from that sensor
        public string parseData(string[] data, string sensor)
        {
            for (int i = 0; i < data.Length; i++)
            {
                // Make sure there is actually data in the element
                if (data[i].Length > 0)
                {
                    // Looking for the first 3-4 characters
                    string search = data[i].Substring(0, sensor.Length);

                    if (search.Equals(sensor))
                    {
                        /* Console Debug Statements
                        Console.WriteLine(data[i]);
                        Console.WriteLine(data[i].Length);
                        return data[i].Substring(3, data[i].Length-1);
                        Console.WriteLine(data[i].Substring(sensor.Length - 1, data[i].Length - 6));
                        */
                        return data[i].Substring(sensor.Length + 1, data[i].Length - 6);
                    }

                }
            }
            return "";

        }

        // When the user clicks the start button, we need to read each dropdown box
        // and set the serial settings accordingly
        public void Start_Click(object sender, RoutedEventArgs e)
        {
            if (started == false)
            {

                Status.Text = "Opening Port";
                // Setting serial settings based on dropdown options
                try
                {
                    if (!port.IsOpen)
                    {
                        port.PortName = (comPort.SelectionBoxItem.ToString());
                        port.BaudRate = int.Parse(baudRate.SelectionBoxItem.ToString());
                    }

                    // Setting up event handler
                    port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

                    port.Open();
                    Status.Text = "Port Open";

                    Start.Content = "Stop";
                    started = true;

                }

                catch (System.ArgumentException)
                {
                    MessageBox.Show("Argument Error: Please Enter values for port name and baud rate");
                    Status.Text = "Error";
                }

                // Make sure program doesn't crash if it can't open up the serial port
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Serial Error: Please ensure your settings are correct");
                    Status.Text = "Error";
                }

            }

            else if (started == true)
            {

                port.DataReceived -= port_DataReceived;
                port.DataReceived -= GraphView.port_DataReceived;

                Start.Content = "Start";
                started = false;

                // I thought this was erroring out, but it works now. WTF??
                port.Close();
                Status.Text = "Port Closed";
            }

        }

        //view the About Window
        private void AboutWindow(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Show();
        }

        //view the Graph Window
        private void viewGraph(object sender, RoutedEventArgs e)
        {
            GraphView graphWindow = new GraphView();
            graphWindow.Show();

        }

        //exit the application
        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
