internal class Program
{
	private static void Main(string[] args)
	{
		// Exchange kullanmadığımız yapıda direkt queue ye mesajı gönderdik ve buna default exchange denir.
		// Exchange type 4 'e ayrılır: 
		// 1- Fanout Exchange  
		// 2- Direct Exchange
		// 3- Topic Exchange
		// 4- Header Exchange

		// consumer'lar exchange bilmezler. Bir tek Queue ye bağlanırlar ve bunu bilirler.
		// Direkt kuyruğa bağlanır ve kuyruktan ilgili mesajı okurlar.
		// Peki bu kuyuğu consumer mı üretmeli? yoksa publisher mı? senaryodan senaryoya göre değişir.
		// bazı senaryolarda exchange ve kuyruk üretme işlemini producera(publisher'a) bırakırız, bazı senaryolarda ise exchange'i producer, kuyruğu da consumer üretir.
		// eğer kuruğu producer oluşturursa o an consumerlar ayakta olmasa bile mesajlar kaybolmaz.
		// eğer kuruğu consumerlar(subscriber) üretirse ve consumerlar o an ayakta değilseler, producer ın üretmiş olduğu mesajlar exchange e gelecek kuyruk oluşmadığından dolayı, mesajlar havada kalacak.
		// yani herhangi bir kuyruğa gitmediğinden silinecektir.

		// Direct Exchange
		// Adı üstünde, producer tarafından bir mesaj geldiğinde bu mesajı route bilgisine göre direkt ilgili kuyruğa gönderir.Fanout da tüm kuyruklara filtreleme yapmadan gönderiyorken burada gelen mesajın route una göre gönderim yapa.
		// Örn gelen mesajın route'unda critical route yazıyorsa ilgili routeuna, error route yazıyorsa ilgili kuyruğuna gönderir.
		// Bu sefer kuyrukları producer tarafında oluşturduğumuz bir senaryo düşünüyoruz.
		// Örn ilk üç ay critical routedaki mesajları bir veritabanına yazayım, daha sonra bir consumer daha eklensin ve buradaki kuyruktaki mesajları da bir dosyaya yazayım ya da bir api'ye göndereyim.
		// her bir consumer ilgili kuyruğa bağlanarak işlemi gerçekleştirebilir.
		// tabi ki bu sırada consumerlar ayakta olmasa dahi kuyrukları producer tarafında oluşturacağımız için hiçbir mesaj kaybolmayacak.
		// ne samanki ilgili consumer kuyruğa bağlanıp mesajı okuyacak o zaman ilgili mesaj kuyruktan silinecek.
	}
}