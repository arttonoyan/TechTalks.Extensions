using System;
using System.Threading.Tasks;

namespace TechTalks.Hangfire.ConsoleTestApp
{
    public interface IMyService
    {
        void TestMethod(int i);
    }

    public class MyService : IMyService
    {
        public void TestMethod(int i)
        {
            Console.WriteLine($"Barev {i}");
            Task.Delay(1000).GetAwaiter().GetResult();
        }
    }
}
