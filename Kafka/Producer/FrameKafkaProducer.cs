using System.Text.Json;
using Confluent.Kafka;
using DataViewerApi.Dto;

public class FrameKafkaProducer
{
    private readonly string _bootstrapServers = "localhost:19092";
    private readonly string _topic = "frame.to.process";

    public async Task SendMessageAsync(FrameToProcessDto frame)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
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