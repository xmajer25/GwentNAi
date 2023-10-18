using GwentNAi.GameSource.Board;
using System.Text;

namespace GwentNAi.GameSource.AssistantClasses
{
    public static class ConsolePrint
    {
        static public readonly int windowHeight = 50;
        static public readonly int windowWidth = 200;
        static public readonly int windowSeparator = 37;


        static public readonly int cardWidth = 10;
        static public readonly int cardHeight = 10;

        public static void CheckTopDistribution()
        {
            for (int k = 1; k <= 4; k++)
            {
                for (int i = 0; i < 9; i++)
                {
                    Console.SetCursorPosition(35 + (i * 15), 6 * k + 1);
                    Console.Write("┎────────┒");

                    Console.SetCursorPosition(36 + (i * 15), 6 * k + 2);
                    Console.Write("12");

                    Console.SetCursorPosition(43 + (i * 15), 6 * k + 2);
                    Console.Write("1");

                    Console.SetCursorPosition(36 + (i * 15), 6 * k + 4);
                    Console.Write("Geralt");

                    for (int j = 1; j <= 4; j++)
                    {
                        Console.SetCursorPosition(35 + (i * 15), (6 * k) + j + 1);
                        Console.Write("┃");
                        Console.SetCursorPosition(44 + (i * 15), (6 * k) + j + 1);
                        Console.Write("┃");
                    }
                    Console.SetCursorPosition(35 + (i * 15), 6 * k + 6);
                    Console.Write("┖────────┚");
                }
            }
        }

        private static void DrawBoardBorders()
        {
            for (int i = 1; i <= 5; i += 2)
            {
                Console.SetCursorPosition(25, i * 6);
                Console.Write(new String('-', windowWidth - 25));
                if (i == 3)
                {
                    Console.SetCursorPosition(25, i * 6 + 1);
                    Console.Write(new String('-', windowWidth - 25));
                }
            }

            for (int i = 0; i < windowSeparator; i++)
            {
                Console.SetCursorPosition(25, i);
                Console.Write("|");
                Console.SetCursorPosition(windowWidth - 16, i);
                Console.Write("|");
            }
        }

        public static void UpdatePoints(int player1Points, int player2Points)
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
            switch (leaderFaction)
            {
                case "monsters":
                    DrawMonster(playerNum);
                    break;
            }
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
            Console.SetCursorPosition(0, 0);
            for(int i = 0; i < windowSeparator - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine(new String(' ', windowWidth));
            }
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
    }
}
