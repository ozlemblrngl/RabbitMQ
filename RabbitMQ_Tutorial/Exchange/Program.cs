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

		// ******** Complex Type'ları mesaj olarak göndermek*******
		// şu ana kadar hep string ifadeleri gönderdik ama biz bir class gönderebiliriz, resim gönderebiliriz, pdf gönderebiliriz vs.
		// şu ana kadar göndermek istediğimiz her şeyi bir byte dizisine çeviriyorduk, bu şekilde göndermek istediğimiz her şeyi bir byte dizisine çevirerek gönderebiliriz.

	}
}