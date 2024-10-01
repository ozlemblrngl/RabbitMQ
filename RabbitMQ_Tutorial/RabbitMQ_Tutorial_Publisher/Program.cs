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

		channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers); // durable yapmazsak uygulama restrat olduğunda exchange silinir.

		Dictionary<string, object> headers = new Dictionary<string, object>();
		headers.Add("format", "pdf");
		headers.Add("shape2", "a4");

		var properties = channel.CreateBasicProperties();
		properties.Headers = headers;

		Thread.Sleep(1000); // consumer tarafında kuyruğu oluşturduğumdan mesajlar boşa düşmesin diye burada diğer tarafın kuyruğu oluşturması için zaman veriyorum
		channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes("header mesajım"));

		Console.WriteLine("mesaj gönderilmiştir.");
		Console.ReadLine();

	}
}