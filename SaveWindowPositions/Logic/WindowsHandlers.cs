using SaveWindowPositions.FileOperations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Automation;
using static SaveWindowPositions.ExternalItems.ExternalDeclarations;

namespace SaveWindowPositions.Logic
{
    public static class WindowsHandlers
    {
        public static void SaveWindows()
        {
            var allWindowHandlersWithPlacements = GetNormalHandlersWithPlacements();
            allWindowHandlersWithPlacements.AddRange(GetChromeHandlersWithPlacements());

            DataOperations.SaveWindowsData(allWindowHandlersWithPlacements);
        }

        public static void RestoreWindows()
        {
            var savedWindowHandlersWithPlacements = DataOperations.RestoreWindowsData();

            var allWindowHandlersWithPlacements = GetNormalHandlersWithPlacements();
            allWindowHandlersWithPlacements.AddRange(GetChromeHandlersWithPlacements());

            foreach (var item in allWindowHandlersWithPlacements)
            {
                var restoredRecord = savedWindowHandlersWithPlacements.FirstOrDefault(x => x.Handler == item.Handler);

                if (restoredRecord == null)
                {
                    continue;
                }

                var restoredRecordPlacement = restoredRecord.Placement;
                int baseShowCmd = restoredRecordPlacement.showCmd;

                restoredRecordPlacement.showCmd = baseShowCmd + 1;
                SetPlacement(item.Handler, ref restoredRecordPlacement);
                restoredRecordPlacement.showCmd = baseShowCmd;
                SetPlacement(item.Handler, ref restoredRecordPlacement);
            }
        }

        private static List<WindowInformation> GetNormalHandlersWithPlacements()
        {
            List<WindowInformation> results = new List<WindowInformation>();

            var allProcesses = Process.GetProcesses().Where(x => x.MainWindowHandle != (IntPtr)0 && !x.ProcessName.Contains("chrome"));

            foreach (var process in allProcesses)
            {
                results.Add(new WindowInformation(process.MainWindowTitle, process.MainWindowHandle, GetPlacement(process.MainWindowHandle)));
            }

            return results;
        }

        private static List<WindowInformation> GetChromeHandlersWithPlacements()
        {
            List<WindowInformation> results = new List<WindowInformation>();

            Process[] chromeProcesses = Process.GetProcessesByName("chrome");
            List<uint> chromeProcessIds = chromeProcesses.Select(x => (uint)x.Id).ToList();

            EnumWindowsProc enumerateHandle = delegate (IntPtr hWnd, int lParam)
            {
                uint id;
                GetWindowThreadProcessId(hWnd, out id);

                if (chromeProcessIds.Contains(id))
                {
                    var clsName = new StringBuilder(256);
                    var hasClass = GetClassName(hWnd, clsName, 256);
                    if (hasClass)
                    {
                        var maxLength = (int)GetWindowTextLength(hWnd);
                        var builder = new StringBuilder(maxLength + 1);
                        GetWindowText(hWnd, builder, (uint)builder.Capacity);

                        var text = builder.ToString();
                        var className = clsName.ToString();

                        if (!string.IsNullOrWhiteSpace(text) && className.Equals("Chrome_WidgetWin_1", StringComparison.OrdinalIgnoreCase))
                        {
                            results.Add(new WindowInformation(text, hWnd, GetPlacement(hWnd)));
                        }
                    }
                }
                return true;
            };

            EnumDesktopWindows(IntPtr.Zero, enumerateHandle, 0);

            return results;
        }

        private static WINDOWPLACEMENT GetPlacement(IntPtr intPtr)
        {
            AutomationElement root = AutomationElement.FromHandle(intPtr);

            var handle = root.Current.NativeWindowHandle;
            IntPtr windowHandle = new IntPtr(handle);

            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(windowHandle, ref placement);

            //RECT rectangle = new RECT();
            //GetWindowRect(windowHandle, out rectangle);
            //var newRectangle = new Rectangle(rectangle.Left, rectangle.Top, rectangle.Right - rectangle.Left, rectangle.Bottom - rectangle.Top);
            //placement.rcNormalPosition = newRectangle;

            return placement;
        }

        private static void SetPlacement(IntPtr intPtr, ref WINDOWPLACEMENT placement)
        {
            SetWindowPlacement(intPtr, ref placement);
        }
    }
}
