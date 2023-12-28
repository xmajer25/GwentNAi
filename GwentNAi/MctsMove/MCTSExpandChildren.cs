using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IExpand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.MctsMove
{
    public static class MCTSExpandChildren
    {
        public static void ExpandOneEnemie(MCTSNode parent)
        {
            GameBoard clonedBoard = (GameBoard)parent.Board.Clone();
            MCTSNode enemie = new MCTSNode(parent, clonedBoard);

            //SWAP PLAYERS -> PLAY RANDOM MOVE -> SWAP BACK
            enemie.Board.TurnUpdate();
            enemie.Board.CurrentPlayerActions.GetAllActions(enemie.Board.GetCurrentBoard(), enemie.Board.GetCurrentLeader().HandDeck, enemie.Board.GetCurrentLeader());
            MCTSRandomMove.PlayRandomEnemieMove(enemie);
            enemie.Board.TurnUpdate();

            enemie.Board.CurrentPlayerActions.GetAllActions(enemie.Board.GetCurrentBoard(), enemie.Board.GetCurrentLeader().HandDeck, enemie.Board.GetCurrentLeader());
            parent.AppendChild(enemie.Board, true, "ENEMIE");
        }

        public static void SwapCard(MCTSNode parent)
        {
            //Children for all possible swaps
            foreach(int index in parent.Board.CurrentPlayerActions.SwapCards.Indexes.ToList())
            {
                GameBoard clonedBoard = (GameBoard)parent.Board.Clone();
                clonedBoard.GetCurrentLeader().SwapCards(index);
                clonedBoard.CurrentPlayerActions.SwapCards.CardSwaps--;
                parent.AppendChild(clonedBoard, false, "Swapping " + index + ". card");
            }

            //Child where no swap has been chosen
            GameBoard noSwapBoard = (GameBoard)parent.Board.Clone();
            noSwapBoard.CurrentPlayerActions.SwapCards.StopSwapping = true;
            parent.AppendChild(noSwapBoard, true, "Stop Swapping" );
        }

        public static void PlayCard(MCTSNode parent) 
        {
            ActionContainer actionContainer = parent.Board.CurrentPlayerActions;

            //PLAY CARD CHILDREN
            for (int cardIndex = 0; cardIndex < actionContainer.PlayCardActions.Count; cardIndex++)
            {
                //EXPAND OPTIONS FOR CARD PLACEMENT 
                DefaultCard playCard = actionContainer.PlayCardActions[cardIndex].ActionCard;
                playCard.PlaySelfExpand(parent.Board);

                //CREATE CHILD FOR EACH OPTION
                for (int row = 0; row < actionContainer.ImidiateActions[0].Count; row++)
                {
                    if (actionContainer.ImidiateActions[0][row].Count == 9) continue;
                    foreach(int index in actionContainer.ImidiateActions[0][row])
                    {
                        //PLAY CARD
                        GameBoard clonedBoard = (GameBoard)parent.Board.Clone();
                        playCard = clonedBoard.CurrentPlayerActions.PlayCardActions[cardIndex].ActionCard;
                        clonedBoard.CurrentPlayerActions.ClearImidiateActions();
                        clonedBoard.GetCurrentLeader().PlayCard(cardIndex, row, index, clonedBoard);



                        //DEPLOY OPTIONS CHILDREN
                        if (clonedBoard.CurrentPlayerActions.AreImidiateActionsFull())
                        {
                            //PICK ENEMIE DEPLOY CHILDREN
                            if (playCard is IDeployExpandPickEnemies PickEnemiesCard)
                            {
                                for(int deployRow = 0; deployRow < clonedBoard.CurrentPlayerActions.ImidiateActions[0].Count; deployRow++)
                                {
                                    foreach(int deployIndex in clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow])
                                    {
                                        GameBoard clonedDeployBoard = (GameBoard)clonedBoard.Clone();
                                        playCard = clonedDeployBoard.GetCurrentBoard()[row][index];

                                        clonedDeployBoard.CurrentPlayerActions.PlayCardActions.Clear();
                                        clonedDeployBoard.CurrentPlayerActions.ClearImidiateActions();

                                        if(playCard is IDeployExpandPickEnemies iPE)
                                            iPE.postPickEnemieAbilitiy(clonedDeployBoard, deployRow, deployIndex);
                                        parent.AppendChild(clonedDeployBoard, false, "Playing (epEnemie)card " + playCard.name + " targeting: " + deployRow + "-" + deployIndex);
                                    }
                                }
                            }
                            //PICK ALLY DEPLOY CHILDREN
                            else if (playCard is IDeployExpandPickAlly PickAllyCard)
                            {
                                for (int deployRow = 0; deployRow < clonedBoard.CurrentPlayerActions.ImidiateActions[0].Count; deployRow++)
                                {
                                    foreach (int deployIndex in clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow])
                                    {
                                        GameBoard clonedDeployBoard = (GameBoard)clonedBoard.Clone();
                                        playCard = clonedDeployBoard.GetCurrentBoard()[row][index];

                                        clonedDeployBoard.CurrentPlayerActions.PlayCardActions.Clear();
                                        clonedDeployBoard.CurrentPlayerActions.ClearImidiateActions();

                                        if (playCard is IDeployExpandPickAlly iPA)
                                            iPA.PostPickAllyAbilitiy(clonedDeployBoard, deployRow, deployIndex);
                                        parent.AppendChild(clonedDeployBoard, false, "Playing (epAlly)card " + playCard.name + " targeting: " + deployRow + "-" + deployIndex);
                                    }
                                }
                            }
                            //PICK CARD DEPLOY CHILDREN
                            else if (playCard is IDeployExpandPickCard PickCardCard)
                            {
                                foreach (int deployIndex in clonedBoard.CurrentPlayerActions.ImidiateActions[0][0])
                                {
                                    GameBoard clonedDeployBoard = (GameBoard)clonedBoard.Clone();
                                    playCard = clonedDeployBoard.GetCurrentBoard()[row][index];

                                    clonedDeployBoard.CurrentPlayerActions.PlayCardActions.Clear();
                                    clonedDeployBoard.CurrentPlayerActions.ClearImidiateActions();

                                    if (playCard is IDeployExpandPickCard iPC)
                                        iPC.postPickCardAbility(clonedDeployBoard, deployIndex);
                                    parent.AppendChild(clonedDeployBoard, false, "Playing (epCard)card " + playCard.name + " targeting: " +  deployIndex);
                                }
                            }
                        }
                        //NO DEPLOY OPTIONS CARD
                        else
                        {
                            clonedBoard.CurrentPlayerActions.PlayCardActions.Clear();
                            parent.AppendChild(clonedBoard, false, "Playing card " + playCard.name);
                        }
                    }
                }
            }

            parent.Board.CurrentPlayerActions.GetPassOrEndTurn(parent.Board.GetCurrentLeader());

            //PASS CHILD
            if (parent.Board.CurrentPlayerActions.canPass)
            {
                GameBoard passBoard = (GameBoard)parent.Board.Clone();
                passBoard.CurrentPlayerActions.GetPassOrEndTurn(passBoard.GetCurrentLeader());
                passBoard.CurrentPlayerActions.PassOrEndTurn();
                parent.AppendChild(passBoard, true, "Passing");
            }

            //END CHILD
            if (parent.Board.CurrentPlayerActions.canEnd)
            {
                GameBoard endBoard = (GameBoard)parent.Board.Clone();
                parent.AppendChild(endBoard, true, "Ending turn");
            }
        }


        public static void OrderCard(MCTSNode parent)
        {
            for(int cardIndex = 0; cardIndex < parent.Board.CurrentPlayerActions.OrderActions.Count; cardIndex++)
            {

                GameBoard clonedBoard = (GameBoard)parent.Board.Clone();

                DefaultCard actionCard = clonedBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                clonedBoard.CurrentPlayerActions.ClearImidiateActions();
                if (actionCard is IOrder orderCard)
                {
                    orderCard.Order(clonedBoard);
                }

                if((actionCard is IOrderExpandPickAll || 
                    actionCard is IOrderExpandPickEnemie || 
                    actionCard is IOrderExpandPickAlly ||
                    actionCard is IPlayCardExpand) && 
                    !clonedBoard.CurrentPlayerActions.AreImidiateActionsFull())
                {
                    continue;
                }

                if (clonedBoard.CurrentPlayerActions.AreImidiateActionsFull())
                {
                    GameBoard clonedOrderBoard = (GameBoard)clonedBoard.Clone();
                    actionCard = clonedOrderBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;

                    //PICK ENEMIE ORDER
                    if (actionCard is IOrderExpandPickEnemie pickEnemieCard)
                    {
                        for (int deployRow = 0; deployRow < clonedBoard.CurrentPlayerActions.ImidiateActions[0].Count; deployRow++)
                        {
                            foreach (int deployIndex in clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow])
                            {
                                GameBoard childBoard = (GameBoard)clonedOrderBoard.Clone();
                                actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                                childBoard.CurrentPlayerActions.ClearImidiateActions();

                                if(actionCard is IOrderExpandPickEnemie pEC)
                                    pEC.postPickEnemieOrder(childBoard, deployRow, deployIndex);

                                childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                                parent.AppendChild(childBoard, false);
                            }
                        }
                    }

                    //PICK ALLY ORDER
                    else if(actionCard is IOrderExpandPickAlly pickAllyCard)
                    {
                        for (int deployRow = 0; deployRow < clonedBoard.CurrentPlayerActions.ImidiateActions[0].Count; deployRow++)
                        {
                            foreach (int deployIndex in clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow])
                            {
                                GameBoard childBoard = (GameBoard)clonedOrderBoard.Clone();
                                actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                                childBoard.CurrentPlayerActions.ClearImidiateActions();

                                if(actionCard is IOrderExpandPickAlly pAC)
                                    pAC.PostPickAllyOrder(childBoard, deployRow, deployIndex);

                                childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                                parent.AppendChild(childBoard, false);
                            }
                        }
                    }

                    //PICK FROM ALL ORDER
                    else if(actionCard is IOrderExpandPickAll pickAllCard)
                    {
                        for(int orderPlayer = 0; orderPlayer < clonedBoard.CurrentPlayerActions.ImidiateActions.Count; orderPlayer++)
                        {
                            for (int deployRow = 0; deployRow < clonedBoard.CurrentPlayerActions.ImidiateActions[orderPlayer].Count; deployRow++)
                            {
                                foreach (int deployIndex in clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow])
                                {
                                    GameBoard childBoard = (GameBoard)clonedOrderBoard.Clone();
                                    actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                                    childBoard.CurrentPlayerActions.ClearImidiateActions();

                                    if (actionCard is IOrderExpandPickAll pAC)
                                        pAC.postPickAllOrder(childBoard, orderPlayer, deployRow, deployIndex);

                                    childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                                    parent.AppendChild(childBoard, false);
                                }
                            }

                        }
                        
                    }

                    //PLAY CARD ORDER
                    else if(actionCard is IPlayCardExpand playCardCard)
                    {
                        for (int deployRow = 0; deployRow < clonedBoard.CurrentPlayerActions.ImidiateActions[0].Count; deployRow++)
                        {
                            foreach (int deployIndex in clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow])
                            {
                                GameBoard childBoard = (GameBoard)clonedOrderBoard.Clone();
                                actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                                childBoard.CurrentPlayerActions.ClearImidiateActions();

                                if (actionCard is IPlayCardExpand pCC)
                                    pCC.PostPlayCardOrder(childBoard, deployRow, deployIndex);

                                childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                                parent.AppendChild(childBoard, false);
                            }
                        }
                    }
                }
            }
        }
    }
}
