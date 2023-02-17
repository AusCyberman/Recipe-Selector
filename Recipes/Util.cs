namespace Recipes;

internal class Util
{
    // Returns when size changes
    internal static async Task WaitSize(CancellationToken token)
    {
        // original window dimensions
        var windowWidth = Console.WindowWidth;
        var windowHeight = Console.WindowHeight;
        while (Console.WindowWidth == windowWidth &&
               Console.WindowHeight == windowHeight)
        {
            token.ThrowIfCancellationRequested();

            // new window Dimensions
            var newWindowWidth = Console.WindowWidth;
            var newWindowHeight = Console.WindowHeight;

            // wait for new dimensions to stabilise
            while (newWindowWidth != Console.WindowWidth && newWindowHeight != Console.WindowHeight)
            {
                newWindowWidth = Console.WindowWidth;
                newWindowHeight = Console.WindowHeight;
                // wait and allow for next updat
                await Task.Delay(200);
            }

            await Task.Yield();
        }
    }

    // returns an offset from the last one, ignoring the first index where it starts at zero
    internal static void PrintDividedArea(string[] words, int totalWidth)
    {
        // maxLength to index from words
        var maxLength = words.Length;
        while (
            // as long as maxLength doesn't go negative
            maxLength > 1 && (
                // make sure first two can fit
                totalWidth / (maxLength - 1) - words[0].Length - words[1].Length / 2 < 0 ||
                // and last one
                (totalWidth / (maxLength - 1)) - words[maxLength - 1].Length - (words[maxLength - 2].Length / 2) < 0 ||
                // make sure that they don't overlap by zipping previous one with next one and checking if the length is less than 0
                words[1..maxLength]
                    .Zip(words[2..maxLength],
                        (a, b) => totalWidth / (maxLength - 1) - a.Length / 2 - b.Length / 2)
                    .Any(a => a < 0) ||
                // make sure there is enough space in general
                words.Take(maxLength).Sum(value => value.Length) + maxLength * 2 > totalWidth))
            maxLength--;

        if (maxLength <= 1)
        {
            Console.WriteLine(words[0]);
            return;
        }

        // spacing between each midpoint
        var eachSpacing = totalWidth / (maxLength - 1);

        for (var i = 0; i < maxLength; ++i)
        {
            // take into account ot on screen
            var spacing = i switch
            {
                // no spacing at start
                0 => 0,
                // if its the end, take away start and end
                1 => maxLength == 2
                    ? eachSpacing - words[i].Length - words[i - 1].Length
                    : eachSpacing - words[i].Length / 2 - words[i - 1].Length,
                _ => i < maxLength - 1
                    ? eachSpacing - words[i].Length / 2 - words[i - 1].Length / 2
                    : eachSpacing - words[i].Length - words[i - 1].Length / 2 - words[i].Length % 2
            };

            Console.Write(new string(' ', spacing) + words[i]);
        }

        Console.WriteLine();
    }

    // Centre `space` into the area `middle` where the two return values are how many units to place before the `middle` and how many units to place after the `middle`
    internal static (int, int) CentreArea(int middleWidth, int space)
    {
        var spaceStartLength = space / 2 - middleWidth / 2 + 1;
        return (spaceStartLength, space - (spaceStartLength + middleWidth));
    }

    internal static string Wrap(string wrap, string text)
    {
        return wrap + text + wrap;
    }

