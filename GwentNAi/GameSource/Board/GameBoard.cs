using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Player;
using System.Data;
using System.Reflection;
using System.Transactions;

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
            GameBoard clonedBoard = new()
            {
                Leader1 = (DefaultLeader)Leader1.Clone(),
                Leader2 = (DefaultLeader)Leader2.Clone(),
                PointSumP1 = PointSumP1,
                PointSumP2 = PointSumP2,
                CurrentlyPlayingLeader = null,
                CurrentPlayerBoard = CurrentPlayerBoard?.Select(innerList => innerList.Select(card => (DefaultCard)card.Clone()).ToList()).ToList(),
                CurrentPlayerActions = (ActionContainer)CurrentPlayerActions.Clone()
            };
            clonedBoard.CurrentlyPlayingLeader = (CurrentlyPlayingLeader == Leader1) ? clonedBoard.Leader1 : clonedBoard.Leader2;
            return clonedBoard;
        }

        public void MoveUpdate()
        {
            RemoveDestroyedCards();
            UpdatePoints();
            if (CurrentlyPlayingLeader.abilityCharges <= 0) CurrentPlayerActions.LeaderActions = null;
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
                    if(card.bleeding > 0) EvaluateBleeding(card);
                    if (card is ITimer TimerCard && Leader1 == CurrentlyPlayingLeader) TimerCard.Timer(this);
                    if (card is IEndTurnUpdate UpdateCard && Leader1 == CurrentlyPlayingLeader) UpdateCard.EndTurnUpdate(this);
                }
            }

            foreach (var row in Leader2.Board)
            {
                foreach (var card in row.ToList())
                {
                    if (card.bleeding > 0) EvaluateBleeding(card);
                    if (card is ITimer TimerCard && Leader2 == CurrentlyPlayingLeader) TimerCard.Timer(this);
                    if (card is IEndTurnUpdate UpdateCard && Leader2 == CurrentlyPlayingLeader) UpdateCard.EndTurnUpdate(this);
                }
            }
        }
        public void ResetBoard()
        {
            Leader1.hasPassed = false;
            Leader2.hasPassed = false;
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

        public void SwapCards()
        {
            CurrentPlayerActions.SwapCards.Indexes.Clear();
            for (int i = 0; i < CurrentlyPlayingLeader.handDeck.Cards.Count; i++)
            {
                CurrentPlayerActions.SwapCards.Indexes.Add(i);
            }
        }

        //When we get to resiliant cards...
        //Create list that will hold them
        //clear row and then make row equal to the list with resiliant cards
        public void RemoveCardsAtRoundEnd()
        {
            foreach(var row in Leader1.Board)
                row.Clear();
            
            foreach (var row in Leader2.Board)
                row.Clear();
        }

        public void RemoveDestroyedCards()
        {
            Leader1.Board.ToList().ForEach(row =>
            {
                row.Where(card => card.currentValue <= 0).ToList().ForEach(card =>
                {
                    Type cardType = card.GetType();
                    DefaultCard graveyardInstance = (DefaultCard)Activator.CreateInstance(cardType);

                    if (!(card is IDoomed))
                        Leader1.graveyardDeck.Cards.Add(graveyardInstance);
                    if (card is IDeathwish DeathWishAbility)
                        DeathWishAbility.DeathwishAbility(this);

                    row.Remove(card);
                });
            });

            Leader2.Board.ToList().ForEach(row =>
            {
                row.Where(card => card.currentValue <= 0).ToList().ForEach(card =>
                {
                    Type cardType = card.GetType();
                    DefaultCard graveyardInstance = (DefaultCard)Activator.CreateInstance(cardType);

                    if (!(card is IDoomed))
                        Leader2.graveyardDeck.Cards.Add(graveyardInstance);
                    if (card is IDeathwish DeathWishCard)
                        DeathWishCard.DeathwishAbility(this);

                    row.Remove(card);
                });
            });
        }
        
        public void EvaluateBleeding(DefaultCard bleedingCard)
        {
            bleedingCard.bleeding--;
            bleedingCard.TakeDemage(1, false, this);
            foreach(var row in Leader1.Board)
            {
                foreach(var card in row)
                {
                    if(card is IBleedingInteraction BleedCard)
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

            PointSumP1 = Leader1.Board.Sum(row => row.Sum(obj => obj.currentValue));
            PointSumP2 = Leader2.Board.Sum(row => row.Sum(obj => obj.currentValue));
        }

        private void UpdateCPCards()
        {
            foreach (var row in CurrentlyPlayingLeader.Board)
            {
                foreach (var card in row)
                {
                    if (card is IUpdate updateCard)
                    {
                        MethodInfo methodInfo = card.GetType().GetMethod("StartTurnUpdate");
                        if(methodInfo != null)
                        {
                            updateCard.StartTurnUpdate();
                        }
                    }
                }
            }
        }

        private void SwapPlayers()
        {
            CurrentlyPlayingLeader = (CurrentlyPlayingLeader == Leader1 ? Leader2 : Leader1);
            CurrentPlayerBoard = (CurrentPlayerBoard == Leader1.Board ? Leader2.Board : Leader1.Board);

            CurrentlyPlayingLeader.hasPlayedCard = false;
            CurrentlyPlayingLeader.hasUsedAbility = false;
        }

        
    }
}
