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
        private static readonly JsonSerializerOptions placementSerializerOptions = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public static void SetPlacement(this Window window, string placementJson)
        {
            if (string.IsNullOrEmpty(placementJson)) return;
            try
            {
                WINDOWPLACEMENT placement = JsonSerializer.Deserialize<WINDOWPLACEMENT>(placementJson, placementSerializerOptions);
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
            return JsonSerializer.Serialize(placement, placementSerializerOptions);
        }
    }
}
