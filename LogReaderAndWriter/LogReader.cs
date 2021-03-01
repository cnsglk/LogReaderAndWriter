using System;
using System.Collections.Generic;
using System.IO;

namespace LogReaderAndWriter
{
    public class LogReader
    {
        public List<LogModel> ListLogModel { get; set; }
        public LogReader(string path)
        {
            ListLogModel = new List<LogModel>();
            var logModel = new LogModel();

            logModel.ParamAndValue = new Dictionary<string, string>();

            string text = File.ReadAllText(path);
            string[] lines = text.Split(Environment.NewLine);


            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line.Contains("enc msg") == true)
                {
                    continue;
                }

                if (line.Contains('/') == true)
                {
                    var lineLenght = line.Length;

                    string dateString = "";

                    if (logModel.TransactionTime == DateTime.MinValue)
                    {
                        dateString = line.Substring(0, 19);
                        logModel.TransactionTime = DateTime.ParseExact(dateString, "dd/MM/yyyy HH:mm:ss", null);
                    }

                    if (lineLenght > 75)
                    {
                        // line numberın önemi varmı kontrol edilecek.
                        string cardNumber = line.Remove(0, 54);
                        logModel.CardNumber = cardNumber;
                        continue;
                    }
                    else
                    {
                        switch (line)
                        {
                            case string word when word.Contains("Terminal Rx"):
                                logModel.TransactionType = "Terminal Rx";
                                logModel.ProcessType = line.Substring(lineLenght - 5).Replace(":", "");
                                break;
                            case string word when word.Contains("Bank Host Rx"):
                                logModel.TransactionType = "Bank Host Rx";
                                logModel.ProcessType = line.Substring(lineLenght - 5).Replace(":", "");
                                break;
                            case string word when word.Contains("Terminal Tx"):
                                logModel.TransactionType = "Terminal Tx";
                                logModel.ProcessType = line.Substring(lineLenght - 5).Replace(":", "");
                                break;
                            case string word when word.Contains("Bank Host Tx"):
                                logModel.TransactionType = "Bank Host Tx";
                                logModel.ProcessType = line.Substring(lineLenght - 5).Replace(":", "");
                                break;
                        }
                        continue;
                    }

                }
                else if (line.Contains('[') == true)
                {
                    string param = line.Remove(0, 31);
                    string key = param.Substring(0, 3);
                    string value = param.Remove(0, 5).Replace(" ", "").Replace("]", "");
                    logModel.ParamAndValue[key] = value;
                    continue;
                }

                if (String.IsNullOrWhiteSpace(line))
                {
                    ListLogModel.Add(logModel);

                    if (String.IsNullOrWhiteSpace(lines[i + 1]))
                    {
                        break;
                    }
                    logModel = new LogModel();
                    logModel.ParamAndValue = new Dictionary<string, string>();
                }
            }

        }
        public void WriteLogInfo()
        {
            foreach (var transaction in ListLogModel)
            {
                if (transaction.ProcessType == "0200")
                {
                    Console.WriteLine(transaction.CardNumber);

                    Console.WriteLine(transaction.TransactionType);

                    Console.WriteLine(transaction.ProcessType);

                    Console.WriteLine(transaction.TransactionTime);

                    foreach (var details in transaction.ParamAndValue)
                    {
                        // Buraya business kurallar gelecek.
                        if (details.Key == "003" && details.Value == "000000")
                        {
                            Console.WriteLine("Bu işlem satış işlemidir");

                        }

                    }

                    Console.WriteLine("-----------------------------------------------------------------------\r\n");


                }

                else
                {
                    continue;
                }


                //foreach (var details in transaction.ParamAndValue)
                //{
                //    // Buraya business kurallar gelecek.
                //    if (details.Key == "003" && details.Value=="000000" )
                //    {
                //        Console.WriteLine("Bu işlem satış işlemidir");

                //    }

                //}

                //Console.WriteLine("-----------------------------------------------------------------------\r\n");
            }
        }
    }
}
