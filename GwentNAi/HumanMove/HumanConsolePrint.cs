using GwentNAi.GameSource.AssistantClasses;
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using System;
using System.Data;
using System.Diagnostics;

namespace GwentNAi.HumanMove
{
    public static class HumanConsolePrint
    {
        static public readonly int windowHeight = 50;
        static public readonly int windowWidth = 200;
        static public readonly int windowSeparator = 37;

        static public ConsoleColor currentColor = ConsoleColor.White;

        public static void swapColor(GameBoard board)
        {
            currentColor = (board.CurrentlyPlayingLeader == board.Leader1 ? ConsoleColor.DarkMagenta : ConsoleColor.DarkCyan);
        }

        public static int[] ListActions(ActionContainer currentPlayerActions)
        {
            Console.ForegroundColor = currentColor;

            int PlayCardActionId = 0;

            Console.SetCursorPosition(0, windowSeparator + 1);
            Console.Write("Possible play card actions (px):");
            Console.SetCursorPosition(0, windowSeparator + 2);

            foreach (var action in currentPlayerActions.PlayCardActions)
            {
                PlayCardActionId++;
                Console.Write("\t" + PlayCardActionId + ".) Play " + action.CardName);
                if (PlayCardActionId == 5) Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            }

            int OrderActionId = 0;

            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            Console.Write("Possible order actions (ox):");
            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);

            foreach (var action in currentPlayerActions.OrderActions)
            {
                OrderActionId++;
                Console.Write("\t" + OrderActionId + ".) Order " + action.CardName);

                if (OrderActionId % 5 == 0) Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            }

            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            Console.Write("Possible leader ability actions (l):");
            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            Console.Write("\t1.)" + currentPlayerActions.LeaderActions.CardName);

            if (currentPlayerActions.canPass)
            {
                Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                Console.Write("\tx.)pass");
            }
            else if (currentPlayerActions.canEnd)
            {
                Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                Console.Write("\tx.)end");
            }

            Console.ForegroundColor = ConsoleColor.White;
            return new int[] { PlayCardActionId, OrderActionId };
        }

        public static void ListPositionsForCard(List<List<DefaultCard>> board, List<List<int>> playIndexes)
        {
            Console.ForegroundColor = currentColor;
            int currentRow = 0;
            Console.SetCursorPosition(0, windowSeparator +  1);
            Console.Write("Enter position (row-pos):");
            foreach (var row in playIndexes)
            {
                if(row.Count == 1)
                {
                    Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                    Console.Write("Row " + currentRow + ":\n\t0\n");
                    currentRow++;
                    continue;
                }
                Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                Console.Write("Row " + currentRow + ":\n\t");
                foreach (var playIndex in row)
                {
                    if (playIndex == row.Last()) break;
                    Console.Write(playIndex + " - " + board[currentRow][playIndex].name + " - ");
                }
                if (row.Count != 0)
                {
                    Console.Write(row.Last());
                    Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                }
                currentRow++;
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ListEnemieExpand(List<List<int>> enemieIndexes)
        {
            Console.ForegroundColor = currentColor;

            Console.SetCursorPosition(0, windowSeparator + 1);
            Console.Write("Pick target (row-column):");
            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);

            for (int i = 0; i < enemieIndexes.Count; i++)
            {
                Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                Console.Write("Row " + i + ":");
                Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                if (enemieIndexes[i].Count == 0) continue;

                for(int j = 0; j < enemieIndexes[i].Count; j++)
                {
                    Console.Write("\t" + enemieIndexes[i][j]);
                }

            }

            Console.ForegroundColor= ConsoleColor.White;
        }
    }
}
