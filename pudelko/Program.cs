using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using PudelkoLibrary;

namespace pudelko
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            #region Kompresuj  =====================================

            Console.WriteLine(new Pudelko(2, 4, 8).Kompresuj());

            #endregion

            #region Sortowanie =====================================

            var lista = new List<Pudelko>()
            {
                new Pudelko(8, 2, 9),
                new Pudelko(2, 4, 8),
                new Pudelko(2.2, 9.8, 8.9),
                new Pudelko(3, 6, 9),
                new Pudelko(2, 2.5, 2.8),
                new Pudelko(9, 8, 4.5),
                new Pudelko(2.3, 2.7, 3.8),
                new Pudelko(3, 7, 4),
                new Pudelko(1, 1, 1),
                new Pudelko(1, 2, 3),
                new Pudelko(1, 5, 6)
            };

            Console.Write("Typ: ");
            Console.Write(lista);
            Console.WriteLine();

            Console.WriteLine("Lista nieposortowana: ");
            foreach (var p in lista)
            {
                Console.WriteLine(p + " obj: " + p.Objetosc + " p: " + p.Pole);
            }

            Console.WriteLine("Lista posortowana: ");


            lista.Sort((p1, p2) => (Convert.ToDouble(p1.Objetosc.Split(" ")[0])
                                + Convert.ToDouble(p1.Pole.Split(" ")[0])
                                + (p1.GetInMeters(p1.A) + p1.GetInMeters(p1.B) + p1.GetInMeters(p1.C)))
                                .CompareTo
                                (Convert.ToDouble(p2.Objetosc.Split(" ")[0])
                                + Convert.ToDouble(p2.Pole.Split(" ")[0])
                                + (p2.GetInMeters(p2.A) + p2.GetInMeters(p2.B) + p2.GetInMeters(p2.C))
                            ));

            foreach (var p in lista)
            {
                Console.WriteLine(p + " obj: " + p.Objetosc + " p: " + p.Pole);
            }

            #endregion
        }
    }

    public static class PudelkoExtend
    {
        public static Pudelko Kompresuj(this Pudelko pudelko)
        {
            double obj = Convert.ToDouble(pudelko.Objetosc.Split(" ")[0]);
            double bok = Math.Pow(obj, 1d / 3);

            return new Pudelko(bok, bok, bok);
        }
    }
}
