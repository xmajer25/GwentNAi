﻿using GwentNAi.GameSource.Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Neutral
{
    public class Renfri : DefaultCard
    {
        public Renfri()
        {
            currentValue = 5;
            maxValue = 5;
            shield = 0;
            provisionValue = 14;
            border = 1;
            type = "unit";
            faction = "neutral";
            name = "Renfri";
            shortName = "Renfri";
            descriptors = new List<string>(){ "Human", "Cursed", "Bandit" };
            timeToOrder = 0;
            bleeding = 0;
        }

        public static void Deploy(Deck deck)
        {
            throw new NotImplementedException();
        }
    }
}
