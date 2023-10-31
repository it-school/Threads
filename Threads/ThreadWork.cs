using System;
using System.Threading;

namespace Threads
{
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
                    //   Take Your time! 100 раз прокрутиться надо вторичному потоку до нормального завершения.
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
                //– Понятно... Значит, не успели. "Наверху" сочли нашу деятельность нецелесообразной
                //  и не дали (потоку) завершить до конца начатое дело!
                Thread.ResetAbort();
                //   (Перехватывают исключение и отменяют остановку потока)
                //   Будем завершать дела. Но будем делать это как положено, а не в аварийном порядке.
                //   Нам указали на дверь, но мы уходим достойно!
                //   (Комментарии постороннего)  
                //   А чтобы стал понятен альтернативный исход – надо закомментировать строку с оператором отмены остановки потока. 
            }
            finally
            {
                //7.
                //– Вот где бы мы остались, если бы не удалось отменить остановку потока! finally блок... Отстой! 
                Console.WriteLine("Thread – in finally statement.");
            }
            //8. 
            // – А вот преждевременный, но достойный уход.
            //   Мы не довели дело до конца только потому, что нам не дали сделать этого. Обстоятельства бывают выше. Уходим достойно. 
            Console.WriteLine("Thread – still alive and working.");
            Console.WriteLine("Thread – finished working.");
        }
    }
}
