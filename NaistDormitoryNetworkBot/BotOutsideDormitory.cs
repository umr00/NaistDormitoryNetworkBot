using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaistDormitoryNetworkBot
{
    class BotOutsideDormitory
    {
        private bool emergencyStatus;
        private Twitter twitter;
        private List<string> script;
        private string emergencyMessage;
        public int interval = 300000;
        string authfilepath = "authkey.txt";
        string scriptfilepath = "scriptForOutsideDormitory.txt";
        int tweetLimit = 130;
        string lastTweet;
        public BotOutsideDormitory()
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
            if (CheckEmergency())
            {
                emergencyStatus = true;
                twitter.Tweet(emergencyMessage);
                lastTweet = emergencyMessage;
            }
        }
        private void ActionForEmergency()
        {
            emergencyStatus = CheckEmergency();
            if (!emergencyStatus)
            {
                return;
            }
            Random random = new Random();

            int scriptNumber = random.Next(script.Count());
            twitter.Tweet(script[scriptNumber]);
            lastTweet = script[scriptNumber];
            script.RemoveAt(scriptNumber);
            if (script.Count() == 0)
            {
                ReadScript();
            }
        }

        private void ReadScript()
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(scriptfilepath, Encoding.GetEncoding("Shift_JIS")))
            {
                emergencyMessage = sr.ReadLine();
                if (emergencyMessage.Length > tweetLimit)
                {
                    emergencyMessage = emergencyMessage.Remove(tweetLimit);
                }
                var list = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                script = list.Where(s => s.Length <= tweetLimit).ToList<string>();
            }
        }

        private bool CheckEmergency()
        {
            if (!emergencyStatus)
            {
                var span = DateTimeOffset.Now - twitter.LastTweetTime();
                if (span.Minutes > 10)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (twitter.LastTweet().Contains(lastTweet))
                {
                    return true;
                }
                return false;
            }
            //twitter
        }
    }
}
