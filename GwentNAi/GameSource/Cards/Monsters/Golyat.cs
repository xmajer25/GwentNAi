using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using GwentNAi.GameSource.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Golyat : DefaultCard, IDeathwish
    {
        public Golyat()
        {
            currentValue = 12;
            maxValue = 12;
            shield = 0;
            provisionValue = 7;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "Golyat";
            shortName = "Golyat";
            descriptors = new List<string>() { "Ogroid" };
            timeToOrder = 0;
            bleeding = 0;
        }

        public void DeathwishAbility(GameBoard board)
        {
            int player, row;
            (player, row) = GetPlayerAndRow(board);
            DefaultLeader enemieLeader = player == 0 ? board.Leader2 : board.Leader1;
            int enemieRow = (row == 0 ? 1 : 0);
            DefaultCard highestPowerUnit = new();
            foreach(var card in enemieLeader.StartingDeck.Cards)
            {
                if(highestPowerUnit.currentValue < card.currentValue)
                {
                    highestPowerUnit = card;
                }
            }

            if (enemieLeader.Board[enemieRow].Count <= 9 )
            {
                enemieLeader.StartingDeck.Cards.Remove(highestPowerUnit);
                enemieLeader.Board[enemieRow].Add(highestPowerUnit);
            }
            
        }

        private (int, int) GetPlayerAndRow(GameBoard board)
        {
            int foundRow = 0;
            foreach(var row in board.Leader1.Board)
            {
                foreach(var card in row)
                {
                    if(card == this)
                    {
                        return (0, foundRow);
                    }
                }
                foundRow++;
            }

            foundRow = 0;
            foreach (var row in board.Leader2.Board)
            {
                foreach (var card in row)
                {
                    if (card == this)
                    {
                        return (1, foundRow);
                    }
                }
                foundRow++;
            }
            return (-1, -1);
        }
    }
}
