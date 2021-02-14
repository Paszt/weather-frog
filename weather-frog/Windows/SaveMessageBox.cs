using System.Windows;

namespace weatherfrog.Windows
{
    public static class SaveMessageBox
    {
        public static MessageBoxResult Show(string fileName)
        {
            SaveMessageBoxWindow smbWin = new(fileName);
            smbWin.ShowDialog();
            return smbWin.Result;
        }
    }
}
