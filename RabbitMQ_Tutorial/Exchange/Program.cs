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

		// Header Exchange
		// diğerlerinde route da gnderdiğimiz bilgileri burada header da gönderiyoruz.
		// producer bir mesaj gönderirken headerda routelama parametrelerini gönderir.
		// örn header ==> format= pdf shape = a4 
		// Sadece iki adet key value olmak zorunda değil başka sayılarda da olabilir.
		// peki kuyruk oluştururken yukarıdaki formattaki headerı nasıl kuyruğa mesaj olarak alacağız? Yine aynı şekilde alabiliriz formatı ve shape i belirlediğimiz gibi yaparak.
		// şu headera sahip olan mesajlar bu kuyruğa gelsin diye filtreleme yaparken, nasıl topic exchange de # ve * vardı burada da x.match ifadesi var.
		// x.match = any dersek key çiftlerinden bir tanesinin değeri aynı olması yeterli örn format ya da shape.
		// x.match = all dersek tüm key value ların aynı olması gerekir.

	}
}