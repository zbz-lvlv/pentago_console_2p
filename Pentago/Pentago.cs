using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentago
{

    public class Pentago
    {

        int m_sideLength;
        bool m_isP2Computer;

        char[,] m_gameBoard;

        int m_slotsLeft;

        public Pentago(int sideLength, bool isP2Computer)
        {
            m_sideLength = sideLength;
            m_isP2Computer = isP2Computer;

            m_gameBoard = new char[m_sideLength, m_sideLength]; //1 = Player 1, 2 = Player 2
            m_slotsLeft = m_sideLength * m_sideLength;
        }

        public void start()
        {
            if (!m_isP2Computer)
            {
                Console.WriteLine("In this game, 'A' refers to player A and 'B' refers to player B.");

                printBoard();

                int victoryState = -1;

                while (true)
                {
                    playerMoves(1);
                    printBoard();
                    victoryState = checkVictory(false);
                    if (victoryState != 0)
                        break;

                    playerRotates(1);
                    printBoard();
                    victoryState = checkVictory(true);
                    if (victoryState != 0)
                        break;

                    playerMoves(2);
                    printBoard();
                    victoryState = checkVictory(false);
                    if (victoryState != 0)
                        break;

                    playerRotates(2);
                    printBoard();
                    victoryState = checkVictory(true);
                    if (victoryState != 0)
                        break;
                }

                //Someone has won or the game came to a draw
                if (victoryState == 1)
                {
                    Console.WriteLine("Player A wins!");
                }
                if (victoryState == 2)
                {
                    Console.WriteLine("Player B wins!");
                }
                if (victoryState == 3)
                {
                    Console.WriteLine("It's a draw!");
                }

            }
            else
            {
                Console.WriteLine("In this game, 'A' refers to player A and 'B' refers to player B.");
            }
        }

        public void printBoard()
        {
            Console.WriteLine(String.Empty);
            Console.WriteLine("  1 2 3 4 5 6 ");

            for (int i = 0; i < m_gameBoard.GetLength(0); i++) //Col no.
            {
                Console.Write((i + 1).ToString() + " ");
                for (int i2 = 0; i2 < m_gameBoard.GetLength(1); i2++) //Row no.
                {
                    Console.Write(m_gameBoard[i2, i].ToString() + " ");
                }
                Console.WriteLine(String.Empty);
            }
        }

        public void rotateBoard(bool isClockwise, int startX, int startY, int endX, int endY)
        {
            char[,] unrotated = new char[3, 3];
            for (int i = startX; i <= endX; i++) //Col no.
            {
                for (int i2 = startY; i2 <= endY; i2++) //Row no.
                {
                    unrotated[i - startX, i2 - startY] = m_gameBoard[i, i2];
                }
            }
            char[,] rotated = new char[3, 3];
            
            //Brute force algorithm that only works with 3x3 matrices. Not the best algo but I'm lazy to think of a better one.
            if (isClockwise)
            {
                rotated[0, 0] = unrotated[0, 2];
                rotated[0, 1] = unrotated[1, 2];
                rotated[0, 2] = unrotated[2, 2];
                rotated[1, 0] = unrotated[2, 1];
                rotated[1, 1] = unrotated[1, 1];
                rotated[1, 2] = unrotated[0, 1];
                rotated[2, 0] = unrotated[0, 0];
                rotated[2, 1] = unrotated[1, 0];
                rotated[2, 2] = unrotated[2, 0];
            }
            else
            {
                rotated[0, 0] = unrotated[2, 0];
                rotated[0, 1] = unrotated[1, 0];
                rotated[0, 2] = unrotated[0, 0];
                rotated[1, 0] = unrotated[2, 1];
                rotated[1, 1] = unrotated[1, 1];
                rotated[1, 2] = unrotated[0, 1];
                rotated[2, 0] = unrotated[2, 2];
                rotated[2, 1] = unrotated[1, 2];
                rotated[2, 2] = unrotated[0, 2];
            }

            for (int i = startX; i <= endX; i++) //Col no.
            {
                for (int i2 = startY; i2 <= endY; i2++) //Row no.
                {
                    m_gameBoard[i, i2] = rotated[i - startX, i2 - startY];
                }
            }
        }

        public void playerMoves(int playerNumber)
        {
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Player " + (char)('A' + playerNumber - 1) + ", please enter the coordinates of the piece you'll be playing.");
                Console.WriteLine("For example, 26 will be 2nd column and 6th row.");
                char[] results = Console.ReadLine().ToCharArray();

                if (results.Length != 2)
                {
                    Console.WriteLine("You've entered an invalid coordinate."); continue;
                }

                if (results[0] - '0' < 1 || results[0] - '0' > 6 || results[1] - '0' < 1 || results[1] - '0' > 6)
                {
                    Console.WriteLine("You've entered an invalid coordinate."); continue;
                }

                if (m_gameBoard[results[0] - '0' - 1, results[1] - '0' - 1] != 0)
                {
                    Console.WriteLine("This coordinate has already been used."); continue;
                }

                m_gameBoard[results[0] - '0' - 1, results[1] - '0' - 1] = (char)('A' + playerNumber - 1);
                break;
            }

        }

        public void playerRotates(int playerNumber)
        { 
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Key in the direction you want to rotate the quandrant (a for anticlockwise, c for clockwise), followed by the quandrant number.");
                Console.WriteLine("Quadrant 1 is from (1,1) to (3,3), Quandrant 2 is from (4,1) to (6,3), Quandrant 3 is from (1,4) to (3,6), Quandrant 4 is from (4,4) to (6,6).");
                Console.WriteLine("Eg. a1 to rotate quadrant 1 anticlockwise by 90 degrees.");
                char[] results = Console.ReadLine().ToCharArray();

                if (results.Length != 2)
                {
                    Console.WriteLine("You've entered an invalid rotation."); continue;
                }

                if (results[0] != 'c' && results[0] != 'a' || results[1] - '0' < 1 || results[1] - '0' > 4)
                {
                    Console.WriteLine("You've entered an invalid rotation."); continue;
                }

                bool isClockwise = results[0] == 'c';

                switch (results[1])
                {
                    case '1':
                        rotateBoard(isClockwise, 1 - 1, 1 - 1, 3 - 1, 3 - 1);
                        break;
                    case '2':
                        rotateBoard(isClockwise, 4 - 1, 1 - 1, 6 - 1, 3 - 1);
                        break;
                    case '3':
                        rotateBoard(isClockwise, 1 - 1, 4 - 1, 3 - 1, 6 - 1);
                        break;
                    case '4':
                        rotateBoard(isClockwise, 4 - 1, 4 - 1, 6 - 1, 6 - 1);
                        break;
                }

                break;
            }
        }

        public void computerMoves()
        {

        }

        public int checkVictory(bool afterRotation)
        {

            bool aWins = false;
            bool bWins = false;

            //Horizontal 5
            for(int i = 0; i < m_gameBoard.GetLength(0) - 4; i++)
            {
                for (int i2 = 0; i2 < m_gameBoard.GetLength(1); i2++)
                {

                    if (
                        m_gameBoard[i    , i2] == 'A' &&
                        m_gameBoard[i + 1, i2] == 'A' &&
                        m_gameBoard[i + 2, i2] == 'A' &&
                        m_gameBoard[i + 3, i2] == 'A' &&
                        m_gameBoard[i + 4, i2] == 'A')
                    {
                        aWins = true;
                    }

                    if (
                        m_gameBoard[i    , i2] == 'B' &&
                        m_gameBoard[i + 1, i2] == 'B' &&
                        m_gameBoard[i + 2, i2] == 'B' &&
                        m_gameBoard[i + 3, i2] == 'B' &&
                        m_gameBoard[i + 4, i2] == 'B')
                    {
                        bWins = true;
                    }
                }
            }

            //Vertical 5
            for (int i = 0; i < m_gameBoard.GetLength(0); i++)
            {
                for (int i2 = 0; i2 < m_gameBoard.GetLength(1) - 4; i2++)
                {

                    if (
                        m_gameBoard[i, i2    ] == 'A' &&
                        m_gameBoard[i, i2 + 1] == 'A' &&
                        m_gameBoard[i, i2 + 2] == 'A' &&
                        m_gameBoard[i, i2 + 3] == 'A' &&
                        m_gameBoard[i, i2 + 4] == 'A')
                    {
                        aWins = true;
                    }

                    if (
                        m_gameBoard[i, i2    ] == 'B' &&
                        m_gameBoard[i, i2 + 1] == 'B' &&
                        m_gameBoard[i, i2 + 2] == 'B' &&
                        m_gameBoard[i, i2 + 3] == 'B' &&
                        m_gameBoard[i, i2 + 4] == 'B')
                    {
                        bWins = true;
                    }
                }
            }

            //Diagonal 5 - Downward sloping
            for (int i = 0; i < m_gameBoard.GetLength(0) - 4; i++)
            {
                for (int i2 = 0; i2 < m_gameBoard.GetLength(1) - 4; i2++)
                {

                    if (
                        m_gameBoard[i    , i2    ] == 'A' &&
                        m_gameBoard[i + 1, i2 + 1] == 'A' &&
                        m_gameBoard[i + 2, i2 + 2] == 'A' &&
                        m_gameBoard[i + 3, i2 + 3] == 'A' &&
                        m_gameBoard[i + 4, i2 + 4] == 'A')
                    {
                        aWins = true;
                    }

                    if (
                        m_gameBoard[i    , i2    ] == 'B' &&
                        m_gameBoard[i + 1, i2 + 1] == 'B' &&
                        m_gameBoard[i + 2, i2 + 2] == 'B' &&
                        m_gameBoard[i + 3, i2 + 3] == 'B' &&
                        m_gameBoard[i + 4, i2 + 4] == 'B')
                    {
                        bWins = true;
                    }
                }
            }

            //Diagonal 5 - Upward sloping
            for (int i = 0; i < m_gameBoard.GetLength(0) - 4; i++)
            {
                for (int i2 = 0; i2 < m_gameBoard.GetLength(1) - 4; i2++)
                {

                    if (
                        m_gameBoard[i + 4, i2    ] == 'A' &&
                        m_gameBoard[i + 3, i2 + 1] == 'A' &&
                        m_gameBoard[i + 2, i2 + 2] == 'A' &&
                        m_gameBoard[i + 1, i2 + 3] == 'A' &&
                        m_gameBoard[i    , i2 + 4] == 'A')
                    {
                        aWins = true;
                    }

                    if (
                        m_gameBoard[i + 4, i2    ] == 'B' &&
                        m_gameBoard[i + 3, i2 + 1] == 'B' &&
                        m_gameBoard[i + 2, i2 + 2] == 'B' &&
                        m_gameBoard[i + 1, i2 + 3] == 'B' &&
                        m_gameBoard[i    , i2 + 4] == 'B')
                    {
                        bWins = true;
                    }
                }
            }

            //Board full: Draw
            if(afterRotation && m_slotsLeft == 0)
                return 3;

            if (aWins && bWins)
                return 3;
            if (bWins)
                return 2;
            if (aWins)
                return 1;
            return 0;
        }

    }
}
