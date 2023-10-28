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

                    board.CurrentlyPlayingLeader.handDeck.Cards[cardIndex].PlaySelfExpand(board);

                    HumanConsolePrint.ListPositionsForCard(board.CurrentPlayerBoard, board.CurrentPlayerActions.ImidiateActions[0]);
                    cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);

                    board.CurrentPlayerActions.ClearImidiateActions();
                    board.CurrentPlayerActions.PlayCardActions.Clear();
                    board.CurrentlyPlayingLeader.PlayCard(cardIndex, cardPos[0], cardPos[1], board);

                    //deploy
                    while (board.CurrentPlayerActions.AreImidiateActionsFull())
                    {
                        if (actionCard is IDeployExpandPickEnemies)
                        {
                            HumanConsolePrint.ListEnemieExpand(board.CurrentPlayerActions.ImidiateActions[0]);
                            cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            actionCard.postPickEnemieAbilitiy((IDeployExpandPickEnemies)actionCard, board, cardPos[0], cardPos[1]);
                        }
                        else if(actionCard is IDeployExpandPickCard)
                        {
                            HumanConsolePrint.ListCardsSimple(board.CurrentPlayerActions.ImidiateActions[0][0]);
                            int index = HumanConsoleGet.GetIndex(board.CurrentPlayerActions.ImidiateActions[0][0]);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            actionCard.postPickCardAbility((IDeployExpandPickCard)actionCard, board, index);
                        }
                    }

                    break;
                case 'o':
                    match = Regex.Match(action, IntPattern);
                    cardIndex = int.Parse(match.Value) - 1;
                    actionCard = board.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;

                    
                    actionCard.Order((IOrder)actionCard, board);
                    while (board.CurrentPlayerActions.AreImidiateActionsFull())
                    {
                        if (actionCard is IOrderExpandPickEnemie)
                        {
                            HumanConsolePrint.ListEnemieExpand(board.CurrentPlayerActions.ImidiateActions[0]);
                            cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            actionCard.postPickEnemieOrder((IOrderExpandPickEnemie)actionCard, board, cardPos[0], cardPos[1]);
                        }   
                        else if(actionCard is IOrderExpandPickAll)
                        {
                            HumanConsolePrint.ListAllExpand(board.CurrentPlayerActions.ImidiateActions);
                            cardPos = HumanConsoleGet.GetPositionFromWholeBoard(board.CurrentPlayerActions.ImidiateActions);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            actionCard.postPickAllOrder((IOrderExpandPickAll)actionCard, board, cardPos[0], cardPos[1], cardPos[2]);
                        }
                        else if(actionCard is IOrderExpandPickAlly)
                        {
                            HumanConsolePrint.ListEnemieExpand(board.CurrentPlayerActions.ImidiateActions[0]);
                            cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            actionCard.PostPickAllyOrder((IOrderExpandPickAlly)actionCard, board, cardPos[0], cardPos[1]);
                        }
                        else if(actionCard is IPlayCardExpand)
                        {
                            HumanConsolePrint.ListPositionsForCard(board.CurrentPlayerBoard, board.CurrentPlayerActions.ImidiateActions[0]);
                            cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            actionCard.PostPlayCardOrder((IPlayCardExpand)actionCard, board, cardPos[0], cardPos[1]);
                        }
                    }
                    break;
                case 'l':
                    board.CurrentPlayerActions.LeaderActions(board);
                    board.CurrentlyPlayingLeader.UseAbility();
                    while (board.CurrentPlayerActions.AreImidiateActionsFull())
                    {
                        if(board.CurrentlyPlayingLeader is IPlayCardExpand)
                        {
                            HumanConsolePrint.ListPositionsForCard(board.CurrentPlayerBoard, board.CurrentPlayerActions.ImidiateActions[0]);
                            cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions[0]);
                            board.CurrentPlayerActions.ClearImidiateActions();
                            board.CurrentlyPlayingLeader.PostPlayCardOrder((IPlayCardExpand)board.CurrentlyPlayingLeader, board, cardPos[0], cardPos[1]);
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
