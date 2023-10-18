
using System.Text.RegularExpressions;

namespace GwentNAi.HumanMove
{
    public static class HumanConsoleGet
    {
        static readonly string IntPattern = @"\d+";
        public static string GetHumanAction(int[] maxActionId, bool canPass, bool canEnd)
        {
            string action = string.Empty;
            Console.WriteLine("Enter your action of choice (by writing one play action and \'n\' order actions or type \'pass\' to skip round");
            Console.WriteLine("Example of valid action: \'p1 or o2 or l\'");
            Console.WriteLine("");
            while (action == string.Empty)
            {
                action = Console.ReadLine();
                if (!IsActionValid(action, maxActionId, canPass, canEnd))
                {
                    Console.WriteLine("This action is not valid, try again:");
                    action = string.Empty;
                }
            }

            return action;
        }

        private static bool IsActionValid(string action, int[] maxActionId, bool canPass, bool canEnd)
        {
            if(action == null) return false;
            if(action == string.Empty) return false;

            if (action == "pass" && canPass) return true;
            else if (action == "pass" && !canPass) return false;
            if (action == "end" && canEnd) return true;

            Match match;
            int cardIndex;

            switch (action[0])
            {
                case 'p':
                    match = Regex.Match(action, IntPattern);
                    cardIndex = int.Parse(match.Value);
       
                    if (cardIndex > maxActionId[0]) return false;
                    if (action.Length > 3) return false; 
                    if (!Char.IsDigit(action[1])) return false;
                    break;
                case 'o':
                    match = Regex.Match(action, IntPattern);
                    cardIndex = int.Parse(match.Value);

                    if (cardIndex > maxActionId[1]) return false;
                    if (action.Length != 2) return false;
                    if (!Char.IsDigit(action[1])) return false;
                    break;
                case 'l':
                    if(action.Length != 1) return false;
                    break;
                default:
                    return false;
            }
            return true;
        }

        public static int[] GetPositionForCard(List<List<int>> playIndexes)
        {
            int row, pos;
            string positionIndex = Console.ReadLine();
            string[] positionIndexes = positionIndex.Split('-');

            if (positionIndexes.Length != 2) return GetPositionForCard(playIndexes);
            try
            {
                row = Convert.ToInt32(positionIndexes[0]);
                pos = Convert.ToInt32(positionIndexes[1]);
            }
            catch
            {
                return GetPositionForCard(playIndexes);
            }

            if(row != 0 && row != 1) return GetPositionForCard(playIndexes);
            if (playIndexes[row].Count < pos + 1) return GetPositionForCard(playIndexes);

            return new int[] { row, pos };
        }
    }
}
