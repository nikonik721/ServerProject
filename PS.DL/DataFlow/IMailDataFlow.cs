using System;
using System.Collections.Generic;
using System.Text;

namespace PC.DL.DataFlow
{
    public interface IMailDataFlow
    {
        void ProcessMessage(byte[] msg);
    }
}
