using System;
using Interfaces.MQ;
using MongoDB.Bson;

namespace Interfaces
{
    public interface IMQManager
    {
        //event EventHandler<ObjectId> OnJobAdd;
        //event EventHandler<ObjectId> OnJobBeginEdit;
        //event EventHandler<ObjectId> OnJobFinishEdit;
        //event EventHandler<ObjectId> OnJobChanged;
        //event EventHandler<ObjectId> OnPlateChanged;
        //event EventHandler<ObjectId> OnPlateAdd;
        //event EventHandler<ObjectId> OnPlateRemove;
        //event EventHandler<ObjectId> OnCustomerAdd;
        //event EventHandler<ObjectId> OnCustomerRemove;
        //event EventHandler<ObjectId> OnCustomerChanged;
        //event EventHandler<ObjectId> OnPlateEventAdd;
        //event EventHandler<ObjectId> OnPlateEventRemove;
        //event EventHandler<ObjectId> OnPlateEventChange;

        //void PublishChanges(MessageEnum me, ObjectId id);

        void Disconnect();

        bool IsOnline();

    }
}
