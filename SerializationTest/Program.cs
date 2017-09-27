using System;
using System.IO;

namespace SerializationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ListRand debugList = new ListRand();
            debugList.Add("zero");
            debugList.Add(null);
            debugList.Add("two");
            debugList.Add("");

            Console.WriteLine("Original list");
            Console.WriteLine(debugList);


            using (FileStream fileStream = File.Create("binarydata.dat"))
            {
                debugList.Serialize(fileStream);
            }
            using (FileStream fileStream = File.OpenRead("binarydata.dat"))
            {
                debugList.Deserialize(fileStream);
            }

            Console.WriteLine("\nSerialized & desirialized list");
            Console.WriteLine(debugList);

            Console.ReadKey();
        }
    }
}
