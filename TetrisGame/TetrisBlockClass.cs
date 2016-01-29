using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    /// <summary>
    /// Tetris Block Handler
    /// </summary>
    public class TetrisBlockClass
    {
        /// <summary>
        /// Tetris blocks list.
        /// </summary>
        private List<int[,]> _tetrisBlocksList;

        /// <summary>
        /// Random Tetris Block generator
        /// </summary>
        private Random _randomBlockGenerator;

        /// <summary>
        /// Constructor.
        /// Initializes the tetris blocks list and the random generator.
        /// </summary>
        public TetrisBlockClass()
        {
            _randomBlockGenerator = new Random(DateTime.Now.Millisecond);

            _tetrisBlocksList = new List<int[,]>();

            // ****
            _tetrisBlocksList.Add(new int[1, 4] { { 1, 1, 1, 1 } });

            // **
            // **
            _tetrisBlocksList.Add(new int[2, 2] { { 2, 2 }, { 2, 2 } });

            // *
            // *
            // **
            _tetrisBlocksList.Add(new int[3, 2] { { 3, 0 }, { 3, 0 }, { 3, 3 } });

            //  *
            //  *
            // **
            _tetrisBlocksList.Add(new int[3, 2] { { 0, 4 }, { 0, 4 }, { 4, 4 } });

            //  *
            // **
            // *
            _tetrisBlocksList.Add(new int[3, 2] { { 0, 5 }, { 5, 5 }, { 5, 0 } });
        }

        /// <summary>
        /// Returns a random Block from random generator.
        /// </summary>
        /// <returns>Random Block</returns>
        public int[,] GetRandomBlockFromGenerator()
        {
            return _tetrisBlocksList[_randomBlockGenerator.Next(_tetrisBlocksList.Count)];
        }

        /// <summary>
        /// Rotates a Block clockwise
        /// </summary>
        /// <param name="tetrisBlock">Block to rotate</param>
        /// <returns>rotated tetris block</returns>
        public static int[,] RotateClockWise(int[,] tetrisBlock)
        {
            int width = tetrisBlock.GetUpperBound(0) + 1;
            int height = tetrisBlock.GetUpperBound(1) + 1;
            int[,] rotatedBlock = new int[height, width];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    rotatedBlock[j, width - i - 1] = tetrisBlock[i, j];
                }
            }

            return rotatedBlock;
        }

        /// <summary>
        /// Rotates the block counter clockwise.
        /// Logic is, rotating the block clockwise thrice is equal to rotating a block counter clockwise once.
        /// </summary>
        /// <param name="tetrisBlock">Block to rotate</param>
        /// <returns>rotated Tetris Block</returns>
        public static int[,] RotateCounterClockwise(int[,] tetrisBlock)
        {
            return RotateClockWise(RotateClockWise(RotateClockWise(tetrisBlock)));
        }
    }
}
