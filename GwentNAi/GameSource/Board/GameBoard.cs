using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.CustomExceptions;
using GwentNAi.GameSource.Player;
using System.Data;
using System.Reflection;

namespace GwentNAi.GameSource.Board
{
    /*
     * Classed used to represent the game board
     * References to both leaders and their parts of the board
     * Methods for updating this board and retrieving information from it
     */
    public class GameBoard : ICloneable
    {
        public DefaultLeader? Leader1 { get; set; }
        public DefaultLeader? Leader2 { get; set; }

        public int PointSumP2 { get; set; }
        public int PointSumP1 { get; set; }


        public DefaultLeader? CurrentlyPlayingLeader { get; set; }
        public List<List<DefaultCard>>? CurrentPlayerBoard { get; set; }

        public ActionContainer CurrentPlayerActions { get; set; } = new();

        /*
         * Creates a deep clone of current board
         */
        public object Clone()
        {
            DefaultLeader oldLeader1 = Leader1;
            DefaultLeader oldLeader2 = Leader2;
            DefaultLeader newLeader1 = (DefaultLeader)oldLeader1.Clone();
            DefaultLeader newLeader2 = (DefaultLeader)oldLeader2.Clone();
            GameBoard clonedBoard = new()
            {
                Leader1 = newLeader1,
                Leader2 = newLeader2,
                PointSumP1 = PointSumP1,
                PointSumP2 = PointSumP2,
                CurrentlyPlayingLeader = (CurrentlyPlayingLeader == oldLeader1) ? newLeader1 : newLeader2,
                CurrentPlayerBoard = (CurrentlyPlayingLeader.Board == oldLeader1.Board ? newLeader1.Board : newLeader2.Board),
                CurrentPlayerActions = (ActionContainer)CurrentPlayerActions.Clone()

            };
            return clonedBoard;
        }

        /*
         * Returns board of the current player
         */
        public List<List<DefaultCard>> GetCurrentBoard()
        {
            if (CurrentPlayerBoard != Leader1.Board && CurrentPlayerBoard != Leader2.Board)
            {
                throw new CustomException("GetCurrentBoard Inner Error");
            }
            return CurrentPlayerBoard == Leader1.Board ? Leader1.Board : Leader2.Board;
        }

        /*
         * Returns board of the enemie player
         */
        public List<List<DefaultCard>> GetEnemieBoard()
        {
            return CurrentPlayerBoard == Leader1.Board ? Leader2.Board : Leader1.Board;
        }

        /*
         * Returns currently playing leader
         */
        public DefaultLeader GetCurrentLeader()
        {
            if (CurrentlyPlayingLeader != Leader1 && CurrentlyPlayingLeader != Leader2)
            {
                throw new CustomException("GetCurrentLeader Inner Error");
            }
            return CurrentlyPlayingLeader == Leader1 ? Leader1 : Leader2;
        }

        /*
         * Returns enemie leader
         */
        public DefaultLeader GetEnemieLeader()
        {
            return CurrentlyPlayingLeader == Leader1 ? Leader2 : Leader1;
        }

        /*
         * An update after each move
         * only used on human player
         */
        public void MoveUpdate()
        {
            RemoveDestroyedCards();
            UpdatePoints();
            if (GetCurrentLeader().AbilityCharges <= 0) CurrentPlayerActions.LeaderActions = null;
        }

        /*
         * An update after each turn
         * turn: when player passes or ends turn
         */
        public void TurnUpdate()
        {
            EndTurnCardsUpdate();
            RemoveDestroyedCards();
            UpdatePoints();
            SwapPlayers();
            UpdateCPCards();
        }

        /*
         * Updates all cards that have a trigger at the end of the turn
         * so far: bleeding, timer, special end turn update
         */
        public void EndTurnCardsUpdate()
        {


            foreach (var row in Leader1.Board)
            {
                foreach (var card in row.ToList())
                {
                    if (card.Bleeding > 0) EvaluateBleeding(card);
                    if (card is ITimer TimerCard && Leader1 == GetCurrentLeader()) TimerCard.Timer(this);
                    if (card is IEndTurnUpdate UpdateCard && Leader1 == GetCurrentLeader()) UpdateCard.EndTurnUpdate(this);
                }
            }

            foreach (var row in Leader2.Board)
            {
                foreach (var card in row.ToList())
                {
                    if (card.Bleeding > 0) EvaluateBleeding(card);
                    if (card is ITimer TimerCard && Leader2 == GetCurrentLeader()) TimerCard.Timer(this);
                    if (card is IEndTurnUpdate UpdateCard && Leader2 == GetCurrentLeader()) UpdateCard.EndTurnUpdate(this);
                }
            }
        }

        /*
         * Resets board at the start of new round
         */
        public void ResetBoard()
        {
            Leader1.HasPassed = false;
            Leader2.HasPassed = false;
            RemoveCardsAtRoundEnd();
            UpdatePoints();
        }

