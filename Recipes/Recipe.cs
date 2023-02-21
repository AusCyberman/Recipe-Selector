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
        // write lines with no extra new line
        Util.WriteTrailingLines((i) => " * ", Ingredients,
            Console.WindowWidth - 1, false);
        Console.WriteLine();
    }

    /// <summary>
    /// Write instructions to screen
    /// </summary>
    private void WriteInstructions()
    {
        Console.WriteLine("    Instructions ");
        Console.WriteLine();

        // function to write lines with trailing line
        Util.WriteTrailingLines(i => " " + (i + 1) + ": "
            , Instructions, Console.WindowWidth - 1, true);
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
        // if false print basic title
        if (!Util.WriteTitle(Name.ToUpper(), ForegroundColor, ConsoleColor.Black).Wait(700))
        {
            // simple reimplementation of it on single line
            Console.ForegroundColor = ForegroundColor;
            Console.WriteLine(" " + Name);
            Console.ResetColor();
        }
        // only write image if space for big title
        else
        {
            WriteImage();
        }

        // Write Image

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