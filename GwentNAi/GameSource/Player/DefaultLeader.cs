
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;
using GwentNAi.GameSource.Decks;

namespace GwentNAi.GameSource.Player
{
    public abstract class DefaultLeader
    {
        public int provisionValue { get; set; }
        public string leaderName { get; set; } = string.Empty;
        public string leaderFaction { get; set; } = string.Empty;
        public bool isStarting { get; set; }
        public int victories { get; set; }
        public int abilityCharges { get; set; }
        public int playerMethod { get; set; } // example: MCTS (0), Human(1)...
        public bool hasPassed { get; set; }
        public bool hasPlayedCard { get; set; }
        public bool hasUsedAbility { get; set; }
        public Deck startingDeck { get; set; } = new Deck();
        public Deck handDeck { get; set; } = new Deck();
        public Deck graveyardDeck { get; set; } = new Deck();

        public List<List<DefaultCard>> Board { get; set; } = new List<List<DefaultCard>>(2) { new List<DefaultCard>(10), new List<DefaultCard>(10) };

        private static Random Shuffler = new();


        public abstract void Order(GameBoard board);
        public abstract void Update();

        public void ShuffleStartingDeck()
        {
            int n = startingDeck.Cards.Count;

            while (n > 1)
            {
                n--;
                int k = Shuffler.Next(n + 1);
                DefaultCard value = startingDeck.Cards[k];
                startingDeck.Cards[k] = startingDeck.Cards[n];
                startingDeck.Cards[n] = value;
            }
        }

        public void DrawCards(int numberOfCards)
        {
            for(int i = 0;  i < numberOfCards; i++)
            {
                if (handDeck.Cards.Count >= 10) break;
                if (startingDeck.Cards.Count <= 0) break;

                DefaultCard drawnCard = startingDeck.Cards.First();
                startingDeck.Cards.RemoveAt(0);
                handDeck.Cards.Add(drawnCard);
            }
        }

        public void SwapCards(int index)
        {
            DefaultCard handCard = handDeck.Cards[index];
            int swappedCardIndex = Shuffler.Next(0, startingDeck.Cards.Count);
            DefaultCard deckCard = startingDeck.Cards[swappedCardIndex];

            handDeck.Cards[index] = deckCard;
            startingDeck.Cards[swappedCardIndex] = handCard;
        }

        public void PlayCard(int CardInHandIndex, int RowIndex, int PosIndex, GameBoard board)
        {
            hasPlayedCard = true;
            DefaultCard card = handDeck.Cards[CardInHandIndex];
            if (card is IDeploy)
            {
                card.Deploy((IDeploy)card, board);
            }
            if (card.descriptors.Contains("Crone"))
            {
                RespondToCrone(board, card);
            }
            Board[RowIndex].Insert(PosIndex, card);
            handDeck.Cards.RemoveAt(CardInHandIndex);
        }

        private void RespondToCrone(GameBoard board, DefaultCard currentlyPlayedCard)
        {
            foreach(var row in board.CurrentlyPlayingLeader.Board)
            {
                foreach(var card in row)
                {
                    if (card == currentlyPlayedCard) continue;
                    if (card is ICroneInteraction) card.RespondToCrone((ICroneInteraction)card);
                }
            }
            foreach (var card in board.CurrentlyPlayingLeader.graveyardDeck.Cards)
            {
                if (card == currentlyPlayedCard) continue;
                if (card is ICroneInteraction) card.RespondToCrone((ICroneInteraction)card);
            }
            foreach (var card in board.CurrentlyPlayingLeader.handDeck.Cards)
            {
                if (card == currentlyPlayedCard) continue;
                if (card is ICroneInteraction) card.RespondToCrone((ICroneInteraction)card);
            }
            foreach (var card in board.CurrentlyPlayingLeader.startingDeck.Cards)
            {
                if (card == currentlyPlayedCard) continue;
                if (card is ICroneInteraction) card.RespondToCrone((ICroneInteraction)card);
            }
        }

        public void Pass()
        {
            hasPassed = true;
        }

        public void EndTurn()
        {

        }

        public void UseAbility()
        {
            hasUsedAbility = true;
        }

        public void PostPlayCardOrder(IPlayCardExpand obj , GameBoard board, int row, int column)
        {
            obj.PostPlayCardOrder(board, row, column);
        }
    }
}
