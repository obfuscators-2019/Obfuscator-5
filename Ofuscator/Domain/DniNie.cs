﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Obfuscator.Domain
{
    public class DniNie
    {
        /// <summary> Tabla de asignación. </summary>
        private const string CORRESPONDENCIA = "TRWAGMYFPDXBNJZSQVHLCKE";

        public static string GenerateNIF()
        {
            var nifGenerators = new Func<string>[]
            {
                GenerateDNI, GenerateNIE
            };
            var randomizer = new Random();
            var randomIndex = randomizer.Next(10) % 2 == 0 ? 1 : 0;

            var result = nifGenerators[randomIndex]();

            return result;
        }

        public static string GenerateDNI()
        {
            var randomizer = new Random();
            var randomNumber = randomizer.Next(10000000, 99999999);
            var numbersInDni = randomNumber.ToString();
            var finalLetter = LetraNIF(numbersInDni);

            string dniGenerated = $"{numbersInDni}{finalLetter}";

            return dniGenerated;
        }

        public static string GenerateNIE()
        {
            var initialLetters = new char[] { 'X', 'Y', 'Z' };

            var randomizer = new Random();
            var randomNumber = randomizer.Next(1000000, 9999999);
            var randomIndex = randomizer.Next(0, 2);
            var nieSection1 = initialLetters[randomIndex] + randomNumber.ToString();
            var finalLetter = LetraNIE(nieSection1);

            string nieGenerated = $"{nieSection1}{finalLetter}";

            return nieGenerated;
        }

        public static List<string> GenerateDNI(int numberOfDNIs)
        {
            return GenerateMultipleDistinct(numberOfDNIs, GenerateDNI);
        }

        public static List<string> GenerateNIE(int numberOfNIEs)
        {
            return GenerateMultipleDistinct(numberOfNIEs, GenerateNIE);
        }

        public static List<string> GenerateNIF(int numberOfNifs)
        {
            return GenerateMultipleDistinct(numberOfNifs, GenerateNIF);
        }

        private static List<string> GenerateMultipleDistinct(int numberOfElements, Func<string> generator)
        {
            IEnumerable<string> results = new List<string>();
            int missingelements = numberOfElements;
            do
            {
                var lastResults = new List<string>();
                for (int i = 0; i < missingelements; i++)
                    lastResults.Add(generator());

                lastResults.AddRange(results);

                results = lastResults.Distinct();
                var currentTotal = results.Count();

                missingelements = numberOfElements - currentTotal;
            } while (missingelements > 0);

            return results.ToList();
        }

        /// <summary> Genera la letra correspondiente a un DNI. </summary>
        /// <param name="dni"> DNI a procesar. </param>
        /// <returns> Letra correspondiente al DNI. </returns>
        private static char LetraNIF(string dni)
        {
            Match match = new Regex(@"\b(\d{8})\b").Match(dni);
            if (match.Success)
                return CORRESPONDENCIA[int.Parse(dni) % 23];
            else
                throw new ArgumentException("El DNI debe contener 8 dígitos.");
        }

        /// <summary> Genera la letra correspondiente a un NIE. </summary>
        /// <param name="nie"> NIE a procesar. </param>
        /// <returns> Letra correspondiente al NIE. </returns>
        private static char LetraNIE(string nie)
        {
            Match match = new Regex(@"\b([X|Y|Z|x|y|z])(\d{7})\b").Match(nie);
            if (match.Success)
            {
                int n = int.Parse(match.Groups[2].Value); // también se podría haber usado como parámetro nie.Substring(1, 7)
                switch (char.ToUpper(nie[0])) // también se podría haber usado como parámetro Char.ToUpper(((String)match.Groups[1].Value)[0])
                {
                    case 'X':
                        return CORRESPONDENCIA[n % 23];
                    case 'Y':
                        return CORRESPONDENCIA[(10000000 + n) % 23];
                    case 'Z':
                        return CORRESPONDENCIA[(20000000 + n) % 23];
                    default:
                        return '\0';
                }
            }
            else
                throw new ArgumentException("El NIE debe comenzar con la letra X, Y o Z seguida de 7 dígitos.");
        }
    }
}
