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

		// her bir subscriber a bir tane mesaj gelsin diyoruz. 
		//bool global de ise false olursa örneğin (0,6,false) oldu bu durumda her bir subscriber a 6 mesaj gider
		// true olursa (0,6,true) bu durumda 3 subsriber varsa her birine 2 şer mesaj gider, 2 subscriber varsa her birine 3 mesaj gider.
		// aşağıdaki örnekte true da desek 1 mesajı bölemeyeceği için false gibi işlem yapar ve her bir subscriber a 1 mesaj gönderir.

		channel.BasicQos(0, 1, false);

		var consumer = new EventingBasicConsumer(channel);

		//bool autoAck true ise bunun anlamı rabbitmq subcriber a mesaj gönderdiğinde bunu doğru da işlese yanlış da işlese siler.
		//false ise rabbitmq mesajı ilettiğinde sen bunu direkt silme bunu doğru işlesin işledikten sonra ben sana haber edeceğim.
		// haber verme kodu da devamında  

		channel.BasicConsume("hello-queue", false, consumer);

		consumer.Received += (object sender, BasicDeliverEventArgs e) =>
		{
			var message = Encoding.UTF8.GetString(e.Body.ToArray());

			Thread.Sleep(1500);

			Console.WriteLine("Gelen Mesaj : " + message);

			// ulaştırılan tag'ı rabbitmq ya gönderiyoruz ve ulaşan mesajı raabitmq kuyruktan siliyor.
			// diğer değeri true girersek o sırada memoryde işlenmiş ama rabbitmq ya gitmemişsse mesajlar onun bilgilerini de rabbitmq ya haberdar etmek için.
			// biz şimdilik false giriyoruz. Böylece sadece ilgili mesajın durumunu raabitmq ya bildir diyoruz. 

			channel.BasicAck(e.DeliveryTag, false);
		};


		Console.ReadLine();
	}


}