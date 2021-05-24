using System;

namespace Nonograma
{
    class Program
    {
        static bool Debug = false;
        static void Main(string[] args)
        {

            int nivelActual = 0;
            int niveles = 4; // Número de niveles con los que cuenta el tablero
            Tablero[] nivelesSuperados = new Tablero[niveles];

            int modoDeJuego = -1;

            char c = ' ';

            while(modoDeJuego != 4) //Mientras no escoja salir del juego
            {
                while (modoDeJuego != 1 && modoDeJuego != 2 && modoDeJuego != 3 && modoDeJuego != 4)
                {
                    Console.Clear();
                    Console.WriteLine("1- Jugar.");
                    Console.WriteLine("2- Ver un tablero completado.");
                    Console.WriteLine("3- Crear un nonograma.");
                    Console.WriteLine("4- Salir.");
                    Console.WriteLine();
                    Console.WriteLine("Escriba el número correspondiente a la acción que desee realizar.");
                    modoDeJuego = int.Parse(Console.ReadLine());
                }

                // Volverá al menú cuando acabe el nivel o pulse q
                while (modoDeJuego != 0 && modoDeJuego != 4)
                {
                    if (modoDeJuego == 1)
                    {// Juega todos los niveles hasta que decida salir (q)
                        while (c != 'q' && nivelActual < niveles)
                        {
                            Console.Clear();
                            Tablero tab = new Tablero(nivelActual);
                            tab.Dibuja();
                            while (c != 'q' && !tab.TableroCompletado())
                            {
                                c = ' ';
                                c = LeeInput();

                                if (c != ' ')
                                {
                                    if (Debug)
                                    {
                                        Console.WriteLine(c);
                                        Console.Clear();
                                    }

                                    if (c == 'l' || c == 'r' || c == 'u' || c == 'd')
                                    {
                                        tab.Mueve(c);
                                    }
                                    else if (c == 'o')
                                    {
                                        tab.Colorea();
                                    }
                                    else if (c == 'x')
                                    {
                                        tab.Tacha();
                                    }
                                    else if (c == 'b')
                                    {
                                        tab.Borra();
                                    }

                                    tab.Dibuja();
                                }
                            } // Bucle de un nivel

                            if (tab.TableroCompletado())
                            {
                                nivelesSuperados[nivelActual] = tab;
                            }

                            if (c != 'q')
                            {
                                nivelActual++;

                                if (nivelActual == niveles)
                                {
                                    Console.WriteLine("Juego completado. Pulse ENTER para volver al menú.");

                                    Console.ReadLine();
                                }
                                else
                                {
                                    Console.WriteLine("Tablero completado. Pulse ENTER para pasar al siguiente.");

                                    Console.ReadLine();
                                }

                            }


                        }
                        modoDeJuego = 0;
                    }
                    else if (modoDeJuego == 2)
                    {
                        Console.WriteLine("Qué nivel desea ver: ");
                        int nivelDeseado = int.Parse(Console.ReadLine());

                        if (nivelDeseado >= nivelActual)
                        {
                            Console.WriteLine("Aún no puede ver el nivel " + nivelDeseado + " , no lo ha superado.");
                            Console.WriteLine("Pulse ENTER para volver al menú.");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.Clear();
                            Tablero tab = nivelesSuperados[nivelDeseado];
                            tab.DibujaSolucion();

                            Console.WriteLine("Pulse ENTER para volver al menú.");
                            Console.ReadLine();
                        }
                        modoDeJuego = 0;
                    }

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
                case "Q": // salir
                    dir = 'q';
                    break;

            }
            // if (tecla != "LeftArrow" && tecla != "RightArrow" && tecla != "UpArrow" && tecla != "DownArrow" && tecla != "Enter" && tecla != "X" && tecla != "Backspace" && tecla != "G" && tecla != "H")

            return dir;
        }

    }
}
