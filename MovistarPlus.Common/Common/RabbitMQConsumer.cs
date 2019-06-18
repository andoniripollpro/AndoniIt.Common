using MovistarPlus.Common.Interface;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using System.Web;

namespace MovistarPlus.Common.Common
{
	public class RabbitMQConsumer : IDisposable
	{
		public event EventHandler<string> MessageArrived;

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
			//this.connectionFactory = connectionFactory ?? throw new ArgumentNullException("connectionFactory");
			this.connectionFactory = new ConnectionFactory();
			this.amqpUrlListen = amqpUrlListen ?? throw new ArgumentNullException("amqpUrlListen");
		}

		public void Listen()
		{
			this.log.Info($"{new StackTrace().ToStringClassMethod()}: Listening on completeUri '{this.amqpUrlListen}'");

			try
			{				
				Uri uri = new Uri(this.amqpUrlListen);
				this.connectionFactory.Uri = uri;
				string queue = HttpUtility.ParseQueryString(uri.Query).Get("queue");

				this.connection = this.connectionFactory.CreateConnection();
				this.channel = this.connection.CreateModel();
				channel.QueueDeclare(queue: queue,
						 durable: true,
						 exclusive: false,
						 autoDelete: false,
						 arguments: null);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					ConsumeMessage(ea);
				};
				consumer.ConsumerCancelled += (obj, msg) =>
				{
					ConsumerCancelled(obj, msg);
				};
				consumer.Shutdown += (obj, msg) =>
				{
					ConsumerShutdown(obj, msg);
				};
				consumer.Unregistered += (obj, msg) =>
				{
					ConsumerUnregistered(obj, msg);
				};
				channel.BasicConsume(queue: queue,
										autoAck: true,
										consumer: consumer);
			}
			catch (Exception e)
			{
				this.log.Fatal($"{new StackTrace().ToStringClassMethod()}: No es capaz de escuchar los mensajes de la cola RabbitMQ", e);
				throw;
			}
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

				MessageArrived?.Invoke(this, message);

				this.log.Debug($"Received message '{message}', correlationId {correlationId}. Rabbit queue: {this.amqpUrlListen}");
			}
			catch (Exception e)
			{
				this.log.Error($"Received message'{message}', correlationId {correlationId}. Rabbit queue: {this.amqpUrlListen}", e, new StackTrace());
			}
		}

		private void ConsumerShutdown(object obj, ShutdownEventArgs msg)
		{
			this.log.Info($"{new StackTrace().ToStringClassMethod()}: Sender: {GetObjectName(obj)} Message: {JsonConvert.SerializeObject(msg)}");
			RecoverIfNeeded();
		}

		private void ConsumerCancelled(object obj, ConsumerEventArgs msg)
		{
			this.log.Info($"{new StackTrace().ToStringClassMethod()}: Sender: {GetObjectName(obj)} Message: {JsonConvert.SerializeObject(msg)}");
			RecoverIfNeeded();
		}

		private void ConsumerUnregistered(object obj, ConsumerEventArgs msg)
		{
			this.log.Info($"{new StackTrace().ToStringClassMethod()}: Sender: {GetObjectName(obj)} Message: {JsonConvert.SerializeObject(msg)}");
			RecoverIfNeeded();
		}
		private void RecoverIfNeeded()
		{
			if (!this.DisposingOnPurpouse)
			{
				this.log.Error($"{new StackTrace().ToStringClassMethod()}: this.DisposingOnPurpouse = false");
				this.connection.Dispose();
				this.channel.Dispose();
				this.Listen();
			}
		}
		private string GetObjectName(object obj)
		{
			return obj.GetType().Name;
		}

		public void Dispose()
		{
			this.DisposingOnPurpouse = true;
			this.connection.Dispose();
			this.channel.Dispose();

			this.log.Info($"{new StackTrace().ToStringClassMethod()}");
		}
	}
}


