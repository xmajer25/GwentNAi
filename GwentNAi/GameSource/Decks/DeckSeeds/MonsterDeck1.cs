﻿using GwentNAi.GameSource.Cards.Monsters;
using GwentNAi.GameSource.Cards.Neutral;
using GwentNAi.GameSource.Cards.Syndicate;
/**
 * used deck:
 * https://www.playgwent.com/en/decks/cc7d610d96aefe4fd09f2843e9d79a1f
 * 29.11 13:00
 */
namespace GwentNAi.GameSource.Decks.DeckSeeds
{
    /*
     * Child class for defing a deck used for gameplay
     */
    public class MonsterDeck1 : DefaultDeck
    {
        public MonsterDeck1()
        {
            Cards = new()
            {
                new AddaStriga(), new Protofleder(), new Ozzrel(), new Golyat(),
                new PugoBoomBreaker(), new Whispess(), new Weavess(), new OldSpeartipAsleep(),
                new Griffin(), new Griffin(), new IceGiant(), new IceGiant(), new WildHuntRider(),
                new WildHuntRider(), new Wyvern(), new Wyvern(), new Ghoul(), new Ghoul(),
                new NekkerWarrior(),  new NekkerWarrior(), new WildHuntHound(), new Nekker(), new Nekker(), new GeraltOfRivia(), new GeraltOfRivia()
            };
            Name = "Monster Deck 1";
        }
    }
}
