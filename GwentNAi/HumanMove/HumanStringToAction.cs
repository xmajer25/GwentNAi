using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IExpand;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace GwentNAi.HumanMove
{
    /*
     * Class contains methods for taking input and converting it into real actions
     * From string..recognize what action is desired...
     * Request additional information if needed
     * Play out the action on the main board
     */
    public static class HumanStringToAction
    {
        static readonly string IntPattern = @"\d+";

        /*
         * Method for playing a card on the board
         * Plays desired card on the board -> expands deploy options if needed
         */
        private static void PlayingCardConvert(string action, GameBoard board)
        {
            Match match = Regex.Match(action, IntPattern);
            int cardIndex = int.Parse(match.Value) - 1;
            DefaultCard actionCard = board.CurrentPlayerActions.PlayCardActions[cardIndex].ActionCard;

            actionCard.GetPlacementOptions(board);

            HumanConsolePrint.ListPositionsForCard(board.CurrentPlayerBoard, board.CurrentPlayerActions.ImidiateActions[0]);
            int[] cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);

            board.CurrentPlayerActions.ClearImidiateActions();
            board.CurrentPlayerActions.PlayCardActions.Clear();
            board.GetCurrentLeader().PlayCard(cardIndex, cardPos[0], cardPos[1], board);

            //deploy options
            while (board.CurrentPlayerActions.AreImidiateActionsFull())
            {
                if (actionCard is IDeployExpandPickEnemies PickEnemiesCard)
                {
                    HumanConsolePrint.ListEnemieExpand(board.CurrentPlayerActions.ImidiateActions[0]);
                    cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                    board.CurrentPlayerActions.ClearImidiateActions();
                    PickEnemiesCard.postPickEnemieAbilitiy(board, cardPos[0], cardPos[1]);
                }
                else if (actionCard is IDeployExpandPickAlly PickAllyCard)
                {
                    HumanConsolePrint.ListEnemieExpand(board.CurrentPlayerActions.ImidiateActions[0]);
                    cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                    board.CurrentPlayerActions.ClearImidiateActions();
                    PickAllyCard.PostPickAllyAbilitiy(board, cardPos[0], cardPos[1]);
                }
                else if (actionCard is IDeployExpandPickCard PickCardCard)
                {
                    HumanConsolePrint.ListCards(board.CurrentPlayerActions.ImidiateActions[0][0]);
                    int index = HumanConsoleGet.GetIndex(board.CurrentPlayerActions.ImidiateActions[0][0]);
                    board.CurrentPlayerActions.ClearImidiateActions();
                    PickCardCard.postPickCardAbility(board, index);
                }
            }
        }


        /*
         * Method for ordering 
         * Request additional information if needed (targeted card for example)
         * Plays out the order on main board
         */
        private static void OrderConvert(string action, GameBoard board)
        {
            Match match = Regex.Match(action, IntPattern);
            int cardIndex = int.Parse(match.Value) - 1;
            DefaultCard actionCard = board.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;


            if (actionCard is IOrder orderCard)
                orderCard.Order(board);

            while (board.CurrentPlayerActions.AreImidiateActionsFull())
            {
                if (actionCard is IOrderExpandPickEnemie PickEnemieCard)
                {
                    HumanConsolePrint.ListEnemieExpand(board.CurrentPlayerActions.ImidiateActions[0]);
                    int[] cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                    board.CurrentPlayerActions.ClearImidiateActions();
                    PickEnemieCard.PostPickEnemieOrder(board, cardPos[0], cardPos[1]);
                }
                else if (actionCard is IOrderExpandPickAll PickAllCard)
                {
                    HumanConsolePrint.ListAllExpand(board.CurrentPlayerActions.ImidiateActions);
                    int[] cardPos = HumanConsoleGet.GetPositionFromWholeBoard(board.CurrentPlayerActions.ImidiateActions);
                    board.CurrentPlayerActions.ClearImidiateActions();
                    PickAllCard.PostPickAllOrder(board, cardPos[0], cardPos[1], cardPos[2]);
                }
                else if (actionCard is IOrderExpandPickAlly PickAllyCard)
                {
                    HumanConsolePrint.ListEnemieExpand(board.CurrentPlayerActions.ImidiateActions[0]);
                    int[] cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                    board.CurrentPlayerActions.ClearImidiateActions();
                    PickAllyCard.PostPickAllyOrder(board, cardPos[0], cardPos[1]);
                }
                else if (actionCard is IPlayCardExpand PlayCardCard)
                {
                    HumanConsolePrint.ListPositionsForCard(board.CurrentPlayerBoard, board.CurrentPlayerActions.ImidiateActions[0]);
                    int[] cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                    board.CurrentPlayerActions.ClearImidiateActions();
                    PlayCardCard.PostPlayCardOrder(board, cardPos[0], cardPos[1]);
                }
            }
        }

        /*
         * Method for using the leader ability
         * Requests indexes for target if needed
         * Plays out the leader ability on main board
         */
        private static void LeaderActionConvert(GameBoard board)
        {
            board.CurrentPlayerActions.LeaderActions(board);
            while (board.CurrentPlayerActions.AreImidiateActionsFull())
            {
                if (board.GetCurrentLeader() is IPlayCardExpand leader)
                {
                    HumanConsolePrint.ListPositionsForCard(board.CurrentPlayerBoard, board.CurrentPlayerActions.ImidiateActions[0]);
                    int[] cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                    board.CurrentPlayerActions.ClearImidiateActions();
                    leader.PostPlayCardOrder(board, cardPos[0], cardPos[1]);
                }
            }
        }


        /*
         * From user input, calls methods for playing out the desired action
         * For 'pass' and 'end' returns -1 to detect the user ending the turn
         */
        public static int Convert(string action, GameBoard board)
        {
            switch (action[0])
            {
                case 'p':
                    if (action == "pass")
                    {
                        board.CurrentPlayerActions.PassOrEndTurn();
                        return -1;
                    }

                    PlayingCardConvert(action, board);
                    break;
                case 'o':
                    OrderConvert(action, board);
                    break;
                case 'l':
                    LeaderActionConvert(board);
                    break;
                case 'e': //end
                    return -1;
            }
            return 0;
        }
    }
}
