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

		// Fanout Exchange
		// burada her bir kuyruğa filtreleme yapmadan mesajlar iletilir. bir mesaj varsa her bir kuyruğa bu mesaj kuyrukları filtrelemeden
		// birer birer gönderilir.
		// kendine bağlı tüm kuyruklara aynı mesajı hiçbir filtreleme yapmadan iletir fanout exchange.
		// örnek bir senaryo hava durumuna ilişkin bilgileri mesajlayan bir yapımız var ve buna kuyruk oluşturup bağlanılmadıysa tüm mesajlar silinir.
		// consumerlar kuyruk oluşturup bağlandığında ise bu mesajlara ulaşırlar ve consumerlar farklı dillerde farklı projeler de olabilir. 
		// burada bir filtreleme yapılmadan mesajlar kuyruklar aracılığıyla consumerlara iletilir.
		// hava tahmini raporunda kaç kuyruğun olacağını bilmediğimizden, producerın kuyrukları üretmesi yerine, kuyruğunu consumer'ın üretip bağlanması makul olandır.
		// aksi durumda producer üretmiş olsa, diyelim ki 20 adet üretti 3 kuyruk kullanılıyor geri kalanı ya memory de ya hard disk te boşuna yer kaplayacaktır.
		// fanout exchange de genellikle consumerlar kendi kuyruklarını olulştururlar. Consumerlar down olduğu zaman da kuyruklar silinir.
		// eğer hiçbir consumer bağlanmazsa producerin gönderdiği mesajlar hiçbir yerde kaydedilmez.
		// consumerlar şunu da yapabilir, kuyruğu oluşturayım ben silindiğim zaman kuyruk kalsın. Bu da olabilir.
	}
}