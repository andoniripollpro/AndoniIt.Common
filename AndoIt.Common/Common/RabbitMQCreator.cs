using AndoIt.Common.Interface;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AndoIt.Common.Common
{
    public class RabbitMQCreator
    {
		private readonly ILog log;

		public RabbitMQCreator(ILog log)
		{
			this.log = log ?? throw new ArgumentNullException(nameof(log));
		}

        public void CreateExchangeQueueAndLinkIfPossible(string exchangeQueueLinkName, IModel channel, string routingKey)
        {
            this.log.Debug($"Start", new StackTrace(), exchangeQueueLinkName, channel, routingKey);

            // Exchange
            try
            {
                channel.ExchangeDeclare(
                    exchange: exchangeQueueLinkName,
                    type: ExchangeType.Fanout, // Se mantiene Fanout, cambiar si es necesario
                    durable: true,
                    autoDelete: false,
                    arguments: null);
            }
            catch (Exception ex)
            {
                this.log.Warn($"Al crear el exchange homónimo al queue: '{exchangeQueueLinkName}'", ex);
            }

            // Queue en modo Quorum 
            try
            {
                var queueArguments = new Dictionary<string, object>
                {
                    { "x-queue-type", "quorum" } // 🚀 Configuración para que la cola sea de tipo Quorum
                };

                channel.QueueDeclare(
                    queue: exchangeQueueLinkName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: queueArguments);
            }
            catch (Exception ex)
            {
                this.log.Warn($"Al crear una queue homónima al exchange en modo Quorum: '{exchangeQueueLinkName}'", ex);
            }

            // Vinculación Exchange-Queue
            try
            {
                channel.QueueBind(exchangeQueueLinkName, exchangeQueueLinkName, routingKey);
            }
            catch (Exception ex)
            {
                this.log.Warn($"Al vincular la queue y el exchange: '{exchangeQueueLinkName}'", ex);
            }

            this.log.Info($"End", new StackTrace());
        }

    }
}
