using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PC.DL.DataFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS.DL.Kafka
{
    public class ResultConsumer : BackgroundService
    {
        private readonly ILogger<ResultConsumer> _logger;
        private readonly ConsumerConfig _kafkaConfig;
        private readonly IMailDataFlow _mailDataFlow;

        public ResultConsumer(ILogger<ResultConsumer> logger, IMailDataFlow mailDataFlow)
        {
            _logger = logger;

            _kafkaConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            _mailDataFlow = mailDataFlow;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                using var consumer = new ConsumerBuilder<int, byte[]>(_kafkaConfig).Build();
                try
                {
                    consumer.Subscribe("data");

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(stoppingToken);

                            try
                            {
                                _mailDataFlow.ProcessMessage(consumeResult.Message.Value);

                                _logger.LogInformation(consumeResult.Message.Key.ToString());
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Kafka consume message error: {0}", ex.Message);
                            }

                            if (consumeResult.IsPartitionEOF)
                                break;

                        }
                        catch (ConsumeException e)
                        {
                            _logger.LogError(e, $"Consumer for mail '{e.ConsumerRecord.Topic}'. ConsumeException, Key: {Encoding.UTF8.GetString(e.ConsumerRecord.Message.Key)}, Error: {JsonConvert.SerializeObject(e.Error)}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Kafka consume envelope error: {0}", ex.Message);
                        }
                    }
                }
                catch (OperationCanceledException e)
                {
                    _logger.LogError(e, $"Consumer for mail data: {e.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Consumer for mail '{string.Join(';', "data")}'. Exception.");
                }
            }, stoppingToken);
        }
    }
}
