using System;
using Interfaces;
using MongoDB.Bson;

namespace JobSpace
{
    [Serializable]
    public sealed class PlateFormat : IPlateFormat
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        /// <summary>
        /// ширина пластины
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// высота пластины
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Id влацельца пластин
        /// </summary>
        public ObjectId OwnerId { get; set; }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }

        public override bool Equals(object obj)
        {
            var o = obj as PlateFormat;

            if (o != null && Width == o.Width && o.Height == Height) return true;

            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        public PlateFormat Duplicate()
        {
            var pf = new PlateFormat
            {
                Width = Width,
                Height = Height,
                OwnerId = OwnerId
            };

            return pf;
        }
    }
}
