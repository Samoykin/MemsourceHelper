namespace MemsourceHelper.Utils
{
    using System.Globalization;
    using System.Linq;
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
            var re = new Regex(@"\{\d+\}|\[●\]|\{j\}|\{1\>|\<1\}|\{i\>\/|\/\<i\}|\{i\>|\<i\}|\{b\>|\<b\}|\{\d\>|\<\d\}|\{u\>|\<u\}|\{bu\>|\<bu\}|(?:\s(\s+?)|\#)");

            while (re.IsMatch(newText))
            {
                var matches = re.Matches(newText);
                newText = matches.Cast<Match>().Aggregate(newText, (current, textMatch) => re.Replace(current, string.Empty));
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
            var provider = (styles & NumberStyles.AllowCurrencySymbol) > 0 ? new CultureInfo("en-US") : CultureInfo.InvariantCulture;

            if (double.TryParse(text, styles, provider, out number))
            {
                text = number.ToString("##,###.00");
            }

            return text;
        }
    }
}
