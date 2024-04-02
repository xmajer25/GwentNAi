using GwentNAi.GameSource.Board;
using GwentNAi.GameSource.Cards;
using GwentNAi.GameSource.Cards.IDefault;
using System.Reflection;
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
            UpdateGraveyard(board.Leader1.Graveyard.Cards.Count, board.Leader2.Graveyard.Cards.Count);
            UpdateDeck(board.Leader1.StartingDeck.Cards.Count, board.Leader2.StartingDeck.Cards.Count);
            UpdateHand(board.Leader1.Hand.Cards.Count, board.Leader2.Hand.Cards.Count);

            int rowIndex = 0;
            int colIndex;

            foreach (var row in board.Leader1.Board)
            {
                colIndex = 5 - (row.Count / 2);
                foreach (var card in row)
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
            Console.Write(card.CurrentValue);

            if (card.Bleeding > 0)
            {
                Console.SetCursorPosition(36 + (index * 15), 6 * playerRow + 1 + offSet + 1);
                Console.Write("(-" + card.Bleeding + ")");
            }
            if (card.Shield > 0)
            {
                Console.SetCursorPosition(43 + (index * 15), 6 * playerRow + 1 + offSet);
                Console.Write(card.Shield);
            }

            Console.SetCursorPosition(36 + (index * 15), 6 * playerRow + 3 + offSet);
            Console.Write(card.ShortName);

            for (int j = 1; j <= 4; j++)
            {
                Console.SetCursorPosition(35 + (index * 15), (6 * playerRow) + j + offSet);
                Console.Write("┃");
                Console.SetCursorPosition(44 + (index * 15), (6 * playerRow) + j + offSet);
                Console.Write("┃");
            }

            if (card is ITimer)
            {
                Type type = card.GetType();
                FieldInfo chargesField = type.GetField("timer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                Console.SetCursorPosition(38 + (index * 15), (6 * playerRow) + 4 + offSet);
                Console.Write((int)chargesField.GetValue(card));
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

        private static void DrawGraveyardBorder()
        {
            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(46, 0 + 33 * i);
                Console.Write("Graveyard");
                Console.SetCursorPosition(46, 1 + 33 * i);
                Console.Write(" ╔╦╦╦╦╦╗");
                Console.SetCursorPosition(46, 2 + 33 * i);
                Console.Write(" ╠     ╣");
                Console.SetCursorPosition(46, 3 + 33 * i);
                Console.Write(" ╚╩╩╩╩╩╝");
            }
        }

        private static void UpdateGraveyard(int p1GraveyardCount, int p2GraveyardCount)
        {
            Console.SetCursorPosition(50, 2);
            Console.Write(p1GraveyardCount);

            Console.SetCursorPosition(50, 35);
            Console.Write(p2GraveyardCount);
        }

        private static void DrawDeckBorder()
        {
            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(146, 0 + 33 * i);
                Console.Write(" Deck");
                Console.SetCursorPosition(146, 1 + 33 * i);
                Console.Write("╔╦╦╦╦╦╗");
                Console.SetCursorPosition(146, 2 + 33 * i);
                Console.Write("╠     ╣");
                Console.SetCursorPosition(146, 3 + 33 * i);
                Console.Write("╚╩╩╩╩╩╝");
            }
        }

        private static void UpdateDeck(int p1DeckCount, int p2DeckCount)
        {
            Console.SetCursorPosition(149, 2);
            Console.Write(p1DeckCount);

            Console.SetCursorPosition(149, 35);
            Console.Write(p2DeckCount);
        }

        private static void DrawHandBorder()
        {
            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(96, 0 + 33 * i);
                Console.Write("     Hand");
                Console.SetCursorPosition(96, 1 + 33 * i);
                Console.Write("╔╦╦╦╦╦╦╦╦╦╦╦╦╦╗");
                Console.SetCursorPosition(96, 2 + 33 * i);
                Console.Write("╠             ╣");
                Console.SetCursorPosition(96, 3 + 33 * i);
                Console.Write("╚╩╩╩╩╩╩╩╩╩╩╩╩╩╝");
            }
        }

        private static void UpdateHand(int p1HandCount, int p2HandCount)
        {
            Console.SetCursorPosition(103, 2);
            Console.Write(p1HandCount);

            Console.SetCursorPosition(103, 35);
            Console.Write(p2HandCount);
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
                Console.ForegroundColor = (i == 0 ? ConsoleColor.DarkMagenta : ConsoleColor.DarkCyan);
                Console.SetCursorPosition(windowWidth - 13, 11 + 12 * i);
                Console.Write("╔════╗");
                Console.SetCursorPosition(windowWidth - 13, 12 + 12 * i);
                Console.Write("╠    ╣");
                Console.SetCursorPosition(windowWidth - 13, 13 + 12 * i);
                Console.Write("╚════╝");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DrawLeader(int playerNum, string leaderFaction)
        {
            Console.ForegroundColor = (playerNum == 0 ? ConsoleColor.DarkMagenta : ConsoleColor.DarkCyan);
            switch (leaderFaction)
            {
                case "monsters":
                    Drawings.DrawMonster(playerNum);
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DrawStaticElements()
        {
            DrawSeparator();
            DrawBoardBorders();
            DrawPointBoxes();
            DrawDeckBorder();
            DrawGraveyardBorder();
            DrawHandBorder();
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
            for (int i = windowSeparator + 1; i < windowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine(new String(' ', windowWidth));
            }
            Console.SetCursorPosition(0, windowHeight - 1);
            Console.Write(new String(' ', windowWidth));
        }


        public static void ClearTop()
        {
            for (int i = 6; i < 18; i++)
            {
                Console.SetCursorPosition(26, i);
                Console.WriteLine(new String(' ', windowWidth - 17 - 25));
            }
            for (int i = 20; i < windowSeparator - 5; i++)
            {
                Console.SetCursorPosition(26, i);
                Console.WriteLine(new String(' ', windowWidth - 17 - 25));
            }

            Console.SetCursorPosition(windowWidth - 12, 12);
            Console.Write("    ");

            Console.SetCursorPosition(windowWidth - 12, 24);
            Console.Write("    ");

            Console.SetCursorPosition(149, 2);
            Console.Write("   ");
            Console.SetCursorPosition(149, 35);
            Console.Write("   ");

            Console.SetCursorPosition(103, 2);
            Console.Write("   ");
            Console.SetCursorPosition(103, 35);
            Console.Write("   ");

            Console.SetCursorPosition(50, 2);
            Console.Write("   ");
            Console.SetCursorPosition(50, 35);
            Console.Write("   ");
        }

        public static void AskForLeaderAbility(int choosingPlayerNumber)
        {
            Console.SetCursorPosition(0, windowSeparator + 1);
            Console.WriteLine("Select Leader for Player #" + choosingPlayerNumber + ". (by typing one of the options listed)");
            Console.WriteLine("1.) ArachasSwarm\t2.) BloodScent\t3.)ForceOfNature");
        }

        public static void AskForDeck(int choosingPlayerNumber)
        {
            Console.SetCursorPosition(0, windowSeparator + 1);
            Console.WriteLine("Select Deck for Player #" + choosingPlayerNumber);
            Console.WriteLine("1.) TestDeck\t2.)MonsterDeck1");
        }

        public static int GetCursorY()
        {
            int Ypos;
            (_, Ypos) = Console.GetCursorPosition();
            return Ypos;
        }
    }
}
