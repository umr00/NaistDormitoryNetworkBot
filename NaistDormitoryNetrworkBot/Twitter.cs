using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;

namespace NaistDormitoryNetrworkBot
{
    class Twitter
    {
        private CoreTweet.Tokens tokens;
        public Twitter(string authkeyFilePath)
        {
            var sr = new System.IO.StreamReader(authkeyFilePath);
            string ConsumerKey = sr.ReadLine();
            string ConsumerSecret = sr.ReadLine();
            string AccessToken = sr.ReadLine();
            string AccessSecret = sr.ReadLine();
            tokens = CoreTweet.Tokens.Create(ConsumerKey, ConsumerSecret, AccessToken, AccessSecret);
            
        }

        public bool Tweet(string text)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(text);
            sb.Append(" [");
            sb.Append(DateTime.Now.ToString("HH時mm分]"));

            try
            {
                tokens.Statuses.Update(status => sb.ToString());
            }
            catch
            {
                return false;
            }
            return true;
        }
        public DateTimeOffset LastTweetTime()
        {
            var tweets = tokens.Statuses.UserTimeline(screen_name => "hinako_at_nara");
            return tweets[0].CreatedAt;
        }
        public String LastTweet()
        {
            var tweets = tokens.Statuses.UserTimeline(screen_name => "hinako_at_nara");
            return tweets[0].Text;
        }

    }
}
