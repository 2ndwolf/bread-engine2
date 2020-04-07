using System;
using System.IO;
using System.Text;

namespace converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            FileStream fs = File.OpenRead("../client/wwwroot/assets/test_map.json");
            byte[] b = new byte[1024];
            UTF8Encoding temp = new UTF8Encoding(true);

            while (fs.Read(b,0,b.Length) > 0) 
            {
                Console.WriteLine(temp.GetString(b));
            }
        }
    }
}
