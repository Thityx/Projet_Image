using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Projet_Info
{
    class Program
    {         
        static void Caractéristiques(MyImage image) //Affiche les propriétés de l'image de base (ici Coco.bmp).
        {               
            Console.WriteLine("Caractéristique de " + image.Nom );
            Console.WriteLine();
            Console.WriteLine("Format : " + image.Type_Image + "\nTaille : " + image.Taille_Fichier + " bits\nTaille de l'Offset : " + image.Taille_Offset + "\nLargeur : " + image.Largeur + " pixels\nHauteur : " + image.Hauteur + " pixels\n\n");
            Console.WriteLine(" Appuyez sur une touche pour continuer ");
            Console.ReadKey();
        }

        static void Traitement_Image(MyImage image)        // Choix de la modification à appliquer à l'image, Menu de choix
        {
            int numeroImage = -1;
            Console.WriteLine("Quelle opération voulez vous effectuer sur " + image.Nom + " :\n" + "\n1. Nuance de gris" + "\n2. Noir et blanc" + "\n3. Image miroir" + "\n4. Rotation" + "\n5. Agrandir" + "\n6. Rétrécir" + "\n7. Appliquer un filtre" + "\n8. Insérer une image dans une autre" + "\n9. Décoder une image dans une autre, Insérer une Image dans une autre avant" + "\n10. Fractale" + "\n11. Histogramme" + "\n\n0. Sortir\n");

            //Do while et Try catch pour être sur de rentrer un nombre valide 
            do
            {
                try { numeroImage = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.Write("Entrez un numéro valide\n"); }
            }
            while (numeroImage != 0 && numeroImage != 1 && numeroImage != 2 && numeroImage != 3 && numeroImage != 4 && numeroImage != 5 && numeroImage != 6 && numeroImage != 7 && numeroImage != 8 && numeroImage != 9 && numeroImage != 10 && numeroImage != 11 && numeroImage != 12);

            switch (numeroImage)
            {
                case 0:
                    Console.Clear(); // Si on ne veut pas aller jusqu'au bout
                    break;
                case 1:
                    Console.Clear();
                    image.Nuance_de_Gris();
                    break;
                case 2:
                    Console.Clear();
                    image.Noir_et_Blanc();
                    break;
                case 3:
                    Console.Clear();
                    image.Miroir();
                    break;
                case 4:
                    Console.Clear();
                    image.Rotation();
                    break;
                case 5:
                    Console.Clear();
                    image.Agrandir();
                    break;
                case 6:
                    Console.Clear();
                    image.Retrecir();
                    break;
                case 7:
                    Console.Clear();
                    image.Choix_Filtre();
                    break;
                case 8:
                    Console.Clear();
                    image.DissimulerImage();
                    break;
                case 9:
                    Console.Clear();
                    image.Decoder_Image();
                    break;
                case 10:
                    Console.Clear();
                    image.Fractales();
                    break;
                case 11:
                    Console.Clear();
                    image.Histogramme();
                    break;
                case 12:
                    Console.Clear();                  
                    break;
            }
        }

        static void Main(string[] args)     //on change ici l'image avec laquelle on travaille
        {
            MyImage image = new MyImage("lena.bmp");
            Console.WriteLine("On travaillera avec"+ image);
            Caractéristiques(image);
            Traitement_Image(image);

            Console.ReadKey();
        }
    }
}
