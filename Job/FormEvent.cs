using System;
using Interfaces;
using MongoDB.Bson;

namespace Job
{
    [Serializable]
    public sealed class FormEvent : IWithId
    {
        public FormEvent()
        {
            Id = ObjectId.GenerateNewId();
        }
        public ObjectId Id { get; set; }
        public DateTime Date { get; set; }
        public FormEventStatus Status { get; set; }
        public int CountForm { get; set; }
        public PlateFormat Format { get; set; }
        public string Description { get; set; }

        internal void Update(FormEvent formEvent)
        {
            Date = formEvent.Date;
            Status = formEvent.Status;
            CountForm = formEvent.CountForm;
            Format = formEvent.Format;
            Description = formEvent.Description;
        }
    }
}
