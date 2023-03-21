using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Info
{
    class Complex
    {

        private double Pr; //réel
        private double Pi; //imaginaire

        public Complex(double a, double b)
        {
            this.Pr = a;
            this.Pi = b;
        }
        public double Norme()
        {
            return Math.Sqrt((Pr * Pr) + (Pi * Pi));
        }
        public void Carre()
        {
            double PartieReelle = (Pr * Pr) - (Pi * Pi); //partie réelle
            Pi = 2.0 * Pr * Pi; //partie complexe
            Pr = PartieReelle;
        }

        public void Add(Complex c)
        {
            Pr += c.Pr;
            Pi += c.Pi;
        }

    }
}
