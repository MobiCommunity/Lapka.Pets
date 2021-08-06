using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Services;

// ReSharper disable NotAccessedField.Local

namespace Lapka.Pets.Infrastructure.Services
{
    internal sealed class MessageBroker : IMessageBroker
    {
        private const string DefaultSpanContextHeader = "span_context";
        private readonly IBusPublisher _busPublisher;
        private readonly ICorrelationContextAccessor _contextAccessor;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;
        private readonly IMessageOutbox _outbox;
        private readonly OutboxOptions _outboxOptions;
        private readonly string _spanContextHeader;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;


        public MessageBroker(IBusPublisher busPublisher,
            ICorrelationContextAccessor contextAccessor,
            IMessagePropertiesAccessor messagePropertiesAccessor, RabbitMqOptions options,
            IMessageOutbox outbox, OutboxOptions outboxOptions, ICorrelationContextAccessor correlationContextAccessor)
        {
            _busPublisher = busPublisher;
            _contextAccessor = contextAccessor;
            //_httpContextAccessor = httpContextAccessor;
            _messagePropertiesAccessor = messagePropertiesAccessor;
            _outbox = outbox;
            _outboxOptions = outboxOptions;
            _correlationContextAccessor = correlationContextAccessor;
            _spanContextHeader = string.IsNullOrWhiteSpace(options.SpanContextHeader)
                ? DefaultSpanContextHeader
                : options.SpanContextHeader;
        }

        public Task PublishAsync(params IEvent[] events) => PublishAsync(events?.AsEnumerable());

        public async Task PublishAsync(IEnumerable<IEvent> events)
        {
            if (events is null)
            {
                return;
            }

            var messageProperties = _messagePropertiesAccessor.MessageProperties;
            var originatedMessageId = messageProperties?.MessageId;
            var correlationId = messageProperties?.CorrelationId;
            var correlationContext = _correlationContextAccessor?.CorrelationContext;

            foreach (var @event in events)
            {
                if (@event is null)
                {
                    continue;
                }

                string messageId = Guid.NewGuid().ToString("N");

                if (_outbox.Enabled)
                {
                    await _outbox.SendAsync(@event, originatedMessageId, messageId, correlationId,
                        messageContext: correlationContext);
                    continue;
                }

                await _busPublisher.PublishAsync(@event, messageId, correlationId, messageContext: correlationContext);
            }
        }
    }
}