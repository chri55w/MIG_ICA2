using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

using System.Globalization;


namespace ChessGame {

    public partial class MainWindow : Window {

        private KinectSensorChooser sensorChooser;

        //object grabbed using kinect gestures identified by its grid data
        string grippedObject = "";

        //chess board to hold all peices.
        ChessBoard gameBoard;

        //speech recognition object.
        SpeechRecognition speechRec;

        public MainWindow() {
            InitializeComponent();
            Loaded += OnLoaded;

            kinectRegionMenu.Visibility = Visibility.Visible;
            kinectRegionGame.Visibility = Visibility.Hidden;

            //Data used for populating the grid data
            string[] gridDataX = new string[8] { "A", "B", "C", "D", "E", "F", "G", "H" };
            string[] gridDataY = new string[8] { "1", "2", "3", "4", "5", "6", "7", "8" };

            //Populate wrap panel with hover icons to represent the hand position
            for (int y = 7; y >= 0; y--) {
                for (int x = 0; x < 8; x++) {

                    kinectHoverBox newKinectHoverBox = new kinectHoverBox(gridDataX[x] + gridDataY[y]);

                    chessgrid.Children.Add(newKinectHoverBox);
                }
            }
            //load in the chessboard image
            chessBoardImg.Source = new CroppedBitmap(new BitmapImage(new Uri(@"../../imgs/chessboard.fw.png", UriKind.Relative)), new Int32Rect(0, 0, 800, 800));

            //initialise the chessboard
            gameBoard = new ChessBoard(GameCanvas);

            //initialise the speech recognition
            speechRec = new SpeechRecognition();

        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) {
            //load the kinect sensor.
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();
        }

        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args) {
            bool error = false;
            if (args.OldSensor != null) {
                try {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                } catch (InvalidOperationException) {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                    error = true;
                }
            }

            if (args.NewSensor != null) {
                try {
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    args.NewSensor.SkeletonStream.Enable();

                    try {
                        args.NewSensor.DepthStream.Range = DepthRange.Near;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                        args.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                    } catch (InvalidOperationException) {
                        // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
                        args.NewSensor.DepthStream.Range = DepthRange.Default;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                        error = true;
                    }
                } catch (InvalidOperationException) {
                    error = true;
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }
            if (!error) {
                kinectRegionMenu.KinectSensor = args.NewSensor;
                kinectRegionGame.KinectSensor = args.NewSensor;
            }
        }


        public void checkButtonPresses(kinectHoverButton button) {
            //Write the button name to console
            Console.WriteLine(button.Name);
            //if the menu region is visible then button should be a menu button.
            if (kinectRegionMenu.Visibility == Visibility.Visible) {
                //handle button types
                if (button.Name == "startbutton") {
                    kinectRegionMenu.Visibility = Visibility.Hidden;
                    kinectRegionGame.Visibility = Visibility.Visible;
                } else if (button.Name == "closebutton") {
                    Console.WriteLine("Exit Button Pressed");
                    this.Close();
                } else {
                    Console.WriteLine("Unknown Button Press");
                    //Unrecognised Button Call
                }
            }
        }
        public void grabObject(string boxLoc) {
            //Identify the gridId of the gripped object
            grippedObject = boxLoc;
        }
        public void releaseObject(string boxLoc) {
            //if gripped object is set then attempt to move the gripped object to the release position.
            if (grippedObject != "") {
                Console.WriteLine("Moving " + grippedObject + " to " + boxLoc);
                gameBoard.swapPieces(grippedObject, boxLoc, GameCanvas);
                //reset the gripped object.
                grippedObject = "";
            }
        }
        public void tryMovePeice(string peiceA, string peiceB) {
            //swap the chess board peices.
            gameBoard.swapPieces(peiceA, peiceB, GameCanvas);
        }
        public void pressStart() {
            if (kinectRegionMenu.Visibility == Visibility.Visible) {
                kinectRegionMenu.Visibility = Visibility.Hidden;
                kinectRegionGame.Visibility = Visibility.Visible;
            }
        }
    }
}
