using System;
using static System.Int32;
namespace ToyRobot
{
    internal class Program
    {
        public static int XAxis = 0;
        public static int YAxis = 0;
        public static Directions CurrentDirection;
        public static bool IsStart = true;


        private static void Main()
        {
            Start();
        }

        private static void Start()
        {
            /* This loop is a recursive call, if user wants to keep playing */
            while (true)
            {
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine("                             Toy Robot                          ");
                Console.WriteLine("                    Enter 'STOP' to stop playing                            ");
                Console.WriteLine("-------------------------------------------------------------------------------");

                ReadCommand();
                ShowFinalReport();

                Console.Write("Do you wish to play again? Y/N : ");
                var check = ReadInput();
                if (check == "Y")
                {
                    XAxis = 0;
                    YAxis = 0;
                    CurrentDirection = Directions.North;
                    IsStart = true;
                    continue;
                }
                break;
            }

        }
        /// <summary>
        /// This method will read and execute the command
        /// </summary>
        private static void ReadCommand()
        {
            var command = "";
            if (IsStart)
            {
                while (command != Constants.PlaceCommand) // Check "Place" command to begin with.
                {
                    Console.Write("Please enter 'PLACE' to start                            : ");
                    command = ReadInput();
                }
            }
            else
            {
                Console.Write("Enter Command (MOVE/LEFT/RIGHT/REPORT/PLACE)             : ");
                command = ReadInput();
            }

            while (command != Constants.StopCommand) // Check and execute commands until report. 
            {
                if (command is not Constants.PlaceCommand && command is not Constants.LeftCommand && command is not Constants.RightCommand
                    && command is not Constants.MoveCommand && command is not Constants.ReportCommand)
                {
                    Console.WriteLine("**Invalid command");
                }
                switch (command)
                {
                    case Constants.PlaceCommand:
                        SetPlace();
                        break;
                    case Constants.LeftCommand or Constants.RightCommand:
                        SetDirection(command);
                        break;
                    case Constants.MoveCommand:
                        Move();
                        break;
                    case Constants.ReportCommand:
                        ShowReport();
                        break;

                }
                Console.Write("Enter Command (MOVE/LEFT/RIGHT/REPORT/PLACE)             : ");
                command = ReadInput();
            }


        }
        /// <summary>
        /// This method will set the place of Robot
        /// this will set X axis, Y axis and direction
        /// </summary>
        private static void SetPlace()
        {
            Console.Write("Enter position X                                         : ");

            XAxis = ValidatePosition("X");

            Console.Write("Enter position Y                                         : ");

            YAxis = ValidatePosition("Y");


            Console.Write("Enter Direction (NORTH/SOUTH/EAST/WEST)                  : ");
            var inputDirection = ReadInput();

            var directionName = !string.IsNullOrEmpty(inputDirection) ? inputDirection : CurrentDirection.ToName();

            if (!Enum.TryParse<Directions>(directionName, true, out var direction) || int.TryParse(directionName, out _))
            {
                direction = Directions.North;
                Console.WriteLine("**Invalid input, The default direction is set to {0} ", direction.ToName());
            }

            CurrentDirection = direction;

            if (string.IsNullOrEmpty(inputDirection) && IsStart)
            {
                Console.WriteLine("The default direction is set to {0} ", CurrentDirection.ToName());
            }

            Console.WriteLine("You have set                                             : PLACE {0}, {1}, {2}", XAxis, YAxis, CurrentDirection.ToName());
            IsStart = false;
        }

        /// <summary>
        /// Validate and return the X and Y axis positions
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Position of X and Y axis</returns>
        private static int ValidatePosition(string position)
        {
            if (!TryParse(ReadInput(), out var input))
            {
                input = 0;
                Console.WriteLine("**Invalid Input. The default {0} axis set to 0 ", position);
            }
            if (input is < Constants.Minimum or > Constants.Maximum)
            {
                Console.WriteLine("**Invalid Input. Number must be between 0 and 5. The default {0} axis set to 0 ", position);
            }

            return input;
        }

        /// <summary>
        /// Execute move command
        /// </summary>
        private static void Move()
        {
            switch (CurrentDirection)
            {
                case Directions.North:
                    YAxis = GetNextPosition(YAxis, Moves.Forward);
                    break;
                case Directions.East:
                    XAxis = GetNextPosition(XAxis, Moves.Forward);
                    break;
                case Directions.South:
                    YAxis = GetNextPosition(YAxis, Moves.Backward);
                    break;
                case Directions.West:
                    XAxis = GetNextPosition(XAxis, Moves.Backward);
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// Set the next position of Robot
        /// Validate Robot's position stays inside the table
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <param name="nextStep"></param>
        /// <returns>Returns next position</returns>
        private static int GetNextPosition(int currentPosition, Moves nextStep)
        {
            var nextPosition = 0;
            switch (nextStep)
            {
                case Moves.Forward:
                    nextPosition = (currentPosition < Constants.Maximum ? currentPosition + 1 : currentPosition);
                    break;
                case Moves.Backward:
                    nextPosition = (currentPosition > Constants.Minimum ? currentPosition - 1 : currentPosition);
                    break;
                default:
                    break;
            }
            return nextPosition;
        }
        /// <summary>
        /// Set the direction of Robot after left and right command
        /// </summary>
        /// <param name="turn"></param>
        private static void SetDirection(string turn)
        {
            switch (turn)
            {
                case Constants.LeftCommand when CurrentDirection != Directions.North:
                    CurrentDirection--;
                    break;
                case Constants.LeftCommand:
                    CurrentDirection = Directions.West;
                    break;
                case Constants.RightCommand when CurrentDirection != Directions.West:
                    CurrentDirection++;
                    break;
                case Constants.RightCommand:
                    CurrentDirection = Directions.North;
                    break;

            }
        }

        /// <summary>
        /// Show Report
        /// </summary>
        private static void ShowReport()
        {
            Console.WriteLine("");
            Console.WriteLine("The Robot is here                                        : X-{0}, Y-{1}, {2}", XAxis, YAxis, CurrentDirection.ToName());
            Console.WriteLine("");

        }

        /// <summary>
        /// Show Final Report
        /// </summary>
        private static void ShowFinalReport()
        {

            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("The Final postion                                        : X-{0}, Y-{1}, {2}", XAxis, YAxis, CurrentDirection.ToName());
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("");
        }

        /// <summary>
        /// Common method to read input/command
        /// </summary>
        /// <returns>Upper case of input </returns>

        private static string ReadInput()
        {
            var input = Console.ReadLine();
            return !string.IsNullOrEmpty(input) ? input.ToUpper() : input;
        }
    }
}
