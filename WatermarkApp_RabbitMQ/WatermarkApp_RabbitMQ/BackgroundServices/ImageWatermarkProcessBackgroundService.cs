
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Drawing;
using System.Text;
using System.Text.Json;
using WatermarkApp_RabbitMQ.Services;

namespace WatermarkApp_RabbitMQ.BackgroundServices
{
    public class ImageWatermarkProcessBackgroundService : BackgroundService
    {
        // start ve stop u manuel ekledim. override on yazarak.

        private readonly RabbitMQClientService _rabbitMQClientService;
        private readonly ILogger<ImageWatermarkProcessBackgroundService> _logger;
        private IModel _channel;

        public ImageWatermarkProcessBackgroundService(RabbitMQClientService rabbitMQClientService, ILogger<ImageWatermarkProcessBackgroundService> logger)
        {
            _rabbitMQClientService = rabbitMQClientService;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMQClientService.Connect();
            _channel.BasicQos(0, 1, false);
            return base.StartAsync(cancellationToken);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(RabbitMQClientService.QueueName, false, consumer);

            consumer.Received += Consumer_Received;
            return Task.CompletedTask;
        }

        private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var productImageCreatedEvent = JsonSerializer.Deserialize<ProductImageCreatedEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", productImageCreatedEvent.ImageName);

                // Dosyanın varlığını kontrol et
                if (!File.Exists(path))
                {
                    _logger.LogError("Resim dosyası bulunamadı: {Path}", path);
                    _channel.BasicNack(@event.DeliveryTag, false, false); // Mesajı tekrar kuyruğa al
                    return Task.CompletedTask;
                }

                var siteName = "www.mysite.com";
                using var img = Image.FromFile(path);

                using var graphic = Graphics.FromImage(img);

                var font = new Font(FontFamily.GenericSansSerif, 150, FontStyle.Bold, GraphicsUnit.Pixel);
                var textSize = graphic.MeasureString(siteName, font);

                var color = Color.FromArgb(128, 255, 255, 255);
                var brush = new SolidBrush(color);

                // sağ alt köşeye watermarkı ekleme

                //var position = new Point(img.Width - ((int)textSize.Width + 30), img.Height - ((int)textSize.Height + 30));

                //graphic.DrawString(siteName, font, brush, position);


                var centerX = (img.Width / 2) - (textSize.Width / 2);
                var centerY = (img.Height / 2) - (textSize.Height / 2);

                // Çapraz çizim işlemi (45 derece rotasyon)
                graphic.TranslateTransform(centerX + (float)textSize.Width / 2, centerY + (float)textSize.Height / 2); // Merkezi al
                graphic.RotateTransform(-135); // 45 derece sağa döndür (çapraz)
                graphic.TranslateTransform(-(centerX + (float)textSize.Width / 2), -(centerY + (float)textSize.Height / 2)); // Geri kaydır

                // Çapraz watermark çizimi
                graphic.DrawString(siteName, font, brush, new PointF(centerX, centerY));

                var watermarkPath = Path.Combine("wwwroot/images/watermarks", productImageCreatedEvent.ImageName);
                img.Save(watermarkPath);

                _channel.BasicAck(@event.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Watermark eklerken bir hata oluştu.");
                _channel.BasicNack(@event.DeliveryTag, false, false); // Hata durumunda mesajı tekrar kuyruğa al
            }

            return Task.CompletedTask;
        }


        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
