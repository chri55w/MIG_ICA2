using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Kinect.Toolkit.Controls;

namespace ChessGame {
    class kinectHoverBox : KinectButtonBase {

        private HandPointer activeHandpointer;
        public string boxCoordTag;

        #region Dependancy Properties
        //create a new dependency property to check when the hand pointer is over the object
        public static readonly DependencyProperty IsHandOverProperty = DependencyProperty.RegisterAttached("IsHandOver", typeof(bool), typeof(kinectHoverBox), new PropertyMetadata(default(bool)));

        public static void SetIsHandOver(UIElement element, bool value) {
            element.SetValue(IsHandOverProperty, value);
        }

        public static bool GetIsHandOver(UIElement element) {
            return (bool)element.GetValue(IsHandOverProperty);
        }

        //create a new dependency property to handle alternating box colours
        public static DependencyProperty ColourProperty = DependencyProperty.RegisterAttached("RColour", typeof(Brush), typeof(kinectHoverButton), new PropertyMetadata(Brushes.AntiqueWhite));

        public static void SetRColour(UIElement element, Brush value) {
            element.SetValue(ColourProperty, value);
        }

        public static Brush GetRColour(UIElement element) {
            return (Brush)element.GetValue(ColourProperty);
        }
        #endregion

        public kinectHoverBox(string boxTag) {
            //Create a string tag for this object
            this.InitializeKinectHoverBox();
            boxCoordTag = boxTag;
        }
        //bind event handlers to the box.
        public void InitializeKinectHoverBox() {
            KinectRegion.AddHandPointerEnterHandler(this, this.OnHandPointerEnter);
            KinectRegion.AddHandPointerLeaveHandler(this, this.OnHandPointerLeave);
            KinectRegion.AddHandPointerGripHandler(this, this.gripObject);
            KinectRegion.AddHandPointerGripReleaseHandler(this, this.releaseObject);
        }

        #region Event Listeners
        //when a hand pointer enters this object.
        private void OnHandPointerEnter(object sender, HandPointerEventArgs e) {
            if (!e.HandPointer.IsPrimaryHandOfUser || !e.HandPointer.IsPrimaryUser) {
                return;
            }

            this.activeHandpointer = e.HandPointer;
            SetIsHandOver(this, true);
        }
        //when a hand pointer leaves this object.
        private void OnHandPointerLeave(object sender, HandPointerEventArgs e) {
            if (this.activeHandpointer != e.HandPointer) {
                return;
            }

            this.activeHandpointer = null;
            SetIsHandOver(this, false);
        }

        //when a hand pointer grips over this object.
        private void gripObject(object sender, HandPointerEventArgs e) {
            if (this.activeHandpointer == e.HandPointer) {
                if (!this.activeHandpointer.IsInGripInteraction) {
                    this.activeHandpointer.IsInGripInteraction = true;

                    //Call back to main window to handle grip
                    var main = Application.Current.MainWindow as MainWindow;
                    main.grabObject(boxCoordTag);
                    Console.WriteLine("gripObject" + boxCoordTag);
                }
            }
        }
        //when a hand pointer releases over this object
        private void releaseObject(object sender, HandPointerEventArgs e) {
            if (this.activeHandpointer == e.HandPointer) {
                this.activeHandpointer.IsInGripInteraction = false;

                //Call back to main window to handle release
                var main = Application.Current.MainWindow as MainWindow;
                main.releaseObject(boxCoordTag);
                Console.WriteLine("releaseObject" + boxCoordTag);
            }
        }
        #endregion
    }
}
