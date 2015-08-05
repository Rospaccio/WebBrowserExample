using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormBrowser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            webBrowserControl.Navigate("file:///C:/Temp/select.html");
            webBrowserControl.Navigated += webBrowserControl_Navigated;
        }

        void webBrowserControl_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //InjectConfirmHijack();
            //IncjectClickOnSpanElementScript();
            
        }

        private void RemoveSelectedAttributeBy()
        {
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
            webBrowserControl.Document.InvokeScript("triggerClicksOnSpan");
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
            webBrowserControl.Document.InvokeScript("hijackConfirm");
        }

        public void InjectScript(String scriptText)
        {
            //mshtml.HTMLDocument htmlDocument = (mshtml.IHTMLDocument) webBrowserControl.Document.get;

            var headElements = webBrowserControl.Document.GetElementsByTagName("head");
            if (headElements.Count == 0)
            {
                throw new IndexOutOfRangeException("No element with tag 'head' has been found in the document");
            }
            var headElement = headElements[0];

            var script = webBrowserControl.Document.CreateElement("script");
            script.InnerHtml = scriptText; // "<script>" + scriptText + "</script>";
            headElement.AppendChild(script);
        }

        private void toggle_Click(object sender, EventArgs e)
        {
            webBrowserControl.Document.GetElementById("togglingOption").SetAttribute("selected", "");
        }
    }
}
