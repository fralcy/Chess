﻿namespace ChessLogic.Pieces
{
    [Serializable]
    public class PositionValue
    {
        public double[,] pawnEvalWhite =
        {
            {0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0},
            {5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0},
            {1.0,  1.0,  2.0,  3.0,  3.0,  2.0,  1.0,  1.0},
            {0.5,  0.5,  1.0,  2.5,  2.5,  1.0,  0.5,  0.5},
            {0.0,  0.0,  0.0,  2.0,  2.0,  0.0,  0.0,  0.0},
            {0.5, -0.5, -1.0,  0.0,  0.0, -1.0, -0.5,  0.5},
            {0.5,  1.0,  1.0, -2.0, -2.0,  1.0,  1.0,  0.5},
            {0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0}
        };

        public double[,] pawnEvalBlack = new double[8, 8];

        public double[,] knightEval =
        {
            {-5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0},
            {-4.0, -2.0,  0.0,  0.0,  0.0,  0.0, -2.0, -4.0},
            {-3.0,  0.0,  1.0,  1.5,  1.5,  1.0,  0.0, -3.0},
            {-3.0,  0.5,  1.5,  2.0,  2.0,  1.5,  0.5, -3.0},
            {-3.0,  0.0,  1.5,  2.0,  2.0,  1.5,  0.0, -3.0},
            {-3.0,  0.5,  1.0,  1.5,  1.5,  1.0,  0.5, -3.0},
            {-4.0, -2.0,  0.0,  0.5,  0.5,  0.0, -2.0, -4.0},
            {-5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0}
        };

        public double[,] bishopEvalWhite = new double[,]
        {
            { -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0 },
            { -1.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -1.0 },
            { -1.0,  0.0,  0.5,  1.0,  1.0,  0.5,  0.0, -1.0 },
            { -1.0,  0.5,  0.5,  1.0,  1.0,  0.5,  0.5, -1.0 },
            { -1.0,  0.0,  1.0,  1.0,  1.0,  1.0,  0.0, -1.0 },
            { -1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0, -1.0 },
            { -1.0,  0.5,  0.0,  0.0,  0.0,  0.0,  0.5, -1.0 },
            { -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0 }
        };

        public double[,] bishopEvalBlack = new double[8, 8];

        public double[,] rookEvalWhite = new double[,]
        {
            {  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0},
            {  0.5,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5},
            { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
            { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
            { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
            { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
            { -0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
            {  0.0,  0.0,  0.0,  0.5,  0.5,  0.0,  0.0,  0.0}
        };

        public double[,] rookEvalBlack = new double[8, 8];

        public double[,] evalQueen =
        {
            { -2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0 },
            { -1.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -1.0 },
            { -1.0,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -1.0 },
            { -0.5,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -0.5 },
            {  0.0,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -0.5 },
            { -1.0,  0.5,  0.5,  0.5,  0.5,  0.5,  0.0, -1.0 },
            { -1.0,  0.0,  0.5,  0.0,  0.0,  0.0,  0.0, -1.0 },
            { -2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0 }
        };

        public double[,] kingEvalWhite =
        {
            { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            { -2.0, -3.0, -3.0, -4.0, -4.0, -3.0, -3.0, -2.0 },
            { -1.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -1.0 },
            {  2.0,  2.0,  0.0,  0.0,  0.0,  0.0,  2.0,  2.0 },
            {  2.0,  3.0,  1.0,  0.0,  0.0,  1.0,  3.0,  2.0 }
        };

        public double[,] kingEvalBlack = new double[8, 8];
        void reverseArray(ref double[,] darr, double[,] sarr)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    darr[i, j] = sarr[8 - 1 - i, j];
                }
            }
        }

        void InitBlackPosValue()
        {
            reverseArray(ref pawnEvalBlack, pawnEvalWhite);
            reverseArray(ref bishopEvalBlack, bishopEvalWhite);
            reverseArray(ref rookEvalBlack, rookEvalWhite);
            reverseArray(ref kingEvalBlack, kingEvalWhite);

        }

        public PositionValue()
        {
            InitBlackPosValue();
        }
    }
}

