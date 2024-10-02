using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

		channel.BasicQos(0, 1, false);

		var consumer = new EventingBasicConsumer(channel);

		var queueName = channel.QueueDeclare().QueueName; // channel üzerinden random bir kuyruk ismi getiriyoruz.
														  // kuyruk declare etmiyoruz bind ediyoruz. Bind'dan kasıt subscriber düştüğünde kuyruk da direkt olarak düşsün demek.
														  // kuyruk da oluşturabiliriz ama gerek yok bind etmek yeterli.

		Dictionary<string, object> headers = new Dictionary<string, object>();
		headers.Add("format", "pdf");
		headers.Add("shape", "a4");
		headers.Add("x-match", "any");
		// yukarıdaki key ve value lar publisher kısmı ile aynı ve hepsinin eşleşmesini istedim ve hepsi de eşleştiği için mesajları alıyor.
		// arkada shape2 yaptım ve burayı any yaptım any yapınca biri uysa çalışıyor, ama x-match all deseydim bu durumda mesajı almayacaktı.
		channel.QueueBind(queueName, "header-exchange", string.Empty, headers);

		channel.BasicConsume(queueName, false, consumer);

		Console.WriteLine("Loglar Dinleniyor.");


		consumer.Received += (object sender, BasicDeliverEventArgs e) =>
		{
			try
			{
				var message = Encoding.UTF8.GetString(e.Body.ToArray());
				Product product = JsonSerializer.Deserialize<Product>(message);
				Console.WriteLine($"Gelen Mesaj: id: {product.Id} -- Ürün: {product.Name} -- Fiyatı: {product.Price} -- Stok: {product.Stock}");
				channel.BasicAck(e.DeliveryTag, false);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Hata : " + ex.Message);
			}

		};


		Console.ReadLine();
	}



}