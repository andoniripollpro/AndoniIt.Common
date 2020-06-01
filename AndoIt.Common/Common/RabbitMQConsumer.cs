using AndoIt.Common.Dto;
using AndoIt.Common.Interface;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Web;

namespace AndoIt.Common.Common
{
	public class RabbitMQConsumer : IDisposable, IRabbitMQConsumer
	{
		public event EventHandler<Envelope<string>> MessageArrived;
		public event EventHandler<Envelope<string>> ErrorFatalEvent;

		private readonly ILog log;
		private readonly ConnectionFactory connectionFactory;

		private readonly string amqpUrlListen;
		private bool DisposingOnPurpouse = false;
		private IConnection connection;
		private IModel channel;

		//public RabbitMQConsumer(ConnectionFactory connectionFactory, ILog log, string amqpUrlListen)
		public RabbitMQConsumer(ILog log, string amqpUrlListen)
		{
			this.log = log ?? throw new ArgumentNullException("log");			
			this.connectionFactory = new ConnectionFactory();
			this.amqpUrlListen = amqpUrlListen ?? throw new ArgumentNullException("amqpUrlListen");
		}

		public void Listen()
		{
			this.log.Info($"Start on completeUri '{this.amqpUrlListen}'", new StackTrace());

			try
			{				
				Uri uri = new Uri(this.amqpUrlListen);
				this.connectionFactory.Uri = uri;
				string queue = HttpUtility.ParseQueryString(uri.Query).Get("queue");

				this.connection = this.connectionFactory.CreateConnection();
				this.connection.ConnectionShutdown += (obj, msg) => ConnectionShutdown(obj, msg);
				this.connection.ConnectionBlocked += (obj, msg) => ConnectionBlocked(obj, msg);

				this.channel = this.connection.CreateModel();
				this.channel.ModelShutdown += (obj, msg) => ModelShutdown(obj, msg);

				channel.QueueDeclare(queue: queue,
						 durable: true,
						 exclusive: false,
						 autoDelete: false,
						 arguments: null);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) => { ConsumeMessage(ea); };
				consumer.ConsumerCancelled += (obj, msg) => { ConsumerCancelled(obj, msg); };
				consumer.Shutdown += (obj, msg) => { ConsumerShutdown(obj, msg); };
				consumer.Unregistered += (obj, msg) => { ConsumerUnregistered(obj, msg); };
				channel.BasicConsume(queue: queue,
										autoAck: false,
										consumer: consumer);
			}
			catch (Exception e)
			{
				this.log.Fatal($"No es capaz de escuchar los mensajes de la cola RabbitMQ", e, new StackTrace());
				throw;
			}
			this.log.Info($"End", new StackTrace());
		}

		public void ConsumeMessage(BasicDeliverEventArgs ea)
		{
			string message = "<Unknown>";
			string correlationId = "<Unknown>";
			try
			{
				var body = ea.Body;
				correlationId = ea.BasicProperties.CorrelationId;
				message = Encoding.UTF8.GetString(body);

				this.MessageArrived?.Invoke(this, new Envelope<string>() { Content = message, CorrelationId = ea.BasicProperties.CorrelationId });

				this.channel.BasicAck(ea.DeliveryTag, false);

				this.log.Debug($"Received message '{message}', correlationId {correlationId}. Rabbit queue: {this.amqpUrlListen}", new StackTrace());
			}
			catch (Exception e)
			{
				this.log.Error($"Received message'{message}', correlationId {correlationId}. Rabbit queue: {this.amqpUrlListen}", e, new StackTrace());
			}
		}

		private void ModelShutdown(object obj, ShutdownEventArgs msg)
		{
			this.log.Info($"Sender: {GetObjectName(obj)} Message: {JsonConvert.SerializeObject(msg)}", new StackTrace());
		}

		private void ConnectionBlocked(object obj, ConnectionBlockedEventArgs msg)
		{
			this.log.Info($"Sender: {GetObjectName(obj)} Message: {JsonConvert.SerializeObject(msg)}", new StackTrace());
		}

		private void ConnectionShutdown(object obj, ShutdownEventArgs msg)
		{
			this.log.Info($"Sender: {GetObjectName(obj)} Message: {JsonConvert.SerializeObject(msg)}", new StackTrace());
		}

		private void ConsumerShutdown(object obj, ShutdownEventArgs msg)
		{
			this.log.Info($"Sender: {GetObjectName(obj)} Message: {JsonConvert.SerializeObject(msg)}", new StackTrace());
		}

		private void ConsumerCancelled(object obj, ConsumerEventArgs msg)
		{
			this.log.Info($"Sender: {GetObjectName(obj)} Message: {JsonConvert.SerializeObject(msg)}", new StackTrace());
		}

		private void ConsumerUnregistered(object obj, ConsumerEventArgs msg)
		{
			this.log.Info($"Sender: {GetObjectName(obj)} Message: {JsonConvert.SerializeObject(msg)}", new StackTrace());
		}

		private string GetObjectName(object obj)
		{
			return obj.GetType().Name;
		}

		/// <summary>
		/// Este método a resultado peor que dejar a RabbitMQ restaurarse a sí mismo.
		/// Lo dejo (aunque no se use) para darle una vuelta en el futuro
		/// </summary>
		private void RecoverIfNeeded()
		{
			try
			{
				if (!this.DisposingOnPurpouse && this.channel != null)
				{
					var internalRestartRetrays = 10;
					this.log.Info($"InternalRestartRetrays hardcoded: {internalRestartRetrays}", new StackTrace());
					new Insister(this.log).Insist(new Action(() => InternalRestart()), internalRestartRetrays);
				}
			}

			catch (Exception ex)
			{
				string errorMessage = "Se ha intentado recuperar la conexión con RabbitMQ, pero no se ha podido";
				this.log.Fatal($"{new StackTrace().ToStringClassMethod()}: {errorMessage}", ex);
				this?.ErrorFatalEvent(this, new Envelope<string>() { Content = errorMessage, Exception = ex });
			}
		}		
		private void InternalRestart()
		{
			this.log.Error($"this.DisposingOnPurpouse = false", null, new StackTrace());

			var internalRestartSeconds = 60 * 60; // Una hora
			this.log.Info($"InternalRestartSeconds hardcoded: {internalRestartSeconds}", new StackTrace());
			Thread.Sleep(internalRestartSeconds * 1000);

			CloseConnection();

			this.Listen();
		}

		public void Dispose()
		{
			this.DisposingOnPurpouse = true;
			CloseConnection();

			this.log.Info($"Connection Closed", new StackTrace());
		}

		private void CloseConnection()
		{
			this.channel?.Close();
			this.connection?.Close();
			this.channel = null;
			this.connection = null;
			GC.Collect();
		}
	}
}


