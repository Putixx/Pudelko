using System;
using System.Collections;
using System.Globalization;

namespace PudelkoLibrary
{
    public enum UnitOfMeasure { milimeter, centimeter, meter };

    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable, IComparable<Pudelko>
    {
        #region Potrzebne metody oraz zmienne =====================================

        public double GetInMeters(string value) => Convert.ToDouble(value.Split(" ")[0]);
        private double ObliczObjetosc() => Math.Round(GetInMeters(A) * GetInMeters(B) * GetInMeters(C), 9);
        private double ObliczPole() => Math.Round(2 * GetInMeters(A) * GetInMeters(B) + 2 * GetInMeters(A) * GetInMeters(C) + 2 * GetInMeters(B) * GetInMeters(C), 6);
        private double[] wymiaryPudelka = new double[3];

        #endregion

        #region Krawędzie =====================================

        private double a;
        public string A 
        { 
            get => $"{a:F3} m";
            private set => a = Convert.ToDouble(value);
        }
        private double b;
        public string B 
        {
            get => $"{b:F3} m";
            private set => b = Convert.ToDouble(value);
        }
        private double c;
        public string C 
        {
            get => $"{c:F3} m";
            private set => c = Convert.ToDouble(value);
        }

        #endregion

        #region Objetosc =====================================

        private double objetosc;
        public string Objetosc
        {
            get => $"{objetosc:F9} m³";
            private set => objetosc = Convert.ToDouble(value);
        }

        #endregion

        #region Pole =====================================

        private double pole;
        public string Pole
        {
            get => $"{pole:F6} m²";
            private set => pole = Convert.ToDouble(value);
        }

        #endregion

        #region Konstruktor =====================================

        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            if (a < 0 || b < 0 || c < 0) throw new ArgumentOutOfRangeException("Żadne z wymiar pudełka nie może mieć ujemnych wartości!");
            if (a == 0 || b == 0 || c == 0) throw new ArgumentOutOfRangeException("Żadne z wymiar pudełka nie może być równe 0!");

            double x = 0;
            if (unit == UnitOfMeasure.meter) x = 1;
            if (unit == UnitOfMeasure.centimeter) x = 0.01;
            if (unit == UnitOfMeasure.milimeter) x = 0.001;

            a = a == null ? 0.1 : a * x;
            b = b == null ? 0.1 : b * x;
            c = c == null ? 0.1 : c * x;

            if (a < 0.001 || b < 0.001 || c < 0.001 || a > 10 || b > 10 || c > 10) throw new ArgumentOutOfRangeException("Wymiary pudełka nie mogą być mniejsze niż 1mm, ani większe niż 10m!");

            A = a.ToString();
            B = b.ToString();
            C = c.ToString();

            objetosc = ObliczObjetosc();
            pole = ObliczPole();

            wymiaryPudelka[0] = GetInMeters(A);
            wymiaryPudelka[1] = GetInMeters(B);
            wymiaryPudelka[2] = GetInMeters(C);
        }

        #endregion

        #region Equals =====================================

        public bool Equals(Pudelko other)
        {
            if (other is null) return false;
            if (Object.ReferenceEquals(this, other)) return true;

            return (A == other.A || A == other.B || A == other.C) && (B == other.A || B == other.B || B == other.C) && (C == other.A || C == other.B || C == other.C);
        }
        public override int GetHashCode() => (A, B, C).GetHashCode();
        public override bool Equals(object obj)
        {
            if (obj is Pudelko)
                return Equals((Pudelko)obj);
            else
                return false;
        }

        public static bool Equals(Pudelko p1, Pudelko p2)
        {
            if ((p1 is null) && (p2 is null)) return true; 
            if (p1 is null) return false;

            return p1.Equals(p2);
        }

