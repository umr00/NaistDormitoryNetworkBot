using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaistDormitoryNetworkBot
{
    class BotInDormitory
    {

        private bool emergencyStatus;
        private Twitter twitter;
        private List<string> script;
        private string restorationMessage;
        public int interval = 300000;
        string scriptfilepath = "scriptForInDormitory.txt";
        string authfilepath = "authkey.txt";
        int tweetLimit = 130;

        public BotInDormitory()
        {
            emergencyStatus = false;
            twitter = new Twitter(authfilepath);
            ReadScript();
        }
        public void Run()
        {
            while (true)
            {
                if (!emergencyStatus)
                {
                    ActionForNormal();
                }
                else
                {
                    ActionForEmergency();
                }
                System.Threading.Thread.Sleep(interval);
            }
        }

        private void ActionForNormal()
        {
            Random random = new Random();
            if (IsInternetConnected())
            {
                int scriptNumber = random.Next(script.Count());
                twitter.Tweet(script[scriptNumber]);
                script.RemoveAt(scriptNumber);
                if (script.Count() == 0)
                {
                    ReadScript();
                }
            }
            else
            {
                emergencyStatus = true;
            }
        }
        private void ActionForEmergency()
        {
            if (IsInternetConnected())
            {
                twitter.Tweet(restorationMessage);
                emergencyStatus = false;
            }
        }

        private void ReadScript()
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(scriptfilepath, Encoding.GetEncoding("Shift_JIS")))
            {
                restorationMessage = sr.ReadLine();
                if (restorationMessage.Length > tweetLimit)
                {
                    restorationMessage = restorationMessage.Remove(tweetLimit);
                }
                var list = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                script = list.Where(s => s.Length <= tweetLimit).ToList<string>();
            }
        }

        /// <summary>
        /// http://dobon.net/vb/dotnet/internet/detectinternetconnect.html
        /// </summary>
        /// <returns></returns>
        private static bool IsInternetConnected()
        {
            //インターネットに接続されているか確認する
            string host = "http://www.yahoo.com";

            System.Net.HttpWebRequest webreq = null;
            System.Net.HttpWebResponse webres = null;
            try
            {
                //HttpWebRequestの作成
                webreq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(host);
                //メソッドをHEADにする
                webreq.Method = "HEAD";
                //受信する
                webres = (System.Net.HttpWebResponse)webreq.GetResponse();
                //応答ステータスコードを表示
                //Console.WriteLine(webres.StatusCode);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (webres != null)
                    webres.Close();
            }
        }

    }
}
