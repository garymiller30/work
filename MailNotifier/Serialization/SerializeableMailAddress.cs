using System;
using System.Net.Mail;

namespace MailNotifier.Serialization
{
    [Serializable]
    internal class SerializeableMailAddress
    {
        String User;
        String Host;
        String Address;
        String DisplayName;

        internal static SerializeableMailAddress GetSerializeableMailAddress(MailAddress ma)
        {
            if (ma == null)
                return null;
            SerializeableMailAddress sma = new SerializeableMailAddress();

            sma.User = ma.User;
            sma.Host = ma.Host;
            sma.Address = ma.Address;
            sma.DisplayName = ma.DisplayName;
            return sma;
        }

        internal MailAddress GetMailAddress()
        {
            return new MailAddress(Address, DisplayName);
        }
    }
}
