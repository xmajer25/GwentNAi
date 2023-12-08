using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.CardRepository;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.MctsMove
{
    public static class MCTSRandomMove
    {
        private static Random Random = new();
        public static void PlayRandomMove(MCTSNode node)
        {
            if (PlayRandomCard(node) == -1) return;
        }

        public static void PlayRandomEnemieMove(MCTSNode node)
        {
            GameBoard board = node.Board;
            if(board.GetCurrentLeader().HandDeck.Cards.Count == 0)
            {
                board.CurrentPlayerActions.PassOrEndTurn();
                return;
            }

            int randomIndex, randomRow;

            board.GetCurrentLeader().HandDeck.Cards.RemoveAt(0);
            DefaultCard enemieCard = node.EnemieCards.GetRandomCard();
            enemieCard.PlaySelfExpand(node.Board);
            (randomIndex, randomRow) = GetRandomRowAndIndexFromImidiateActions(board.CurrentPlayerActions);
            board.CurrentPlayerActions.ClearImidiateActions();
            board.CurrentPlayerActions.PlayCardActions.Clear();
            board.GetCurrentLeader().HasPlayedCard = true;
            board.GetCurrentBoard()[randomRow].Insert(randomIndex, enemieCard);
        }

        private static int PlayRandomCard(MCTSNode node)
        {
            ActionContainer possibleActions = node.Board.CurrentPlayerActions;
            DefaultLeader leader = node.Board.GetCurrentLeader();
            int randomCardIndex, randomIndex, randomRow;

            if (possibleActions.PlayCardActions.Count == 0)
            {
                //node.Board.CurrentPlayerActions.PassOrEndTurn();
                leader.Pass();
                return -1;
            }
            randomCardIndex = Random.Next(0, possibleActions.PlayCardActions.Count);
            leader.HandDeck.Cards[randomCardIndex].PlaySelfExpand(node.Board);

            (randomIndex, randomRow) = GetRandomRowAndIndexFromImidiateActions(possibleActions);

            possibleActions.ClearImidiateActions();
            possibleActions.PlayCardActions.Clear();
            leader.PlayCard(randomCardIndex, randomRow, randomIndex, node.Board);
            return 0;
        }

        private static (int, int) GetRandomRowAndIndexFromImidiateActions(ActionContainer actions)
        {
            int randomIndex, randomRow;
            if (actions.ImidiateActions[0][0].Count == 0 && actions.ImidiateActions[0][1].Count == 0) return (-1, -1);
            else if (actions.ImidiateActions[0][0].Count == 0) randomRow = 1;
            else if (actions.ImidiateActions[0][1].Count == 0) randomRow = 0;
            else randomRow = Random.Next(0, actions.ImidiateActions[0].Count);

            randomIndex = Random.Next(0, actions.ImidiateActions[0][randomRow].Count);
            randomIndex = actions.ImidiateActions[0][randomRow][randomIndex];

            return (randomIndex, randomRow);
        }
    }
}
