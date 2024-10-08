﻿using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace WatermarkApp_RabbitMQ.Services
{
	public class RabbitMQPublisher
	{
		private readonly RabbitMQClientService _rabbitMQClientService;

		public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
		{
			_rabbitMQClientService = rabbitMQClientService;
		}

		public void Publish(ProductImageCreatedEvent productImageCreatedEvent)
		{
			var channel = _rabbitMQClientService.Connect();
			var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);

			var bodyByte = Encoding.UTF8.GetBytes(bodyString); // bana bir string ver sana byte ını vereyim.

			var properties = channel.CreateBasicProperties();

			properties.Persistent = true;

			channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingWatermark, basicProperties: properties, body: bodyByte);
		}
	}
}
