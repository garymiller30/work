using System;
using Interfaces;
using MongoDB.Bson;

namespace JobSpace.CustomerNotify
{
    public sealed class CustomerMailNotify : IWithId, ICustomerMailNotify
    {
        public object Id { get; set; } = new ObjectId();

        public bool Enabled { get; set; } = true;

        public object CustomerId { get; set; }
        public int StatusCode { get; set; }
        public string Tema { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;

    }
}
