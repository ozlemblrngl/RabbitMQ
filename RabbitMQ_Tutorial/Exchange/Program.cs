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

		// Direct Exchange
		// Adı üstünde, producer tarafından bir mesaj geldiğinde bu mesajı route bilgisine göre direkt ilgili kuyruğa gönderir.Fanout da tüm kuyruklara filtreleme yapmadan gönderiyorken burada gelen mesajın route una göre gönderim yapa.
		// Örn gelen mesajın route'unda critical route yazıyorsa ilgili routeuna, error route yazıyorsa ilgili kuyruğuna gönderir.
		// Bu sefer kuyrukları producer tarafında oluşturduğumuz bir senaryo düşünüyoruz.
		// Örn ilk üç ay critical routedaki mesajları bir veritabanına yazayım, daha sonra bir consumer daha eklensin ve buradaki kuyruktaki mesajları da bir dosyaya yazayım ya da bir api'ye göndereyim.
		// her bir consumer ilgili kuyruğa bağlanarak işlemi gerçekleştirebilir.
		// tabi ki bu sırada consumerlar ayakta olmasa dahi kuyrukları producer tarafında oluşturacağımız için hiçbir mesaj kaybolmayacak.
		// ne samanki ilgili consumer kuyruğa bağlanıp mesajı okuyacak o zaman ilgili mesaj kuyruktan silinecek.

		// Topic Exchange
		// Biraz daha detaylı routelama yapısına sahip bir exchangedir.
		// producer olarak routekeyde normal bir string ifade yazmak yerine noktalarla beraber ifadeler belirleriz.
		// örn Routing Kye = Critical.Error.Warning gibi
		// istediğimiz string ifadeleri kullanabiliriz ama bu ifadeleri noktalarla birbirinden ayırmamız gerekiyor.
		// consumer olarak route muzda direkt olarak route key i yazarız. Bu route a sahip olan mesajlar bize ulaşır.
		// bu sefer kuyruk oluşturma işlemini consumer a bırakacağız çünkü bırada varyasyonlar çok fazla.
		// Örn ortası Error olsun başı ve sonu herhangi bir şeyolabilir diyebiliriz şı şekilde *Error* gibi
		// ya da ortası herhangi bir şey olsun başı Critical sonu Warning olsun diyebiliriz: Critical.*.Warning olsun dersek başı ve sonu yazımdaki gibi olur.
		// burada * ifadesi tek bir string ifadeye karşılık gelir.
		// burada 5 nokd-tqa da olabilir başka sayıda nokta da olabilir önemli olan stringlerin arasında nokta olması gerektiği.
		// son string kısmı Error olanlar gelsin dersek eğer o zaman diyez(#) kullanırız.  ==> #.Error
		// topic exchange gerçekten de çok detaylı bir routelama yapmak istediğimizde kullanacağımız bir exchange'dir.
	}
}