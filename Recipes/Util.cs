using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Recipes
{
    internal class Util
    {
        // returns an offset from the last one, ignoring the first index where it starts at zero
        internal static void PrintDividedArea(string[] words, int totalWidth)
        {
            var maxLength = words.Length;
            while (maxLength > 1 && (
                       (totalWidth / (maxLength - 1)) - words[0].Length - (words[1].Length / 2) < 0 ||
                       words[1..maxLength]
                           .Zip(words[2..maxLength],
                               (a, b) => (totalWidth / (maxLength - 1)) - (a.Length / 2) - (b.Length / 2))
                           .Any(a => a < 0) ||
                       words.Take(maxLength).Sum(value => value.Length) + maxLength * 2 > totalWidth))
            {
                maxLength--;
            }

            if (maxLength <= 1)
            {
                Console.WriteLine(words[0]);
                return;
            }

            var eachSpacing = totalWidth / (maxLength - 1);

            for (int i = 0; i < maxLength; ++i)
            {
                var spacing = i switch
                {
                    0 => 0,
                    1 =>
                        eachSpacing - (words[i].Length / 2) - words[i - 1].Length,
                    _ => i < maxLength - 1
                        ? eachSpacing - (words[i].Length / 2) - (words[i - 1].Length / 2)
                        : eachSpacing - (words[i].Length) - (words[i - 1].Length / 2) - words[i].Length % 2,
                };


                Console.Write(new string(' ', spacing) + words[i]);
            }

            Console.WriteLine();
        }

        internal static (int, int) CentreText(int middle, int space)
        {
            var spaceStartLength = space / 2 - (middle / 2) + 1;
            return (spaceStartLength, space - (spaceStartLength + middle));
        }

        internal static string Wrap(string wrap, string text)
        {
            return wrap + text + wrap;
        }

        internal async static Task<ConsoleKeyInfo> ReadKeyAsync(CancellationToken token)
        {
            while (true)
            {
                // detect if cancelled by external resize task
                token.ThrowIfCancellationRequested();
                if (Console.KeyAvailable)
                {
                    return Console.ReadKey(true);
                }

                // yield control back to executor in order to not waste resources
                await Task.Yield();
            }
        }

        internal static void WriteTitle(string Name, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            int sidePaddingLength = 8;

            int realScreenWidth = Console.WindowWidth - 1;

            var cornerLength = 2;
            var internalPaddingLength = realScreenWidth - cornerLength - sidePaddingLength;

            // Whitespace  at edges of box
            string sidePadding = new string(' ', sidePaddingLength / 2);

            Console.WriteLine();

            // Writing top edge of box
            Console.WriteLine(
                Wrap(sidePadding, Wrap("O", new string('-', internalPaddingLength))));
            // upper padding
            Console.WriteLine(Wrap(sidePadding, Wrap("|", new string(' ', internalPaddingLength)))
            );

            // Calculate space before title
            var (spaceStartLength, spaceEndLength) = CentreText(Name.Length,
                internalPaddingLength);

            // Title Name

            Console.Write(sidePadding + "|" + new string(' ', spaceStartLength));

            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(Name);
            Console.ResetColor();
            Console.WriteLine(new string(' ', spaceEndLength) + "|");

            // lower padding
            Console.WriteLine(Wrap(sidePadding, Wrap("|", new string(' ', internalPaddingLength))));

            // lower line
            Console.WriteLine(
                Wrap(sidePadding, Wrap("O", new string('-', internalPaddingLength))));
            Console.WriteLine();
        }


        internal async static Task<int?> CreateMenu(string[] choices, int index, CancellationToken token)
        {
            int middleSize = 0;
            var invalidLetters = "jkr";
            var choiceDict = new Dictionary<char, int>();
            for (int i = 0; i < choices.Length; ++i)
            {
                var choice = choices[i];

                var choiceIndex = 0;
                while (!invalidLetters.Contains(char.ToLower(choice[choiceIndex])) &&
                       choiceDict.ContainsKey(char.ToLower(choice[choiceIndex])) && choiceIndex < choices.Length)
                {
                    choiceIndex++;
                }

                choiceDict[char.ToLower(choice[choiceIndex])] = i;
                choices[i] = choice.Remove(choiceIndex, 1).Insert(choiceIndex, "|" + choice[choiceIndex] + "|")
                    .Insert(0, (i + 1) + ". ");
                if (middleSize < choices[i].Length)
                {
                    middleSize = choices[i].Length;
                }
            }

            while (Console.WindowWidth < middleSize || Console.WindowHeight < choices.Length)
            {
                await Task.Yield();
            }

            // offset from the left side of screen for start of menu
            var (leftOffset, _) = CentreText(middleSize, Console.WindowWidth);
            // ofset from top of screen
            var (topOffset, _) = CentreText(choices.Length, Console.WindowHeight);

            Console.CursorTop = topOffset;
            foreach (var choice in choices)
            {
                Console.CursorLeft = 0;
                Console.WriteLine(new string(' ', leftOffset) + choice);
            }

            Console.CursorTop -= choices.Length - index;

            void resetLine()
            {
                ResetLine();
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
                ConsoleKeyInfo key = await ReadKeyAsync(token);

                Console.CursorLeft = 0;
                // switch on specific key type
                switch (key.Key)
                {
                    case ConsoleKey.K:
                    case ConsoleKey.UpArrow:
                        resetLine();
                        index = (index - 1 + choices.Length) % choices.Length;

                        break;
                    case ConsoleKey.J:
                    case ConsoleKey.DownArrow:
                        resetLine();
                        index = (index + 1) % choices.Length;
                        break;

                    case ConsoleKey.Enter:
                        Console.CursorTop += choices.Length - index;
                        ResetLine();
                        return index;
                    case ConsoleKey.R:
                        return null;
                    case ConsoleKey.Escape:
                        Console.CursorTop += choices.Length - index + 1;
                        var text = "Are you sure you want to quit? (y/n): ";
                        var (startOffset, _) = CentreText(text.Length, Console.WindowWidth);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("\n\n" + new string(' ', startOffset) + text);
                        Console.ResetColor();
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
                    default:
                        if (char.IsDigit(key.KeyChar))
                        {
                            var number = int.Parse(key.KeyChar.ToString()) - 1;
                            if (number < choices.Length)
                            {
                                resetLine();
                                index = number;
                            }

                            break;
                        }

                        var lower = char.ToLower(key.KeyChar);
                        if (choiceDict.ContainsKey(lower))
                        {
                            resetLine();
                            index = choiceDict[lower];
                        }

                        break;
                }
            }
        }

        internal static void ResetLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.ResetColor();
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}