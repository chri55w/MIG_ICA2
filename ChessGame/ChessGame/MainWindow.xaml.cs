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

        KinectRegion[] kinectRegions;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;

            kinectRegionMenu.Visibility = Visibility.Visible;
            kinectRegionHelp.Visibility = Visibility.Hidden;
            kinectRegionGame.Visibility = Visibility.Hidden;
            kinectRegions =  new KinectRegion[3];

            kinectRegions[0] = kinectRegionMenu;
            kinectRegions[1] = kinectRegionHelp;
            kinectRegions[2] = kinectRegionGame;

            Brush boxcolour = Brushes.Black;
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {

                    kinectHoverBox newKinectHoverBox = new kinectHoverBox();

                    //newKinectHoverBox.SetCurrentValue(newKinectHoverBox.DependencyObjectType., boxcolour);

                    chessgrid.Children.Add(newKinectHoverBox);

                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) {
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
                }
                catch (InvalidOperationException)
                {
                    error = true;
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }
            if (!error) {
                for (int i = 0; i < kinectRegions.Count(); i++) { 
                    kinectRegions[i].KinectSensor = args.NewSensor;
                }
            }
        }


        public void checkButtonPresses(kinectHoverButton button)
        {
            Console.WriteLine(button.Name);
            if (kinectRegionMenu.Visibility == Visibility.Visible) { 
                if (button.Name == "startbutton") {
                    kinectRegionMenu.Visibility = Visibility.Hidden;
                    kinectRegionHelp.Visibility = Visibility.Hidden;
                    kinectRegionGame.Visibility = Visibility.Visible;
                } else if (button.Name == "helpbutton") {

                } else if (button.Name == "closebutton") {

                } else {

                }
            }
        }
    }
}
