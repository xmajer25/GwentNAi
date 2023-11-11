using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class IceGiant : DefaultCard
    {
        public IceGiant()
        {
            currentValue = 7;
            maxValue = 7;
            shield = 0;
            provisionValue = 5;
            border = 0;
            type = "unit";
            faction = "monster";
            name = "Ice Giant";
            shortName = "IceGiant";
            descriptors = new List<string>() { "Ogroid" };
            timeToOrder = 0;
            bleeding = 0;
        }
    }
}
