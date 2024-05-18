using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsusFanControl;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AsusFanControlGUI
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;
        AsusControl asusControl = new AsusControl();
        int fanSpeed = 0;

        public Form1()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            toolStripMenuItemTurnOffControlOnExit.Checked = Properties.Settings.Default.turnOffControlOnExit;
            toolStripMenuItemForbidUnsafeSettings.Checked = Properties.Settings.Default.forbidUnsafeSettings;
            //trackBarFanSpeed.Value = Properties.Settings.Default.fanSpeed;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create and start the timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1000 milliseconds = 1 second
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update fan speeds
            labelRPM.Text = string.Join(" ", asusControl.GetFanSpeeds());

            // Update CPU temperature
            labelCPUTemp.Text = $"{asusControl.Thermal_Read_Cpu_Temperature()}";
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.turnOffControlOnExit)
                asusControl.SetFanSpeeds(0);
        }

        private void toolStripMenuItemTurnOffControlOnExit_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.turnOffControlOnExit = toolStripMenuItemTurnOffControlOnExit.Checked;
            Properties.Settings.Default.Save();
        }

        private void toolStripMenuItemForbidUnsafeSettings_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.forbidUnsafeSettings = toolStripMenuItemForbidUnsafeSettings.Checked;
            Properties.Settings.Default.Save();
        }

        private void toolStripMenuItemCheckForUpdates_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Karmel0x/AsusFanControl/releases");
        }

        private void setFanSpeed(int value, bool isTurnedOn)
        {
            Properties.Settings.Default.fanSpeed = value;
            Properties.Settings.Default.Save();

            if (!isTurnedOn)
                value = 0;

            if (value == 0)
                labelValue.Text = "turned off";
            else
                labelValue.Text = value.ToString();

            if (fanSpeed == value)
                return;

            fanSpeed = value;
            asusControl.SetFanSpeeds(value);
        }

        private void trackBarFanSpeed_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.forbidUnsafeSettings)
            {
                if (trackBarFanSpeed.Value < 40)
                    trackBarFanSpeed.Value = 40;
                else if (trackBarFanSpeed.Value > 99)
                    trackBarFanSpeed.Value = 99;
            }

            setFanSpeed(trackBarFanSpeed.Value, fanControl.Checked);
        }

        private void trackBarFanSpeed_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
                return;

            trackBarFanSpeed_MouseCaptureChanged(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            labelRPM.Text = string.Join(" ", asusControl.GetFanSpeeds());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            labelCPUTemp.Text = $"{asusControl.Thermal_Read_Cpu_Temperature()}";
        }




        // My Code:
        private Point maxPoint;
        private Point minPoint;

        private void pictureBoxFanCurve_Paint(object sender, PaintEventArgs e)
        {
            // Draw the fan curve graph
            Graphics g = e.Graphics;

            // Set up the graph dimensions
            int padding = 20;
            int graphWidth = pictureBoxFanCurve.Width - 2 * padding;
            int graphHeight = pictureBoxFanCurve.Height - 2 * padding;

            // Draw the temperature axis and labels
            int tempMin = 30;
            int tempMax = 100;
            int tempInterval = 10;

            g.DrawLine(Pens.Black, padding, pictureBoxFanCurve.Height - padding, pictureBoxFanCurve.Width - padding, pictureBoxFanCurve.Height - padding);

            for (int temp = tempMin; temp <= tempMax; temp += tempInterval)
            {
                int x = padding + (temp - tempMin) * graphWidth / (tempMax - tempMin);
                g.DrawLine(Pens.Black, x, pictureBoxFanCurve.Height - padding - 5, x, pictureBoxFanCurve.Height - padding + 5);
                g.DrawString(temp.ToString(), DefaultFont, Brushes.Black, x - 10, pictureBoxFanCurve.Height - padding + 10);
            }

            g.DrawString("Temperature (°C)", DefaultFont, Brushes.Black, pictureBoxFanCurve.Width / 2 - 40, pictureBoxFanCurve.Height - padding + 20);

            // Draw the fan speed axis and labels
            int speedMin = 0;
            int speedMax = 100;
            int speedInterval = 20;

            g.DrawLine(Pens.Black, padding, padding, padding, pictureBoxFanCurve.Height - padding);

            for (int speed = speedMin; speed <= speedMax; speed += speedInterval)
            {
                int y = pictureBoxFanCurve.Height - padding - speed * graphHeight / speedMax;
                g.DrawLine(Pens.Black, padding - 5, y, padding + 5, y);
                g.DrawString(speed.ToString(), DefaultFont, Brushes.Black, 5, y - 10);
            }

            g.DrawString("Fan Speed (%)", DefaultFont, Brushes.Black, padding - 10, padding - 20, new StringFormat(StringFormatFlags.DirectionVertical));

            // Draw the fan curve points
            foreach (Point point in fanCurvePoints.Values)
            {
                int x = padding + (point.X - tempMin) * graphWidth / (tempMax - tempMin);
                int y = pictureBoxFanCurve.Height - padding - point.Y * graphHeight / speedMax;
                g.FillEllipse(Brushes.Green, x - 3, y - 3, 12, 12);
            }

            // Draw the maximum and minimum temperature points
            int maxX = padding + (maxPoint.X - tempMin) * graphWidth / (tempMax - tempMin);
            int maxY = pictureBoxFanCurve.Height - padding - maxPoint.Y * graphHeight / speedMax;
            g.FillEllipse(Brushes.Red, maxX - 5, maxY - 5, 10, 10);

            int minX = padding + (minPoint.X - tempMin) * graphWidth / (tempMax - tempMin);
            int minY = pictureBoxFanCurve.Height - padding - minPoint.Y * graphHeight / speedMax;
            g.FillEllipse(Brushes.Yellow, minX - 5, minY - 5, 10, 10);

            // Draw lines connecting the fan curve points
            if (fanCurvePoints.Count >= 2)
            {
                Point[] graphPoints = fanCurvePoints.Values
                    .OrderBy(p => p.X)
                    .Select(p => new Point(
                        padding + (p.X - tempMin) * graphWidth / (tempMax - tempMin),
                        pictureBoxFanCurve.Height - padding - p.Y * graphHeight / speedMax
                    ))
                    .ToArray();

                using (Pen thickPen = new Pen(Color.Black, 3))
                {
                    g.DrawLines(Pens.Black, graphPoints);
                }
            }
        }

        private void pictureBoxFanCurve_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            // Set up the graph dimensions
            int padding = 20;
            int graphWidth = pictureBoxFanCurve.Width - 2 * padding;
            int graphHeight = pictureBoxFanCurve.Height - 2 * padding;

            // Define the temperature and fan speed ranges
            int tempMin = 30;
            int tempMax = 100;
            int speedMin = 0;
            int speedMax = 100;

            // Convert the mouse coordinates to graph coordinates
            int temperature = tempMin + (e.X - padding) * (tempMax - tempMin) / graphWidth;
            int fanSpeed = speedMax - (e.Y - padding) * speedMax / graphHeight;

            // Check if a point with the same y-axis already exists
            if (fanCurvePoints.Values.Any(p => p.Y == fanSpeed))
            {
                return; // Do not add the new point
            }

            if (fanCurvePoints.Count >= 15)
            {
                MessageBox.Show("Maximum number of points reached.");
                return;
            }

            // Generate a unique ID for the new point
            int newID = fanCurvePoints.Count > 0 ? fanCurvePoints.Keys.Max() + 1 : 1;

            // Add a new point to the fan curve
            Point newPoint = new Point(temperature, fanSpeed);
            fanCurvePoints[newID] = newPoint;

            pictureBoxFanCurve.Invalidate(); // Redraw the graph
        }

        private int selectedPointId = 0;

        private (int temperature, int fanSpeed) mousePosition(Point e)
        {
            // Set up the graph dimensions
            int padding = 20;
            int graphWidth = pictureBoxFanCurve.Width - 2 * padding;
            int graphHeight = pictureBoxFanCurve.Height - 2 * padding;

            // Define the temperature and fan speed ranges
            int tempMin = 30;
            int tempMax = 100;
            int speedMin = 0;
            int speedMax = 100;

            // Convert the mouse coordinates to graph coordinates
            int temperature = tempMin + (e.X - padding) * (tempMax - tempMin) / graphWidth;
            int fanSpeed = speedMax - (e.Y - padding) * speedMax / graphHeight;

            return (temperature, fanSpeed);
        }


        private void pictureBoxFanCurve_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                (int temperature, int fanSpeed) = mousePosition(e.Location);

                var nearestPoints = fanCurvePoints
                       .OrderBy(p => Distance(p.Value, new Point(temperature, fanSpeed)));

                var reachablePoint = nearestPoints
                    .FirstOrDefault(p => Distance(p.Value, new Point(temperature, fanSpeed)) <= 20);

                if (reachablePoint.Value != Point.Empty)
                {
                    fanCurvePoints.Remove(reachablePoint.Key);
                    Console.WriteLine($"Point ID: {reachablePoint.Key} deleted.");
                    pictureBoxFanCurve.Invalidate();
                }
            }



        }

        private void pictureBoxFanCurve_MouseDown(object sender, MouseEventArgs e)
        {
            Console.Write("MouseDown: ");

            if (e.Button == MouseButtons.Left)
            {
                Console.WriteLine(e.Button);

                // Get mouse position
                (int temperature, int fanSpeed) = mousePosition(e.Location);


                var nearestPoints = fanCurvePoints
                    .OrderBy(p => Distance(p.Value, new Point(temperature, fanSpeed)));

                var reachablePoint = nearestPoints
                    .FirstOrDefault(p => Distance(p.Value, new Point(temperature, fanSpeed)) <= 20);

                if (reachablePoint.Value != Point.Empty)
                {
                    selectedPointId = reachablePoint.Key;
                    Console.WriteLine($"Distance: {Distance(reachablePoint.Value, new Point(temperature, fanSpeed))}");
                }
                else
                {
                    selectedPointId = 0;
                }
            }
        }

        private void pictureBoxFanCurve_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedPointId != 0)
            {
                // Console.Write("MouseMove, Key: ");

                // Get mouse location on grid
                (int temperature, int fanSpeed) = mousePosition(e.Location);
                fanCurvePoints[selectedPointId] = new Point(temperature, fanSpeed);
                pictureBoxFanCurve.Invalidate(); // Redraw the graph
            }
        }

        private void pictureBoxFanCurve_MouseUp(object sender, MouseEventArgs e)
        {
            selectedPointId = 0;

            // fanCurvePoints.ToList().ForEach(point => Console.Write($"ID: {point.Key}, X: {point.Value.X}, Y: {point.Value.Y}"));
            // Console.WriteLine();
        }

        private double Distance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private Dictionary<int, Point> fanCurvePoints = new Dictionary<int, Point>();

        private Timer fanCurveTimer; // Declare the timer as a class-level variable

        bool isControlYeilded = false;
        private void fanCurve_CheckedChanged(object sender, EventArgs e)
        {
            if (fanCurveControl.Checked)
            {
                // Start a timer to read the temperature and update the fan speed every 3 seconds
                fanCurveTimer = new Timer();
                fanCurveTimer.Interval = 1000; // 3 seconds
                fanCurveTimer.Tick += (s, args) =>
                {
                    // Read the current temperature
                    ulong currentTemp = asusControl.Thermal_Read_Cpu_Temperature(); // Implement the ReadTemperature method to get the current temperature

                    // Find the fan curve points that bracket the current temperature
                    KeyValuePair<int, Point> lowerPoint = fanCurvePoints.OrderByDescending(p => p.Value.X).FirstOrDefault(p => (ulong)p.Value.X <= currentTemp);
                    KeyValuePair<int, Point> upperPoint = fanCurvePoints.OrderBy(p => p.Value.X).FirstOrDefault(p => (ulong)p.Value.X >= currentTemp);

                    // Calculate the fan speed based on linear interpolation between the bracket points
                    int fanSpeed;
                    // Check if the current temperature is within the range of the fan curve points
                    if ((ulong)lowerPoint.Key == 0 || (ulong)upperPoint.Key == 0)
                    {
                        // Temperature is outside the range, yield control to the system
                        if (!isControlYeilded)
                        {
                            asusControl.SetFanSpeeds(0);
                            isControlYeilded = true;
                        }
                    }
                    else if (lowerPoint.Value.X == upperPoint.Value.X)
                    {
                        isControlYeilded = false;
                        fanSpeed = lowerPoint.Value.Y;
                    }
                    else
                    {
                        isControlYeilded = false;

                        double ratio = (currentTemp - (ulong)lowerPoint.Value.X) / (double)(upperPoint.Value.X - lowerPoint.Value.X);
                        fanSpeed = (int)(lowerPoint.Value.Y + (upperPoint.Value.Y - lowerPoint.Value.Y) * ratio);


                        // Apply hysteresis to prevent rapid fan speed changes
                        int hysteresis = 2; // Adjust the hysteresis value as needed
                        if (fanSpeed > lastFanSpeed + hysteresis || fanSpeed < lastFanSpeed - hysteresis)
                        {
                            // Update the fan speed
                            fanSpeed = Math.Max(1, Math.Min(100, fanSpeed));
                            setFanSpeed(fanSpeed, true); // Implement the SetFanSpeed method to control the fan speed
                            lastFanSpeed = fanSpeed;
                        }
                    }
                };
                fanCurveTimer.Start();
            }
            else
            {
                // Stop the timer when the fan curve is unchecked
                if (fanCurveTimer != null)
                {
                    fanCurveTimer.Stop();
                    fanCurveTimer.Dispose();
                    fanCurveTimer = null;
                }
            }
        }

        // Keep track of the last fan speed to apply hysteresis
        private int lastFanSpeed = 0;

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (fanControl.Checked)
            {
            setFanSpeed(trackBarFanSpeed.Value, fanCurveControl.Checked);
            }
            
        }


    }

}
