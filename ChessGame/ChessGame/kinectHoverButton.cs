using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Kinect.Toolkit.Controls;

namespace ChessGame {
    public class kinectHoverButton : KinectButtonBase {

        private HandPointer activeHandpointer;
        bool pressed = false;

        #region Dependancy Properties
            public static readonly DependencyProperty IsHandOverProperty = DependencyProperty.RegisterAttached("IsHandOver", typeof(bool), typeof(kinectHoverButton), new PropertyMetadata(default(bool)));

            public static void SetIsHandOver(UIElement element, bool value)
            {
                element.SetValue(IsHandOverProperty, value);
            }

            public static bool GetIsHandOver(UIElement element)
            {
                return (bool)element.GetValue(IsHandOverProperty);
            }
        #endregion
        

        #region Button Creation
            public kinectHoverButton() {
                this.InitializeKinectHoverButton();
            }

            private void InitializeKinectHoverButton() {
                KinectRegion.AddHandPointerEnterHandler(this, this.OnHandPointerEnter);
                KinectRegion.AddHandPointerLeaveHandler(this, this.OnHandPointerLeave);
                KinectRegion.AddQueryInteractionStatusHandler(this, this.whileHandInside);
            }
        #endregion

        
        #region Event Listeners

            private void OnHandPointerEnter(object sender, HandPointerEventArgs e) {
                if (!e.HandPointer.IsPrimaryHandOfUser || !e.HandPointer.IsPrimaryUser) {
                    return;
                }

                this.activeHandpointer = e.HandPointer;
                SetIsHandOver(this, true);
            }

            private void OnHandPointerLeave(object sender, HandPointerEventArgs e) {
                if (this.activeHandpointer != e.HandPointer) {
                    return;
                }

                this.activeHandpointer = null;
                SetIsHandOver(this, false);
            }

            private void whileHandInside(object sender, HandPointerEventArgs e) {
                //Checking for hand press
                if (this.activeHandpointer == e.HandPointer) {
                    if (pressed && !this.activeHandpointer.IsPressed) { 
                        pressed = false;
                    }
                    if (!pressed && this.activeHandpointer.IsPressed) {
                        var main = Application.Current.MainWindow as MainWindow;
                        main.checkButtonPresses(this);
                        pressed = true;
                    }
                }
            }
        #endregion
    }
}
