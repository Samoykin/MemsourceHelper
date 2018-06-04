namespace MemsourceHelper
{
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

        /// <summary>Замена запятой на пробел в числах.</summary>
        /// <param name="text">Текст.</param>
        /// <returns>Отформатированная строка.</returns>
        public static string ReplaceComma(string text)
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

        /// <summary>Замена точки на запятую в числах.</summary>
        /// <param name="text">Текст.</param>
        /// <returns>Отформатированная строка.</returns>
        public static string ReplacePoint(string text)
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
    }
}
