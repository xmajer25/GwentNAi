using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IExpand;
using System;
using System.Diagnostics;

namespace GwentNAi.MctsMove
{
    /*
     * Static class containing methods for expansion
     * Expanding both our and enemie moves
     * Expanding swaps, playing cards, orders, leader ability, passing and ending turn
     */
    public static class MCTSExpandChildren
    {
        static readonly int MAX_ROWS = 2; //number of rows on one side of the board
        static readonly int MAX_PLAYERS = 2; // number of players

        /*
         * Expands one random move for the enemie player
         */
        public static void ExpandOneEnemie(MCTSNode parent)
        {
            GameBoard clonedBoard = (GameBoard)parent.Board.Clone();
            MCTSNode enemie = new(parent, clonedBoard);

            //SWAP PLAYERS -> PLAY RANDOM MOVE -> SWAP BACK
            enemie.Board.TurnUpdate();
            enemie.Board.CurrentPlayerActions.GetAllActions(enemie.Board.GetCurrentBoard(), enemie.Board.GetCurrentLeader().Hand, enemie.Board.GetCurrentLeader());
            string enemieMove = MCTSRandomMove.PlayRandomEnemieMove(enemie);
            enemie.Board.TurnUpdate();

            enemie.Board.CurrentPlayerActions.GetAllActions(enemie.Board.GetCurrentBoard(), enemie.Board.GetCurrentLeader().Hand, enemie.Board.GetCurrentLeader());
            parent.AppendChild(enemie.Board, true, enemieMove);
        }

        /*
         * Expands all of the options for swapping cards 
         */
        public static void SwapCard(MCTSNode parent)
        {
            //Children for all possible swaps
            foreach (int index in parent.Board.CurrentPlayerActions.CardSwaps.Indexes.ToList())
            {
                GameBoard clonedBoard = (GameBoard)parent.Board.Clone();
                clonedBoard.GetCurrentLeader().SwapCards(index);
                clonedBoard.CurrentPlayerActions.CardSwaps.CardSwaps--;
                parent.AppendChild(clonedBoard, false, "Swapping " + index + ". card");
            }

            //Child where no swap has been chosen
            GameBoard noSwapBoard = (GameBoard)parent.Board.Clone();
            noSwapBoard.CurrentPlayerActions.CardSwaps.StopSwapping = true;
            parent.AppendChild(noSwapBoard, true, "Stop Swapping");
        }

