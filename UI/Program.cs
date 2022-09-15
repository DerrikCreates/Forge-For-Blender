using System.Numerics;
using Serilog;
using Serilog.Formatting.Compact;

namespace DerriksForgeTools;

using System.Runtime.InteropServices;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    ///
    ///
    [STAThread]
    static void Main()
    {
        AllocConsole();
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.File(new CompactJsonFormatter(),
                "Logs/log.json",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();


        Log.Information("Starting App");
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());

       
    }


    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool AllocConsole();
}