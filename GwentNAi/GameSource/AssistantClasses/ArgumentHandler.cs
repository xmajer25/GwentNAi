using GwentNAi.GameSource.Player;

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
            if(args.Length == 2)
            {
                if (args[1] == "-mcts") player2 = 0;
                else if (args[1] == "-p" || args[1] == "--player") player2 = 1;
                else PrintWrongArgs();
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
