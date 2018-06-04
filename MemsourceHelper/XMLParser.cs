namespace MemsourceHelper
{
    using System.IO;
    using System.Linq;
    using System.Xml;
    using NLog;    

    /// <summary>Менеджер переводов.</summary>
    public class XMLParser
    {
        private Logger logger;        

        /// <summary>Initializes a new instance of the <see cref="XMLParser" /> class.</summary>
        /// <param name="logger">Логгер.</param>
        public XMLParser(Logger logger)
        {
            this.logger = logger;
        }

        /// <summary>XML парсер.</summary>
        /// <param name="lang">Язык перевода.</param>
        /// <param name="cellQuantity">Количество ячеек.</param>
        /// <returns>Состояние.</returns>
        public bool Parse(string lang, int cellQuantity)
        {
            var filePath = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.mxliff").FirstOrDefault();

            if (!string.IsNullOrEmpty(filePath))
            { 
                var xDoc = new XmlDocument();
                xDoc.Load(filePath);

                foreach (XmlNode node in xDoc.LastChild)
                {
                    cellQuantity -= this.ParseNode(xDoc, node.LastChild, filePath, lang, cellQuantity);
                }

                return true;
            }
            else
            {                
                this.logger.Info("Проект не найден. Поместите проект в папку с программой.");
                return false;
            }
        }

        /// <summary>Распарсить исходный файл.</summary>
        /// <param name="xDoc">Документ.</param>
        /// <param name="node">Узел.</param>
        /// <param name="filepath">Путь к файлу.</param>
        /// <param name="lang">Язык перевода.</param>
        /// <param name="cellQuantity">Количество ячеек.</param>
        /// <returns>Значение.</returns>
        private int ParseNode(XmlDocument xDoc, XmlNode node, string filepath, string lang, int cellQuantity)
        {
            string sourse;
            string target;
            var newTarget = string.Empty;

            var count = 0;

            foreach (XmlNode xnode in node)
            {
                if (count < cellQuantity)
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
                                    var replasedText = ReplaceManager.ReplaceSigns(sourse);
                                    newTarget = YandexTranslator.Translate(replasedText, lang);

                                    newTarget = ReplaceManager.ReplaceComma(newTarget);
                                    newTarget = ReplaceManager.ReplacePoint(newTarget);

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
            this.logger.Info("Перевод закончен.");
            return count;
        }
    }
}
