using GwentNAi.GameSource.Cards.IDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class Drone :DefaultCard, IDoomed
    {
        public Drone() 
        {
            currentValue = 1;
            maxValue = 1;
            provisionValue = 0;
            border = 0;
            type = "unit";
            faction = "monster";
            name = "Drone";
            shortName = "Drone";
            descriptors = new List<string>() { "Insectoid", "Token" };
            timeToOrder = -1;
        }
    }
}
