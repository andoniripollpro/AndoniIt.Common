using System;
using MovistarPlus.Common.Dto;
using RabbitMQ.Client.Events;

namespace MovistarPlus.Common.Interface
{
	public interface IRabbitMQConsumer
	{
		event EventHandler<Envelope<string>> ErrorFatalEvent;
		event EventHandler<Envelope<string>> MessageArrived;

		void ConsumeMessage(BasicDeliverEventArgs ea);
		void Dispose();
		void Listen();
	}
}