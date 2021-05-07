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
                if (datosColumnas[i].Contains(",") && datosColumnas[i].Split(',').Length > maxDatosCol)
                {
                    maxDatosCol = datosColumnas[i].Split(',').Length;
                }
            }

            datosFilas = levels.ReadLine().Split(" ");
            int filas = datosFilas.Length; // Determinamos el número de filas (2º dato)
            for (int i = 0; i < filas; i++)
            { // Si es una fila con comas, se comprueba si su tamaño es mayor (hay que añadir otra columna para la visualización)

                if (datosFilas[i].Contains(",") && datosFilas[i].Split(',').Length > maxDatosFil)
                {
                    maxDatosFil = datosFilas[i].Split(',').Length;
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
            int cont = 0; // variable que ayudará con el dibujo 

                // En primer lugar escribo los datos
                // Datos de las columnas
            Console.SetCursorPosition(maxDatosFil*2 + 2, 0); // Lo que ocupan los datos de la izquierda, más el centrado
            for (int i = 0; i < datosColumnas.Length; i++)
            {
                for (int j = 0; j < datosColumnas[i].Length; j++)
                {
                    if (datosColumnas[i][j] != ',')
                    {
                        cont++;
                        Console.Write(datosColumnas[i][j]);
                    }
                    else
                    {
                        Console.SetCursorPosition(maxDatosFil*2 + 4*(i) + 2, cont);
                    }
                }
                cont = 0;
                Console.SetCursorPosition(maxDatosFil*2 + 4*(i + 1) + 2, 0); 
            }/// ESTO ESTÁ BIEN PARA EL ANCHO 4, SI ES ANCHO DOS, SE CAMBIA EL 4 POR UN 2 Y SE QUITA EL + 2


                // Datos de las filas
            Console.SetCursorPosition(0, maxDatosCol);
            for (int i = 0; i < datosFilas.Length; i++)
            {
                for (int j = 0; j < datosFilas[i].Length; j++)
                {
                    if (datosFilas[i][j] != ',')
                    {
                        Console.Write(datosFilas[i][j] + " ");
                    }
                }

                Console.SetCursorPosition(0 , maxDatosCol + (i+1)*2); /// SI SE HACE DE TAMAÑO MENOR, QUITAR *2
            }

            // Hay que cambiar la posición del puntero: 
            Console.SetCursorPosition(maxDatosFil*2, maxDatosCol); // Dos huecos para el número, y uno entre medias

            // Ahora dibujo el tablero en sí
            cont = 1;
            // Además, si estoy en esa fila y columna concretas, se dibujan diferente
            for (int n = 0; n < tab.GetLength(0); n++)
            {
                for (int j = 1; j < 3; j++)
                {
                    for (int m = 0; m < tab.GetLength(1); m++)
                    {
                        if (tab[n, m] == Casillas.Coloreada)
                        {
                            if (jugador.fil == n || jugador.col == m)
                            {
                                Console.BackgroundColor = ConsoleColor.Cyan;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write("    ");
                        }
                        else if (tab[n, m] == Casillas.Tachada)
                        {
                            if (jugador.fil == n || jugador.col == m)
                            {
                                Console.BackgroundColor = ConsoleColor.Cyan;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            
                            if (j == 1) // En la "primera vuelta"
                            {
                                Console.Write("  / ");
                            }
                            else
                            {
                                Console.Write(" /  ");
                            }
                        }
                        else if (tab[n, m] == Casillas.Libre)
                        {
                            if (jugador.fil == n || jugador.col == m)
                            {
                                Console.BackgroundColor = ConsoleColor.Cyan;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                            Console.Write("    ");
                        }
                    }
                    Console.SetCursorPosition(maxDatosFil*2, maxDatosCol + cont);
                    cont++;
                }
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
