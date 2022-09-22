using SaveWindowPositions.FileOperations;
using SaveWindowPositions.Logic;
using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SaveWindowPositions
{
    internal class MenuTryApplicationContext : ApplicationContext
    {
        readonly NotifyIcon _notifyIcon = new NotifyIcon();

        public MenuTryApplicationContext()
        {
            _notifyIcon.Icon = Properties.Resources.ApplicationIcon;
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.MouseClick += (object sender, MouseEventArgs e) =>
            {
                if (e.Button is MouseButtons.Left)
                {
                    MethodInfo methodInfo = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                    methodInfo.Invoke(_notifyIcon, null);
                }
            };

            var autoWork = DataOperations.RestoreApplicationData().AutoRestoreState;
            var autoWorkMenuItem = _notifyIcon.ContextMenuStrip.Items.Add("Działanie automatyczne");
            if (autoWork)
            {
                autoWorkMenuItem.Image = Properties.Resources.CheckIcon;
            }
            autoWorkMenuItem.Click += AutoWorkClick;

            _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            var saveMenuItem = _notifyIcon.ContextMenuStrip.Items.Add("Zapisz");
            saveMenuItem.Image = Properties.Resources.SaveIcon;
            saveMenuItem.Click += SaveClick;

            var restoreMenuItem = _notifyIcon.ContextMenuStrip.Items.Add("Przywróć");
            restoreMenuItem.Image = Properties.Resources.RestoreIcon;
            restoreMenuItem.Click += RestoreClick;

            _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            var exitMenuItem = _notifyIcon.ContextMenuStrip.Items.Add("Zamknij program");
            exitMenuItem.Image = Properties.Resources.CloseIcon;
            exitMenuItem.Click += CloseClick;

            _notifyIcon.Visible = true;
        }

        private void AutoWorkClick(object sender, EventArgs e)
        {
            var newStateForAutoWork = !DataOperations.RestoreApplicationData().AutoRestoreState;

            if (newStateForAutoWork)
            {
                _notifyIcon.ContextMenuStrip.Items[0].Image = Properties.Resources.CheckIcon;
                DataOperations.SaveApplicationData(true);
            }
            else
            {
                _notifyIcon.ContextMenuStrip.Items[0].Image = null;
                DataOperations.SaveApplicationData(false);
            }
        }

        private void SaveClick(object sender, EventArgs e)
        {
            WindowsHandlers.SaveWindows();
        }

        private void RestoreClick(object sender, EventArgs e)
        {
            WindowsHandlers.RestoreWindows();
        } 

        private void CloseClick(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;

            Application.Exit();
        }
    }
}
