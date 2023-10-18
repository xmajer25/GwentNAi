using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Decks;
using GwentNAi.GameSource.Player;

namespace GwentNAi.GameSource.Board
{
    public class ActionContainer
    {
        public List<PossibleAction> OrderActions = new();
        public List<PossibleAction> PlayCardActions = new();
        public List<List<int>> ImidiateActions = new(); //these are filled by card expansion in cards
        public PossibleAction LeaderActions = new();
        public Action PassOrEndTurn = () => { };

        public bool canPass { get; set; }
        public bool canEnd { get; set; }
        public bool canLeaderAbility { get; set; }

        public void GetAllActions(List<List<DefaultCard>> CurrentPlayerBoard, Deck CurrentPlayerHand, DefaultLeader Leader)
        {
            OrderActions.Clear();
            PlayCardActions.Clear();
            LeaderActions = new();

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
                //CurrentPlayerLeaderAction.CardName = Leader.leaderName;
                //CurrentPlayerLeaderAction.CardAction = Leader.Order;
                //CurrentPlayerLeaderAction.ActionName = "Leader Ability";
                LeaderActions.CardName = Leader.leaderName;
            }
        }
    }
}
