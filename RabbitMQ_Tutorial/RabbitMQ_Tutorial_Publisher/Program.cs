using RabbitMQ.Client;
using System.Text;

internal class Program
{
	private static void Main(string[] args)

	//bir kere bağlantı oluşturduğumuzda birden fazla kanal oluşturabiliriz. Çünkü bağlantı oluşturmak maliyetli bir şeydir
	//ve bir bağlantıda birden fazla kanalın kullanılması sağlanır.
	{
		// uri aşağıda yazmak best practice değil, appsetting de yazmamız lazım normalde.

		var factory = new ConnectionFactory();
		factory.Uri = new Uri("amqps://otbrrpua:CjXwGN5SwWcSupcScy7r0HIM1xo2915_@woodpecker.rmq.cloudamqp.com/otbrrpua");

		//using (var connection = factory.CreateConnection())
		//{

		//} olarak da kullanılabilir.
		//Aradaki fark aşağıdakinin bu sefer süslü parantezi maine kadar genişletmiş olmasıdır.

		using var connection = factory.CreateConnection();

		// aşağıda oluşturduğumuz kanal üzerinden rabbitmq ile haberleşebiliriz. 
		var channel = connection.CreateModel();

		// oluşturulan kanal üzerinden mesajların boşa düşmemesi için bir kuyruk (queue) oluşturmamız lazım.
		// aşağıda kuyruk oluşturuyoruz
		// bool durable parametresini true yapmamız lazım.
		// yapmazsak mesajlar in memory de tutulur ve yeniden başlama durumunda hepsi yok olur.
		// True olduğunda ise kuyruklar fiziksel olarak kaydedilir ve restart durumunda kaybolmaz.
		// bool exclusive 'i true yaparsak farklı kanallardan rabbitmq ya ulaşamayız ama biz burada farklı process le rabbitmq'ya
		// farklı kanallardan da ulaşmak istediğimizden false yapıyoruz 3. parametreyi.
		// bool autodelete te de bu kuyruğa bağlı olan subscriber bağlantısını kopartırsa kuyruk da silinir demek.
		// bu bizim isteyeceğimiz bir durum değil keza subsriber yanlışlıkla down olursa kuyruk da silinir.
		// Biz istiyoruz ki kuyruk her zaman ayakta dursun. O nedenle otomatik silmeyi kapatıyoruz, false a çekiyoruz.
		// ama subscriberların hepsi yani sonuncusu dştüğünde artık kuyruğumuz silinsin diyorsak true da yapabiliriz.

		channel.QueueDeclare("hello-queue", true, false, false);

		// rabbitmq ya mesajlarımızı bir byte dizimi olarak göndeririz ve bu büyük bir avantajdır. pdf de gönderebiliriz. İmage da gönderebiliriz.
		// istediğimiz her şeyi gönderebiliriz.

		string message = "Hello world";

		var messageBody = Encoding.UTF8.GetBytes(message);

		// aşağıdaki metodun extension unda echange var ancak biz şu an exchange kullanmayacağız.
		// exchange kullanılmayan hali de default exchange olarak geçer. 
		// bu durumda routingkey'e kuruğun adını vermemiz gerekir. bkz. hello-queue

		channel.BasicPublish(string.Empty, "hello-queu", null, messageBody);

		Console.WriteLine("mesaj gönderilmiştir");

		Console.ReadLine();

	}
}