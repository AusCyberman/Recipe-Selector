using System.Drawing;
using System.Runtime.InteropServices;

namespace Recipes;

internal class Program
{
    // List of all Recipes
    private static Recipe[] Recipes =
    {
        new("Donna Hay’s Fluffy Pancakes",
            "https://www.donnahay.com.au/recipes/breakfast/fluffy-pancakes",
            ConsoleColor.Yellow,
            """
                ██▓▓████████████
          ██████▒▒▒▒░░▒▒▒▒░░░░▒▒██████
      ████▒▒░░▒▒░░░░░░░░░░░░░░░░▒▒▒▒▒▒████
    ██▒▒░░░░░░░░░░░░░░▒▒▒▒░░░░░░░░░░▒▒▒▒▒▒██
  ██▒▒░░░░░░░░░░░░░░▒▒▒▒▒▒▒▒░░░░░░░░░░▒▒▒▒▒▒██
  ██▒▒░░░░░░░░      ▒▒▒▒▒▒▒▒  ░░░░░░░░░░░░░░▓▓
██▒▒░░░░░░        ▒▒▒▒▒▒▒▒▒▒▓▓    ░░░░░░░░░░▒▒██
██▒▒░░░░░░        ▒▒▒▒▒▒▒▒▒▒▒▒▒▒  ░░░░░░▒▒▒▒▒▒██
██▒▒░░░░░░          ▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒▒▒██
██▒▒░░░░░░░░░░        ▒▒▒▒▒▒▒▒▒▒▒▒░░░░▒▒▒▒▒▒▒▒██
██▒▒▒▒░░░░░░░░░░░░░░░░░░░░▒▒▒▒▒▒░░░░▒▒▒▒▒▒░░▓▓██
██  ▒▒▒▒▒▒░░░░░░░░░░░░░░░░░░░░░░▒▒▒▒░░▒▒▒▒▓▓░░██
██    ▒▒▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░░░▒▒▒▒▒▒▓▓▓▓░░░░██
██▒▒      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░▒▒▒▒▓▓▓▓▓▓░░░░░░▓▓██
██░░▒▒          ▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓░░░░░░░░░░▓▓▒▒██
██  ░░▓▓▓▓          ░░░░░░░░░░░░░░░░░░▓▓▓▓▒▒░░██
██    ░░░░▒▒▒▒▒▒    ░░░░░░░░░░░░▓▓▓▓▓▓▒▒▒▒░░░░██
██▓▓      ░░░░░░▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▒▒░░░░░░▓▓██
██░░▒▒          ░░░░▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░░░░░▓▓▒▒██
██  ░░▒▒▒▒          ░░░░░░░░░░░░░░░░░░▓▓▓▓▒▒░░██
  ██  ░░░░▒▒▒▒▒▒    ░░░░░░░░░░░░▓▓▓▓▓▓▒▒▒▒░░▓▓██
  ██      ░░░░░░▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▒▒░░░░░░████
    ██          ░░░░▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░░░░░▒▒
      ████          ░░░░░░░░░░░░░░░░░░▓▓▓▓
          ██████    ░░░░░░░░░░░░▓▓▓▓▓▓
                ▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓
""",
            new[]
            {
                "1 and 1/3 cups (175g) plain flour",
                "1 tablespoon baking powder",
                "1/4 cup (55g) caster (superfine) sugar",
                "1 egg",
                "1 cup (250ml) milk",
                "2 teaspoons vanilla extract (my secret trick)",
                "40g unsalted butter, melted"
            }, new[]
            {
                "Place the flour, baking powder and sugar in a big bowl and mix to combine, using a whisk.",
                "Break the egg into a medium jug. Add the milk, vanilla and melted butter and whisk to combine.",
                "Using a spoon, dig a well in the centre of the flour mixture. Pour the egg mixture into the well, then whisk it all together to make a smooth batter.",
                "Heat a large, lightly greased non-stick frying pan over medium heat. Using a ladle or a cup, add about ¼ cup (60ml) of the batter to the pan for each pancake (you can cook 2–3 at a time, just leave yourself enough space for flipping). Cook pancakes for 1–2 minutes each side or until they’re golden, using a turner to flip them.",
                "Stack and serve the pancakes warm from the pan with some natural yoghurt, maple syrup and berries (or your own favourite toppings)."
            }),
        new("Apple Crumble",
            "https://www.taste.com.au/recipes/quick-easy-apple-crumble/4f2e1fb8-2060-4e27-9833-a5ab7ab69717",
            ConsoleColor.DarkGreen,
            """""""""""
                             ___
                          _/`.-'`.
                _      _/` .  _.'
       ..:::::.(_)   /` _.'_./
     .oooooooooo\ \o/.-'__.'o.
    .ooooooooo`._\_|_.'`oooooob.
  .ooooooooooooooooooooo&&oooooob.
 .oooooooooooooooooooo&@@@@@@oooob.
.ooooooooooooooooooooooo&&@@@@@ooob.
doooooooooooooooooooooooooo&@@@@ooob
doooooooooooooooooooooooooo&@@@oooob
dooooooooooooooooooooooooo&@@@ooooob
dooooooooooooooooooooooooo&@@oooooob
`dooooooooooooooooooooooooo&@ooooob'
 `doooooooooooooooooooooooooooooob'
  `doooooooooooooooooooooooooooob'
   `doooooooooooooooooooooooooob'
    `doooooooooooooooooooooooob'
     `doooooooooooooooooooooob'
      `dooooooooobodoooooooob'
       `doooooooob dooooooob'
         `"""""""' `""""""'
""""""""""",
            new[]
            {
                "3 medium apples, peeled, cored and diced ",
                "1 tbsp lemon juice",
                "1 tbsp caster sugar",
                "1/4 cup water",
                "1/3 cup plain flour",
                "1/3 cup caster sugar",
                "1/3 cup Uncle Tobys Traditional Rolled Oats",
                "60g butter, chopped",
                "Ice-cream or whipped cream, to serve"
            },
            new[]
            {
                "Preheat oven to 180C/160C fan-forced. Combine apples, juice, sugar and water in small saucepan over low-medium heat. Cook, stirring for 3 minutes or until apple is slightly softened.",
                "To make the crumble, combine the flour, sugar, oats and butter in a bowl. Use your fingertips to rub the butter into the flour mixture until the mixture resembles breadcrumbs.",
                "Transfer apples into a 3 cup-capacity ovenproof dish, draining off most of the liquid. Sprinkle crumble mixture over the apples. Bake in oven for 20-25 minutes or until golden. Serve warm with ice-cream or whipped cream."
            }),
        new("Sushi",
            "https://www.taste.com.au/recipes/sushi-2/0d57247f-03a6-434d-b26b-071b91614d41",
            ConsoleColor.White,
            """"
 ,;'O@';,    ,;'O@';,    ,;'O@';,
|',_@H_,'|  |',_@H_,'|  |',_@H_,'|
|        |  |        |  |        |
 '.____.'    '.____.'    '.____.'
"""",
            new[]
            {
                "2 1/2 cups (540g) koshihikari rice", "3 3/4 cups (935ml) cold water ",
                "1/2 cup (125ml) rice vinegar ",
                "2 tbsp caster sugar", "1/2 tsp salt", "6 nori sheets",
                "200g fresh salmon, cut into 1cm-thick batons",
                "1 avocado, halved, stoned, peeled, thinly sliced", "Light soy sauce, to serve",
                "Wasabi paste, to serve", "Pickled ginger, to serve"
            }, new[]
            {
                "Place the rice in a sieve. Rinse under cold running water, to remove any excess starch, until water runs clear. Place the rice and water in a large saucepan, covered, over high heat. Bring to the boil. Reduce heat to low and cook, covered, for 12 minutes or until all the water is absorbed. Remove from heat. Set aside, covered, for 10 minutes to cool slightly.",
                "Combine the vinegar, sugar and salt in a small bowl. Transfer the rice to a large glass bowl. Use a wooden paddle to break up rice lumps while gradually adding the vinegar mixture, gently folding to combine. Continue folding and fanning the rice for 15 minutes or until rice is cool.",
                "Place a sushi mat on a clean surface with slats running horizontally. Place a nori sheet, shiny-side down, on the mat. Use wet hands to spread one-sixth of the rice over the nori sheet, leaving a 3cm-wide border along the edge furthest away from you.",
                "Place salmon and avocado along the centre of the rice. Hold filling in place while rolling the mat over to enclose rice and filling. Repeat with remaining nori, rice, salmon and avocado.",
                "Use a sharp knife to slice sushi widthways into 1.5cm-thick slices. Place on serving dishes with soy sauce, wasabi and pickled ginger, if desired."
            }),
        new("Chilli Prawn Spaghetti",
            "https://www.taste.com.au/recipes/chilli-prawn-tomato-spaghetti/c6ab8078-1b4b-408a-9249-89ea428a2790",
            ConsoleColor.Yellow,
            """"
SOMEBODY TOUCHA MY SPAGHET, WHERE'D IT GO!
"""",
            new[]
            {
                "200g dried spaghetti pasta", "2 tsp extra virgin olive oil", "2 garlic cloves, thinly sliced",
                "Pinch dried chilli flakes", "10 (325g) medium green prawns, peeled (tails intact), deveined",
                "3 medium tomatoes, deseeded, finely chopped", "1 tbsp chopped fresh continental parsley leaves"
            },
            new[]
            {
                "Cook pasta in a large saucepan of boiling, salted water following packet directions, until tender. Drain, reserving 1/4 cup cooking liquid.",
                "Meanwhile, heat oil in a large frying pan over medium-high heat. Add garlic and chilli. Cook for 1 minute or until fragrant. Add prawns. Cook, stirring, for 2 to 3 minutes or until pink and cooked through. Add pasta, tomato and cooking liquid. Cook for 2 minutes or until heated through. Season with salt and pepper. Stir through parsley. Serve.`"
            }
        ),
        new("Healthy fried rice with egg",
            "https://www.taste.com.au/recipes/healthy-fried-rice-egg-recipe/937v81xz",
            ConsoleColor.DarkYellow,
            """
      ████
    ██░░░░██
  ██░░░░░░░░██
  ██░░░░░░░░██
██░░░░░░░░░░░░██
██░░░░░░░░░░░░██
██░░░░░░░░░░░░██
  ██░░░░░░░░██
    ████████
""",
            new[]
            {
                "2 tsp sesame oil", "2 eggs, lightly whisked", "1 large carrot, cut into matchsticks",
                "4 green shallots, thinly sliced, plus extra, thinly sliced, to serve",
                "1 head (about 300g) broccoli, stalks thinly sliced, head cut into small florets",
                "150g snow peas, cut into thirds", "1 bunch gai lan (Chinese broccoli), cut into 3cm pieces",
                "1 large red capsicum, deseeded, finely chopped", "405g (3 cups) cooked brown rice",
                "2 tbsp salt-reduced soy sauce", "1 tbsp mirinh"
            },
            new[]
            {
                "Lightly spray a large wok with oil. Heat over medium heat. Add half the egg and cook for 30 seconds then swirl to coat the base. Cook for a further 30 seconds. Gently loosen the edge of the omelette and transfer to a chopping board. Repeat with remaining egg. Roll up omelette and thinly slice.",
                "Increase heat to high. Add the sesame oil, carrot and shallot to the wok. Stir-fry for 1 minute. Add the broccoli, snow peas, gai lan and capsicum. Stir-fry for 2 minutes or until tender crisp. Add the rice, soy sauce and mirin. Stir-fry until well combined and heated through. Add half the omelette and stir until combined.",
                "Divide the fried rice among serving bowls. Top with the remaining omelette and extra shallot to serve."
            }),
        new(
            "Lamb Tikka Curry",
            "https://www.taste.com.au/recipes/lamb-tikka-curry-recipe/sxf892yk",
            ConsoleColor.Gray,
            """"
        __  _
    .-.'  `; `-._  __  _
   (_,         .-:'  `; `-._
 ,'o"(        (_,           )
(__,-'      ,'o"(            )>
   (       (__,-'            )
    `-'._.--._(             )
       |||  |||`-'._.--._.-'
                  |||  |||
"""", new[]
            {
                "1 brown onion, thinly sliced", "200g butternut pumpkin, peeled, seeded, cut into 3cm pieces",
                "500g lamb mince", "1/4 cup (60g) tikka curry paste", "1 cup small cauliflower florets",
                "400g can diced tomatoes", "1/2 cup (125ml) chicken stock", "100g baby spinach leaves",
                "100g Greek-style yoghurt", "Steamed basmati rice, to serve"
            },
            new[]
            {
                "Spray a large frying pan with olive oil spray. Place over medium-high heat. Add the onion and pumpkin. Cook, stirring, for 5 mins or until lightly browned. Add mince and cook, stirring with a wooden spoon to break up lumps, for 3-4 mins or until the mince changes colour.",
                "Add curry paste to the mince mixture in the pan and cook, stirring, for 1 min or until aromatic. Add cauliflower, tomato and stock. Bring to the boil. Reduce heat to medium and partially cover. Cook for 10 mins or until the pumpkin is tender and sauce thickens slightly.",
                "Stir spinach into the curry. Cook for 1 min or until spinach wilts. Remove from heat. Stir in yoghurt. Serve with rice."
            }),
        new(
            "Caramilk buttercrust slice",
            "https://www.taste.com.au/recipes/caramilk-buttercrust-slice-recipe/1c5f5c1e-8c1c-4b8d-8c1c-4b8d8c1c4b8d",
            ConsoleColor.DarkYellow,
            """
()()()()()()
|\         |
|.\. . . . |
\'.\       |
 \.:\ . . .|
  \'o\     |
   \.'\. . |
    \".\   |
     \'`\ .|
      \.'\ |
       \__\|
""",
            new[]
            {
                "250g packet plain sweet biscuits", "125g butter, melted", "2 tsp gelatine powder",
                "500g cream cheese, at room temperature, chopped", "395g can sweetened condensed milk",
                "180g packet Cadbury Caramilk chocolate, melted", "39g Cadbury Twirl Caramilk bar, crumbled"
            },
            new[]
            {
                "Grease a 4cm-deep, 16 x 26cm (base size) slice pan and line with baking paper, allowing the paper to overhang the sides.",
                "Break the biscuits into a food processor and process until coarsely crushed. Add the butter and process until combined. Transfer to the prepared pan. Use a straight-sided glass to spread and press mixture firmly over base and into sides of pan. Place in the fridge for 20 minutes or until firm.",
                "Place 2 tbs water in a small microwave-safe bowl. Sprinkle with the gelatine and stir to combine. Microwave for 10 seconds (do not overheat). Use a fork to whisk until the gelatine dissolves. Set aside to cool slightly.",
                "Use electric beaters to beat cream cheese in a bowl until smooth. Add the condensed milk and melted chocolate. Beat until combined. Add the gelatine mixture and beat until well combined. Pour over the biscuit mixture and smooth the surface. Place in the fridge for 4 hours or until set.",
                "Transfer the slice to a serving board. Sprinkle with crumbled chocolate. Serve."
            })
    };

