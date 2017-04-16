using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
namespace JoApp_Loader
{
    class Program
    {
        public static ManualResetEvent tcpClientConnected = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            Console.WriteLine("[*] By Mr.8ThBiT :)");
           // Process.Start("http://8thbit.net/"); 
            bool appflag = true;
            try
            {   
                string hostfile = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                hostfile = Path.Combine(hostfile, @"system32\drivers\etc\hosts");
                string[] lines = File.ReadAllLines(hostfile);
                bool flag = false;
                bool flag2 = false;
                foreach (string line in lines)
                {
                    if (line.Trim().StartsWith("#"))
                        continue;
                    if (line.Contains("bejo.ir"))
                    {
                        Console.WriteLine("[+] Host file is already patched.");
                        flag = true;
                        break; 
                    }
                }
                if (flag == false )
                {
                    try
                    {
                        File.AppendAllText(hostfile, Environment.NewLine + "127.0.0.1\tbejo.ir\r\n");
                        Console.WriteLine("[+] Host file were patched.");
                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine("[-] Unable to add record to host file.");
                        appflag = false;
                    }
                }
                Run("netsh.exe int ip delete addr 1 78.46.19.104/32 ");
                Run("netsh.exe int ip delete addr 1 78.46.19.104 ");
                Run("netsh.exe int ip add addr 1 78.46.19.104/32 st=ac sk=tr");
            }
            catch (Exception e)
            {
                Console.WriteLine("[-] " + e.Message);
                appflag = false;
            }
            
            Console.WriteLine("[+] Starting application on port 80");
            TcpListener listener = new TcpListener(IPAddress.Any,80);
            try
            {
                start(listener); 
            }
            catch (Exception e)
            {
                Console.WriteLine("[-] Port is busy ...\n[-] Trying to Kill Port");
                try
                {
                    var prc = new ProcManager();
                    if (prc.KillByPort(80))
                    {
                        Console.WriteLine("[+] Trying to sit on port 80.");
                        start(listener); 
                    }
                   
                }
                catch (Exception e2)
                {
                    appflag = false;
                    Console.WriteLine("[-] Unable to kill port 80.");
                }
            }
            try
            {
                Process.Start("JoApp.exe");
            }
            catch (Exception e)
            {
                
                Console.WriteLine("[-] Unable to find JoApp executable file,\r\n\ttry to copy the loader in JoApp folder :)");
                if(appflag)
                    Console.WriteLine("[+] however Everything is working fine you can execute JoApp.exe manually");
            }
            Console.WriteLine("[*] Press Enter Or ESC to exit");
            while (true)
            {

                ConsoleKeyInfo x = Console.ReadKey();
                if (x.Key == ConsoleKey.Escape || x.Key == ConsoleKey.Enter)
                    Environment.Exit(0); 
            }
        }
        private static void Run(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            startInfo.Verb = "runas";
            process.StartInfo = startInfo;
            process.Start();
        }
        static void start(TcpListener listener)
        {
            listener.Start();
            startResponding(listener);
        }
        static void startResponding(TcpListener listener)
        {
            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptConnection), listener); 
        }
        static Random rnd = new Random();
        private static void AcceptConnection(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(ar);
            Console.WriteLine("[+] Connection Accepted From : {0}" , client.Client.RemoteEndPoint);
            string str = "" ;
            if (client.GetStream().CanRead)
            {
                byte[] myReadBuffer = new byte[1024];
                StringBuilder myCompleteMessage = new StringBuilder();
                int numberOfBytesRead = 0;

                // Incoming message may be larger than the buffer size. 
                do
                {
                    numberOfBytesRead = client.GetStream().Read(myReadBuffer, 0, myReadBuffer.Length);

                    myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));

                }
                while (client.GetStream().DataAvailable);

                // Print out the received message to the console.
                str = myCompleteMessage.ToString();
            }
            string code = "";
            string url = Regex.Match(str, "(POST|GET) (.*) HTTP/1.1", RegexOptions.IgnoreCase).Groups[2].Value;
            if (url == "/service_joapp/v2/LOGIN.php")
            {
                code = JoLib.GenerateLoginPacket();
            } else if (url == "/service_joapp/v2/GETCHARGE.php")
            {
                code = JoLib.GenerateRemainingDays();
            } else if (url == "/service_joapp/v2/NEW_PROJECT.php")
            {
                code = JoLib.GenerateNewProject(); 
            } else if (url == "/service_joapp/v2/ALLOW.php")
            {
                code = JoLib.GenerateAllowPacket();
            } else if (url == "/service_joapp/v2/TEMP.php")
            {
                code = JoLib.GenerateTempPacket();
            } else if (url == "/service_joapp/v2/SUCCESS_PROJECT.php")
            {
                code = JoLib.GenerateSuccessProject();
            } else if (url.Contains("/downloads/version_joapp.php"))
            {
                code = "\r\n<br><br>Cracked By 8ThBiT";
            }

            
            Console.WriteLine("[+] Responding to {0}" , url);
            


          //  code = Encoding.UTF8.GetString(Convert.FromBase64String(code));
           // code = code.Replace("{0}", randomMail());
            string  packet =  "HTTP/1.1 200 OK\r\n";
            packet += "Connection: Keep-Alive\r\n";
            packet += "Content-Length: " + code.Length + "\r\n";
            packet += "Content-Type: text/html;\r\n";
            packet += "Keep-Alive: timeout=5, max=50\r\n\r\n";
            packet += code;
            byte[] packetStub = System.Text.Encoding.UTF8.GetBytes(packet);
            if (client.Connected)
            {
                client.GetStream().Write(packetStub, 0, packetStub.Length);
                client.GetStream().Flush();
                client.GetStream().Close();
            }
            startResponding(listener);
        }
    }
    public class PRC
    {
        public int PID { get; set; }
        public int Port { get; set; }
        public string Protocol { get; set; }
    }
    public class ProcManager
    {
        public bool KillByPort(int port)
        {
            var processes = GetAllProcesses();
            if (processes.Any(p => p.Port == port))
                try
                {
                    Process.GetProcessById(processes.First(p => p.Port == port).PID).Kill();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[-] " + ex.Message);
                    return false;
                }
            else
            {
                return false;
            }
        }

        public List<PRC> GetAllProcesses()
        {
            var pStartInfo = new ProcessStartInfo();
            pStartInfo.FileName = "netstat.exe";
            pStartInfo.Arguments = "-a -n -o";
            pStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            pStartInfo.UseShellExecute = false;
            pStartInfo.RedirectStandardInput = true;
            pStartInfo.RedirectStandardOutput = true;
            pStartInfo.RedirectStandardError = true;

            var process = new Process()
            {
                StartInfo = pStartInfo
            };
            process.Start();

            var soStream = process.StandardOutput;

            var output = soStream.ReadToEnd();
            if (process.ExitCode != 0)
                throw new Exception("[-] Internal Error");

            var result = new List<PRC>();

            var lines = Regex.Split(output, "\r\n");
            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("Proto"))
                    continue;

                var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var len = parts.Length;
                if (len > 2)
                    result.Add(new PRC
                    {
                        Protocol = parts[0],
                        Port = int.Parse(parts[1].Split(':').Last()),
                        PID = int.Parse(parts[len - 1])
                    });


            }
            return result;
        }
    }
}