    // Read and intercept a key asynchronously
    internal static async Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept, CancellationToken token)
    {
        while (true)
        {
            // detect if cancelled by external resize task
            token.ThrowIfCancellationRequested();

            // detect if key is available
            if (Console.KeyAvailable) return Console.ReadKey(intercept);

            // yield control back to executor in order to not waste resources
            await Task.Yield();
        }
    }

    // Write a title with `O` corners and line edges and text centred in the middle
    internal static void WriteTitle(string name, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        // TotalPadding on each edges
        var sidePaddingLength = 8;

        var realScreenWidth = Console.WindowWidth - 1;

        // total length of corners at top and bottom edges
        var cornerLength = 2;
        var internalPaddingLength = realScreenWidth - cornerLength - sidePaddingLength;

        // Whitespace at edges of box is only counted on one side
        var sidePadding = new string(' ', sidePaddingLength / 2);

        Console.WriteLine();

        // Writing top edge of box
        Console.WriteLine(
            Wrap(sidePadding, Wrap("O", new string('-', internalPaddingLength))));
        // upper padding
        Console.WriteLine(Wrap(sidePadding, Wrap("|", new string(' ', internalPaddingLength)))
        );

        // Calculate space before title
        var (spaceStartLength, spaceEndLength) = CentreArea(name.Length,
            internalPaddingLength);

        // Write title itself
        Console.Write(sidePadding + "|" + new string(' ', spaceStartLength));

        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
        Console.Write(name);
        Console.ResetColor();
        Console.WriteLine(new string(' ', spaceEndLength) + "|");

        // lower padding
        Console.WriteLine(Wrap(sidePadding, Wrap("|", new string(' ', internalPaddingLength))));

        // lower line
        Console.WriteLine(
            Wrap(sidePadding, Wrap("O", new string('-', internalPaddingLength))));
        Console.WriteLine();
    }


    // Create an asynchronous menu in order to enable yielding back to
    internal static async Task<int?> CreateMenu(string[] choices, int index, CancellationToken token
    )
    {
        var middleSize = 0;
        // string of invalid letters
        const string invalidLetters = "jkr";
        // Dictionary of letter to associated choice
        var choiceLetterDict = new Dictionary<char, int>();
        for (var i = 0; i < choices.Length; ++i)
        {
            var choice = choices[i];

            // leter index of choice
            var indexInChoice = 0;
            // while the current letter is already in the choiceLetter dict continue, or if the letter is invalid (j, k, r)
            while (!invalidLetters.Contains(char.ToLower(choice[indexInChoice])) &&
                   choiceLetterDict.ContainsKey(char.ToLower(choice[indexInChoice])) && indexInChoice < choice.Length)
                indexInChoice++;

            // set the letter to the choice index
            choiceLetterDict[char.ToLower(choice[indexInChoice])] = i;
            choices[i] = choice.Remove(indexInChoice, 1).Insert(indexInChoice, "|" + choice[indexInChoice] + "|")
                .Insert(0, i + 1 + ". ");

            // if the choice is longer than the current longest choice, set the middle size to the length of the choice
            if (middleSize < choices[i].Length) middleSize = choices[i].Length;
        }

        // if the screen lacks space, yield back to executor until screen is appropriate size
        while (Console.WindowWidth < middleSize || Console.WindowHeight < choices.Length) await Task.Yield();

        // offset from the left side of screen for start of menu
        var (leftOffset, _) = CentreArea(middleSize, Console.WindowWidth);
        // ofset from top of screen for start of menu
        var (topOffset, _) = CentreArea(choices.Length, Console.WindowHeight);

        Console.CursorTop = topOffset;
        foreach (var choice in choices)
        {
            Console.CursorLeft = 0;
            Console.WriteLine(new string(' ', leftOffset) + choice);
        }

        Console.CursorTop -= choices.Length - index;


        // print a notification to the screen
        void PrintNotifcation(string content)
        {
            Console.CursorTop += choices.Length - index + 3;
            var (startOffset, _) = CentreArea(content.Length, Console.WindowWidth);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(new string(' ', startOffset) + content);
            Console.ResetColor();
        }

        // clear the line and then write the plain line
        void ResetLineToUnselected()
        {
            ResetLine();
            // write unselected
            Console.Write(new string(' ', leftOffset) + choices[index]);
        }

        while (true)
        {
            Console.CursorTop = topOffset + index;

            ResetLine();

            // Write the currently selected choice
            Console.CursorLeft = leftOffset;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(Wrap(" ", "> " + choices[index].PadRight(middleSize)));
            Console.CursorLeft = leftOffset;

            // Wait for key press and assign to variable `key`
            Console.ResetColor();

            // Read the key asynchronously in order to yield back control to the parent task
            var key = await ReadKeyAsync(true, token);

            Console.CursorLeft = 0;
            // switch on specific key type
            switch (key.Key)
            {
                // actions for Up arrow and k (vim)
                case ConsoleKey.K:
                case ConsoleKey.UpArrow:
                    ResetLineToUnselected();
                    //  take 1 off index and avoid negative modulo
                    index = (index - 1 + choices.Length) % choices.Length;

                    break;
                // actions for Down arrow and j (vim)
                case ConsoleKey.J:
                case ConsoleKey.DownArrow:
                    ResetLineToUnselected();
                    index = (index + 1) % choices.Length;
                    break;

                // select the current choice
                case ConsoleKey.Enter:
                    Console.CursorTop += choices.Length - index;
                    ResetLine();
                    return index;
                // reload the current menu (typically for when the screen is resized)
                case ConsoleKey.R:
                    return null;

                // quit the menu
                case ConsoleKey.Escape:
                    // prompt quitting
                    PrintNotifcation("Are you sure you want to quit? (y/n): ");
                    switch (Console.ReadLine())
                    {
                        case "y":
                        case "yes":
                            return -1;
                        default:
                            Console.CursorTop--;
                            ResetLine();
                            break;
                    }

                    break;
                // process jumping to characters
                default:
                    // if the key is a digit, parse it and subtract 1 to get the index
                    if (char.IsDigit(key.KeyChar))
                    {
                        var number = int.Parse(key.KeyChar.ToString()) - 1;
                        // check if it is in the range of choices
                        if (number < choices.Length)
                        {
                            ResetLineToUnselected();
                            index = number;
                        }

                        break;
                    }

                    // if the key is a letter, check if it is in the choiceLetterDict
                    var lower = char.ToLower(key.KeyChar);
                    if (choiceLetterDict.ContainsKey(lower))
                    {
                        ResetLineToUnselected();
                        index = choiceLetterDict[lower];
                    }

                    break;
            }
        }
    }

    // Reset the current line to be blank
    private static void ResetLine()
    {
        var currentLineCursor = Console.CursorTop;
        Console.ResetColor();
        Console.CursorLeft = 0;
        // Write out entire line with spaces
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }
}