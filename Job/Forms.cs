using System;
using System.Runtime.Serialization;
using Interfaces;
using MongoDB.Bson;

namespace Job
{
    [DataContract]
    [Serializable]
    public sealed class Forms : IWithId
    {
        private int _komplekts;
        private int _count;
        private int _brak;
        private PlateFormat _format = new PlateFormat();
        private int _plotted;
        private int _plottedSaved;
        private int _brakSaved;
        [Obsolete]
        private String _owner;

        public ObjectId Id { get; set; }

        public bool IsSelected { get; set; }

        public bool UsePlottedSave { get; set; }

        public Forms()
        {
            _komplekts = 1;
            Id = ObjectId.GenerateNewId();
        }

        public Forms(int width, int height)
        {
            _format.Width = width;
            _format.Height = height;

            Id = ObjectId.GenerateNewId();
        }

        public int Komplekts
        {
            get { return _komplekts; }
            set { _komplekts = value; }
        }
        /// <summary>
        /// заплановано форм
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        /// <summary>
        /// власник форм
        /// </summary>
        /// 

        [Obsolete]
        public string Owner
        {
            get { return _owner ?? string.Empty; }
            set { _owner = value; }
        }

        public int Brak
        {
            get { return _brak; }
            set { _brak = value; }
        }
        /// <summary>
        /// намальовано форм
        /// </summary>
        public int Plotted
        {
            get { return _plotted; }
            set { _plotted = value; }
        }

        public PlateFormat Format
        {
            get { return _format; }
            set { _format = value; }
        }

        public int Unplotted => _count*_komplekts - _plotted + _brak;
            
        public void SavePlotted()
        {
            //UsePlottedSave = true;
            _plottedSaved = _plotted;
            _brakSaved = _brak;
        }

        public int GetPlottedDelta()
        {
            return _plotted - _plottedSaved - _brakSaved;
        }

        public bool IsPlotted => _plotted - _count * _komplekts == 0;

/*
        public bool Equals(Forms forms, string customer)
        {
            if (forms.Format.Equals(Format))
            {
                if (forms.Format.OwnerId.Equals(customer))
                    return true;
            }
            return false;
        }
*/

        public override bool Equals(object obj)
        {
            var f = obj as Forms;
            if (f != null)
            {
                if (f.Format.Equals(Format))
                {
                    if (f.Format.OwnerId.Equals(Format.OwnerId))
                        return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode($"{Format}{Format.OwnerId}");
        }

        public override string ToString()
        {
            return Format.ToString();
        }

        public void Update(Forms forms)
        {
            Count = forms.Count;
            Format = forms.Format;
           // Owner = forms.Owner;
        }
    }
}
