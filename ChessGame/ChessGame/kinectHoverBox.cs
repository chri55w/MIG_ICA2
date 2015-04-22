﻿using System;
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

        #region Dependancy Properties
            public static readonly DependencyProperty IsHandOverProperty = DependencyProperty.RegisterAttached("IsHandOver", typeof(bool), typeof(kinectHoverBox), new PropertyMetadata(default(bool)));

            public static void SetIsHandOver(UIElement element, bool value) {
                element.SetValue(IsHandOverProperty, value);
            }

            public static bool GetIsHandOver(UIElement element) {
                return (bool)element.GetValue(IsHandOverProperty);
            }


            public static readonly DependencyProperty ColourProperty = DependencyProperty.RegisterAttached("RColour", typeof(Brush), typeof(kinectHoverButton), new PropertyMetadata(default(Brush)));

            public static void SetRColour(UIElement element, Brush value)
            {
                element.SetValue(ColourProperty, value);
            }

            public static Brush GetRColour(UIElement element)
            {
                return (Brush)element.GetValue(ColourProperty);
            }
        #endregion

        public kinectHoverBox() {
            this.InitializeKinectHoverBox();
        }
        private void InitializeKinectHoverBox() {
            KinectRegion.AddHandPointerEnterHandler(this, this.OnHandPointerEnter);
            KinectRegion.AddHandPointerLeaveHandler(this, this.OnHandPointerLeave);
        }
        
        #region Event Listeners
            protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e) {
                base.OnMouseEnter(e);
                SetIsHandOver(this, true);
            }

            protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e) {
                base.OnMouseLeave(e); 
                SetIsHandOver(this, false);
            }

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
        #endregion
    }
}