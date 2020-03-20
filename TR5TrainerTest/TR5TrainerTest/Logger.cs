using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TR5TrainerTest
{
    public static class Logger
    {
        public static FileStream fp;

        public static readonly string filename = Path.Combine(Path.GetTempPath(), "TR5Trainer_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".txt");

        public static void Init()
        {
            fp = File.Create(filename);
        }

        public static void Log(string fmt, params object[] args)
        {
            if (!fp.CanWrite)
            {
                fp = File.OpenWrite(filename);
            }

            using (var sw = new StreamWriter(fp, Encoding.UTF8))
                sw.WriteLine("[" + DateTime.Now.ToString("s") + "] " + fmt, args);
        }

        public static void Close()
        {
            fp.Close();
        }
    }
}
