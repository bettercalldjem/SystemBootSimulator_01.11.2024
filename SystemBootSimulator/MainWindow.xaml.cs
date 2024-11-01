using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BootSimulator
{
    public partial class MainWindow : Window
    {
        private Logger logger;

        public MainWindow()
        {
            InitializeComponent();
            logger = new Logger();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            await StartBootSequence();
        }

        private async Task StartBootSequence()
        {
            await InitializeHardware();
            if (!IsHardwareInitialized())
            {
                logger.Log("Инициализация оборудования завершилась с ошибкой.");
                return;
            }

            await LoadDrivers();
            if (!IsDriversLoaded())
            {
                logger.Log("Загрузка драйверов завершилась с ошибкой.");
                return;
            }

            await StartServices();
            logger.Log("Процесс загрузки завершён.");
        }

        private async Task InitializeHardware()
        {
            HardwareInitStatus.Text = "В процессе...";
            for (int i = 0; i <= 100; i += 10)
            {
                HardwareInitProgress.Value = i;
                await Task.Delay(500); 
                if (i == 50 && new Random().Next(2) == 0)
                {
                    HardwareInitStatus.Text = "Ошибка!";
                    logger.Log("Ошибка при инициализации оборудования.");
                    return;
                }
            }
            HardwareInitStatus.Text = "Завершен";
            logger.Log("Инициализация оборудования завершена.");
        }

        private async Task LoadDrivers()
        {
            DriverLoadStatus.Text = "В процессе...";
            for (int i = 0; i <= 100; i += 10)
            {
                DriverLoadProgress.Value = i;
                await Task.Delay(500); 
                if (i == 80 && new Random().Next(2) == 0)
                {
                    DriverLoadStatus.Text = "Ошибка!";
                    logger.Log("Ошибка при загрузке драйверов.");
                    return;
                }
            }
            DriverLoadStatus.Text = "Завершен";
            logger.Log("Загрузка драйверов завершена.");
        }

        private async Task StartServices()
        {
            ServiceStartStatus.Text = "В процессе...";
            for (int i = 0; i <= 100; i += 10)
            {
                ServiceStartProgress.Value = i;
                await Task.Delay(500); 
                if (i == 70 && new Random().Next(2) == 0)
                {
                    ServiceStartStatus.Text = "Ошибка!";
                    logger.Log("Ошибка при запуске служб.");
                    return;
                }
            }
            ServiceStartStatus.Text = "Завершен";
            logger.Log("Запуск служб завершен.");
        }

        private void ViewLogsButton_Click(object sender, RoutedEventArgs e)
        {
            var logWindow = new LogWindow(logger.GetLogs());
            logWindow.Show();
        }

        private bool IsHardwareInitialized() => true; private bool IsDriversLoaded() => true;
    }

    public class Logger
    {
        private List<string> logEntries = new List<string>();

        public void Log(string message)
        {
            logEntries.Add($"{DateTime.Now}: {message}");
        }

        public List<string> GetLogs() => logEntries;
    }

    public class LogWindow : Window
    {
        public LogWindow(List<string> logs)
        {
            Title = "Логи";
            Width = 400;
            Height = 300;

            var logListBox = new ListBox();
            foreach (var log in logs)
            {
                logListBox.Items.Add(log);
            }

            Content = logListBox;
        }
    }
}
