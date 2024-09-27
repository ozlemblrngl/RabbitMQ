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

		// kuyruk oluşturma işlemini ister publisher ister subscriber kısmında yapabiliriz.
		// eğer publisherda kuyruğun oluştuğundan eminsek buradaki kodu silebiliriz ancak değilsek bu durumda hata alırız.
		// publisherda yoksa burada kuyruğun adının olması lazım.Yani aşağıdaki kodun.
		// hem publisher da hem subscriberda olması da hataya neden olmaz.Ancak parametreleri aynı olmalı.
		// channel.QueueDeclare("hello-queue", true, false, false);

		var consumer = new EventingBasicConsumer(channel);

		//bool autoAck true ise bunun anlamı rabbitmq subcriber a mesaj gönderdiğinde bunu doğru da işlese yanlış da işlese siler.
		//false ise rabbitmq mesajı ilettiğinde sen bunu direkt silme bunu doğru işlesin işledikten sonra ben sana haber edeceğim.
		//gerçek hayatta false yaparız ama burada öğrenme amacı olduğundan true yapacağız. 

		channel.BasicConsume("hello-queue", true, consumer);

		consumer.Received += (object sender, BasicDeliverEventArgs e) =>
		{
			var message = Encoding.UTF8.GetString(e.Body.ToArray());

			Console.WriteLine("Gelen Mesaj : " + message);
		};


		Console.ReadLine();
	}


}