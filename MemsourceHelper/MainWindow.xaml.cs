namespace MemsourceHelper
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Xml;

    /// <summary>Interaction logic for MainWindow.xaml.</summary>
    public partial class MainWindow : Window
    {
        private YandexTranslator translator;
        private bool flag;

        /// <summary>Initializes a new instance of the <see cref="MainWindow" /> class.</summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.translator = new YandexTranslator();
            this.Title = "Memsource Helper v" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        /// <summary>Замена спецсимволов.</summary>
        /// <param name="text">Входная строка.</param>
        /// <returns>Отформатированная строка.</returns>
        public static string ReplaceSigns(string text)
        {
            var newText = text;
            MatchCollection matches = null;

            var re = new Regex(@"\{\d+\}|\[●\]|\{j\}|\{1\>|\<1\}|\{i\>\/|\/\<i\}|\{i\>|\<i\}|\{b\>|\<b\}|\{\d\>|\<\d\}|\{u\>|\<u\}|\{bu\>|\<bu\}|(?:\s(\s+?)|\#)");

            while (re.IsMatch(newText))
            {
                matches = re.Matches(newText);
                foreach (Match textMatch in matches)
                {
                    newText = re.Replace(newText, string.Empty);
                }
            }

            return newText;
        }

        /// <summary>Тестовый метод.</summary>
        /// <param name="text">Текст.</param>
        /// <returns>Строка.</returns>
        public static string RepTest(string text)
        {
            var newText = text;

            MatchCollection matches = null;

            var re = new Regex(@"\d\,\d");

            while (re.IsMatch(newText))
            {
                matches = re.Matches(newText);
                foreach (Match textMatch in matches)
                {
                    var oldStr = textMatch.ToString();
                    var newStr = oldStr.Replace(",", " ");
                    
                    if (text.IndexOf(oldStr) != -1)
                    {
                        newText = newText.Replace(oldStr, newStr);
                    }
                }
            }

            return newText;
        }

        /// <summary>Тестовый метод2.</summary>
        /// <param name="text">Текст.</param>
        /// <returns>Строка.</returns>
        public static string RepTest2(string text)
        {
            var newText = text;

            MatchCollection matches = null;

            var re = new Regex(@"\d\.\d");

            while (re.IsMatch(newText))
            {
                matches = re.Matches(newText);
                foreach (Match textMatch in matches)
                {
                    var oldStr = textMatch.ToString();
                    var newStr = oldStr.Replace(".", ",");

                    if (text.IndexOf(oldStr) != -1)
                    {
                        newText = newText.Replace(oldStr, newStr);
                    }
                }
            }

            return newText;
        }
        
        /// <summary>Перевод файла.</summary>
        public async void TranslateFile()
        {
            string lang;
            int cellQuantity;

            if (rbEnRu.IsChecked == true)
            {
                lang = "en-ru";
            }
            else
            {
                lang = "ru-en";
            }

            if (string.IsNullOrEmpty(tbQuantity.Text) || tbQuantity.Text == "0")
            {
                cellQuantity = 1000000;
            }
            else
            {
                cellQuantity = int.Parse(tbQuantity.Text);
            }

            if (this.flag == false)
            {
                tb4.Text = "Идет перевод";
            }

                try
                {
                this.flag = await Task<bool>.Factory.StartNew(() =>
                    {
                        return XMLParse(lang, cellQuantity);
                    });

                    if (this.flag == true)
                    {
                        tb4.Text = "Перевод закончен";
                    }
                    else
                    {
                        tb4.Text = "Ошибка перевода";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка." + ex);                    
                }      
        }

        /// <summary>XML парсер.</summary>
        /// <param name="lang">Язык перевода.</param>
        /// <param name="cellQuantity">Количество ячеек.</param>
        /// <returns>Состояние.</returns>
        public bool XMLParse(string lang, int cellQuantity)
        {
            var parseFlag = false;            

            string currentDir;
            string[] fileList = new string[6];

            currentDir = Directory.GetCurrentDirectory();
            fileList = Directory.GetFiles(currentDir, "*.mxliff");

            if (fileList[0].Length > 0)
            {
                var filepath = fileList[0];
                
                var xDoc = new XmlDocument();
                xDoc.Load(filepath);

                foreach (XmlNode n2 in xDoc.LastChild)
                {
                    var n3 = n2.LastChild; // body
                    var count = 0;

                    count = this.ParseFile(xDoc, n3, filepath, lang, cellQuantity);

                    cellQuantity -= count;
                }

                parseFlag = true;
                }
                else
                {
                    MessageBox.Show("Проект не найден. Поместите проект в папку с программой.");
                }

            return parseFlag;
        }

        /// <summary>Распарсить исходный файл.</summary>
        /// <param name="xDoc">Документ.</param>
        /// <param name="n3">Узел.</param>
        /// <param name="filepath">Путь к файлу.</param>
        /// <param name="lang">Язык перевода.</param>
        /// <param name="cellQuantityT">Количество ячеек.</param>
        /// <returns>Значение.</returns>
        public int ParseFile(XmlDocument xDoc, XmlNode n3, string filepath, string lang, int cellQuantityT)
        {
            string sourse;
            string target;
            var newTarget = string.Empty;

            var count = 0;

            foreach (XmlNode xnode in n3)
            {
                if (count < cellQuantityT)
                {
                    // получаем атрибут name
                    if (xnode.Attributes.Count > 0)
                    {
                        var attr = xnode.Attributes.GetNamedItem("id");

                        if (attr != null)
                        {
                            var nn = xnode["trans-unit"];
                            if (nn != null)
                            {
                                sourse = nn["source"].InnerText;
                                target = nn["target"].InnerText;

                                if (target.Length == 0)
                                {
                                    var replasedText = ReplaceSigns(sourse);
                                    newTarget = this.translator.Translate(replasedText, lang);

                                    newTarget = RepTest(newTarget);
                                    newTarget = RepTest2(newTarget);
                                    nn["target"].InnerText = newTarget;
                                }
                            }
                        }
                    }

                    count++;
                }
                else
                {
                    break;
                }
            }

            xDoc.Save(filepath);
            return count;
        }              

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.TranslateFile();
        }
    }
}