
using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Cards.IExpand;
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
        public Deck StartingDeck { get; set; } = new Deck();
        public Deck HandDeck { get; set; } = new Deck();
        public Deck GraveyardDeck { get; set; } = new Deck();

        public List<List<DefaultCard>> Board { get; set; } = new List<List<DefaultCard>>(2) { new List<DefaultCard>(10), new List<DefaultCard>(10) };

        private static Random Shuffler = new();

        public abstract object Clone();

        public abstract void Order(GameBoard board);
        public abstract void Update();

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
            for(int i = 0;  i < numberOfCards; i++)
            {
                if (HandDeck.Cards.Count >= 10) break;
                if (StartingDeck.Cards.Count <= 0) break;

                DefaultCard drawnCard = StartingDeck.Cards.First();
                StartingDeck.Cards.RemoveAt(0);
                HandDeck.Cards.Add(drawnCard);
            }
        }

        public void SwapCards(int index)
        {
            DefaultCard handCard = HandDeck.Cards[index];
            int swappedCardIndex = Shuffler.Next(0, StartingDeck.Cards.Count);
            DefaultCard deckCard = StartingDeck.Cards[swappedCardIndex];

            HandDeck.Cards[index] = deckCard;
            StartingDeck.Cards[swappedCardIndex] = handCard;
        }

        public void PlayCard(int CardInHandIndex, int RowIndex, int PosIndex, GameBoard board)
        {
            HasPlayedCard = true;
            DefaultCard card = HandDeck.Cards[CardInHandIndex];
            Board[RowIndex].Insert(PosIndex, card);
            HandDeck.Cards.RemoveAt(CardInHandIndex);
            if (card is IDeploy DeployCard)
            {
                DeployCard.Deploy(board);
                board.RemoveDestroyedCards();
            }
                
            RespondToDeployedCard(board, card);
        }

        public void PlayCard(DefaultCard card, int RowIndex, int PosIndex, GameBoard board)
        {
            Board[RowIndex].Insert(PosIndex, card);
            if (card is IDeploy DeployCard)
            {
                DeployCard.Deploy(board);
            }
            RespondToDeployedCard(board, card);
        }

        private void RespondToDeployedCard(GameBoard board, DefaultCard currentlyPlayedCard)
        {
            foreach(var row in board.GetCurrentBoard())
            {
                foreach(var card in row)
                {
                    if (card == currentlyPlayedCard) continue;
                    if (card is ICroneInteraction CroneInteractionCard && currentlyPlayedCard.descriptors.Contains("Crone")) CroneInteractionCard.RespondToCrone();
                    if (card is IThrive ThriveCard) ThriveCard.Thrive(currentlyPlayedCard.currentValue);
                }
            }
            foreach (var card in board.GetCurrentLeader().GraveyardDeck.Cards)
            {
                if (card == currentlyPlayedCard) continue;
                if (card is ICroneInteraction CroneInteractionCard && currentlyPlayedCard.descriptors.Contains("Crone")) CroneInteractionCard.RespondToCrone();
            }
            foreach (var card in board.GetCurrentLeader().HandDeck.Cards)
            {
                if (card == currentlyPlayedCard) continue;
                if (card is ICroneInteraction CroneInteractionCard && currentlyPlayedCard.descriptors.Contains("Crone")) CroneInteractionCard.RespondToCrone();
            }
            foreach (var card in board.GetCurrentLeader().StartingDeck.Cards)
            {
                if (card == currentlyPlayedCard) continue;
                if (card is ICroneInteraction CroneInteractionCard && currentlyPlayedCard.descriptors.Contains("Crone")) CroneInteractionCard.RespondToCrone();
            }
        }

        public void Pass()
        {
            HasPassed = true;
        }

        public void EndTurn()
        {

        }

        public void UseAbility()
        {
            HasUsedAbility = true;
        }

        public void PostPlayCardOrder(IPlayCardExpand obj , GameBoard board, int row, int column)
        {
            obj.PostPlayCardOrder(board, row, column);
        }
    }
}
