using Akka.Actor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ServerApplication.Worker.Worker
{
    public class ServerApplicationWorker : ReceiveActor
    {
        private static string filePath;
        private StreamWriter _writer;
        public ServerApplicationWorker()
        {
            filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\NewFile\\";

            Receive<string>(WriteToFile);
        }

        protected override void PreStart()
        {
            CheckDirectory(filePath);
            _writer = new StreamWriter(File.Open(filePath + "newfile.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            base.PreStart();
        }

        protected override void PostStop()
        {
            _writer.Dispose();
            base.PostStop();
        }

        private void WriteToFile(string line)
        {
            _writer.WriteLine(line);
        }

        private void CheckDirectory(string dir)
        {
            var dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }
    }
}
