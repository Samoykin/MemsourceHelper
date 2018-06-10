namespace MemsourceHelper.Model
{
    using System;
    using System.Xml.Serialization;

    /// <summary>Оболочка.</summary>
    public class Shell
    {
        /// <summary>Корневой элемент.</summary>
        [Serializable]
        [XmlRootAttribute("Settings")]
        public class RootElement
        {
            /// <summary>БД MSSQL.</summary>
            public Api Api { get; set; }
        }

        /// <summary>Параметры MSSQL.</summary>
        [Serializable]
        public class Api
        {
            /// <summary>Имя сервера.</summary>
            [XmlAttribute]
            public string Key { get; set; }
        }        
    }
}