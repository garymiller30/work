using System;
using Interfaces;
using MongoDB.Bson;

namespace JobSpace
{
    [Serializable]
    public sealed class FtpSettings : IWithId, IFtpSettings
    {
        public object Id { get; set; }
        /// <summary>
        /// адрес сервера
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// название в списке серверов
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// логин
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// используется активный режим
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// кодировка
        /// </summary>
        public int Encode { get; set; }

        public string RootFolder { get; set; }
        
        public FtpSettings()
        {
            Id = ObjectId.GenerateNewId();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
