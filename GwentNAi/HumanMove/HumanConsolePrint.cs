using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using System;
using System.Data;

namespace GwentNAi.HumanMove
{
    public static class HumanConsolePrint
    {
        public static int[] ListActions(ActionContainer currentPlayerActions)
        {
            int PlayCardActionId = 0;
            Console.WriteLine("Possible play card actions:");
            foreach (var action in currentPlayerActions.PlayCardActions)
            {
                PlayCardActionId++;
                Console.Write("\t" + PlayCardActionId + ".) Play " + action.CardName);
                if (PlayCardActionId % 4 == 0) Console.Write("\n");
            }

            int OrderActionId = 0;
            Console.WriteLine("\nPossible order actions:");
            foreach (var action in currentPlayerActions.OrderActions)
            {
                OrderActionId++;
                Console.Write("\t" + OrderActionId + ".) Order " + action.CardName);
                if (OrderActionId % 4 == 0) Console.Write("\n");
            }

            Console.WriteLine("\nPossible leader ability actions:");
            Console.WriteLine("\t1.)" + currentPlayerActions.LeaderActions.CardName);

            if (currentPlayerActions.canPass) Console.WriteLine("\tpass");
            else if(currentPlayerActions.canEnd) Console.WriteLine("\tend");

            Console.Write("\n\n");
            return new int[] { PlayCardActionId, OrderActionId };
        }

        public static void ListPositionsForCard(List<List<DefaultCard>> board, List<List<int>> playIndexes)
        {
            int currentRow = 0;
            Console.WriteLine("Enter position (row-pos):");
            foreach (var row in playIndexes)
            {
                if(row.Count == 1)
                {
                    Console.Write("Row " + currentRow + ":\n\t0\n");
                    currentRow++;
                    continue;
                }
                Console.Write("Row " + currentRow + ":\n\t");
                foreach (var playIndex in row)
                {
                    if (playIndex == row.Last()) break;
                    Console.Write(playIndex + " - " + board[currentRow][playIndex].name + " - ");
                }
                Console.WriteLine(row.Last());
                currentRow++;
            }
        }

        public static void ListEnemieExpand(List<List<int>> enemieIndexes)
        {
            Console.WriteLine("Pick target (row-column):");
            for(int i = 0; i < enemieIndexes.Count; i++)
            {
                Console.WriteLine("Row 0:");
                if (enemieIndexes[i].Count == 0) break;

                for(int j = 0; j < enemieIndexes[i].Count; j++)
                {
                    Console.Write("\t" + enemieIndexes[i][j]);
                }
                Console.Write("\n");
            }
        }
    }
}
