namespace Recipes;

internal class Util
{
    /// <summary>
    /// Returns when size changes
    /// </summary>
    /// <param name="token">Token to allow early cancellation</param>
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

    /// <summary>
    /// Write elements of <see cref="lines"/> where no line can overflow
    /// </summary>
    /// <param name="startFunction">Function to generate start of line</param>
    /// <param name="lines">List of all line parts</param>
    /// <param name="lineLength">Total length of line to fill up</param>
    /// <param name="endWithExtraNewLine">If the line is finished with an extra new line</param>
    internal static void WriteTrailingLines(Func<int, string> startFunction, string[] lines, int lineLength,
        bool endWithExtraNewLine)
    {
        for (var i = 0; i < lines.Length; ++i)
        {
            var startPart = startFunction(i);
            // the length of the line minus 1, for padding sake
            // in case of line being longer than lineLength, split it into multiple lines
            if (lines[i].Length + startPart.Length > lineLength)
            {
                // split line into words
                var parts = lines[i].Split(' ');

                // sequentially write words to console, starting a new line if the current line is full
                Console.Write(startPart + new string(parts[0]));
                foreach (var part in parts[1..])
                {
                    // if adding the next word to the current line would exceed the line length, start a new line
                    if (startPart.Length >= Console.CursorLeft)
                    {
                        Console.WriteLine(part);
                        continue;
                    }

                    if (Console.CursorLeft + part.Length >= lineLength && Console.CursorLeft > startPart.Length - 1)
                    {
                        Console.WriteLine();
                        // Write line padding to length of start length
                        Console.Write(new string(' ', startPart.Length - 1));
                    }

                    Console.Write(" " + part);
                }

                Console.WriteLine();
            }
            // otherwise just write the line to the console
            else
            {
                Console.WriteLine(startPart + lines[i]);
            }

            // end With line
            if (endWithExtraNewLine)
            {
                Console.WriteLine();
            }
        }
    }

