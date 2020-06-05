using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    public class LogManager
    {
        #region singleton boilerplate
        private static volatile LogManager instance;
        private static readonly object syncRoot = new Object();

        private LogManager() { }

        public static LogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new LogManager();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        public void BindConsole()
        {
            FileStream ostrm = new FileStream("log.txt", FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(ostrm)
            {
                AutoFlush = true
            };

            Console.SetOut(writer);

            if (PandaMonogameConfig.Logging)
                Console.WriteLine("Log file created on " + DateTime.Now.ToString());
        }
    }
}
