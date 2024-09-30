namespace JobSpace
{
    public static partial class Settings
    {
        
        public static string Server { get; set; }
        public static string ServerLogin { get; set; }
        public static string ServerPassword { get; set; }


        static Settings()
        {
           
            Server = "ftp.vtsprint.com.ua\\SQLEXPRESS";
            ServerLogin = "sa";
            ServerPassword = "122222222";

        }
    }
}
