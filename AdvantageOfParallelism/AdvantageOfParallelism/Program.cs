using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AdvantageOfParallelism
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchMark(EmptyForTest, nameof(EmptyForTest));
            BenchMark(EmptyParallelTest, nameof(EmptyParallelTest));
            BenchMark(LinearFactorialTest, nameof(LinearFactorialTest));
            BenchMark(TasksFactorialTest, nameof(TasksFactorialTest));
            BenchMark(ParallelFactorialTest, nameof(ParallelFactorialTest));

            Console.WriteLine();
            Console.WriteLine("end...");
            Console.ReadLine();
        }

        static void BenchMark(Action<int> action, string name, int timesToIterate = 1000000)
        {
            var stopwatch = new Stopwatch();

            //pause to stabilize the system
            Thread.Sleep(1000);
            stopwatch.Start();

            action(timesToIterate);

            stopwatch.Stop();
            Console.WriteLine($"{name} elapsed milliseconds: {stopwatch.ElapsedMilliseconds}");
        }

        static void EmptyForTest(int timesToIterate)
        {
            for (int i = 0; i < timesToIterate; i++) ;
        }

        static void EmptyParallelTest(int timesToIterate)
        {
            Parallel.For(0, timesToIterate, i => { });
        }

        static int GetFactorial(int n)
        {
            return (n >= 1) ? (n * GetFactorial(n - 1)) : 1;
        }

        static void LinearFactorialTest(int timesToIterate)
        {
            for (int i = 0; i < timesToIterate; i++)
                GetFactorial(900);
        }

        static void TasksFactorialTest(int timesToIterate)
        {
            var tasks = new Task[timesToIterate];

            for (int i = 0; i < timesToIterate; i++)
                tasks[i] = Task.Factory.StartNew(() => GetFactorial(900));

            Task.WaitAll(tasks);
        }

        static void ParallelFactorialTest(int timesToIterate)
        {
            Parallel.For(0, timesToIterate, i => GetFactorial(900));
        }
    }
}
