using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SerializationTest
{
    public static class FileReadWriter
    {
        public static void Write(FileStream fileStream, int intToSerialize)
        {
            byte[] intBytes = BitConverter.GetBytes(intToSerialize);
            fileStream.Write(intBytes, 0, intBytes.Length);
        }

        public static void Write(FileStream fileStream, string stringToSerialize)
        {
            byte[] stringBytes;
            if (stringToSerialize != null)
            {
                stringBytes = Encoding.UTF8.GetBytes(stringToSerialize);
            } else
            {
                stringBytes = Encoding.UTF8.GetBytes(String.Empty);
            }

            byte[] lengthBytes = BitConverter.GetBytes(stringBytes.Length);
            fileStream.Write(lengthBytes, 0, lengthBytes.Length);
            fileStream.Write(stringBytes, 0, stringBytes.Length);
        }

        public static int ReadInt32(FileStream fileStream)
        {
            byte[] intBytes = new byte[4];
            fileStream.Read(intBytes, 0, 4);
            int deserializedInt = BitConverter.ToInt32(intBytes, 0);
            return deserializedInt;
        }

        public static string ReadString(FileStream fileStream)
        {
            int stringLength = ReadInt32(fileStream);
            byte[] stringBytes = new byte[stringLength];
            fileStream.Read(stringBytes, 0, stringLength);
            return Encoding.UTF8.GetString(stringBytes);
        }

        public static void PrintFileBytes(string filename)
        {
            byte[] dataWritten = File.ReadAllBytes(filename);
            foreach (byte b in dataWritten)
            {
                Console.Write("{0:x2} ", b);
            }
            Console.WriteLine(" - {0} bytes", dataWritten.Length);

        }
    }
}
