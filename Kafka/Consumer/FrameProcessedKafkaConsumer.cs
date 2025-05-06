using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using DataViewerApi.Dto;
using DataViewerApi.Service;

namespace DataViewerApi.Kafka.Consumer;

public class FrameProcessedKafkaConsumer : BackgroundService
{
    private readonly ILogger<FrameProcessedKafkaConsumer> _logger;
    private readonly string _bootstrapServers = "localhost:19092";
    private readonly string _topic = "frame.processed";
    private readonly string _groupId = "videoprocessor-consumer";
    private readonly ConsumerConfig _config;
    private IConsumer<Ignore, string> _consumer;
    
    private readonly IServiceProvider _serviceProvider;

    public FrameProcessedKafkaConsumer(ILogger<FrameProcessedKafkaConsumer> logger, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _logger = logger;

        _config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
        _consumer.Subscribe(_topic);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    var processedFrame = JsonSerializer.Deserialize<ProcessedFrameDto>(consumeResult.Message.Value);
                    if (processedFrame != null)
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var frameService = scope.ServiceProvider.GetRequiredService<IFrameService>();
                            await frameService.ReceiveProcessedFrame(processedFrame);
                        }
                    }
                    _logger.LogInformation($"Message received: {consumeResult.Message.Value}");
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consumer cancelled.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            finally
            {
                _consumer.Close(); 
            }
        }, stoppingToken);
    }
}