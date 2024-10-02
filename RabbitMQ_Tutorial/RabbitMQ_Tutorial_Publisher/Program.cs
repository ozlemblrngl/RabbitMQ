using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

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
		properties.Persistent = true; // artık mesajlar da kalıcı hale gelir. Bu diğer tüm exchangeler için de geçerli.

		// aşağıdaki bir class gönderiyoruz.
		var product = new Product { Id = 1, Name = "Kalem", Price = 150, Stock = 10 };
		var productJsonString = JsonSerializer.Serialize(product); // stringe serialize ediyorum.

		Thread.Sleep(1000); // consumer tarafında kuyruğu oluşturduğumdan mesajlar boşa düşmesin diye burada diğer tarafın kuyruğu oluşturması için zaman veriyorum
		channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes(productJsonString));

		Console.WriteLine("mesaj gönderilmiştir.");
		Console.ReadLine();

	}
}