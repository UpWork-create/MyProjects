using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Lab7
{
    class RationalNumber:IComparable<RationalNumber>
    {
        private int Numerator;
        private int _Denominator;
        private int Denominator
        {
            get { return _Denominator; }
            set
            {
                if (value == 0)
                    throw new Exception("Div by zero");
                else if (value < 0)
                    throw new Exception("Denominator can be only positive number");
                else
                    _Denominator = value;
            }
        }

        public RationalNumber(int Numerator,int Denominator)
        {
            this.Numerator = Numerator;
            this.Denominator = Denominator;
        }

        public static RationalNumber operator+(RationalNumber number1, RationalNumber number2)
        {
            int NewDenominator = AppliedMath.NOK(number1._Denominator,number2._Denominator);
            int NOD = Math.Abs(AppliedMath.NOD(number1.Numerator * NewDenominator / number1.Denominator + number2.Numerator * NewDenominator / number2.Denominator, NewDenominator));
            return new RationalNumber((number1.Numerator*NewDenominator/number1.Denominator+number2.Numerator * NewDenominator / number2.Denominator)/NOD,NewDenominator/NOD);
        }
        public static RationalNumber operator+(RationalNumber number1, int number2)
        {
            int NOD = Math.Abs(AppliedMath.NOD(number1.Numerator + number2 * number1.Denominator, number1.Denominator));
            return new RationalNumber((number1.Numerator + number2 * number1.Denominator)/NOD, number1.Denominator/NOD);
        }

        public static RationalNumber operator -(RationalNumber number1, RationalNumber number2)
        {
            int NewDenominator = AppliedMath.NOK(number1._Denominator, number2._Denominator);
            int NOD = Math.Abs(AppliedMath.NOD(number1.Numerator * NewDenominator / number1.Denominator - number2.Numerator * NewDenominator / number2.Denominator, NewDenominator));
            return new RationalNumber((number1.Numerator * NewDenominator / number1.Denominator - number2.Numerator * NewDenominator / number2.Denominator)/NOD, NewDenominator/NOD);
        }
        public static RationalNumber operator -(RationalNumber number1, int number2)
        {
            int NOD = Math.Abs(AppliedMath.NOD(number1.Numerator - number2 * number1.Denominator, number1.Denominator));
            return new RationalNumber((number1.Numerator - number2 * number1.Denominator)/NOD, number1.Denominator/NOD);
        }

        public static RationalNumber operator *(RationalNumber number1, RationalNumber number2)
        {
            int NOD = Math.Abs(AppliedMath.NOD(number1.Numerator * number2.Numerator, number1.Denominator * number2.Denominator));
            return new RationalNumber((number1.Numerator*number2.Numerator)/NOD,(number1.Denominator*number2.Denominator)/NOD);
        }
        public static RationalNumber operator *(RationalNumber number1, int number2)
        {
            int NOD = Math.Abs(AppliedMath.NOD(number1.Numerator * number2, number1.Denominator));
            return new RationalNumber(number1.Numerator * number2/NOD, number1.Denominator/NOD);
        }

        public static RationalNumber operator /(RationalNumber number1, RationalNumber number2)
        {
            int NOD = Math.Abs(AppliedMath.NOD(number1.Numerator * number2.Denominator, number1.Denominator * number2.Numerator));
            return new RationalNumber(number1.Numerator * number2.Denominator/NOD, number1.Denominator * number2.Numerator/NOD);
        }
        public static RationalNumber operator /(RationalNumber number1, int number2)
        {
            RationalNumber temp = new RationalNumber(number2, 1);
            int NOD = Math.Abs(AppliedMath.NOD(number1.Numerator * temp.Denominator, number1.Denominator * temp.Numerator));
            return new RationalNumber(number1.Numerator * temp.Denominator/NOD, number1.Denominator * temp.Numerator/NOD);
        }

        public static RationalNumber operator++(RationalNumber number1)
        {
            return new RationalNumber(number1.Numerator + number1.Denominator, number1.Denominator);
        }

        public static RationalNumber operator --(RationalNumber number1)
        {
            return new RationalNumber(number1.Numerator - number1.Denominator, number1.Denominator);
        }

        public static bool operator==(RationalNumber number1, RationalNumber number2)
        {
            return (number1.Numerator * number2.Denominator == number2.Numerator * number1.Denominator);
        }
        public static bool operator !=(RationalNumber number1, RationalNumber number2)
        {
            return (number1.Numerator * number2.Denominator != number2.Numerator * number1.Denominator);
        }
        public static bool operator ==(RationalNumber number1,int number2)
        {
            RationalNumber temp = new RationalNumber(number2, 1);
            return (number1.Numerator * temp.Denominator == temp.Numerator * number1.Denominator);
        }
        public static bool operator !=(RationalNumber number1,int number2)
        {
            RationalNumber temp = new RationalNumber(number2, 1);
            return (number1.Numerator * temp.Denominator != temp.Numerator * number1.Denominator);
        }

        public static bool operator<(RationalNumber number1, RationalNumber number2)
        {
            return (number1.Numerator * number2.Denominator < number2.Numerator * number1.Denominator);
        }
        public static bool operator >(RationalNumber number1, RationalNumber number2)
        {
            return (number1.Numerator * number2.Denominator > number2.Numerator * number1.Denominator);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) throw new Exception("Wrong operand");

            RationalNumber temp = (RationalNumber)obj;
            return this == temp;
        }

        public override int GetHashCode()
        {
            return (Convert.ToDouble(Numerator) / Convert.ToDouble(Denominator)).GetHashCode();
        }

        public override string ToString()
        {
            if (Numerator == 0)
                return "0";
            else
                return ($"{Numerator}/{Denominator}");
        }
        public string ToString(string Format)
        {
            if (Format == string.Empty)
                return ToString();
            else if (Format == "with fractions")
            {
                string temp = string.Empty;
                int NOD = Math.Abs(AppliedMath.NOD(Numerator, Denominator));
                for (int i = 0; i < (Numerator / NOD).ToString().Length; i++)
                    temp = string.Concat(temp, '-');

                if (Numerator >= 0)
                    return ($"{Numerator / NOD}\n{temp}\n{Denominator / NOD}");
                else
                {
                    temp = temp.Remove(1, 1);
                    return ($"{Numerator / NOD}\n {temp}\n {Denominator / NOD}");
                }
            }
            else if (Format == "mixed fraction")
            {
                if (Math.Abs(Numerator) < Denominator)
                    return ToString();
                else if (Numerator % Denominator == 0)
                    return (Numerator / Denominator).ToString();
                else
                {
                    if (Numerator == 0)
                        return "0";
                    else
                    {
                        int NOD = Math.Abs(AppliedMath.NOD(Numerator - Denominator * (Numerator / Denominator), Denominator));
                        return $"{Numerator / Denominator}+({(Numerator - Denominator * (Numerator / Denominator)) / NOD}/{Denominator / NOD})";
                    }
                }
            }
            else if (Format == "reduced fraction")
            {
                int NOD = Math.Abs(AppliedMath.NOD(Numerator, Denominator));
                return ($"{Numerator / NOD}/{Denominator / NOD}");
            }
            return string.Empty;
        }

        public int CompareTo(RationalNumber other)
        {
            if (this < other)
                return -1;
            else if (this > other)
                return 1;
            else
                return 0;
        }

        public static implicit operator int(RationalNumber number)
        {
            return number.Numerator / number.Denominator;
        }
        public static implicit operator double(RationalNumber number)
        {
            return (Convert.ToDouble(number.Numerator) / Convert.ToDouble(number.Denominator));
        }
        public static explicit operator RationalNumber(int number)
        {
            return new RationalNumber(number, 1);
        }
        public static explicit operator RationalNumber(double number)
        {
            int Index = 1;
            while(number%1!=0)
            {
                number *= 10;
                Index *= 10;
            }
            return new RationalNumber(Convert.ToInt32(number), Index);
        }

        public static RationalNumber ConvertToRationalNum(string number)
        {
            if(number.Contains('/'))
            {
                string Numerator = string.Empty;
                string Denominator = string.Empty;
                int Index = 0;
                while(number[Index]!='/')
                {
                    Numerator=string.Concat(Numerator,number[Index]);
                    Index++;
                }
                Index++;
                while(Index<number.Length)
                {
                    Denominator=string.Concat(Denominator, number[Index]);
                    Index++;
                }
                return new RationalNumber(Convert.ToInt32(Numerator), Convert.ToInt32(Denominator));
            }
            else if(number.Contains(','))
            {
                double temp = Convert.ToDouble(number);
                return (RationalNumber)temp;
            }
            else             
                return new RationalNumber(Convert.ToInt32(number), 1);
        }
    }
}
