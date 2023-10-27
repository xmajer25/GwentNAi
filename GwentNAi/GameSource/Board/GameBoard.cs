using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Player;
using System.Data;
using System.Reflection;

namespace GwentNAi.GameSource.Board
{
    public class GameBoard
    {
        public DefaultLeader? Leader1 { get; set; }
        public DefaultLeader? Leader2 { get; set; }

        public int PointSumP2 { get; set; }
        public int PointSumP1 { get; set; }


        public DefaultLeader? CurrentlyPlayingLeader { get; set; }
        public List<List<DefaultCard>>? CurrentPlayerBoard { get; set; }

        public ActionContainer CurrentPlayerActions { get; set; } = new();

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
                foreach (var card in row)
                {
                    if(card.bleeding > 0) EvaluateBleeding(card);
                }
            }

            foreach (var row in Leader2.Board)
            {
                foreach (var card in row)
                {
                    if (card.bleeding > 0) EvaluateBleeding(card);
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
            List<List<int>> swapOptions = new List<List<int>>(2) { new List<int>(10), new List<int>(0)};
            for (int i = 0; i < CurrentlyPlayingLeader.handDeck.Cards.Count; i++)
            {
                swapOptions[0].Add(i);
            }
            CurrentPlayerActions.ImidiateActions[0] = swapOptions;
        }

        //When we get to resiliant cards...
        //Create list that will hold them
        //clear row and then make row equal to the list with resiliant cards
        public void RemoveCardsAtRoundEnd()
        {
            foreach(var row in Leader1.Board)
            {
                row.Clear();
                /*foreach(var card in row)
                {
                }*/
            }
            foreach (var row in Leader2.Board)
            {
                row.Clear();
                /*foreach (var card in row)
                {
                }*/
            }
        }

        public void RemoveDestroyedCards()
        {
            int currentRow = 0;
            int currentColumn = 0;
            List<int> cardsToRemoveIndexes = new List<int>();

            foreach (var row in Leader1.Board)
            {
                foreach (var card in row)
                {
                    if (card.currentValue <= 0) cardsToRemoveIndexes.Add(currentColumn);
                    currentColumn++;
                }

                cardsToRemoveIndexes.Reverse();
                foreach (int index in cardsToRemoveIndexes)
                {
                    if (Leader1.Board[currentRow][index] is not IDoomed) Leader1.graveyardDeck.Cards.Add(Leader1.Board[currentRow][index]);
                    Leader1.Board[currentRow].RemoveAt(index);
                }
                cardsToRemoveIndexes.Clear();
                currentColumn = 0;
                currentRow++;
            }

            currentRow = 0;
            currentColumn = 0;
            cardsToRemoveIndexes.Clear();
            foreach (var row in Leader2.Board)
            {
                foreach (var card in row)
                {
                    if (card.currentValue <= 0) cardsToRemoveIndexes.Add(currentColumn);
                    currentColumn++;
                }

                cardsToRemoveIndexes.Reverse();
                foreach (int index in cardsToRemoveIndexes)
                {
                    if (Leader2.Board[currentRow][index] is not IDoomed) Leader2.graveyardDeck.Cards.Add(Leader1.Board[currentRow][index]);
                    Leader2.Board[currentRow].RemoveAt(index);
                }
                cardsToRemoveIndexes.Clear();
                currentColumn = 0;
                currentRow++;
            }
        }
        
        public void EvaluateBleeding(DefaultCard bleedingCard)
        {
            bleedingCard.bleeding--;
            bleedingCard.currentValue--;
            foreach(var row in Leader1.Board)
            {
                foreach(var card in row)
                {
                    if(card is IBleedingInteraction)
                    {
                        card.RespondToBleeding((IBleedingInteraction)(card));
                    }
                }
            }
            foreach (var row in Leader2.Board)
            {
                foreach (var card in row)
                {
                    if (card is IBleedingInteraction)
                    {
                        card.RespondToBleeding((IBleedingInteraction)(card));
                    }
                }
            }
        }

        private void UpdatePoints()
        {
            PointSumP1 = PointSumP2 = 0;
            foreach (var row in Leader1.Board)
            {
                foreach (var card in row)
                {
                    PointSumP1 += card.currentValue;
                }
            }

            foreach (var row in Leader2.Board)
            {
                foreach (var card in row)
                {
                    PointSumP2 += card.currentValue;
                }
            }
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
                            card.StartTurnUpdate((IUpdate) card);
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
