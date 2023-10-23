using GwentNAi.GameSource.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.Player.Monsters
{
    public class BloodScent : DefaultLeader
    {
        public BloodScent() 
        {
            provisionValue = 15;
            leaderName = "BloodScent";
            leaderFaction = "monsters";
            abilityCharges = 3;
            hasPassed = false;
        }
        public override void Order(GameBoard board)
        {
            if (abilityCharges == 0) return;
            abilityCharges--;
            Console.WriteLine("Current charges: " + abilityCharges);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
