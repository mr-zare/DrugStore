﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace project
{
    class disease
    {
        public string name;
        public List<string> daroomos = new List<string>();
        public List<string> darooman = new List<string>();
        public disease(string name)
        {
            this.name = name;
        }

        public void deletefromMOS(string name)
        {
            for (int i = 0; i < daroomos.Count; i++)
            {
                if (daroomos[i] == name)
                {
                    daroomos.RemoveAt(i);
                }
            }
        }
        public void deletefromMAN(string name)
        {
            for (int i = 0; i < darooman.Count; i++)
            {
                if (darooman[i] == name)
                {
                    darooman.RemoveAt(i);
                }
            }
        }
        public void adddrugeffect(string drugef)
        {
            string[] spil = drugef.Split(",");

            if (spil[1].Contains("+") == true)
            {
                daroomos.Add(drugef);
                string[] drugname = spil[0].Split('(');

                long hash = drugStore.polynomialRollingHash(drugname[1]) % 2000;
                if (hash < 0)
                {
                    hash *= -1;
                }
                drugStore.hash_table[hash][drugname[1]].goodfor.Add(this.name);
            }
            if (spil[1].Contains("-") == true)
            {
                darooman.Add(drugef);
                string[] drugname = spil[0].Split('(');

                long hash = drugStore.polynomialRollingHash(drugname[1]) % 2000;
                if (hash < 0)
                {
                    hash *= -1;
                }
                drugStore.hash_table[hash][drugname[1]].badfor.Add(this.name);
            }
        }
    }
    class drug
    {
        public string name;
        public int price;
        public Dictionary<string, string> drugsEffects = new Dictionary<string, string>();
        public List<string> goodfor = new List<string>();
        public List<string> badfor = new List<string>();
        public drug(string name, int price)
        {
            this.name = name;
            this.price = price;
        }
        public void addEffect(string drugName, string Effect)
        {
            drugsEffects.Add(drugName, Effect);
        }
        // public void addAlergy(string allergy, char Effect)
        // {
        //     alergies.Add(allergy, Effect);
        // }
        public void deletefromGOOD(string name)
        {
            for (int i = 0; i < goodfor.Count; i++)
            {
                if (goodfor[i] == name)
                {
                    goodfor.RemoveAt(i);
                }
            }
        }
        public void deletefromBAD(string name)
        {
            for (int i = 0; i < badfor.Count; i++)
            {
                if (badfor[i] == name)
                {
                    badfor.RemoveAt(i);
                }
            }
        }
    }
    class drugStore
    {
        public static double percent = 1;
        public static List<string> random_drug = new List<string>();
        public static List<string> random_disease = new List<string>();
        public static List<string> random_effect = new List<string>();
        public static long polynomialRollingHash(String str)
        {
            // P and M
            int p = 31;
            int m = (int)(1e9 + 9);
            long power_of_p = 1;
            long hash_val = 0;

            // Loop to calculate the hash value
            // by iterating over the elements of String
            for (int i = 0; i < str.Length; i++)
            {
                hash_val = (hash_val + (str[i] -
                            'a' + 1) * power_of_p) % m;
                power_of_p = (power_of_p * p) % m;
            }
            return hash_val;
        }
        public static Dictionary<string, disease>[] hash_table2 = new Dictionary<string, disease>[500];

        public static Dictionary<string, drug>[] hash_table = new Dictionary<string, drug>[2000];
        public drugStore()
        {
            for (int i = 0; i < 2000; i++)
            {
                hash_table[i] = new Dictionary<string, drug>();
            }

            for (int i = 0; i < 500; i++)
            {
                hash_table2[i] = new Dictionary<string, disease>();
            }
        }
        public void init()
        {
            int random_dc = 0;
            int random_dsc = 0;
            int random_ec = 0;
            for (int i = 0; i < 2000; i++)
            {
                hash_table[i] = new Dictionary<string, drug>();
            }
            string[] lines = System.IO.File.ReadAllLines("drugs.txt");
            foreach (string line in lines)
            {
                string[] splited = line.Split(':');
                long hash = polynomialRollingHash(splited[0].Remove(splited[0].Length - 1, 1)) % 2000;
                if (random_dc < 15)
                {
                    random_drug.Add(splited[0].Remove(splited[0].Length - 1, 1));
                    random_dc++;
                }

                if (hash < 0)
                {
                    hash *= -1;
                }
                hash_table[hash].Add(splited[0].Remove(splited[0].Length - 1, 1), new drug(splited[0].Remove(splited[0].Length - 1, 1), int.Parse(splited[1])));
            }

            for (int i = 0; i < 500; i++)
            {
                hash_table2[i] = new Dictionary<string, disease>();
            }
            string[] lines1 = System.IO.File.ReadAllLines("alergies.txt");
            foreach (string line in lines1)
            {
                if (line != " " && line != "\n")
                {
                    string[] splited = line.Split(':');
                    long hash = polynomialRollingHash(splited[0].Remove(splited[0].Length - 1, 1)) % 500;
                    disease a = new disease(splited[0].Remove(splited[0].Length - 1, 1));
                    if (random_dsc < 15)
                    {
                        random_disease.Add(splited[0].Remove(splited[0].Length - 1, 1));
                        random_dsc++;
                    }
                    if (hash < 0)
                    {
                        hash *= -1;
                    }
                    string[] splited2 = splited[1].Split(";");
                    foreach (string s in splited2)
                    {
                        if (s != " " && s != "\n")
                        {
                            a.adddrugeffect(s);
                        }


                    }
                    hash_table2[hash].Add(splited[0].Remove(splited[0].Length - 1, 1), a);
                }

            }

            ////////////////////////////////////////// effect
            string[] lines2 = System.IO.File.ReadAllLines("effects.txt");
            foreach (string line in lines2)
            {
                string[] splited = line.Split(':');
                long hash = polynomialRollingHash(splited[0].Remove(splited[0].Length - 1, 1)) % 2000;

                if (hash < 0)
                {
                    hash *= -1;
                }
                string[] splited2 = splited[1].Split(";");
                for (int i = 0; i < splited2.Length; i++)
                {
                    if (splited2[i] != " ")
                    {
                        string[] newstr = splited2[i].Split('(');
                        string[] newstr1 = newstr[1].Split(')');
                        string[] final = newstr1[0].Split(',');
                        hash_table[hash][splited[0].Remove(splited[0].Length - 1, 1)].addEffect(final[0], final[1]);
                        if (random_ec < 15)
                        {
                            random_effect.Add(final[1]);
                            random_ec++;
                        }
                    }

                }
            }

        }
        public void adddisease(string diseasename)
        {
            long hash = polynomialRollingHash(diseasename) % 500;
            if (hash < 0)
            {
                hash *= -1;
            }

            if (hash_table2[hash].ContainsKey(diseasename) == true)
            {
                throw new Exception("Bimari mojoode!!nemitooni dobare ezafash koni!!");
                Program.log.Add("bimari:" + diseasename + " mojoode!!nemitooni dobare ezafash koni!!".ToString());
            }
            else
            {
                hash_table2[hash].Add(diseasename, new disease(diseasename));

                //add random drugs
                Random rnd = new Random();
                int r_d = rnd.Next(1, 15);
                hash_table2[hash][diseasename].adddrugeffect(" (" + random_drug[r_d] + ",+) ");
                int r_d2 = rnd.Next(1, 15);
                while (r_d2 == r_d)
                {
                    r_d2 = rnd.Next(1, 15);
                }
                hash_table2[hash][diseasename].adddrugeffect(" (" + random_drug[r_d2] + ",-) ");
                Console.WriteLine("######## Daroohaye tasadofi#######");
                Program.log.Add("baraye bimari" + diseasename + "Darooye tasadofie zir baraye in bimari khoob ast: " + random_drug[r_d].ToString());
                Console.WriteLine("Darooye tasadofie zir baraye in bimari khoob ast:");
                Console.WriteLine("     " + random_drug[r_d]);
                Console.WriteLine("Darooye tasadofie zir baraye in bimari bad ast:");
                Program.log.Add("baraye bimari" + diseasename + "Darooye tasadofie zir baraye in bimari bad ast: " + random_drug[r_d2].ToString());
                Console.WriteLine("     " + random_drug[r_d2]);
            }
        }
        public void addNewDrug(string name, int price)
        {

            //add the new drug to our objects


            long hash = polynomialRollingHash(name) % 2000;
            if (hash < 0)
            {
                hash *= -1;
            }

            if (hash_table[hash].ContainsKey(name) == true)
            {
                Console.WriteLine("In daroo mojoode!! nemitooni dobare ezafash koni!!");
                Program.log.Add("daaroo:" + name + "mojoode");
            }
            else
            {
                hash_table[hash].Add(name, new drug(name, price));

                //some random drugs for this drug effect
                Random rnd = new Random();

                int d = rnd.Next(1, 15); // returns random integers >= 0 and < 16
                int e = rnd.Next(1, 15);
                long hash_d = polynomialRollingHash(random_drug[d]) % 2000;
                if (hash_d < 0)
                {
                    hash_d *= -1;
                }
                hash_table[hash_d][random_drug[d]].addEffect(name, random_effect[e]);
                hash_table[hash][name].addEffect(random_drug[d], random_effect[e]);

                int d2 = rnd.Next(1, 15);
                while (d2 == d)
                {
                    d2 = rnd.Next(1, 15);
                }
                int e2 = rnd.Next(1, 15);
                long hash_d2 = polynomialRollingHash(random_drug[d2]) % 2000;
                if (hash_d2 < 0)
                {
                    hash_d2 *= -1;
                }
                hash_table[hash_d2][random_drug[d2]].addEffect(name, random_effect[e2]);
                hash_table[hash][name].addEffect(random_drug[d2], random_effect[e2]);
                //write this random to user
                Console.WriteLine("######## daroohaye tasadofi#######");
                Console.WriteLine("2 Darooye random ba effect hayeshan be in daroo ezafe shod:");
                Console.WriteLine("     " + random_drug[d] + " , " + random_effect[e]);
                Console.WriteLine("     " + random_drug[d2] + " , " + random_effect[e2]);
                Program.log.Add("baraye daroo" + name + "2 Darooye random ba effect hayeshan be in daroo ezafe shod:  "
                    + random_drug[d] + " , " + random_effect[e] + "va  " + random_drug[d2] + " , " + random_effect[e2]);

                //some random alergy for this drug alergy
                int d1 = rnd.Next(1, 15);
                long hash_d1 = polynomialRollingHash(random_disease[d1]) % 500;
                if (hash_d1 < 0)
                {
                    hash_d1 *= -1;
                }
                hash_table2[hash_d1][random_disease[d1]].adddrugeffect(" (" + name + ",+) ");

                int d3 = rnd.Next(1, 15);
                long hash_d3 = polynomialRollingHash(random_disease[d3]) % 500;
                if (hash_d3 < 0)
                {
                    hash_d3 *= -1;
                }
                hash_table2[hash_d3][random_disease[d3]].adddrugeffect(" (" + name + ",-) ");

                // //write this random alergy to the user
                Console.WriteLine("######## bimarihaye tasadofi#######");
                Console.WriteLine("In daroo baraye bimarie randome zir mofid ast:");
                Console.WriteLine(random_disease[d1] + "  " + " (" + name + ",+) ");
                Console.WriteLine("In daroo baraye bimarie randome zir mozer ast:");
                Console.WriteLine(random_disease[d3] + "  " + " (" + name + ",-) ");
                Program.log.Add("baraye darooye jadide: " + name + "In daroo baraye bimarie randome zir mofid ast:" + random_disease[d1] + "  " + " (" + name + ",+) "
                    + "In daroo baraye bimarie randome zir mozer ast:" + random_disease[d3] + "  " + " (" + name + ",-) ");

            }
        }
        public void readDisease(string diseasename)
        {
            long hash = polynomialRollingHash(diseasename) % 500;
            if (hash < 0)
            {
                hash *= -1;
            }
            if (hash_table2[hash].ContainsKey(diseasename) == true)
            {
                string outpmos = "";
                string outpman = "";
                Console.WriteLine("Daroo haii ke roye in bimari asare mostbat darand:");

                foreach (string m in hash_table2[hash][diseasename].daroomos)
                {
                    Console.Write(m + " ");
                    outpmos += m + " ";
                }
                Console.WriteLine();
                Console.WriteLine("Daroo haii ke roye in bimari asare manfi darand:");
                foreach (string m in hash_table2[hash][diseasename].darooman)
                {
                    Console.Write(m + " ");
                    outpman += m + " ";
                }
                Console.WriteLine();
                Program.log.Add("Daroo haii ke roye" + diseasename + "asare mostbat darand:" + outpmos);
                Program.log.Add("Daroo haii ke roye" + diseasename + "asare manfi darand:" + outpman);
            }
            else
            {

                Console.WriteLine("Bimari dar darookhane mojood nis!!");
                Program.log.Add("bimari" + diseasename + "dar darookhane mojoode");
            }

        }
        public void deleteDisease(string diseasename)
        {
            long hash = polynomialRollingHash(diseasename) % 500;
            if (hash < 0)
            {
                hash *= -1;
            }
            if (hash_table2[hash].ContainsKey(diseasename) == true)
            {
                string god = "";
                string godb = "";
                Console.WriteLine("In bimari az liste daroohaye zir be onvane darooye khoob hazf shod:");
                foreach (string dr in hash_table2[hash][diseasename].daroomos)
                {
                    string[] temp = dr.Split('(');
                    string[] temp1 = temp[1].Split(',');
                    string g = temp1[0];
                    long hash2 = polynomialRollingHash(g) % 2000;
                    if (hash2 < 0)
                    {
                        hash2 *= -1;
                    }
                    hash_table[hash2][g].deletefromGOOD(diseasename);
                    Console.Write(g + " ");
                    god += g + " ";
                }
                Program.log.Add("bimari " + diseasename + " az liste daroohaye zir be onvane darooye khoob hazf shod:" + god);
                Console.WriteLine();
                Console.WriteLine("In bimari az liste daroohaye zir be onvane darooye bad hazf shod:");
                foreach (string dr in hash_table2[hash][diseasename].darooman)
                {
                    string[] temp = dr.Split('(');
                    string[] temp1 = temp[1].Split(',');
                    string g = temp1[0];
                    long hash2 = polynomialRollingHash(g) % 2000;
                    if (hash2 < 0)
                    {
                        hash2 *= -1;
                    }
                    hash_table[hash2][g].deletefromBAD(diseasename);
                    Console.Write(g + " ");
                    godb += g + " ";
                }
                Program.log.Add("bimari " + diseasename + " az liste daroohaye zir be onvane darooye khoob hazf shod:" + godb);
                Console.WriteLine();
                hash_table2[hash].Remove(diseasename);
            }
            else
            {

                Console.WriteLine("Bimari dar darookhane mojood nis!!");
                Program.log.Add("bimari  " + diseasename + " dar darookhane mojood nist");
            }

        }
        public void readDrug(string name)
        {
            long hash = polynomialRollingHash(name) % 2000;
            if (hash < 0)
            {
                hash *= -1;
            }
            if (hash_table[hash].ContainsKey(name) == true)
            {
                Console.WriteLine(hash_table[hash][name].name + " : " + (hash_table[hash][name].price) * drugStore.percent);
                Console.WriteLine("In daroo khoob ast baraye:");
                string gd = "";
                string gdb = "";
                string asar = "";
                foreach (string item in hash_table[hash][name].goodfor)
                {
                    Console.Write(item + " ");
                    gd += item + " ";
                }
                Program.log.Add("darooye " + name + "khoobe baraye:" + gd);
                Console.WriteLine();
                Console.WriteLine("In daroo bad ast baraye:");
                foreach (string item in hash_table[hash][name].badfor)
                {
                    Console.Write(item + " ");
                    gdb += item + " ";
                }
                Program.log.Add("darooye " + name + "bade baraye:" + gdb);
                Console.WriteLine();
                Console.WriteLine("Asaraat in daroo hast:");
                foreach (KeyValuePair<string, string> item in hash_table[hash][name].drugsEffects)
                {
                    Console.WriteLine("[ " + item.Key + " , " + item.Value + " ]");
                    asar += "[ " + item.Key.ToString() + " , " + item.Value.ToString() + " ]" + " ";
                }
                Program.log.Add("asaraat daroo" + name + "hast: " + asar);
            }
            else
            {

                Console.WriteLine("Daroo dar darookhane mojood nis!!");
                Program.log.Add("daroo  " + name + "dar darookhane mojood nist");
            }

        }
        public void percentprice(double per)
        {
            percent *= (1 + per);
            Program.log.Add("percent now: " + percent);
        }
        public void deleteDrug(string drugname)
        {
            long hash = polynomialRollingHash(drugname) % 2000;
            if (hash < 0)
            {
                hash *= -1;
            }
            if (hash_table[hash].ContainsKey(drugname) == true)
            {
                string gd = "";
                string gdb = "";
                string asar = "";
                Console.WriteLine("In daroo az liste daroo haye mosbat baraye bimari haye zir hazf shod:");
                foreach (string g in hash_table[hash][drugname].goodfor)
                {
                    long h2 = polynomialRollingHash(g) % 500;
                    if (h2 < 0)
                    {
                        h2 *= -1;
                    }
                    hash_table2[h2][g].deletefromMOS(drugname);
                    Console.Write(g + " ");
                    gd += g + " ";
                }
                Program.log.Add("baraye darooye hazfi" + drugname + "in daroo az list mosbat baraye bimari haye rooberoo hazfe" + gd);
                Console.WriteLine();
                Console.WriteLine("In daroo az liste daroo haye manfi baraye bimari haye zir hazf shod:");
                foreach (string g in hash_table[hash][drugname].badfor)
                {
                    long h3 = polynomialRollingHash(g) % 500;
                    if (h3 < 0)
                    {
                        h3 *= -1;
                    }
                    hash_table2[h3][g].deletefromMAN(drugname);
                    Console.Write(g + " ");
                    gdb += g + " ";
                }
                Program.log.Add("baraye darooye hazfi" + drugname + "in daroo az list manfi baraye bimari haye rooberoo hazfe" + gdb);

                Console.WriteLine();
                Console.WriteLine("In daroo az liste effect haye daroohaye zir hazf shod:");
                foreach (KeyValuePair<string, string> e in hash_table[hash][drugname].drugsEffects)
                {
                    long hash2 = polynomialRollingHash(e.Key) % 2000;
                    if (hash2 < 0)
                    {
                        hash2 *= -1;
                    }
                    hash_table[hash2][e.Key].drugsEffects.Remove(drugname);
                    Console.WriteLine(e.Key + " ");
                    asar += e.Key.ToString() + " ";
                }
                Console.WriteLine();
                hash_table[hash].Remove(drugname);
                Program.log.Add("baraye darooye hazfi" + drugname + "In daroo az liste effect haye daroohaye roberoo hazf shod:" + asar);
            }
            else
            {
                Console.WriteLine("Daroo dar darookhane nist!!");
                Program.log.Add("daroo  " + drugname + "dar darookhane mojood nist");
            }
        }
        public static void calcpresc(List<string> drugs)
        {
            try
            {
                double sum = 0;
                foreach (string drug in drugs)
                {
                    long hash = polynomialRollingHash(drug) % 2000;
                    if (hash < 0)
                    {
                        hash *= -1;
                    }
                    sum += (hash_table[hash][drug].price * drugStore.percent);
                }
                Console.WriteLine(sum);
                Program.log.Add("majmooe hazine noskhe barabar ast ba:  " + sum + " toman");
            }
            catch (System.Exception)
            {

                Console.WriteLine("bazi az daroo haye vared shode dar darookhane mojood nabood!!");
                Program.log.Add("bazi az daroo haye vared shode dar darookhane mojood nabood!!");

            }
        }
        public static bool tadakhol(List<string> drugs)
        {
            try
            {
                for (int i = 1; i < drugs.Count; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        long hash = polynomialRollingHash(drugs[i]) % 2000;
                        if (hash < 0)
                        {
                            hash *= -1;
                        }
                        long hash1 = polynomialRollingHash(drugs[j]) % 2000;
                        if (hash1 < 0)
                        {
                            hash1 *= -1;
                        }
                        if (hash_table[hash1][drugs[j]].drugsEffects.ContainsKey(drugs[i]) == true)
                        {
                            return false;
                        }

                    }

                }
                return true;
            }
            catch (System.Exception)
            {

                Console.WriteLine("bazi az daroo ha dar daroo khane mojood nabood!!");
                Program.log.Add("dar barresi tadakhol bazi az daroo ha dar daroo khane mojood nabood!!");
            }
            return true;

        }
        public static void hasasiatbimari(List<string> drugs, List<string> disease)
        {
            bool c = drugStore.tadakhol(drugs);
            if (c == false)
            {
                Console.WriteLine("beine daroo ha tadakhol vojood darad!");
                Program.log.Add("beine daroo ha tadakhol vojood darad!");
            }
            else
            {
                for (int i = 0; i < drugs.Count; i++)
                {
                    foreach (string bimari in disease)
                    {
                        long hash = polynomialRollingHash(drugs[i]) % 2000;
                        if (hash < 0)
                        {
                            hash *= -1;
                        }
                        for (int k = 0; k < hash_table[hash][drugs[i]].badfor.Count; k++)
                        {
                            if (hash_table[hash][drugs[i]].badfor[k] == bimari)
                            {
                                Console.WriteLine("bazi az daroo haye noskhe baraye bazi az bimari haye fard bad ast!!");
                                Program.log.Add("darooye  " + hash_table[hash][drugs[i]] + "baraye bimari " + bimari + "fard bad mibashad.");

                                return;
                            }
                        }

                    }
                }
                Console.WriteLine("hich tadakholi yaft nashod");
                Program.log.Add("hich tadakholi bein darooha va bimarihaye varede yaft nashod");
            }
        }
    }
    class Program
    {
        public static List<string> log = new List<string>();
        public static drugStore d = new drugStore();
        static void UserActions(int flag)
        {

            if (flag == 1)
            {
                var timer = new Stopwatch();
                timer.Start();
                d.init();
                timer.Stop();

                TimeSpan timeTaken = timer.Elapsed;
                string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                Console.WriteLine(foo);
            }
            else if (flag == 2)
            {
                Console.WriteLine("nam e daroo ha ra dar yek khat va ba fasele ba ham vared konid:");
                string drugname = Console.ReadLine();
                string[] t = drugname.Split(" ");
                List<string> drugnames = new List<string>();
                foreach (string d in t)
                {
                    drugnames.Add(d);
                }
                var timer = new Stopwatch();
                timer.Start();
                bool c = drugStore.tadakhol(drugnames);
                if (c == true)
                {
                    Console.WriteLine("tadakhol yaft nashod");
                }
                else
                {
                    Console.WriteLine("tadakhol yaft shod");
                }
                timer.Stop();

                TimeSpan timeTaken = timer.Elapsed;
                string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                Console.WriteLine(foo);
                drugname = "darooha noskhe: " + drugname;
                log.Add(drugname);

            }
            else if (flag == 3)
            {
                Console.WriteLine("nam e daroo ha roo dar yek hat va ba fasele ba ham vared konid:");
                string drugname = Console.ReadLine();
                string[] t = drugname.Split(" ");
                List<string> drugnames = new List<string>();
                foreach (string d in t)
                {
                    drugnames.Add(d);
                }
                Console.WriteLine("nam e bimari ha roo dar yek hat va ba fasele ba ham vared konid:");
                string diseasename = Console.ReadLine();
                string[] t1 = diseasename.Split(" ");
                List<string> diseasenames = new List<string>();
                foreach (string d in t1)
                {
                    diseasenames.Add(d);
                }
                var timer = new Stopwatch();
                timer.Start();
                drugStore.hasasiatbimari(drugnames, diseasenames);
                timer.Stop();

                TimeSpan timeTaken = timer.Elapsed;
                string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                Console.WriteLine(foo);
                drugname = "darooha noskhe: " + drugname;
                log.Add(drugname);
                diseasename = "bimariha: " + diseasename;
                log.Add(diseasename);

            }
            else if (flag == 4)
            {
                Console.WriteLine("nam e daroo ha ye noskhe ro ke qeimatesh ro mikhay roo dar yek hat va ba fasele ba ham vared konid:");
                string drugname = Console.ReadLine();
                string[] t = drugname.Split(" ");
                List<string> drugnames = new List<string>();
                foreach (string d in t)
                {
                    drugnames.Add(d);
                }
                var timer = new Stopwatch();
                timer.Start();
                drugStore.calcpresc(drugnames);
                timer.Stop();

                TimeSpan timeTaken = timer.Elapsed;
                string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                Console.WriteLine(foo);
                drugname = "darooha noskhe baraye qeimat: " + drugname;
                log.Add(drugname);

            }
            else if (flag == 5)
            {
                Console.WriteLine("darsad taqiraat qeimat ro elam konid");
                try
                {
                    double percent = double.Parse(Console.ReadLine());

                    if (percent < -1.0)
                    {
                        throw new Exception("!voroodi ashari beine -1 ta binahat bayad be tabe dade!!!");
                    }
                    var timer = new Stopwatch();
                    timer.Start();
                    d.percentprice(percent);
                    timer.Stop();

                    TimeSpan timeTaken = timer.Elapsed;
                    string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                    Console.WriteLine(foo);

                    string m = "darsad taqir: " + percent.ToString();
                    log.Add(m);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            else if (flag == 6)
            {
                //taghirat rooye sakhtar dade
                try
                {
                    Console.WriteLine("1.daroo");
                    Console.WriteLine("2.bimari");
                    int a = int.Parse(Console.ReadLine());

                    if (a == 1)
                    {
                        try
                        {
                            Console.WriteLine("1.hazf");
                            Console.WriteLine("2.ezafe");
                            int m = int.Parse(Console.ReadLine());
                            if (m == 1)
                            {
                                Console.WriteLine("nam e daroo baraye hazf:");
                                string name = Console.ReadLine();
                                var timer = new Stopwatch();
                                timer.Start();
                                d.deleteDrug(name);
                                timer.Stop();

                                TimeSpan timeTaken = timer.Elapsed;
                                string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                                Console.WriteLine(foo);
                                name = "nam e daroo baraye hazf: " + name;
                                log.Add(name);
                            }
                            else if (m == 2)
                            {
                                try
                                {
                                    Console.WriteLine("nam e daroo va qeimat ra baraye ezafe shodan ba fasele vared konid:");
                                    string name = Console.ReadLine();
                                    string[] names = name.Split(' ');
                                    int prices = int.Parse(names[1]);
                                    var timer = new Stopwatch();
                                    timer.Start();
                                    d.addNewDrug(names[0], prices);
                                    timer.Stop();

                                    TimeSpan timeTaken = timer.Elapsed;
                                    string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                                    Console.WriteLine(foo);
                                    name = "nam e daroo baraye ezafe va qeimat:" + names[0] + prices.ToString();
                                    log.Add(name);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("voroodi ra dorost vared konid!!!");
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            Console.WriteLine("voroodi ra dorost vared konid!!!");
                        }
                    }
                    else if (a == 2)
                    {
                        try
                        {
                            Console.WriteLine("1.hazf");
                            Console.WriteLine("2.ezafe");
                            int m = int.Parse(Console.ReadLine());

                            if (m == 1)
                            {
                                Console.WriteLine("nam e bimari baraye hazf:");
                                string name = Console.ReadLine();
                                var timer = new Stopwatch();
                                timer.Start();
                                d.deleteDisease(name);
                                timer.Stop();

                                TimeSpan timeTaken = timer.Elapsed;
                                string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                                Console.WriteLine(foo);
                                name = "nam e daroo baraye hazf:" + name;
                                log.Add(name);
                            }
                            else if (m == 2)
                            {
                                Console.WriteLine("nam e bimari baraye ezafe:");
                                string name = Console.ReadLine();
                                var timer = new Stopwatch();
                                timer.Start();
                                d.adddisease(name);
                                timer.Stop();

                                TimeSpan timeTaken = timer.Elapsed;
                                string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                                Console.WriteLine(foo);
                                name = "nam e daroo baraye ezafe:" + name;
                                log.Add(name);
                            }
                        }
                        catch (System.Exception)
                        {
                            Console.WriteLine("voroodi ra dorost vared konid!!!");
                        }
                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("voroodi ra dorost vared konid");
                }
            }
            else if (flag == 7)
            {
                //search dar sakhtar dade
                int option = -1;


                while (option != 3)
                {
                    try
                    {
                        Console.WriteLine("Gozine mored nazar khod ra vared konid:".PadLeft(27, ' '));
                        Console.WriteLine("1.Jost va jooye daroo");
                        Console.WriteLine("2.Jost va jooye bimari");
                        Console.WriteLine("3.Baazgasht be menu");
                        option = int.Parse(Console.ReadLine());
                        Console.Clear();
                    }
                    catch (System.Exception)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine("Voroodi na moatabare!! voroodi adad nist!! dobare vared namaiid!");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                    if (option == 1)
                    {
                        Console.WriteLine("nam e daroo ra vared konid:");
                        string drug_name = Console.ReadLine();
                        var timer = new Stopwatch();
                        timer.Start();
                        d.readDrug(drug_name);
                        timer.Stop();

                        TimeSpan timeTaken = timer.Elapsed;
                        string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                        Console.WriteLine(foo);
                        drug_name = "nam e daroo jost va jooyi: " + drug_name;
                        log.Add(drug_name);
                    }
                    else if (option == 2)
                    {
                        Console.WriteLine("nam e bimari ra vared konid:");
                        string disease_name = Console.ReadLine();
                        var timer = new Stopwatch();
                        timer.Start();
                        d.readDisease(disease_name);
                        timer.Stop();

                        TimeSpan timeTaken = timer.Elapsed;
                        string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                        Console.WriteLine(foo);
                        disease_name = "nam e bimari jost va jooyi: " + disease_name;
                        log.Add(disease_name);
                    }
                    else if (option == 3)
                    {
                        //be menu mire dige!!!
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Voroodi dar mahdoode moatabar nist!!dobare vared namaiid!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

            }
            else
            {
                throw new Exception("Voroodi namoatabare!! voroodi dar mahdoode dorost nist!!");
            }
        }
        public static void save_data()
        {
            StreamWriter wd = new StreamWriter("Dataset/drugs.txt");
            StreamWriter wb = new StreamWriter("Dataset/diseases.txt");
            StreamWriter wa = new StreamWriter("Dataset/alergies.txt");
            StreamWriter we = new StreamWriter("Dataset/effects.txt");

            for (int i = 0; i < 2000; i++)
            {
                foreach (KeyValuePair<string, drug> line in drugStore.hash_table[i])
                {
                    wd.WriteLine(line.Key.ToString() + " : " + line.Value.price * drugStore.percent);
                    if (line.Value.drugsEffects.Count != 0)
                    {
                        we.Write(line.Key + " : ");
                        foreach (KeyValuePair<string, string> line1 in line.Value.drugsEffects)
                        {
                            we.Write("(" + line1.Key + "," + line1.Value + ") ; ");
                        }
                        we.WriteLine();
                    }


                }

            }
            for (int i = 0; i < 500; i++)
            {
                foreach (KeyValuePair<string, disease> line in drugStore.hash_table2[i])
                {
                    string add = "";
                    wb.WriteLine(line.Key.ToString());
                    add += (line.Key.ToString() + " :");
                    foreach (string s in line.Value.daroomos)
                    {
                        string[] sp = s.Split(",");
                        if (sp[1].Contains(" ") == false)
                        {
                            add += (s + " ;");
                        }
                        else { add += (s + ";"); }
                    }
                    foreach (string s in line.Value.darooman)
                    {
                        string[] sp = s.Split(",");
                        if (sp[1].Contains(" ") == false)
                        {
                            add += (s + " ;");
                        }
                        else { add += (s + ";"); }
                    }

                    for (int k = 0; k < add.Length - 2; k++)
                    {
                        wa.Write(add[k]);
                    }
                    wa.WriteLine();
                }
            }
            wa.Close();
            wb.Close();
            wd.Close();
            we.Close();
        }
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Blue;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("                                                            salam mojadad!");
            Console.ForegroundColor = ConsoleColor.White;
            int flag = 0;
            while (flag != 8)
            {
                Console.WriteLine();
                Console.WriteLine("Gozine mored nazar khod ra vared konid:".PadLeft(27, ' '));
                Console.WriteLine("1.Khandane file haye data".PadLeft(30, ' '));
                Console.WriteLine("2.Vojood ya adam vojood tadakholat darooii dar yek darooye tajvizi".PadLeft(71, ' '));
                Console.WriteLine("3.vojood ya adam vojood hasaasiat darooii ra dar noskhe ba bimari erjaii arzyabi konid".PadLeft(91, ' '));
                Console.WriteLine("4.mohasebe qeimat daroohaye tajvizi".PadLeft(40, ' '));
                Console.WriteLine("5.taqiire qeimat darooha".PadLeft(29, ' '));
                Console.WriteLine("6.afzoodan ya hazf az saakhtar daade".PadLeft(41, ' '));
                Console.WriteLine("7.jost va joo".PadLeft(18, ' '));
                Console.WriteLine("8.khorooj".PadLeft(14, ' '));
                Console.WriteLine("9.khorooj va namayesh taqirat".PadLeft(34, ' '));

                try
                {

                    flag = int.Parse(Console.ReadLine());
                    Console.Clear();
                }
                catch (System.Exception)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine("voroodi mojood nist!! voroodi adad nist!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
                if (flag == 9)
                {
                    foreach (string item in log)
                    {
                        Console.WriteLine(item);
                    }
                    var timer = new Stopwatch();
                    timer.Start();
                    save_data();
                    timer.Stop();

                    TimeSpan timeTaken = timer.Elapsed;
                    string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                    Console.WriteLine(foo);
                    break;
                }
                if (flag == 8)
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    save_data();
                    timer.Stop();

                    TimeSpan timeTaken = timer.Elapsed;
                    string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.ffffff");
                    Console.WriteLine(foo);
                    break;
                }
                try
                {
                    UserActions(flag);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                try
                {
                    if (flag > 9 || flag < 1)
                    {
                        throw new Exception("voroodi dar mahdoode adad haye qabl entekhab nist");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}


