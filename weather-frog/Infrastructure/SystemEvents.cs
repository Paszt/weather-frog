using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Interop;
using weatherfrog.Interop;

namespace weatherfrog.Infrastructure
{
    sealed class SystemEvents
    {
        public event EventHandler DisplaySettingsChanged;
        public event EventHandler ResumedFromSuspension;

        private int uCallBack;

        public SystemEvents()
        {
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            Microsoft.Win32.SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            // window used to hook into wndproc
            Window win = new();
            WindowInteropHelper helper = new(win);
            HwndSource source = HwndSource.FromHwnd(helper.EnsureHandle());
            RegisterAppBar(source.Handle);
            source.AddHook(WndProc);
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Resume) ResumedFromSuspension?.Invoke(sender, e);
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e) =>
            DisplaySettingsChanged?.Invoke(sender, e);

        private void RegisterAppBar(IntPtr handle)
        {
            APPBARDATA abd = APPBARDATA.Default;
            abd.hWnd = handle;
            uCallBack = WinAPI.RegisterWindowMessage("AppBarMessage");
            abd.uCallbackMessage = uCallBack;
            _ = WinAPI.SHAppBarMessage((int)ABMsg.ABM_NEW, ref abd);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == uCallBack) DisplaySettingsChanged?.Invoke(this, new EventArgs());
            return IntPtr.Zero;
        }
    }
}
