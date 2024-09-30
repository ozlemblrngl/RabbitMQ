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
		factory.Uri = new Uri("amqps://otbrrpua:CjXwGN5SwWcSupcScy7r0HIM1xo2915_@woodpecker.rmq.cloudamqp.com/otbrrpua");

		using var connection = factory.CreateConnection();
		var channel = connection.CreateModel();

		channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct); // durable yapmazsak uygulama restrat olduğunda exchange silinir.


		Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
		{
			var routeKey = $"route-{x}";
			var queueName = $"direct-queue-{x}"; // kuyruğun ismini oluşturdujk
			channel.QueueDeclare(queueName, true, false, false); // kuyruğu declare ettik 

			channel.QueueBind(queueName, "logs-direct", routeKey, null); // kuyruğu bind ettik
		});


		Enumerable.Range(1, 50).ToList().ForEach(x =>
		{

			LogNames log = (LogNames)new Random().Next(1, 5); // lognames tipi içinde 1 ile 5 arasında 4 tip için random belirle ve logname e çevir tipini

			string message = $"log-type: {log}";

			var messageBody = Encoding.UTF8.GetBytes(message);

			//route da belirtmemiz lazım ki direct gelen mesaja göre ilgili route kuyruğuna gönderecek.

			var routeKey = $"route-{log}";


			channel.BasicPublish("logs-direct", routeKey, null, messageBody);


			Console.WriteLine($"log gönderilmiştir {message}");
		});


		Console.ReadLine();

	}
}