using System;
using System.Collections.Generic;
using System.Text;

namespace Lab7
{
    static class AppliedMath
    {
        public static int NOD(int num1,int num2)
        {
            return num2 != 0 ? NOD(num2, num1 % num2) : num1; ;
        }
        public static int NOK(int num1, int num2)
        {
            return num1 / NOD(num1, num2)*num2;
        }
    }
}
