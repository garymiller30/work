namespace Job
{
//    public static class CustomerCustomFolderPath
//    {
//        private static List<CustomerCustomFolder> _customerCustomFolders;
//
//        
//
//        private static void Load()
//        {
//            _customerCustomFolders = new List<CustomerCustomFolder>();
//
//
//            if (File.Exists(CustomFoldersFileName))
//            {
//                try
//                {
//                    var ser = File.ReadAllText(CustomFoldersFileName);
//                    if (!string.IsNullOrEmpty(ser))
//                    {
//                        _customerCustomFolders = JsonConvert.DeserializeObject<List<CustomerCustomFolder>>(ser);
//
//                    }
//
//                }
//                catch
//                {
//                    
//                }
//            }
//            
//        }
//
//        private const string CustomFoldersFileName = "CustomerCustomFolder.json";
//
//
//        public static string Get(ObjectId customerId)
//        {
//
//            if (_customerCustomFolders == null) Load();
//
//            var customer = _customerCustomFolders?.FirstOrDefault(x => x.Id == customerId.ToString());
//            if (customer != null)
//            {
//                return string.IsNullOrEmpty(customer.Folder) ? string.Empty : customer.Folder;
//            }
//
//            return string.Empty;
//        }
//
//        public static void Set(ObjectId customerId, string path)
//        {
//            if (_customerCustomFolders == null) Load();
//
//            var customer = _customerCustomFolders.FirstOrDefault(x => x.Id == customerId.ToString());
//            if (customer == null)
//            {
//                var customerAdd = new CustomerCustomFolder() {Id = customerId.ToString(),Folder = path};
//                _customerCustomFolders.Add(customerAdd);
//            }
//            else
//            {
//                customer.Folder = path;
//            }
//
//            Save();
//        }
//
//        private static void Save()
//        {
//            var ser = JsonConvert.SerializeObject(_customerCustomFolders, Formatting.Indented);
//
//            File.WriteAllText(CustomFoldersFileName,ser);
//
//        }
//
/*
///*
//        public static void ForceReload()
//        {
//            Load();
//        }
//*/
//    }

/*
    public class CustomerCustomFolder
    {
        public string Id { get; set; }
        public string Folder { get; set; }
    }
*/
}
