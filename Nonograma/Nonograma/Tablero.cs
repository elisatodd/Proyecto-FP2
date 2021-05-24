using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nonograma;

namespace Nonograma
{
    class Tablero
    {
        // Los cuatro posibles valores de una casilla
        enum Casillas { Libre, Coloreada, Tachada};
        Casillas[,] tab; // Tablero de juego
        string[] datosFilas, datosColumnas; // Contienen los datos sobre qué debe colocarse en cada casilla
        int maxDatosFil, maxDatosCol; // Indican el espacio que se debe dejar entre el borde de la consola y el tablero
        Coor jugador; // Posición del jugador

        // Sirve para que, con tableros grandes, la ejecución sea más fluida
        // Sólo llevará a cabo la comprobación cuando haya las suficientes casillas rellenas: si hay más o menos, es absurdo comprobar
        int casillasColoreadas = 0, casillasNecesarias = 0;


        // Los niveles ya superados
        Tablero[] nivelesSuperados = new Tablero[4];

        Lista[] vectorDatosColumnas;
        Lista[] vectorDatosFilas;

        public Tablero(int n) // Crea un tablero del nivel indicado 
        {
                    /// LECTURA DE TABLERO
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
            // Determinamos el nº de columnas (1er dato)
            int columnas = datosColumnas.Length;

            for (int i = 0; i < columnas; i++)
            {
                // Si es una columna con comas, se comprueba si su tamaño es mayor (hay que añadir otra fila para la visualización)
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

                    /// CREACIÓN Y RELLENO DE TABLERO
            tab = new Casillas[filas, columnas];

            // Todas las casillas están libres al principio
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    tab[i, j] = Casillas.Libre;
                }
            }

                    /// CREACIÓN Y RELLENO DEL VECTOR DE LISTAS
            // El vector tomará tantos argumentos como columnas tenga el tablero, porque comprueba los datos de las columnas
            vectorDatosColumnas = new Lista[columnas];
            // Se rellena el array de listas de datos
            for (int i = 0; i < datosColumnas.Length; i++)
            {
                for (int j = 0; j < datosColumnas[i].Length; j++)
                {
                    if (datosColumnas[i][j] != ',')
                    {
                        // Si es el primer nodo en esa posición, se debe crear uno nuevo
                        if (vectorDatosColumnas[i] == null)
                        {
                            vectorDatosColumnas[i] = new Lista();
                        }

                        vectorDatosColumnas[i].InsertaFinal(int.Parse(datosColumnas[i][j].ToString()));
                        casillasNecesarias+= int.Parse(datosColumnas[i][j].ToString());
                    }
                }
            }

            // El vector tomará tantos argumentos como filas tenga el tablero, porque comprueba los datos de las filas
            vectorDatosFilas = new Lista[filas];
            // Se rellena el array de listas de datos
            for (int i = 0; i < datosFilas.Length; i++)
            {
                for (int j = 0; j < datosFilas[i].Length; j++)
                {
                    if (datosFilas[i][j] != ',')
                    {
                        // Si es el primer nodo en esa posición, se debe crear uno nuevo
                        if (vectorDatosFilas[i] == null)
                        {
                            vectorDatosFilas[i] = new Lista();
                        }

                        vectorDatosFilas[i].InsertaFinal(int.Parse(datosFilas[i][j].ToString()));
                    }
                }
            }


            /// JUGADOR
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
                        // Si el dato tiene dos cifras
                        if ( (j+1) < datosColumnas[i].Length && datosColumnas[i][j+1] != ',')
                        {
                            Console.Write(datosColumnas[i][j + 1]);
                            j++;
                        }
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
                        Console.Write(datosFilas[i][j]);
                        // Si el dato tiene dos cifras
                        if ((j+1) < datosFilas[i].Length && datosFilas[i][j+1] != ',')
                        {
                            Console.Write(datosFilas[i][j+1]);
                            j++;
                        }
                        Console.Write(" ");
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
                                Console.BackgroundColor = ConsoleColor.DarkCyan;
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


                /// MOVIMIENTO
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


