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
        public static void HandleArguments(string[] args, ref Func<GameBoard, int> p1Move, ref Func<GameBoard, int> p2Move, DefaultLeader p1, DefaultLeader p2)
        {
            int p2Args = 1;

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
