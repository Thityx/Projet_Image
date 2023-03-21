using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Info
{
    class QrCode
    {
        static string Choix_Caractères()
        {
            Console.WriteLine("Choisissez la phrase a coder en QR-Code");
            Console.WriteLine("Seulement ces caractères : Chiffres , Majuscules, Espace, $ , % , * , + , . , / , :");
            string mot = Convert.ToString(Console.ReadLine());

            string Mot_A_Coder = mot.ToString();


            Console.WriteLine(" ");
            Console.WriteLine("Votre message à coder est: " + Mot_A_Coder);
            return Mot_A_Coder;
        }

        public int Valeur_Alpha_Num(char Caractère) // Permet d'obtenir la valeur alphanumérique d'un caractère pour le qr Code
        {
            if (Caractère >= '0' && Caractère <= '9') // Valeur correspond a un chiffre
            {
                return Caractère - 48;
            }
            else if (Caractère >= 'A' && Caractère <= 'Z') //Lettre majuscules
            {
                return Caractère - 48 - 7;        // -7 pour avoir juste les majuscules
            }
            if (Caractère == ' ')              // Valeur pour caractères spéciaux
            {
                return 36;
            }
            else if (Caractère == '$')
            {
                return 37;
            }
            else if (Caractère == '%')
            {
                return 38;
            }
            else if (Caractère == '*')
            {
                return 39;
            }
            else if (Caractère == '+')
            {
                return 40;
            }
            else if (Caractère == '-')
            {
                return 41;
            }
            else if (Caractère == '.')
            {
                return 42;
            }
            else if (Caractère == '/')
            {
                return 43;
            }
            else if (Caractère == ':')
            {
                return 44;
            }
            else
            {
                return -1;
            }

        }

        public string EncodageMessageQrCode(string message)
        {
            string debut = "0010";     
            string messageEncode = "";
            string fin = "0000";

            message = message.ToUpper();

            for (int i = 0; i < message.Length; i = i++)          
            {
                    int valeur = Valeur_Alpha_Num(message[i]);                                    
                    messageEncode = messageEncode + Convert.ToString(valeur);                     
            }

            string codeQr_en_Bit = debut + messageEncode + fin;
            byte[] codeQr_en_Tableau_de_Byte = new byte[0];                   

            string tampon = "";
            int j = 0;
            int compteBit = 0;      
            
            for (int i = 0; i < codeQr_en_Bit.Length; i++)
            {
                tampon = tampon + codeQr_en_Bit[i];
                compteBit++;

                if (compteBit == 8)
                {
                    codeQr_en_Tableau_de_Byte[j] = Convert.ToByte(tampon);
                    tampon = "";
                    j++;                      
                    compteBit = 0;
                }
            }

           return codeQr_en_Bit;
        }
    }
}
