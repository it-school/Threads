using System;
using System.Threading;

namespace Threads
{    class ThreadTest
    {
        string state;
        public bool suspend;
        public ThreadTest(string state)
        {
            this.state = state;
            this.suspend = false;
        }

        public void Run()
        {
            while (!this.suspend)
            {
                Console.WriteLine(state);
                Thread.Sleep(100);
            }

            Console.WriteLine("Correct finish");
        }
    }
}
