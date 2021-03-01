using System;
using System.Collections.Generic;

namespace LogReaderAndWriter
{
    public class LogModel
    {
        public string CardNumber { get; set; }
        public Dictionary<string, string> ParamAndValue { get; set; }
        public string ProcessType { get; set; }
        public DateTime TransactionTime { get; set; }
        public string TransactionType { get; set; }

    }
}
