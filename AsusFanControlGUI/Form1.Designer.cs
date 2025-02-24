namespace AsusFanControlGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.trackBarFanSpeed = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RefreshFanRPM = new System.Windows.Forms.Button();
            this.labelRPM = new System.Windows.Forms.Label();
            this.labelCPUTemp = new System.Windows.Forms.Label();
            this.RefreshCPUTemp = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemForbidUnsafeSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.allowFanCurveSettingViaTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startupSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startMinimisedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startWithWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToDefaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBoxFanCurve = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.fanControlRadioButton = new System.Windows.Forms.RadioButton();
            this.fanCurveRadioButton = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.ResetCurvePoints = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label3 = new System.Windows.Forms.Label();
            this.CurvePointsTextbox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.InvisibleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFanSpeed)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFanCurve)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarFanSpeed
            // 
            this.trackBarFanSpeed.Enabled = false;
            this.trackBarFanSpeed.Location = new System.Drawing.Point(17, 96);
            this.trackBarFanSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarFanSpeed.Maximum = 100;
            this.trackBarFanSpeed.Name = "trackBarFanSpeed";
            this.trackBarFanSpeed.Size = new System.Drawing.Size(519, 56);
            this.trackBarFanSpeed.TabIndex = 0;
            this.trackBarFanSpeed.Value = 50;
            this.trackBarFanSpeed.ValueChanged += new System.EventHandler(this.trackBarFanSpeed_ValueChanged);
            this.trackBarFanSpeed.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trackBarFanSpeed_KeyUp);
            this.trackBarFanSpeed.MouseMove += new System.Windows.Forms.MouseEventHandler(this.trackBarFanSpeed_MouseMove);
            this.trackBarFanSpeed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarFanSpeed_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 629);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current value:";
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(118, 629);
            this.labelValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(11, 16);
            this.labelValue.TabIndex = 2;
            this.labelValue.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 661);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Current RPM:";
            // 
            // RefreshFanRPM
            // 
            this.RefreshFanRPM.Location = new System.Drawing.Point(13, 655);
            this.RefreshFanRPM.Margin = new System.Windows.Forms.Padding(4);
            this.RefreshFanRPM.Name = "RefreshFanRPM";
            this.RefreshFanRPM.Size = new System.Drawing.Size(29, 28);
            this.RefreshFanRPM.TabIndex = 4;
            this.RefreshFanRPM.Text = "↻";
            this.RefreshFanRPM.UseVisualStyleBackColor = true;
            this.RefreshFanRPM.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelRPM
            // 
            this.labelRPM.AutoSize = true;
            this.labelRPM.Location = new System.Drawing.Point(153, 661);
            this.labelRPM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRPM.Name = "labelRPM";
            this.labelRPM.Size = new System.Drawing.Size(11, 16);
            this.labelRPM.TabIndex = 5;
            this.labelRPM.Text = "-";
            // 
            // labelCPUTemp
            // 
            this.labelCPUTemp.AutoSize = true;
            this.labelCPUTemp.Location = new System.Drawing.Point(185, 697);
            this.labelCPUTemp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCPUTemp.Name = "labelCPUTemp";
            this.labelCPUTemp.Size = new System.Drawing.Size(11, 16);
            this.labelCPUTemp.TabIndex = 9;
            this.labelCPUTemp.Text = "-";
            // 
            // RefreshCPUTemp
            // 
            this.RefreshCPUTemp.Location = new System.Drawing.Point(13, 691);
            this.RefreshCPUTemp.Margin = new System.Windows.Forms.Padding(4);
            this.RefreshCPUTemp.Name = "RefreshCPUTemp";
            this.RefreshCPUTemp.Size = new System.Drawing.Size(29, 28);
            this.RefreshCPUTemp.TabIndex = 8;
            this.RefreshCPUTemp.Text = "↻";
            this.RefreshCPUTemp.UseVisualStyleBackColor = true;
            this.RefreshCPUTemp.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 697);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Current CPU temp:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.optionsToolStripMenuItem,
            this.toolStripMenuItemCheckForUpdates,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(685, 28);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemForbidUnsafeSettings,
            this.allowFanCurveSettingViaTextToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(89, 24);
            this.toolStripMenuItem1.Text = "Advanced";
            // 
            // toolStripMenuItemForbidUnsafeSettings
            // 
            this.toolStripMenuItemForbidUnsafeSettings.CheckOnClick = true;
            this.toolStripMenuItemForbidUnsafeSettings.Name = "toolStripMenuItemForbidUnsafeSettings";
            this.toolStripMenuItemForbidUnsafeSettings.Size = new System.Drawing.Size(293, 26);
            this.toolStripMenuItemForbidUnsafeSettings.Text = "Forbid unsafe settings";
            this.toolStripMenuItemForbidUnsafeSettings.CheckedChanged += new System.EventHandler(this.toolStripMenuItemForbidUnsafeSettings_CheckedChanged);
            // 
            // allowFanCurveSettingViaTextToolStripMenuItem
            // 
            this.allowFanCurveSettingViaTextToolStripMenuItem.CheckOnClick = true;
            this.allowFanCurveSettingViaTextToolStripMenuItem.Name = "allowFanCurveSettingViaTextToolStripMenuItem";
            this.allowFanCurveSettingViaTextToolStripMenuItem.Size = new System.Drawing.Size(293, 26);
            this.allowFanCurveSettingViaTextToolStripMenuItem.Text = "Allow FanCurve Seting via Text";
            this.allowFanCurveSettingViaTextToolStripMenuItem.Click += new System.EventHandler(this.allowFanCurveSettingViaTextToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startupSettingsToolStripMenuItem,
            this.restartApplicationToolStripMenuItem,
            this.resetToDefaultsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // startupSettingsToolStripMenuItem
            // 
            this.startupSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startMinimisedToolStripMenuItem,
            this.startWithWindowsToolStripMenuItem});
            this.startupSettingsToolStripMenuItem.Name = "startupSettingsToolStripMenuItem";
            this.startupSettingsToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.startupSettingsToolStripMenuItem.Text = "Startup settings";
            // 
            // startMinimisedToolStripMenuItem
            // 
            this.startMinimisedToolStripMenuItem.CheckOnClick = true;
            this.startMinimisedToolStripMenuItem.Name = "startMinimisedToolStripMenuItem";
            this.startMinimisedToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.startMinimisedToolStripMenuItem.Text = "Start Minimised";
            this.startMinimisedToolStripMenuItem.Click += new System.EventHandler(this.startMinimisedToolStripMenuItem_Click);
            // 
            // startWithWindowsToolStripMenuItem
            // 
            this.startWithWindowsToolStripMenuItem.CheckOnClick = true;
            this.startWithWindowsToolStripMenuItem.Name = "startWithWindowsToolStripMenuItem";
            this.startWithWindowsToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.startWithWindowsToolStripMenuItem.Text = "Start with Windows";
            this.startWithWindowsToolStripMenuItem.Click += new System.EventHandler(this.startWithWindowsToolStripMenuItem1_Click_1);
            // 
            // restartApplicationToolStripMenuItem
            // 
            this.restartApplicationToolStripMenuItem.Name = "restartApplicationToolStripMenuItem";
            this.restartApplicationToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.restartApplicationToolStripMenuItem.Text = "Restart application";
            this.restartApplicationToolStripMenuItem.Click += new System.EventHandler(this.restartApplicationToolStripMenuItem_Click);
            // 
            // resetToDefaultsToolStripMenuItem
            // 
            this.resetToDefaultsToolStripMenuItem.Name = "resetToDefaultsToolStripMenuItem";
            this.resetToDefaultsToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.resetToDefaultsToolStripMenuItem.Text = "Reset to defaults";
            this.resetToDefaultsToolStripMenuItem.Click += new System.EventHandler(this.resetToDefaultsToolStripMenuItem_Click);
            // 
            // toolStripMenuItemCheckForUpdates
            // 
            this.toolStripMenuItemCheckForUpdates.Name = "toolStripMenuItemCheckForUpdates";
            this.toolStripMenuItemCheckForUpdates.Size = new System.Drawing.Size(142, 24);
            this.toolStripMenuItemCheckForUpdates.Text = "Check for updates";
            this.toolStripMenuItemCheckForUpdates.Click += new System.EventHandler(this.toolStripMenuItemCheckForUpdates_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(475, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "XX% Fan";
            // 
            // pictureBoxFanCurve
            // 
            this.pictureBoxFanCurve.Location = new System.Drawing.Point(13, 225);
            this.pictureBoxFanCurve.Name = "pictureBoxFanCurve";
            this.pictureBoxFanCurve.Size = new System.Drawing.Size(645, 312);
            this.pictureBoxFanCurve.TabIndex = 0;
            this.pictureBoxFanCurve.TabStop = false;
            this.pictureBoxFanCurve.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxFanCurve_Paint);
            this.pictureBoxFanCurve.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFanCurve_MouseClick);
            this.pictureBoxFanCurve.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFanCurve_MouseDoubleClick);
            this.pictureBoxFanCurve.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFanCurve_MouseDown);
            this.pictureBoxFanCurve.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFanCurve_MouseMove);
            this.pictureBoxFanCurve.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFanCurve_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.fanControlRadioButton);
            this.groupBox1.Controls.Add(this.fanCurveRadioButton);
            this.groupBox1.Controls.Add(this.trackBarFanSpeed);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(644, 182);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(17, 27);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(72, 20);
            this.radioButton1.TabIndex = 16;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Turn off";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // fanControlRadioButton
            // 
            this.fanControlRadioButton.AutoSize = true;
            this.fanControlRadioButton.Location = new System.Drawing.Point(17, 67);
            this.fanControlRadioButton.Name = "fanControlRadioButton";
            this.fanControlRadioButton.Size = new System.Drawing.Size(137, 20);
            this.fanControlRadioButton.TabIndex = 15;
            this.fanControlRadioButton.Text = "Turn on fan control";
            this.fanControlRadioButton.UseVisualStyleBackColor = true;
            this.fanControlRadioButton.CheckedChanged += new System.EventHandler(this.fanControl_CheckedChanged);
            // 
            // fanCurveRadioButton
            // 
            this.fanCurveRadioButton.AutoSize = true;
            this.fanCurveRadioButton.Location = new System.Drawing.Point(17, 150);
            this.fanCurveRadioButton.Name = "fanCurveRadioButton";
            this.fanCurveRadioButton.Size = new System.Drawing.Size(130, 20);
            this.fanCurveRadioButton.TabIndex = 14;
            this.fanCurveRadioButton.Text = "Turn on fan curve";
            this.fanCurveRadioButton.UseVisualStyleBackColor = true;
            this.fanCurveRadioButton.CheckedChanged += new System.EventHandler(this.fanCurve_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(623, 552);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(33, 27);
            this.button3.TabIndex = 22;
            this.button3.Text = "S";
            this.toolTip1.SetToolTip(this.button3, "Set the fancurve based on the entered string.");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ResetCurvePoints
            // 
            this.ResetCurvePoints.Location = new System.Drawing.Point(13, 552);
            this.ResetCurvePoints.Name = "ResetCurvePoints";
            this.ResetCurvePoints.Size = new System.Drawing.Size(35, 27);
            this.ResetCurvePoints.TabIndex = 23;
            this.ResetCurvePoints.Text = "R";
            this.toolTip1.SetToolTip(this.ResetCurvePoints, "Reset");
            this.ResetCurvePoints.UseVisualStyleBackColor = true;
            this.ResetCurvePoints.Click += new System.EventHandler(this.button4_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "AsusFanControlEnhanced";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(115, 52);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 24);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 24);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Hysteresis:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(139, 23);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(41, 22);
            this.numericUpDown1.TabIndex = 18;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.numericUpDown2);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(442, 629);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(214, 95);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Advanced";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(183, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 16);
            this.label9.TabIndex = 24;
            this.label9.Text = "s";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(183, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 16);
            this.label8.TabIndex = 23;
            this.label8.Text = "ms";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDown2.Location = new System.Drawing.Point(112, 56);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(68, 22);
            this.numericUpDown2.TabIndex = 22;
            this.numericUpDown2.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 16);
            this.label7.TabIndex = 21;
            this.label7.Text = "Update Speed:";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 755);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(645, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = ".";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CurvePointsTextbox
            // 
            this.CurvePointsTextbox.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurvePointsTextbox.Location = new System.Drawing.Point(53, 552);
            this.CurvePointsTextbox.Name = "CurvePointsTextbox";
            this.CurvePointsTextbox.ReadOnly = true;
            this.CurvePointsTextbox.Size = new System.Drawing.Size(564, 27);
            this.CurvePointsTextbox.TabIndex = 21;
            this.CurvePointsTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.CurvePointsTextbox.MouseHover += new System.EventHandler(this.textBox1_MouseHover);
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Location = new System.Drawing.Point(12, 740);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(645, 2);
            this.label10.TabIndex = 24;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Location = new System.Drawing.Point(15, 605);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(645, 2);
            this.label11.TabIndex = 25;
            // 
            // InvisibleLabel
            // 
            this.InvisibleLabel.AutoSize = true;
            this.InvisibleLabel.Location = new System.Drawing.Point(103, 225);
            this.InvisibleLabel.Name = "InvisibleLabel";
            this.InvisibleLabel.Size = new System.Drawing.Size(56, 16);
            this.InvisibleLabel.TabIndex = 26;
            this.InvisibleLabel.Text = "Invisible";
            this.InvisibleLabel.Visible = false;
            this.InvisibleLabel.Click += new System.EventHandler(this.InvisibleLabel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 786);
            this.Controls.Add(this.InvisibleLabel);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ResetCurvePoints);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.CurvePointsTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pictureBoxFanCurve);
            this.Controls.Add(this.labelCPUTemp);
            this.Controls.Add(this.RefreshCPUTemp);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelRPM);
            this.Controls.Add(this.RefreshFanRPM);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "AsusFanControlEnhanced";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFanSpeed)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFanCurve)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBarFanSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button RefreshFanRPM;
        private System.Windows.Forms.Label labelRPM;
        private System.Windows.Forms.Label labelCPUTemp;
        private System.Windows.Forms.Button RefreshCPUTemp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemForbidUnsafeSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCheckForUpdates;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBoxFanCurve;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton fanCurveRadioButton;
        private System.Windows.Forms.RadioButton fanControlRadioButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.TextBox CurvePointsTextbox;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button ResetCurvePoints;
        private System.Windows.Forms.ToolStripMenuItem allowFanCurveSettingViaTextToolStripMenuItem;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startupSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startMinimisedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startWithWindowsToolStripMenuItem;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolStripMenuItem restartApplicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToDefaultsToolStripMenuItem;
        private System.Windows.Forms.Label InvisibleLabel;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