        /*
         * Expands all of the options for playing a card from hand to the board
         * If card has deploy options -> expands them as well
         */
        public static void PlayCard(MCTSNode parent)
        {
            //END IF CARD WAS PLAYED THIS TURN
            if (parent.Board.GetCurrentLeader().HasPlayedCard)
            {
                return;
            }
            //GET ACTIONS
            parent.Board.CurrentPlayerActions.PlayCardActions.Clear();
            parent.Board.CurrentPlayerActions.GetPlayActions(parent.Board.GetCurrentLeader().Hand);

            //CHECK
            for (int i = 0; i < parent.Board.GetCurrentLeader().Hand.Cards.Count; i++)
            {
                if (parent.Board.GetCurrentLeader().Hand.Cards[i].Name != parent.Board.CurrentPlayerActions.PlayCardActions[i].CardName) throw new Exception("Baad");
            }

            ActionContainer actionContainer = parent.Board.CurrentPlayerActions;

            //PLAY CARD CHILDREN
            for (int cardIndex = 0; cardIndex < actionContainer.PlayCardActions.Count; cardIndex++)
            {

                //EXPAND OPTIONS FOR CARD PLACEMENT 
                DefaultCard playCard = actionContainer.PlayCardActions[cardIndex].ActionCard;
                playCard.GetPlacementOptions(parent.Board);
                List<List<int>> positions = actionContainer.ImidiateActions[0]
                                            .Select(innerList => innerList.ToList()) // Shallow clone of inner list
                                            .Select(innerListClone => innerListClone.Select(item => item).ToList()) // Deep clone of inner list
                                            .ToList();

                //CREATE CHILD FOR EACH OPTION
                for (int row = 0; row < positions.Count; row++)
                {
                    int positionsRowCount = positions[row].Count;
                    //Skip if row is full
                    if (positions[row].Count == 9) continue;

                    for (int posIndex = 0; posIndex < positions[row].Count; posIndex++)
                    {
                        int index = positions[row][posIndex];
                        //PLAY CARD
                        GameBoard clonedBoard = (GameBoard)parent.Board.Clone();
                        clonedBoard.CurrentPlayerActions.ClearImidiateActions();
                        clonedBoard.GetCurrentLeader().PlayCard(cardIndex, row, index, clonedBoard);


                        //DEPLOY OPTIONS CHILDREN
                        if (clonedBoard.CurrentPlayerActions.AreImidiateActionsFull())
                        {
                            //PICK ENEMIE DEPLOY CHILDREN
                            if (playCard is IDeployExpandPickEnemies)
                            {
                                DeployPickEnemieExpand(parent, clonedBoard, row, index);
                            }
                            //PICK ALLY DEPLOY CHILDREN
                            else if (playCard is IDeployExpandPickAlly)
                            {
                                DeployPickAllyExpand(parent, clonedBoard, row, index);
                            }
                            //PICK CARD DEPLOY CHILDREN
                            else if (playCard is IDeployExpandPickCard)
                            {
                                DeployPickCardExpand(parent, clonedBoard, row, index);
                            }

                            clonedBoard.CurrentPlayerActions.ClearImidiateActions();
                        }
                        //NO DEPLOY OPTIONS CARD
                        else
                        {
                            clonedBoard.CurrentPlayerActions.PlayCardActions.Clear();
                            parent.AppendChild(clonedBoard, false, "Playing card " + playCard.Name + " to " + row + "-" + index);
                        }
                    }
                }
            }
        }

        /*
         * Called after a played card has further targets
         * Expands all of the options for selecting an enemie card
         * Appends new children to the list
         */
        private static void DeployPickEnemieExpand(MCTSNode parent, GameBoard originalBoard, int row, int index)
        {
            for (int deployRow = 0; deployRow < MAX_ROWS; deployRow++)
            {
                for (int i = 0; i < originalBoard.CurrentPlayerActions.ImidiateActions[0][deployRow].Count; i++)
                {
                    int deployIndex = originalBoard.CurrentPlayerActions.ImidiateActions[0][deployRow][i];

                    GameBoard clonedDeployBoard = (GameBoard)originalBoard.Clone();
                    DefaultCard playCard = clonedDeployBoard.GetCurrentBoard()[row][index];

                    clonedDeployBoard.CurrentPlayerActions.PlayCardActions.Clear();
                    clonedDeployBoard.CurrentPlayerActions.ClearImidiateActions();

                    if (playCard is IDeployExpandPickEnemies iPE)
                    {
                        iPE.postPickEnemieAbilitiy(clonedDeployBoard, deployRow, deployIndex);
                        clonedDeployBoard.RemoveDestroyedCards();
                    }

                    parent.AppendChild(clonedDeployBoard, false, "Playing (epEnemie)card " + playCard.Name + " to " + row + "-" + index + " targeting: " + deployRow + "-" + deployIndex);
                }
            }
        }

