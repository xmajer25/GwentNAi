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
 *  swapping cards at turn start (thinking of adding it to action list)
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

        private static int roundStart;

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
            ConsolePrint.UpdateBoard(board);

            //game
            while (board.Leader1.victories != 2 && board.Leader2.victories != 2)
            {
                roundStart = 2;
                while (board.Leader1.hasPassed == false || board.Leader2.hasPassed == false)
                {
                    if(roundStart != 0)
                    {
                        board.SwapCards();
                        roundStart--;
                    }
                    else
                    {
                        foreach(var row in board.CurrentPlayerActions.ImidiateActions) row.Clear();
                       
                    }

                    board.CurrentPlayerActions.GetAllActions(board.CurrentPlayerBoard, board.CurrentlyPlayingLeader.handDeck, board.CurrentlyPlayingLeader);
                    int moveOutcome = 0;
                    //move
                    while (moveOutcome == 0)
                    {
                        moveOutcome = (board.CurrentlyPlayingLeader == board.Leader1 ? player1Move(board) : player2Move(board));
                        board.CurrentPlayerActions.GetAllActions(board.CurrentPlayerBoard, board.CurrentlyPlayingLeader.handDeck, board.CurrentlyPlayingLeader);
                        board.MoveUpdate();
                        ConsolePrint.UpdateBoard(board);
                    }
                    board.Update();
                }
                DetermineRoundWinner();
                board.ResetBoard();
                board.DrawBothHands(3);
                ConsolePrint.UpdateBoard(board);
            }
            DetermineGameWinner();
            Console.ReadLine();
        }

        static private void DetermineGameWinner()
        {
            if(board.Leader1.victories == board.Leader2.victories)
            {
                Drawings.DrawTie();
            }
            else if(board.Leader1.victories >= board.Leader2.victories)
            {
                Drawings.DrawVictory(0);
                Drawings.DrawDefeat(1);
            }
            else
            {
                Drawings.DrawDefeat(0);
                Drawings.DrawVictory(1);
            }
        }

        static private void DetermineRoundWinner()
        {
            if (board.PointSumP1 == board.PointSumP2)
            {
                board.Leader1.victories++;
                board.Leader2.victories++;
            }
            else if (board.PointSumP1 > board.PointSumP2)
            {
                board.Leader1.victories++;
            }
            else
            {
                board.Leader2.victories++;
            }
            Drawings.DrawCrown(board);
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

            ConsolePrint.AskForLeaderAbility(2);
            DefaultLeader leader2 = StringToPlayerConvertor.Convert(Console.ReadLine());
            ConsolePrint.DrawLeader(1, leader2.leaderFaction);
            ConsolePrint.ClearBottom();

            return (leader1, leader2: leader2);
        }

        static private void DeckSetting()
        {
            ConsolePrint.AskForDeck(1);
            board.Leader1.startingDeck = StringToDeckConvertor.Convert(Console.ReadLine());
            ConsolePrint.ClearBottom();

            ConsolePrint.AskForDeck(2);
            board.Leader2.startingDeck = StringToDeckConvertor.Convert(Console.ReadLine());
            ConsolePrint.ClearBottom();
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
