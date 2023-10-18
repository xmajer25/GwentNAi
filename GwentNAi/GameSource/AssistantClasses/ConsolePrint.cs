using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using System.Text;

namespace GwentNAi.GameSource.AssistantClasses
{
    public static class ConsolePrint
    {
        static public readonly int windowHeight = 60;
        static public readonly int windowWidth = 200;
        static public readonly int windowSeparator = 37;
        

        public static void UpdateBoard(GameBoard board)
        {
            ClearTop();
            Console.ForegroundColor = ConsoleColor.White;
            
            UpdatePoints(board.PointSumP1, board.PointSumP2);

            int rowIndex = 0; 
            int colIndex;

            foreach(var row in board.Leader1.Board)
            {
                colIndex = 5 - (row.Count / 2);
                foreach(var card in row)
                {
                    DrawCard(card, 0, rowIndex, colIndex);
                    colIndex++;
                }
                rowIndex++;
            }

            foreach (var row in board.Leader2.Board)
            {
                colIndex = 5 - (row.Count / 2);
                foreach (var card in row)
                {
                    DrawCard(card, 1, rowIndex, colIndex);
                    colIndex++;
                }
                rowIndex++;
            }
        }

        private static void DrawCard(DefaultCard card, int player, int row, int index)
        {
            int playerRow = (row + 2 * player) + 1;
            if (player == 1) playerRow -= 2;
            int offSet = (player == 0 ? 0 : 2);
            Console.SetCursorPosition(35 + (index * 15), 6 * playerRow + offSet);
            Console.Write("┎────────┒");

            Console.SetCursorPosition(36 + (index * 15), 6 * playerRow + 1 + offSet);
            Console.Write(card.pointValue);

            Console.SetCursorPosition(43 + (index * 15), 6 * playerRow + 1 + offSet);
            Console.Write("x");

            Console.SetCursorPosition(36 + (index * 15), 6 * playerRow + 3 + offSet);
            Console.Write(card.shortName);

            for (int j = 1; j <= 4; j++)
            {
                Console.SetCursorPosition(35 + (index * 15), (6 * playerRow) + j + offSet);
                Console.Write("┃");
                Console.SetCursorPosition(44 + (index * 15), (6 * playerRow) + j + offSet);
                Console.Write("┃");
            }
            Console.SetCursorPosition(35 + (index * 15), 6 * playerRow + 5 + offSet);
            Console.Write("┖────────┚");
        }

        private static void DrawBoardBorders()
        {
      
            Console.SetCursorPosition(25, 18);
            Console.Write(new String('-', windowWidth - 25));
   
            Console.SetCursorPosition(25, 19);
            Console.Write(new String('-', windowWidth - 25));
            
            for (int i = 0; i < windowSeparator; i++)
            {
                Console.SetCursorPosition(25, i);
                Console.Write("|");
                Console.SetCursorPosition(windowWidth - 16, i);
                Console.Write("|");
            }
        }

        private static void UpdatePoints(int player1Points, int player2Points)
        {
            string p1StringPoints = player1Points.ToString();
            Console.SetCursorPosition((windowWidth - 12) + (3 - p1StringPoints.Length), 12);
            Console.Write(player1Points);

            string p2StringPoints = player2Points.ToString();
            Console.SetCursorPosition((windowWidth - 12) + (3 - p2StringPoints.Length), 24);
            Console.Write(player2Points);
        }

        private static void DrawPointBoxes()
        {
            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(windowWidth - 13, 11 + 12 * i);
                Console.Write("╔════╗");
                Console.SetCursorPosition(windowWidth - 13, 12 + 12 * i);
                Console.Write("╠    ╣");
                Console.SetCursorPosition(windowWidth - 13, 13 + 12 * i);
                Console.Write("╚════╝");
            }
        }

