using System;
using System.Collections.Generic;
using System.Text;

namespace Lab7
{
    static class ServiceClass
    {
        public static void PrintLine(string line)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(line);
            Console.ResetColor();
        }
        public static void PrintArray(object []arr)
        {
            for(int i=0;i<arr.Length;i++)
                Console.Write($"{arr[i].ToString()}  ");
        }
    }
}
