using System.Text.Json;
using Confluent.Kafka;
using DataViewerApi.Dto;

namespace DataViewerApi.Kafka.Producer;

public class VideoToProcessKafkaProducer
{
    private readonly string _bootstrapServers = "localhost:19092";
    private readonly string _topic = "video.to.process";
    
    public async Task SendMessageAsync(VideoToProcessDto frame)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers,
            MessageMaxBytes = 10 * 1024 * 1024
        };
        
        var json = JsonSerializer.Serialize(frame);

        using var producer = new ProducerBuilder<Null, string>(config).Build();

        var result = await producer.ProduceAsync(_topic, new Message<Null, string>
        {
            Value = json
        });

        Console.WriteLine($"Message sended to: {result.TopicPartitionOffset}");
    }
}