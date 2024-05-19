using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsusFanControl;
using AsusFanControlGUI.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AsusFanControlGUI
{
    public partial class Form1 : Form
    {
        private readonly Random rnd = new Random();
        readonly AsusControl asusControl = new AsusControl();
        int fanSpeed = 0;

        public Form1()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            if (Debugger.IsAttached)
                Settings.Default.Reset();

            init();
        }

        private async void init()
        {
            if (IsHandleCreated)
            {
                toolStripMenuItemTurnOffControlOnExit.Checked = Properties.Settings.Default.turnOffControlOnExit;
                toolStripMenuItemForbidUnsafeSettings.Checked = Properties.Settings.Default.forbidUnsafeSettings;
                trackBarFanSpeed.Value = Properties.Settings.Default.fanSpeed;
                radioButton1.Checked = Properties.Settings.Default.fanControlState == "Off";
                fanControl.Checked = Properties.Settings.Default.fanControlState == "Manual";
                fanCurve.Checked = Properties.Settings.Default.fanControlState == "Curve";
                // Manually trigger events
                radioButton1_CheckedChanged(radioButton1, EventArgs.Empty);
                fanCurve_CheckedChanged(fanCurve, EventArgs.Empty);
                fanControl_CheckedChanged(fanControl, EventArgs.Empty);

                Properties.Settings.Default.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "FanCurvePoints")
                    {
                        textBox1.Text = Properties.Settings.Default.FanCurvePoints;
                    }
                };
                SetFanCurvePoints(null);
                //SetFanCurvePoints("20,1;60,1;61,20;70,20;71,30;80,55");
                Timer_Tick();


            }
            else
            {
                // Restart the init function after a short delay
                await System.Threading.Tasks.Task.Delay(20);
                init();
            }
        }

        private async void Timer_Tick()
        {
            if (autoRefresh.Checked && WindowState != FormWindowState.Minimized)
            {
                Console.WriteLine($"Refreshing {rnd.Next(100)}");

                // Update fan speeds and CPU temperature on a separate task
                await Task.Run(() =>
                {
                    // Get fan speeds
                    string fanSpeeds = string.Join(" ", asusControl.GetFanSpeeds());

                    // Get CPU temperature
                    string cpuTemp = $"{asusControl.Thermal_Read_Cpu_Temperature()}";

                    // Update UI on the main thread
                    BeginInvoke(new Action(() =>
                    {
                        labelRPM.Text = fanSpeeds;
                        labelCPUTemp.Text = cpuTemp;
                    }));
                });
                Timer_Tick();
            }
            else
            {
                await Task.Delay(1000).ContinueWith(t => { Timer_Tick(); });
            }
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
            System.Diagnostics.Process.Start("https://github.com/Darren80/AsusFanControlEnhanced/releases");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Properties.Settings.Default.fanControlState = "Off";
                Properties.Settings.Default.Save();
                asusControl.SetFanSpeeds(0);
            }
        }

        private void fanControl_CheckedChanged(object sender, EventArgs e)
        {
            if (fanControl.Checked)
            {
                Properties.Settings.Default.fanControlState = "Manual";
                Properties.Settings.Default.Save();
                trackBarFanSpeed_MouseCaptureChanged(sender, e);
            }

        }

        private void setFanSpeed(int value, bool isTurnedOn)
        {

            Properties.Settings.Default.fanSpeed = value;
            Properties.Settings.Default.Save();

            if (!isTurnedOn)
                value = 0;

            if (value == 0)
                BeginInvoke(new Action(() => labelValue.Text = "turned off"));
            else
                BeginInvoke(new Action(() => labelValue.Text = value.ToString() + "%"));

            if (fanSpeed == value)
                return;

            fanSpeed = value;
            asusControl.SetFanSpeeds(value);
        }

        private async void trackBarFanSpeed_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.forbidUnsafeSettings)
            {
                if (trackBarFanSpeed.Value < 40)
                    trackBarFanSpeed.Value = 40;
                else if (trackBarFanSpeed.Value > 99)
                    trackBarFanSpeed.Value = 99;
            }

            Decimal trackBarFanSpeedValue = trackBarFanSpeed.Value;
            label5.Text = trackBarFanSpeedValue.ToString() + "% Fan";

            await Task.Run(() =>
               setFanSpeed((int)trackBarFanSpeedValue, fanControl.Checked)
            );
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
        private Dictionary<int, Point> fanCurvePoints = new Dictionary<int, Point>()
{
    { 1, new Point(20, 1) },
    { 4, new Point(60, 1) },
    { 5, new Point(61, 20) },
    { 7, new Point(70, 20) },
    { 8, new Point(71, 30) },
    { 9, new Point(80, 55) },
};
        private Timer fanCurveTimer; // Declare the timer as a class-level variable

        // Set up the graph dimensions
        const int padding = 40;

        // Draw the temperature axis and labels
        const int tempMin = 20;
        const int tempMax = 100;
        const int tempInterval = 10;
        private void pictureBoxFanCurve_Paint(object sender, PaintEventArgs e)
        {
            // Get the graphics object to draw on the picture box
            Graphics g = e.Graphics;

            // Calculate the width and height of the graph area
            int graphWidth = pictureBoxFanCurve.Width - 80;
            int graphHeight = pictureBoxFanCurve.Height - 80;

            // Draw the X-axis (temperature axis)
            g.DrawLine(Pens.Black, 40, pictureBoxFanCurve.Height - 40, pictureBoxFanCurve.Width - 40, pictureBoxFanCurve.Height - 40);

            // Draw temperature labels and tick marks on the X-axis
            for (int temp = 20; temp <= 100; temp += 10)
            {
                int x = 40 + (temp - 20) * graphWidth / 80;
                g.DrawLine(Pens.Black, x, pictureBoxFanCurve.Height - 40 - 5, x, pictureBoxFanCurve.Height - 40 + 5);
                g.DrawString(temp.ToString(), Control.DefaultFont, Brushes.Black, x - 10, pictureBoxFanCurve.Height - 40 + 10);
            }

            // Draw the X-axis label (Temperature)
            g.DrawString("Temperature (°C)", Control.DefaultFont, Brushes.Black, pictureBoxFanCurve.Width / 2 - 40, pictureBoxFanCurve.Height - 40 + 20);

            // Draw the Y-axis (fan speed axis)
            g.DrawLine(Pens.Black, 40, 40, 40, pictureBoxFanCurve.Height - 40);

            // Draw fan speed labels and tick marks on the Y-axis
            for (int speed = 0; speed <= 100; speed += 20)
            {
                int y = pictureBoxFanCurve.Height - 40 - speed * graphHeight / 100;
                g.DrawLine(Pens.Black, 35, y, 45, y);
                g.DrawString(speed.ToString(), Control.DefaultFont, Brushes.Black, 5f, y - 10);
            }

            // Draw the Y-axis label (Fan Speed)
            g.DrawString("Fan Speed (%)", Control.DefaultFont, Brushes.Black, 20f, 0f, new StringFormat(StringFormatFlags.DirectionVertical));

            // Draw green dots for each fan curve point
            foreach (Point point in fanCurvePoints.Values)
            {
                int x = 40 + (point.X - 20) * graphWidth / 80;
                int y = pictureBoxFanCurve.Height - 40 - point.Y * graphHeight / 100;
                g.FillEllipse(Brushes.Green, x - 3, y - 3, 12, 12);
            }

            // If there are at least two fan curve points, connect them with a thick black line
            if (fanCurvePoints.Count >= 2)
            {
                Point[] graphPoints = fanCurvePoints.Values
                    .OrderBy(p => p.X)
                    .Select(p => new Point(40 + (p.X - 20) * graphWidth / 80, pictureBoxFanCurve.Height - 40 - p.Y * graphHeight / 100))
                    .ToArray();

                using Pen thickPen = new Pen(Color.Black, 3f);
                g.DrawLines(thickPen, graphPoints);
            }
        }

        private void pictureBoxFanCurve_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            (int temperature, int fanSpeed) = mousePosition(e.Location);

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
            int padding = 40;
            int graphWidth = pictureBoxFanCurve.Width - 2 * padding;
            int graphHeight = pictureBoxFanCurve.Height - 2 * padding;

            // Define the temperature and fan speed ranges
            int tempMin = 20;
            int tempMax = 100;
            int speedMax = 100;

            // Convert the mouse coordinates to graph coordinates
            int temperature = tempMin + (e.X - padding) * (tempMax - tempMin) / graphWidth;
            int fanSpeed = speedMax - (e.Y - padding) * speedMax / graphHeight;

            return (temperature, fanSpeed);
        }

        private KeyValuePair<int, Point> nearestPointToMouse(MouseEventArgs e, int maxDistance)
        {

            (int temperature, int fanSpeed) = mousePosition(e.Location);


            var nearestPoints = fanCurvePoints
                       .OrderBy(p => Distance(p.Value, new Point(temperature, fanSpeed)));

            KeyValuePair<int, Point> reachablePoint = nearestPoints
                .FirstOrDefault(p => Distance(p.Value, new Point(temperature, fanSpeed)) <= maxDistance);

            return reachablePoint;
        }

        private int maxDistance = 8;
        private void pictureBoxFanCurve_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeyValuePair<int, Point> reachablePoint = nearestPointToMouse(e, maxDistance);
                if (reachablePoint.Value != Point.Empty)
                {
                    fanCurvePoints.Remove(reachablePoint.Key);
                    pictureBoxFanCurve.Invalidate();

                    Console.WriteLine($"Point ID: {reachablePoint.Key} deleted.");
                }
            }
        }

        private void pictureBoxFanCurve_MouseDown(object sender, MouseEventArgs e)
        {
            Console.Write("MouseDown: ");

            if (e.Button == MouseButtons.Left)
            {
                // Console.WriteLine(e.Button);
                KeyValuePair<int, Point> reachablePoint = nearestPointToMouse(e, maxDistance);
                if (reachablePoint.Value != Point.Empty)
                {
                    selectedPointId = reachablePoint.Key;
                    //Console.WriteLine($"Distance: {Distance(reachablePoint.Value, new Point(temperature, fanSpeed))}");
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
                // Get mouse location on grid
                (int temperature, int fanSpeed) = mousePosition(e.Location);
                //Update location of point
                fanCurvePoints[selectedPointId] = new Point(temperature, fanSpeed);

                // Show the tooltip with the current X and Y values
                toolTip1.SetToolTip(pictureBoxFanCurve, $"Temperature: {temperature}°C, Fan Speed: {fanSpeed}%");

                // Redraw the graph
                pictureBoxFanCurve.Invalidate();
            }
        }

        private void pictureBoxFanCurve_MouseUp(object sender, MouseEventArgs e)
        {
            selectedPointId = 0;
            toolTip1.SetToolTip(pictureBoxFanCurve, "Fan Curve Graph");
            SaveFanCurvePoints();

            // fanCurvePoints.ToList().ForEach(point => Console.Write($"ID: {point.Key}, X: {point.Value.X}, Y: {point.Value.Y}"));
            // Console.WriteLine();
        }

        private double Distance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        bool isControlYeilded = false;
        private void fanCurve_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.fanControlState = "Curve";
            Properties.Settings.Default.Save();
            if (fanCurve.Checked)
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
                    setFanSpeed(lowerPoint.Value.Y, true); // Implement the SetFanSpeed method to control the fan speed

                    Console.WriteLine($"Set fan speed to {lowerPoint.Value.Y}% {rnd.Next(1000)}, last fan speed = {lastFanSpeed}");
                    // Update UI on the main thread
                    BeginInvoke(new Action(() =>
                    {
                        label3.Text = $"Low: {lowerPoint.Value.X} High: {upperPoint.Value.X}";
                    }));
                    lastFanSpeed = lowerPoint.Value.Y;
                }
                else
                {
                    isControlYeilded = false;

                    double ratio = (currentTemp - (ulong)lowerPoint.Value.X) / (double)(upperPoint.Value.X - lowerPoint.Value.X);
                    fanSpeed = (int)(lowerPoint.Value.Y + (upperPoint.Value.Y - lowerPoint.Value.Y) * ratio);


                    // Apply hysteresis to prevent rapid fan speed changes
                    int hysteresis = (int)numericUpDown1.Value; // Adjust the hysteresis value as needed
                    if (fanSpeed > lastFanSpeed + hysteresis || fanSpeed < lastFanSpeed - hysteresis || fanSpeed < 10)
                    {
                        // Update the fan speed
                        fanSpeed = Math.Max(1, Math.Min(100, fanSpeed));
                        setFanSpeed(fanSpeed, true); // Implement the SetFanSpeed method to control the fan speed

                        Console.WriteLine($"Set fan speed to {fanSpeed}% {rnd.Next(1000)}, last fan speed = {lastFanSpeed}");
                        // Update UI on the main thread
                        BeginInvoke(new Action(() =>
                        {
                            label3.Text = $"Low: {lowerPoint.Value.X} High: {upperPoint.Value.X}";
                        }));
                        lastFanSpeed = fanSpeed;

                    }

                };
                Task.Delay((int)numericUpDown2.Value).ContinueWith(t => { fanCurve_CheckedChanged(null, null); });


            }
        }

        // Keep track of the last fan speed to apply hysteresis
        private int lastFanSpeed = 0;



        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Show();
                WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = false;
            }
        }

        private void SaveFanCurvePoints()
        {
            string fanCurvePointsString = string.Join("-", fanCurvePoints.OrderBy(x => x.Value.X).Select(x => $"{x.Value.X},{x.Value.Y}"));

            Properties.Settings.Default.FanCurvePoints = fanCurvePointsString;
            Properties.Settings.Default.Save();

            Console.WriteLine(fanCurvePointsString);
        }

        private void SetFanCurvePoints(String? fanCurveString)
        {
            int count = 0;
            string fanCurvePointsString = fanCurveString ?? Properties.Settings.Default.FanCurvePoints;
            if (!string.IsNullOrEmpty(fanCurvePointsString))
            {
                Console.WriteLine(fanCurvePointsString);


                // Parse the string
                try
                {
                    fanCurvePoints = fanCurvePointsString.Split('-')
                    .Select(x =>
                    {
                        string[] parts = x.Split(',');
                        return new KeyValuePair<int, Point>(count++, new Point(int.Parse(parts[0]), int.Parse(parts[1])));
                    })
                    .ToDictionary(x => x.Key, x => x.Value);

                    //Save
                    textBox1.Text = fanCurvePointsString;
                    SaveFanCurvePoints();
                }
                catch 
                {
                    MessageBox.Show("Invalid string.");
                }
            }

            pictureBoxFanCurve.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.FanCurvePoints;
            SetFanCurvePoints(null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetFanCurvePoints(textBox1.Text);
        }

        private void trackBarFanSpeed_MouseUp(object sender, MouseEventArgs e)
        {
            trackBarFanSpeed_MouseCaptureChanged(sender, e);
        }
    }

}
