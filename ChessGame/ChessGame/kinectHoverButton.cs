using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Kinect.Toolkit.Controls;

namespace ChessGame
{
    class kinectHoverButton : KinectButtonBase
    {
        public static readonly DependencyProperty IsHandOverProperty = DependencyProperty.RegisterAttached("IsHandOver", typeof(bool), typeof(Extensions), new PropertyMetadata(default(bool)));

        public static void SetIsHandOver(UIElement element, bool value)
        {
            element.SetValue(IsHandOverProperty, value);
        }

        public static bool GetIsHandOver(UIElement element)
        {
            return (bool)element.GetValue(IsHandOverProperty);
        }

        private HandPointer activeHandpointer;

        public kinectHoverButton() {
            this.InitializeKinectHoverButton();
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            SetIsHandOver(this, true);
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e); 
            SetIsHandOver(this, false);
        }

        private void InitializeKinectHoverButton()
        {
            KinectRegion.AddHandPointerEnterHandler(this, this.OnHandPointerEnter);
            KinectRegion.AddHandPointerLeaveHandler(this, this.OnHandPointerLeave);
        }

        private void OnHandPointerEnter(object sender, HandPointerEventArgs e)
        {
            if (!e.HandPointer.IsPrimaryHandOfUser || !e.HandPointer.IsPrimaryUser)
            {
                return;
            }

            this.activeHandpointer = e.HandPointer;
            SetIsHandOver(this, true);
        }

        private void OnHandPointerLeave(object sender, HandPointerEventArgs e)
        {
            if (this.activeHandpointer != e.HandPointer)
            {
                return;
            }

            this.activeHandpointer = null;
            SetIsHandOver(this, false);
        }
    }
}
