﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;


namespace ChessGame {
    class ChessPeice {

        private string peiceType;
        public Image peiceImage = new Image();

        public ChessPeice(string type) {
            peiceType = type;
            peiceImage.Width = 80;
            peiceImage.Height = 80;
            peiceImage.Source = new CroppedBitmap(new BitmapImage(new Uri("../../imgs/peices/" + type + ".png", UriKind.Relative)), new Int32Rect(0, 0, 80, 80));
            
        }
    }
}
