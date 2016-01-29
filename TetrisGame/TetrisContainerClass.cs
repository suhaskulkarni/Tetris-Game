using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    /// <summary>
    /// The Tetris Container class
    /// </summary>
    public class TetrisContainerClass
    {
        /// <summary>
        /// Game Over Event Handler
        /// </summary>
        public delegate void GameOverHandler();

        /// <summary>
        /// Fixed block event handler.
        /// </summary>
        public delegate void BlockFixedHandler();

        /// <summary>
        /// Handles when a whole line or lines are completed.
        /// </summary>
        /// <param name="Lines">Number of Lines completed</param>
        public delegate void LinesCompletedHandler(int linesCompleted);

        /// <summary>
        /// Game over event.
        /// </summary>
        public event GameOverHandler GameOverEventHandler;

        /// <summary>
        /// Event when block is fixed.
        /// </summary>
        public event BlockFixedHandler BlockFixedEventHandler;

        /// <summary>
        /// Event for lines completed
        /// </summary>
        public event LinesCompletedHandler LinesCompletedEventHandler;

        /// <summary>
        /// Enum for Tetris input keys.
        /// </summary>
        public enum InputKey
        {
            Left,
            Right,
            Clockwise,
            CounterClockwise
        }

        /// <summary>
        /// The Tetris board container array.
        /// </summary>
        private int[,] _tetrisBoardContainer;

        /// <summary>
        /// X Co-ordinate of Tetris Block.
        /// </summary>
        private int _blockPositionX;

        /// <summary>
        /// Y Co-ordinate of Tetris Block.
        /// </summary>
        private int _blockPositionY;

        /// <summary>
        /// Array of current moving tetris block.
        /// </summary>
        private int[,] _currentTetrisBlock = null;

        /// <summary>
        /// Array of next tetris block.
        /// </summary>
        private int[,] _nextTetrisBlock = null;

        /// <summary>
        /// Generates a Tetris block.
        /// </summary>
        private TetrisBlockClass blockGenerator = new TetrisBlockClass();

        /// <summary>
        /// True as long as the Game is running.
        /// </summary>
        private bool _isInGame;

        /// <summary>
        /// Tetris Game constructor as an initializer.
        /// </summary>
        /// <param name="Width">Width of Container</param>
        /// <param name="Height">Height of Container</param>
        public TetrisContainerClass(int Width, int Height)
        {
            if (Width >= 20 && Height >= 20)
            {
                _tetrisBoardContainer = new int[Height, Width];
            }
            else
            {
                throw new Exception("The Tetris body should be atleast 20x20");
            }

        }

        /// <summary>
        /// Gets the Tetris Block on the console.
        /// </summary>
        public void GetTetrisBlock()
        {
            _isInGame = true;
            _blockPositionY = 0;
            _blockPositionX = _tetrisBoardContainer.GetUpperBound(1) / 2;
            _currentTetrisBlock = _nextTetrisBlock != null ? _nextTetrisBlock : blockGenerator.GetRandomBlockFromGenerator();
            _nextTetrisBlock = blockGenerator.GetRandomBlockFromGenerator();
            if (!CanBlockBePositionedAt(_currentTetrisBlock, _blockPositionX, _blockPositionY))
            {
                GameOver();
            }
        }

        /// <summary>
        /// Game is over, so handle the game over event.
        /// </summary>
        public void GameOver()
        {
            _isInGame = false;
            if (GameOverEventHandler != null)
            {
                GameOverEventHandler();
            }
        }

        /// <summary>
        /// Moves the game to next step.
        /// </summary>
        public void NextStep()
        {
            if (_isInGame)
            {
                if (CanBlockBePositionedAt(_currentTetrisBlock, _blockPositionX, _blockPositionY + 1))
                {
                    _blockPositionY++;
                }
                else
                {
                    _tetrisBoardContainer = FixBlockInTheBoard(_currentTetrisBlock, _tetrisBoardContainer, _blockPositionX, _blockPositionY);

                    this.GetTetrisBlock();
                }

                int linesCompleted = CheckLinesCompleted();
                if (LinesCompletedEventHandler != null)
                {
                    LinesCompletedEventHandler(linesCompleted);
                }
            }
        }

        /// <summary>
        /// Check the input from the keyboard and handle it.
        /// </summary>
        /// <param name="inputKey">Tetris Key</param>
        public void keyPress(InputKey inputKey)
        {
            if (_isInGame)
            {
                int[,] tempBlock;
                switch (inputKey)
                {
                    //Handle the left key input, that is "A".
                    case InputKey.Left:
                        if (_blockPositionX > 0 && CanBlockBePositionedAt(_currentTetrisBlock, _blockPositionX - 1, _blockPositionY))
                        {
                            _blockPositionX--;
                        }
                        break;

                    //Handle the right key input, that is "D".
                    case InputKey.Right:
                        if (_blockPositionX < _tetrisBoardContainer.GetUpperBound(0) && CanBlockBePositionedAt(_currentTetrisBlock, _blockPositionX + 1, _blockPositionY))
                        {
                            _blockPositionX++;
                        }
                        break;

                    //Handle the Counter clockwise key input, that is "W".
                    case InputKey.CounterClockwise:
                        tempBlock = TetrisBlockClass.RotateCounterClockwise(_currentTetrisBlock);
                        if (CanBlockBePositionedAt(tempBlock, _blockPositionX, _blockPositionY))
                        {
                            _currentTetrisBlock = TetrisBlockClass.RotateCounterClockwise(_currentTetrisBlock);
                        }
                        break;

                    //Handle the Counter clockwise key input, that is "S".
                    case InputKey.Clockwise:
                        tempBlock = TetrisBlockClass.RotateClockWise(_currentTetrisBlock);
                        if (CanBlockBePositionedAt(tempBlock, _blockPositionX, _blockPositionY))
                        {
                            _currentTetrisBlock = TetrisBlockClass.RotateClockWise(_currentTetrisBlock);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Checks if a Block can be positioned at a specific Location.
        /// </summary>
        /// <param name="tetrisBlock">Block to check</param>
        /// <param name="blockPosX">X position of block</param>
        /// <param name="blockPosY">Y position of block</param>
        /// <returns>true, if positionable</returns>
        private bool CanBlockBePositionedAt(int[,] tetrisBlock, int blockPosX, int blockPosY)
        {
            int[,] copyOfTetrisBoard = (int[,])_tetrisBoardContainer.Clone();

            if (blockPosX + tetrisBlock.GetUpperBound(1) <= copyOfTetrisBoard.GetUpperBound(1) && blockPosY + tetrisBlock.GetUpperBound(0) <= copyOfTetrisBoard.GetUpperBound(0))
            {
                for (int upperBound1 = 0; upperBound1 <= tetrisBlock.GetUpperBound(1); upperBound1++)
                {
                    for (int upperBound0 = 0; upperBound0 <= tetrisBlock.GetUpperBound(0); upperBound0++)
                    {
                        //I=X Coord
                        //J=Y Coord
                        if (tetrisBlock[upperBound0, upperBound1] != 0)
                        {
                            if (copyOfTetrisBoard[blockPosY + upperBound0, blockPosX + upperBound1] != 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Fixes a Block in the Tetris board and returns the new board.
        /// </summary>
        /// <param name="tetrisBlock">Block to fix</param>
        /// <param name="tetrisContainer">Container to use</param>
        /// <param name="blockPosX">X postion of block</param>
        /// <param name="blockPosY">Y postion of block</param>
        /// <returns>New Container with fixed tetris Block</returns>
        private int[,] FixBlockInTheBoard(int[,] tetrisBlock, int[,] tetrisContainer, int blockPosX, int blockPosY)
        {
            if (blockPosX + tetrisBlock.GetUpperBound(1) <= tetrisContainer.GetUpperBound(1) && blockPosY + tetrisBlock.GetUpperBound(0) <= tetrisContainer.GetUpperBound(0))
            {
                for (int upperBound1 = 0; upperBound1 <= tetrisBlock.GetUpperBound(1); upperBound1++)
                {
                    for (int upperBound0 = 0; upperBound0 <= tetrisBlock.GetUpperBound(0); upperBound0++)
                    {
                        if (tetrisBlock[upperBound0, upperBound1] != 0)
                        {
                            tetrisContainer[blockPosY + upperBound0, blockPosX + upperBound1] = tetrisBlock[upperBound0, upperBound1];
                        }
                    }
                }
            }
            if (BlockFixedEventHandler != null)
            {
                BlockFixedEventHandler();
            }
            return tetrisContainer;
        }

        /// <summary>
        /// Checks for completed Lines and returns the Number of Lines found (and removed).
        /// </summary>
        /// <returns></returns>
        private int CheckLinesCompleted()
        {
            for (int upperBound0 = 0; upperBound0 < _tetrisBoardContainer.GetUpperBound(0) + 1; upperBound0++)
            {
                bool isLineCompleted = true;
                for (int upperBound1 = 0; upperBound1 < _tetrisBoardContainer.GetUpperBound(1) + 1; upperBound1++)
                {
                    isLineCompleted = isLineCompleted && _tetrisBoardContainer[upperBound0, upperBound1] != 0;
                }
                if (isLineCompleted)
                {
                    RemoveCompletedLine(upperBound0--);
                    return CheckLinesCompleted() + 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Removes a completed line at the specified Index.
        /// </summary>
        /// <param name="index">Y Pos (0 Based)</param>
        private void RemoveCompletedLine(int index)
        {
            // Move lines inside container one step down.
            for (int i = index; i > 0; i--)
            {
                for (int j = 0; j <= _tetrisBoardContainer.GetUpperBound(1); j++)
                {
                    _tetrisBoardContainer[i, j] = _tetrisBoardContainer[i - 1, j];
                }
            }
            // Empties top line.
            for (int j = 0; j <= _tetrisBoardContainer.GetUpperBound(1); j++)
            {
                _tetrisBoardContainer[0, j] = 0;
            }
        }

        /// <summary>
        /// Gets the Tetris board (readonly)
        /// </summary>
        public int[,] GetTetrisBoardAndBlock
        {
            get
            {
                int[,] tetrisBlock = (int[,])_currentTetrisBlock.Clone();
                int[,] tempContainer = (int[,])_tetrisBoardContainer.Clone();

                for (int upperBound0 = 0; upperBound0 <= tetrisBlock.GetUpperBound(0); upperBound0++)
                {
                    for (int upperBound1 = 0; upperBound1 <= tetrisBlock.GetUpperBound(1); upperBound1++)
                    {
                        if (tetrisBlock[upperBound0, upperBound1] != 0)
                        {
                            tetrisBlock[upperBound0, upperBound1] = 7;
                        }
                    }
                }
                tempContainer = FixBlockInTheBoard(tetrisBlock, tempContainer, _blockPositionX, _blockPositionY);

                return tempContainer;
            }
        }

        /// <summary>
        /// Game running State (readonly)
        /// </summary>
        public bool IsGameRunning
        {
            get
            {
                return _isInGame;
            }
        }
    }
}
