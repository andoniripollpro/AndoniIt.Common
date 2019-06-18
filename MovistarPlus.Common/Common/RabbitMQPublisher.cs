using MovistarPlus.Common.Interface;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Web;

namespace MovistarPlus.Common.Common
{
	public class RabbitMQPublisher
	{
		private readonly ILog log;
		private readonly ConnectionFactory connectionFactory;

		public RabbitMQPublisher(ILog log, string amqpUrlPublish)
		{
			this.log = log ?? throw new ArgumentNullException("log");			
			this.connectionFactory = new ConnectionFactory();
			this.connectionFactory.Uri = new Uri(amqpUrlPublish ?? throw new ArgumentNullException("amqpUrlPublish"));
		}

		public void Publish(string datosPublicacion, string routingKey = "", string correlationId = "", string completeConfirmationUri = "")
		{
			this.log.Debug($"Start: {datosPublicacion}");
			string exchangeName = HttpUtility.ParseQueryString(this.connectionFactory.Uri.Query).Get("exchange");
			string confirmationQueue = string.Empty;
			if (!string.IsNullOrEmpty(completeConfirmationUri))
			{
				Uri confirmationUri = new Uri(completeConfirmationUri);
				confirmationQueue = HttpUtility.ParseQueryString(confirmationUri.Query).Get("exchange");
			}

			using (IConnection connection = this.connectionFactory.CreateConnection())
			{
				using (IModel channel = connection.CreateModel())
				{
					IBasicProperties properties = channel.CreateBasicProperties();
					properties.ContentType = "application/json";
					properties.ContentEncoding = "UTF-8";
					properties.DeliveryMode = 2;
					properties.ReplyTo = confirmationQueue;
					if (correlationId != string.Empty)
						properties.CorrelationId = correlationId;

					channel.ExchangeDeclare(exchange: exchangeName,
						type: "fanout",
						durable: true,
						 autoDelete: false,
						 arguments: null);

					byte[] payload = Encoding.UTF8.GetBytes(datosPublicacion);
					channel.BasicPublish(exchangeName, routingKey, properties, payload);
					this.log.Info(string.Format("channel.BasicPublish({0}, {1}, {2}, {3})", exchangeName, routingKey, properties, payload), new System.Diagnostics.StackTrace());
				}
			}
		}
	}
}