        /*
         * Called after a played card has further targets
         * Expands all of the options for selecting an allied card
         * Appends new children to the list
         */
        private static void DeployPickAllyExpand(MCTSNode parent, GameBoard originalBoard, int row, int index)
        {
            for (int deployRow = 0; deployRow < MAX_ROWS; deployRow++)
            {
                for (int i = 0; i < originalBoard.CurrentPlayerActions.ImidiateActions[0][deployRow].Count; i++)
                {
                    int deployIndex = originalBoard.CurrentPlayerActions.ImidiateActions[0][deployRow][i];
                    GameBoard clonedDeployBoard = (GameBoard)originalBoard.Clone();
                    DefaultCard playCard = clonedDeployBoard.GetCurrentBoard()[row][index];

                    clonedDeployBoard.CurrentPlayerActions.PlayCardActions.Clear();
                    clonedDeployBoard.CurrentPlayerActions.ClearImidiateActions();

                    if (playCard is IDeployExpandPickAlly iPA)
                    {
                        iPA.PostPickAllyAbilitiy(clonedDeployBoard, deployRow, deployIndex);
                        clonedDeployBoard.RemoveDestroyedCards();
                    }

                    parent.AppendChild(clonedDeployBoard, false, "Playing (epAlly)card " + playCard.Name + " to " + row + "-" + index + " targeting: " + deployRow + "-" + deployIndex);
                }
            }
        }

        /*
         * Called after a played card has further targets
         * Expands all of the options for selecting a card
         * Appends new children to the list
         */
        private static void DeployPickCardExpand(MCTSNode parent, GameBoard originalBoard, int row, int index)
        {
            for (int i = 0; i < originalBoard.CurrentPlayerActions.ImidiateActions[0][0].Count; i++)
            {
                int deployIndex = originalBoard.CurrentPlayerActions.ImidiateActions[0][0][i];
                GameBoard clonedDeployBoard = (GameBoard)originalBoard.Clone();
                DefaultCard playCard = clonedDeployBoard.GetCurrentBoard()[row][index];

                clonedDeployBoard.CurrentPlayerActions.PlayCardActions.Clear();
                clonedDeployBoard.CurrentPlayerActions.ClearImidiateActions();

                if (playCard is IDeployExpandPickCard iPC)
                {
                    iPC.postPickCardAbility(clonedDeployBoard, deployIndex);
                    clonedDeployBoard.RemoveDestroyedCards();
                }

                parent.AppendChild(clonedDeployBoard, false, "Playing (epCard)card " + playCard.Name + " to " + row + "-" + index + " targeting: " + deployIndex);
            }
        }

        /*
         * Method for expanding the end of the turn (pass or end)
         */
        public static void PassOrEndTurn(MCTSNode parent)
        {
            parent.Board.CurrentPlayerActions.GetPassOrEndTurn(parent.Board.GetCurrentLeader());

            //PASS CHILD
            if (parent.Board.CurrentPlayerActions.CanPass)
            {
                GameBoard passBoard = (GameBoard)parent.Board.Clone();
                passBoard.CurrentPlayerActions.GetPassOrEndTurn(passBoard.GetCurrentLeader());
                passBoard.CurrentPlayerActions.PassOrEndTurn();
                parent.AppendChild(passBoard, true, "Passing");
            }

            //END CHILD
            if (parent.Board.CurrentPlayerActions.CanEnd)
            {
                GameBoard endBoard = (GameBoard)parent.Board.Clone();
                parent.AppendChild(endBoard, true, "Ending turn");
            }
        }

