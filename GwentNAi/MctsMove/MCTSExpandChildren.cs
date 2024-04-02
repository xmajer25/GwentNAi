using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IExpand;

namespace GwentNAi.MctsMove
{
    public static class MCTSExpandChildren
    {
        static readonly int MAX_ROWS = 2;
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

        public static void PlayCard(MCTSNode parent)
        {
            //PASS IF CARD WAS PLAYED THIS TURN
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
            int playCardCountCheck = actionContainer.PlayCardActions.Count;
            int cardInHandCountCheck = parent.Board.GetCurrentLeader().Hand.Cards.Count;

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
                    if (positions[row].Contains(parent.Board.GetCurrentBoard()[row].Count + 1)) throw new Exception("Inner Error: Positions off by one");

                    for (int posIndex = 0; posIndex < positions[row].Count; posIndex++)
                    {
                        int index = positions[row][posIndex];
                        if (positionsRowCount != positions[row].Count) throw new Exception("Inner Error: position ammount changed");
                        if (index > parent.Board.GetCurrentBoard()[row].Count) throw new Exception("Index out of range");
                        //PLAY CARD
                        GameBoard clonedBoard = (GameBoard)parent.Board.Clone();
                        if (clonedBoard.CurrentPlayerBoard[0].Count != parent.Board.GetCurrentBoard()[0].Count) throw new Exception("Inner Error: Bad cloning");
                        if (clonedBoard.CurrentPlayerBoard[1].Count != parent.Board.GetCurrentBoard()[1].Count) throw new Exception("Inner Error: Bad cloning");
                        //playCard = clonedBoard.CurrentPlayerActions.PlayCardActions[cardIndex].ActionCard;
                        clonedBoard.CurrentPlayerActions.ClearImidiateActions();

                        //BUNCH OF CHECKS BC. THIS THING WONT WORK
                        if (cardInHandCountCheck != parent.Board.GetCurrentLeader().Hand.Cards.Count) throw new Exception("Inner error: Altered cards in hand count");
                        if (playCardCountCheck != actionContainer.PlayCardActions.Count) throw new Exception("Inner Error: Altered play card actions count");
                        if (parent.Board.GetCurrentLeader().Hand.Cards.Count != actionContainer.PlayCardActions.Count) throw new Exception("Inner Error: Match error pca -> hand deck");
                        if (playCard.Name != clonedBoard.GetCurrentLeader().Hand.Cards[cardIndex].Name) throw new Exception("Inner error: wrong card");
                        if (index > clonedBoard.GetCurrentBoard()[row].Count) throw new Exception("Inner Error: index out of range");
                        int CardCountCheck = parent.Board.GetCurrentBoard()[row].Count;

                        clonedBoard.GetCurrentLeader().PlayCard(cardIndex, row, index, clonedBoard);
                        if (index >= clonedBoard.GetCurrentBoard()[row].Count && CardCountCheck != clonedBoard.GetCurrentBoard()[row].Count) throw new Exception("Inner Error: Index out of range");


                        //DEPLOY OPTIONS CHILDREN
                        if (clonedBoard.CurrentPlayerActions.AreImidiateActionsFull())
                        {
                            if (index >= clonedBoard.GetCurrentBoard()[row].Count) throw new Exception("Inner Error: index out of range ");
                            //PICK ENEMIE DEPLOY CHILDREN
                            if (playCard is IDeployExpandPickEnemies)
                            {
                                if (index >= clonedBoard.GetCurrentBoard()[row].Count) throw new Exception("Inner Error: index out of range ");
                                for (int deployRow = 0; deployRow < MAX_ROWS; deployRow++)
                                {
                                    if (index >= clonedBoard.GetCurrentBoard()[row].Count) throw new Exception("Inner Error: index out of range ");
                                    for (int i = 0; i < clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow].Count; i++)
                                    {
                                        if (index >= clonedBoard.GetCurrentBoard()[row].Count) throw new Exception("Inner Error: index out of range ");
                                        int deployIndex = clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow][i];
                                        if (deployIndex >= clonedBoard.GetEnemieBoard()[deployRow].Count) throw new Exception("Inner Error");

                                        GameBoard clonedDeployBoard = (GameBoard)clonedBoard.Clone();
                                        if (index >= clonedBoard.GetCurrentBoard()[row].Count) throw new Exception("Inner Error: index out of range ");
                                        playCard = clonedDeployBoard.GetCurrentBoard()[row][index];

                                        clonedDeployBoard.CurrentPlayerActions.PlayCardActions.Clear();
                                        clonedDeployBoard.CurrentPlayerActions.ClearImidiateActions();

                                        if (playCard is IDeployExpandPickEnemies iPE)
                                        {
                                            iPE.postPickEnemieAbilitiy(clonedDeployBoard, deployRow, deployIndex);
                                            clonedDeployBoard.RemoveDestroyedCards();
                                        }

                                        parent.AppendChild(clonedDeployBoard, false, "Playing (epEnemie)card " + playCard.Name + " to" + row + "-" + index + " targeting: " + deployRow + "-" + deployIndex);
                                    }
                                }
                            }
                            //PICK ALLY DEPLOY CHILDREN
                            else if (playCard is IDeployExpandPickAlly)
                            {
                                for (int deployRow = 0; deployRow < MAX_ROWS; deployRow++)
                                {
                                    for (int i = 0; i < clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow].Count; i++)
                                    {
                                        int deployIndex = clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow][i];

                                        if (deployIndex >= clonedBoard.GetCurrentBoard()[deployRow].Count) throw new Exception("Inner error");

                                        GameBoard clonedDeployBoard = (GameBoard)clonedBoard.Clone();
                                        playCard = clonedDeployBoard.GetCurrentBoard()[row][index];

                                        clonedDeployBoard.CurrentPlayerActions.PlayCardActions.Clear();
                                        clonedDeployBoard.CurrentPlayerActions.ClearImidiateActions();

                                        if (playCard is IDeployExpandPickAlly iPA)
                                        {
                                            iPA.PostPickAllyAbilitiy(clonedDeployBoard, deployRow, deployIndex);
                                            clonedDeployBoard.RemoveDestroyedCards();
                                        }

                                        parent.AppendChild(clonedDeployBoard, false, "Playing (epAlly)card " + playCard.Name + " to" + row + "-" + index + " targeting: " + deployRow + "-" + deployIndex);
                                    }
                                }
                            }
                            //PICK CARD DEPLOY CHILDREN
                            else if (playCard is IDeployExpandPickCard)
                            {
                                for (int i = 0; i < clonedBoard.CurrentPlayerActions.ImidiateActions[0][0].Count; i++)
                                {
                                    int deployIndex = clonedBoard.CurrentPlayerActions.ImidiateActions[0][0][i];
                                    GameBoard clonedDeployBoard = (GameBoard)clonedBoard.Clone();
                                    playCard = clonedDeployBoard.GetCurrentBoard()[row][index];

                                    clonedDeployBoard.CurrentPlayerActions.PlayCardActions.Clear();
                                    clonedDeployBoard.CurrentPlayerActions.ClearImidiateActions();

                                    if (playCard is IDeployExpandPickCard iPC)
                                    {
                                        iPC.postPickCardAbility(clonedDeployBoard, deployIndex);
                                        clonedDeployBoard.RemoveDestroyedCards();
                                    }

                                    parent.AppendChild(clonedDeployBoard, false, "Playing (epCard)card " + playCard.Name + " to" + row + "-" + index + " targeting: " + deployIndex);
                                }
                            }

