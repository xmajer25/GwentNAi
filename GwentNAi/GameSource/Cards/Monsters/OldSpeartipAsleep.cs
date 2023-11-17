using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards.IDefault;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class OldSpeartipAsleep : DefaultCard, ITimer
    {
        private int timer = 3;
        public OldSpeartipAsleep()
        {
            currentValue = 6;
            maxValue = 6;
            shield = 2;
            provisionValue = 6;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "OdlSpeartip:Asleep";
            shortName = "Speart:A";
            descriptors = new List<string>() { "Ogroid" };
            timeToOrder = 0;
            bleeding = 0;
        }

        public override void TakeDemage(int damage, bool lethal, GameBoard board)
        {
            int _excessDamage = shield - damage;
            shield -= damage;
            if (shield < 0) shield = 0;
            currentValue -= _excessDamage;

            Transform(board, false);
        }

        public void Timer(GameBoard board)
        {
            if (timer == 0) Transform(board, true);
            else timer--;
        }

        private void Transform(GameBoard board, bool byTimer)
        {
            for (int i = 0; i < board.Leader1.Board.Count; i++)
            {
                for (int j = 0; j < board.Leader1.Board[i].Count; j++)
                {
                    if (board.Leader1.Board[i][j] == this)
                    {
                        OldSpeartip transformedCard = GetOldSpeartip(board.Leader2.StartingDeck.Cards);
                        if (transformedCard == null) return;

                        board.Leader1.Board[i][j] = transformedCard;
                        if (byTimer)
                        {
                            board.Leader1.Board[i][j].currentValue += 6;
                            board.Leader1.Board[i][j].maxValue += 6;
                        }
                        return;
                    }
                }
            }
            for (int i = 0; i < board.Leader2.Board.Count; i++)
            {
                for (int j = 0; j < board.Leader2.Board[i].Count; j++)
                {
                    if (board.Leader2.Board[i][j] == this)
                    {
                        OldSpeartip transformedCard = GetOldSpeartip(board.Leader2.StartingDeck.Cards);
                        if (transformedCard == null) return; 

                        board.Leader2.Board[i][j] = transformedCard;
                        if (byTimer)
                        {
                            board.Leader2.Board[i][j].currentValue += 6;
                            board.Leader2.Board[i][j].maxValue += 6;
                        }
                        return;
                    }
                }
            }
        }

        private OldSpeartip GetOldSpeartip(List<DefaultCard> Deck)
        {
            foreach(var card in Deck)
            {
                if(card.GetType() == typeof(OldSpeartip))
                {
                    Deck.Remove(card);
                    return new OldSpeartip();
                }
            }
            return null;
        }
    }
}
