﻿using System;
using System.Runtime.Serialization;

namespace Felhasznalo_LV_DGV
{
    [Serializable]
    internal class ABKivetel : Exception
    {
        public ABKivetel()
        {
        }

        public ABKivetel(string message) : base(message)
        {
        }

        public ABKivetel(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ABKivetel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}