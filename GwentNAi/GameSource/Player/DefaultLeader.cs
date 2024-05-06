
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Decks;

namespace GwentNAi.GameSource.Player
{
    /*
     * Parent class for all of the specific leader abilities
     * Contains methods general for all leaders
     * Methods for altering, and getting infromation from cards in hand...
     */
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

        /*
         * Overridable method for creating deep clones of leaders
         */
        public abstract object Clone();

        /*
         * Overridable method for using leader ability
         */
        public abstract void Order(GameBoard board);


        //settings for mcts
        public int Simulations { get; set; }
        public int Iterations { get; set; }


        /*
         * Shuffles cards in StartingDeck randomly
         */
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

        /*
         * Moves n Cards from Deck into hand
         */
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

        /*
         * Swap a card in hand with a random card from deck
         */
        public void SwapCards(int index)
        {
            if (StartingDeck.Cards.Count == 0) return;
            DefaultCard handCard = Hand.Cards[index];
            int swappedCardIndex = Shuffler.Next(0, StartingDeck.Cards.Count);
            DefaultCard deckCard = StartingDeck.Cards[swappedCardIndex];

            Hand.Cards[index] = deckCard;
            StartingDeck.Cards[swappedCardIndex] = handCard;
        }

        /*
         * Method for playing a card from hand on a board
         */
        public void PlayCard(int CardInHandIndex, int RowIndex, int PosIndex, GameBoard board)
        {
            //Play card
            HasPlayedCard = true;
            DefaultCard card = Hand.Cards[CardInHandIndex];
            Board[RowIndex].Insert(PosIndex, card);

            //Remove from hand
            Hand.Cards.RemoveAt(CardInHandIndex);

            //Trigger deploy ability of card
            if (card is IDeploy DeployCard)
            {
                DeployCard.Deploy(board);
                board.RemoveDestroyedCards();
            }

            RespondToDeployedCard(board, card);
        }

        /*
         * Method for playing a card with the card taken as argument
         */
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

        /*
         * Method for playing a card 
         * Special for the it doesn't count as leader playing a card
         * (could be from an ability or so..)
         */
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

        /*
         * Triggers all cards responding to us playing a card on the board
         */
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

        /*
         * Leader passed the turn
         */
        public void Pass()
        {
            HasPassed = true;
        }

        /*
         * Leader ended their turn
         * can not be null
         */
        public static void EndTurn()
        {

        }

        /*
         * Sets true after using an order or leader ability
         * -> can not pass if true
         */
        public void UseAbility()
        {
            HasUsedAbility = true;
        }

        /*
         * Changes information about leader after using leader ability
         */
        public void PostAbilitySettings()
        {
            AbilityCharges--;
            UseAbility();
        }
    }
}
