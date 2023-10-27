
using GwentNAi.GameSource.AssistantClasses;
using System.Text.RegularExpressions;

namespace GwentNAi.HumanMove
{
    public static class HumanConsoleGet
    {
        static readonly string IntPattern = @"\d+";
        public static string GetHumanAction(int[] maxActionId, bool canPass, bool canEnd, bool hasLeaderAbility)
        {
            Console.ForegroundColor = HumanConsolePrint.currentColor;

            string action = string.Empty;
            Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
            Console.Write("Enter your action of choice:");
            while (action == string.Empty)
            {
                action = Console.ReadLine();
                if (!IsActionValid(action, maxActionId, canPass, canEnd, hasLeaderAbility))
                {
                    Console.SetCursorPosition(0, ConsolePrint.GetCursorY() + 1);
                    Console.WriteLine("This action is not valid, try again:");
                    action = string.Empty;
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            ConsolePrint.ClearBottom();
            return action;
        }

        private static bool IsActionValid(string action, int[] maxActionId, bool canPass, bool canEnd, bool hasLeaderAbility)
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
                    if(!hasLeaderAbility) return false;
                    if(action.Length != 1) return false;
                    break;
                default:
                    return false;
            }
            return true;
        }

        public static int[] GetPositionFromWholeBoard(List<List<List<int>>> board)
        {
            int player, row, pos;

            Console.ForegroundColor = HumanConsolePrint.currentColor;
            string positionIndex = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;

            string[] positionIndexes = positionIndex.Split('-');
            if (positionIndexes.Length != 3) return GetPositionFromWholeBoard(board);
            try
            {
                player = Convert.ToInt32(positionIndexes[0]);
                row = Convert.ToInt32(positionIndexes[1]);
                pos = Convert.ToInt32(positionIndexes[2]);
            }
            catch
            {
                return GetPositionFromWholeBoard(board);
            }

            if(player != 0 && player != 1) return GetPositionFromWholeBoard(board);
            if (row != 0 && row != 1) return GetPositionFromWholeBoard(board);
            if (!board[player][row].Contains(pos)) return GetPositionFromWholeBoard(board);
            ConsolePrint.ClearBottom();

            return new int[] {player, row, pos };
        }

        public static int[] GetPositionForCard(List<List<int>> playIndexes)
        {
            int row, pos;

            Console.ForegroundColor = HumanConsolePrint.currentColor;
            string positionIndex = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;


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
            ConsolePrint.ClearBottom();
            return new int[] { row, pos };
        }

        public static int GetIndex(List<int> indexes)
        {
            Console.ForegroundColor = HumanConsolePrint.currentColor;
            string indexStr = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            if (indexStr == "end")
            {
                ConsolePrint.ClearBottom();
                return -1;
            }
            int index;

            try
            {
                index = Convert.ToInt32(indexStr);
            }
            catch
            {
                return GetIndex(indexes);
            }

            if(!indexes.Contains(index)) return GetIndex(indexes);
            ConsolePrint.ClearBottom();
            return index;
        }
    }
}
