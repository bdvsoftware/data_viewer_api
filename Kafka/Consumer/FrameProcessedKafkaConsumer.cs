using Confluent.Kafka;

namespace DataViewerApi.Kafka.Consumer;

public class FrameProcessedKafkaConsumer : BackgroundService
{
    private readonly ILogger<FrameProcessedKafkaConsumer> _logger;
    private readonly string _bootstrapServers = "localhost:19092";
    private readonly string _topic = "frame.processed";
    private readonly string _groupId = "videoprocessor-consumer";

    public FrameProcessedKafkaConsumer(ILogger<FrameProcessedKafkaConsumer> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    _logger.LogInformation($"Mensaje recibido: {consumeResult.Message.Value}");
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consumo cancelado.");
            }
            finally
            {
                consumer.Close();
            }
        }, stoppingToken);
    }
}