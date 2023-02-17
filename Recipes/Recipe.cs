namespace Recipes;

internal class Recipe
{
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

    // Name of recipe
    public string Name { get; }

    // List of ingredients
    public string[] Ingredients { get; }

    // image to  write for image
    public string Image { get; }

    // Source of recipe
    public string Source { get; }

    // Colour of printed text
    public ConsoleColor ForegroundColor { get; }

    // List of instructions
    public string[] Instructions { get; }

    // Write ingredients to screen
    private void WriteIngredients()
    {
        Console.WriteLine("   Ingredients");
        Console.WriteLine();
        for (var i = 0; i < Ingredients.Length; ++i) Console.WriteLine(" * " + Ingredients[i]);

        Console.WriteLine();
    }

    // Write instructions to screen
    private void WriteInstructions()
    {
        Console.WriteLine("    Instructions ");
        Console.WriteLine();
        for (var i = 0; i < Instructions.Length; ++i)
        {
            var startPart = " " + (i + 1) + ": ";
            var lineLength = Console.WindowWidth + startPart.Length;
            // in case of instruction being longer than console width, split it into multiple lines
            if (Instructions[i].Length + startPart.Length > Console.WindowWidth)
            {
                // split instruction into words
                var parts = Instructions[i].Split(' ');

                // sequentially write words to console, starting a new line if the current line is full
                Console.Write(startPart + new string(parts[0]));
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
            // otherwise just write the instruction to the console
            else
            {
                Console.WriteLine(startPart + Instructions[i]);
            }

            Console.WriteLine();
        }
    }

    // Print recipe to screen
    public void Print()
    {
        // Write title
        Util.WriteTitle(Name.ToUpper(), ForegroundColor, ConsoleColor.Black);

        // Write Image
        Console.WriteLine(Image);

        Console.WriteLine();


        // Write ingredients
        Console.ForegroundColor = ForegroundColor;
        WriteIngredients();

        // Write instructions
        WriteInstructions();
        Console.ForegroundColor = ConsoleColor.DarkGray;

        // Write source
        Console.WriteLine("Credit to " + Source);
        Console.ResetColor();
    }
}