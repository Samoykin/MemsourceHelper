namespace MemsourceHelper.Utils
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using Model;
    using Newtonsoft.Json;

    /// <summary>Яндекс-переводчик.</summary>
    public class YandexTranslator
    {
        private readonly string key;

        /// <summary>Initializes a new instance of the <see cref="YandexTranslator" /> class.</summary>
        /// <param name="key">Ключ.</param>
        public YandexTranslator(string key)
        {
            this.key = key;
        }

        /// <summary>Перевод фраз.</summary>
        /// <param name="text">Строка перевода.</param>
        /// <param name="lang">Направление перевода.</param>
        /// <returns>Переведенный текст.</returns>
        public string Translate(string text, string lang)
        {
            if (text.Length > 0)
            {
                var request = WebRequest.Create("https://translate.yandex.net/api/v1.5/tr.json/translate?"
                    + "key=" + this.key
                    + "&text=" + text
                    + "&lang=" + lang);
                var response = request.GetResponse();

                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    string line;
                    if ((line = stream.ReadLine()) != null)
                    {
                        text = JsonConvert.DeserializeObject<Translation>(line).Text.Aggregate(string.Empty, (current, str) => current + str);
                    }
                }

                return text;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}