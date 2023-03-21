using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Info
{
    class Pixel //Classe qui va contenir les octets pour les couleurs RGB( RED, GREEN, BLUE)
    {
        //Attributs
        int red;
        int green;
        int blue;

    
        // Propriétés
        public int Red
        {
            get
            {
                return red;
            }

            set
            {
                red = value;
            }
        }
        public int Green
        {
            get
            {
                return green;
            }

            set
            {
                green = value;
            }
        }
        public int Blue
        {
            get
            {
                return blue;
            }

            set
            {
                blue = value;
            }
        }
        
        
        // Constructeurs
        public Pixel(int R, int G, int B)
        {
            if (R >= 0 && R < 256 && B >= 0 && B < 256 && G >= 0 && G < 256) //on vérifie que les pixels sont bien dans l'interval [0,255]
            {
                red = R;
                green = G;
                blue = B;  
            }
        }

        public Pixel(Pixel pixel)
        {
            red = pixel.Red;
            green = pixel.Green;
            blue = pixel.Blue;
        }


        // Méthodes On les place ici car elles agissent directement sur les valeurs des pixels
        public void nuanceGris()
        {
            int gris = (this.red + this.blue + this.green) / 3;
            this.red = gris;
            this.blue = gris;
            this.green = gris;
        }

        public void NoirEtBlanc()
        {
            int gris = (this.red + this.blue + this.green) / 3;
            if (gris <= 128)
            {
                this.red = 0;
                this.blue = 0;
                this.green = 0;
            }
            else
            {
                this.red = 255;
                this.blue = 255;
                this.green = 255;
            }
        }
    }
}
