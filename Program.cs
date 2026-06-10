using System;
using System.IO;
using System.Windows.Forms;

namespace Practice17_18_MultiWindowCalculator_Maxim;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        if (args.Length > 0 && args[0] == "--screenshots")
        {
            string folder = args.Length > 1 ? args[1] : Directory.GetCurrentDirectory();
            Directory.CreateDirectory(folder);

            using CalculatorForm form = new();
            form.MakeScreens(folder);
            return;
        }

        Application.Run(new CalculatorForm());
    }
}
