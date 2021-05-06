using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Nonograma
{
    class Tablero
    {
        // Los cuatro posibles valores de una casilla
        enum Casillas { Libre, Coloreada, Tachada};
        Casillas[,] tab;
        string[] datosFilas, datosColumnas;
        int maxDatosFil, maxDatosCol;
        Coor jugador;


        public Tablero(int n) // Crea un tablero del nivel indicado 
        {
            StreamReader levels = new StreamReader("levels.txt");

            string s = levels.ReadLine();

            while (s != "level" + n) /// DUDA: es más eficaz esto o dar más vueltas pero leyendo una única línea en cada vuelta??
            {
                s = levels.ReadLine(); // Leo las dos líneas que ocupa cada nivel
                s = levels.ReadLine();
                s = levels.ReadLine(); // Y el espacio en blanco
                s = levels.ReadLine();
            }

            // Cantidad de espacios necesarios para escribir los datos sobre el tablero
            maxDatosCol = 1;
            maxDatosFil = 1;

            datosColumnas = levels.ReadLine().Split(" ");
            // Determinamos el nº de columnas (1º dato)
            int columnas = datosColumnas.Length;

            for (int i = 0; i < columnas; i++)
            { // Si es una columna con comas, se comprueba si su tamaño es mayor (hay que añadir otra fila para la visualización)
                if (datosColumnas[i].Contains(",") && datosColumnas[i].Split(',').Length > maxDatosFil)
                {
                    maxDatosFil = datosColumnas[i].Split(',').Length;
                }
            }

            datosFilas = levels.ReadLine().Split(" ");
            int filas = datosFilas.Length; // Determinamos el número de filas (2º dato)
            for (int i = 0; i < filas; i++)
            { // Si es una fila con comas, se comprueba si su tamaño es mayor (hay que añadir otra columna para la visualización)
                if (datosFilas[i].Contains(",") && datosFilas[i].Split(',').Length > maxDatosCol)
                {
                    maxDatosCol = datosFilas[i].Split(',').Length;
                }
            }

            levels.Close();

            tab = new Casillas[filas, columnas];

            // Todas las casillas están libres al principio
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    tab[i, j] = Casillas.Libre;
                }
            }

            // Se le asigna al jugador una posición inicial
            jugador = new Coor(0, 0);

        }

        public void Dibuja() 
        {
            // En primer lugar escribo los datos
                // Datos de las columnas
            Console.SetCursorPosition(maxDatosCol + 3, 0); // Dos huecos para el número, y uno entre medias

            for (int i = 0; i < datosColumnas.Length; i++)
            {
                for (int j = 0; j < datosColumnas[i].Length; j++)
                {
                    if (datosColumnas[i][j] != ',')
                    {
                        Console.Write(datosColumnas[i][j]);
                    }
                    else
                    {
                        Console.SetCursorPosition(maxDatosCol + 3 + i * 2, j + 1);
                    }
                }

                Console.SetCursorPosition(maxDatosCol + 3 + (i + 1) * 2, 0);
            }
                // Datos de las filas
            Console.SetCursorPosition(0, maxDatosCol); 

            for (int i = 0; i < datosFilas.Length; i++)
            {
                for (int j = 0; j < datosFilas[i].Length; j++)
                {
                    if (datosFilas[i][j] != ',')
                    {
                        Console.Write(datosFilas[i][j]);
                    }
                    else
                    {
                        Console.SetCursorPosition(j + 2, maxDatosFil + i);
                    }
                }

                Console.SetCursorPosition(0 , maxDatosCol + (i + 1));
            }

            // Hay que cambiar la posición del puntero: 
            Console.SetCursorPosition(maxDatosCol + 3, maxDatosFil); // Dos huecos para el número, y uno entre medias

            // Ahora dibujo el tablero en sí
            // Además, si estoy en esa fila y columna concretas, se dibujan diferente
            for (int n = 0; n < tab.GetLength(0); n++)
            {
                for (int m = 0; m < tab.GetLength(1); m++)
                {
                    if (tab[n,m] == Casillas.Coloreada)
                    {
                        if (jugador.fil == n || jugador.col == m)
                        {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                        Console.Write("  ");
                    }else if (tab[n, m] == Casillas.Tachada)
                    {
                        if (jugador.fil == n || jugador.col == m)
                        {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("XX");
                    }else if (tab[n, m] == Casillas.Libre)
                    {
                        if (jugador.fil == n || jugador.col == m)
                        {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        Console.Write("  ");
                    }
                }
                Console.WriteLine();
                // Le sumo 1 en la 2ª coordenada para que no se sobreescriba el tablero
                Console.SetCursorPosition(maxDatosCol + 3, maxDatosFil + 1 + n); 
            }

            // Reseteo de los colores
            Console.BackgroundColor = default;
            Console.ForegroundColor = ConsoleColor.White;

        }


        // Método que devuelve si la siguiente posición en la dirección dir está dentro del tablero
        private bool SiguientePosicion(Coor dir, out Coor nextPos)
        {
            nextPos = jugador + dir; // Utiliza jugador porque no se va a mover otra cosa que no sea el jugador

            // Si la nueva posición no se sale de los límites, puede moverse
            return (nextPos.fil >= 0 && nextPos.col >= 0 && nextPos.fil < tab.GetLength(0) && nextPos.col < tab.GetLength(1));
        }

        // Se encarga del movimiento del jugador por el tablero
        public void Mueve(char c)
        {
            /// Coor[] direcciones = { new Coor(0, 1), new Coor(0, -1), new Coor(1, 0), new Coor(-1, 0) };

            switch (c) // Según la dirección en la que quiere moverse, se comprueba si es posible
            {
                case 'l':
                    if (SiguientePosicion(new Coor(0, -1), out Coor newPosL)){
                        jugador = newPosL;
                    }
                    break;
                case 'r':
                    if (SiguientePosicion(new Coor(0, 1), out Coor newPosR))
                    {
                        jugador = newPosR;
                    }
                    break;
                case 'u':
                    if (SiguientePosicion(new Coor(-1, 0), out Coor newPosU))
                    {
                        jugador = newPosU;
                    }
                    break;
                case 'd':
                    if (SiguientePosicion(new Coor(1, 0), out Coor newPosD))
                    {
                        jugador = newPosD;
                    }
                    break;
            }

        }


    }
}
