using System;
using System.Diagnostics;   // Это пространство имен требуется для работы с классом Process.
using System.Reflection;    // Это пространство имен требуется для работы с классом Assembly.
using System.Threading;

namespace Threads
{
    class ApplicationDomainDemo
    {
        public static void Demo()
        {
            DomainInfo();
            ShowAssemblies();
            ShowThreads();
        }
        public static void DomainInfo()
        {
            AppDomain appD1 = AppDomain.CurrentDomain;
            Console.WriteLine($"Base Directory: {appD1.BaseDirectory}");
            Console.WriteLine($"Current Thread Domain: {appD1.FriendlyName}");
            Console.WriteLine($"Domain ID (вместо устаревшего AppDomain.CurrentDomain.GetCurrentThreadId()): {Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine();

            AppDomain appD2 = Thread.GetDomain();
            Console.WriteLine($"Base Directory: {appD2.BaseDirectory}");
            Console.WriteLine($"Current Thread Domain: {appD2.FriendlyName}");
            Console.WriteLine($"Domain ID (вместо устаревшего AppDomain.CurrentDomain.GetCurrentThreadId()): {Thread.CurrentThread.ManagedThreadId}");
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

        public static void ShowThreads()
        {
            Console.WriteLine(Environment.NewLine + "Process Threads:");
            Process proc = Process.GetCurrentProcess();
            foreach (ProcessThread aPhysicalThread in proc.Threads)
                Console.WriteLine(aPhysicalThread.Id.ToString() + " : " + aPhysicalThread.ThreadState);
        }

    }
}
