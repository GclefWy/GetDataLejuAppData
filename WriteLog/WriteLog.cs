using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteLog
{
    public class WriteLog
    {
        public static void WLog(string log)
        {

            //string str = System.Environment.CurrentDirectory;
            if (File.Exists(Environment.CurrentDirectory + @"/log/log-" + DateTime.Today.ToShortDateString().Replace("/", "-") + ".txt"))
            {
                string strFilePath = Environment.CurrentDirectory + @"/log/log-" + DateTime.Today.ToShortDateString().Replace("/", "-") + ".txt";
                FileStream fs = new FileStream(strFilePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.WriteLine(DateTime.Now.ToString() + "|" + log);
                sw.Close();
                fs.Close();
            }
            else
            {
                string strFilePath = Environment.CurrentDirectory + @"/log/log-" + DateTime.Today.ToShortDateString().Replace("/", "-") + ".txt";
                File.Create(strFilePath).Dispose();
                FileStream fs = new FileStream(strFilePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.WriteLine(DateTime.Now.ToString() + "|" + log);
                sw.Close();
                fs.Close();
            }
        }
    }
}
