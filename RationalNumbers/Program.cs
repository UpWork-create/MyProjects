using System;

namespace Lab7
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RationalNumber number1 = new RationalNumber(-147, 8);
                RationalNumber number2 = new RationalNumber(4, 16);
                RationalNumber number3 = new RationalNumber(2, 4);
                RationalNumber number4 = new RationalNumber(-18, 4);
                RationalNumber number5 = RationalNumber.ConvertToRationalNum("0,25");
                RationalNumber number6 = RationalNumber.ConvertToRationalNum("-5/7");
                RationalNumber number7 = RationalNumber.ConvertToRationalNum("-5,00");
                RationalNumber []NumberList = new RationalNumber[] { number1, number2, number3, number4, number5, number6,number7 };

                ServiceClass.PrintLine("Math Operations:");
                Console.WriteLine($"({number1})+({number2})={number1+number2}");
                Console.WriteLine($"({number2})-({number5})={number2 - number5}");
                Console.WriteLine($"({number3})*({number4})={number3*number4}");
                Console.WriteLine($"({number1})/({number2})={number1/number2}\n");

                ServiceClass.PrintLine("Comparison operations:");
                Console.WriteLine($"({number1})=({number2})-->{number1==number2}");
                Console.WriteLine($"({number5})=({number4})-->{number5 == number4}");
                Console.WriteLine($"({number5})<({number6})-->{number5<number6}");
                Console.WriteLine($"({number3})>({number5})-->{number3 > number5}\n");

                ServiceClass.PrintLine("Casting Operaions:");
                int Inumber = number4;
                double Dnumber = number1;
                Console.WriteLine(Inumber);
                Console.WriteLine(Dnumber+"\n");

                ServiceClass.PrintLine("IComparable:");
                ServiceClass.PrintArray(NumberList);
                Console.WriteLine();
                Array.Sort(NumberList);
                ServiceClass.PrintArray(NumberList);
                Console.WriteLine("\n");

                ServiceClass.PrintLine("Equal:");
                Console.WriteLine($"({number1}) Equals ({number2})--->{number1.Equals(number2)}");
                Console.WriteLine($"({number3}) Equals ({number5})--->{number3.Equals(number5)}\n");

                ServiceClass.PrintLine($"Different string formats of ({number4}):");
                Console.WriteLine(number4.ToString("reduced fraction") + "\n");
                Console.WriteLine(number4.ToString("mixed fraction") + "\n");
                Console.WriteLine(number4.ToString("with fractions") + "\n");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.TargetSite);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
