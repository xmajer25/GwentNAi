using GwentNAi.GameSource.AssistantClasses;
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.Neutral;
using GwentNAi.GameSource.Decks;
using GwentNAi.GameSource.Player;
using GwentNAi.HumanMove;
using GwentNAi.MctsMove;

/***
 *-=-=-=-=-=-=-=-=-=-TODOOOOODTODODOD-=-=-=-=-=-=--=-=-=
 * typeof(MyType).GetInterfaces().Contains(typeof(IMyInterface))
 * -------------------------------------------------------------
 *  add leader action
 *  figure out how the fuck are we gonna do the expansion
 *      iam thinking like, add new list to action container that will have to be resolved
 *  big yikes - there are units that can be placed in enemye row --- will have to do something about that (no idea what..)
 *  do a function in Program to determine round winner after 2 players have passed, than a function to determine overall winner
 *      iam also thinking like one loop for whole game, one for round and one for turn
 * -------------------------------------------------------------
 * skibidi toilet :))))
 * -=-=-=-=-=-=-=-=-=TURUTUTUTUUUUUU-=-=-=-=-=-=-=-=-=-=
 */
namespace GwentNAi.GameSource
{
    public class Program
    {
        private static int player1 = 0;
        private static int player2 = 0;

        private static GameBoard board = new();
        private static Func<GameBoard, int> player1Move = (GameBoard board) => 0;
        private static Func<GameBoard, int> player2Move = (GameBoard board) => 0;

        static void Main(string[] args)
        {
            ArgumentHandler.ArgHandler(args, ref player1, ref player2);
            ConsolePrint.Init();
            ConsolePrint.DrawStaticElements();

            PlayMethodSetting(player1, player2);
            (board.Leader1, board.Leader2) = LeaderSetting();
            DeckSetting();
            DetermineStartingPlayer();
            board.ShufflerBothDecks();
            board.DrawBothHands(10);


            while (board.Leader1.victories != 2 && board.Leader2.victories != 2)
            {
                board.CurrentPlayerActions.GetAllActions(board.CurrentPlayerBoard, board.CurrentlyPlayingLeader.handDeck, board.CurrentlyPlayingLeader);
                int moveOutcome = 0;

                while(moveOutcome == 0)
                {
                    moveOutcome = (board.CurrentlyPlayingLeader == board.Leader1 ? player1Move(board) : player2Move(board));
                    board.CurrentPlayerActions.GetAllActions(board.CurrentPlayerBoard, board.CurrentlyPlayingLeader.handDeck, board.CurrentlyPlayingLeader);
                    board.MoveUpdate();
                    ConsolePrint.UpdateBoard(board);
                }

                board.Update();
            }
            Console.ReadLine();
        }

        static private void PlayMethodSetting(int player1Method, int player2Method)
        {
            if (player1 == 0) player1Move = MCTSPlayer.MCTSMove;
            if (player1 == 1) player1Move = HumanPlayer.HumanMove;

            if (player2 == 0) player2Move = MCTSPlayer.MCTSMove;
            if (player2 == 1) player2Move = HumanPlayer.HumanMove;
        }

        static private (DefaultLeader leader1, DefaultLeader leader2) LeaderSetting()
        {
            ConsolePrint.AskForLeaderAbility(1);
            DefaultLeader leader1 = StringToPlayerConvertor.Convert(Console.ReadLine());
            ConsolePrint.DrawLeader(0, leader1.leaderFaction);
            ConsolePrint.ClearBottom();
            //DefaultLeader leader1 = StringToPlayerConvertor.Convert("bloodscent");

            ConsolePrint.AskForLeaderAbility(2);
            DefaultLeader leader2 = StringToPlayerConvertor.Convert(Console.ReadLine());
            ConsolePrint.DrawLeader(1, leader2.leaderFaction);
            ConsolePrint.ClearBottom();
            //DefaultLeader leader2 = StringToPlayerConvertor.Convert("arachasswarm");

            return (leader1, leader2: leader2);
        }

        static private void DeckSetting()
        {
            ConsolePrint.AskForDeck(1);
            board.Leader1.startingDeck = StringToDeckConvertor.Convert(Console.ReadLine());
            ConsolePrint.ClearBottom();
            //board.Leader1.startingDeck = StringToDeckConvertor.Convert("renfri");

            ConsolePrint.AskForDeck(2);
            board.Leader2.startingDeck = StringToDeckConvertor.Convert(Console.ReadLine());
            ConsolePrint.ClearBottom();
            //board.Leader2.startingDeck = StringToDeckConvertor.Convert("renfri");
        }

        static private void DetermineStartingPlayer()
        {
            Random coinFlip = new Random();
            if (coinFlip.Next(2) == 0)
            {
                board.CurrentlyPlayingLeader = board.Leader1;
                board.CurrentPlayerBoard = board.Leader1.Board;
            }
            else
            {
                board.CurrentlyPlayingLeader = board.Leader2;
                board.CurrentPlayerBoard = board.Leader2.Board;
            }
        }
    }
}