        /*
         * Method for expanding all order actions
         * For orders that need a target -> expands all of these options as well
         * Append new children to the list
         */
        public static void OrderCard(MCTSNode parent)
        {
            //Return if player will not be able to pass or end turn
            if (parent.Board.GetCurrentLeader().Hand.Cards.Count == 0 && !parent.Board.GetCurrentLeader().HasPlayedCard) return;

            for (int cardIndex = 0; cardIndex < parent.Board.CurrentPlayerActions.OrderActions.Count; cardIndex++)
            {

                GameBoard clonedBoard = (GameBoard)parent.Board.Clone();
                DefaultCard actionCard = clonedBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                clonedBoard.CurrentPlayerActions.ClearImidiateActions();
                if (actionCard is IOrder orderCard)
                {
                    orderCard.Order(clonedBoard);
                }
                if ((actionCard is IOrderExpandPickAll ||
                    actionCard is IOrderExpandPickEnemie ||
                    actionCard is IOrderExpandPickAlly ||
                    actionCard is IPlayCardExpand) &&
                    !clonedBoard.CurrentPlayerActions.AreImidiateActionsFull())
                {
                    continue;
                }

                //ADDITIONAL ACTIONS AFTER ORDER
                if (clonedBoard.CurrentPlayerActions.AreImidiateActionsFull())
                {
                    GameBoard clonedOrderBoard = (GameBoard)clonedBoard.Clone();
                    actionCard = clonedOrderBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;

                    //PICK ENEMIE ORDER
                    if (actionCard is IOrderExpandPickEnemie)
                    {
                        OrderPickEnemieExpand(parent, clonedBoard, clonedOrderBoard, cardIndex);
                    }

                    //PICK ALLY ORDER
                    else if (actionCard is IOrderExpandPickAlly)
                    {
                        OrderPickAllyExpand(parent, clonedBoard, clonedOrderBoard, cardIndex);
                    }

                    //PICK FROM ALL ORDER
                    else if (actionCard is IOrderExpandPickAll)
                    {
                        OrderPickFromAllExpand(parent, clonedBoard, clonedOrderBoard, cardIndex);
                    }

                    //PLAY CARD ORDER
                    else if (actionCard is IPlayCardExpand)
                    {
                        OrderPlayCardExpand(parent, clonedBoard, clonedOrderBoard, cardIndex);
                    }
                }
            }
        }

        /*
         * Called after order targets an enemie card
         * Expands all of the options for the order
         * Appends new children to the list
         */
        private static void OrderPickEnemieExpand(MCTSNode parent, GameBoard originalBoard, GameBoard clonedBoard, int cardIndex)
        {
            for (int deployRow = 0; deployRow < MAX_ROWS; deployRow++)
            {
                for (int i = 0; i < originalBoard.CurrentPlayerActions.ImidiateActions[0][deployRow].Count; i++)
                {
                    int deployIndex = originalBoard.CurrentPlayerActions.ImidiateActions[0][deployRow][i];

                    GameBoard childBoard = (GameBoard)clonedBoard.Clone();
                    DefaultCard actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                    childBoard.CurrentPlayerActions.ClearImidiateActions();

                    if (actionCard is IOrderExpandPickEnemie pEC)
                    {
                        pEC.PostPickEnemieOrder(childBoard, deployRow, deployIndex);
                        childBoard.RemoveDestroyedCards();
                    }


                    childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                    parent.AppendChild(childBoard, false, "Order pick enemie by: " + actionCard.Name + " targeting: " + deployRow + "-" + deployIndex);
                }
            }
        }

        /*
         * Called after order targets an allied card
         * Expands all of the options for the oder
         * Appends new children to the list
         */
        private static void OrderPickAllyExpand(MCTSNode parent, GameBoard originalBoard, GameBoard clonedBoard, int cardIndex)
        {
            for (int deployRow = 0; deployRow < MAX_ROWS; deployRow++)
            {
                for (int i = 0; i < originalBoard.CurrentPlayerActions.ImidiateActions[0][deployRow].Count; i++)
                {
                    int deployIndex = originalBoard.CurrentPlayerActions.ImidiateActions[0][deployRow][i];

                    GameBoard childBoard = (GameBoard)clonedBoard.Clone();
                    DefaultCard actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                    childBoard.CurrentPlayerActions.ClearImidiateActions();

                    if (actionCard is IOrderExpandPickAlly pAC)
                    {
                        pAC.PostPickAllyOrder(childBoard, deployRow, deployIndex);
                        childBoard.RemoveDestroyedCards();
                    }


                    childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                    parent.AppendChild(childBoard, false, "Order pick ally by: " + actionCard.Name + " targeting: " + deployRow + "-" + deployIndex);
                }
            }
        }

