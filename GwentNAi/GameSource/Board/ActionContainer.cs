using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Decks;
using GwentNAi.GameSource.Player;
using GwentNAi.MctsMove.Enums;
using System.Reflection;

namespace GwentNAi.GameSource.Board
{
    public class ActionContainer : ICloneable
    {
        public List<PossibleAction> OrderActions = new();
        public List<PossibleAction> PlayCardActions = new();
        public List<List<List<int>>> ImidiateActions { get; set; } = new List<List<List<int>>>(2) {new List<List<int>>(2) { new List<int>(10), new List<int>(10) },
                                                                                     new List<List<int>>(2) { new List<int>(10), new List<int>(10) } };
        public Action<GameBoard>? LeaderActions = null;
        public Action? PassOrEndTurn { get; set; } = () => { };
        public SwapCards SwapCards = new();

        public bool canPass { get; set; }
        public bool canEnd { get; set; }
        public bool canLeaderAbility { get; set; }

        public object Clone()
        {
            ActionContainer clonedActionContainer = new ActionContainer()
            {
                PlayCardActions = PlayCardActions.Select(action => (PossibleAction)action.Clone()).ToList(),
                SwapCards = (SwapCards)SwapCards.Clone(),
                canPass = canPass,
                canEnd = canEnd,
                canLeaderAbility = canLeaderAbility
            };

            return clonedActionContainer;
        }

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
            if(!Leader.HasPlayedCard) GetPlayAction(CurrentPlayerHand);
            GetLeaderAction(Leader);
            GetPassOrEndTurn(Leader);
        }

        public void GetPassOrEndTurn(DefaultLeader Leader)
        {
            if(Leader.HasPlayedCard == false && Leader.HasUsedAbility == false) 
            {
                PassOrEndTurn = Leader.Pass;
                canPass = true;
                canEnd = false;
            }
            else if (Leader.HasPlayedCard == true)
            {
                PassOrEndTurn = Leader.EndTurn;
                canPass = false;
                canEnd = true;
            }
            else
            {
                PassOrEndTurn = null;
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
                        if(card is ICharge)
                        {
                            Type type = card.GetType();
                            FieldInfo chargesField = type.GetField("charge", BindingFlags.Instance | BindingFlags.Public);
                            if(chargesField != null)
                            {
                                int charges = (int)chargesField.GetValue(card);
                                if (charges <= 0) continue;
                            }
                        }
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
            if (Leader.AbilityCharges != 0)
                LeaderActions = Leader.Order;
            else
                LeaderActions = null;
        }
    }
}
