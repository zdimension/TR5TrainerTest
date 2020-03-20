using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TR5TrainerTest
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            foreach (var f in Directory.GetFiles(Path.GetTempPath(), "TR5Trainer_*"))
            {
                try
                {
                    File.Delete(f);
                }
                catch
                {
                    
                }
            }

            Logger.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        public static MainWindow wnd;
    }
}
