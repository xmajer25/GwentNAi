using GwentNAi.GameSource.AssistantClasses;
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Decks;
using GwentNAi.GameSource.Player;


namespace GwentNAi.GameSource
{
    public class Program
    {
        private static GameBoard board = new();
        private static Func<GameBoard, int> player1Move = (GameBoard board) => 0;
        private static Func<GameBoard, int> player2Move = (GameBoard board) => 0;


        static void Main(string[] args)
        {
            Logging.ClearFile();
            ConsolePrint.Init();
            ConsolePrint.DrawStaticElements();
            (board.Leader1, board.Leader2) = LeaderSetting();

            ArgumentHandler.HandleArguments(args, ref player1Move, ref player2Move, board.Leader1, board.Leader2);
            
            
            DeckSetting();
            board.CurrentlyPlayingLeader = board.Leader2;
            board.CurrentPlayerBoard = board.Leader2.Board;
            //DetermineStartingPlayer();
            //board.ShufflerBothDecks();
            board.DrawBothHands(10);
            ConsolePrint.UpdateBoard(board);


            //game
            while (board.Leader1.Victories != 2 && board.Leader2.Victories != 2)
            {
                board.CurrentPlayerActions.CardSwaps.PlayersToSwap = 2;
                while (board.Leader1.HasPassed == false || board.Leader2.HasPassed == false)
                {
                    if (board.CurrentPlayerActions.CardSwaps.PlayersToSwap != 0) board.GetSwapCards();



                    int moveOutcome = 0;
                    Logging.LogCards(board);
                    //move
                    while (moveOutcome == 0)
                    {
                        if (board.CurrentlyPlayingLeader.HasPassed) break;

                        board.CurrentPlayerActions.GetAllActions(board.GetCurrentBoard(), board.GetCurrentLeader().Hand, board.GetCurrentLeader());
                        moveOutcome = (board.GetCurrentLeader() == board.Leader1 ? player1Move(board) : player2Move(board));
                        board.MoveUpdate();
                        ConsolePrint.UpdateBoard(board);
                    }
                    board.TurnUpdate();
                    Logging.SeparateTurnLogs();
                    ConsolePrint.UpdateBoard(board);
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
            if (board.Leader1.Victories == board.Leader2.Victories)
            {
                Logging.LogVictory(0, board.PointSumP1, board.PointSumP2, "Game");
                Drawings.DrawTie();
            }
            else if (board.Leader1.Victories >= board.Leader2.Victories)
            {
                Logging.LogVictory(1, board.PointSumP1, board.PointSumP2, "Game");
                Drawings.DrawVictory(0);
                Drawings.DrawDefeat(1);
            }
            else
            {
                Logging.LogVictory(2, board.PointSumP1, board.PointSumP2, "Game");
                Drawings.DrawDefeat(0);
                Drawings.DrawVictory(1);
            }
        }

        static private void DetermineRoundWinner()
        {
            if (board.PointSumP1 == board.PointSumP2)
            {
                Logging.LogVictory(0, board.PointSumP1, board.PointSumP2, "Turn");
                board.Leader1.Victories++;
                board.Leader2.Victories++;
            }
            else if (board.PointSumP1 > board.PointSumP2)
            {
                Logging.LogVictory(1, board.PointSumP1, board.PointSumP2, "Turn");
                board.Leader1.Victories++;
            }
            else
            {
                Logging.LogVictory(2, board.PointSumP1, board.PointSumP2, "Turn");
                board.Leader2.Victories++;
            }
            Drawings.DrawCrown(board);
        }

        static private (DefaultLeader leader1, DefaultLeader leader2) LeaderSetting()
        {
            ConsolePrint.AskForLeaderAbility(1);
            DefaultLeader leader1 = StringToPlayerConvertor.Convert(Console.ReadLine());
            ConsolePrint.DrawLeader(0, leader1.LeaderFaction);
            ConsolePrint.ClearBottom();

            ConsolePrint.AskForLeaderAbility(2);
            DefaultLeader leader2 = StringToPlayerConvertor.Convert(Console.ReadLine());
            ConsolePrint.DrawLeader(1, leader2.LeaderFaction);
            ConsolePrint.ClearBottom();

            return (leader1, leader2);
        }

        static private void DeckSetting()
        {
            ConsolePrint.AskForDeck(1);
            board.Leader1.StartingDeck = StringToDeckConvertor.Convert(Console.ReadLine());
            ConsolePrint.ClearBottom();

            ConsolePrint.AskForDeck(2);
            board.Leader2.StartingDeck = StringToDeckConvertor.Convert(Console.ReadLine());
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
