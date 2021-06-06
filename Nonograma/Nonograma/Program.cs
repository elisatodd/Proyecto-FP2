using System;
using System.IO;

namespace Nonograma
{
    class Program
    {
        static bool Debug = false;
        const int max = 500;
        static void Main(string[] args)
        {
            int nivelActual = 0;
            int nivelActualPersonal = 0;
            int nivelesPersonales = 0;

            int niveles = 4; // Número de niveles con los que cuenta el tablero "oficial"
            Tablero[] nivelesSuperados = new Tablero[niveles];
            Tablero[] nivelesPersonalesSuperados = new Tablero[max]; // Puede añadir como máximo (max) niveles personales


            // Si el archivo de guardado tiene información, la copia
            StreamReader ajustesGuardado = new StreamReader("ajustesguardado.txt");
            string s = ajustesGuardado.ReadLine();

            if (s != null)
            {
                nivelActual = int.Parse(s);
                nivelActualPersonal = int.Parse(ajustesGuardado.ReadLine());
                nivelesPersonales = int.Parse(ajustesGuardado.ReadLine());

                for (int i = 0; i < nivelActual; i++)
                {
                    s = ajustesGuardado.ReadLine();
                    s = ajustesGuardado.ReadLine();
                    nivelesSuperados[i] = new Tablero(i, "levels.txt");
                    nivelesSuperados[i].RellenaTablero(s);

                }
                for (int j = 0; j < nivelActualPersonal; j++)
                {
                    s = ajustesGuardado.ReadLine();
                    s = ajustesGuardado.ReadLine();
                    nivelesPersonalesSuperados[j] = new Tablero(j, "playerlevels.txt");
                    nivelesPersonalesSuperados[j].RellenaTablero(s);
                    
                }
            }
            ajustesGuardado.Close();

            
            int modoDeJuego = -1;

            char c = ' ';

            while(modoDeJuego != 6) //Mientras no escoja salir del juego
            {
                while (modoDeJuego != 1 && modoDeJuego != 2 && modoDeJuego != 3 && modoDeJuego != 4 && modoDeJuego != 5 && modoDeJuego != 6)
                {
                    Console.Clear();

                    Console.WriteLine(" . . Elisa Todd Rodríguez . . Proyecto Final FP2 . . ");
                    Console.WriteLine();
                    Console.WriteLine("     N O N O G R A M A ");
                    Console.WriteLine();
                    
                    Console.WriteLine("1- Jugar.");
                    Console.WriteLine("2- Ver un tablero completado.");
                    Console.WriteLine("3- Crear un nonograma.");
                    Console.WriteLine("4- Jugar un nonograma personalizado.");
                    Console.WriteLine("5- Ver un tablero personalizado completado.");
                    Console.WriteLine("6- Salir.");
                    Console.WriteLine();
                    Console.WriteLine("Escriba el número correspondiente a la acción que desee realizar.");

                    try{
                        modoDeJuego = int.Parse(Console.ReadLine());
                    }catch
                    {
                        Console.WriteLine("Debes introducir un número entero.");
                        Console.WriteLine("Pulsa ENTER para volver a intentar.");
                        Console.ReadLine();
                    }

                    if (modoDeJuego > 6 || modoDeJuego <= 0)
                    {
                        Console.WriteLine("El valor debe estar comprendido entre 1 y 6.");
                        Console.WriteLine("Pulsa ENTER para volver a intentar.");
                        Console.ReadLine();
                    }

                }

                c = ' ';

                // Volverá al menú cuando acabe el nivel o pulse q
                while (modoDeJuego != 0 && modoDeJuego != 6)
                {
                    if(modoDeJuego == 1)
                    {// Juega todos los niveles hasta que decida salir (q)
                        while (c != 'q' && nivelActual < niveles)
                        {
                            Console.Clear();
                            Tablero tab = new Tablero(nivelActual, "levels.txt");
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
                    else if(modoDeJuego == 2)
                    {
                        Console.Clear();
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
                    else if(modoDeJuego == 3)
                    {
                        Tablero tab = new Tablero(0, "levels.txt"); // Crea un tablero cualquiera con la constructora
                        tab.CreaNivel(); // Lo pone entero vacío con los valores que desee el usuario

                        Console.WriteLine("Pulse 'Q' cuando su tablero esté terminado.");

                        c = LeeInput();
                        while(c != 'q')
                        {
                            c = ' ';
                            c = LeeInput();

                            if (c != ' ')
                            {
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

                                tab.DibujaSinDatos();
                            }
                            Console.WriteLine();
                            Console.WriteLine("Pulse 'Q' cuando su tablero esté terminado.");
                        }

                        tab.GuardaNuevoNivel();
                        nivelesPersonales++;
                        modoDeJuego = 0;
                    }
                    else if(modoDeJuego == 4)
                    {
                        Console.Clear();

                        if (nivelesPersonales == 0)
                        {
                            Console.WriteLine("Aún no hay niveles creados. \nPulse ENTER para volver al menú y crear uno.");
                            Console.ReadLine();
                        }
                        else if (nivelActualPersonal == nivelesPersonales)
                        {
                            Console.WriteLine("Ya ha completado todos los niveles creados. \nPulse ENTER para volver al menú y añadir más.");
                            Console.ReadLine();
                        }
                        else
                        {
                            // Juega todos los niveles personales hasta que decida salir (q)
                            while (c != 'q' && nivelActualPersonal < nivelesPersonales)
                            {
                                Console.Clear();
                                Tablero tab = new Tablero(nivelActualPersonal, "playerlevels.txt");
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
                                    nivelesPersonalesSuperados[nivelActualPersonal] = tab;
                                }

                                if (c != 'q')
                                {
                                    nivelActualPersonal++;

                                    if (nivelActualPersonal == nivelesPersonales)
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
                        }
                        modoDeJuego = 0;
                    }
                    else if (modoDeJuego == 5)
                    {
                        Console.WriteLine("Qué nivel desea ver: ");
                        int nivelDeseado = int.Parse(Console.ReadLine());

                        if (nivelDeseado >= nivelActualPersonal)
                        {
                            Console.WriteLine("Aún no puede ver el nivel personal " + nivelDeseado + " , no lo ha superado.");
                            Console.WriteLine("Pulse ENTER para volver al menú.");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.Clear();
                            Tablero tab = nivelesPersonalesSuperados[nivelDeseado];
                            tab.DibujaSolucion();

                            Console.WriteLine("Pulse ENTER para volver al menú.");
                            Console.ReadLine();
                        }
                        modoDeJuego = 0;
                    }
                }

                if (modoDeJuego == 6)
                {
                    Console.WriteLine();
                    Console.WriteLine("¿Quiere guardar partida? Se guardará su progreso actual y sus tableros personalizados.");
                    Console.WriteLine("Pulse S si quiere guardar.");
                    char guardado = char.Parse(Console.ReadLine());
                    
                    guardado = char.ToUpper(guardado);

                    if (guardado == 'S') // guarda partida
                    {
                        // Guardo los valores actuales del juego para cargarlos al empezar el juego.
                        StreamWriter ajustes = new StreamWriter("ajustesguardado.txt");

                        ajustes.WriteLine(nivelActual);
                        ajustes.WriteLine(nivelActualPersonal); 
                        ajustes.WriteLine(nivelesPersonales);


                        // Guardo la informacion de los tableros completados

                        for (int i = 0; i < nivelActual; i++)
                        {
                            ajustes.WriteLine("level" + i);
                            ajustes.WriteLine(nivelesSuperados[i].ProcesaTablero());
                        }

                        for (int j = 0; j < nivelActualPersonal; j++)
                        {
                            ajustes.WriteLine("level" + j);
                            ajustes.WriteLine(nivelesPersonalesSuperados[j].ProcesaTablero());
                        }

                        ajustes.Close();
                    }
                    else // no guarda nada
                    {
                        // Se borra el contenido personalizado y los datos de la partida
                        StreamWriter playerLevels = new StreamWriter("playerlevels.txt");
                        playerLevels.Close();

                        StreamWriter playerLevelsAux = new StreamWriter("playerlevelsaux.txt");
                        playerLevelsAux.Close();

                        StreamWriter ajustes = new StreamWriter("ajustesguardado.txt");
                        ajustes.Close();

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
                case "Q": // salir
                    dir = 'q';
                    break;

            }
            // if (tecla != "LeftArrow" && tecla != "RightArrow" && tecla != "UpArrow" && tecla != "DownArrow" && tecla != "Enter" && tecla != "X" && tecla != "Backspace" && tecla != "G" && tecla != "H")

            return dir;
        }

    }
}
