using System;

namespace Threads
{
    class ThreadWorker
    {
        int _timeToWork;
        int _n;

        public ThreadWorker()
        {
            _n = 0;
            _timeToWork = 0;
        }
        // Конструктор с параметрами...
        public ThreadWorker(int n, int time)
        {
            _n = n;
            _timeToWork = time;
        }

        public void DoItEasy() // Тело рабочей функции...	
        {
            int i;
            for (i = 0; i < _timeToWork; i++)
            {
                if (_n == 0)
                    Console.Write("{0,25}\r", i);
                else
                    Console.Write("{0,10}\r", i);
            }
            Console.WriteLine("\nThreadWorker was here!");
        }
    }
}
