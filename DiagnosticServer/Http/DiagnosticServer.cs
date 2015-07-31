using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NHttp;

namespace DiagnosticServer.Http
{
    public class DiagnosticServer
    {
        public int Port { private get; set; }
        private NHttp.HttpServer httpServer;

        public DiagnosticServer(int port)
        {
            Port = port;
        }

        public void Listen()
        {
            httpServer = new HttpServer();
            httpServer.RequestReceived += RequestReceivedInternal;

            httpServer.EndPoint.Port = 8080;
            httpServer.Start();

        }

        public void Shutdown()
        {
            httpServer.Stop();
        }

        private void RequestReceivedInternal(object sender, HttpRequestEventArgs eventArgs)
        {
            using (var stream = new StreamWriter(eventArgs.Response.OutputStream))
            {
                stream.Write(ReadResponseFileContent());
            }
        }

        private String ReadResponseFileContent()
        {
            StreamReader reader = new StreamReader("Resources/heartbeat.html");
            String content = null;
            using (reader)
            {
                content = reader.ReadToEnd();
            }

            return content;
        }
    }
}
