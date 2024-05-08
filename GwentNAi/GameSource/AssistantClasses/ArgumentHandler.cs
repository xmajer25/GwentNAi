using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Player;
using GwentNAi.HumanMove;
using GwentNAi.MctsMove;
using System.Reflection.Metadata.Ecma335;

namespace GwentNAi.GameSource
{
    public static class ArgumentHandler
    {
        /*
         * Returns true if there are at least two arguments
         */
        private static bool AtLeastTwoArgument(int argCount)
            => argCount > 1;
        /*
         * Handles program arguments 
         * sets simulations iterations for ai agent
         * sets methods to use for each type of player (ai or human)
        */
        public static void HandleArguments(string[] args, ref Func<GameBoard, int> p1Move, ref Func<GameBoard, int> p2Move, DefaultLeader p1, DefaultLeader p2)
        {
            int p2Args = 1;

            if (!AtLeastTwoArgument(args.Length)) PrintWrongArgs();
        
            if (args[0] == "-human")
            {
                p1Move = HumanPlayer.HumanMove;
            }
            else if (args[0] == "-mcts")
            {
                p1Move = MCTSPlayer.MCTSMove;
                try
                {
                    p1.Simulations = int.Parse(args[1]);
                    p1.Iterations = int.Parse(args[2]);
                }
                catch
                {
                    PrintWrongArgs();
                }
                
                p2Args = 3;
            }
            else
            {
                PrintWrongArgs();
            }

            if (args[p2Args] == "-human")
            {
                p2Move = HumanPlayer.HumanMove;
            }
            else if(args[p2Args] == "-mcts") 
            {
                p2Move = MCTSPlayer.MCTSMove;
                try
                {
                    p2.Simulations = int.Parse(args[p2Args + 1]);
                    p2.Iterations = int.Parse(args[p2Args + 2]);
                }
                catch
                {
                    PrintWrongArgs();
                }   
            }
            else
            {
                PrintWrongArgs();                
            }
        }

        /*
         * Prints a small help after incorrect arguments were used
         * Terminates program after 5 sec
         */
        static private void PrintWrongArgs()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Wrong program arguments used");
            Console.WriteLine("Expecting: -human or -mcts [simulations] [iterations] twice for each player");
            Console.WriteLine("Example: -human -mcts 1 100000");
            Console.WriteLine("\n\nClosing the application...");
            Timer timer = new Timer(CloseAppOnWrongArgs, null, 5000, Timeout.Infinite);
            Thread.Sleep(6000);
        }

        /*
         * Closes the app when incorrect arguments are written
         */
        static void CloseAppOnWrongArgs(object state)
        {
            Environment.Exit(0);
        }
    }
}