                /// CAMBIO DE CASILLAS
        // Pone el valor de la casilla actual como coloreada (en negro)
        public void Colorea()
        {
            tab[jugador.fil, jugador.col] = Casillas.Coloreada;
            casillasColoreadas++;
        }

        // Pone el valor de la casilla actual como tachada (con una cruz)
        public void Tacha()
        {
            if (tab[jugador.fil, jugador.col] == Casillas.Coloreada)
            {
                casillasColoreadas--;
            }
            tab[jugador.fil, jugador.col] = Casillas.Tachada;
        }

        // Pone el valor de la casilla actual como libre (sin nada)
        public void Borra()
        {
            if (tab[jugador.fil, jugador.col] == Casillas.Coloreada)
            {
                casillasColoreadas--;
            }
            tab[jugador.fil, jugador.col] = Casillas.Libre;
        }


                /// BUCLE DE JUEGO
        // Determina si el tablero está completo o no:
        // Está completo si se cumplen todas las condiciones
        public bool TableroCompletado()
        {
            bool completado = true;

            if (casillasColoreadas != casillasNecesarias)
            {
                completado = false;
            }
            else
            {
                // Primero comprueba las columnas
                Lista[] vectorActualColumnas = new Lista[tab.GetLength(1)];

                int contador = 0;
                // relleno el vector de la versión actual del tablero 
                for (int i = 0; i < tab.GetLength(1); i++)
                {
                    for (int j = 0; j < tab.GetLength(0); j++)
                    {
                        if (vectorActualColumnas[i] == null)
                        {
                            vectorActualColumnas[i] = new Lista();
                        }

                        if (tab[j, i] == Casillas.Coloreada)
                        {
                            try
                            { // si es la n-esima casilla de un grupo de casillas seguidas
                                vectorActualColumnas[i].InsertaNesimo(contador, vectorActualColumnas[i].nEsimo(contador) + 1);
                                vectorActualColumnas[i].BorraUltimoNodo();
                            }
                            catch (Exception k)
                            { // es la primera casilla de un grupo de casillas seguidas
                                vectorActualColumnas[i].InsertaFinal(1);
                            }
                        }
                        else
                        {
                            try
                            {
                                int k = vectorActualColumnas[i].nEsimo(contador);
                                contador++;
                            }
                            catch (Exception k)
                            {

                            }
                        }
                    }
                    contador = 0;
                }

                int m = 0;
                // recorro cada posición del array = cada columna
                while (m < vectorDatosColumnas.Length && completado)
                {
                    int n = 0;

                    while (n < vectorDatosColumnas[m].CuentaEltos() && completado)
                    {
                        try
                        {
                            int nEsimoActual = vectorActualColumnas[m].nEsimo(n);
                            if (nEsimoActual != vectorDatosColumnas[m].nEsimo(n))
                            {
                                completado = false;
                            }
                        }
                        catch (Exception k) // el correcto tiene un valor en n pero el actual no
                        {
                            completado = false;
                        }

                        n++;
                    }
                    m++;
                }
                 // Si las columnas son correctas, comprueba las filas

                if (completado)
                {
                    Lista[] vectorActualFilas = new Lista[tab.GetLength(0)];

                    // relleno el vector de la versión actual del tablero 
                    for (int i = 0; i < tab.GetLength(0); i++)
                    {
                        for (int j = 0; j < tab.GetLength(1); j++)
                        {
                            if (vectorActualFilas[i] == null)
                            {
                                vectorActualFilas[i] = new Lista();
                            }

                            if (tab[i,j] == Casillas.Coloreada)
                            {
                                try
                                { // si es la n-esima casilla de un grupo de casillas seguidas
                                    vectorActualFilas[i].InsertaNesimo(contador, vectorActualFilas[i].nEsimo(contador) + 1);
                                    vectorActualFilas[i].BorraUltimoNodo();
                                }
                                catch (Exception k)
                                { // es la primera casilla de un grupo de casillas seguidas
                                    vectorActualFilas[i].InsertaFinal(1);
                                }
                            }
                            else
                            {
                                try
                                {
                                    int k = vectorActualFilas[i].nEsimo(contador);
                                    contador++;
                                }
                                catch (Exception k)
                                {

                                }
                            }
                        }
                        contador = 0;
                    }

                    m = 0;
                    // recorro cada posición del array = cada fila
                    while (m < vectorDatosFilas.Length && completado)
                    {
                        int n = 0;

                        while (n < vectorDatosFilas[m].CuentaEltos() && completado)
                        {
                            try
                            {
                                int nEsimoActual = vectorActualFilas[m].nEsimo(n);
                                if (nEsimoActual != vectorDatosFilas[m].nEsimo(n))
                                {
                                    completado = false;
                                }
                            }
                            catch (Exception k) // el correcto tiene un valor en n pero el actual no
                            {
                                completado = false;
                            }

                            n++;
                        }
                        m++;
                    }
                }
            }
            return completado;
        }

