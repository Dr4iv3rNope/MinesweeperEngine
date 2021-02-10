# MinesweeperEngine

## Example
```csharp
using System;
using MinesweeperEngine;

namespace MinesweeperEngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // printing debug messages into console
            Engine.Instance.OnLog += msg => Console.WriteLine(msg);

            // creating 10x10 field
            // with mine count 10
            Engine.Instance.NewGame(5, 8, 10);

            // main game loop
            while (Engine.Instance.State == EngineState.Playing)
            {
                // pretty printing game field
                for (int y = -1; y < Engine.Instance.Height; y++)
                {
                    for (int x = -1; x < Engine.Instance.Width; x++)
                    {
                        // if this is left top corner, then we fill it with spaces
                        if (x == -1 && y == -1)
                        {
                            Console.Write("  ");
                            continue;
                        }
                        else // otherwise printing field coords
                        {
                            if (x == -1) // printing y coords
                            {
                                Console.Write(y);
                                Console.Write(' ');
                                continue;
                            }
                            else if (y == -1) // printing x coords
                            {
                                Console.Write(x);
                                Console.Write(' ');
                                continue;
                            }
                        }

                        var field = Engine.Instance.GetField((uint)x, (uint)y);

                        // printing field
                        // if it's explored then it will be '.'
                        // if it's flagged then it will be 'X'
                        switch (field.Type)
                        {
                            case FieldType.Empty:
                            {
                                if (field.IsExplored)
                                    Console.Write('.');
                                else
                                    Console.Write(field.IsFlagged ? 'X' : '?');
                                break;
                            }

                            case FieldType.Indicator:
                            {
                                if (field.IsExplored)
                                    Console.Write((field as FieldIndicator).MineCount);
                                else
                                    Console.Write(field.IsFlagged ? 'X' : '?');
                                break;
                            }

                            case FieldType.Mine:
                            {
                                if (field.IsExplored)
                                    Console.Write('*');
                                else
                                    Console.Write(field.IsFlagged ? 'X' : '?');
                                break;
                            }
                        }

                        Console.Write(' ');
                    }

                    Console.WriteLine();
                }

                //
                // Getting X and Y coords for future actions
                //

                uint sx, sy;

                Console.Write("X: ");
                while (!uint.TryParse(Console.ReadLine(), out sx))
                {
                    Console.WriteLine("Failed to parse X coordinate!");
                }

                Console.Write("Y: ");
                while (!uint.TryParse(Console.ReadLine(), out sy))
                {
                    Console.WriteLine("Failed to parse Y coordinate!");
                }

                Console.Write("Enter action (df): ");
                // what we gonna do?
                switch (Console.ReadLine().Trim().ToLower())
                {
                    case "d": // defuse field
                    {
                        if (Engine.Instance.GetField(sx, sy).IsFlagged)
                        {
                            Console.Write("That field is flagged, are you sure you want to defuse it? (ync): ");

                            var defuse = false;

                            switch (Console.ReadLine().Trim().ToLower())
                            {
                                case "y":
                                {
                                    defuse = true;
                                    break;
                                }

                                case "c":
                                case "n":
                                {
                                    break;
                                }
                            }

                            if (!defuse)
                                break;
                        }

                        Engine.Instance.Defuse(sx, sy);
                        break;
                    }

                    case "f": // flag field
                    {
                        Engine.Instance.Flag(sx, sy);
                        break;
                    }

                    default:
                    {
                        Console.WriteLine("Unknown action");
                        break;
                    }
                }
            }
        }
    }
}
```
