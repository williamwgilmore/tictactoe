using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    class Program
    {
        private static int _humanWins = 0;
        private static int _computerWins = 0;
        private static int _catsGames = 0;
        static void Main(string[] args)
        {
            // 1 2 3
            // 4 5 6
            // 7 8 9
            var board = new char[9];
            while (true)
            {
                ResetBoard(ref board);
                Play(board);
                Console.WriteLine($"Human wins   : {_humanWins}");
                Console.WriteLine($"Computer wins: {_computerWins}");
                Console.WriteLine($"Cats games   : {_catsGames}");
                Console.WriteLine("Press any key to play again.");
                Console.ReadLine();
            }
        }

        public static void Play(char[] board)
        {
            var isGameOver = false;
            var turn = 0;

            while (!isGameOver)
            {
                PrintBoard(board);
                GetInput(ref board);
                turn++;

                // Check for user win
                isGameOver = CheckForWin(board, 'X');
                if (isGameOver)
                {
                    PrintBoard(board);
                    _humanWins++;

                    Console.WriteLine("Congragulations!");
                }
                else if (turn == 9)
                {
                    isGameOver = true;
                    PrintBoard(board);

                    _catsGames++;
                    Console.WriteLine("Cats game!");
                }
                else
                {
                    ComputerPlay(ref board);
                    turn++;

                    // Check for computer win
                    isGameOver = CheckForWin(board, 'O');
                    if (isGameOver)
                    {
                        PrintBoard(board);

                        _computerWins++;
                        Console.WriteLine("The computer has won this game.");
                    }
                }
            }
        }

        public static void GetInput(ref char[] board)
        {
            var input = Console.ReadLine();
            char charInput;

            var isChar = char.TryParse(input, out charInput);
            if (!isChar)
            {
                GetInput(ref board);
            }

            var validPosition = Array.IndexOf(board, charInput);
            if (validPosition != -1)
            {
                board[validPosition] = 'X';
            }
            else
            {
                Console.WriteLine("Invalid selection, please try again.");
                GetInput(ref board);
            }
        }

        public static void ComputerPlay(ref char[] board)
        {
            // max of 8 moves for the first move of the game
            var moves = new List<(char position, int score, int depth)>();
            var positions = new List<int>();

            foreach (var position in board)
            {
                if (position != 'X' && position != 'O')
                {
                    moves.Add((position, 0, 0));
                }
            }

            var maxScore = -9;
            var bestPosition = -1;
            for (var i = 0; i < moves.Count; i++)
            {
                var score = FindScore(moves[i].position, 'O', board);
                if (score > maxScore)
                {
                    bestPosition = i + 1;
                }
            }

            board[bestPosition] = 'O';
        }

        // Check for fastest win in every position, -1 is a loss, 1 is a win, 0 is a cats game

        // Recursive down the tree
        // Combine the result set - all possible values
        // Changing the board
        public static int FindScore(char position, char letter, char[] board)
        {
            // Not optimal
            if (position == 'O' || position == 'X')
            {
                return 0;
            }

            int intPosition = position - '0' - 1;

            board[intPosition] = letter;
            if (CheckForWin(board, letter))
            {
                if (letter == 'O')
                {
                    return 1;
                }
                else 
                {
                    return -1;
                }
            }
            
            if (CheckForCatsGame(board))
            {
                return 0;
            }

            if (letter == 'O')
            {
                letter = 'X';
            }
            else
            {
                letter = 'O';
            }

            return board.Sum(c => FindScore(c, letter, board));
        }

        public static bool CheckForWin(char[] board, char letter)
        {
            if (board[0] == board[1] && board[1] == board[2] && board[0] == letter
                || board[3] == board[4] && board[4] == board[5] && board[3] == letter
                || board[6] == board[7] && board[7] == board[8] && board[6] == letter
                || board[0] == board[3] && board[3] == board[6] && board[0] == letter
                || board[1] == board[4] && board[4] == board[7] && board[1] == letter
                || board[2] == board[5] && board[5] == board[8] && board[2] == letter
                || board[0] == board[4] && board[4] == board[8] && board[0] == letter
                || board[6] == board[4] && board[4] == board[2] && board[6] == letter)
            {
                return true;
            }

            return false;
        }

        public static bool CheckForCatsGame(char[] board)
        {
            foreach (var position in board)
            {
                if (position != 'X' && position != 'O')
                {
                    return false;
                }
            }

            return true;
        }

        public static void ResetBoard(ref char[] board)
        {
            for (int i = 0; i < board.Length; i++)
            {
                var value = i + 1;
                board[i] = Convert.ToChar(value.ToString());
            }
        }

        public static void PrintBoard(char[] board)
        {
            Console.Clear();

            for (int i = 0; i < board.Length; i++)
            {
                if ((i + 1) % 3 == 0)
                {
                    Console.WriteLine(board[i]);
                    if (i != 8)
                    {
                        Console.WriteLine("---------");
                    }
                }
                else
                {
                    Console.Write($"{board[i]} | ");
                }
            }
        }
    }
}