                            clonedBoard.CurrentPlayerActions.ClearImidiateActions();
                        }
                        //NO DEPLOY OPTIONS CARD
                        else
                        {
                            clonedBoard.CurrentPlayerActions.PlayCardActions.Clear();
                            parent.AppendChild(clonedBoard, false, "Playing card " + playCard.Name + " to" + row + "-" + index);
                        }
                    }
                }
            }
        }

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

                if (clonedBoard.CurrentPlayerActions.AreImidiateActionsFull())
                {
                    GameBoard clonedOrderBoard = (GameBoard)clonedBoard.Clone();
                    actionCard = clonedOrderBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;

                    //PICK ENEMIE ORDER
                    if (actionCard is IOrderExpandPickEnemie)
                    {
                        for (int deployRow = 0; deployRow < clonedBoard.CurrentPlayerActions.ImidiateActions[0].Count; deployRow++)
                        {
                            for (int i = 0; i < clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow].Count; i++)
                            {
                                int deployIndex = clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow][i];

                                GameBoard childBoard = (GameBoard)clonedOrderBoard.Clone();
                                actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                                childBoard.CurrentPlayerActions.ClearImidiateActions();

                                if (actionCard is IOrderExpandPickEnemie pEC)
                                {
                                    pEC.postPickEnemieOrder(childBoard, deployRow, deployIndex);
                                    childBoard.RemoveDestroyedCards();
                                }


                                childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                                parent.AppendChild(childBoard, false, "Order pick enemie by: " + actionCard.Name + "targeting: " + deployRow + "-" + deployIndex);
                            }
                        }
                    }

                    //PICK ALLY ORDER
                    else if (actionCard is IOrderExpandPickAlly)
                    {
                        for (int deployRow = 0; deployRow < clonedBoard.CurrentPlayerActions.ImidiateActions[0].Count; deployRow++)
                        {
                            for (int i = 0; i < clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow].Count; i++)
                            {
                                int deployIndex = clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow][i];

                                GameBoard childBoard = (GameBoard)clonedOrderBoard.Clone();
                                actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                                childBoard.CurrentPlayerActions.ClearImidiateActions();

                                if (actionCard is IOrderExpandPickAlly pAC)
                                {
                                    pAC.PostPickAllyOrder(childBoard, deployRow, deployIndex);
                                    childBoard.RemoveDestroyedCards();
                                }


                                childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                                parent.AppendChild(childBoard, false, "Order pick ally by: " + actionCard.Name + "targeting: " + deployRow + "-" + deployIndex);
                            }
                        }
                    }

                    //PICK FROM ALL ORDER
                    else if (actionCard is IOrderExpandPickAll)
                    {
                        for (int orderPlayer = 0; orderPlayer < clonedBoard.CurrentPlayerActions.ImidiateActions.Count; orderPlayer++)
                        {
                            for (int deployRow = 0; deployRow < clonedBoard.CurrentPlayerActions.ImidiateActions[orderPlayer].Count; deployRow++)
                            {
                                for (int i = 0; i < clonedBoard.CurrentPlayerActions.ImidiateActions[orderPlayer][deployRow].Count; i++)
                                {
                                    int deployIndex = clonedBoard.CurrentPlayerActions.ImidiateActions[orderPlayer][deployRow][i];

                                    GameBoard childBoard = (GameBoard)clonedOrderBoard.Clone();
                                    actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                                    childBoard.CurrentPlayerActions.ClearImidiateActions();

                                    if (actionCard is IOrderExpandPickAll pAC)
                                    {
                                        pAC.postPickAllOrder(childBoard, orderPlayer, deployRow, deployIndex);
                                        childBoard.RemoveDestroyedCards();
                                    }


                                    childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                                    parent.AppendChild(childBoard, false, "Order pick all by: " + actionCard.Name + "targeting: " + orderPlayer + "-" + deployRow + "-" + deployIndex);
                                }
                            }

                        }

                    }

                    //PLAY CARD ORDER
                    else if (actionCard is IPlayCardExpand)
                    {
                        for (int deployRow = 0; deployRow < clonedBoard.CurrentPlayerActions.ImidiateActions[0].Count; deployRow++)
                        {
                            for (int i = 0; i < clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow].Count; i++)
                            {
                                int deployIndex = clonedBoard.CurrentPlayerActions.ImidiateActions[0][deployRow][i];

                                GameBoard childBoard = (GameBoard)clonedOrderBoard.Clone();
                                actionCard = childBoard.CurrentPlayerActions.OrderActions[cardIndex].ActionCard;
                                childBoard.CurrentPlayerActions.ClearImidiateActions();

                                if (actionCard is IPlayCardExpand pCC)
                                {
                                    pCC.PostPlayCardOrder(childBoard, deployRow, deployIndex);
                                    childBoard.RemoveDestroyedCards();
                                }


                                childBoard.CurrentPlayerActions.OrderActions.RemoveAt(cardIndex);
                                parent.AppendChild(childBoard, false, "Order play card by: " + actionCard.Name + "targeting: " + deployRow + "-" + deployIndex);
                            }
                        }
                    }
                }
            }
        }

        public static void LeaderAbility(MCTSNode parent)
        {
            //GET CURRENT ABILITIES
            parent.Board.CurrentPlayerActions.GetLeaderAction(parent.Board.GetCurrentLeader());

            //IF NO ABILITY -> RETURN
            if (parent.Board.CurrentPlayerActions.LeaderActions == null) return;

            //EXPAND POSSIBILITIES
            parent.Board.CurrentPlayerActions.ClearImidiateActions();
            parent.Board.CurrentPlayerActions.LeaderActions(parent.Board);

            //carefull -> so that leader ability playCard methods dont act like they are play from hand...HasPlayedCard is not set to true :)
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