        public static void DrawLeader(int playerNum, string leaderFaction)
        {
            Console.ForegroundColor = (playerNum == 0 ? ConsoleColor.DarkMagenta : ConsoleColor.DarkCyan);
            switch (leaderFaction)
            {
                case "monsters":
                    DrawMonster(playerNum);
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DrawStaticElements()
        {
            DrawSeparator();
            DrawBoardBorders();
            DrawPointBoxes();
        }


        public static void DrawMonster(int playerNum)
        {
            Console.SetCursorPosition(1, 0 + 20 * playerNum);
            Console.Write("   M O N S T E R S");
            Console.SetCursorPosition(3, 1 + 20 * playerNum);
            Console.Write("      |\\/\\/|");
            Console.SetCursorPosition(3, 2 + 20 * playerNum);
            Console.Write("      |====|");
            Console.SetCursorPosition(3, 3 + 20 * playerNum);
            Console.Write("      |    |");
            Console.SetCursorPosition(3, 4 + 20 * playerNum);
            Console.Write("  .-;`\\..../`;-.");
            Console.SetCursorPosition(3, 5 + 20 * playerNum);
            Console.Write(" /  | ___|___|  \\");
            Console.SetCursorPosition(3, 6 + 20 * playerNum);
            Console.Write("|   / __/|\\__\\   |");
            Console.SetCursorPosition(3, 7 + 20 * playerNum);
            Console.Write(";--'\\ __/|\\__/\\--;");
            Console.SetCursorPosition(3, 8 + 20 * playerNum);
            Console.Write("<__>, __/|\\__,<__>");
            Console.SetCursorPosition(3, 9 + 20 * playerNum);
            Console.Write("|  |/        \\|  |");
            Console.SetCursorPosition(3, 10 + 20 * playerNum);
            Console.Write("\\::/|========|\\::/");
            Console.SetCursorPosition(3, 11 + 20 * playerNum);
            Console.Write("|||\\|        |/|||");
            Console.SetCursorPosition(3, 12 + 20 * playerNum);
            Console.Write("''' |___/\\___| '''");
            Console.SetCursorPosition(3, 13 + 20 * playerNum);
            Console.Write("     \\_ || _/");
            Console.SetCursorPosition(3, 14 + 20 * playerNum);
            Console.Write("     <_ >< _>");
            Console.SetCursorPosition(3, 15 + 20 * playerNum);
            Console.Write("     |  ||  |");
            Console.SetCursorPosition(3, 16 + 20 * playerNum);
            Console.Write("     |  ||  |");
        }

        public static void Init()
        {
            Console.SetBufferSize(windowWidth, windowHeight);
            Console.SetWindowSize(windowWidth, windowHeight);
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();
        }

        private static void DrawSeparator()
        {
            Console.SetCursorPosition(0, windowSeparator);
            Console.WriteLine(new String('#', windowWidth));
        }
    

        public static void ClearBottom()
        {
            Console.SetCursorPosition(0, windowSeparator + 1);
            for(int i = windowSeparator + 1; i < windowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine(new String(' ', windowWidth));
            }
            Console.SetCursorPosition(0, windowHeight - 1);
            Console.Write(new String(' ', windowWidth));
        }

        public static void ClearTop() 
        {
            for(int i = 7; i < 18; i++)
            {
                Console.SetCursorPosition(26, i);
                Console.WriteLine(new String(' ', windowWidth - 17 - 25));
            }
            for(int i  = 20; i < windowSeparator - 5; i++)
            {
                Console.SetCursorPosition(26, i);
                Console.WriteLine(new String(' ', windowWidth - 17 - 25));
            }

            Console.SetCursorPosition(windowWidth - 12, 12);
            Console.Write("    ");

            Console.SetCursorPosition(windowWidth - 12, 24);
            Console.Write("    ");
        }

        public static void AskForLeaderAbility(int choosingPlayerNumber)
        {
            Console.SetCursorPosition(0, windowSeparator + 1);
            Console.WriteLine("Select Leader for Player #" + choosingPlayerNumber + ". (by typing one of the options listed)");
            Console.WriteLine("1.) ArachasSwarm\t2.) BloodScent");
        }

        public static void AskForDeck(int choosingPlayerNumber)
        {
            Console.SetCursorPosition(0, windowSeparator + 1);
            Console.WriteLine("Select Deck for Player #" + choosingPlayerNumber);
            Console.WriteLine("1.) Renfri");
        }

        public static int GetCursorY()
        {
            int Ypos;
            (_, Ypos) = Console.GetCursorPosition();
            return Ypos;
        }
    }
}
