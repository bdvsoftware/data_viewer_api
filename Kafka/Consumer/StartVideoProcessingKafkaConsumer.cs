using System.Text.Json;
using Confluent.Kafka;
using DataViewerApi.Dto;
using DataViewerApi.Service;

namespace DataViewerApi.Kafka.Consumer;

public class StartVideoProcessingKafkaConsumer : BackgroundService
{
    private readonly ILogger<StartVideoProcessingKafkaConsumer> _logger;
    private readonly string _bootstrapServers = "localhost:19092";
    private readonly string _topic = "video.to.process";
    private readonly string _groupId = "videoprocessor-consumer";
    private readonly ConsumerConfig _config;
    private IConsumer<Ignore, string> _consumer;
    
    private readonly IServiceProvider _serviceProvider;
    
    public StartVideoProcessingKafkaConsumer(ILogger<StartVideoProcessingKafkaConsumer> logger, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _logger = logger;

        _config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            MaxPollIntervalMs = 1200000
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
                    var videoToProcess = JsonSerializer.Deserialize<VideoToProcessDto>(consumeResult.Message.Value);
                    if (videoToProcess != null)
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var frameService = scope.ServiceProvider.GetRequiredService<IFrameService>();
                            await frameService.ProduceFrames(videoToProcess.VideoId, videoToProcess.VideoUrl, videoToProcess.Threshold);
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