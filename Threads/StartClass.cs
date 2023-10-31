using System;
using System.Threading;

namespace Threads
{
    class StartClass
    {
        public static void StartThreadInfo()
        {
            int i = 0;
            bool isNamed = false;
            do
            {
                try
                {
                    if (Thread.CurrentThread.Name == null)
                    {
                        Console.Write("Get the name for current Thread > ");
                        Thread.CurrentThread.Name = Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine($"Current Thread : {Thread.CurrentThread.Name}.");
                        Console.WriteLine($"Запущен ли поток: {Thread.CurrentThread.IsAlive}");
                        Console.WriteLine($"Приоритет потока: {Thread.CurrentThread.Priority}");
                        Console.WriteLine($"Состояние потока: {Thread.CurrentThread.ThreadState}");
                        Console.WriteLine($"Домен приложения: {Thread.GetDomain().FriendlyName}");
                        if (!isNamed)
                        {
                            Console.Write("Rename it. Please...");
                            Thread.CurrentThread.Name = Console.ReadLine();
                        }
                    }
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine($"{e}:{e.Message}");
                    isNamed = true;
                }

                i++;
            }
            while (i < 2);
        }
    }
}