    private static async Task Main()
    {
        var index = 0;
        // check if windows
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // make the console go to the top left
            Console.SetWindowPosition(0, 0);
            // Expand console to largest size, except with some smaller edges
            Console.SetWindowSize(Console.LargestWindowWidth - 2, Console.LargestWindowHeight);
        }

        // instructions which users can select, last one disappears when screen too small
        var instructions = new[]
        {
            "ESC to exit", "Use Up/Down arrows or J/K to navigate", "ENTER to Select",
            "R to reload",
            "Press letter surrounded in || or number to jump to selection"
        };


        // asynchronous function to wait for a key press, then return the loop state of reset
        var sizeReset = async (CancellationToken token) =>
        {
            await Util.WaitSize(token);
            return LoopState.Reset;
        };

        // asynchronous function to create menu and set the current index to the resulting selection
        var menu = async (CancellationToken token) =>
        {
            var names = Recipes.Select(recipe => recipe.Name).ToArray();
            var currentIndex = await Util.CreateMenu(names, index, token);
            // detect reset
            if (currentIndex == null)
                return LoopState.Reset;
            // detect exit
            if (currentIndex < 0) return LoopState.End;


            index = currentIndex.Value;
            return LoopState.Display;
        };

        while (true)
        {
            // Reset colour and data
            Console.ResetColor();
            Console.Clear();

            Console.WriteLine();
            // Write top instructions with starting padding
            Console.Write(new string(' ', 4));
            Util.PrintDividedArea(instructions, Console.WindowWidth - 5);
            Console.WriteLine();


            // Write menu title
            await Util.WriteTitle(new string("RECIPE SELECTOR".SelectMany(a => a + " ").ToArray()).TrimEnd(),
                ConsoleColor.DarkCyan,
                ConsoleColor.Black);


            // cancellation token in order to stop unmanaged threads
            var source = new CancellationTokenSource();


            // switch on result of menu result or resize event
            switch (await await Task.WhenAny(menu(source.Token), sizeReset(source.Token)))

            {
                // Display menu
                case LoopState.Display:
                    source.Cancel();
                    break;
                // Exit program
                case LoopState.End:
                    return;
                // redraw screen
                case LoopState.Reset:
                    source.Cancel();
                    continue;
            }


            // cancellation token to stop resize task still running when reading recipe
            var recipeSource = new CancellationTokenSource();

            // throws warning, so pragma ignores it
#pragma warning disable CS4014
            // task to run in background for updating size
            var task = Task.Run(async () =>
#pragma warning restore CS4014
            {
                var token = recipeSource.Token;
                // Recipe print loop
                while (!token.IsCancellationRequested)
                {
                    Console.Clear();
                    Recipes[index].Print();

                    // Reset colour so that the press key stands out
                    Console.ResetColor();
                    Console.WriteLine("Press any key to return to menu");
                    Console.ResetColor();
                    // Wait for size change
                    await Util.WaitSize(token);
                }
                // termianted
            });
            // Reset colour to default color

            // Prompt for 'y' or 'yes' to end program, or continue on all other prompts
            await Util.ReadKeyAsync(false, recipeSource.Token);
            recipeSource.Cancel();
            Console.WriteLine(task.Exception);
        }
    }


    /// <summary>
    /// Tracks the state of screen resizing and reloading
    /// </summary>
    private enum LoopState
    {
        Reset,
        End,
        Display
    }
}