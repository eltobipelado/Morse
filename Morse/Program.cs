using System;
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks; 

namespace TraductorMorseFacil
{
    class Program
    {
        private static readonly Dictionary<char, string> TextoAMorse = new Dictionary<char, string>
        {
            {'A', ".-"}, {'B', "-..."}, {'C', "-.-."}, {'D', "-.."}, {'E', "."}, {'F', "..-."},
            {'G', "--."}, {'H', "...."}, {'I', ".."}, {'J', ".---"}, {'K', "-.-"}, {'L', ".-.."},
            {'M', "--"}, {'N', "-."}, {'O', "---"}, {'P', ".--."}, {'Q', "--.-"}, {'R', ".-."},
            {'S', "..."}, {'T', "-"}, {'U', "..-"}, {'V', "...-"}, {'W', ".--"}, {'X', "-..-"},
            {'Y', "-.--"}, {'Z', "--.."},
            {'0', "-----"}, {'1', ".----"}, {'2', "..---"}, {'3', "...--"}, {'4', "....-"},
            {'5', "....."}, {'6', "-...."}, {'7', "--..." }, {'8', "---.."}, {'9', "----."},
        };

        private static readonly Dictionary<string, char> MorseATexto = TextoAMorse.ToDictionary(entry => entry.Value, entry => entry.Key);

        private const int DuracionPuntoMs = 100;
        private const int DuracionRayaMs = DuracionPuntoMs * 3;
        private const int PausaSimboloMs = DuracionPuntoMs;
        private const int PausaLetraMs = DuracionPuntoMs * 3;
        private const int PausaPalabraMs = DuracionPuntoMs * 7;

        private static async Task ReproducirSonidoMorse(string codigoMorse)
        {
            Console.WriteLine("Reproduciendo sonidos");
            for (int i = 0; i < codigoMorse.Length; i++)
            {
                char simbolo = codigoMorse[i];
                if (simbolo == '.') { Console.Beep(800, DuracionPuntoMs); await Task.Delay(PausaSimboloMs); }
                else if (simbolo == '-') { Console.Beep(800, DuracionRayaMs); await Task.Delay(PausaSimboloMs); }
                else if (simbolo == ' ')
                {
                    if (i + 1 < codigoMorse.Length && codigoMorse[i + 1] == ' ') 
                    { await Task.Delay(PausaPalabraMs - PausaSimboloMs); i++; }
                    else { await Task.Delay(PausaLetraMs - PausaSimboloMs); } 
                }
            }
            Console.WriteLine("Sonidos finalizados");
        }

        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("--- Traductor Morse ---");
                Console.WriteLine("Escribe tu mensaje. O escribe 'salir' para terminar");
                Console.Write("Escribie: ");
                string entradaUsuario = Console.ReadLine().Trim();

                if (entradaUsuario.ToLower() == "salir") { Console.WriteLine("¡Nos vemos!"); break; }
                if (string.IsNullOrEmpty(entradaUsuario)) { Console.WriteLine("No hay nada. Intentalo de nuevo, por favor"); continue; }

                bool esMorse = entradaUsuario.All(c => c == '.' || c == '-' || c == ' ') && (entradaUsuario.Contains('.') || entradaUsuario.Contains('-'));

                if (esMorse)
                {
                    Console.WriteLine("Es código Morse");
                    string textoDecodificado = string.Join(" ",entradaUsuario.Split(new string[] { "  " }, StringSplitOptions.None).Select(palabraMorse => string.Join("",palabraMorse.Split(' ').Select(letraMorse => MorseATexto.GetValueOrDefault(letraMorse, ' ')))));
                    Console.WriteLine($"Tu mensaje en texto es: {textoDecodificado}");
                    await ReproducirSonidoMorse(entradaUsuario);
                }
                else
                {
                    Console.WriteLine("Es texto");
                    string morseGenerado = string.Join(" ", entradaUsuario.ToUpper().Select(c => TextoAMorse.GetValueOrDefault(c, ""))).Replace("  ", " ");
                    Console.WriteLine($"Tu mensaje en código Morse es: {morseGenerado}");
                    await ReproducirSonidoMorse(morseGenerado);
                }
                Console.WriteLine("Traducción completada.");
            }
        }
    }
}