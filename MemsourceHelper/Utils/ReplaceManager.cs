namespace MemsourceHelper
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>Менеджер замены символов.</summary>
    public class ReplaceManager
    {
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

        /// <summary>Преобразование чисел в формат RU.</summary>
        /// <param name="text">Текст.</param>
        /// <returns>Отформатированная строка.</returns>
        public static string RuFormatNumber(string text)
        {
            var styles = NumberStyles.Float | NumberStyles.AllowThousands;

            double number;
            CultureInfo provider;

            if ((styles & NumberStyles.AllowCurrencySymbol) > 0)
            {
                provider = new CultureInfo("en-US");
            }
            else
            {
                provider = CultureInfo.InvariantCulture;
            }

            if (double.TryParse(text, styles, provider, out number))
            {
                text = number.ToString("##,###.00");
            }

            return text;
        }
    }
}
