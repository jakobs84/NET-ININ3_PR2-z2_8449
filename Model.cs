using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NET_ININ3_PR2_z1
{
    class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        double
            liczbaA,
            liczbaB
            ;
        string buforDziałania = null;
        bool
            flagaUłamka = false,
            flagaDziałania = false
            ;
        string buforIO = "0";
        public string IO
        {
            get { return buforIO; }
            set
            {
                buforIO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IO"));
                //
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bufory"));
            }
        }


        public double LiczbaA {
            get => liczbaA;
            set { 
                liczbaA = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bufory"));
            }
        }


        public double LiczbaB {
            get => liczbaB;
            set
            {
                liczbaB = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bufory"));
            }
        }


        public string BuforDziałania {
            get => buforDziałania;
            set
            {
                buforDziałania = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bufory"));
            }
        }

        readonly Dictionary<string, (string L, string P)> działaniaJednoargumentowe = new Dictionary<string, (string L, string P)>()
        {
            ["x²"] = ("", "²"),
            ["√"] = ("√", ""),
            ["%"] = ("", ""),
            ["1/x"] = ("1/", ""),
            ["!"] = ("", "!"),
            ["LN"] = ("ln(", ")"),
            ["FLO"] = ("", ""),
            ["CEL"] = ("", "")

        };



        public string Bufory { 
            get {
                if (buforDziałania == null)
                    return "";
                if (flagaDziałania == false)
                    return $"{liczbaA} {buforDziałania}";
                if (działaniaJednoargumentowe.ContainsKey(BuforDziałania))
                    return działaniaJednoargumentowe[BuforDziałania].L + LiczbaA + działaniaJednoargumentowe[BuforDziałania].P;
                return $"{liczbaA} {buforDziałania} {liczbaB}";
            }
        }

        internal void DziałanieZwykłe(string znak)
        {
            if (flagaDziałania == true)
                ;
            else if (BuforDziałania == null)
            {
                BuforDziałania = znak;
                LiczbaA = double.Parse(buforIO);
                flagaDziałania = true;
            }
            else
            {
                BuforDziałania = znak;
                LiczbaB = double.Parse(buforIO);
                flagaDziałania = true;
                LiczbaA = WykonajDziałanie();
                IO = LiczbaA.ToString();
            }
        }

        internal void Procent()
        {
            flagaDziałania = true;
            LiczbaB = double.Parse(buforIO)/100 * liczbaA;
            PodajWynik();
        }

        internal void DziałanieJednoargumentowe(string działanie)
        {
            BuforDziałania = działanie;
            flagaDziałania = true;
            LiczbaA = double.Parse(buforIO);
            IO = WykonajDziałanie().ToString();
        }

        internal void PodajWynik()
        {
            if (flagaDziałania == false) { 
                LiczbaB = double.Parse(buforIO);
                flagaDziałania = true;
            }
            IO = WykonajDziałanie().ToString();
            liczbaA = double.Parse(IO);
        }

        private double WykonajDziałanie()
        {
            if (BuforDziałania == "+")
                return LiczbaA + LiczbaB;
            if (BuforDziałania == "%")
                return (liczbaA * liczbaB) / 100; 
            else if (BuforDziałania == "x²")
                return LiczbaA * LiczbaA;
            else if (BuforDziałania == "-")
                return LiczbaA - LiczbaB;
            else if (BuforDziałania == "*")
                return LiczbaA * LiczbaB;
            else if (BuforDziałania == "/")
                return LiczbaA / LiczbaB;
            else if (BuforDziałania == "x²")
                return liczbaA * liczbaA;
            else if (BuforDziałania == "√")
                return Math.Pow(liczbaA, 0.5);
            else if (BuforDziałania == "1/x")
                return 1.0 / liczbaA;
            else if (BuforDziałania == "MOD")
                return liczbaA % liczbaB;
            else if (BuforDziałania == "LN")
                return Math.Log(liczbaA);
            else if (BuforDziałania == "FLO")
                return Math.Floor(liczbaA);
            else if (BuforDziałania == "CEL")
                return Math.Ceiling(liczbaA);
            else if (buforDziałania == "!")
            {
               int  wynik = 1;

                for (int i = 1; i <= Math.Round(liczbaA); i++)
                {
                    wynik *= i;
                }
                return wynik;
            }
            return 0;
        }

        internal void Resetuj()
        {
            Zeruj();
            BuforDziałania = default;
            LiczbaA = default;
            LiczbaB = default;
        }
        internal void Zeruj() {
            flagaUłamka = false;
            flagaDziałania = false;
            IO = "0";
        }
        internal void Cofnij()
        {
            if (buforIO == "0")
                return;
            else if (
                buforIO == "0,"
                ||
                buforIO == "-0,"
                ||
                (buforIO[0] == '-' && buforIO.Length == 2)
                )
                Zeruj();
            else
            {
                char usuwanyZnak = buforIO[buforIO.Length-1];
                IO = buforIO.Substring(0, buforIO.Length - 1);
                if(usuwanyZnak == ',')
                    flagaUłamka = false;
            }
        }
        internal void DopiszCyfrę(string cyfra)
        {
            if (flagaDziałania)
                Zeruj();
            if (buforIO == "0")
                buforIO = "";
            IO += cyfra;
        }
        internal void ZmieńZnak()
        {
            flagaDziałania = false;
            if (buforIO == "0")
                return;
            else if (buforIO[0] == '-')
                IO = buforIO.Substring(1);
            else
                IO = '-' + IO;
        }
        internal void PostawPrzecinek()
        {
            if (flagaDziałania)
                Zeruj();
            if (flagaUłamka || buforIO[buforIO.Length - 1] == ',')
                return;
            else
            {
                IO += ',';
                flagaUłamka = true;
            }
        }
    }
}
