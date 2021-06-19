using Newtonsoft.Json;
using PS.DL.PackGenerator.Interface;
using PS.DL.Rabbit;
using PS.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PS.DL.PackGenerator.Generator
{
    public class PackGenerator : IPackGenerator
    {
        private readonly RabbitProducer _rabbitProducer;

        public PackGenerator(RabbitProducer rabitProducer)
        {
            _rabbitProducer = rabitProducer;
        }

        public async Task<int> GeneratePacks(int envelopeNum)
        {
            var result = new List<Mail>();

            for (int i = 0; i < envelopeNum; i++)
            {
                Random rand = new Random();
                var id = rand.Next();

                var pack = new Mail()
                {
                    Id = id,
                    EnvelopeNumber = envelopeNum
                };
                result.Add(pack);
            }

            await _rabbitProducer.Publish(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)));

            return envelopeNum;
        }
    }
}
