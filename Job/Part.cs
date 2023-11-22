using System;
using Interfaces;
using MongoDB.Bson;

namespace Job
{
    
    [Serializable]
    public sealed class Part : IPart
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public DateTime FinishTime { get; set; }


        private Forms _forms;
        public Forms Form
        {
            get => _forms ?? (_forms = new Forms());
            set => _forms = value;
        }

        public bool PrintOnSide { get; set; }
        public string AnotherFirm { get; set; }

        public Part()
        {
            Name = string.Empty;
            Status = string.Empty;
            Id = ObjectId.GenerateNewId();
        }
        /// <summary>
        /// бумажный пруф
        /// </summary>
        public bool PaperProof { get; set; }
        public override string ToString()
        {
            if (Status != string.Empty)
            {
                return $"[{Name} - {Status}]";
            }
            return _forms.IsPlotted ? string.Empty : $"[{Name}]";
        }


        public int GetCountUnplottedForms(Forms forms)
        {
            //Form.Owner = forms.Owner;
            return forms.Equals(Form) ? Form.Count * Form.Komplekts - Form.Plotted + Form.Brak : 0;
        }

        public int GetCountPlottedForms(PlateFormat plateFormat)
        {
            return plateFormat.ToString().Equals(Form.Format.ToString()) ? Form.Plotted : 0;
        }

        public int GetCountPlottedForms(int year, int month, PlateFormat plateFormat)
        {
            if (FinishTime.Year == year && FinishTime.Month == month &&
                plateFormat.ToString().Equals(Form.Format.ToString()))
                return Form.Plotted - Form.Brak;

            return 0;

            
        }

        public int GetCountPlottedForms(int year, DateTime date, PlateFormat plateFormat,ObjectId owner)
        {
            if (FinishTime.Year == year && FinishTime.Month == date.Month && FinishTime.Day == date.Day &&
                plateFormat.ToString().Equals(Form.Format.ToString()) && owner == Form.Format.OwnerId)
                return Form.Plotted - Form.Brak;

            return 0;

            
        }


        public int GetPlottedDelta(PlateFormat plateFormat)
        {
            return plateFormat.ToString().Equals(Form.Format.ToString()) ? Form.GetPlottedDelta() : 0;
        }

        public bool IsComplete
        {

            get
            {
                if (Form.Count == 0) return false;
                return Form.Plotted - Form.Brak - Form.Count * Form.Komplekts == 0;
            }

        }

        public void Complete()
        {
            Form.Plotted = Form.Count;
            FinishTime = DateTime.Now;
        }
    }
}
