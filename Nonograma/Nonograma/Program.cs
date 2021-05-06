using System;

namespace Nonograma
{
    class Program
    {
        static bool Debug = false;
        static void Main(string[] args)
        {
            Tablero tab = new Tablero(1);

            tab.Dibuja();

            char c = ' ';

            while (true) // Bucle principal de juego
            {
                c = LeeInput();
                
                if (c != ' ')
                {
                    if (Debug)
                    {
                        Console.WriteLine(c);
                        Console.Clear();
                    }

                    tab.Mueve(c);
                    tab.Dibuja();
                    c = ' ';
                }

            }
        }


        static char LeeInput()
        {
            string tecla = Console.ReadKey().Key.ToString();
            char dir = ' ';

            // se asigna un caracter por cada posible entrada
            switch (tecla)
            {
                case "LeftArrow": // Moverse hacia la izquierda
                    dir = 'l';
                    break;
                case "RightArrow": // Moverse hacia la derecha
                    dir = 'r';
                    break;
                case "UpArrow": // Moverse hacia arriba
                    dir = 'u';
                    break;
                case "DownArrow": // Moverse hacia abajo
                    dir = 'd';
                    break;
                case "Enter": // Colorear una casilla
                    dir = 'o';
                    break;
                case "X": // Colocar una x en una casilla
                    dir = 'x';
                    break;
                case "Backspace": // Borrar lo que hay en la casilla
                    dir = 'b';
                    break;
                case "G": // Guardar
                    dir = 'g';
                    break;
                case "H": // Cargar
                    dir = 'h';
                    break;

            }
            // if (tecla != "LeftArrow" && tecla != "RightArrow" && tecla != "UpArrow" && tecla != "DownArrow" && tecla != "Enter" && tecla != "X" && tecla != "Backspace" && tecla != "G" && tecla != "H")

            return dir;
        }

    }
}
