using System;
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
        //String to recognise peice type and used for fetching the image from directory.
        private string peiceType;
        //Image object declaration.
        public Image peiceImage = new Image();

        public ChessPeice(string type) {
            //initialise type.
            peiceType = type;

            //Explicitly state Width and Height to fix DPI mismatching issues.
            peiceImage.Width = 80;
            peiceImage.Height = 80;

            //update the image source.
            peiceImage.Source = new CroppedBitmap(new BitmapImage(new Uri("../../imgs/peices/" + type + ".png", UriKind.Relative)), new Int32Rect(0, 0, 80, 80));

        }
    }
}
