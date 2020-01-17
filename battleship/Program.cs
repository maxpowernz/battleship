using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShip
{

    public class Ship
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public string ShipName { get; set; }
    }

    public class BattleGround
    {
        // Properties
        const int maxShips = 2;
        static readonly int maxTrys = 5;
        const int maxCols = 8;
        const int maxRows = 8;
        static readonly bool debug = true;
        static Ship sunkenShip;
        static readonly List<Ship> Ships = new List<Ship>();

        static void StartGame()
        {
            Console.ResetColor();
            Ships.Clear();
            PlaceShips();

            if (debug)
            {
                ShowShipLocation();
            }

            MakeGuess(0);
        }

        static void MakeGuess(int tryNum)
        {

            if (tryNum == maxTrys)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Game Over :(");

                Console.ResetColor();
                Console.WriteLine("Play again? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    StartGame();
                }
                Environment.Exit(-1);
                return;
            }

            Console.WriteLine($"Turns left {maxTrys - tryNum}, Number of ships left: {Ships.Count()}");

            Console.Write("Enter Row: ");
            int inputRow = int.Parse(Console.ReadLine());

            Console.Write("Enter column: ");
            int inputCol = int.Parse(Console.ReadLine());

            if (FindClosest(inputRow, inputCol))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Beep();
                Console.WriteLine($"You sunk my battleship: {sunkenShip.ShipName}");
                tryNum--;
                Console.WriteLine();
            }

            if (Ships.Count == 0)
            {
                Console.WriteLine("You win!");
                Console.ResetColor();
                Console.WriteLine("Play again? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    StartGame();
                }
                Environment.Exit(-1);
                return;
            }

            else
            {
                Console.ResetColor();
                MakeGuess(tryNum + 1);
            }
        }


        static bool FindClosest(int inputRow, int inputCol)
        {

            int closestMatch = maxRows * maxCols;
            Ship matchedShip = new Ship();

            //check ship is a match and return if true
            foreach (var ship in Ships)
            {
                if (ship.Row == inputRow && ship.Col == inputCol)
                {
                    Ships.Remove(ship);
                    sunkenShip = ship;
                    return true;
                }
            }

            //find the closest match
            foreach (var ship in Ships)
            {
                int proximity = Math.Abs(ship.Row - inputRow) + Math.Abs(ship.Col - inputCol);

                if (proximity <= closestMatch)
                {
                    closestMatch = proximity;
                    matchedShip = ship;
                }

            }

            if (closestMatch <= 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Your hot");

            }
            else if (closestMatch <= 3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Your warm");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Your cold");
            }

            if (debug)
            {
                Console.ResetColor();
                Console.WriteLine("(set debug to false to hide this)");
                Console.WriteLine($"closest ship: {matchedShip.ShipName}");
                Console.WriteLine("proximity: " + closestMatch);
            }

            Console.WriteLine();
            return false;

        }


        static void PlaceShips()
        {
            Random random = new Random();

            for (int i = 1; i <= maxShips; i++)
            {
                int randomRow = random.Next(1, maxRows + 1);
                int randomCol = random.Next(1, maxCols + 1);

                //check thers no ship already in there
                if (Ships.Where(s => s.Row == randomRow && s.Col == randomCol).Count() == 0)
                {
                    Ships.Add(new Ship() { Row = randomRow, Col = randomCol, ShipName = "Ship: " + i });
                }
                else
                {
                    //negate counter so we get another ship
                    i--;
                }
            }
        }

        static void ShowShipLocation()
        {
            Console.WriteLine("Ship locations (set debug to false to hide this)");
            Ships.ForEach(ship => Console.WriteLine($"{ship.Row} {ship.Col} {ship.ShipName}"));
        }

        static void Main(string[] args)
        {

            StartGame();
            Console.ResetColor();
            Console.WriteLine("Press enter to exit");
            Console.Read();
        }
    }
}

