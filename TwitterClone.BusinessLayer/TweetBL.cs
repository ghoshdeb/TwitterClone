using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterClone.DataLayer;
namespace TwitterClone.BusinessLayer
{
    public class TweetBL
    {
        public List<TWEET> GetAll()
        {
            using (TweetContext db = new TweetContext())
            {
                List<TWEET> alltweet = db.tweets.ToList();

                return alltweet;
            }
        }
        //public List<TWEET> GetById(string id)
        //{

        //}
        public List<TWEET> GetById(string id)
        {
            using (TweetContext db = new TweetContext())
            {
                UserBL obj = new UserBL();
                List<string> followingList = obj.GetAllFollowingId(id);
                followingList.Add(id);
                List<TWEET> alltweetByUsers = new List<TWEET>();
                foreach (var item in followingList)
                {
                    var query = from p in db.tweets
                                where p.user_id == item
                                select new { user_id = p.user_id, message = p.message, created = p.created, tweet_id=p.tweet_id };
                    List<TWEET> alltweetByUserstweet = query.ToList().Select(r => new TWEET
                    {
                        tweet_id=r.tweet_id,
                        user_id = r.user_id,
                        message = r.message,
                        created = r.created
                    }).ToList();
                   
                    foreach (var item1 in alltweetByUserstweet)
                    {
                        alltweetByUsers.Add(item1);
                    }
                }
                //alltweetByUsers = alltweetByUsers.OrderByDescending(o => o.created).ToList();

                return alltweetByUsers.OrderByDescending(o => o.created).ToList();
            }

        }
        public void AddTweet(TWEET tweet)
        {
            using (TweetContext db = new TweetContext())
            {
                db.tweets.Add(tweet);
                db.SaveChanges();
            }

        }
        public void UpdateTweet(TWEET tweet)
        {
            using (TweetContext db = new TweetContext())
            {
                TWEET obj = db.tweets.Find(tweet.tweet_id);
                obj.message = tweet.message;
                obj.created = tweet.created;               
                db.SaveChanges();
            }

        }
        public void DeleteTweet(TWEET tweet)
        {
            using (TweetContext db = new TweetContext())
            {
                TWEET obj = db.tweets.Find(tweet.tweet_id);
                db.tweets.Remove(obj);
                db.SaveChanges();
            }

        }
    }
}
