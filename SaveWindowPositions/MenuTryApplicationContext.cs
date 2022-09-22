using SaveWindowPositions.Logic;
using System;
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

            var autoWorkMenuItem = _notifyIcon.ContextMenuStrip.Items.Add("Działanie automatyczne");
            autoWorkMenuItem.Image = Properties.Resources.CheckIcon;
            autoWorkMenuItem.Click += AutoWorkClick;

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
            _notifyIcon.Visible = false;

            Application.Exit();
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
