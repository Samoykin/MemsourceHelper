namespace MemsourceHelper
{
    using System.IO;
    using System.Linq;
    using System.Xml;
    using NLog;
    using Utils;
    using static Model.Shell;    

    /// <summary>Менеджер переводов.</summary>
    public class XMLParser
    {
        private Logger logger;
        private YandexTranslator translator;
        private RootElement settings = new RootElement();

        /// <summary>Initializes a new instance of the <see cref="XMLParser" /> class.</summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="settings">Параметры.</param>
        public XMLParser(Logger logger, RootElement settings)
        {
            this.logger = logger;
            this.settings = settings;
        }

        /// <summary>XML парсер.</summary>
        /// <param name="lang">Язык перевода.</param>
        /// <param name="cellQuantity">Количество ячеек.</param>
        /// <returns>Состояние.</returns>
        public bool Parse(string lang, int cellQuantity)
        {
            var newTarget = string.Empty;
            var count = 0;

            var filePath = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.mxliff").FirstOrDefault();
            this.translator = new YandexTranslator(this.settings.Api.Key);

            if (!string.IsNullOrEmpty(filePath))
            { 
                var xDoc = new XmlDocument();
                xDoc.Load(filePath);

                foreach (XmlNode node in xDoc.GetElementsByTagName("group"))
                {
                    if (count < cellQuantity)
                    {
                        var targetNode = node["trans-unit"];
                        if (targetNode != null)
                        {
                            if (string.IsNullOrEmpty(targetNode["target"].InnerText))
                            {
                                newTarget = this.translator.Translate(ReplaceManager.ReplaceSigns(targetNode["source"].InnerText), lang);

                                if (lang == "en-ru")
                                {
                                    newTarget = ReplaceManager.RuFormatNumber(newTarget);
                                }

                                targetNode["target"].InnerText = newTarget;
                            }
                        }

                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                xDoc.Save(filePath);
                
                return true;
            }
            else
            {                
                this.logger.Info("Проект не найден. Поместите проект в папку с программой.");
                return false;
            }
        }        
    }
}