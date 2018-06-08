namespace MemsourceHelper.Model
{
    /// <summary>Перевод.</summary>
    public class Translation
    {
        /// <summary>Код.</summary>
        public string Code { get; set; }

        /// <summary>Язык.</summary>
        public string Lang { get; set; }

        /// <summary>Перевод.</summary>
        public string[] Text { get; set; }
    }
}
