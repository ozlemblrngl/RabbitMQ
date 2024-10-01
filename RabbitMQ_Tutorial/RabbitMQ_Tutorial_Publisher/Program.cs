using RabbitMQ.Client;
using System.Text;

public enum LogNames
{
	Critical = 1,
	Error = 2,
	Warning = 3,
	Info = 4,
}
internal class Program
{
	private static void Main(string[] args)

	{

		var factory = new ConnectionFactory();
		factory.Uri = new Uri("amqps://pwegeele:4bDyiaGRp-wOKQQJA--3zG-zDJ1aq6Sr@shark.rmq.cloudamqp.com/pwegeele");

		using var connection = factory.CreateConnection();
		var channel = connection.CreateModel();

		channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic); // durable yapmazsak uygulama restrat olduğunda exchange silinir.

		// kuyruğu burada oluşturmuyoruz consumer tarafında oluşturuyoruz. Declare ya da bind da yapmıyoruz burada.

		Random random = new Random();

		Enumerable.Range(1, 50).ToList().ForEach(x =>
		{
			LogNames log1 = (LogNames)random.Next(1, 5);
			LogNames log2 = (LogNames)random.Next(1, 5);
			LogNames log3 = (LogNames)random.Next(1, 5);
			var routeKey = $"{log1}.{log2}.{log3}";
			string message = $"log-type: {log1}-{log2}-{log3}";

			var messageBody = Encoding.UTF8.GetBytes(message);

			channel.BasicPublish("logs-topic", routeKey, null, messageBody);


			Console.WriteLine($"log gönderilmiştir {message}");
		});


		Console.ReadLine();

	}
}