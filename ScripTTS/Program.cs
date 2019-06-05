using System;

namespace ScriptSeparator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("NOTE: line which starts with '-' is title of each separated script.");
            Console.WriteLine("Input txt file path: ");

            string filePath = Console.ReadLine();

            try
            {
                new Recorder(filePath).Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }
}
