using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Player;

namespace GwentNAi.MctsMove
{
    /*
     * Static class contains methods for generating random moves
     * Methods executed during simulations and enemies expansion
     */
    public static class MCTSRandomMove
    {
        private static Random Random = new();

        /*
         * Play random card for enemie player on random position
         * Selects a random card from all of the cards available
         * Returns moves played in form of string
         */
        public static string PlayRandomEnemieMove(MCTSNode node)
        {
            GameBoard board = node.Board;

            //PASS IF NO CARDS LEFT
            if (board.GetCurrentLeader().Hand.Cards.Count == 0 || board.OurBoardIsFull())
            {
                board.CurrentPlayerActions.PassOrEndTurn();
                return "Enemie passing -> no cards left to play";
            }

            //PASS IF ENEMIE HAS PASSED AND WE HAVE MORE POINTS
            int ourPointSum = (board.GetCurrentLeader() == board.Leader1 ? board.PointSumP1 : board.PointSumP2);
            int enemiePointSum = (board.GetCurrentLeader() == board.Leader1 ? board.PointSumP2 : board.PointSumP1);
            if (board.GetEnemieLeader().HasPassed && ourPointSum > enemiePointSum)
            {
                board.CurrentPlayerActions.PassOrEndTurn();
                return "Enemie passing -> point advantage";
            }

            int randomIndex, randomRow;

            //REMOVE RANDOM CARD IN HAND
            //PLAY RANDOM CARD
            //(simulates random enemie move)
            board.GetCurrentLeader().Hand.Cards.RemoveAt(0);
            DefaultCard enemieCard = node.EnemieCards.GetRandomCard();

            enemieCard.GetPlacementOptions(node.Board);
            (randomIndex, randomRow) = GetRandomRowAndIndexFromImidiateActions(board.CurrentPlayerActions);

            //CLEAR ACTION OPTIONS
            board.CurrentPlayerActions.ClearImidiateActions();
            board.CurrentPlayerActions.PlayCardActions.Clear();

            //PLAY CARD
            board.GetCurrentLeader().PlayCard(enemieCard, randomRow, randomIndex, node.Board);
            return "Enemie playing card -> " + enemieCard.Name;
        }

        /*
         * Plays random card from our hand on random position
         * Returns -1 if passing (no cards, full board..)
         * Returns 0 if card was played
         */
        public static int PlayRandomCard(MCTSNode node)
        {
            ActionContainer possibleActions = node.Board.CurrentPlayerActions;
            DefaultLeader leader = node.Board.GetCurrentLeader();
            GameBoard board = node.Board;
            int randomCardIndex, randomIndex, randomRow;

            //PASS IF BOARD FULL OR NO CARDS
            if (possibleActions.PlayCardActions.Count == 0 || node.Board.OurBoardIsFull())
            {
                node.Board.CurrentPlayerActions.PassOrEndTurn();
                return -1;
            }

            //PASS IF ENEMIE PASSED AND WE ARE WINNING
            int ourPointSum = (board.GetCurrentLeader() == board.Leader1 ? board.PointSumP1 : board.PointSumP2);
            int enemiePointSum = (board.GetCurrentLeader() == board.Leader1 ? board.PointSumP2 : board.PointSumP1);
            if (board.GetEnemieLeader().HasPassed && ourPointSum > enemiePointSum)
            {
                board.CurrentPlayerActions.PassOrEndTurn();
                return -1;
            }

            //PLAY RONDOM
            randomCardIndex = Random.Next(0, possibleActions.PlayCardActions.Count);
            leader.Hand.Cards[randomCardIndex].GetPlacementOptions(node.Board);

            (randomIndex, randomRow) = GetRandomRowAndIndexFromImidiateActions(possibleActions);

            //CLEAR ACTION OPTIONS
            possibleActions.ClearImidiateActions();
            possibleActions.PlayCardActions.Clear();

            //PLAY CARD
            leader.PlayCard(randomCardIndex, randomRow, randomIndex, node.Board);

            //NO PASS RETURN
            return 0;
        }

        /*
         * For random gameplay, cards need to be played on random indexes
         * From ImidiateActions selects random row-column
         * Returns random position
         */
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
