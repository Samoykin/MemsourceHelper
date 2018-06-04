namespace MemsourceHelper
{    
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using NLog;

    /// <summary>Interaction logic for MainWindow.xaml.</summary>
    public partial class MainWindow : Window
    {
        private const string Enru = "en-ru";
        private const string Ruen = "ru-en";
        private const int Quantity = 1000000;

        private bool flag;
        private Logger logger = LogManager.GetCurrentClassLogger();
        private XMLParser parcer;        

        /// <summary>Initializes a new instance of the <see cref="MainWindow" /> class.</summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.logger.Info("Запуск программы Memsource Helper.");
            this.parcer = new XMLParser(this.logger);            
            this.Title = "Memsource Helper v" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        /// <summary>Перевод файла.</summary>
        public async void TranslateFile()
        {
            string lang;
            int cellQuantity;

            if (rbEnRu.IsChecked == true)
            {
                lang = Enru;
            }
            else
            {
                lang = Ruen;
            }

            if (string.IsNullOrEmpty(tbQuantity.Text) || tbQuantity.Text == "0")
            {
                cellQuantity = Quantity;
            }
            else
            {
                cellQuantity = int.Parse(tbQuantity.Text);
            }

            if (this.flag == false)
            {
                tb4.Text = "Идет перевод";
                this.logger.Info("Запущен процесс перевода файла.");
                btnTranslate.IsEnabled = false;
            }

                try
                {
                this.flag = await Task<bool>.Factory.StartNew(() =>
                    {                        
                        return parcer.Parse(lang, cellQuantity);
                    });

                    btnTranslate.IsEnabled = true;
                    if (this.flag == true)
                    {
                        tb4.Text = "Перевод закончен";
                        this.logger.Info("Закончен перевод файла.");
                    }
                    else
                    {
                        tb4.Text = "Ошибка перевода";
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
    }
}