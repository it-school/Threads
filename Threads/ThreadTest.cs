using System;
using System.Threading;

namespace Threads
{
    class ThreadTest
    {
        private string name;
        public bool suspend;
        public ThreadTest(string name)
        {
            this.name = name;
            suspend = false;
        }

        public void Run()
        {
            int counter = 0;
            while (!suspend)
            {
                Console.WriteLine($"{name}: {counter += 100}");
                Thread.Sleep(100);
            }

            Console.WriteLine("Correct finish");
        }
    }
}
