using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI;

public static class ModernColorPicker
{
    public static async Task<System.Drawing.Color?> PickColorAsync(System.Drawing.Color currentColor)
    {
        var picker = new Windows.UI.ViewManagement.ColorPicker();
        picker.Color = Windows.UI.Color.FromArgb(currentColor.A, currentColor.R, currentColor.G, currentColor.B);
        
        // This requires STA thread and a window handle
        var hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
        
        // Initialize for the window
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        
        var result = await picker.PickSingleColorAsync();
        if (result != null)
        {
            return System.Drawing.Color.FromArgb(result.Value.A, result.Value.R, result.Value.G, result.Value.B);
        }
        return null;
    }
}