using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TopX
{
    static class Program
    {
        const string ProcessesToWatchFileName = "ProcessesToWatch.txt";
        static string ProcessesToWatchPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProcessesToWatchFileName);


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var processesToWatch = GetProcessesToWatch();
            Application.Run(new CrosshairOverlay(processesToWatch));
        }


        static IEnumerable<string> GetProcessesToWatch()
        {
            var processesToWatch = new List<string>();

            if (File.Exists(ProcessesToWatchPath))
            {
                try
                {
                    string line;
                    using (var file = new StreamReader(ProcessesToWatchPath))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            line = line.ToLower().TrimEnd(".exe");
                            processesToWatch.Add(line);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Application.Exit();
                }
            }

            return processesToWatch;
        }

        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value))
                return source;

            return source.Remove(source.LastIndexOf(value));
        }

    }
}

