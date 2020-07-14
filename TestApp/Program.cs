using System;
using System.Diagnostics;

namespace TestApp
{
    using SimpleKeyValueStore;

    class Program
    {
        private static SimpleKeyValueStore kvs = new SimpleKeyValueStore();

        static void Main(string[] args)
        {
            Benchmark(DoStuff, 10_000_000);

            Console.ReadKey();
        }

        private static void DoStuff()
        {

            //kvs.Set("hello", "world 3");
            //kvs.Flush();

            kvs.TryGet("hello");

            //(string message, bool ok) = kvs.TryGet("hello");
            //if (ok)
            //{
            //    Console.WriteLine("got value: " + message);
            //}
            //else
            //{
            //    Console.WriteLine("could not get value");
            //}
        }

        private static void Benchmark(Action act, int interval)
        {
            GC.Collect();
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < interval; i++)
            {
                act.Invoke();
            }
            sw.Stop();
            Console.WriteLine("Elapsed: " + sw.ElapsedMilliseconds + "ms");
        }
    }
}
