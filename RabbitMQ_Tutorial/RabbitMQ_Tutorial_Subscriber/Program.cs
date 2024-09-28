using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

internal class Program
{
	private static void Main(string[] args)
	{
		var factory = new ConnectionFactory();
		factory.Uri = new Uri("amqps://otbrrpua:CjXwGN5SwWcSupcScy7r0HIM1xo2915_@woodpecker.rmq.cloudamqp.com/otbrrpua");

		using var connection = factory.CreateConnection();
		var channel = connection.CreateModel();

		var randomQueueName = channel.QueueDeclare().QueueName; // random kuyrukismi oluşturuyoruz.
																//channel.QueueDeclare(randomQueueName,true,false,false);   // eğer bunu yazsaydık ilgili subscriber(instance) kapansa dahi kuyruk kalırdı. Bu kuyruğun kalmasını istemediğimiz için aşağıdaki kodu yazdık
		channel.QueueBind(randomQueueName, "logs-fanout", "", null);
		// burada yeni bir kuyruk oluşturmuyoruz kanal üzerinden kuyruğu bind ediyoruz.Burada subscriber bağlantısını kestiğinde kuyruk da silinir. O şekilde ayarlıyoruz burada.
		// o nedenle bir kuyruk oluşturmak yerine channel üzerinden bir kuyruk bind ediyoruz.
		// artık uygulama her ayağa kalktığında bir kuyruk bind olacak ama down olduğunda ilgili kuyruk silinecek.

		channel.BasicQos(0, 1, false);

		var consumer = new EventingBasicConsumer(channel);

		channel.BasicConsume(randomQueueName, false, consumer);

		Console.WriteLine("Loglar Dinleniyor.");


		// channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout); 
		// artık bunu yazmamıza gerek yok, publisher exchange i oluşturdu kod diğer tarafta var, yazsak da sorun olmaz  sadece ayarları değişik yaparsak sorun olur

		consumer.Received += (object sender, BasicDeliverEventArgs e) =>
		{
			var message = Encoding.UTF8.GetString(e.Body.ToArray());


			Console.WriteLine("Gelen Mesaj : " + message);

			channel.BasicAck(e.DeliveryTag, false);
		};


		Console.ReadLine();
	}


}