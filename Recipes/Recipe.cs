namespace Recipes;

/// <summary>
/// Class for containing and printing recipe details
/// </summary>
internal class Recipe
{
    public Recipe(string name, string source, ConsoleColor foregroundColor, string image, string[] ingredients,
        string[] instructions)
    {
        Name = name;
        Image = fixImage(image);
        ForegroundColor = foregroundColor;
        Ingredients = ingredients;
        Instructions = instructions;
        Source = source;
    }

    /// <summary>
    /// Fix image to have consistent width in order to maintain shape
    /// </summary>
    /// <param name="image">Image to fix</param>
    /// <returns>Fixed image</returns>
    private string fixImage(string image)
    {
        var lines = image.Split("\n");
        // max width of image
        var maxWidth = lines.Max(line => line.Length);
        var fixedImage = "";
        // maintain shape of image by adding spaces to the end of each line
        foreach (var line in lines)
        {
            fixedImage += line + new string(' ', maxWidth - line.Length) + "\n";
        }

        return fixedImage;
    }

    /// <summary>
    /// Name of <see cref="Recipe"/>
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// List of ingredients
    /// </summary>
    public string[] Ingredients { get; }

    /// <summary>
    /// Image to write
    /// </summary>
    public string Image { get; }

    /// <summary>
    /// Source of recipe
    /// </summary>
    public string Source { get; }

    /// <summary>
    /// Color of printed text
    /// </summary>
    public ConsoleColor ForegroundColor { get; }

    /// <summary>
    /// List of instructions
    /// </summary>
    public string[] Instructions { get; }

    /// <summary>
    /// Write ingredients to screen
    /// </summary>
    private void WriteIngredients()
    {
        Console.WriteLine("   Ingredients");
        Console.WriteLine();
        for (var i = 0; i < Ingredients.Length; ++i) Console.WriteLine(" * " + Ingredients[i]);

        Console.WriteLine();
    }

    /// <summary>
    /// Write instructions to screen
    /// </summary>
    private void WriteInstructions()
    {
        Console.WriteLine("    Instructions ");
        Console.WriteLine();
        for (var i = 0; i < Instructions.Length; ++i)
        {
            var startPart = " " + (i + 1) + ": ";
            // the length of the line minus 1, for padding sake
            var lineLength = Console.WindowWidth - 1;
            // in case of instruction being longer than console width, split it into multiple lines
            if (Instructions[i].Length + startPart.Length > Console.WindowWidth)
            {
                // split instruction into words
                var parts = Instructions[i].Split(' ');

                // sequentially write words to console, starting a new line if the current line is full
                Console.Write(startPart + new string(parts[0]));
                foreach (var part in parts[1..])
                {
                    // if adding the next word to the current line would exceed the line length, start a new line
                    if (Console.CursorLeft + part.Length >= lineLength)
                    {
                        Console.WriteLine();
                        // Pad line with length of length of instruction indicator ,`startPart`, to keep alignment minus the following space to simplify following code
                        Console.Write(new string(' ', startPart.Length - 1));
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

    /// <summary>
    ///  Write image to screen
    /// </summary>
    private void WriteImage()
    {
        // Width of entire screen minus 2 for padding
        var width = Console.WindowWidth - 2;
        // write each line centred
        foreach (var line in Image.Split("\n"))
        {
            var (start, end) = Util.CentreArea(line.Length, width);
            Console.WriteLine(" " + new string(' ', start) + line + new string(' ', end));
        }
    }

    /// <summary>
    /// Print recipe to screen
    /// </summary>
    public void Print()
    {
        // Write title
        Util.WriteTitle(Name.ToUpper(), ForegroundColor, ConsoleColor.Black).Wait();

        // Write Image
        WriteImage();

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