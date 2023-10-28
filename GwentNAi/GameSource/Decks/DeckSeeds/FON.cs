using GwentNAi.GameSource.Cards.Monsters;
using GwentNAi.GameSource.Cards.Syndicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
/**
 * used deck:
 * https://www.playgwent.com/en/decks/cc7d610d96aefe4fd09f2843e9d79a1f
 */
namespace GwentNAi.GameSource.Decks.DeckSeeds
{
    public class FON : Deck
    {
        public FON()
        {
            Cards = new()
            {
                new AddaStriga(), new Katakan(), new Protofleder(), new Ozzrel(), new Golyat(),
                new PugoBoomBreaker()
            };
            Name = "Force Of Nature";
        }
    }
}