    /// <summary>
    /// Writes to screen all the <see cref="parts"/> centred on equal intervals where the first is pinned to the left hand side and the last is pinned to the right hand side.
    /// if parts cannot fit on screen, the last is deleted
    /// </summary>
    /// <param name="parts">Parts to shown on screen</param>
    /// <param name="totalWidth">length the area in which to place the parts</param>
    internal static void PrintDividedArea(string[] parts, int totalWidth)
    {
        // maxLength to index from parts
        var maxLength = parts.Length;
        while (
            // as long as maxLength doesn't go negative
            maxLength > 1 && (
                // make sure first two can fit
                totalWidth / (maxLength - 1) - parts[0].Length - parts[1].Length / 2 < 0 ||
                // and last one
                (totalWidth / (maxLength - 1)) - parts[maxLength - 1].Length - (parts[maxLength - 2].Length / 2) < 0 ||
                // make sure that they don't overlap by zipping previous one with next one and checking if the length is less than 0
                parts[1..maxLength]
                    .Zip(parts[2..maxLength],
                        (a, b) => totalWidth / (maxLength - 1) - a.Length / 2 - b.Length / 2)
                    .Any(a => a < 0) ||
                // make sure there is enough space in general
                parts.Take(maxLength).Sum(value => value.Length) + maxLength * 2 > totalWidth))
            maxLength--;

        if (maxLength <= 1)
        {
            Console.WriteLine(parts[0]);
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
                    // if there is only two, take away the start and the end to get the spacing
                    ? eachSpacing - parts[i].Length - parts[i - 1].Length
                    // otherwise take away the start and the middle of the current one
                    : eachSpacing - parts[i].Length / 2 - parts[i - 1].Length,
                _ => i < maxLength - 1
                    // if its not the end, take away the middle of the current one and the middle of the previous one
                    ? eachSpacing - parts[i].Length / 2 - parts[i - 1].Length / 2
                    // in case last one, take away the middle of the second last one and the entire end length
                    : eachSpacing - parts[i].Length - parts[i - 1].Length / 2 - parts[i].Length % 2
            };

            Console.Write(new string(' ', spacing) + parts[i]);
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Centre <see cref="space"/> into the area <see cref="middleWidth"/> where the two return values are how many units to place before the <see cref="middleWidth"/> and how many units to place after
    /// </summary>
    /// <param name="middleWidth">Width of middle area</param>
    /// <param name="space">Total width available</param>
    /// <returns></returns>
    internal static (int, int) CentreArea(int middleWidth, int space)
    {
        // take all space, divide it by tw oand take off the middleWidth / 2
        var spaceStartLength = space / 2 - middleWidth / 2;
        // if the starting spaces negative, make the space zero
        if (spaceStartLength <= 0)
            return (0, 0);
        return (spaceStartLength, space - (spaceStartLength + middleWidth));
    }

    /// <summary>
    /// Wrap <see cref="text"/> with <see cref="wrap"/>
    /// </summary>
    /// <param name="wrap">Text to place at start and end</param>
    /// <param name="text"></param>
    /// <returns></returns>
    internal static string Wrap(string wrap, string text)
    {
        return wrap + text + wrap;
    }

    /// <summary>
    /// Read a key asynchronously
    /// </summary>
    /// <param name="intercept">Print readkey to screen or not</param>
    /// <param name="token">Token to allow for early exit</param>
    /// <returns></returns>
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

    /// <summary>
    /// Write a title with "O" corners and line edges and <see cref="name"/> centred in the middle. Async to allow <see cref="WaitSize"/>
    /// </summary>
    /// <param name="name">Text to centre</param>
    /// <param name="foregroundColor">Foreground color of text</param>
    /// <param name="backgroundColor">Background color of the text</param>
    /// <returns></returns>
    internal static async Task WriteTitle(string name, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        // TotalPadding on each edges
        var sidePaddingLength = 8;

        var realScreenWidth = Console.WindowWidth - 1;

        // total length of corners at top and bottom edges
        var cornerLength = 2;
        var internalPaddingLength = realScreenWidth - cornerLength - sidePaddingLength;


        // Calculate space before title
        var (spaceStartLength, spaceEndLength) = CentreArea(name.Length,
            internalPaddingLength);

        // If screen is too small, yield back using await size
        while (cornerLength + sidePaddingLength + name.Length > realScreenWidth)
        {
            await WaitSize(CancellationToken.None);
            (spaceStartLength, spaceEndLength) = CentreArea(name.Length,
                internalPaddingLength);
            realScreenWidth = Console.WindowWidth - 1;
            internalPaddingLength = realScreenWidth - cornerLength - sidePaddingLength;
        }

        // Whitespace at edges of box is only counted on one side
        var sidePadding = new string(' ', sidePaddingLength / 2);

        Console.WriteLine();

        // Writing top edge of box
        Console.WriteLine(
            Wrap(sidePadding, Wrap("O", new string('-', internalPaddingLength))));
        // upper padding
        Console.WriteLine(Wrap(sidePadding, Wrap("|", new string(' ', internalPaddingLength)))
        );


        // Write title itself
        Console.Write(sidePadding + "|" + new string(' ', spaceStartLength));

        // change colours to passeed in colours
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


    /// <summary>
    /// Create an asynchronous menu with controls
    /// <list type="table">
    ///     <item>
    ///         <term>J/Down Arrow</term>
    ///         <description>Down</description>
    ///     </item>
    ///     <item>
    ///         <term>K/Up Arrow</term>
    ///         <description>Up</description>
    ///     </item>
    ///     <item>
    ///          <term>R</term>
    ///          <description>Reload screen</description>
    ///     </item>
    ///     <item>
    ///         <term>ESC</term>
    ///         <description>Prompt to close program</description>
    ///     </item>
    ///     <item>
    ///         <term>Enter</term>
    ///         <description>Select</description>
    ///     </item>
    ///     <item>
    ///         <term>Numbers</term>
    ///         <description>Jump to number</description>
    ///     </item>
    ///     <item>
    ///         <term>Other letters</term>
    ///         <description>Jump to letter surrounded with pipes</description>
    ///     </item>
    /// </list>
    /// Function is async in order to enable yielding back to executor
    /// </summary>
    /// <param name="choices">List of possible choices</param>
    /// <param name="startIndex">Index to start choices at</param>
    /// <param name="token">Token to allow early exit</param>
    /// <returns></returns>
    internal static async Task<int?> CreateMenu(string[] choices, int startIndex, CancellationToken token
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
            while ((invalidLetters.Contains(char.ToLower(choice[indexInChoice])) ||
                    choiceLetterDict.ContainsKey(char.ToLower(choice[indexInChoice]))) && indexInChoice < choice.Length)
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

        // Reset cursor to current index
        Console.CursorTop -= choices.Length - startIndex;


        // print a notification to the screen
        void PrintNotifcation(string content)
        {
            Console.CursorTop += choices.Length - startIndex + 3;
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
            Console.Write(new string(' ', leftOffset) + choices[startIndex]);
        }

        while (true)
        {
            Console.CursorTop = topOffset + startIndex;

            ResetLine();

            // Write the currently selected choice


            Console.CursorLeft = leftOffset;

            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(Wrap(" ", "> " + choices[startIndex].PadRight(middleSize)));
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
                    startIndex = (startIndex - 1 + choices.Length) % choices.Length;

                    break;
                // actions for Down arrow and j (vim)
                case ConsoleKey.J:
                case ConsoleKey.DownArrow:
                    ResetLineToUnselected();
                    startIndex = (startIndex + 1) % choices.Length;
                    break;

                // select the current choice
                case ConsoleKey.Enter:
                    Console.CursorTop += choices.Length - startIndex;
                    ResetLine();
                    return startIndex;
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
                            startIndex = number;
                        }

                        break;
                    }

                    // if the key is a letter, check if it is in the choiceLetterDict
                    var lower = char.ToLower(key.KeyChar);
                    if (choiceLetterDict.ContainsKey(lower))
                    {
                        ResetLineToUnselected();
                        startIndex = choiceLetterDict[lower];
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// Reset the current line to be blank
    /// </summary>
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