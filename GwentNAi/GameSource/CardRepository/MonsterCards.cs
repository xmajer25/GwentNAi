using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.CardRepository
{
    public static class MonsterCards
    {
        private static readonly List<DefaultCard> Cards = new List<DefaultCard>()
        {
            new AddaStriga(), new Brewess(), new Ekimmara(), new Ghoul(), new Golyat(), new Griffin(), new IceGiant(),
            new Katakan(), new Nekker(), new NekkerWarrior(), new OldSpeartip(), new OldSpeartipAsleep(), new Ozzrel(), 
            new Protofleder(), new Weavess(), new Whispess(), new WildHuntHound(), new WildHuntRider(), new Wyvern()
        };

        public static DefaultCard GetRandomCard()
        {
            Random random = new Random();
            int randomIndex = random.Next(Cards.Count);
            return Cards[randomIndex];
        }
    }
}
