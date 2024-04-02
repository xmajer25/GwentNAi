using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Protofleder : DefaultCard, IDeploy, IDeployExpandPickEnemies
    {
        public Protofleder()
        {
            CurrentValue = 4;
            MaxValue = 4;
            Shield = 0;
            Border = 1;
            Type = "unit";
            Faction = "monster";
            Name = "Protofleder";
            ShortName = "Protofl.";
            Descriptors = new List<string>() { "Vampire" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            List<List<int>> enemieIndexes = new List<List<int>>(2) { new List<int>(10), new List<int>(10) };
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();

            for (int i = 0; i < enemieBoard.Count; i++)
            {
                for (int cardIndex = 0; cardIndex < enemieBoard[i].Count; cardIndex++)
                {
                    board.CurrentPlayerActions.ImidiateActions[0][i].Add(cardIndex);
                }
            }
        }

        public void postPickEnemieAbilitiy(GameBoard board, int row, int index)
        {
            List<List<DefaultCard>> enemieBoard = board.GetEnemieBoard();
            enemieBoard[row][index].Bleeding += 3;

            CurrentValue += enemieBoard[row][index].Bleeding;
            MaxValue = MaxValue - CurrentValue + enemieBoard[row][index].Bleeding;
        }
    }
}
