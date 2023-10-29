using GwentNAi.GameSource.Cards.IDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GwentNAi.GameSource.Cards.Monsters
{
    public class WoodlandSpirit : DefaultCard, IDoomed
    {
        public WoodlandSpirit()
        {
            currentValue = 9;
            maxValue = 9;
            shield = 0;
            provisionValue = 0;
            border = 1;
            type = "unit";
            faction = "monster";
            name = "WoodlandSpirit";
            shortName = "Woodland";
            descriptors = new List<string>() { "Relict", "Token" };
            timeToOrder = -1;
            bleeding = 0;
        }
    }
}
