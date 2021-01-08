using System;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Interop;
using weatherfrog.Interop;

namespace weatherfrog.Extensions
{
    public static class Windows
    {

        public static void SetPlacement(this Window window, string placementJson) =>
            SetPlacement(new WindowInteropHelper(window).Handle, placementJson);

        private static void SetPlacement(IntPtr windowHandle, string placementJson)
        {
            if (string.IsNullOrEmpty(placementJson)) { return; }
            try
            {
                JsonSerializerOptions options = new() { Converters = { new JsonStringEnumConverter() } };
                WINDOWPLACEMENT placement = JsonSerializer.Deserialize<WINDOWPLACEMENT>(placementJson, options);
                placement.Length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
                placement.Flags = 0;
                if (placement.ShowCmd == ShowWindowCommands.ShowMinimized) placement.ShowCmd = ShowWindowCommands.Normal;
                WinAPI.SetWindowPlacement(windowHandle, ref placement);
            }
            // Fail silently on serialization failure
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public static string GetPlacement(this Window window) => GetPlacement(new WindowInteropHelper(window).Handle);

        public static string GetPlacement(IntPtr windowHandle)
        {
            WINDOWPLACEMENT placement = WINDOWPLACEMENT.Default;
            WinAPI.GetWindowPlacement(windowHandle, ref placement);
            JsonSerializerOptions options = new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };
            string returnValue = JsonSerializer.Serialize<WINDOWPLACEMENT>(placement, options);
            return JsonSerializer.Serialize<WINDOWPLACEMENT>(placement, options);
        }

    }
}
