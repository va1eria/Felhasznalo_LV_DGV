using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Felhasznalo_LV_DGV
{
    internal class Felhasznalo
    {
        int? uID;
        string felhasznalonev, jelszo;
        DateTime? regisztracioIdeje;

        public int? UID 
        { 
            get => uID;
            set
            {
                if (uID == null)
                {
                    uID = value;
                }
                else
                {
                    throw new InvalidOperationException("Az UID csak egyszer allithato be!");
                }
            }
        }
        public string Felhasznalonev { get => felhasznalonev; }
        public string Jelszo 
        { 
            get => jelszo;
            set
            {
                jelszo = BitConverter.ToString(new SHA256CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(value))).ToLower().Replace("-", "");
            }
        }
        public DateTime? RegisztracioIdeje 
        { 
            get => regisztracioIdeje;
            set
            {
                if (regisztracioIdeje==null)
                {
                    regisztracioIdeje = value;
                }
                else
                {
                    throw new InvalidOperationException("A regisztracio idejet az adatbazis adja meg, mely csak egyszer az adatbazis altal allithato be!");
                }
            }
        }

        // Programon belul amikor letrehozunk egy felhasznalot -> nem tudjuk azokat az adatokat, amit az adatbazis general ki (uid, regido)
        public Felhasznalo(string felhasznalonev, string jelszo)
        {
            this.felhasznalonev = felhasznalonev;
            Jelszo = jelszo;
        }

        // Adatbazisbol valo felolvasas -> itt mar mindent tudunk, igy be is tudjuk allitani a konstruktorban
        public Felhasznalo(string felhasznalonev, string jelszo, int? uID, DateTime? regisztracioIdeje)
        {
            UID = uID;
            RegisztracioIdeje = regisztracioIdeje;
            this.felhasznalonev= felhasznalonev;
            this.jelszo = jelszo;
        }

        public override string ToString()
        {
            return $"[{uID}] - {felhasznalonev}";
        }
    }
}
