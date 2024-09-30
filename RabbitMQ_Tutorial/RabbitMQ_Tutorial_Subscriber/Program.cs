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

		//burada herhangi bir kuyruk oluşturmamıza gerek yok çünkü producerda oluşturduk ve yine producerda bind ettik o nedenle burada bind da etmiyoruz. 

		channel.BasicQos(0, 1, false);

		var consumer = new EventingBasicConsumer(channel);

		var queueName = "direct-queue-Critical";

		channel.BasicConsume(queueName, true, consumer); // burada autoack değeri false tu ve true yaptık ki consumerdan manuel onay gerektirmeden mesajlar gelebilsin.
														 // Bu sayede rabbitmq mesajı gönderir göndermez consumer otomatik onaylamış olacak.
														 // Mesaj düşecek aksi halde mesaj kuyrukta biriktikçe birikiyor.

		Console.WriteLine("Loglar Dinleniyor.");


		consumer.Received += (object sender, BasicDeliverEventArgs e) =>
		{
			// mesajın düşüp düşmediğini kontrollemek için try catch yazdım. try içindeki kod çalıştırmak istediğim yer. Aşağıdaki kodu tekrar tekrar aynı mesajlar için çağırdığımdan aynı deliverytag kullanılıyor muhtemelen ve bu da çakışmaya ve hataya neden oluyor.
			try
			{
				Thread.Sleep(100);
				var message = Encoding.UTF8.GetString(e.Body.ToArray());
				Console.WriteLine("Gelen Mesaj : " + message);
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "log-critical.txt");
				File.AppendAllText(filePath, message + "\n");

				channel.BasicAck(e.DeliveryTag, false);
			}
			catch (Exception ex)
			{
				Console.WriteLine("hata: " + ex.Message);
			}

		};


		Console.ReadLine();
	}

}