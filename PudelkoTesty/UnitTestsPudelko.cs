using Microsoft.VisualStudio.TestTools.UnitTesting;
using PudelkoLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace PudelkoUnitTests
{

    [TestClass]
    public static class InitializeCulture
    {
        [AssemblyInitialize]
        public static void SetEnglishCultureOnAllUnitTest(TestContext context)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
    }

    // ========================================

    [TestClass]
    public class UnitTestsPudelkoConstructors
    {
        private static double defaultSize = 0.1; // w metrach
        private static double accuracy = 0.002; //dokładność 3 miejsca po przecinku

        private void AssertPudelko(Pudelko p, double expectedA, double expectedB, double expectedC)
        {
            Assert.AreEqual(expectedA, p.GetInMeters(p.A), delta: accuracy);
            Assert.AreEqual(expectedB, p.GetInMeters(p.B), delta: accuracy);
            Assert.AreEqual(expectedC, p.GetInMeters(p.C), delta: accuracy);
        }

        #region Constructor tests ================================

        [TestMethod, TestCategory("Constructors")]
        public void Constructor_Default()
        {
            Pudelko p = new Pudelko();

            Assert.AreEqual(defaultSize, p.GetInMeters(p.A), delta: accuracy);
            Assert.AreEqual(defaultSize, p.GetInMeters(p.B), delta: accuracy);
            Assert.AreEqual(defaultSize, p.GetInMeters(p.C), delta: accuracy);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.543, 3.1,
                 1.0, 2.543, 3.1)]
        [DataRow(1.0001, 2.54387, 3.1005,
                 1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
        public void Constructor_3params_DefaultMeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(a, b, c);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.543, 3.1,
                 1.0, 2.543, 3.1)]
        [DataRow(1.0001, 2.54387, 3.1005,
                 1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
        public void Constructor_3params_InMeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(100.0, 25.5, 3.1,
                 1.0, 0.255, 0.031)]
        [DataRow(100.0, 25.58, 3.13,
                 1.0, 0.255, 0.031)] // dla centymertów liczy się tylko 1 miejsce po przecinku
        public void Constructor_3params_InCentimeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(a: a, b: b, c: c, unit: UnitOfMeasure.centimeter);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(100, 255, 3,
                 0.1, 0.255, 0.003)]
        [DataRow(100.0, 25.58, 3.13,
                 0.1, 0.025, 0.003)] // dla milimetrów nie liczą się miejsca po przecinku
        public void Constructor_3params_InMilimeters(double a, double b, double c,
                                                     double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a, b: b, c: c);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }


        // ----

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.5, 1.0, 2.5)]
        [DataRow(1.001, 2.599, 1.001, 2.599)]
        [DataRow(1.0019, 2.5999, 1.001, 2.599)]
        public void Constructor_2params_DefaultMeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(a, b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.5, 1.0, 2.5)]
        [DataRow(1.001, 2.599, 1.001, 2.599)]
        [DataRow(1.0019, 2.5999, 1.001, 2.599)]
        public void Constructor_2params_InMeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(a: a, b: b, unit: UnitOfMeasure.meter);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11.0, 2.5, 0.11, 0.025)]
        [DataRow(100.1, 2.599, 1.001, 0.025)]
        [DataRow(2.0019, 0.25999, 0.02, 0.002)]
        public void Constructor_2params_InCentimeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.centimeter, a: a, b: b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11, 2.0, 0.011, 0.002)]
        [DataRow(100.1, 2599, 0.1, 2.599)]
        [DataRow(200.19, 2.5999, 0.2, 0.002)]
        public void Constructor_2params_InMilimeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a, b: b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        // -------

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(2.5)]
        public void Constructor_1param_DefaultMeters(double a)
        {
            Pudelko p = new Pudelko(a);

            Assert.AreEqual(a, p.GetInMeters(p.A));
            Assert.AreEqual(0.1, p.GetInMeters(p.B));
            Assert.AreEqual(0.1, p.GetInMeters(p.C));
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(2.5)]
        public void Constructor_1param_InMeters(double a)
        {
            Pudelko p = new Pudelko(a);

            Assert.AreEqual(a, p.GetInMeters(p.A));
            Assert.AreEqual(0.1, p.GetInMeters(p.B));
            Assert.AreEqual(0.1, p.GetInMeters(p.C));
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11.0, 0.11)]
        [DataRow(100.1, 1.001)]
        [DataRow(2.0019, 0.02)]
        public void Constructor_1param_InCentimeters(double a, double expectedA)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.centimeter, a: a);

            AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11, 0.011)]
        [DataRow(100.1, 0.1)]
        [DataRow(200.19, 0.2)]
        public void Constructor_1param_InMilimeters(double a, double expectedA)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a);

            AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
        }

        // ---

        public static IEnumerable<object[]> DataSet1Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5, 3.1},
            new object[] {1.0, -2.5, 3.1},
            new object[] {1.0, 2.5, -3.1},
            new object[] {-1.0, -2.5, 3.1},
            new object[] {-1.0, 2.5, -3.1},
            new object[] {1.0, -2.5, -3.1},
            new object[] {-1.0, -2.5, -3.1},
            new object[] {0, 2.5, 3.1},
            new object[] {1.0, 0, 3.1},
            new object[] {1.0, 2.5, 0},
            new object[] {1.0, 0, 0},
            new object[] {0, 2.5, 0},
            new object[] {0, 0, 3.1},
            new object[] {0, 0, 0},
            new object[] {10.1, 2.5, 3.1},
            new object[] {10, 10.1, 3.1},
            new object[] {10, 10, 10.1},
            new object[] {10.1, 10.1, 3.1},
            new object[] {10.1, 10, 10.1},
            new object[] {10, 10.1, 10.1},
            new object[] {10.1, 10.1, 10.1}
        };

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_DefaultMeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InMeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1, 1)]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(-1, -1, 1)]
        [DataRow(-1, 1, -1)]
        [DataRow(1, -1, -1)]
        [DataRow(-1, -1, -1)]
        [DataRow(0, 1, 1)]
        [DataRow(1, 0, 1)]
        [DataRow(1, 1, 0)]
        [DataRow(0, 0, 1)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(0, 0, 0)]
        [DataRow(0.01, 0.1, 1)]
        [DataRow(0.1, 0.01, 1)]
        [DataRow(0.1, 0.1, 0.01)]
        [DataRow(1001, 1, 1)]
        [DataRow(1, 1001, 1)]
        [DataRow(1, 1, 1001)]
        [DataRow(1001, 1, 1001)]
        [DataRow(1, 1001, 1001)]
        [DataRow(1001, 1001, 1)]
        [DataRow(1001, 1001, 1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InCentimeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.centimeter);
        }


        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1, 1)]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(-1, -1, 1)]
        [DataRow(-1, 1, -1)]
        [DataRow(1, -1, -1)]
        [DataRow(-1, -1, -1)]
        [DataRow(0, 1, 1)]
        [DataRow(1, 0, 1)]
        [DataRow(1, 1, 0)]
        [DataRow(0, 0, 1)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(0, 0, 0)]
        [DataRow(0.1, 1, 1)]
        [DataRow(1, 0.1, 1)]
        [DataRow(1, 1, 0.1)]
        [DataRow(10001, 1, 1)]
        [DataRow(1, 10001, 1)]
        [DataRow(1, 1, 10001)]
        [DataRow(10001, 10001, 1)]
        [DataRow(10001, 1, 10001)]
        [DataRow(1, 10001, 10001)]
        [DataRow(10001, 10001, 10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InMiliimeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.milimeter);
        }


        public static IEnumerable<object[]> DataSet2Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5},
            new object[] {1.0, -2.5},
            new object[] {-1.0, -2.5},
            new object[] {0, 2.5},
            new object[] {1.0, 0},
            new object[] {0, 0},
            new object[] {10.1, 10},
            new object[] {10, 10.1},
            new object[] {10.1, 10.1}
        };

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_DefaultMeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InMeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 0)]
        [DataRow(0, 0)]
        [DataRow(0.01, 1)]
        [DataRow(1, 0.01)]
        [DataRow(0.01, 0.01)]
        [DataRow(1001, 1)]
        [DataRow(1, 1001)]
        [DataRow(1001, 1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InCentimeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.centimeter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 0)]
        [DataRow(0, 0)]
        [DataRow(0.1, 1)]
        [DataRow(1, 0.1)]
        [DataRow(0.1, 0.1)]
        [DataRow(10001, 1)]
        [DataRow(1, 10001)]
        [DataRow(10001, 10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InMilimeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.milimeter);
        }




        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(10.1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_DefaultMeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(10.1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InMeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(0.01)]
        [DataRow(1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InCentimeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a, unit: UnitOfMeasure.centimeter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(0.1)]
        [DataRow(10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InMilimeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a, unit: UnitOfMeasure.milimeter);
        }

        #endregion


        #region ToString tests ===================================

        [TestMethod, TestCategory("String representation")]
        public void ToString_Default_Culture_EN()
        {
            var p = new Pudelko(2.5, 9.321);
            string expectedStringEN = "2.500 m × 9.321 m × 0.100 m";

            Assert.AreEqual(expectedStringEN, p.ToString());
        }

        [DataTestMethod, TestCategory("String representation")]
        [DataRow(null, 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
        [DataRow("m", 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
        [DataRow("cm", 2.5, 9.321, 0.1, "250.0 cm × 932.1 cm × 10.0 cm")]
        [DataRow("mm", 2.5, 9.321, 0.1, "2500 mm × 9321 mm × 100 mm")]
        public void ToString_Formattable_Culture_EN(string format, double a, double b, double c, string expectedStringRepresentation)
        {
            var p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);
            Assert.AreEqual(expectedStringRepresentation, p.ToString(format));
        }

        [TestMethod, TestCategory("String representation")]
        [ExpectedException(typeof(FormatException))]
        public void ToString_Formattable_WrongFormat_FormatException()
        {
            var p = new Pudelko(1);
            var stringformatedrepreentation = p.ToString("wrong code");
        }

        #endregion


        #region Pole, Objętość ===================================

        [DataTestMethod, TestCategory("Pole")]
        [DataRow(2, 5, 4, 76)]
        [DataRow(4, 6, 3, 108)] 
        public void Pole_Default(double a, double b, double c, double expectedPole)
        {
            Pudelko p = new Pudelko(a, b, c);

            Assert.AreEqual(expectedPole, Convert.ToDouble(p.Pole.Split(" ")[0]));
        }

        [DataTestMethod, TestCategory("Pole")]
        [DataRow(2, 5, 4, 76)]
        [DataRow(4, 6, 3, 108)] 
        public void Pole_Meters(double a, double b, double c, double expectedPole)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);

            Assert.AreEqual(expectedPole, Convert.ToDouble(p.Pole.Split(" ")[0]));
        }

        [DataTestMethod, TestCategory("Pole")]
        [DataRow(200, 500, 400, 76)]
        [DataRow(400, 600, 300, 108)]
        public void Pole_Centimeters(double a, double b, double c, double expectedPole)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.centimeter);

            Assert.AreEqual(expectedPole, Convert.ToDouble(p.Pole.Split(" ")[0]));
        }

        [DataTestMethod, TestCategory("Pole")]
        [DataRow(2000, 5000, 4000, 76)]
        [DataRow(4000, 6000, 3000, 108)] 
        public void Pole_Milimeters(double a, double b, double c, double expectedPole)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.milimeter);

            Assert.AreEqual(expectedPole, Convert.ToDouble(p.Pole.Split(" ")[0]));
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(2, 5, 4, 40)]
        [DataRow(4, 6, 3, 72)] 
        public void Objetosc_Default(double a, double b, double c, double expectedObjetosc)
        {
            Pudelko p = new Pudelko(a, b, c);

            Assert.AreEqual(expectedObjetosc, Convert.ToDouble(p.Objetosc.Split(" ")[0]));
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(2, 5, 4, 40)]
        [DataRow(4, 6, 3, 72)] 
        public void Objetosc_Meters(double a, double b, double c, double expectedObjetosc)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);

            Assert.AreEqual(expectedObjetosc, Convert.ToDouble(p.Objetosc.Split(" ")[0]));
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(200, 500, 400, 40)]
        [DataRow(400, 600, 300, 72)] 
        public void Objetosc_Centimeters(double a, double b, double c, double expectedObjetosc)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.centimeter);

            Assert.AreEqual(expectedObjetosc, Convert.ToDouble(p.Objetosc.Split(" ")[0]));
        }

        [DataTestMethod, TestCategory("Objetosc")]
        [DataRow(2000, 5000, 4000, 40)]
        [DataRow(4000, 6000, 3000, 72)] 
        public void Objetosc_Milimeters(double a, double b, double c, double expectedObjetosc)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.milimeter);

            Assert.AreEqual(expectedObjetosc, Convert.ToDouble(p.Objetosc.Split(" ")[0]));
        }

        #endregion

        #region Equals ===========================================

        [DataTestMethod, TestCategory("Equals")]
        [DataRow(2, 5, 4, 4, 5, 2)]
        [DataRow(4, 6, 3, 3, 4, 6)]
        [DataRow(1, 2, 3, 2, 1, 3)]
        [DataRow(4, 4, 2, 2, 4, 4)]
        [DataRow(2, 2, 2, 2, 2, 2)]
        [DataRow(9, 6, 2, 2, 9, 6)]
        public void Pudelka_Equals(double a1, double b1, double c1, double a2, double b2, double c2)
        {
            Pudelko p1 = new Pudelko(a: a1, b: b1, c: c1);
            Pudelko p2 = new Pudelko(a: a2, b: b2, c: c2);

            Assert.AreEqual(p1, p2);
        }

        #endregion

        #region Operators overloading ===========================

        [DataTestMethod, TestCategory("Operator Equals")]
        [DataRow(2, 5, 4, 4, 5, 2)]
        [DataRow(4, 6, 3, 3, 4, 6)]
        [DataRow(1, 2, 3, 2, 1, 3)]
        [DataRow(4, 4, 2, 2, 4, 4)]
        [DataRow(2, 2, 2, 2, 2, 2)]
        [DataRow(9, 6, 2, 2, 9, 6)]
        public void Pudelka_Equals_Operator(double a1, double b1, double c1, double a2, double b2, double c2)
        {
            Pudelko p1 = new Pudelko(a: a1, b: b1, c: c1);
            Pudelko p2 = new Pudelko(a: a2, b: b2, c: c2);

            Assert.AreEqual(true , p1 == p2);
        }

        [DataTestMethod, TestCategory("Operator not Equals")]
        [DataRow(2, 5, 4, 4, 5, 2)]
        [DataRow(4, 6, 3, 3, 4, 6)]
        [DataRow(1, 2, 3, 2, 1, 3)]
        [DataRow(4, 4, 2, 2, 4, 4)]
        [DataRow(2, 2, 2, 2, 2, 2)]
        [DataRow(9, 6, 2, 2, 9, 6)]
        public void Pudelka_Not_Equals_Operator(double a1, double b1, double c1, double a2, double b2, double c2)
        {
            Pudelko p1 = new Pudelko(a: a1, b: b1, c: c1);
            Pudelko p2 = new Pudelko(a: a2, b: b2, c: c2);

            Assert.AreEqual(false, p1 != p2);
        }

        [DataTestMethod, TestCategory("Operator Addition")]
        [DataRow(2, 5, 4, 4, 5, 2)]
        [DataRow(4, 6, 3, 3, 4, 6)]
        public void Pudelka_Addition_Operator(double a1, double b1, double c1, double a2, double b2, double c2)
        {
            Pudelko p1 = new Pudelko(a: a1, b: b1, c: c1);
            Pudelko p2 = new Pudelko(a: a2, b: b2, c: c2);

            Assert.AreEqual(Pudelko.Dodaj(p1, p2), p1 + p2);
        }

        #endregion

        #region Conversions =====================================
        [TestMethod]
        public void ExplicitConversion_ToDoubleArray_AsMeters()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            double[] tab = (double[])p;
            Assert.AreEqual(3, tab.Length);
            Assert.AreEqual(p.GetInMeters(p.A), tab[0]);
            Assert.AreEqual(p.GetInMeters(p.B), tab[1]);
            Assert.AreEqual(p.GetInMeters(p.C), tab[2]);
        }

        [TestMethod]
        public void ImplicitConversion_FromAalueTuple_As_Pudelko_InMilimeters()
        {
            var (a, b, c) = (2500, 9321, 100); // in milimeters, ValueTuple
            Pudelko p = (a, b, c);
            Assert.AreEqual((int)(p.GetInMeters(p.A) * 1000), a);
            Assert.AreEqual((int)(p.GetInMeters(p.B) * 1000), b);
            Assert.AreEqual((int)(p.GetInMeters(p.C) * 1000), c);
        }

        #endregion

        #region Indexer, enumeration ============================
        [TestMethod]
        public void Indexer_ReadFrom()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            Assert.AreEqual(p.GetInMeters(p.A), p[0]);
            Assert.AreEqual(p.GetInMeters(p.B), p[1]);
            Assert.AreEqual(p.GetInMeters(p.C), p[2]);
        }

        [TestMethod]
        public void ForEach_Test()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            var tab = new[] { p.GetInMeters(p.A), p.GetInMeters(p.B), p.GetInMeters(p.C) };
            int i = 0;
            foreach (double x in p)
            {
                Assert.AreEqual(x, tab[i]);
                i++;
            }
        }

        #endregion

        #region Parsing =========================================

        [DataTestMethod, TestCategory("Parsing")]
        [DataRow(2, 5, 4, "2.000 m × 5.000 m × 4.000 m")]
        [DataRow(4, 6, 3, "4.000 m × 6.000 m × 3.000 m")]
        [DataRow(1, 2, 3, "1.000 m × 2.000 m × 3.000 m")]
        public void PudelkoParsing(double a1, double b1, double c1, string p2)
        {
            Pudelko p1 = new Pudelko(a: a1, b: b1, c: c1);

            Assert.AreEqual(p1, Pudelko.Parse(p2));
        }

        #endregion

    }
}