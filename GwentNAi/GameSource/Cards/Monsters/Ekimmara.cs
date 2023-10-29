using GwentNAi.GameSource.Cards.IDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Ekimmara : DefaultCard, IDoomed
    {
        public Ekimmara()
        {
            currentValue = 3;
            maxValue = 3;
            shield = 0;
            provisionValue = 0;
            border = 0;
            type = "unit";
            faction = "monster";
            name = "Ekimmara";
            shortName = "Ekimmara";
            descriptors = new List<string>() { "Vampire", "Token"};
            timeToOrder = 0;
            bleeding = 0;
        }
    }
}
