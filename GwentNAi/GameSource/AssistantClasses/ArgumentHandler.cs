using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Player;
using GwentNAi.HumanMove;
using GwentNAi.MctsMove;

namespace GwentNAi.GameSource
{
    public static class ArgumentHandler
    {
        /**
        * Function : ArgumentHandler
        * ***************************
        * Handles program arguments and alters player1 and player2 values
        * 
        * args: program arguments used
        * player1: refers to who will be playing as p1
        * player2: refers to who will play as p2
        * (0:mcts 1:human)
        */
        public static void ArgHandler(string[] args, ref int player1, ref int player2)
        {
            if (args.Length < 0 || args.Length > 2)
            {
                PrintWrongArgs();
                return;
            }
            else if (args.Length == 0)
            {
                player1 = 0;
                player2 = 0;
                return;
            }
            else if (args.Length >= 1)
            {
                if (args[0] == "-h" || args[0] == "--help") { PrintHelp(); return; }
                else if (args[0] == "-mcts") player1 = 0;
                else if (args[0] == "-p" || args[0] == "--player") player1 = 1;
                else PrintWrongArgs();
            }
            if (args.Length == 2)
            {
                if (args[1] == "-mcts") player2 = 0;
                else if (args[1] == "-p" || args[1] == "--player") player2 = 1;
                else PrintWrongArgs();
            }
        }

        public static void HandleArguments(string[] args, ref Func<GameBoard, int> p1Move, ref Func<GameBoard, int> p2Move, DefaultLeader p1, DefaultLeader p2)
        {
            int p2Args = 1;
            if (args[0] == "-h")
            {
                PrintHelp();
                return;
            }

            if (args[0] == "-human")
            {
                p1Move = HumanPlayer.HumanMove;
            }
            else
            {
                p1Move = MCTSPlayer.MCTSMove;
                p1.Simulations = int.Parse(args[1]);
                p1.Iterations = int.Parse(args[2]);
                p2Args = 3;
            }

            if (args[p2Args] == "-human")
            {
                p2Move = HumanPlayer.HumanMove;
            }
            else
            {
                p2Move = MCTSPlayer.MCTSMove;
                p2.Simulations = int.Parse(args[p2Args + 1]);
                p2.Iterations = int.Parse(args[p2Args + 2]);
            }
        }

        public static void GetMctsSettings(int playerNum, string[] args, DefaultLeader mctsLeader)
        {
            if (playerNum == 0)
            {
                mctsLeader.Iterations = int.Parse(args[2]);
                mctsLeader.Simulations = int.Parse(args[3]);
            }
            if (playerNum == 1)
            {
                mctsLeader.Iterations = int.Parse(args[2]);
                mctsLeader.Simulations = int.Parse(args[3]);
            }
        }

        /**
         * Function: PrintHelp
         * ********************
         * Prints helper after -h / --help program argument was used
         */
        static private void PrintHelp()
        {
            Console.WriteLine("Gwent Console Application For AI Gameplay");
            Console.WriteLine("Options:\n\t-h / --help : prints helper for this program\n\t-p / --player : let's you play against the ai");
            Console.WriteLine("When no option is used two AIs play against each other and print the playout in console");
        }

        /**
         * Function: PrintWrongArgs
         * ************************
         * Prints little information when incorect program arguments where used 
         */
        static private void PrintWrongArgs()
        {
            Console.WriteLine("Wrong program arguments used");
            Console.WriteLine("For help run program with \"-h\" or \"--help\"");
        }
    }
}
