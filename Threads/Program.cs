using System;
using System.Diagnostics; // Это пространство имен требуется для работы с классом Process.
using System.Reflection; // Это пространство имен требуется для работы с классом Assembly.
using System.Threading;

namespace Threads
{
    class Program
    {
        static void Main(string[] args)
        {
            
            MyAppDomain.DomainInfo();
            MyAppDomain.ShowAssemblies();
            MyAppDomain.ShowThreads();
          

            StartClass.StartThreadInfo();
          


            //          Яйцо или курица ----------------
            Thread eggThread = new Thread(new ThreadStart(EggVoice.Start));
            Thread chickenThread = new Thread(new ThreadStart(ChickenVoice.Start));

            eggThread.Start();
            chickenThread.Start();
            


            //          Запуск потока                       
            Worker w0 = new Worker(0, 10000);
            Worker w1 = new Worker(1, 10000);
            ThreadStart t0, t1;
            t0 = new ThreadStart(w0.DoItEasy);
            t1 = new ThreadStart(w1.DoItEasy);

            Thread th0, th1;
            th0 = new Thread(t0);
            th1 = new Thread(t1);
            // При создании потока не обязательно использовать делегат.
            // Возможен и такой вариант. Главное — это сигнатура функции.
            th0.Start();
            th1.Start();
            



            ThreadStart myThreadDelegate = new ThreadStart(ThreadWork.DoWork);
            Thread myThread = new Thread(myThreadDelegate);
            



            //          Отстранение потока от выполнения
            myThread.Start();
            Thread.Sleep(10);
            
            Console.WriteLine("Main - aborting my thread.");
            myThread.Suspend();
            Console.WriteLine("Main ending.");
            


            
            // Прерывание потока
            //1. Мероприятия по организации вторичного потока!
            myThreadDelegate = new ThreadStart(ThreadWork.DoMoreWork);
            myThread = new Thread(myThreadDelegate);
            //2. Вторичный поток стартовал!
            myThread.Start();

            //3. А вот первичный поток – самоусыпился!
            //   И пока первичный поток спит, вторичный поток – работает!
            Thread.Sleep(50);

            //5. Но вот первичный поток проснулся – и первое, что он
            //   делает, – это прерывает вторичный поток! 
            Console.WriteLine("Main – aborting my thread.");
            myThread.Abort();

            //9. Теперь везде все дела посворачивали... 
            Console.WriteLine("Main ending.");





            // Метод Join()
            myThreadDelegate = new ThreadStart(ThreadWork.DoMoreWork);
            myThread = new Thread(myThreadDelegate);
            myThread.Start();
            Thread.Sleep(100);
            //myThread.Join(); // Закомментировать вызов метода и осознать разницу.
            Console.WriteLine("Main ending.");
            



            // ClassWork - остановка потока извне вызовом его метода
            ThreadTest tt = new ThreadTest("t1");
            ThreadStart myThreadDel = new ThreadStart(tt.Run);
            Thread myThread1 = new Thread(myThreadDel);
            myThread1.Start();

            Thread.Sleep(2000);
            tt.suspend = true;
        }
    }

    class MyAppDomain
    {
        public static void DomainInfo()
        {
            //  AppDomain appD1 = AppDomain.CurrentDomain;
            AppDomain appD1 = Thread.GetDomain();

            Console.WriteLine($"Base Directory: {appD1.BaseDirectory}");
            Console.WriteLine($"Current Thread Domain: {appD1.FriendlyName}");
            Console.WriteLine($"Domain ID (вместо устаревшего AppDomain.CurrentDomain.GetCurrentThreadId()): {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        public static void ShowThreads()
        {
            Console.WriteLine(Environment.NewLine + "Process Threads:");
            Process proc = Process.GetCurrentProcess();
            foreach (ProcessThread aPhysicalThread in proc.Threads)
                Console.WriteLine(aPhysicalThread.Id.ToString() + ":" + aPhysicalThread.ThreadState);
        }

        public static void ShowAssemblies()
        {
            Console.WriteLine(Environment.NewLine + "Assemblies:");

            AppDomain ad = Thread.GetDomain(); // Получили ссылку на домен.

            // В рамках домена может быть множество сборок. Можно получить список сборок домена.
            Assembly[] loadedAssemblies = ad.GetAssemblies();

            // У домена имеется FriendlyName, которое ему присваивается при создании. При этом у него даже нет доступного конструктора.
            Console.WriteLine($"Assemblies in {ad.FriendlyName} domain:");
            foreach (Assembly assembly in loadedAssemblies)
                Console.WriteLine(assembly.FullName);
        }
    }

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

    class Worker
    {
        int timeToWork;
        int n;

        public Worker()
        {
            n = 0;
            timeToWork = 0;
        }
        // Конструктор с параметрами...
        public Worker(int nKey, int tKey)
        {
            n = nKey;
            timeToWork = tKey;
        }

        public void DoItEasy() // Тело рабочей функции...	
        {
            int i;
            for (i = 0; i < timeToWork; i++)
            {
                if (n == 0)
                    Console.Write("{0,25}\r", i);
                else
                    Console.Write("{0,10}\r", i);
            }
            Console.WriteLine("\nWorker was here!");
        }
    }

    public class ThreadWork
    {
        public static void DoWork()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Thread – working.");
                Thread.Sleep(25);
            }

            Console.WriteLine("Thread - still alive and working.");
            Console.WriteLine("Thread - finished working.");
        }

        public static void DoMoreWork()
        {
            int i;

            try
            {
                for (i = 0; i < 100; i++)
                {
                    //4. Вот скромненько так работает...
                    //   Поспит немножко – а потом опять поработает.
                    //   Take Your time! 100 раз прокрутиться надо 
                    //   вторичному потоку до нормального завершения.
                    Console.WriteLine($"Thread – working {i}.");
                    Thread.Sleep(10);
                }
            }
            catch (ThreadAbortException e)
            {
                //6.
                //– Ну дела! А где это мы...
                Console.WriteLine("Thread – caught ThreadAbortException – resetting.");
                Console.WriteLine($"Exception message: {e.Message}");
                //  (Голос сверху)
                //– Вы находитесь в блоке обработки исключения, связанного с непредвиденным завершением потока.
                //– Понятно... Значит, не успели. "Наверху" сочли нашу деятельность
                //  нецелесообразной и не дали (потоку) завершить до конца начатое дело!
                Thread.ResetAbort();
                //   (Перехватывают исключение и отменяют остановку потока)
                //   Будем завершать дела. Но будем делать это как положено,
                //   а не в аварийном порядке. Нам указали на дверь, но мы
                //   уходим достойно!
                //   (Комментарии постороннего)  
                //   А чтобы стал понятен альтернативный исход – надо
                //   закомментировать строку с оператором отмены остановки потока. 
            }
            finally
            {
                //7.
                //– Вот где бы мы остались, если бы не удалось отменить
                // остановку потока! finally блок... Отстой! 
                Console.WriteLine("Thread – in finally statement.");
            }
            //8. 
            // – А вот преждевременный, но достойный уход.
            //   Мы не довели дело до конца только потому, что нам не дали
            //   сделать этого. Обстоятельства бывают выше. Уходим достойно. 
            Console.WriteLine("Thread – still alive and working.");
            Console.WriteLine("Thread – finished working.");
        }
    }
}
