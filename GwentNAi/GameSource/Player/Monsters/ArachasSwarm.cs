using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Player.Monsters
{
    public class ArachasSwarm : DefaultLeader
    {
        static int charges = 5;

        public ArachasSwarm() 
        {
            provisionValue = 15;
            leaderName = "ArachasSwarm";
            leaderFaction = "monsters";
            abilityCharges = 5;
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
