using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Ghoul : DefaultCard, IDeploy, IDeployExpandPickCard
    {
        public Ghoul()
        {
            CurrentValue = 1;
            MaxValue = 1;
            Shield = 0;
            Border = 0;
            Type = "unit";
            Faction = "monster";
            Name = "Ghoul";
            ShortName = "Ghoul";
            Descriptors = new List<string>() { "Necrophage" };
            TimeToOrder = 0;
            Bleeding = 0;
        }

        public void Deploy(GameBoard board)
        {
            if (!isMelee(board)) return;

            List<DefaultCard> graveYard = board.GetCurrentLeader().Graveyard.Cards;
            List<int> graveYardIndexes = new List<int>();

            for (int cardIndex = 0; cardIndex < graveYard.Count; cardIndex++)
            {
                DefaultCard card = graveYard[cardIndex];
                if (card.Type == "unit" && card.Border == 0)
                {
                    graveYardIndexes.Add(cardIndex);
                }
            }

            board.CurrentPlayerActions.ImidiateActions[0][0] = graveYardIndexes;
        }

        public void postPickCardAbility(GameBoard board, int cardIndex)
        {
            DefaultCard consumedCard = board.GetCurrentLeader().Graveyard.Cards[cardIndex];
            CurrentValue += consumedCard.MaxValue;
            MaxValue = MaxValue - CurrentValue + consumedCard.MaxValue;
            board.GetCurrentLeader().Graveyard.Cards.RemoveAt(cardIndex);
        }

        private bool isMelee(GameBoard board)
        {
            int meleeRow = (board.GetCurrentLeader() == board.Leader1 ? 1 : 0);
            int currentRow = board.GetCurrentBoard()[0].IndexOf((DefaultCard)this) == -1 ? 1 : 0;
            return currentRow == meleeRow;
        }
    }
}
