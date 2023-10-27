using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Decks;
using GwentNAi.GameSource.Player;

namespace GwentNAi.GameSource.Board
{
    public class ActionContainer
    {
        public List<PossibleAction> OrderActions = new();
        public List<PossibleAction> PlayCardActions = new();
        public List<List<List<int>>> ImidiateActions { get; set; } = new List<List<List<int>>>(2) {new List<List<int>>(2) { new List<int>(10), new List<int>(10) },
                                                                                     new List<List<int>>(2) { new List<int>(10), new List<int>(10) } };
        public Action<GameBoard>? LeaderActions = null;
        public Action PassOrEndTurn = () => { };

        public bool canPass { get; set; }
        public bool canEnd { get; set; }
        public bool canLeaderAbility { get; set; }

        public void ClearImidiateActions()
        {
            foreach(var player in ImidiateActions)
            {
                foreach(var row in player)
                {
                    row.Clear();
                }
            }
        }

        public bool AreImidiateActionsFull()
        {
            if (ImidiateActions[0][0].Count > 0) return true;
            if (ImidiateActions[0][1].Count > 0) return true;
            if (ImidiateActions[1][0].Count > 0) return true;
            if (ImidiateActions[1][1].Count > 0) return true;
            return false;
        }


        public void GetAllActions(List<List<DefaultCard>> CurrentPlayerBoard, Deck CurrentPlayerHand, DefaultLeader Leader)
        {
            OrderActions.Clear();
            PlayCardActions.Clear();

            GetOrderActions(CurrentPlayerBoard);
            GetPlayAction(CurrentPlayerHand);
            GetLeaderAction(Leader);
            GetPassOrEndTurn(Leader);
        }

        public void GetPassOrEndTurn(DefaultLeader Leader)
        {
            if(Leader.hasPlayedCard == false && Leader.hasUsedAbility == false) 
            {
                PassOrEndTurn = Leader.Pass;
                canPass = true;
                canEnd = false;
            }
            else if (Leader.hasPlayedCard == true)
            {
                PassOrEndTurn = Leader.EndTurn;
                canPass = false;
                canEnd = true;
            }
            else
            {
                canPass = false;
                canEnd = false;
            }
        }

        public void GetOrderActions(List<List<DefaultCard>> CurrentPlayerBoard)
        {
            foreach(var row in  CurrentPlayerBoard)
            {
                foreach (var card in row)
                {
                    if (card is IOrder && card.timeToOrder == 0)
                    {
                        OrderActions.Add(new PossibleAction() { ActionCard = card, CardName = card.name });
                    }
                }
            }
        }

        public void GetPlayAction(Deck CurrentPlayerHand)
        {
            foreach(var card in CurrentPlayerHand.Cards)
            {
                PlayCardActions.Add(new PossibleAction() { ActionCard = card, CardName = card.name });
            }
        }

        public void GetLeaderAction(DefaultLeader Leader)
        {
            if (Leader.abilityCharges != 0)
            {
                LeaderActions = Leader.Order;
            }
            else
            {
                LeaderActions = null;
            }
        }
    }
}
