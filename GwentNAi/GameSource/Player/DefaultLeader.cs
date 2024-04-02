
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Decks;

namespace GwentNAi.GameSource.Player
{
    public abstract class DefaultLeader : ICloneable
    {
        public int ProvisionValue { get; set; }
        public string LeaderName { get; set; } = string.Empty;
        public string LeaderFaction { get; set; } = string.Empty;
        public bool IsStarting { get; set; }
        public int Victories { get; set; }
        public int AbilityCharges { get; set; }
        public int PlayerMethod { get; set; } // example: MCTS (0), Human(1)...
        public bool HasPassed { get; set; }
        public bool HasPlayedCard { get; set; }
        public bool HasUsedAbility { get; set; }
        public DefaultDeck StartingDeck { get; set; } = new DefaultDeck();
        public DefaultDeck Hand { get; set; } = new DefaultDeck();
        public DefaultDeck Graveyard { get; set; } = new DefaultDeck();

        public List<List<DefaultCard>> Board { get; set; } = new List<List<DefaultCard>>(2) { new List<DefaultCard>(10), new List<DefaultCard>(10) };

        private static readonly Random Shuffler = new();

        public abstract object Clone();

        public abstract void Order(GameBoard board);

        //settings for mcts
        public int Simulations { get; set; }
        public int Iterations { get; set; }


        public void ShuffleStartingDeck()
        {
            int n = StartingDeck.Cards.Count;

            while (n > 1)
            {
                n--;
                int k = Shuffler.Next(n + 1);
                DefaultCard value = StartingDeck.Cards[k];
                StartingDeck.Cards[k] = StartingDeck.Cards[n];
                StartingDeck.Cards[n] = value;
            }
        }

        public void DrawCards(int numberOfCards)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                if (Hand.Cards.Count >= 10) break;
                if (StartingDeck.Cards.Count <= 0) break;

                DefaultCard drawnCard = StartingDeck.Cards.First();
                StartingDeck.Cards.RemoveAt(0);
                Hand.Cards.Add(drawnCard);
            }
        }

        public void SwapCards(int index)
        {
            if (StartingDeck.Cards.Count == 0) return;
            DefaultCard handCard = Hand.Cards[index];
            int swappedCardIndex = Shuffler.Next(0, StartingDeck.Cards.Count);
            DefaultCard deckCard = StartingDeck.Cards[swappedCardIndex];

            Hand.Cards[index] = deckCard;
            StartingDeck.Cards[swappedCardIndex] = handCard;
        }

        public void PlayCard(int CardInHandIndex, int RowIndex, int PosIndex, GameBoard board)
        {
            //Check if PosIndex is correct
            if (PosIndex > Board[RowIndex].Count) throw new Exception("Inner Error: index out of range");
            if (PosIndex != 0 && Board[RowIndex][PosIndex - 1] == null) throw new Exception("Inner Error: insert out of order");

            //Play card
            HasPlayedCard = true;
            DefaultCard card = Hand.Cards[CardInHandIndex];
            Board[RowIndex].Insert(PosIndex, card);

            //Check if card was placed correctly
            if (PosIndex != Board[RowIndex].IndexOf(card)) throw new Exception("Inner Error: Card placed in wrong position (PosIndex was too high)");

            //Remove from hand
            Hand.Cards.RemoveAt(CardInHandIndex);

            if (card is IDeploy DeployCard)
            {
                DeployCard.Deploy(board);
                board.RemoveDestroyedCards();
            }

            RespondToDeployedCard(board, card);
        }

        public void PlayCard(DefaultCard card, int RowIndex, int PosIndex, GameBoard board)
        {
            HasPlayedCard = true;
            Board[RowIndex].Insert(PosIndex, card);

            if (card is IDeploy DeployCard)
            {
                DeployCard.Deploy(board);
                board.RemoveDestroyedCards();
            }

            RespondToDeployedCard(board, card);
        }

        public void PlayCardByAbility(DefaultCard card, int RowIndex, int PosIndex, GameBoard board)
        {
            Board[RowIndex].Insert(PosIndex, card);

            if (card is IDeploy DeployCard)
            {
                DeployCard.Deploy(board);
                board.RemoveDestroyedCards();
            }

            RespondToDeployedCard(board, card);
        }

        private static void RespondToDeployedCard(GameBoard board, DefaultCard currentlyPlayedCard)
        {
            foreach (var row in board.GetCurrentBoard())
            {
                foreach (var card in row)
                {
                    if (card == currentlyPlayedCard) continue;
                    if (card is ICroneInteraction CroneInteractionCard && currentlyPlayedCard.Descriptors.Contains("Crone")) CroneInteractionCard.RespondToCrone();
                    if (card is IThrive ThriveCard) ThriveCard.Thrive(currentlyPlayedCard.CurrentValue);
                }
            }
            foreach (var card in board.GetCurrentLeader().Graveyard.Cards)
            {
                if (card == currentlyPlayedCard) continue;
                if (card is ICroneInteraction CroneInteractionCard && currentlyPlayedCard.Descriptors.Contains("Crone")) CroneInteractionCard.RespondToCrone();
            }
            foreach (var card in board.GetCurrentLeader().Hand.Cards)
            {
                if (card == currentlyPlayedCard) continue;
                if (card is ICroneInteraction CroneInteractionCard && currentlyPlayedCard.Descriptors.Contains("Crone")) CroneInteractionCard.RespondToCrone();
            }
            foreach (var card in board.GetCurrentLeader().StartingDeck.Cards)
            {
                if (card == currentlyPlayedCard) continue;
                if (card is ICroneInteraction CroneInteractionCard && currentlyPlayedCard.Descriptors.Contains("Crone")) CroneInteractionCard.RespondToCrone();
            }
        }

        public void Pass()
        {
            HasPassed = true;
        }

        public static void EndTurn()
        {

        }

        public void UseAbility()
        {
            HasUsedAbility = true;
        }

        public void PostAbilitySettings()
        {
            AbilityCharges--;
            UseAbility();
        }
    }
}
