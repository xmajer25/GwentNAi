using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.AssistantClasses
{
    public static class Logging
    {
        private static readonly string fileName = "..\\GameLogs.txt";

        public static void ClearFile()
        {
            using (StreamWriter sw = new(fileName))
            {
            }
        }

        private static string GetSpacing(string firstWord)
            => new String(' ', 50 - firstWord.Length);

        public static void SeparateTurnLogs()
        {
            using (StreamWriter sw = new(fileName, true))
            {
                sw.WriteLine();
                sw.WriteLine(new String('*', 100));
                sw.WriteLine();
            }
        }

        public static void LogMove(GameBoard board, string move)
        {
            string leader = board.GetCurrentLeader() == board.Leader1 ? "Leader1" : "Leader2";
            using (StreamWriter sw = new(fileName, true))
            {
                sw.WriteLine(leader + " move: " + move);
            }
        }

        public static void LogVictory(int gameResult, int l1Points, int l2Points, string typeOfWin)
        {
            using (StreamWriter sw = new(fileName, true))
            {
                sw.WriteLine(new String('*', 100));
                switch (gameResult)
                {
                    case 0:
                        sw.WriteLine(typeOfWin + " Result: Tie");
                        break;
                    case 1:
                        sw.WriteLine(typeOfWin + " Result: Leader 1 Won");
                        break;
                    case 2:
                        sw.WriteLine(typeOfWin + " Result: Leader 2 Won");

                        break;
                }
                if (typeOfWin != "Game")
                {
                    sw.WriteLine("Leader 1 Points: " + l1Points);
                    sw.WriteLine("Leader 2 Points: " + l2Points);
                }
                
                sw.WriteLine(new String('*', 100));
            }
        }

        public static void LogCards(GameBoard board)
        {
            List<DefaultCard> L1StartingCards = board.Leader1.StartingDeck.Cards;
            List<DefaultCard> L2StartingCards = board.Leader2.StartingDeck.Cards;

            List<DefaultCard> L1HandCards = board.Leader1.Hand.Cards;
            List<DefaultCard> L2HandCards = board.Leader2.Hand.Cards;
            
            List<DefaultCard> L1GraveyardCards = board.Leader1.Graveyard.Cards;
            List<DefaultCard> L2GraveyardCards = board.Leader2.Graveyard.Cards;

            using (StreamWriter sw = new(fileName, true))
            {
                sw.WriteLine("Leader1:" + GetSpacing("Leader1:") + "Leader2:");

                sw.WriteLine("\tStartingDeck:" + GetSpacing("StartingDeck:") + "StartingDeck:");
                for (int i = 0; i < (L1StartingCards.Count < L2StartingCards.Count ? L2StartingCards.Count : L1StartingCards.Count); i++)
                {
                    string card1 = i < L1StartingCards.Count ? L1StartingCards[i].Name : "";
                    string card2 = i < L2StartingCards.Count ? L2StartingCards[i].Name : "";
                    sw.WriteLine("\t\t" + card1 + GetSpacing(card1) + card2);
                }

                sw.WriteLine("\tHand:" + GetSpacing("Hand:") + "Hand:");
                for (int i = 0; i < (L1HandCards.Count < L2HandCards.Count ? L2HandCards.Count : L1HandCards.Count); i++)
                {
                    string card1 = i < L1HandCards.Count ? L1HandCards[i].Name : "";
                    string card2 = i < L2HandCards.Count ? L2HandCards[i].Name : "";
                    sw.WriteLine("\t\t" + card1 + GetSpacing(card1) + card2);
                }

                sw.WriteLine("\tGraveyard:" + GetSpacing("Graveyard:") + "Graveyard:");
                for (int i = 0; i < (L1GraveyardCards.Count < L2GraveyardCards.Count ? L2GraveyardCards.Count : L1GraveyardCards.Count); i++)
                {
                    string card1 = i < L1GraveyardCards.Count ? L1GraveyardCards[i].Name : "";
                    string card2 = i < L2GraveyardCards.Count ? L2GraveyardCards[i].Name : "";
                    sw.WriteLine("\t\t" + card1 + GetSpacing(card1) + card2);
                }
            }
            LogGameBoard(board);
        }

        private static void LogGameBoard(GameBoard board)
        {
            using (StreamWriter sw = new(fileName, true))
            {
                sw.WriteLine();
                sw.WriteLine("\t\t--- Board ---");
                sw.WriteLine("Leader1:");
                foreach(var row in board.Leader1.Board)
                {
                    sw.Write('-');
                    foreach(var card in row)
                    {
                        sw.Write(card.Name + "(" + card.CurrentValue + ")");
                        sw.Write('-');
                    }
                    sw.WriteLine();
                }
                sw.WriteLine("Leader2:");
                foreach (var row in board.Leader2.Board)
                {
                    sw.Write('-');
                    foreach (var card in row)
                    {
                        sw.Write(card.Name + "(" + card.CurrentValue + ")");
                        sw.Write('-');
                    }
                    sw.WriteLine();
                }
                sw.WriteLine();
            }
        }
    }
}
