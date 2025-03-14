﻿using Interfaces;
using MongoDB.Bson;

namespace JobSpace
{
    public class Category :ICategory, IWithId
    {
        public object Id { get; set; } = ObjectId.GenerateNewId();
        public string Name { get;set; }
    }
}
