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
using mshtml;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WebBrowserExample
{
    /// <summary>
    /// Interaction logic for WebBrowserAdapter.xaml
    /// </summary>
    public partial class WebBrowserAdapter : UserControl
    {
        public WebBrowserAdapter()
        {
            InitializeComponent();
            this.Loaded += WebBrowserAdapter_Loaded;
        }

        void WebBrowserAdapter_Loaded(object sender, RoutedEventArgs e)
        {
            WebBrowserControl.LoadCompleted += WebBrowserControl_LoadCompleted;
            //WebBrowserControl.Navigate("http://www.google.com");
            WebBrowserControl.Navigate("http://localhost:9080/console/span.html");
            //WebBrowserControl.Navigate("file:///C:/Temp/span.html");
        }

        void WebBrowserControl_LoadCompleted(object sender, NavigationEventArgs e)
        {
            //IncjectClickOnSpanElementScript();
            //InjectConfirmHijack();
            //HookHTMLElements();
            HookOnMouseMove();
        }

        private void InjectConfirmHijack()
        {
            String script =
@" function hijackConfirm(){
    alert('yep!');
    window.oldConfirm = window.confirm;
    window.confirm = function(){ return true };
}";
            InjectScript(script);
            WebBrowserControl.InvokeScript("hijackConfirm");
        }

        private void HookOnMouseMove()
        {
            var document = WebBrowserControl.Document as HTMLDocument;
            var documentEvents = document as HTMLDocumentEvents_Event;
            documentEvents.onmousemove += documentEvents_onmousemove;
        }

        void documentEvents_onmousemove()
        {
            var now = DateTime.Now;
            Console.WriteLine("mousemove: " + now.Ticks);
        }

        private void IncjectClickOnSpanElementScript()
        {
            String script =
@"      function triggerClicksOnSpan(){
        var spans = document.getElementsByTagName('span');
        for(var i = 0; i < spans.length; i++){
          spans[i].click();
        }
      }";

            InjectScript(script);
            WebBrowserControl.InvokeScript("triggerClicksOnSpan");
        }

        private void HookHTMLElements()
        {
            var document = WebBrowserControl.Document as HTMLDocument;
            var inputElements = document.getElementsByTagName("span");
            foreach (var item in inputElements)
            {
                HTMLInputElement inputElement = item as HTMLInputElement;
                if (inputElement.type == "text"
                    || inputElement.type == "password"
                    || inputElement.type == "search")
                {
                    HTMLButtonElementEvents_Event htmlButtonEvent = inputElement as HTMLButtonElementEvents_Event;
                    htmlButtonEvent.onclick += FocusCallback;
                    htmlButtonEvent.onblur += htmlButtonEvent_onblur;
                }
            }
        }

        void htmlButtonEvent_onblur()
        {
            CloseTouchKeyboard();
        }

        public bool FocusCallback()
        {
            Console.WriteLine("Callback method executed!");
            ShowTouchKeyboard();
            return true;
        }

        public void InjectScript(String scriptText)
        {
            HTMLDocument htmlDocument = (HTMLDocument)WebBrowserControl.Document;

            var headElements = htmlDocument.getElementsByTagName("head");
            if (headElements.length == 0)
            {
                throw new IndexOutOfRangeException("No element with tag 'head' has been found in the document");
            }
            var headElement = headElements.item(0);

            IHTMLScriptElement script = (IHTMLScriptElement)htmlDocument.createElement("script");
            script.text = scriptText;
            headElement.AppendChild(script);
        }

        public static readonly int WM_SYSCOMMAND = 274;
        public static readonly uint SC_CLOSE = 61536;
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr KeyboardWnd, int WM_SYSCOMMAND, uint SC_CLOSE, int p);

        public static void ShowTouchKeyboard()
        {
            using (var keyboard = new Process())
            {
                keyboard.StartInfo = new ProcessStartInfo(@"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe", "/ManualLaunch");
                keyboard.Start();
            }
        }

        public static void CloseTouchKeyboard()
        {
            IntPtr keyboardWnd = FindWindow("IPTip_Main_Window", null);
            if (!keyboardWnd.Equals(IntPtr.Zero))
            {
                PostMessage(keyboardWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
            }
        }
    }
}
