using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class OldSpeartip: DefaultCard
    {
        public OldSpeartip()
        {
            currentValue = 12;
            maxValue = 12;
            shield = 0;
            provisionValue = 11;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "OdlSpeartip";
            shortName = "Speartip";
            descriptors = new List<string>() { "Ogroid" };
            timeToOrder = 0;
            bleeding = 0;
        }
    }
}
