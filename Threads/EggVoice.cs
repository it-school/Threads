using System;
using System.Threading;

namespace Threads
{
    public class EggVoice
    {
        public static void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    Thread.Sleep(new TimeSpan(0, 0, 1));        //Приостанавливает поток на 1 сек
//                  Thread.Sleep(new TimeSpan(0, 0, 0, 1));     //Приостанавливает поток на 1 сек
//                  Thread.Sleep(new TimeSpan(0, 0, 0, 1, 0));  //Приостанавливает поток на 1 сек
                }
                catch (Exception) { }
                Console.WriteLine("egg!");
            }
            // Слово «яйцо» сказано 5 раз
        }
    }

    public class ChickenVoice
    {
        public static void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    Thread.Sleep(1000); //Приостанавливает поток на 1 сек
                }
                catch (Exception) { }

                Console.WriteLine("chicken!");
            }
            //Слово «курица» сказано 5 раз
        }
    }
}