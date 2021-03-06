﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace ChessGame {
    class ChessBoard {
        ChessPeice[,] boardPeices;

        public ChessBoard(Canvas gameCanvas) {
            //initialise, populate and draw all peices
            boardPeices = new ChessPeice[8, 8];
            populateChessPeices();
            reDrawBoard(gameCanvas);
        }
        //hard coded chess start positions.
        void populateChessPeices() {
            boardPeices[0, 0] = new ChessPeice("RookB");
            boardPeices[1, 0] = new ChessPeice("KnightB");
            boardPeices[2, 0] = new ChessPeice("BishopB");
            boardPeices[3, 0] = new ChessPeice("QueenB");
            boardPeices[4, 0] = new ChessPeice("KingB");
            boardPeices[5, 0] = new ChessPeice("BishopB");
            boardPeices[6, 0] = new ChessPeice("KnightB");
            boardPeices[7, 0] = new ChessPeice("RookB");
            boardPeices[0, 1] = new ChessPeice("PawnB");
            boardPeices[1, 1] = new ChessPeice("PawnB");
            boardPeices[2, 1] = new ChessPeice("PawnB");
            boardPeices[3, 1] = new ChessPeice("PawnB");
            boardPeices[4, 1] = new ChessPeice("PawnB");
            boardPeices[5, 1] = new ChessPeice("PawnB");
            boardPeices[6, 1] = new ChessPeice("PawnB");
            boardPeices[7, 1] = new ChessPeice("PawnB");

            boardPeices[0, 2] = new ChessPeice("null");
            boardPeices[1, 2] = new ChessPeice("null");
            boardPeices[2, 2] = new ChessPeice("null");
            boardPeices[3, 2] = new ChessPeice("null");
            boardPeices[4, 2] = new ChessPeice("null");
            boardPeices[5, 2] = new ChessPeice("null");
            boardPeices[6, 2] = new ChessPeice("null");
            boardPeices[7, 2] = new ChessPeice("null");
            boardPeices[0, 3] = new ChessPeice("null");
            boardPeices[1, 3] = new ChessPeice("null");
            boardPeices[2, 3] = new ChessPeice("null");
            boardPeices[3, 3] = new ChessPeice("null");
            boardPeices[4, 3] = new ChessPeice("null");
            boardPeices[5, 3] = new ChessPeice("null");
            boardPeices[6, 3] = new ChessPeice("null");
            boardPeices[7, 3] = new ChessPeice("null");
            boardPeices[0, 4] = new ChessPeice("null");
            boardPeices[1, 4] = new ChessPeice("null");
            boardPeices[2, 4] = new ChessPeice("null");
            boardPeices[3, 4] = new ChessPeice("null");
            boardPeices[4, 4] = new ChessPeice("null");
            boardPeices[5, 4] = new ChessPeice("null");
            boardPeices[6, 4] = new ChessPeice("null");
            boardPeices[7, 4] = new ChessPeice("null");
            boardPeices[0, 5] = new ChessPeice("null");
            boardPeices[1, 5] = new ChessPeice("null");
            boardPeices[2, 5] = new ChessPeice("null");
            boardPeices[3, 5] = new ChessPeice("null");
            boardPeices[4, 5] = new ChessPeice("null");
            boardPeices[5, 5] = new ChessPeice("null");
            boardPeices[6, 5] = new ChessPeice("null");
            boardPeices[7, 5] = new ChessPeice("null");

            boardPeices[0, 6] = new ChessPeice("PawnW");
            boardPeices[1, 6] = new ChessPeice("PawnW");
            boardPeices[2, 6] = new ChessPeice("PawnW");
            boardPeices[3, 6] = new ChessPeice("PawnW");
            boardPeices[4, 6] = new ChessPeice("PawnW");
            boardPeices[5, 6] = new ChessPeice("PawnW");
            boardPeices[6, 6] = new ChessPeice("PawnW");
            boardPeices[7, 6] = new ChessPeice("PawnW");
            boardPeices[0, 7] = new ChessPeice("RookW");
            boardPeices[1, 7] = new ChessPeice("KnightW");
            boardPeices[2, 7] = new ChessPeice("BishopW");
            boardPeices[3, 7] = new ChessPeice("QueenW");
            boardPeices[4, 7] = new ChessPeice("KingW");
            boardPeices[5, 7] = new ChessPeice("BishopW");
            boardPeices[6, 7] = new ChessPeice("KnightW");
            boardPeices[7, 7] = new ChessPeice("RookW");
        }

        void reDrawBoard(Canvas gameCanvas) {
            //clear all peices
            gameCanvas.Children.Clear();
            //redraw all peices
            for (int y = 7; y >= 0; y--) {
                for (int x = 7; x >= 0; x--) {
                    Image thisChessImage = boardPeices[x, 7 - y].peiceImage;
                    Canvas.SetLeft(thisChessImage, x * 100 + 10);
                    Canvas.SetRight(thisChessImage, x * 100 + 90);
                    Canvas.SetBottom(thisChessImage, y * 100 + 90);
                    Canvas.SetTop(thisChessImage, y * 100 + 10);
                    gameCanvas.Children.Add(thisChessImage);
                }
            }
        }

        //swap two chess peices.
        public void swapPieces(string peice1, string peice2, Canvas gameCanvas) {
            //split peice positions and calclulate the offsets into the array
            int offset1A = ConvertGridIDToIDX(peice1[0].ToString());
            int offset1N = ConvertGridIDToIDX(peice1[1].ToString());

            int offset2A = ConvertGridIDToIDX(peice2[0].ToString());
            int offset2N = ConvertGridIDToIDX(peice2[1].ToString());

            //make a copy of both peices
            ChessPeice peiceA = boardPeices[offset1A, offset1N];
            ChessPeice peiceB = boardPeices[offset2A, offset2N];

            //assign the peices to the swapped peice
            boardPeices[offset1A, offset1N] = peiceB;
            boardPeices[offset2A, offset2N] = peiceA;

            //re draw the board
            reDrawBoard(gameCanvas);
        }

        int ConvertGridIDToIDX(string gridID) {
            //calculate offsets into board peices using string / char.
            if (gridID == "1" || gridID == "A") {
                return 0;
            } else if (gridID == "2" || gridID == "B") {
                return 1;
            } else if (gridID == "3" || gridID == "C") {
                return 2;
            } else if (gridID == "4" || gridID == "D") {
                return 3;
            } else if (gridID == "5" || gridID == "E") {
                return 4;
            } else if (gridID == "6" || gridID == "F") {
                return 5;
            } else if (gridID == "7" || gridID == "G") {
                return 6;
            } else if (gridID == "8" || gridID == "H") {
                return 7;
            }
            return -1;
        }
    }
}