        public static bool operator ==(Pudelko p1, Pudelko p2) => Equals(p1, p2);
        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);

        #endregion

        #region Operator + =====================================

        public static Pudelko Dodaj(Pudelko p1, Pudelko p2)
        {
            double newObj = p1.objetosc + p2.objetosc;
            double newA = p1.GetInMeters(p1.A) + p2.GetInMeters(p2.A);
            double newB = p1.GetInMeters(p1.B) > p2.GetInMeters(p2.B) ? p1.GetInMeters(p1.B) : p2.GetInMeters(p2.B);
            double newC = newObj / (newA*newB);

            return new Pudelko(newA, newB, newC, UnitOfMeasure.meter);
        }

        public static Pudelko operator +(Pudelko p1, Pudelko p2) => Dodaj(p1, p2);

        #endregion

        #region Explicit =====================================

        public static explicit operator double[](Pudelko p)
        {
            double[] doubleTab = new double[3];
            doubleTab[0] = p.GetInMeters(p.A);
            doubleTab[1] = p.GetInMeters(p.B);
            doubleTab[2] = p.GetInMeters(p.C);

            return doubleTab;
        }

        #endregion

        #region Implicit =====================================

        public static implicit operator Pudelko((int a, int b, int c) p)
        {
            Pudelko pudelko = new Pudelko(a: p.a, b: p.b, c: p.c, unit: UnitOfMeasure.milimeter);

            return pudelko;
        }

        #endregion

        #region Indexor =====================================

        public double this[int i]
        {
            get => wymiaryPudelka[i];
        }

        #endregion

        #region Pętla foreach =====================================

        public IEnumerator GetEnumerator()
        {
            for(int i = 0; i < wymiaryPudelka.Length; i++)
            {
                yield return wymiaryPudelka[i];
            }
        }

        #endregion

        #region Parse =====================================

        public static Pudelko Parse(string pudelko)
        {
            string[] wejscie = pudelko.Split(" ");

            if (wejscie[1] == "m") return new Pudelko(a: Convert.ToDouble(wejscie[0]), b: Convert.ToDouble(wejscie[3]), c: Convert.ToDouble(wejscie[6]), unit: UnitOfMeasure.meter);
            else if (wejscie[1] == "cm") return new Pudelko(a: Convert.ToDouble(wejscie[0]), b: Convert.ToDouble(wejscie[3]), c: Convert.ToDouble(wejscie[6]), unit: UnitOfMeasure.centimeter);
            else return new Pudelko(a: Convert.ToDouble(wejscie[0]), b: Convert.ToDouble(wejscie[3]), c: Convert.ToDouble(wejscie[6]), unit: UnitOfMeasure.milimeter);
        }

        #endregion

        #region Comparer =====================================

        public int CompareTo(Pudelko other)
        {
            if (other is null) return 1; 
            if (this.Equals(other)) return 0; 

            if (this.objetosc != other.objetosc)
                return this.objetosc.CompareTo(other.objetosc);

            
            if (!this.pole.Equals(other.pole)) 
                return this.pole.CompareTo(other.pole);

            double suma1 = this.a + this.b + this.c;
            double suma2 = other.a + other.b + other.c;

            return suma1.CompareTo(suma2);
        }

        #endregion

        #region ToString() =====================================

        public override string ToString() => ToString("m", CultureInfo.CurrentCulture); 
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);
        public string ToString(string format, IFormatProvider provider)
        {
            if (String.IsNullOrEmpty(format)) format = "m";
            if (provider == null) provider = CultureInfo.CurrentCulture;

            switch (format)
            {
                case "m":
                    return $"{A} × {B} × {C}";
                case "cm":
                    return $"{(GetInMeters(A) * 100):F1} cm × {(GetInMeters(B) * 100):F1} cm × {(GetInMeters(C) * 100):F1} cm";
                case "mm":
                    return $"{(GetInMeters(A) * 1000)} mm × {(GetInMeters(B) * 1000)} mm × {(GetInMeters(C) * 1000)} mm";
                default:
                    throw new FormatException(String.Format($"The {format} format string is not supported."));
            }
        }

        #endregion
    }
}
