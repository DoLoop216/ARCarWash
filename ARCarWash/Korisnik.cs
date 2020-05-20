using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ARCarWash
{
    public class Korisnik
    {
        public enum KorisnikType
        {
            User = 0,
            Admin = 1,
            arDevAdministrator = 9000
        }
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public KorisnikType Tip { get; set; }

        public static bool Exist(string UserName)
        {
            if (!File.Exists("Nalozi.txt"))
                return false;

            string[] lines = File.ReadAllLines("Nalozi.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                Korisnik k = JsonConvert.DeserializeObject<Korisnik>(lines[i]);

                if (k.UserName == UserName)
                    return true;
            }
            return false;
        }

        public static int GetMaxID()
        {
            if (!File.Exists("Nalozi.txt"))
                return 0;

            int maxID = 0;
            string[] lines = File.ReadAllLines("Nalozi.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                Korisnik k = JsonConvert.DeserializeObject<Korisnik>(lines[i]);

                if (k.ID > maxID)
                    maxID = k.ID;
            }
            return maxID;
        }

        /// <summary>
        /// Verify if user credentials are valid. If they are then return Korisnik object. Otherwise return null
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static Korisnik Verify(string UserName, string Password)
        {
            if (!File.Exists("Nalozi.txt"))
                return null;

            string[] lines = File.ReadAllLines("Nalozi.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                Korisnik k = JsonConvert.DeserializeObject<Korisnik>(lines[i]);

                if (k.UserName == UserName && k.Password == Password)
                    return k;
            }
            return null;
        }

        public static Korisnik arDevAdministrator()
        {
            Korisnik k = new Korisnik();
            k.ID = 9000;
            k.Tip = KorisnikType.arDevAdministrator;
            k.UserName = "arDevAdministrator";
            return k;
        }
    }
}
