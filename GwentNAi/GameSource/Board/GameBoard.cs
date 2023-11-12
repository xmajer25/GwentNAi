﻿using GwentNAi.GameSource.Cards;
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
                Leader1 = Leader1,
                Leader2 = Leader2,
                PointSumP1 = PointSumP1,
                PointSumP2 = PointSumP2,
                CurrentlyPlayingLeader = CurrentlyPlayingLeader,
                CurrentPlayerBoard = CurrentPlayerBoard,
                CurrentPlayerActions = CurrentPlayerActions
            };

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
                    if (card is ITimer && Leader1 == CurrentlyPlayingLeader) card.Timer((ITimer)card, this);
                    if (card is IEndTurnUpdate UpdateCard && Leader1 == CurrentlyPlayingLeader) UpdateCard.EndTurnUpdate(this);
                }
            }

            foreach (var row in Leader2.Board)
            {
                foreach (var card in row.ToList())
                {
                    if (card.bleeding > 0) EvaluateBleeding(card);
                    if (card is ITimer && Leader2 == CurrentlyPlayingLeader) card.Timer((ITimer)card, this);
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
                    Type cardType = Leader1.Board[currentRow][index].GetType();
                    DefaultCard graveyardInstance = (DefaultCard)Activator.CreateInstance(cardType);

                    if (Leader1.Board[currentRow][index] is not IDoomed) 
                        Leader1.graveyardDeck.Cards.Add(graveyardInstance);
                    if (Leader1.Board[currentRow][index] is IDeathwish)
                        Leader1.Board[currentRow][index].DeathwishAbility((IDeathwish)Leader1.Board[currentRow][index], this);
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
                    Type cardType = Leader2.Board[currentRow][index].GetType();
                    DefaultCard graveyardInstance = (DefaultCard)Activator.CreateInstance(cardType);

                    if (Leader2.Board[currentRow][index] is not IDoomed) 
                        Leader2.graveyardDeck.Cards.Add(graveyardInstance);
                    if (Leader2.Board[currentRow][index] is IDeathwish) 
                        Leader2.Board[currentRow][index].DeathwishAbility((IDeathwish)Leader2.Board[currentRow][index], this);

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
            bleedingCard.TakeDemage(1, false, this);
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
