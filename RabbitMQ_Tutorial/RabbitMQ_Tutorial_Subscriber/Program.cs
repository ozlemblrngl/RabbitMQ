using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

internal class Program
{
	private static void Main(string[] args)
	{
		var factory = new ConnectionFactory();
		factory.Uri = new Uri("amqps://pwegeele:4bDyiaGRp-wOKQQJA--3zG-zDJ1aq6Sr@shark.rmq.cloudamqp.com/pwegeele");

		using var connection = factory.CreateConnection();
		var channel = connection.CreateModel();

		channel.BasicQos(0, 1, false);

		var consumer = new EventingBasicConsumer(channel);

		var queueName = channel.QueueDeclare().QueueName; // channel üzerinden random bir kuyruk ismi getiriyoruz.
														  // kuyruk declare etmiyoruz bind ediyoruz. Bind'dan kasıt subscriber düştüğünde kuyruk da direkt olarak düşsün demek.
														  // kuyruk da oluşturabiliriz ama gerek yok bind etmek yeterli.

		var routeKey = "Critical.Warning.Error";
		//"*.Error.*" yaptım başı ve sonu değişken ama ortası mutlaka Error olan bir routeKey tanımladık.
		// ama bunda çalışmadı diğer hepsini # ile çağırdığımda geldi. özellikle routekey isimleri belirlediğimde geldi ama wildcard lı çalıştıramadım.
		// Sorunu bulamadım.
		channel.QueueBind(queueName, "logs-topic", routeKey);

		channel.BasicConsume(queueName, false, consumer);

		Console.WriteLine("Loglar Dinleniyor.");


		consumer.Received += (object sender, BasicDeliverEventArgs e) =>
		{
			try
			{
				Thread.Sleep(1000);
				var message = Encoding.UTF8.GetString(e.Body.ToArray());
				Console.WriteLine("Gelen Mesaj: " + message);
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