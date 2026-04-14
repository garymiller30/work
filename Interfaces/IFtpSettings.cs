
namespace Interfaces
{
    public interface IFtpSettings
    {
        object Id { get; set; }
        /// <summary>
        /// адрес сервера
        /// </summary>
        string Server { get; set; }
        /// <summary>
        /// название в списке серверов
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// логин
        /// </summary>
        string User { get; set; }
        /// <summary>
        /// используется активный режим
        /// </summary>
        bool IsActive { get; set; }
        /// <summary>
        /// пароль
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// кодировка
        /// </summary>
        int Encode { get; set; }

        string RootFolder { get; set; }
    }
}
