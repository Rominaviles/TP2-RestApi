using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_ORM
{
    public class HeadTitle
    {
        public static void ShowTitle(string titulo, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            
            int consoleWidth = Console.WindowWidth;
            int posicion = (consoleWidth - titulo.Length) / 2;

            Console.ForegroundColor = color;
            Console.WriteLine(titulo.PadLeft(posicion + titulo.Length));
            Console.ResetColor();
        }
    }
}