        void GuardaTableroSuperado(int i, Tablero tab)
        {
            // Se referencia este mismo tablero

            /* StreamWriter nivelesCopia = new StreamWriter("nivelesSuperadosCopia");
            StreamReader niveles = new StreamReader("nivelesSuperados");

            string s = niveles.ReadLine();
            while(s != " " && s != null)
            {
                nivelesCopia.WriteLine(s);
                s = niveles.ReadLine();
            }

            nivelesCopia.WriteLine("level" + i);
            nivelesCopia.WriteLine(); */

        }

        // Dibuja sin tener cuenta al jugador
        public void DibujaSolucion()
        {
            int cont = 0; // variable que ayudará con el dibujo 

            // En primer lugar escribo los datos
            // Datos de las columnas
            Console.SetCursorPosition(maxDatosFil * 2 + 2, 0); // Lo que ocupan los datos de la izquierda, más el centrado
            for (int i = 0; i < datosColumnas.Length; i++)
            {
                for (int j = 0; j < datosColumnas[i].Length; j++)
                {
                    if (datosColumnas[i][j] != ',')
                    {
                        cont++;
                        Console.Write(datosColumnas[i][j]);
                        // Si el dato tiene dos cifras
                        if ((j + 1) < datosColumnas[i].Length && datosColumnas[i][j + 1] != ',')
                        {
                            Console.Write(datosColumnas[i][j + 1]);
                            j++;
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(maxDatosFil * 2 + 4 * (i) + 2, cont);
                    }
                }
                cont = 0;
                Console.SetCursorPosition(maxDatosFil * 2 + 4 * (i + 1) + 2, 0);
            }/// ESTO ESTÁ BIEN PARA EL ANCHO 4, SI ES ANCHO DOS, SE CAMBIA EL 4 POR UN 2 Y SE QUITA EL + 2


            // Datos de las filas
            Console.SetCursorPosition(0, maxDatosCol);
            for (int i = 0; i < datosFilas.Length; i++)
            {
                for (int j = 0; j < datosFilas[i].Length; j++)
                {
                    if (datosFilas[i][j] != ',')
                    {
                        Console.Write(datosFilas[i][j]);
                        // Si el dato tiene dos cifras
                        if ((j + 1) < datosFilas[i].Length && datosFilas[i][j + 1] != ',')
                        {
                            Console.Write(datosFilas[i][j + 1]);
                            j++;
                        }
                        Console.Write(" ");
                    }
                }

                Console.SetCursorPosition(0, maxDatosCol + (i + 1) * 2); /// SI SE HACE DE TAMAÑO MENOR, QUITAR *2
            }

            // Hay que cambiar la posición del puntero: 
            Console.SetCursorPosition(maxDatosFil * 2, maxDatosCol); // Dos huecos para el número, y uno entre medias

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
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            
                            Console.Write("    ");
                        }
                        else if (tab[n, m] == Casillas.Tachada)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            
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
                            Console.BackgroundColor = ConsoleColor.White;
                            
                            Console.Write("    ");
                        }
                    }
                    Console.SetCursorPosition(maxDatosFil * 2, maxDatosCol + cont);
                    cont++;
                }
            }

            // Reseteo de los colores
            Console.BackgroundColor = default;
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
