using Convey.MessageBrokers.RabbitMQ;
using System;

namespace Lapka.Pets.Infrastructure.Exceptions
{
    public class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                _ => null
            };
    }

}