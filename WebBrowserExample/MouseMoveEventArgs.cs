using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WebBrowserExample
{
    public class MouseMoveEventArgs : RoutedEventArgs
    {
        public int ClientX { get; set; }
        public int ClientY { get; set; }

        public MouseMoveEventArgs(int clientX, int clientY)
        {
            ClientX = clientX;
            ClientY = clientY;
        }
    }
}
