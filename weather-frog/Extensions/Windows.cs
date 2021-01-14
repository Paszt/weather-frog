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
        public static void SetPlacement(this Window window, string placementJson)
        {
            if (string.IsNullOrEmpty(placementJson)) { return; }
            try
            {
                WINDOWPLACEMENT placement = JsonSerializer.Deserialize<WINDOWPLACEMENT>(placementJson,
                    new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter() } });
                placement.Length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
                placement.Flags = 0;
                if (placement.ShowCmd == ShowWindowCommands.ShowMinimized) placement.ShowCmd = ShowWindowCommands.Normal;
                WinAPI.SetWindowPlacement(new WindowInteropHelper(window).Handle, ref placement);
            }
            // Fail silently on serialization failure
            catch (Exception) { }
        }

        public static string GetPlacement(this Window window)
        {
            WINDOWPLACEMENT placement = WINDOWPLACEMENT.Default;
            WinAPI.GetWindowPlacement(new WindowInteropHelper(window).Handle, ref placement);
            JsonSerializerOptions options = new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };
            return JsonSerializer.Serialize(placement, options);
        }
    }
}
