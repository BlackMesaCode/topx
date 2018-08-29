using System;
using System.Windows.Forms;

namespace TopX
{
    public class SystemTray
    {
        public MenuItem ToggleMenuItem { get; set; }
        public MenuItem CloseMenuItem { get; set; }

        public NotifyIcon NotifyIcon { get; }

        public SystemTray(NotifyIcon notifyIcon)
        {
            NotifyIcon = notifyIcon;
            NotifyIcon.Icon = Properties.Resources.Crosshair;

            // Menu Item "Close"
            CloseMenuItem = new MenuItem();
            CloseMenuItem.Text = "Close";
            CloseMenuItem.Click += CloseMenuItem_Click;

            // Menu Item "Toggle"
            ToggleMenuItem = new MenuItem();
            ToggleMenuItem.Text = "Toggle";

            // Populate Context Menu
            var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(0, ToggleMenuItem);
            contextMenu.MenuItems.Add(1, CloseMenuItem);

            notifyIcon.ContextMenu = contextMenu;

            Application.ApplicationExit += new EventHandler(OnApplicationExit);
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            NotifyIcon.Dispose();
        }

    }
}
