using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TetrisGame
{
    /// <summary>
    /// Tetris main program.
    /// </summary>
    public class TetrisMainProgram
    {
        /// <summary>
        /// Tetris Game
        /// </summary>
        private static TetrisContainerClass _tetriscontainer;

        /// <summary>
        /// The block Mover Thread
        /// </summary>
        private static Thread _blockMover;

        /// <summary>
        /// Locks the drawing of tetris block
        /// </summary>
        private static bool _tetrisDrawLocker;

        /// <summary>
        /// Main entrance of the tetris application
        /// </summary>
        /// <param name="args">Args</param>
        /// <returns>0</returns>
        static int Main(string[] args)
        {
            //preparing Console
            Console.Clear();
            Console.CursorVisible = false;

            _tetrisDrawLocker = false;
            _tetriscontainer = new TetrisContainerClass(Constants.TetrisBoardWidth, Constants.TetrisBoardHeight);

            //Start the Game and run the "Block Mover" Thread
            _tetriscontainer.GetTetrisBlock();

            _blockMover = new Thread(new ThreadStart(StepForward));
            _blockMover.IsBackground = true;
            _blockMover.Start();

            // Get the inputs from the keyboard
            while (_tetriscontainer.IsGameRunning)
            {
                // Delay added to make the block looks moving
                Thread.Sleep(20);
                if (Console.KeyAvailable)
                {
                    lock (_tetriscontainer)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            // Move the Tetris block to the left by pressing on "A" key.
                            case ConsoleKey.A:
                                _tetriscontainer.keyPress(TetrisContainerClass.InputKey.Left);
                                break;

                            // Move the Tetris block to the right by pressing on "D" key.
                            case ConsoleKey.D:
                                _tetriscontainer.keyPress(TetrisContainerClass.InputKey.Right);
                                break;

                            // Rotate the Tetris block counter clockwise by pressing on "W" key.
                            case ConsoleKey.W:
                                _tetriscontainer.keyPress(TetrisContainerClass.InputKey.CounterClockwise);
                                break;

                            // Rotate the Tetris block clockwise by pressing on "S" key.
                            case ConsoleKey.S:
                                _tetriscontainer.keyPress(TetrisContainerClass.InputKey.Clockwise);
                                break;
                            default:
                                break;
                        }

                        DrawTetrisBoard();
                    }
                }
            }

            //Wait for thread to end and then End the Game and close
            Thread.Sleep(500);
            Console.Clear();

            Console.Write("Game Over!");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
            Console.CursorVisible = true;
            return 0;
        }

        /// <summary>
        /// Steps the game forward
        /// </summary>
        static void StepForward()
        {
            while (_tetriscontainer.IsGameRunning)
            {
                _tetriscontainer.NextStep();
                DrawTetrisBoard();

                // Delay added to move the tetris block down slowly.
                Thread.Sleep(Constants.DelayForMovingDownTetrisBlock);
            }
        }

        /// <summary>
        /// Draws the tetris board.
        /// </summary>
        private static void DrawTetrisBoard()
        {
            // Locks the board.
            while (_tetrisDrawLocker) ;
            _tetrisDrawLocker = true;

            // Current Position of the cursor.
            int positionX = Console.CursorLeft;
            int positionY = Console.CursorTop;

            // Gets the cursor position and draws the board.
            Console.SetCursorPosition(positionX, positionY);
            WriteTetrisBoardOnConsole(_tetriscontainer.GetTetrisBoardAndBlock, true);

            // Goes back to the starting position.
            Console.SetCursorPosition(positionX, positionY);
            _tetrisDrawLocker = false;
        }

        /// <summary>
        /// Writes the 2D Tetris board on the console.
        /// </summary>
        /// <param name="tetrisBoard">Tetris board array</param>
        /// <param name="isWriteBorder">Write Border around Array?</param>
        static void WriteTetrisBoardOnConsole(int[,] tetrisBoard, bool isWriteBorder)
        {
            int positionX = Console.CursorLeft;
            for (int upperBound_0 = 0; upperBound_0 <= tetrisBoard.GetUpperBound(0); upperBound_0++)
            {
                if (isWriteBorder)
                {
                    Console.Write(Constants.TetrisBoardPiece);
                }
                for (int upperBound_1 = 0; upperBound_1 <= tetrisBoard.GetUpperBound(1); upperBound_1++)
                {
                    //Change color according to the Number
                    switch (tetrisBoard[upperBound_0, upperBound_1])
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            Console.Write(Constants.TetrisBoardPiece);
                            break;
                        default:
                            Console.Write(Constants.Empty);
                            break;
                    }
                }

                if (isWriteBorder)
                {
                    Console.WriteLine(Constants.TetrisBoardPiece);
                }
                else
                {
                    Console.WriteLine();
                }

                Console.CursorLeft = positionX;
            }
            for (int upperBound_1 = 0; upperBound_1 <= tetrisBoard.GetUpperBound(1) + 2; upperBound_1++)
            {
                if (isWriteBorder)
                {
                    Console.Write(Constants.TetrisBoardPiece);
                }
            }
        }
    }
}
