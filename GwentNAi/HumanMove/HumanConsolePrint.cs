using GwentNAi.GameSource.AssistantClasses;
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Decks;

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
            currentColor = (board.GetCurrentLeader() == board.Leader1 ? ConsoleColor.DarkMagenta : ConsoleColor.DarkCyan);
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
            if (currentPlayerActions.LeaderActions != null)
            {
                Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                Console.Write("\t1.) Leader ability");
            }

            if (currentPlayerActions.CanPass)
            {
                Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                Console.Write("\tx.)pass");
            }
            else if (currentPlayerActions.CanEnd)
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
            Console.SetCursorPosition(0, windowSeparator + 1);
            Console.Write("Enter position (row-pos):");
            foreach (var row in playIndexes)
            {
                if (row.Count == 1)
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
                    Console.Write(playIndex + " - " + board[currentRow][playIndex].Name + " - ");
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

                for (int j = 0; j < enemieIndexes[i].Count; j++)
                {
                    Console.Write("\t" + enemieIndexes[i][j]);
                }

            }
            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ListAllExpand(List<List<List<int>>> cardIndexes)
        {
            Console.ForegroundColor = currentColor;

            Console.SetCursorPosition(0, windowSeparator + 1);
            Console.Write("Pick target (player-row-column):");
            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            for (int i = 0; i < cardIndexes.Count; i++)
            {
                Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                Console.Write("Player " + i + ":");
                if (cardIndexes[i].Count == 0) continue;

                for (int j = 0; j < cardIndexes[i].Count; j++)
                {
                    Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                    Console.Write("\tRow" + j + ":");
                    Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                    Console.Write("\t");
                    for (int k = 0; k < cardIndexes[i][j].Count; k++)
                    {
                        Console.Write("\t" + cardIndexes[i][j][k]);
                    }
                }

            }
            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ListCards(List<int> cardIndexes, DefaultDeck hand, string msg)
        {
            Console.ForegroundColor = currentColor;
            Console.SetCursorPosition(0, windowSeparator + 1);
            Console.Write(msg);
            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);

            foreach (int index in cardIndexes)
            {
                Console.Write("\t" + index + ".)" + hand.Cards[index].Name);
                if (index == 5) Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            }
            Console.Write("\tx.) end");
            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void ListCardsSimple(List<int> cardIndexes)
        {
            Console.ForegroundColor = currentColor;
            Console.SetCursorPosition(0, windowSeparator + 1);
            Console.Write("Pick card :))");
            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);

            foreach (int index in cardIndexes)
            {
                Console.Write("\t" + index + ".)");
                if (index == 5) Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            }
            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
