using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Projet_Info
{
    class MyImage
    {
       #region Attributs
        //Attributs 
        string nom;     //nom de l'image
        string type_Image;  //extension de l'image
        int taille_Fichier;  //taille du fichier
        int taille_Offset;  //taille de offset du fichier
        int largeur;  //largeur de l'image
        int hauteur;  //hauteur de l'image
        int couleur; //nombres d'octets par couleur
        int offset; // index de l'offset, utile pour construire le header
        byte[] header = new byte[54];
        byte[] myfile; // Contient le header 
        Pixel[,] image;    //matrice de pixels 
        #endregion Attributs


        #region Propriétés
        // Propriétés
        public string Nom
        {
            get { return nom; }
        }
        public string Type_Image
        {
            get { return type_Image; }
        }
        public int Taille_Fichier
        {
            get { return taille_Fichier; }
        }
        public int Taille_Offset
        {
            get { return taille_Offset; }
        }
        public int Largeur
        {
            get { return largeur; }
        }
        public int Hauteur
        {
            get { return hauteur; }
        }
        public int Couleur
        {
            get { return couleur; }
        }

        public Pixel[,] Matrice_Pixel   // Pour dissimuler une image dans une autre il faut récupérer sa matrice de pixel
        {
            get { return image; }
        }
        #endregion Propriétés


        #region Constructeur
        // Constructeur
        public MyImage(string nomfichier)
        {
            From_Image_To_File(nomfichier);
            nom = nomfichier;

            for (int i = 0; i < 54; i++)            // On récupère le header : i allant de 0 à 53
            {
                header[i] = myfile[i];
            }

            for (offset = 0; offset < 2; offset++)            //Infos typeImage : i allat de 0 à 1
            {
                type_Image = "BMP";
            }

            // tailleFichier : i = 2 à 5
            offset = 2;         //on sesert de notre index de l'offset pour le remplir facilement 
            taille_Fichier = Convertir_Endian_To_Int(4);


            /// Informations sur l'Image

            // taille offset : i = 14 à 17
            offset = 14;
            taille_Offset = Convertir_Endian_To_Int(4);

            // largeur : i = 18 et 21
            offset = 18;
            largeur = Convertir_Endian_To_Int(4);

            // hauteur : i = 22 et 25
            offset = 22;
            hauteur = Convertir_Endian_To_Int(4);

            // Couleur : i = 28 et 29
            offset = 28;
            couleur = Convertir_Endian_To_Int(2);


            // On transformenotreilage en matrice de pixels
            offset = 54;
            image = new Pixel[hauteur, largeur];

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j] = new Pixel(myfile[offset], myfile[offset + 1], myfile[offset + 2]);
                    offset += 3;
                }
            }
        }


        public MyImage()    // Vide au cas ou
        {

        }
        #endregion Constructeur


        // Méthode Constructeurs

        private void From_Image_To_File(string nomfichier)      //On met notre image dans un fichier de bytes
        {
            myfile = File.ReadAllBytes(nomfichier);
        }

        private int Convertir_Endian_To_Int(int nbOctet, int fin = 0, int valeur = 0)   //Fonction récursive 
        {
            if (fin == nbOctet)
            {
                return valeur;
            }
            else
            {
                valeur += Convert.ToInt32(myfile[offset + fin] * Math.Pow(256, fin));
                fin++;
                return Convertir_Endian_To_Int(nbOctet, fin, valeur);
            }

        }

        private byte[] Convertir_Int_To_Endian(int nombre)  // webdesigner.com 0xFF
        {
            byte[] endian = new byte[4];
            endian[0] = (byte)nombre;                   //on choisit le premier octet et on le rentre dans le tableau d'endian
            endian[1] = (byte)(((uint)nombre >> 8) & 0xFF); //on choisit le premier octet qui correspond aux chiffres superieurs à 255 (cad le 9ème octet) 
            endian[2] = (byte)(((uint)nombre >> 16) & 0xFF); //etc
            endian[3] = (byte)(((uint)nombre >> 24) & 0xFF);
            return endian;
        }



        // Méthodes Modifications Image
    
        public void Nuance_de_Gris()    // On parcourt la matrice de pixel et on applique à chaque Pixel la méthode de classe créant la nuance de gris
        {
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    image[ligne, colonne].nuanceGris();
                }
            }
            CreerImage(0); // Rien à modifier dans le header
        }

        public void Noir_et_Blanc() // On parcourt la matrice de pixel et on applique à chaque Pixel la méthode de classe créant l'image en noir et blanc
        {
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    image[ligne, colonne].NoirEtBlanc();
                }
            }
            CreerImage(0); // Rien à modifier dans le header
        }


        public void Miroir()
        {
            int choix = -1;
            
            do          //Do while et Try catch pour être sur de rentrer un nombre valide 
            {
                Console.WriteLine("Quelle opération miroir voulez vous effectuer?" + " :\n" +
                    "\n1. Grand Effet Miroir" +
                    "\n2. Miroir Vertical " +
                    "\n3. Miroir Horizontal" +
                    "\n\n0. Sortir\n");
                try { choix = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.WriteLine("Entrez un numéro valide"); }
            }
            while (choix != 0 && choix != 1 && choix != 2 && choix != 3);

            switch (choix)
            {
                case 0:
                    Environment.Exit(0); // Pour quitter
                    break;
                case 1:
                    Grand_Effet_Miroir();
                    break;
                case 2:
                    EffetMiroirVertical();
                    break;
                case 3:
                    EffetMiroirHorizontal();
                    break;
            }

        }

        public void Grand_Effet_Miroir()    //Nous montre Coco qui se regarde dans un miroir, double l'image en largeur
        {
            Pixel[,] imageMiroir = new Pixel[hauteur, 2 * largeur];

            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    imageMiroir[ligne, colonne] = image[ligne, colonne];
                    imageMiroir[ligne, imageMiroir.GetLength(1) - 1 - colonne] = image[ligne, colonne];
                }
            }
            image = imageMiroir;
            CreerImage(1);
        }

        public void EffetMiroirVertical()         //applique un effet miroir par rapport à une droite horizontale passant par le centre de l'image
        {
            Pixel[,] new_matrice_Pixels = new Pixel[hauteur, largeur];

            for (int lignes = 0; lignes < image.GetLength(0); lignes++)       //on parcours la matrice de pixel
            {
                for (int colonnes = 0; colonnes < image.GetLength(1); colonnes++)
                {
                    new_matrice_Pixels[lignes, image.GetLength(1) - 1 - colonnes] = image[lignes, colonnes];
                }
            }
            image = new_matrice_Pixels;
            CreerImage(0);
        }

        public void EffetMiroirHorizontal()             //applique un effet miroir par rapport à une droite verticale passant par le centre de l'image (miroir normal)
        {
            Pixel[,] new_matrice_Pixels = new Pixel[hauteur, largeur];

            for (int lignes = 0; lignes < image.GetLength(0); lignes++)
            {
                for (int colonnes = 0; colonnes < image.GetLength(1); colonnes++)
                {
                    new_matrice_Pixels[image.GetLength(0) - 1 - lignes, colonnes] = image[lignes, colonnes];        //on inverse ici lignes et colonnes par rapport à l'autre fonction miroir
                }
            }
            image = new_matrice_Pixels;
            CreerImage(0);
        }


        public void Rotation()  //Tourne l'image de 90°, 180° ou 270°
        {
            int angle = 0;
            do
            {
                Console.Write("Choisissez un angle entre 90, 180 ou 270 : ");
                try { angle = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.WriteLine("La rotation ne peut s'effectuer qu'à 90, 180 ou 270°"); }
            }
            while (angle != 90 && angle != 180 && angle != 270);

            if (angle == 90)
            {
                rotation90();
                CreerImage(2);
            }
            else if (angle == 180)
            {
                rotation180();
                CreerImage(0);
            }
            else if (angle == 270)
            {
                rotation270();
                CreerImage(2);
            }
        }

        public void rotation90()
        {
            Pixel[,] imageTemporaire = new Pixel[largeur, hauteur];
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    imageTemporaire[colonne, image.GetLength(0) - 1 - ligne] = image[ligne, colonne];
                }
            }
            image = imageTemporaire;
        }

        public void rotation180()
        {
            Pixel[,] imageTemporaire = new Pixel[hauteur, largeur];

            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    imageTemporaire[hauteur - 1 - ligne, largeur - 1 - colonne] = image[ligne, colonne];
                }
            }
            image = imageTemporaire;
        }

        public void rotation270()
        {
            Pixel[,] imageTemporaire = new Pixel[largeur, hauteur];
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    imageTemporaire[largeur - 1 - colonne, ligne] = image[ligne, colonne];
                }
            }
            image = imageTemporaire;
        }


        public void Agrandir()  //On fait un carré de pixel avec un seul, selon le multiple d'agrandissement
        {
            Console.WriteLine("Entrez un entier positif");

            int multiple = Convert.ToInt32(Console.ReadLine()); //multiple d'agrandissement

            Pixel[,] Grande_Image = new Pixel[hauteur * multiple, largeur * multiple];

            for (int ligne = 0; ligne < Grande_Image.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < Grande_Image.GetLength(1); colonne++)
                {
                    Grande_Image[ligne, colonne] = image[ligne / multiple, colonne / multiple];
                }
            }
            image = Grande_Image;
            CreerImage(3, multiple);
        }

        public void Retrecir()  //On prend un pixel sur diviseur    1/diviseur
        {
            Console.WriteLine("Entrez un entier positif");

            int diviseur = Convert.ToInt32(Console.ReadLine()); //multiple de rétrecissement

            Pixel[,] Petite_Image = new Pixel[hauteur / diviseur, largeur / diviseur];
            for (int ligne = 0; ligne < Petite_Image.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < Petite_Image.GetLength(1); colonne++)
                {
                    Petite_Image[ligne, colonne] = image[ligne * diviseur, colonne * diviseur];
                }
            }
            image = Petite_Image;
            CreerImage(4, diviseur); 
        }



        // Méthodes Filtres avec Matrices de Convolution

        public void Choix_Filtre()        // Définit la matrice de convolution en fonction du filtre demandé puis lance la fonction appliquant le filtre
        {
            int numeroImage = -1;   //Pour le choix du filtre à appliquer
            int[,] convolution = null;  //matrice de convolution qu'on définit après

            bool estFlou = false;   //Pour l'application du filtre flou

            Console.WriteLine("Choisissez le filtre que vous souhaitez appliquer.\n" +
                "\n1. Détection de contour" +
                "\n2. Renforcement des bords" +
                "\n3. Flou" +
                "\n4. Repoussage" +
                "\n\n0. Sortir\n");

            numeroImage = Convert.ToInt32(Console.ReadLine());
            switch (numeroImage) // On definit la matrice de convolution en fonction du filtre demandé 
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    int[,] contour = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };    //matrice contour
                    convolution = contour;
                    break;
                case 2:
                    int[,] renforcement = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };    //matrice renforcement des bords
                    convolution = renforcement;
                    break;
                case 3:
                    int[,] flou = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };    //matrice flou de l'image
                    convolution = flou;
                    estFlou = true;
                    break;
                case 4:
                    int[,] repoussage = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };    //matrice repoussage
                    convolution = repoussage;
                    break;
            }
            Convolution(convolution, estFlou);
        }

        private void Convolution(int[,] convolution, bool flou)             //Si on veut appliquer un flou alors le bool sera "true" (plus facile à coder)
        {                                                                   //On a défini la matrice de convolution dans la méthode "Choix_Filtre"
            Pixel[,] imageTemporaire = new Pixel[hauteur, largeur];

            for (int i = 1; i < hauteur - 1; i++)        // Effectue le calcul sur les pixels RGB avec la matrice de convolution sauf ceux sur les bords (les Sacrifiés) (pas de tore comme le jeu de la vie)
            {
                for (int j = 1; j < largeur - 1; j++)
                {
                    int valueRed = image[i - 1, j - 1].Red * convolution[0, 0] + image[i - 1, j].Red * convolution[0, 1] + image[i - 1, j + 1].Red * convolution[0, 2] +
                                    image[i, j - 1].Red * convolution[1, 0] + image[i, j].Red * convolution[1, 1] + image[i, j + 1].Red * convolution[1, 2] +
                                    image[i + 1, j - 1].Red * convolution[2, 0] + image[i + 1, j].Red * convolution[2, 1] + image[i + 1, j + 1].Red * convolution[2, 2];

                    int valueGreen = image[i - 1, j - 1].Green * convolution[0, 0] + image[i - 1, j].Green * convolution[0, 1] + image[i - 1, j + 1].Green * convolution[0, 2] +
                                     image[i, j - 1].Green * convolution[1, 0] + image[i, j].Green * convolution[1, 1] + image[i, j + 1].Green * convolution[1, 2] +
                                     image[i + 1, j - 1].Green * convolution[2, 0] + image[i + 1, j].Green * convolution[2, 1] + image[i + 1, j + 1].Green * convolution[2, 2];

                    int valueBlue = image[i - 1, j - 1].Blue * convolution[0, 0] + image[i - 1, j].Blue * convolution[0, 1] + image[i - 1, j + 1].Blue * convolution[0, 2] +
                                    image[i, j - 1].Blue * convolution[1, 0] + image[i, j].Blue * convolution[1, 1] + image[i, j + 1].Blue * convolution[1, 2] +
                                    image[i + 1, j - 1].Blue * convolution[2, 0] + image[i + 1, j].Blue * convolution[2, 1] + image[i + 1, j + 1].Blue * convolution[2, 2];


                    if (flou == true)       //division par 9 (nbr de coefs dans la matrice de convolution) juste pour le flou les autres étant égal à 0 ou 1
                    {
                        valueBlue = valueBlue / 9;
                        valueGreen = valueGreen / 9;
                        valueRed = valueRed / 9;
                    }

                    //Cas ou les valeurs des pixels ne seraient pas entre 0 et 255
                    if (valueRed < 0) { valueRed = 0; }
                    if (valueRed > 255) { valueRed = 255; }

                    if (valueBlue < 0) { valueBlue = 0; }
                    if (valueBlue > 255) { valueBlue = 255; }

                    if (valueGreen < 0) { valueGreen = 0; }
                    if (valueGreen > 255) { valueGreen = 255; }

                    imageTemporaire[i, j] = new Pixel(valueBlue, valueGreen, valueRed);
                }
            }

           //Les Sacrifiés (pixels sur les bords): on les remplit ici, un par un à la main avec les pixels d'à côté

            imageTemporaire[0, 0] = imageTemporaire[1, 1];                                          // coin supérieur gauche
            imageTemporaire[0, largeur - 1] = imageTemporaire[1, largeur - 2];                      // coin supérieur droit
            imageTemporaire[hauteur - 1, 0] = imageTemporaire[hauteur - 2, 1];                      // coin inférieur gauche
            imageTemporaire[hauteur - 1, largeur - 1] = imageTemporaire[hauteur - 2, largeur - 2];  // coin inférieur droit

            for (int ligne = 1; ligne < hauteur - 1; ligne++)       //pour les bords des côté
            {
                imageTemporaire[ligne, 0] = imageTemporaire[ligne, 1];                      // bord gauche
                imageTemporaire[ligne, largeur - 1] = imageTemporaire[ligne, largeur - 2];  // bord droit
            }

            for (int colonne = 1; colonne < largeur - 1; colonne++)     //pour les bord du haut et du bas
            {
                imageTemporaire[0, colonne] = imageTemporaire[1, colonne];                      // bord du haut
                imageTemporaire[hauteur - 1, colonne] = imageTemporaire[hauteur - 2, colonne];  // Bord du bas
            }

            image = imageTemporaire;
            CreerImage(0);
        }



        //Méthodes Fractales, Histogramme, Dissimuler
        public void Fractales()
        {
            Pixel[,] nouvelleImage = new Pixel[image.GetLength(0), image.GetLength(1)];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    double Pr = (double)(i - (image.GetLength(0) / 2)) / (double)(image.GetLength(1) / 4);
                    double Pi = (double)(j - (image.GetLength(1) / 2)) / (double)(image.GetLength(0) / 4);
                    Complex z = new Complex(0, 0);
                    Complex c = new Complex(Pr, Pi);
                    int index = 0;
                    do
                    {
                        z.Carre();
                        z.Add(c);
                        if (z.Norme() > 2) { break; }
                        index++;
                    } while (index < 25);

                    if (index == 25)
                    {
                        nouvelleImage[i, j] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        nouvelleImage[i, j] = new Pixel(176, 181, 204);
                    }

                }
            }
            image = nouvelleImage;
            CreerImage(0);
        }

        //Méthodes dissimulation image

        public void DissimulerImage()  // Dissimule une image dans une autre de plus grande taille
        {
            Pixel[,] imageTemporaire = new Pixel[hauteur, largeur];

            MyImage Image_a_Dissimuler  = new MyImage("Coco.bmp");

            Pixel[,] image2 = Image_a_Dissimuler.Matrice_Pixel; // Récupérer la matrice de pixels de l'image qu'on veut dissimuler

            int ajustHauteur = (hauteur - Image_a_Dissimuler.Hauteur) / 2;      //pour centrer l'image à dissimuler : on centre pour la retouver au milieu lors du décodage
            int ajustLargeur = (largeur - Image_a_Dissimuler.Largeur) / 2;      //pour centrer l'image à dissimuler

            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    if (ligne >= ajustHauteur && ligne < (hauteur - ajustHauteur) && colonne >= ajustLargeur && colonne < (largeur - ajustLargeur))
                    {
                        int[] binaire1_Blue = Convertir_Int_To_Tab_Byte(image[ligne, colonne].Blue); // On récupère la valeur associé à la couleur bleue de ce pixel
                        int[] binaire2_Blue = Convertir_Int_To_Tab_Byte(image2[ligne - ajustHauteur, colonne - ajustLargeur].Blue); // On récupère la valeur associé à la couleur bleue de l'image à dissimuler

                        int[] nouveauBinaire_Blue = new int[8];     //on crée le pixel de l'image contenant les 2 images
                        for (int i = 0; i < 8; i++)
                        {
                            if (i <= 3)
                            {
                                nouveauBinaire_Blue[i] = binaire2_Blue[4 + i]; // Les 4 premiers bits du nouvel octet correspondent aux 4 derniers de l'image à dissimuler
                            }
                            else
                            {
                                nouveauBinaire_Blue[i] = binaire1_Blue[i]; // Les 4 derniers bits du nouvel octet correspondent aux 4 premiers de l'image dans laquelle on veut dissimuler
                            }
                        }
                        int valueBlue = Convert.ToInt32(Convertir_Tab_Byte_ToInt(nouveauBinaire_Blue));

                        //comme pour le pixel bleu mais avec le vert
                        int[] binaire1_Green = Convertir_Int_To_Tab_Byte(image[ligne, colonne].Green);
                        int[] binaire2_Green = Convertir_Int_To_Tab_Byte(image2[ligne - ajustHauteur, colonne - ajustLargeur].Green);

                        int[] nouveauBinaire_Green = new int[8];
                        for (int i = 0; i < 8; i++)
                        {
                            if (i <= 3)
                            {
                                nouveauBinaire_Green[i] = binaire2_Green[4 + i];
                            }
                            else
                            {
                                nouveauBinaire_Green[i] = binaire1_Green[i];
                            }
                        }
                        int valueGreen = Convert.ToInt32(Convertir_Tab_Byte_ToInt(nouveauBinaire_Green));

                        //On répète l'opération pour le pixel rouge
                        int[] binaire1_Red = Convertir_Int_To_Tab_Byte(image[ligne, colonne].Red);
                        int[] binaire2_Red = Convertir_Int_To_Tab_Byte(image2[ligne - ajustHauteur, colonne - ajustLargeur].Red);

                        int[] nouveauBinaire_Red = new int[8];
                        for (int i = 0; i < 8; i++)
                        {
                            if (i <= 3)
                            {
                                nouveauBinaire_Red[i] = binaire2_Red[4 + i];
                            }
                            else
                            {
                                nouveauBinaire_Red[i] = binaire1_Red[i];
                            }
                        }
                        int valueRed = Convertir_Tab_Byte_ToInt(nouveauBinaire_Red);

                        imageTemporaire[ligne, colonne] = new Pixel(valueBlue, valueGreen, valueRed);
                    }

                    else // dans la zone ou il n'y a pas d'image à dissimuler
                    {
                        imageTemporaire[ligne, colonne] = image[ligne, colonne];
                    }
                }
            }
            image = imageTemporaire;
            CreerImage(0); // Rien à modifier dans le header, l'image étant de la même taille
        }

        public int[] Convertir_Int_To_Tab_Byte(int nombre)   // Convertit un entier en un octet (tableau d'entier de dimension 8)
        {
            int reste;
            int[] octet = new int[8];
            int i = 0;
            while (nombre != 0 && i < 8)
            {
                if (nombre == 0)
                {
                    octet[i] = 0;
                }
                reste = nombre % 2;
                octet[i] = reste;
                nombre = (nombre - reste) / 2;
                i++;
            }
            return octet;
        }

        public int Convertir_Tab_Byte_ToInt(int[] octet) // Convertit un octet (tableau d'entier de dimension 8) en un entier
        {
            int nombre = 0;
            for (int i = 0; i < 8; i++)
            {
                nombre += Convert.ToInt32(octet[i] * Math.Pow(2, i));
            }
            return nombre;
        }


        public void Decoder_Image()  // Decode une image dissimulé dans une autre de plus grande taille
        {
           
            MyImage Image_a_Decoder = new MyImage("nouvelle_image.bmp");

            Pixel[,] image2 = Image_a_Decoder.Matrice_Pixel; // Récupérer la matrice de pixels de l'image qu'on veut dissimuler

            Pixel[,] imageTemporaire = new Pixel[hauteur, largeur];

            for (int ligne = 0; ligne < Image_a_Decoder.Hauteur; ligne++)
            {
                for (int colonne = 0; colonne < Image_a_Decoder.Largeur; colonne++)
                {
                        int[] binaire1_Blue = Convertir_Int_To_Tab_Byte(image[ligne, colonne].Blue); // On récupère la valeur associé à la couleur bleue de ce pixel

                        int[] nouveauBinaire_Blue = new int[8];     

                        for (int i = 0; i < 8; i++)
                        {
                            if (i <= 3)
                            {
                                nouveauBinaire_Blue[i] = binaire1_Blue[i+4]; // Les 4 premiers bits du nouvel octet correspondent aux 4 derniers de l'image à dissimuler
                            }
                            else
                            {
                                nouveauBinaire_Blue[i] = 255; // Les 4 derniers bits du nouvel octet correspondent aux 4 premiers de l'image dans laquelle on veut dissimuler
                            }
                        }
                        int valueBlue = Convert.ToInt32(Convertir_Tab_Byte_ToInt(nouveauBinaire_Blue));

                    //comme pour le pixel bleu mais avec le vert
                    int[] binaire1_Green = Convertir_Int_To_Tab_Byte(image[ligne, colonne].Green); // On récupère la valeur associé à la couleur bleue de ce pixel

                    int[] nouveauBinaire_Green = new int[8];

                    for (int i = 0; i < 8; i++)
                    {
                        if (i <= 3)
                        {
                            nouveauBinaire_Green[i] = binaire1_Green[i+4];
                        }
                        else
                        {
                            nouveauBinaire_Green[i] = 0; 
                        }
                    }
                    int valueGreen = Convert.ToInt32(Convertir_Tab_Byte_ToInt(nouveauBinaire_Green));

                    //On répète l'opération pour le pixel rouge
                    int[] binaire1_Red = Convertir_Int_To_Tab_Byte(image[ligne, colonne].Red); // On récupère la valeur associé à la couleur bleue de ce pixel

                    int[] nouveauBinaire_Red = new int[8];

                    for (int i = 0; i < 8; i++)
                    {
                        if (i <= 3)
                        {
                            nouveauBinaire_Red[i] = binaire1_Red[i+4]; // Les 4 premiers bits du nouvel octet correspondent aux 4 derniers de l'image à dissimuler
                        }
                        else
                        {
                            nouveauBinaire_Red[i] = 0; 
                        }
                    }
                    int valueRed = Convert.ToInt32(Convertir_Tab_Byte_ToInt(nouveauBinaire_Red));

                    imageTemporaire[ligne, colonne] = new Pixel(valueRed, valueGreen,valueBlue );
                }
            }
            image = imageTemporaire;
            CreerImage(0); // Rien à modifier dans le header, l'image étant de la même taille
        }

        //Histogramme

        public bool EstGrise()
        {
            for (int y = 0; y < this.image.GetLength(0); y++)
            {
                for (int x = 0; x < this.image.GetLength(1); x++)
                {
                    if (image[y, x].Red != image[y, x].Green || image[y, x].Green != image[y, x].Blue || image[y, x].Red != image[y, x].Blue)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public int MaxTab(int[] tab)
        {
            int max = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] > max)
                {
                    max = tab[i];
                }
            }
            return max;
        }

        public void Histogramme()
        {
            Pixel[,] hist = new Pixel[100, 256];
            int[] Rouge = new int[256];
            int[] Vert = new int[256];
            int[] Bleu = new int[256];
            int[] Gris = new int[256];
            bool imageGrise = EstGrise();
            for (int i = 0; i < 256; i++)
            {
                Rouge[i] = 0;
                Vert[i] = 0;
                Bleu[i] = 0;
                Gris[i] = 0;
            }
            int valeur = 0;
            for (int y = 0; y < this.image.GetLength(0); y++)
            {
                for (int x = 0; x < this.image.GetLength(1); x++)
                {
                    valeur = image[y, x].Red;
                    Rouge[valeur]++;
                    valeur = image[y, x].Green;
                    Vert[valeur]++;
                    valeur = image[y, x].Blue;
                    Bleu[valeur]++;
                    Gris[valeur]++;
                }
            }
            if (imageGrise == true)
            {
                hist = CreationHistogramme(Gris, 'G');
                image = hist;
                CreerImage(5);
            }
            else
            {
                Console.WriteLine("1. Histogramme rouge");
                Console.WriteLine("2. Histogramme Vert");
                Console.WriteLine("3. Histogramme Bleu");
                Console.Write("Veuillez rentrer le numéro de l'histogramme de couleur que vous voulez choisir : ");
                int numero = Convert.ToInt32(Console.ReadLine());
                switch (numero)
                {
                    case 1:
                        hist = CreationHistogramme(Rouge, 'R');
                        image = hist;
                        CreerImage(5);
                        break;
                    case 2:
                        hist = CreationHistogramme(Vert, 'V');
                        image = hist;
                        CreerImage(5);
                        break;

                    case 3:
                        hist = CreationHistogramme(Bleu, 'B');
                        image = hist;
                        CreerImage(5);
                        break;

                }
            }

        }

        public Pixel[,] CreationHistogramme(int[] tableau, char lettre)
        {
            Pixel[,] Histogramme = new Pixel[100, 256];
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    Histogramme[i, j] = new Pixel(255, 255, 255);
                }
            }

            int max = MaxTab(tableau);
            for (int compteur = 0; compteur < tableau.Length; compteur++)
            {
                tableau[compteur] = (tableau[compteur] * 100) / max;
            }

            for (int i = 0; i < Histogramme.GetLength(1); i++)
            {
                int j = 0;
                while (j < tableau[i])
                {
                    if (lettre == 'R')
                    {
                        Histogramme[j, i].Red = 255;
                        Histogramme[j, i].Green = 0;
                        Histogramme[j, i].Blue = 0;

                    }
                    else if (lettre == 'V')
                    {
                        Histogramme[j, i].Red = 0;
                        Histogramme[j, i].Green = 255;
                        Histogramme[j, i].Blue = 0;
                    }
                    else if (lettre == 'B')
                    {
                        Histogramme[j, i].Red = 0;
                        Histogramme[j, i].Green = 0;
                        Histogramme[j, i].Blue = 255;
                    }
                    else if (lettre == 'G')
                    {
                        Histogramme[j, i].Red = 0;
                        Histogramme[j, i].Green = 0;
                        Histogramme[j, i].Blue = 0;
                    }
                    j++;
                }
            }
            return Histogramme;
        }
    

        //Méthode de création d'images   
        
        private void CreerImage(int cas, int multiple = 0)        // Créer la nouvelle image         //cas: action sur l'image choisie
        {
            byte[] creerFile = null; // Même principe que le tableau de byte myfile
            byte[] temporaire = new byte[4];

            switch (cas)            //  Modifications à faire dans le header selon le choix
            {
                case 0: // nuance gris ; noir et blanc ; rotation 180 ; convolution ; miroir ; Fractale
                    creerFile = new byte[taille_Fichier];
                    break;
                case 1: // Grand_Effet_Miroir
                    int nouvelle_Largeur = 2 * largeur;
                    temporaire = Convertir_Int_To_Endian(nouvelle_Largeur);
                    int nouvelle_Taille = (taille_Fichier - 54) * 2 + 54;
                    creerFile = new byte[nouvelle_Taille];
                    for (int i = 0; i <= 3; i++)
                    {
                        header[18 + i] = temporaire[i];
                    }
                    break;
                case 2: // Rotation 90° ou 270°
                    creerFile = new byte[taille_Fichier];
                    for (int i = 0; i <= 3; i++)
                    {
                        temporaire[i] = header[22 + i];     // On permute dans le header, la largeur et hauteur
                        header[22 + i] = header[18 + i];
                        header[18 + i] = temporaire[i];
                    }
                    break;
                case 3: // Agrandir
                    creerFile = new byte[((largeur * multiple) * (hauteur * multiple) * 3) + 54]; // Multiplier par 3 pour les 3 couleurs-------Multiple: taille de l'agrandissement de l'image dans ce cas
                    temporaire = Convertir_Int_To_Endian(largeur * multiple);
                    for (int i = 0; i <= 3; i++)
                    {
                        header[18 + i] = temporaire[i];
                    }
                    temporaire = Convertir_Int_To_Endian(hauteur * multiple);
                    for (int i = 0; i <= 3; i++)
                    {
                        header[22 + i] = temporaire[i];
                    }
                    break;
                case 4: // Rétrécir
                    creerFile = new byte[((largeur / multiple) * (hauteur / multiple) * 3) + 54]; // Multiplier par 3 pour les 3 couleurs-------Multiple: taille du rétrécissement de l'image dans ce cas
                    temporaire = Convertir_Int_To_Endian(largeur / multiple);
                    for (int i = 0; i <= 3; i++)
                    {
                        header[18 + i] = temporaire[i];
                    }
                    temporaire = Convertir_Int_To_Endian(hauteur / multiple);
                    for (int i = 0; i <= 3; i++)
                    {
                        header[22 + i] = temporaire[i];
                    }
                    break;
            }

            for (int offset = 0; offset < 54; offset++)     //création de l'offset
            {
                creerFile[offset] = header[offset];
            }

            int ajustOffset = 0;

            for (int ligne = 0; ligne < image.GetLength(0); ligne++)            //construction des pixels qui composent l'image
            {
                for (int colonne = 0; colonne < image.GetLength(1); colonne++)
                {
                    creerFile[54 + ajustOffset] = Convert.ToByte(image[ligne, colonne].Blue);
                    creerFile[55 + ajustOffset] = Convert.ToByte(image[ligne, colonne].Green);
                    creerFile[56 + ajustOffset] = Convert.ToByte(image[ligne, colonne].Red);
                    ajustOffset += 3;
                }
            }

            File.WriteAllBytes("nouvelle_image.bmp", creerFile);  //Crée l'image et l'affiche sur internet     
            Process.Start("nouvelle_image.bmp");
        }
    }
}