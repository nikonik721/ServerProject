using MessagePack;
using PS.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace PC.DL.DataFlow
{
    public class MailDataFlow : IMailDataFlow
    {
        private TransformBlock<byte[], Mail> _deserializeBlock;

        public MailDataFlow()
        {
            _deserializeBlock = new TransformBlock<byte[], Mail>(msg => MessagePackSerializer.Deserialize<Mail>(msg));
        }
        public async void ProcessMessage(byte[] msg)
        {
            await _deserializeBlock.SendAsync(msg);
        }
    }
}
