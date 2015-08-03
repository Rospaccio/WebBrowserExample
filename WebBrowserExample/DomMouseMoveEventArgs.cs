using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WebBrowserExample
{
    public class DomMouseMoveEventArgs : RoutedEventArgs
    {
        public int ClientX { get; set; }
        public int ClientY { get; set; }

        public DomMouseMoveEventArgs(int clientX, int clientY)
        {
            ClientX = clientX;
            ClientY = clientY;
        }
    }
}
