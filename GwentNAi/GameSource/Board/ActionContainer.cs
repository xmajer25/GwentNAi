using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Decks;
using GwentNAi.GameSource.Player;
using System.Reflection;

namespace GwentNAi.GameSource.Board
{
    public class ActionContainer : ICloneable
    {
        public List<PossibleAction> OrderActions = new();
        public List<PossibleAction> PlayCardActions = new();
        public List<List<List<int>>> ImidiateActions { get; set; } = new List<List<List<int>>>(2)
        {
            new List<List<int>>(2) { new List<int>(10), new List<int>(10) },
            new List<List<int>>(2) { new List<int>(10), new List<int>(10) }
        };
        public Action<GameBoard>? LeaderActions = null;
        public Action? PassOrEndTurn { get; set; } = () => { };
        public SwapCards CardSwaps = new();

        public bool CanPass { get; set; }
        public bool CanEnd { get; set; }

        public object Clone()
        {
            ActionContainer clonedActionContainer = new ActionContainer()
            {
                PlayCardActions = PlayCardActions.Select(action => (PossibleAction)action.Clone()).ToList(),
                OrderActions = OrderActions.Select(action => (PossibleAction)action.Clone()).ToList(),
                CardSwaps = (SwapCards)CardSwaps.Clone(),
                CanPass = CanPass,
                CanEnd = CanEnd,
            };

            clonedActionContainer.ImidiateActions = ImidiateActions
            .Select(outerList => outerList
                .Select(innerList => innerList.Select(item => item).ToList()) // Deep clone of inner list
                .ToList())
            .ToList();

            if (!AreImidiateActionsEqual(ImidiateActions, clonedActionContainer.ImidiateActions))
            {
                throw new InvalidOperationException("ImidiateActions are not equal after cloning.");
            }

            return clonedActionContainer;
        }

        private static bool AreImidiateActionsEqual(List<List<List<int>>> list1, List<List<List<int>>> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                var outerList1 = list1[i];
                var outerList2 = list2[i];

                if (outerList1.Count != outerList2.Count)
                {
                    return false;
                }

                for (int j = 0; j < outerList1.Count; j++)
                {
                    var innerList1 = outerList1[j];
                    var innerList2 = outerList2[j];

                    if (!innerList1.SequenceEqual(innerList2))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public void ClearImidiateActions()
        {
            foreach (var player in ImidiateActions)
            {
                foreach (var row in player)
                {
                    row.Clear();
                }
            }
            if (AreImidiateActionsFull()) throw new Exception("Inner Error: bad clear");
        }

        public bool AreImidiateActionsFull()
        {
            if (ImidiateActions[0][0].Count > 0) return true;
            if (ImidiateActions[0][1].Count > 0) return true;
            if (ImidiateActions[1][0].Count > 0) return true;
            if (ImidiateActions[1][1].Count > 0) return true;
            return false;
        }


        public void GetAllActions(List<List<DefaultCard>> CurrentPlayerBoard, DefaultDeck CurrentPlayerHand, DefaultLeader Leader)
        {
            OrderActions.Clear();
            PlayCardActions.Clear();

            GetOrderActions(CurrentPlayerBoard);
            if (!Leader.HasPlayedCard) GetPlayActions(CurrentPlayerHand);
            GetLeaderAction(Leader);
            GetPassOrEndTurn(Leader);
        }

        public void GetPassOrEndTurn(DefaultLeader Leader)
        {
            if (Leader.HasPlayedCard == false && Leader.HasUsedAbility == false)
            {
                PassOrEndTurn = Leader.Pass;
                CanPass = true;
                CanEnd = false;
            }
            else if (Leader.HasPlayedCard == true)
            {
                PassOrEndTurn = DefaultLeader.EndTurn;
                CanPass = false;
                CanEnd = true;
            }
            else
            {
                PassOrEndTurn = null;
                CanPass = false;
                CanEnd = false;
            }
        }

        private void GetOrderActions(List<List<DefaultCard>> CurrentPlayerBoard)
        {
            foreach (var row in CurrentPlayerBoard)
            {
                foreach (var card in row)
                {
                    if (card is IOrder && card.TimeToOrder == 0)
                    {
                        if (card is ICharge)
                        {
                            Type type = card.GetType();
                            FieldInfo chargesField = type.GetField("charge", BindingFlags.Instance | BindingFlags.Public);
                            if (chargesField != null)
                            {
                                int charges = (int)chargesField.GetValue(card);
                                if (charges <= 0) continue;
                            }
                        }
                        OrderActions.Add(new PossibleAction() { ActionCard = card, CardName = card.Name });
                    }
                }
            }
        }

        public void GetPlayActions(DefaultDeck CurrentPlayerHand)
        {
            PlayCardActions.Clear();
            for (int i = 0; i < CurrentPlayerHand.Cards.Count; i++)
            {
                DefaultCard card = CurrentPlayerHand.Cards[i];
                PlayCardActions.Add(new PossibleAction() { ActionCard = card, CardName = card.Name });
            }

            for (int i = 0; i < PlayCardActions.Count; i++)
            {
                if (PlayCardActions[i].CardName != CurrentPlayerHand.Cards[i].Name) throw new Exception("Inner error: bad play actions");
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
