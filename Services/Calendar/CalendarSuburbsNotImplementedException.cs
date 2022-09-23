using System;
using System.Runtime.Serialization;

namespace EskomCalendarApi.Services.Calendar
{
    [Serializable]
    internal class CalendarSuburbsNotImplementedException : Exception
    {
        public CalendarSuburbsNotImplementedException()
        {
        }

        public CalendarSuburbsNotImplementedException(string message) : base(message)
        {
        }

        public CalendarSuburbsNotImplementedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CalendarSuburbsNotImplementedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}