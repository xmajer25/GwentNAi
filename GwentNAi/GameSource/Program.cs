using GwentNAi.GameSource.AssistantClasses;
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.Neutral;
using GwentNAi.GameSource.Decks;
using GwentNAi.GameSource.Player;
using GwentNAi.HumanMove;
using GwentNAi.MctsMove;


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

            PlayMethodSetting();
            (board.Leader1, board.Leader2) = LeaderSetting();
            DeckSetting();
            DetermineStartingPlayer();
            board.ShufflerBothDecks();
            board.DrawBothHands(10);
            ConsolePrint.UpdateBoard(board);

            //game
            while (board.Leader1.Victories != 2 && board.Leader2.Victories != 2)
            {
                board.CurrentPlayerActions.SwapCards.PlayersToSwap = 2;
                while (board.Leader1.HasPassed == false || board.Leader2.HasPassed == false)
                {
                    if (board.CurrentPlayerActions.SwapCards.PlayersToSwap != 0) board.SwapCards();
                    

                    
                    int moveOutcome = 0;
                    //move
                    while (moveOutcome == 0)
                    {
                        board.CurrentPlayerActions.GetAllActions(board.GetCurrentBoard(), board.GetCurrentLeader().HandDeck, board.GetCurrentLeader());
                        moveOutcome = (board.GetCurrentLeader() == board.Leader1 ? player1Move(board) : player2Move(board));
                        board.MoveUpdate();
                        ConsolePrint.UpdateBoard(board);
                    }
                    board.TurnUpdate();
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
            if(board.Leader1.Victories == board.Leader2.Victories)
            {
                Drawings.DrawTie();
            }
            else if(board.Leader1.Victories >= board.Leader2.Victories)
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
                board.Leader1.Victories++;
                board.Leader2.Victories++;
            }
            else if (board.PointSumP1 > board.PointSumP2)
            {
                board.Leader1.Victories++;
            }
            else
            {
                board.Leader2.Victories++;
            }
            Drawings.DrawCrown(board);
        }

        static private void PlayMethodSetting()
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
