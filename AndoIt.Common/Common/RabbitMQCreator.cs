using AndoIt.Common.Interface;
using RabbitMQ.Client;
using System;
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
			//Exchange
			try
			{
				//type: ExchangeType.Direct
				channel.ExchangeDeclare(exchange: exchangeQueueLinkName,
					type: ExchangeType.Direct,
					durable: true,
					autoDelete: false,
					arguments: null);
			}
			catch (Exception ex)
			{
				this.log.Warn($"Al crear el exchange homonima al queue: '{exchangeQueueLinkName}'", ex);
				return;
			}
			//Queue
			try
			{
				channel.QueueDeclare(queue: exchangeQueueLinkName,
					durable: true,
					exclusive: false,
					autoDelete: false,
					arguments: null);
			}
			catch (Exception ex)
			{
				this.log.Warn($"Al crear una queue homonima al exchange: '{exchangeQueueLinkName}'", ex);
				return;
			}
			//Exchange-Queue Link
			try
			{
				channel.QueueBind(exchangeQueueLinkName, exchangeQueueLinkName, routingKey);
			}
			catch (Exception ex)
			{
				this.log.Warn($"Al lincar la queue y la exchange: '{exchangeQueueLinkName}'", ex);
			}
			this.log.Info($"End", new StackTrace());
		}
	}
}
