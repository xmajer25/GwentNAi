using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.CustomExceptions;
using GwentNAi.GameSource.Player;
using System.Data;
using System.Reflection;

namespace GwentNAi.GameSource.Board
{
    public class GameBoard : ICloneable
    {
        public DefaultLeader? Leader1 { get; set; }
        public DefaultLeader? Leader2 { get; set; }

        public int PointSumP2 { get; set; }
        public int PointSumP1 { get; set; }


        public DefaultLeader? CurrentlyPlayingLeader { get; set; }
        public List<List<DefaultCard>>? CurrentPlayerBoard { get; set; }

        public ActionContainer CurrentPlayerActions { get; set; } = new();

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

        public List<List<DefaultCard>> GetCurrentBoard()
        {
            if (CurrentPlayerBoard != Leader1.Board && CurrentPlayerBoard != Leader2.Board)
            {
                throw new CustomException("GetCurrentBoard Inner Error");
            }
            return CurrentPlayerBoard == Leader1.Board ? Leader1.Board : Leader2.Board;
        }

        public List<List<DefaultCard>> GetEnemieBoard()
        {
            return CurrentPlayerBoard == Leader1.Board ? Leader2.Board : Leader1.Board;
        }

        public DefaultLeader GetCurrentLeader()
        {
            if (CurrentlyPlayingLeader != Leader1 && CurrentlyPlayingLeader != Leader2)
            {
                throw new CustomException("GetCurrentLeader Inner Error");
            }
            return CurrentlyPlayingLeader == Leader1 ? Leader1 : Leader2;
        }

        public DefaultLeader GetEnemieLeader()
        {
            return CurrentlyPlayingLeader == Leader1 ? Leader2 : Leader1;
        }

        public void MoveUpdate()
        {
            RemoveDestroyedCards();
            UpdatePoints();
            if (GetCurrentLeader().AbilityCharges <= 0) CurrentPlayerActions.LeaderActions = null;
        }

        public void TurnUpdate()
        {
            EndTurnCardsUpdate();
            RemoveDestroyedCards();
            UpdatePoints();
            SwapPlayers();
            UpdateCPCards();
        }

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
        public void ResetBoard()
        {
            Leader1.HasPassed = false;
            Leader2.HasPassed = false;
            RemoveCardsAtRoundEnd();
            UpdatePoints();
        }

        public void ShufflerBothDecks()
        {
            Leader1.ShuffleStartingDeck();
            Leader2.ShuffleStartingDeck();
        }

        public void DrawBothHands(int numberOfCards)
        {
            Leader1.DrawCards(numberOfCards);
            Leader2.DrawCards(numberOfCards);
        }

        public void GetSwapCards()
        {
            CurrentPlayerActions.CardSwaps.Indexes.Clear();
            for (int i = 0; i < GetCurrentLeader().Hand.Cards.Count; i++)
            {
                CurrentPlayerActions.CardSwaps.Indexes.Add(i);
            }
        }

        //When we get to resiliant cards...
        //Create list that will hold them
        //clear row and then make row equal to the list with resiliant cards
        public void RemoveCardsAtRoundEnd()
        {
            foreach (var row in Leader1.Board)
                row.Clear();

            foreach (var row in Leader2.Board)
                row.Clear();
        }

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

        private void UpdatePoints()
        {
            PointSumP1 = PointSumP2 = 0;

            PointSumP1 = Leader1.Board.Sum(row => row.Sum(obj => obj.CurrentValue));
            PointSumP2 = Leader2.Board.Sum(row => row.Sum(obj => obj.CurrentValue));
        }

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

        private void SwapPlayers()
        {
            CurrentlyPlayingLeader = GetEnemieLeader();
            CurrentPlayerBoard = GetEnemieBoard();

            if (CurrentlyPlayingLeader.Board != CurrentPlayerBoard) throw new CustomException("Inner Error -> Swap Failed");

            CurrentlyPlayingLeader.HasPlayedCard = false;
            CurrentlyPlayingLeader.HasUsedAbility = false;
        }


    }
}
