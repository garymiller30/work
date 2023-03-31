using System;
using System.Collections.Generic;
using Interfaces;
using MongoDB.Bson;

namespace Job
{
    public sealed class Customer : IWithId, ICustomer
    {
        public bool Show { get; set; } = true;
        public object Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public string OwnerPlateDefault { get; set; }

        private List<PlateFormat> _usedPlateFormats = new List<PlateFormat>();

        /// <summary>
        /// какие форматы пластин использует заказчик
        /// </summary>
        public List<PlateFormat> UsedPlateFormats
        {
            get { return _usedPlateFormats ?? (_usedPlateFormats = new List<PlateFormat>()); }
            set { _usedPlateFormats = value; }
        }

        private bool[] _selectedPlatesIdx = new bool[50];

        [Obsolete]
        public bool[] SelectedPlatesIdx
        {
            get { return _selectedPlatesIdx ?? (_selectedPlatesIdx = new bool[50]); }
            set { _selectedPlatesIdx = value; }
        }

        public PlateFormat[] GetUsedPlateFormats()
        {

            return UsedPlateFormats.ToArray();

            //var l = new List<PlateFormat>();
            //var p = Managers.Forms.GetKnowsFormses();

            //for (int i = 0; i < p.Length;i++ )
            //{
            //	if (_selectedPlatesIdx[i])
            //	{
            //		l.Add(p[i].Format);
            //	}
            //}

            //return l.ToArray();

        }
        [Obsolete]
        public bool UseCustomFolder { get; set; }
        [Obsolete]
        public string CustomFolder { get; set; }

        [Obsolete]
        public string DefaultEmail { get; set; }

        public Customer()
        {
            Id = ObjectId.GenerateNewId();

        }

        private List<string> _ftpServers;
        /// <summary>
        /// используемые фтп сервера
        /// </summary>
        public List<string> FtpServers
        {
            get { return _ftpServers ?? (_ftpServers = new List<string>()); }
            set { _ftpServers = value; }
        }


        #region [ FTP Settings ]

        public bool IsFtpEnable { get; set; }
        [Obsolete]
        public string Server { get; set; }
        [Obsolete]
        public string User { get; set; }
        [Obsolete]
        public string Password { get; set; }
        [Obsolete]
        public string RootDirectory { get; set; }
        [Obsolete]
        public bool ActiveMode { get; set; }
        [Obsolete]
        public int CodePage { get; set; }
        #endregion

        public void Update(Customer customer)
        {
            Name = customer.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is ICustomer o)
            {
                return Name.Equals(o.Name);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static int SortByName(ICustomer x, ICustomer y)
        {
            if (x == null)
            {
                if (y == null)
                    return 0;
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }

        //public void SetCustomFolder(string fSelectedPath)
        //{

        //    CustomFolder = fSelectedPath;
        //    //CustomerCustomFolderPath.Set(Id, fSelectedPath);

        //}

        //public string GetCustomFolder()
        //{
        //    return CustomFolder ?? string.Empty;
        //    //return CustomerCustomFolderPath.Get(Id);
        //}
    }
}
