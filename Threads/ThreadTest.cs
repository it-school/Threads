using System;
using System.Threading;

namespace Threads
{
    class ThreadTest
    {
        private string name;
        public bool abort;
        public ThreadTest(string name)
        {
            this.name = name;
            abort = false;
        }

        public void Run()
        {
            int counter = 0;
            while (!abort)
            {
                Console.WriteLine($"{name}: {counter += 100}");
                Thread.Sleep(100);
            }

            Console.WriteLine("Correct finish");
        }
    }
}
