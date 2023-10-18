﻿using GwentNAi.GameSource.Board;
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

                    board.CurrentlyPlayingLeader.handDeck.Cards[cardIndex].PlayCardExpand(board);

                    HumanConsolePrint.ListPositionsForCard(board.CurrentPlayerBoard, board.CurrentPlayerActions.ImidiateActions);
                    cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions);
                    board.CurrentPlayerActions.ImidiateActions.Clear();

                    board.CurrentlyPlayingLeader.PlayCard(cardIndex, cardPos[0], cardPos[1]);
                    board.CurrentPlayerActions.PlayCardActions.Clear();
                    break;
                case 'o':
                    match = Regex.Match(action, IntPattern);
                    cardIndex = int.Parse(match.Value) - 1;
                    actionCard = board.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;

                    board.CurrentlyPlayingLeader.UseAbility();
                    actionCard.Order((IOrder)actionCard, board);
                    while(board.CurrentPlayerActions.ImidiateActions.Count > 0)
                    {
                        if (actionCard is IOrderExpandPickEnemie)
                        {
                            HumanConsolePrint.ListEnemieExpand(board.CurrentPlayerActions.ImidiateActions);
                            cardPos = HumanConsoleGet.GetPositionForCard(board.CurrentPlayerActions.ImidiateActions);
                            board.CurrentPlayerActions.ImidiateActions.Clear();
                            actionCard.postPickEnemieOrder((IOrderExpandPickEnemie)actionCard, board, cardPos[0], cardPos[1]);
                        }   
                    }
                    break;
                case 'l':
                    break;
                case 'e': //end
                    return -1;
            }
            return 0;
        }
    }
}