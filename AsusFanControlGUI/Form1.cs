using AsusFanControl;
using AsusFanControlGUI.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AsusFanControlGUI
{
    public partial class Form1 : Form
    {
        private readonly Random rnd = new Random();
        readonly AsusControl asusControl = new AsusControl();
        int currentFanSpeed = 0;
        ulong currentTemp = 0;


        Boolean isRunningOnBattery = false;
        Boolean hasPointChanged = false;




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
                startErrorHandler();

                toolStripMenuItemTurnOffControlOnExit.Checked = Properties.Settings.Default.turnOffControlOnExit;
                toolStripMenuItemForbidUnsafeSettings.Checked = Properties.Settings.Default.forbidUnsafeSettings;
                startMinimisedToolStripMenuItem.Checked = Properties.Settings.Default.startMinimised;
                startWithWindowsToolStripMenuItem.Checked = Properties.Settings.Default.startWithWindows;
                trackBarFanSpeed.Value = Properties.Settings.Default.fanSpeed;
                radioButton1.Checked = Properties.Settings.Default.fanControlState == "Off";
                fanControl.Checked = Properties.Settings.Default.fanControlState == "Manual";
                fanCurve.Checked = Properties.Settings.Default.fanControlState == "Curve";
                allowFanCurveSettingViaTextToolStripMenuItem.Checked = Properties.Settings.Default.allowFanCurveSettingViaText;
                numericUpDown1.Value = Properties.Settings.Default.hysteresis;
                numericUpDown2.Value = Properties.Settings.Default.updateSpeed;
                autoSwitchBetweenBatteryProfilesToolStripMenuItem.Checked = Properties.Settings.Default.autoSwitchBetweenBatteryProfiles;
                // Manually trigger events
                radioButton1_CheckedChanged(radioButton1, EventArgs.Empty);
                fanCurve_CheckedChanged(fanCurve, EventArgs.Empty);
                fanControl_CheckedChanged(fanControl, EventArgs.Empty);
                allowFanCurveSettingViaTextToolStripMenuItem_Click(allowFanCurveSettingViaTextToolStripMenuItem, EventArgs.Empty);

                Properties.Settings.Default.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "FanCurvePointsBattery")
                    {
                        textBox1.Text = Properties.Settings.Default.FanCurvePointsBattery;
                    }
                    else if (e.PropertyName == "FanCurvePointsPower")
                    {
                        textBox1.Text = Properties.Settings.Default.FanCurvePointsPower;
                    }
                };

                string powerOrBattery = isRunningOnBattery ? "Battery" : "Power";

                string curvePoints = Properties.Settings.Default["FanCurvePoints" + powerOrBattery].ToString();

                SetFanCurvePoints(curvePoints);

                Timer_Tick();
            }
            else
            {
                // Restart the init function after a short delay
                await System.Threading.Tasks.Task.Delay(20);
                init();
            }

            if (autoSwitchBetweenBatteryProfilesToolStripMenuItem.Checked)
            {
                radioButton2.Enabled = false;
                radioButton4.Enabled = false;
            }
            else
            {
                radioButton2.Enabled = true;
                radioButton4.Enabled = true;
            }
        }

        private async void startErrorHandler()
        {
            int minTemp = 1;
            int maxTemp = 200;
            Console.WriteLine("Running");
            if ((fanCurve.Checked || fanControl.Checked) && (currentTemp < (ulong)minTemp || currentTemp > (ulong)maxTemp))
            {
                // Give it a second chance
                await Task.Delay(1000);
                ulong temp = await Task.Run(() => asusControl.Thermal_Read_Cpu_Temperature());
                if (temp >= (ulong)minTemp && temp < (ulong)maxTemp)
                {
                    return;
                }

                Properties.Settings.Default.wasError = true;
                Properties.Settings.Default.errorMsg = $"CPU temprature were outside of good range at {currentTemp}°C, either something has not loaded properly or CPU sensors are faulty.";
                Properties.Settings.Default.Save();
                Console.WriteLine("Restarting");
                Application.Restart();
                Environment.Exit(0);
            }
        }

        private async void Timer_Tick()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                await Task.Delay(1000);
                Timer_Tick();
                return;
            }


            if (Settings.Default.autoSwitchBetweenBatteryProfiles)
            {
                SaveFanCurvePoints();
                isRunningOnBattery = SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Offline;
                Console.WriteLine("is running on battery: " + isRunningOnBattery);
                if (isRunningOnBattery)
                {

                    radioButton2.Checked = true;
                }
                else
                {
                    radioButton4.Checked = true;

                }
            }



            //Console.WriteLine($"Refreshing {rnd.Next(100)}");
            // Update fan speeds and CPU temperature.
            // Run both tasks concurrently
            Task<string> fanSpeedsTask = Task.Run(() => string.Join(" ", asusControl.GetFanSpeeds()));
            Task<string> cpuTempTask = Task.Run(() => $"{asusControl.Thermal_Read_Cpu_Temperature()}");

            // Wait for both tasks to complete
            await Task.WhenAll(fanSpeedsTask, cpuTempTask);

            // Get the results from the completed tasks
            labelRPM.Text = fanSpeedsTask.Result;
            labelCPUTemp.Text = cpuTempTask.Result;
            currentTemp = (ulong)Decimal.Parse(cpuTempTask.Result);
            pictureBoxFanCurve.Invalidate();
            startErrorHandler();

            await Task.Delay(250);
            Timer_Tick();
        }

        private void AddToStartup()
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name;
            string appPath = Assembly.GetExecutingAssembly().Location;

            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey.SetValue(appName, appPath);
        }

        private void RemoveFromStartup()
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name;

            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey.DeleteValue(appName, false);
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            //    if (Properties.Settings.Default.turnOffControlOnExit)
            //        asusControl.SetFanSpeeds(0);
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

                radioButton2.Enabled = false;
                radioButton4.Enabled = false;

                notifyIcon1.Text = $"AsusFanControlEnhanced - Off";
                setFanSpeed(0, null);
            }
        }

        private async void fanControl_CheckedChanged(object sender, EventArgs e)
        {
            if (fanControl.Checked)
            {
                Properties.Settings.Default.fanControlState = "Manual";
                Properties.Settings.Default.Save();
                trackBarFanSpeed.Enabled = true;

                radioButton2.Enabled = false;
                radioButton4.Enabled = false;

                trackBarSetFanSpeed();
                await Task.Delay(2000);
                fanControl_CheckedChanged(sender, e);
            }
            else
            {
                trackBarFanSpeed.Enabled = false;
            }

        }

        bool firstRun = true;
        private async void setFanSpeed(int value, bool? xyz)
        {
            if (currentFanSpeed == value)
                return;

            await Task.Run(() => asusControl.SetFanSpeeds(value));
            currentFanSpeed = value;

            if (value == 0)
                labelValue.Text = "turned off";
            else
                labelValue.Text = value.ToString() + "% (PWM Fan)";

            if (firstRun)
            {
                await Task.Delay(1000);
                currentFanSpeed = 999999;
                setFanSpeed(value, null);
                firstRun = false;
            }
        }

        private void trackBarSetFanSpeed()
        {
            if (Properties.Settings.Default.forbidUnsafeSettings)
            {
                if (trackBarFanSpeed.Value < 40)
                    trackBarFanSpeed.Value = 40;
                else if (trackBarFanSpeed.Value > 99)
                    trackBarFanSpeed.Value = 99;
            }

            Properties.Settings.Default.fanSpeed = trackBarFanSpeed.Value;
            Properties.Settings.Default.Save();

            Decimal trackBarFanSpeedValue = trackBarFanSpeed.Value;
            label5.Text = trackBarFanSpeedValue.ToString() + "% Fan";
            Console.WriteLine($"Setting speed to: {(int)trackBarFanSpeedValue}");
            label3.Text = $"Setting speed to: {(int)trackBarFanSpeedValue}%";// (Stamp: {rnd.Next(1000)})";
            notifyIcon1.Text = $"AsusFanControlEnhanced - Fan Speed: {(int)trackBarFanSpeedValue}%";
            if ((int)trackBarFanSpeedValue == 0)
                notifyIcon1.Text = $"AsusFanControlEnhanced - Off";

            setFanSpeed((int)trackBarFanSpeedValue, fanControl.Checked);
        }

        private void trackBarFanSpeed_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
                return;

            trackBarSetFanSpeed();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            List<int> fanSpeed = await Task.Run(() => asusControl.GetFanSpeeds());
            labelRPM.Text = string.Join(" ", fanSpeed);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            ulong temp = await Task.Run(() => asusControl.Thermal_Read_Cpu_Temperature());
            currentTemp = temp;
            labelCPUTemp.Text = $"{temp}";
            startErrorHandler();
        }

        // My Code:
        private Point maxPoint;
        private Point minPoint;
        private Dictionary<int, Point> fanCurvePoints = new Dictionary<int, Point>()
        {
            { 1, new Point(20, 40) },
            { 2, new Point(30, 40) },
            { 3, new Point(40, 40) },
            { 4, new Point(50, 40) },
            { 5, new Point(60, 40) },
            { 6, new Point(70, 40) },
            { 7, new Point(80, 40) },
            { 8, new Point(90, 40) },
            { 9, new Point(100, 40) },
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

                // Draw vertical gridlines
                g.DrawLine(Pens.LightGray, x, 40, x, pictureBoxFanCurve.Height - 40);
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

                // Draw horizontal gridlines
                g.DrawLine(Pens.LightGray, 40, y, pictureBoxFanCurve.Width - 40, y);
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
                thickPen.LineJoin = LineJoin.Round;
                g.DrawLines(thickPen, graphPoints);
            }
            // Map currentTemp from range 20-100 to range 40-444
            double mappedValue = 40 + ((currentTemp - 20) / 80.0) * (444 - 40);

            // Ensure mappedValue stays within the range 40-444
            mappedValue = Math.Max(40, Math.Min(444, mappedValue));

            // Calculate the x-coordinate for drawing the line
            int redLineX = (int)mappedValue;

            // Draw the red line using the calculated x-coordinate
            g.DrawLine(Pens.Red, redLineX, 40, redLineX, pictureBoxFanCurve.Height - 40);

        }

        private void pictureBoxFanCurve_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            (int temperature, int fanSpeed) = mousePosition(e.Location);

            if (fanCurvePoints.Count >= 20)
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

            //
            runFanCurve(true, true);
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
            byte minimumTemperature = 20;
            byte maximumTemperature = 105;
            byte minFanSpeed = 1;
            byte maxFanSpeed = 100;
            if (selectedPointId != 0)
            {
                // Get mouse location on grid
                (int temperature, int fanSpeed) = mousePosition(e.Location);

                if (temperature < minimumTemperature || temperature > maximumTemperature)
                {
                    temperature = Math.Max(minimumTemperature, Math.Min(temperature, maximumTemperature));
                }
                if (fanSpeed < minFanSpeed || fanSpeed > maxFanSpeed)
                {
                    fanSpeed = Math.Max(minFanSpeed, Math.Min(fanSpeed, maxFanSpeed));
                }

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
            hasPointChanged = true;
            SaveFanCurvePoints();
            runFanCurve(true, true);

            // fanCurvePoints.ToList().ForEach(point => Console.Write($"ID: {point.Key}, X: {point.Value.X}, Y: {point.Value.Y}"));
            // Console.WriteLine();
        }

        private double Distance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private void fanCurve_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.fanControlState = "Curve";
            Properties.Settings.Default.Save();

            radioButton2.Enabled = true;
            radioButton4.Enabled = true;
            Console.WriteLine(fanCurve.Checked);
            runFanCurve();
        }

        public enum CurveType
        {
            Linear,
            Quadratic,
            Cubic
        }

        double curvatureFactor = 0.5;

        // Fan speed calculation
        double CalculateFanSpeed(double currentTemp)
        {
            // Find the fan curve points that bracket the current temperature
            KeyValuePair<int, Point> lowerPoint = fanCurvePoints.OrderByDescending(p => p.Value.X).FirstOrDefault(p => (ulong)p.Value.X <= currentTemp);
            KeyValuePair<int, Point> upperPoint = fanCurvePoints.OrderBy(p => p.Value.X).FirstOrDefault(p => (ulong)p.Value.X >= currentTemp);

            if (lowerPoint.Key == upperPoint.Key)
            {
                return lowerPoint.Value.Y;
            }

            // Check if the current temperature is within the range of the fan curve points
            if (lowerPoint.Key == 0 || upperPoint.Key == 0)
            {
                // Temperature is outside the range, yield control to the system.
                label3.Text = "Control yeilded to system when temprature is outside range.";
                Console.WriteLine("Temperature is outside the range, yield control to the system.");
                notifyIcon1.Text = $"AsusFanControlEnhanced - Off";
                return 0;
            }
            else
            {

            }

            double ratio = (currentTemp - lowerPoint.Value.X) / (upperPoint.Value.X - lowerPoint.Value.X);
            return lowerPoint.Value.Y + (upperPoint.Value.Y - lowerPoint.Value.Y) * ratio;
        }

        private async void runFanCurve(bool bypassHysteresisCheck = false, bool runOnce = false)
        {
            if (!fanCurve.Checked)
            {
                label3.Text = $"";
                return;
            }
            //Console.WriteLine("Fan Curve, " + (int)numericUpDown2.Value);

            // Read the current temperature
            ulong temp = await Task.Run(() => asusControl.Thermal_Read_Cpu_Temperature()); // Implement the ReadTemperature method to get the current temperature
            currentTemp = temp;
            startErrorHandler();
            //Console.WriteLine("Temp, " + currentTemp);

            double fanSpeed = CalculateFanSpeed(temp);

            // Apply hysteresis to prevent rapid fan speed changes
            int hysteresis = (int)numericUpDown1.Value; // Adjust the hysteresis value as needed
            if ((int)temp > lastTemperature + hysteresis || (int)temp < lastTemperature - hysteresis || fanSpeed < 10 || bypassHysteresisCheck)
            {
                // Update the fan speed
                fanSpeed = Math.Max(0, Math.Min(100, fanSpeed));
                setFanSpeed((int)fanSpeed, true); // Implement the SetFanSpeed method to control the fan speed

                Console.WriteLine($"Set fan speed to {(int)fanSpeed}% {rnd.Next(1000)}, last fan speed = {lastTemperature}");
                if (fanSpeed != 0)
                {
                    label3.Text = $"Set fan speed to {(int)fanSpeed}%, current temp: {temp}°C";// (Stamp: {rnd.Next(1000)})";
                    notifyIcon1.Text = $"AsusFanControlEnhanced - Current Temp: {(int)temp}°C - Fan Speed: {(int)fanSpeed}%";
                }
                lastTemperature = (int)temp;

            }

            if (!runOnce)
            {
                await Task.Delay((int)numericUpDown2.Value);
                runFanCurve();
            }
        }

        // Keep track of the last fan speed to apply hysteresis
        private int lastTemperature = 0;



        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                MinimizeToTray();
            }
        }

        public void MinimizeToTray()
        {
            this.Hide();
            notifyIcon1.Visible = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
            mayShowError();
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
                mayShowError();
            }
        }

        private void SaveFanCurvePoints()
        {
            if (!hasPointChanged) return;

            string fanCurvePointsString = string.Join("-", fanCurvePoints.OrderBy(x => x.Value.X).Select(x => $"{x.Value.X},{x.Value.Y}"));
            string powerOrBattery = isRunningOnBattery ? "Battery" : "Power";

            Console.WriteLine($"Saving Fan Curve Points for {powerOrBattery}: {fanCurvePointsString} |||| {Properties.Settings.Default["FanCurvePoints" + powerOrBattery]}");
            Properties.Settings.Default["FanCurvePoints" + powerOrBattery] = fanCurvePointsString;
            Properties.Settings.Default.Save();
            textBox1.Text = fanCurvePointsString;

            hasPointChanged = false;
            Console.WriteLine($"{powerOrBattery} SAVED");
        }

        private void SetFanCurvePoints(String? fanCurveString)
        {
            if (string.IsNullOrEmpty(fanCurveString))
            {
                return;
            }

            fanCurvePoints = fanCurveString.Split('-')
                .Select((x, index) =>
                {
                    string[] parts = x.Split(',');
                    return new KeyValuePair<int, Point>(index + 1, new Point(int.Parse(parts[0]), int.Parse(parts[1])));
                })
                .ToDictionary(x => x.Key, x => x.Value);

            textBox1.Text = fanCurveString;
            pictureBoxFanCurve.Invalidate();
        }



        private void button4_Click(object sender, EventArgs e)
        {
            string powerOrBattery = isRunningOnBattery ? "Battery" : "Power";

            textBox1.Text = Properties.Settings.Default["FanCurvePoints" + powerOrBattery].ToString();
            SetFanCurvePoints(textBox1.Text);


        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SetFanCurvePoints(textBox1.Text);
                MessageBox.Show("Save successful.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void trackBarFanSpeed_MouseUp(object sender, MouseEventArgs e)
        {
            trackBarSetFanSpeed();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
            {
                return;
            }

            try
            {
                SetFanCurvePoints(textBox1.Text);
                MessageBox.Show("Save successful.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void allowFanCurveSettingViaTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (allowFanCurveSettingViaTextToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.allowFanCurveSettingViaText = true;
                Properties.Settings.Default.Save();
                button3.Enabled = true; textBox1.ReadOnly = false; button4.Enabled = true;
            }
            else
            {
                Properties.Settings.Default.allowFanCurveSettingViaText = false;
                Properties.Settings.Default.Save();
                button3.Enabled = false; textBox1.ReadOnly = true; button4.Enabled = false;
            }
        }

        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(textBox1.Text, textBox1);
        }

        private void startWithWindowsToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            if (startWithWindowsToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.startWithWindows = true;
                Properties.Settings.Default.Save();
                AddToStartup();
            }
            else
            {
                Properties.Settings.Default.startWithWindows = false;
                Properties.Settings.Default.Save();
                RemoveFromStartup();
            }
        }

        private void startMinimisedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (startMinimisedToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.startMinimised = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.startMinimised = false;
                Properties.Settings.Default.Save();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            bool startMinimized = Properties.Settings.Default.startMinimised;
            if (startMinimized)
            {
                MinimizeToTray();
                return;
            }

            mayShowError();
        }

        private void mayShowError()
        {
            Console.WriteLine($"asdad: {Properties.Settings.Default.wasError}");
            if (Properties.Settings.Default.wasError == true)
            {
                MessageBox.Show($"An error caused the application to restart:\n\nError: {Properties.Settings.Default.errorMsg}");
                Properties.Settings.Default.wasError = false;
                Properties.Settings.Default.errorMsg = "";
                Properties.Settings.Default.Save();
            }
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            pictureBoxFanCurve.Invalidate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.turnOffControlOnExit)
                asusControl.SetFanSpeeds(0);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hysteresis = (int)numericUpDown1.Value;
            Properties.Settings.Default.Save();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.updateSpeed = (int)numericUpDown2.Value;
            Properties.Settings.Default.Save();
        }

        private void resetToDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            Application.Restart();
            Environment.Exit(0);
        }

        private void restartApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void trackBarFanSpeed_ValueChanged(object sender, EventArgs e)
        {

        }

        private void trackBarFanSpeed_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Show the tooltip
                toolTip1.Show(trackBarFanSpeed.Value.ToString(), trackBarFanSpeed, 0, -20, 2000);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) // Battery
        {
            Console.WriteLine("radioButton2.Checked: " + radioButton2.Checked);

            if (radioButton2.Checked)
            {

                isRunningOnBattery = true;
                SetFanCurvePoints(Properties.Settings.Default.FanCurvePointsBattery);
            }
            else
            {

                isRunningOnBattery = false;
                SetFanCurvePoints(Properties.Settings.Default.FanCurvePointsPower);

            }

        }


        private void autoSwitchBetweenBatteryProfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (autoSwitchBetweenBatteryProfilesToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.autoSwitchBetweenBatteryProfiles = true;
                Properties.Settings.Default.Save();

                radioButton2.Enabled = false;
                radioButton4.Enabled = false;

            }
            else
            {
                Properties.Settings.Default.autoSwitchBetweenBatteryProfiles = false;
                Properties.Settings.Default.Save();
                radioButton2.Enabled = true;
                radioButton4.Enabled = true;
            }
        }

        //notifyIcon1.BalloonTipText = string.Join(" ", asusControl.GetFanSpeeds()) + $" Temp: {asusControl.Thermal_Read_Cpu_Temperature()}";       }
    }
}
