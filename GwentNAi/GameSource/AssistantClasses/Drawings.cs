using GwentNAi.GameSource.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GwentNAi.GameSource.AssistantClasses
{
    public static class Drawings
    {
        static public readonly int windowWidth = 200;
        static public readonly int windowSeparator = 37;

        public static void DrawVictory(int player)
        {
            Console.ForegroundColor = (player == 0 ? ConsoleColor.DarkMagenta : ConsoleColor.DarkCyan);
            int offset = (player == 0 ? 0 : 1);

            Console.SetCursorPosition(67, 7 + 14 * offset);
            Console.Write("            88");
            Console.SetCursorPosition(67, 8 + 14 * offset);
            Console.Write("            \"\"              ,d");
            Console.SetCursorPosition(67, 9 + 14 * offset);
            Console.Write("                            88");
            Console.SetCursorPosition(67, 10 + 14 * offset);
            Console.Write("8b       d8 88  ,adPPYba, MM88MMM ,adPPYba,  8b,dPPYba, 8b       d8");
            Console.SetCursorPosition(67, 11 + 14 * offset);
            Console.Write("`8b     d8' 88 a8\"     \"\"   88   a8\"     \"8a 88P'   \"Y8 `8b     d8'");
            Console.SetCursorPosition(67, 12 + 14 * offset);
            Console.Write(" `8b   d8'  88 8b           88   8b       d8 88          `8b   d8'");
            Console.SetCursorPosition(67, 13 + 14 * offset);
            Console.Write("  `8b,d8'   88 \"8a,   ,aa   88,  \"8a,   ,a8\" 88           `8b,d8'");
            Console.SetCursorPosition(67, 14 + 14 * offset);
            Console.Write("    \"8\"     88  `\"Ybbd8\"'   \"Y888 `\"YbbdP\"'  88             Y88'");
            Console.SetCursorPosition(67, 15 + 14 * offset);
            Console.Write("                                                            d8'");
            Console.SetCursorPosition(67, 16 + 14 * offset);
            Console.Write("                                                           d8'");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DrawDefeat(int player)
        {
            Console.ForegroundColor = (player == 0 ? ConsoleColor.DarkMagenta : ConsoleColor.DarkCyan);
            int offset = (player == 0 ? 0 : 1);

            Console.SetCursorPosition(67, 7 + 14 * offset);
            Console.Write("         88               ad88");
            Console.SetCursorPosition(67, 8 + 14 * offset);
            Console.Write("         88              d8\"                          ,d");
            Console.SetCursorPosition(67, 9 + 14 * offset);
            Console.Write("         88              88                           88");
            Console.SetCursorPosition(67, 10 + 14 * offset);
            Console.Write(" ,adPPYb,88  ,adPPYba, MM88MMM ,adPPYba, ,adPPYYba, MM88MMM");
            Console.SetCursorPosition(67, 11 + 14 * offset);
            Console.Write("a8\"    `Y88 a8P_____88   88   a8P_____88 \"\"     `Y8   88");
            Console.SetCursorPosition(67, 12 + 14 * offset);
            Console.Write("8b       88 8PP\"\"\"\"\"\"\"   88   8PP\"\"\"\"\"\"\" ,adPPPPP88   88");
            Console.SetCursorPosition(67, 13 + 14 * offset);
            Console.Write("\"8a,   ,d88 \"8b,   ,aa   88   \"8b,   ,aa 88,    ,88   88,");
            Console.SetCursorPosition(67, 14 + 14 * offset);
            Console.Write(" `\"8bbdP\"Y8  `\"Ybbd8\"'   88    `\"Ybbd8\"' `\"8bbdP\"Y8   \"Y888");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DrawTie()
        {
            for(int i = 0; i < 2; i++)
            {
                Console.ForegroundColor = (i == 0 ? ConsoleColor.DarkMagenta : ConsoleColor.DarkCyan);

                Console.SetCursorPosition(87, 7 + 14 * i);
                Console.Write("        88");
                Console.SetCursorPosition(87, 8 + 14 * i);
                Console.Write("  ,d    \"\"");
                Console.SetCursorPosition(87, 9 + 14 * i);
                Console.Write("  88");
                Console.SetCursorPosition(87, 10 + 14 * i);
                Console.Write("MM88MMM 88  ,adPPYba,");
                Console.SetCursorPosition(87, 11 + 14 * i);
                Console.Write("  88    88 a8P_____88");
                Console.SetCursorPosition(87, 12 + 14 * i);
                Console.Write("  88    88 8PP\"\"\"\"\"\"\"");
                Console.SetCursorPosition(87, 13 + 14 * i);
                Console.Write("  88,   88 \"8b,   ,aa");
                Console.SetCursorPosition(87, 14 + 14 * i);
                Console.Write("  \"Y888 88  `\"Ybbd8\"'");

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void DrawCrown(GameBoard board)
        {
            if (board.Leader1.victories >= 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.SetCursorPosition(windowWidth - 14, 0);
                Console.Write("      <");
                Console.SetCursorPosition(windowWidth - 14, 1);
                Console.Write("    .::");
                Console.SetCursorPosition(windowWidth - 14, 2);
                Console.Write("@\\\\/W\\/");
                Console.SetCursorPosition(windowWidth - 14, 3);
                Console.Write(" \\\\/^\\/");
                Console.SetCursorPosition(windowWidth - 14, 4);
                Console.Write("  \\_O_<");
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (board.Leader1.victories >= 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.SetCursorPosition(windowWidth - 7, 0);
                Console.Write(">");
                Console.SetCursorPosition(windowWidth - 7, 1);
                Console.Write("::.");
                Console.SetCursorPosition(windowWidth - 7, 2);
                Console.Write("\\/W\\//@");
                Console.SetCursorPosition(windowWidth - 7, 3);
                Console.Write("\\/^\\//");
                Console.SetCursorPosition(windowWidth - 7, 4);
                Console.Write(">_O_/");
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (board.Leader2.victories >= 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(windowWidth - 14, windowSeparator - 5);
                Console.Write("      <");
                Console.SetCursorPosition(windowWidth - 14, windowSeparator - 4);
                Console.Write("    .::");
                Console.SetCursorPosition(windowWidth - 14, windowSeparator - 3);
                Console.Write("@\\\\/W\\/");
                Console.SetCursorPosition(windowWidth - 14, windowSeparator - 2);
                Console.Write(" \\\\/^\\/");
                Console.SetCursorPosition(windowWidth - 14, windowSeparator - 1);
                Console.Write("  \\_O_<");
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (board.Leader2.victories >= 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(windowWidth - 7, windowSeparator - 5);
                Console.Write(">");
                Console.SetCursorPosition(windowWidth - 7, windowSeparator - 4);
                Console.Write("::.");
                Console.SetCursorPosition(windowWidth - 7, windowSeparator - 3);
                Console.Write("\\/W\\//@");
                Console.SetCursorPosition(windowWidth - 7, windowSeparator - 2);
                Console.Write("\\/^\\//");
                Console.SetCursorPosition(windowWidth - 7, windowSeparator - 1);
                Console.Write(">_O_/");
                Console.ForegroundColor = ConsoleColor.White;
            }
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
    }
}
