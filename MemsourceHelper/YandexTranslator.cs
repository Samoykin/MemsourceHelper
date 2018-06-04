namespace MemsourceHelper
{
    using System.IO;
    using System.Net;

    /// <summary>Яндекс-переводчик.</summary>
    public class YandexTranslator
    {
        /// <summary>Перевод фраз.</summary>
        /// <param name="s">Строка перевода.</param>
        /// <param name="lang">Направление перевода.</param>
        /// <returns>Переведенный текст.</returns>
        public static string Translate(string s, string lang)
        {
            if (s.Length > 0)
            {
                var request = WebRequest.Create("https://translate.yandex.net/api/v1.5/tr.json/translate?"
                    + "key=trnsl.1.1.20161209T160045Z.8690ab398b294afe.7d2ec8f5ed14fa9b98c9b7faa9d3dbb8f88943dc"
                    + "&text=" + s
                    + "&lang=" + lang);
                var response = request.GetResponse();

                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    string line;
                    if ((line = stream.ReadLine()) != null)
                    {
                        s = line.Substring(line.IndexOf(":[\"") + 3);
                        s = s.Remove(s.Length - 3);
                    }
                }

                return s;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
