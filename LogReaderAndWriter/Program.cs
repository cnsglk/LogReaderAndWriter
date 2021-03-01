using System;

namespace LogReaderAndWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = @"C:\Users\AmadeusUser\Desktop\JA7400068235_20210118.log";

            var logReader = new LogReader(fileName);

            logReader.WriteLogInfo();




            Console.ReadKey();
        }
    }
}