        /*
         * Called after order needs target from the whole board
         * Expands all of the options for the order
         * Appends new children to the list
         */
        private static void OrderPickFromAllExpand(MCTSNode parent, GameBoard originalBoard, GameBoard clonedBoard, int cardIndex)
        {
            for (int orderPlayer = 0; orderPlayer < MAX_PLAYERS; orderPlayer++)
            {
                for (int deployRow = 0; deployRow < MAX_ROWS; deployRow++)
                {
                    for (int i = 0; i < originalBoard.CurrentPlayerActions.ImidiateActions[orderPlayer][deployRow].Count; i++)
                    {
                        int deployIndex = originalBoard.CurrentPlayerActions.ImidiateActions[orderPlayer][deployRow][i];

                        GameBoard childBoard = (GameBoard)clonedBoard.Clone();
                        DefaultCard actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                        childBoard.CurrentPlayerActions.ClearImidiateActions();

                        if (actionCard is IOrderExpandPickAll pAC)
                        {
                            pAC.PostPickAllOrder(childBoard, orderPlayer, deployRow, deployIndex);
                            childBoard.RemoveDestroyedCards();
                        }


                        childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                        parent.AppendChild(childBoard, false, "Order pick all by: " + actionCard.Name + " targeting: " + orderPlayer + "-" + deployRow + "-" + deployIndex);
                    }
                }

            }
        }

        /*
         * Called after order has options for summoning a card
         * Expands all of the options for the order
         * Appends new children to the list
         */
        private static void OrderPlayCardExpand(MCTSNode parent, GameBoard originalBoard, GameBoard clonedBoard, int cardIndex)
        {
            for (int deployRow = 0; deployRow < MAX_ROWS; deployRow++)
            {
                for (int i = 0; i < originalBoard.CurrentPlayerActions.ImidiateActions[0][deployRow].Count; i++)
                {
                    int deployIndex = originalBoard.CurrentPlayerActions.ImidiateActions[0][deployRow][i];

                    GameBoard childBoard = (GameBoard)clonedBoard.Clone();
                    DefaultCard actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                    childBoard.CurrentPlayerActions.ClearImidiateActions();

                    if (actionCard is IPlayCardExpand pCC)
                    {
                        pCC.PostPlayCardOrder(childBoard, deployRow, deployIndex);
                        childBoard.RemoveDestroyedCards();
                    }


                    childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                    parent.AppendChild(childBoard, false, "Order play card by: " + actionCard.Name + " targeting: " + deployRow + "-" + deployIndex);
                }
            }
        }

        /*
         * Method for expanding all of the options for leader ability
         */
        public static void LeaderAbility(MCTSNode parent)
        {
            //GET CURRENT ABILITIES
            parent.Board.CurrentPlayerActions.GetLeaderAction(parent.Board.GetCurrentLeader());

            //IF NO ABILITY -> RETURN
            if (parent.Board.CurrentPlayerActions.LeaderActions == null) return;

            //EXPAND POSSIBILITIES
            parent.Board.CurrentPlayerActions.ClearImidiateActions();
            parent.Board.CurrentPlayerActions.LeaderActions(parent.Board);

            if (parent.Board.CurrentPlayerActions.AreImidiateActionsFull())
            {
                for (int row = 0; row < MAX_ROWS; row++)
                {
                    foreach (int col in parent.Board.CurrentPlayerActions.ImidiateActions[0][row])
                    {
                        GameBoard childBoard = (GameBoard)parent.Board.Clone();
                        childBoard.CurrentPlayerActions.ClearImidiateActions();

                        if (childBoard.GetCurrentLeader() is IPlayCardExpand leader)
                        {
                            leader.PostPlayCardOrder(childBoard, row, col);
                            childBoard.RemoveDestroyedCards();
                        }

                        parent.AppendChild(childBoard, false, "Leader ability targeting: " + row + "-" + col);
                    }
                }
            }

        }
    }
}
