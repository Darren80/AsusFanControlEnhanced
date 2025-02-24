using AsusFanControl.Domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Configuration;
using System.Text;

namespace AsusFanControlGUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Subscribe to unhandled exception events
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // Simulate an unhandled exception on the UI thread
            //throw new InvalidOperationException("Simulated UI thread exception");

            // Configure the DI container
            var serviceProvider = ConfigureServices();

            // Resolve the main form and run the application
            using (var form = serviceProvider.GetRequiredService<Form1>())
            {
                Application.Run(form);
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register domain layer
            services.AddSingleton<FanCurve>();

            // Register application layer

            // Register infrastructure layer

            // Register presentation layer
            services.AddSingleton<Form1>();

            return services.BuildServiceProvider();
        }

        static string GetApplicationSettings()
        {
            StringBuilder settings = new StringBuilder();
            foreach (SettingsProperty property in Properties.Settings.Default.Properties)
            {
                settings.AppendLine($"{property.Name}: {Properties.Settings.Default[property.Name]}");
            }
            return settings.ToString();
        }
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
            {
                ShowErrorDialog(e.Exception);
            }

    static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ShowErrorDialog(e.ExceptionObject as Exception);
    }

    static void ShowErrorDialog(Exception ex)
    {
        // Get the current state of application settings
        string settings = GetApplicationSettings();

        // Build the message
        string message = $"An error occured, please try restarting the application.\n\n" +
                         "If the error persists then create a GitHub issue at:\n" +
                         "https://github.com/Darren80/AsusFanControlEnhanced/issues if one does not already exist \n\n" +
                         $"Stack Trace:\n{ex.StackTrace}\n\n" +
                         $"Application Settings:\n{settings}\n\n" +
                         $"Error Message:\n{ex.Message}\n\n";

        // Create a custom form to display the error message with scroll bars
        using (var form = new Form())
        {
            form.Text = "Unhandled Exception - AsusFanControlEnhanced";
            form.Size = new System.Drawing.Size(600, 400);
            form.StartPosition = FormStartPosition.CenterScreen;
                //form.Icon = Properties.Resources.ResourceManager.GetObject("AppIcon") as System.Drawing.Icon;

            // Add a RichTextBox to display the message with scroll bars
            var textBox = new RichTextBox
            {
                Text = message,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Font = new System.Drawing.Font("Consolas", 9.75f) // Use a monospaced font for better readability
            };

            // Add Restart and Exit buttons
            var restartButton = new Button
            {
                Text = "Restart",
                DialogResult = DialogResult.Yes,
                Dock = DockStyle.Bottom
            };

            var exitButton = new Button
            {
                Text = "Exit",
                DialogResult = DialogResult.No,
                Dock = DockStyle.Bottom
            };

            // Add controls to the form
            form.Controls.Add(textBox);
            form.Controls.Add(restartButton);
            form.Controls.Add(exitButton);

            // Handle button clicks
            form.AcceptButton = restartButton;
            form.CancelButton = exitButton;

            // Show the form and handle the result
            DialogResult result = form.ShowDialog();

            if (result == DialogResult.Yes)
            {
                // Restart the application
                RestartApplication();
            }
            else if (result == DialogResult.No)
            {
                // Exit the application
                Environment.Exit(1);
            }
        }
    }

    static void RestartApplication()
    {
        // Reset application properties
        Properties.Settings.Default.Reset();
        Properties.Settings.Default.Save();

        // Restart the application
        string applicationPath = Application.ExecutablePath;
        System.Diagnostics.Process.Start(applicationPath);
        Environment.Exit(0);
    }
}
}
