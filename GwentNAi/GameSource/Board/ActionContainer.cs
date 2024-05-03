using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Decks;
using GwentNAi.GameSource.Player;
using System.Diagnostics;
using System.Reflection;

namespace GwentNAi.GameSource.Board
{
    /**
     * ActionContainer is a class for each board
     * contains possible actions for current player
     * methods for obtaining possible actions and getting information about them
     */
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

        /**
         * from Interface ICloneable
         * creates a deep clone of this object
         */
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

        /**
         * Returns true if two imidiate actions are the same
         */
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

        /*
         * Clears imidiate actions
         * only clears rows in the multi-dimensional list to keep the structure
         */
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

        /*
         * Returns true if there is at least one imidiate action
         */
        public bool AreImidiateActionsFull()
        {
            if (ImidiateActions[0][0].Count > 0) return true;
            if (ImidiateActions[0][1].Count > 0) return true;
            if (ImidiateActions[1][0].Count > 0) return true;
            if (ImidiateActions[1][1].Count > 0) return true;
            return false;
        }

        /*
         * Function for obtaining all of the possible action
         * This includes: orders, playing cards, leader action, passing or ending turn
         */
        public void GetAllActions(List<List<DefaultCard>> CurrentPlayerBoard, DefaultDeck CurrentPlayerHand, DefaultLeader Leader)
        {
            OrderActions.Clear();
            PlayCardActions.Clear();

            GetOrderActions(CurrentPlayerBoard);
            if (!Leader.HasPlayedCard) GetPlayActions(CurrentPlayerHand);
            GetLeaderAction(Leader);
            GetPassOrEndTurn(Leader);
        }

        /*
         * Gets reference to a method of passing or ending turn
         * Pass possible if leader has not played card nor used ability
         */
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

        /*
         * Searches board of current player to find all the cards
         * that can allow order
         */
        public void GetOrderActions(List<List<DefaultCard>> CurrentPlayerBoard)
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

        /*
         * Get options for playing cards
         */
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

        /*
         * Gets leader ability if charges are left
         * else null
         */
        public void GetLeaderAction(DefaultLeader Leader)
        {
            if (Leader.AbilityCharges != 0)
                LeaderActions = Leader.Order;
            else
                LeaderActions = null;
        }
    }
}
