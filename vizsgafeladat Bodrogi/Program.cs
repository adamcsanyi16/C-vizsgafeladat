using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vizsgafeladat_Bodrogi
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "fuvar.csv";
            List<Fuvar> fuvarok = new List<Fuvar>();

            using (var reader = new StreamReader(filePath))
            {
                string[] header = reader.ReadLine().Split(';');
                string[] expectedHeader = { "taxi_id", "indulas", "idotartam", "tavolsag", "viteldij", "borravalo", "fizetes_modja" };               

                while (!reader.EndOfStream)
                {
                    string[] row = reader.ReadLine().Split(';');
                    Fuvar fuvar = new Fuvar
                    {
                        TaxiId = int.Parse(row[0]),
                        IndulasIdopont = DateTime.ParseExact(row[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        UtazasIdotartam = int.Parse(row[2]),
                        MegtettTavolsag = double.Parse(row[3]),
                        Viteldij = double.Parse(row[4]),
                        Borravalo = double.Parse(row[5]),
                        FizetesModja = row[6]
                    };
                    fuvarok.Add(fuvar);
                }
            }

            //3.feladat
            Console.WriteLine("3. feladat: "+ fuvarok.Count);

            //4.feladat
            int id = 6185;
            double osszeg = 0;
            foreach (Fuvar fuvar in fuvarok)
            {
                if (fuvar.TaxiId == id)
                {
                    double bevetel = fuvar.Viteldij + fuvar.Borravalo;
                    osszeg += bevetel;
                }
            }
            Console.WriteLine($"4. feladat: 4 fuvar alatt {osszeg}$");

            //5.feladat
            int bankkartya = 0;
            int keszpenz = 0;
            int vitatott = 0;
            int ingyenes = 0;
            int ismeretlen = 0;
            foreach (Fuvar fuvar in fuvarok)
            {
                if (fuvar.FizetesModja == "bankkártya")
                {
                    bankkartya++;
                }
                else if (fuvar.FizetesModja == "készpénz")
                {
                    keszpenz++;
                }
                else if (fuvar.FizetesModja == "vitatott")
                {
                    vitatott++;
                }
                else if (fuvar.FizetesModja == "ingyenes")
                {
                    ingyenes++;
                }
                else
                {
                    ismeretlen++;
                }

            }
            Console.WriteLine($"5. feladat: \n bankkártya: {bankkartya} \n készpénz: {keszpenz} \n vitatott: {vitatott} \n ingyenes: {ingyenes} \n ismeretlen: {ismeretlen}");

            //6.feladat
            double osszTav = 0;
            foreach (Fuvar fuvar in fuvarok)
            {
                osszTav += fuvar.MegtettTavolsag;
            }

            Console.WriteLine($"6. feladat: {Math.Round(osszTav* 1.6, 2)}");

            //7.feladat
            int max = int.MinValue;
            int maxId = 0;
            double maxTav = 0;
            double maxViteldij = 0;
            foreach (Fuvar fuvar in fuvarok)
            {
                if (fuvar.UtazasIdotartam > max)
                {
                    max = fuvar.UtazasIdotartam;
                    maxId = fuvar.TaxiId;
                    maxTav = fuvar.MegtettTavolsag;
                    maxViteldij = fuvar.Viteldij;
                }
            }

            Console.WriteLine($"7.feladat: Leghosszabb fuvar \n Fuvar hossza: {max} másodperc \n Taxi azonosító:{maxId}\n Megtegtett távolság: {maxTav} \n Vitelidij: {maxViteldij}$");

            //8.feladat
            List<Fuvar> hibasFuvarok = fuvarok
            .Where(fuvar => HibasFuvar(fuvar))
            .OrderBy(fuvar => fuvar.IndulasIdopont)
            .ToList();

            FuvarokToTxt("hibak.txt", hibasFuvarok);

            Console.ReadKey();
        }

        static bool HibasFuvar(Fuvar fuvar)
        {
            return fuvar.UtazasIdotartam > 0 && fuvar.Viteldij >= 0 && fuvar.MegtettTavolsag == 0;
        }

        static void FuvarokToTxt(string filePath, List<Fuvar> fuvarok)
        {
            using (var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("taxi_id;indulas;idotartam;tavolsag;viteldij;borravalo;fizetes_modja");

                foreach (Fuvar fuvar in fuvarok)
                {
                    writer.WriteLine($"{fuvar.TaxiId};{fuvar.IndulasIdopont:yyyy-MM-dd HH:mm:ss};{fuvar.UtazasIdotartam};{fuvar.MegtettTavolsag};{fuvar.Viteldij};{fuvar.Borravalo};{fuvar.FizetesModja}");
                }
            }
        }

    }

    

    class Fuvar
    {
        public int TaxiId { get; set; }
        public DateTime IndulasIdopont { get; set; }
        public int UtazasIdotartam { get; set; }
        public double MegtettTavolsag { get; set; }
        public double Viteldij { get; set; }
        public double Borravalo { get; set; }
        public string FizetesModja { get; set; }

        public override string ToString()
        {
            return $"Taxi ID: {TaxiId}, Indulás időpontja: {IndulasIdopont}, Utazás időtartama: {UtazasIdotartam}, Megtett távolság: {MegtettTavolsag}, Viteldíj: {Viteldij}, Borravaló: {Borravalo}, Fizetés módja: {FizetesModja}";
        }
    }
}
