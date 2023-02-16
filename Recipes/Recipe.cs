using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Recipes
{
    internal class Recipe
    {
        // Name of recipe


        public string Name { get; private set; }

        // List of ingredients
        public string[] Ingredients { get; private set; }

        public string Image { get; private set; }

        // Source
        public string Source { get; private set; }

        public ConsoleColor ForegroundColor { get; private set; }

        // List of instructions
        public string[] Instructions { get; private set; }


        public Recipe(string name, string source, ConsoleColor foregroundColor, string image, string[] ingredients,
            string[] instructions)
        {
            Name = name;
            Image = image;
            ForegroundColor = foregroundColor;
            Ingredients = ingredients;
            Instructions = instructions;
            Source = source;
        }

        private void WriteIngredients()
        {
            Console.WriteLine("   Ingredients");
            Console.WriteLine();
            for (int i = 0; i < Ingredients.Length; ++i)
            {
                Console.WriteLine(" * " + Ingredients[i]);
            }

            Console.WriteLine();
        }

        private void WriteInstructions()
        {
            Console.WriteLine("    Instructions ");
            Console.WriteLine();
            for (int i = 0; i < Instructions.Length; ++i)
            {
                var start_part = " " + (i + 1) + ": ";
                if (Instructions[i].Length > Console.WindowWidth)
                {
                    var parts = Instructions[i].Split(' ');
                    var lineLength = Console.WindowWidth - start_part.Length;
                    Console.Write(start_part + new string(parts[0]));
                    foreach (var part in parts[1..])
                    {
                        if (Console.CursorLeft + part.Length >= lineLength)
                        {
                            Console.WriteLine();
                            Console.Write("   ");
                        }

                        Console.Write(" " + part);
                    }

                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(start_part + Instructions[i]);
                }

                Console.WriteLine();
            }
        }

        public void Print()
        {
            // Write title
            Util.WriteTitle(Name.ToUpper(), ForegroundColor, ConsoleColor.Black);

            // Write Image
            Console.WriteLine(Image);

            Console.WriteLine();

            Console.ForegroundColor = ForegroundColor;
            WriteIngredients();

            WriteInstructions();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Credit to " + Source);
            Console.ResetColor();
        }
    }
}