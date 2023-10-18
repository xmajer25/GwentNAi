using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Player.Monsters
{
    public class BloodScent : DefaultLeader
    {
        static int charges = 3;
        public BloodScent() 
        {
            provisionValue = 15;
            leaderName = "BloodScent";
            leaderFaction = "monsters";
            abilityCharges = 3;
            hasPassed = false;
        }
        public override void Order()
        {
            if (charges == 0) return;
            charges--;
            Console.WriteLine("Current charges: " + charges);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