        /*
         * Shuffle cards in hand for both players
         * (start of the game)
         */
        public void ShufflerBothDecks()
        {
            Leader1.ShuffleStartingDeck();
            Leader2.ShuffleStartingDeck();
        }

        /*
         * Places n cards from deck into hand for both players
         */
        public void DrawBothHands(int numberOfCards)
        {
            Leader1.DrawCards(numberOfCards);
            Leader2.DrawCards(numberOfCards);
        }

        /*
         * Fills in options for swaping cards at the start of a round
         */
        public void GetSwapCards()
        {
            CurrentPlayerActions.CardSwaps.Indexes.Clear();
            for (int i = 0; i < GetCurrentLeader().Hand.Cards.Count; i++)
            {
                CurrentPlayerActions.CardSwaps.Indexes.Add(i);
            }
        }

        /*
         * Clears all cards placed on the board
         */
        public void RemoveCardsAtRoundEnd()
        {
            foreach (var row in Leader1.Board)
                row.Clear();

            foreach (var row in Leader2.Board)
                row.Clear();
        }

        /*
         * Removes all cards on the board with health less than or equal to zero
         */
        public void RemoveDestroyedCards()
        {
            Leader1.Board.ToList().ForEach(row =>
            {
                row.Where(card => card.CurrentValue <= 0).ToList().ForEach(card =>
                {
                    Type cardType = card.GetType();
                    DefaultCard graveyardInstance = (DefaultCard)Activator.CreateInstance(cardType);

                    if (!(card is IDoomed))
                        Leader1.Graveyard.Cards.Add(graveyardInstance);
                    if (card is IDeathwish DeathWishAbility)
                        DeathWishAbility.DeathwishAbility(this);

                    row.Remove(card);
                });
            });

            Leader2.Board.ToList().ForEach(row =>
            {
                row.Where(card => card.CurrentValue <= 0).ToList().ForEach(card =>
                {
                    Type cardType = card.GetType();
                    DefaultCard graveyardInstance = (DefaultCard)Activator.CreateInstance(cardType);

                    if (!(card is IDoomed))
                        Leader2.Graveyard.Cards.Add(graveyardInstance);
                    if (card is IDeathwish DeathWishCard)
                        DeathWishCard.DeathwishAbility(this);

                    row.Remove(card);
                });
            });
        }

        /*
         * Methods takes a bleeding card as an argument
         * take 1 damage and remove 1 bleeding point
         * Trigger all cards that respond to bleeding
         */
        public void EvaluateBleeding(DefaultCard bleedingCard)
        {
            bleedingCard.Bleeding--;
            bleedingCard.TakeDemage(1, false, this);
            foreach (var row in Leader1.Board)
            {
                foreach (var card in row)
                {
                    if (card is IBleedingInteraction BleedCard)
                    {
                        BleedCard.RespondToBleeding();
                    }
                }
            }
            foreach (var row in Leader2.Board)
            {
                foreach (var card in row)
                {
                    if (card is IBleedingInteraction BleedCard)
                    {
                        BleedCard.RespondToBleeding();
                    }
                }
            }
        }

        /*
         * Counts up all the points on board for both players
         * Updates PointSum for both players
         */
        private void UpdatePoints()
        {
            PointSumP1 = PointSumP2 = 0;

            PointSumP1 = Leader1.Board.Sum(row => row.Sum(obj => obj.CurrentValue));
            PointSumP2 = Leader2.Board.Sum(row => row.Sum(obj => obj.CurrentValue));
        }

        /*
         * Triggers all the card methods for updating at the start of a turn
         */
        private void UpdateCPCards()
        {
            foreach (var row in GetCurrentLeader().Board)
            {
                foreach (var card in row)
                {
                    if (card is IUpdate updateCard)
                    {
                        MethodInfo methodInfo = card.GetType().GetMethod("StartTurnUpdate");
                        if (methodInfo != null)
                        {
                            updateCard.StartTurnUpdate();
                        }
                    }
                }
            }
        }

        /*
         * At the end of each turn swap players
         */
        private void SwapPlayers()
        {
            CurrentlyPlayingLeader = GetEnemieLeader();
            CurrentPlayerBoard = GetEnemieBoard();

            CurrentlyPlayingLeader.HasPlayedCard = false;
            CurrentlyPlayingLeader.HasUsedAbility = false;
        }

        /*
         * Returns ture if there is no more space on the board
         */
        public bool BoardIsFull()
        {
            bool p1BoardFull = Leader1.Board[0].Count == 9 && Leader1.Board[1].Count == 9;
            bool p2BoardFull = Leader2.Board[0].Count == 9 && Leader2.Board[1].Count == 9;

            return p1BoardFull && p2BoardFull;
        }
        
        /*
         * Returns true if there is only one space left on the board (of one player)
         */
        public bool CurrentBoardIsOneFromFull()
        {
            return GetCurrentBoard()[0].Count + GetCurrentBoard()[1].Count == 17;
        }

    }
}
