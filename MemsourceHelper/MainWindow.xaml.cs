namespace MemsourceHelper
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using NLog;
    using Utils;
    using static Model.Shell;
    
    /// <inheritdoc />
    /// <summary>Interaction logic for MainWindow.xaml.</summary>
    public partial class MainWindow : Window
    {
        private const string SettingsPath = "Settings.xml";
        private const string Enru = "en-ru";
        private const string Ruen = "ru-en";
        private const int Quantity = 1000000;

        private bool flag;
        private Logger logger = LogManager.GetCurrentClassLogger();
        private XmlParser parcer;
        private RootElement settings = new RootElement(); // Конфигурация 

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:MemsourceHelper.MainWindow" /> class.</summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.logger.Info("Запуск программы Memsource Helper.");
                       
            this.Title = $"Memsource Helper v{Assembly.GetExecutingAssembly().GetName().Version}";

            // Вычитывание параметров из XML
            // Инициализация модели настроек
            var settingsXml = new SettingsXml<RootElement>(SettingsPath);
            this.settings.Api = new Api();

            if (!File.Exists(SettingsPath))
            {
                this.settings = this.SetDefaultValue(this.settings); // Значения по умолчанию
                settingsXml.WriteXml(this.settings);
            }
            else
            {
                this.settings = settingsXml.ReadXml(this.settings);
            }

            this.parcer = new XmlParser(this.logger, this.settings);
        }

        /// <summary>Перевод файла.</summary>
        private async void TranslateFile()
        {
            int cellQuantity;

            var lang = rbEnRu.IsChecked == true ? Enru : Ruen;

            if (string.IsNullOrEmpty(tbQuantity.Text) || tbQuantity.Text == "0")
            {
                cellQuantity = Quantity;
            }
            else
            {
                cellQuantity = int.Parse(tbQuantity.Text);
            }

            if (!this.flag)
            {
                tbStatus.Text = "Идет перевод";
                this.logger.Info("Запущен процесс перевода файла.");
                btnTranslate.IsEnabled = false;
            }

                try
                {
                    this.flag = await Task<bool>.Factory.StartNew(() => this.parcer.Parse(lang, cellQuantity));

                    btnTranslate.IsEnabled = true;
                    if (this.flag)
                    {
                        tbStatus.Text = "Перевод закончен";
                        this.logger.Info("Закончен перевод файла.");
                    }
                    else
                    {
                        tbStatus.Text = "Ошибка перевода";
                    }
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex.Message);                 
                }      
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.TranslateFile();
        }

        private RootElement SetDefaultValue(RootElement set)
        {
            set.Api.Key = "trnsl.1.1.20161209T160045Z.8690ab398b294afe.7d2ec8f5ed14fa9b98c9b7faa9d3dbb8f88943dc";            

            return set;
        }
    }
}