using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reverse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // set up test data for user to input integer  
            Console.WriteLine("Enter an integer: ");
            int input = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("You entered: " + input);

            // call the method to reverse the integer  
            int reversed = ReverseInt(input);
            Console.WriteLine("The ReverseInt:"+ reversed);

            //call the method to reverse the integer by RerverseInt2
            int reversed2 = ReverseInt2(input);
            Console.WriteLine("The ReverseInt2:" + reversed2);
                 
        }
        public static int ReverseInt(int number)
        {
            char[] charArray = number.ToString().ToCharArray();
            Array.Reverse(charArray);
            return int.Parse(new string(charArray));
        }

        public static int ReverseInt2(int input)
        {
            int reversed = 0;
            while (input != 0)
            {
                int remainder = input % 10;
                reversed = reversed * 10 + remainder;
                input /= 10;
            }
            return reversed;
        }

    }
}
