using System;
using System.Collections.Generic;
using System.Text;

namespace Nonograma
{
    class Lista
    {
        private class Nodo
        {
            public int dato;
            public Nodo sig;
        }

        Nodo pri;

        public Lista()
        {
            pri = null;
        }


        // Añadir nodo al final de la lista
        public void InsertaFinal(int e)
        {
            // Lista vacía
            if (pri == null)
            {
                pri = new Nodo(); // Se crea un nodo en pri
                pri.dato = e;
                pri.sig = null;
            }
            else // Lista no vacía
            {
                Nodo aux = pri;

                while (aux.sig != null)
                {
                    aux = aux.sig;
                }

                // Creamos el nuevo nodo donde apunta aux
                aux.sig = new Nodo();
                aux = aux.sig; // Ponemos aux en el nuevo nodo
                aux.dato = e;
                aux.sig = null;

            }
        }

        public bool BuscaDato(int e)
        {
            Nodo aux = BuscaNodo(e);
            return (aux != null);
        }

        // Devuelve la referencia al nodo donde está e (si no lo hubiera, devuelve null)
        private Nodo BuscaNodo(int e)
        {
            Nodo aux = pri;

            while (aux != null && aux.dato != e)
            {
                aux = aux.sig;
            }
            return aux; // En aux estará null o el primer nodo con el elto. e
        }

        public void Ver()
        {
            Console.Write("\nLista: ");
            Nodo aux = pri;
            while (aux != null)
            {
                Console.Write(aux.dato + " ");
                aux = aux.sig;
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public override string ToString()
        {
            string s = "";
            Nodo aux = pri;
            while (aux != null)
            {
                s += aux.dato + " ";
                aux = aux.sig;
            }

            return s;
        }

        // inserta el elemento e al principio de la lista.
        public void InsertaIni(int e)
        {
            if (pri == null)
            {
                pri = new Nodo();
                pri.dato = e;
                pri.sig = null;
            }
            else
            {
                Nodo aux = pri;

                // B lleva el dato que hay en aux, c el que hay en aux.sig (el próximo que se tendrá que guardar)
                int b = aux.dato, c = aux.dato;

                while (aux != null)
                {
                    if (aux.sig == null)
                    {
                        aux.sig = new Nodo();
                        aux = aux.sig;
                        aux.dato = c;
                        aux.sig = null;
                    }
                    else
                    {
                        b = c;
                        c = aux.sig.dato;
                        aux.sig.dato = b;
                    }

                    aux = aux.sig;
                }

                pri.dato = e;

            }

        }

        // devuelve la suma de los elementos de la lista.
        public int Suma()
        {
            int suma = 0;

            Nodo aux = pri;

            while (aux != null)
            {
                suma += aux.dato;
                aux = aux.sig;
            }

            return suma;
        }

        // Devuelve el número de elementos de la lista.
        public int CuentaEltos()
        {
            int elementos = 0;

            Nodo aux = pri;

            while (aux != null)
            {
                elementos++;
                aux = aux.sig;
            }

            return elementos;
        }

        // devuelve el número de ocurrencias del elemento e en la lista
        public int CuentaOcurrencias(int e)
        {
            int nOcurrencias = 0;

            Nodo aux = pri;

            while (aux != null)
            {
                if (aux.dato == e) nOcurrencias++;
                aux = aux.sig;
            }

            return nOcurrencias;
        }

        // Devuelve el n-ésimo nodo de la lista; null si no existe tal elemento
        private Nodo nEsimoNodo(int n)
        {
            if (n < 0)
            {
                throw new Exception("EL valor se sale de los límites de la lista (número negativo).");
            }
            else
            {
                Nodo aux = pri;
                int pos = 0;
                while (aux != null && pos < n)
                {
                    pos++;
                    aux = aux.sig;
                }
                return aux;
            }
        }

        /* utilizando el método anterior, devuelve el n-ésimo elemento de la lista;
         * lanza una excepción en si no existe tal elemento. */
        public int nEsimo(int n)
        {
            Nodo aux = nEsimoNodo(n);

            if (aux == null)
            {
                throw new Exception("No existe valor para la posición. Se sale de los límites de la lista.");
            }
            else
            {
                return aux.dato;
            }
        }


        // inserta el elemento e en el lugar n-ésimo de la lista.
        public void InsertaNesimo(int n, int e)
        {
            if (n == 0)
            {
                InsertaIni(e);
            }
            else
            {
                // En aux se guarda la posicion n-1 ésima a la que se debe guardar e
                Nodo aux = nEsimoNodo(n - 1); /// podria poner una excepcion para cuando ponga un indice muy alto

                Nodo aux1 = aux.sig;
                aux.sig = new Nodo();
                aux.sig.dato = e;
                aux.sig.sig = aux1;
            }
        }

        /* : si el elemento e está en la lista borra
su primera aparición y devuelve true en ese caso; deja la lista intacta
y devuelve false si e no está en la lista. */
        public bool BorraElto(int e)
        {
            bool elto = false;

            if (BuscaDato(e)) // Si está el elemento
            {
                elto = true;

                Nodo aux = pri;

                if (pri.dato == e)
                {
                    pri = pri.sig;
                }
                else
                {
                    while (aux.sig != null && aux.sig.dato != e)
                    {
                        aux = aux.sig;
                    }

                    if (aux.sig != null)
                    {
                        aux.sig = aux.sig.sig;
                    }
                }
            }
                return (elto);
        }

        public void BorraUltimoNodo()
        {
            Nodo aux = pri;
            while (aux.sig.sig != null)
            {
                aux = aux.sig;
            }
            aux.sig = null;
        }
    }
}
