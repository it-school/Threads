using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Threads
{
    class Program
    {
        /// <summary>
        /// Яйцо или курица
        /// </summary>
        static void EggOrChicken()
        {
            Thread eggThread = new Thread(new ThreadStart(EggVoice.Start));
            Thread chickenThread = new Thread(new ThreadStart(ChickenVoice.Start));

            eggThread.Start();
            chickenThread.Start();
        }

        /// <summary>
        /// Остановка потока извне, стимулируя его к самостоятельному завершению
        /// </summary>
        private static void ThreadSoftStopDemo()
        {
            ThreadTest tt = new ThreadTest("ThreadStartAndSoftStop");
            ThreadStart myThreadDel = new ThreadStart(tt.Run);
            Thread myThread1 = new Thread(myThreadDel);
            myThread1.Start();
            Thread.Sleep(2000);
            tt.abort = true;
        }

        /// <summary>
        /// вычисляет общий размер файлов в каталоге. Он ожидает получить путь к одному каталогу в качестве аргумента и сообщает количество и общий размер файлов в этом каталоге. После подтверждения существования каталога он использует метод Parallel.For для перечисления файлов в этом каталоге и определения их размеров. После этого размер каждого файла добавляется в переменную totalSize. Обратите внимание, что добавление выполняется путем вызова Interlocked.Add, чтобы оно имело форму атомарной операции. В противном случае несколько задач могут одновременно попытаться обновить переменную totalSize.
        /// </summary>
        /// <param name="path"></param>
        static void ParallelDirectorySizeCalculation(string path)
        {
            long totalSize = 0;

            if (!Directory.Exists(path))
            {
                Console.WriteLine("The directory does not exist!");
                return;
            }

            string[] files = Directory.GetFiles(path);
            Parallel.For(0, files.Length,
                         index =>
                         {
                             FileInfo fi = new FileInfo(files[index]);
                             long size = fi.Length;
                             Interlocked.Add(ref totalSize, size);
                         });
            Console.WriteLine("Directory '{0}':", path);
            Console.WriteLine("{0:N0} files, {1:N0} bytes", files.Length, totalSize);
        }

        private static void ThreadWorkingDemo()
        {
            ThreadStart myThreadDelegate = new ThreadStart(ThreadWork.DoWork);
            Thread myThread = new Thread(myThreadDelegate);

            myThread.Start();
            Thread.Sleep(10);

            Console.WriteLine("Main - aborting my thread.");
            myThread.Suspend(); //  Отстранение потока от выполнения (устаревший способ)
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
            myThread.Join(); // Закомментировать вызов метода, чтобы осознать разницу)
            Console.WriteLine("Main ending.");
        }

        /// <summary>
        /// Запуск потока
        /// </summary>
        private static void ThreadStartDemo()
        {
            ThreadWorker w0 = new ThreadWorker(0, 100000);
            ThreadWorker w1 = new ThreadWorker(1, 100000);
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
        }
        static void Main(string[] args)
        {
            // ApplicationDomainDemo.Demo();

            //  StartClass.StartThreadInfo(); // Изменение атрибутов потока

            // ThreadStartDemo();

            // ThreadWorkingDemo();

            // ThreadSoftStopDemo();


            // ---------------------------------------------------


            // EggOrChicken();

            // ParallelDirectorySizeCalculation(@"c:\windows\system32");

            // ParallelDirFilesDemo.Demo(@"C:\OpenServer");

            MultiplyMatrices.Demo();
        }
    }
}
