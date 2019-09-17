using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Obfuscator
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0) RunConsoleInterface(args);
            else RunWinFormInterface();
        }

        private static void RunConsoleInterface(string[] args)
        {
            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"ERROR: File {args[0]} not found");
                PrintHelpOnConsole();
                return;
            }

        }

        private static void PrintHelpOnConsole()
        {
            Console.WriteLine("USAGE:");
            Console.WriteLine("\tObfuscator [filename]");
            Console.WriteLine();
            Console.WriteLine("\tIf NO filename is provided, a graphic UI is shown");
            Console.WriteLine("\tIf a filename is provided, the obfuscation operations inside that filename will be executed");
            Console.WriteLine();
        }

        private static void RunWinFormInterface()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
