using System;
using System.Threading.Tasks;
using static System.Console;
using System.Numerics;


namespace TaskDelayTimeout
{
    class Program                                                                   //this solution is worse than cancellationtoken, because 2nd thread
                                                                                    //is still working even if we skip its result
    {
        static async Task <BigInteger> Timeout(uint factorialNumber, TimeSpan maxDelay)
        {
            var readFromConsole = Factorial(factorialNumber);

            /*var test = Task.Run(() =>
            {
                BigInteger result = 1;
                for (uint i = 1; i <= factorialNumber; i++)
                {
                    result *= i;
                }
                return result;
            });*/

            var timeoutTask = Task.Delay(5000);

            var completedTask = await Task.WhenAny(readFromConsole, timeoutTask);              //or (test, timeoutTask)

            if (completedTask == timeoutTask)
            {
                return 0;
            }
            else
            {
                return await readFromConsole;                                                  //or return await test
            }
        }
                
        static async Task<BigInteger> Factorial(uint f)
        {
            return await Task.Run(() =>
            {
                 BigInteger result = 1;
                 for (uint i = 1; i <= f; i++)
                 {
                     result *= i;
                 }
                 return result;
            });
        }
        

        static async Task Main(string[] args)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, 500);
            var r = await Timeout(10000, ts);
            WriteLine(r);
            ReadKey();
        }
    }
}
