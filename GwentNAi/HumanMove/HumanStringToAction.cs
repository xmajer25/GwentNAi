using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IExpand;
using System.Text.RegularExpressions;

namespace GwentNAi.HumanMove
{
    public static class HumanStringToAction
    {
        static readonly string IntPattern = @"\d+";
        public static int Convert(string action, GameBoard board)
        {
            Match match;
            int cardIndex;
            DefaultCard actionCard;
            int[] cardPos;

            switch (action[0])
            {
                case 'p':
                    if (action == "pass")
                    {
                        board.CurrentPlayerActions.PassOrEndTurn();
                        return -1;
                    }

                    match = Regex.Match(action, IntPattern);
                    cardIndex = int.Parse(match.Value) - 1;
                    actionCard = board.CurrentPlayerActions.PlayCardActions[cardIndex].ActionCard;

                    actionCard.GetPlacementOptions(board);

                    HumanConsolePrint.ListPositionsForCard(board.CurrentPlayerBoard, board.CurrentPlayerActions.ImidiateActions[0]);
                    cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);

                    board.CurrentPlayerActions.ClearImidiateActions();
                    board.CurrentPlayerActions.PlayCardActions.Clear();
                    board.GetCurrentLeader().PlayCard(cardIndex, cardPos[0], cardPos[1], board);

                    //deploy
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
                            HumanConsolePrint.ListCardsSimple(board.CurrentPlayerActions.ImidiateActions[0][0]);
                            int index = HumanConsoleGet.GetIndex(board.CurrentPlayerActions.ImidiateActions[0][0]);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            PickCardCard.postPickCardAbility(board, index);
                        }
                    }

                    break;
                case 'o':
                    match = Regex.Match(action, IntPattern);
                    cardIndex = int.Parse(match.Value) - 1;
                    actionCard = board.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;


                    if (actionCard is IOrder orderCard)
                        orderCard.Order(board);

                    while (board.CurrentPlayerActions.AreImidiateActionsFull())
                    {
                        if (actionCard is IOrderExpandPickEnemie PickEnemieCard)
                        {
                            HumanConsolePrint.ListEnemieExpand(board.CurrentPlayerActions.ImidiateActions[0]);
                            cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            PickEnemieCard.PostPickEnemieOrder(board, cardPos[0], cardPos[1]);
                        }
                        else if (actionCard is IOrderExpandPickAll PickAllCard)
                        {
                            HumanConsolePrint.ListAllExpand(board.CurrentPlayerActions.ImidiateActions);
                            cardPos = HumanConsoleGet.GetPositionFromWholeBoard(board.CurrentPlayerActions.ImidiateActions);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            PickAllCard.PostPickAllOrder(board, cardPos[0], cardPos[1], cardPos[2]);
                        }
                        else if (actionCard is IOrderExpandPickAlly PickAllyCard)
                        {
                            HumanConsolePrint.ListEnemieExpand(board.CurrentPlayerActions.ImidiateActions[0]);
                            cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            PickAllyCard.PostPickAllyOrder(board, cardPos[0], cardPos[1]);
                        }
                        else if (actionCard is IPlayCardExpand PlayCardCard)
                        {
                            HumanConsolePrint.ListPositionsForCard(board.CurrentPlayerBoard, board.CurrentPlayerActions.ImidiateActions[0]);
                            cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            PlayCardCard.PostPlayCardOrder(board, cardPos[0], cardPos[1]);
                        }
                    }
                    break;
                case 'l':
                    board.CurrentPlayerActions.LeaderActions(board);
                    while (board.CurrentPlayerActions.AreImidiateActionsFull())
                    {
                        if (board.GetCurrentLeader() is IPlayCardExpand leader)
                        {
                            HumanConsolePrint.ListPositionsForCard(board.CurrentPlayerBoard, board.CurrentPlayerActions.ImidiateActions[0]);
                            cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            leader.PostPlayCardOrder(board, cardPos[0], cardPos[1]);
                        }
                    }
                    break;
                case 'e': //end
                    return -1;
            }
            return 0;
        }
    }
}
