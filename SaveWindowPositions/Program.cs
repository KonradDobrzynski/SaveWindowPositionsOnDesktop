using Microsoft.Win32;
using SaveWindowPositions.FileOperations;
using SaveWindowPositions.Logic;
using System;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace SaveWindowPositions
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RegisterAutoWork();

            Application.Run(new MenuTryApplicationContext());
        }

        private static void RegisterAutoWork()
        {
            SystemEvents.DisplaySettingsChanged += (object sender, EventArgs e) =>
            {
                if (DataOperations.RestoreApplicationData().AutoRestoreState)
                {
                    Thread.Sleep(3000);
                    WindowsHandlers.RestoreWindows();
                }
            };

            Timer saveWindowsStateTimer = new Timer();
            saveWindowsStateTimer.Interval = 5000;
            saveWindowsStateTimer.Tick += (object sender, EventArgs e) =>
            {
                if (DataOperations.RestoreApplicationData().AutoRestoreState)
                {
                    WindowsHandlers.SaveWindows();
                }
            };
            saveWindowsStateTimer.Start();
        }
    }
}
