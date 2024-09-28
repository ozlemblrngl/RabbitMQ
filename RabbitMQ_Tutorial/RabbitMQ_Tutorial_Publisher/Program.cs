using RabbitMQ.Client;
using System.Text;

internal class Program
{
	private static void Main(string[] args)

	{

		var factory = new ConnectionFactory();
		factory.Uri = new Uri("amqps://otbrrpua:CjXwGN5SwWcSupcScy7r0HIM1xo2915_@woodpecker.rmq.cloudamqp.com/otbrrpua");

		using var connection = factory.CreateConnection();
		var channel = connection.CreateModel();

		channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout); // durable yapmazsak uygulama restrat olduğunda exchange silinir.

		Enumerable.Range(1, 50).ToList().ForEach(x =>
		{
			string message = $"log {x}";

			var messageBody = Encoding.UTF8.GetBytes(message);

			channel.BasicPublish("logs-fanout", "", null, messageBody);


			Console.WriteLine($"mesaj gönderilmiştir {message}");
		});


		Console.ReadLine();

	}
}