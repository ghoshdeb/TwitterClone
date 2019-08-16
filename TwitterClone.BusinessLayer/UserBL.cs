using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterClone.DataLayer;
namespace TwitterClone.BusinessLayer
{
    public class UserBL
    {
        public void AddUser(PERSON item)
        {
            using (TweetContext db = new TweetContext())
            {
                db.persons.Add(item);
                db.SaveChanges();
            }
        }
        public PERSON Validate(string uname, string pwd)
        {
            using (TweetContext db = new TweetContext())
            {
                PERSON obj = db.persons.SingleOrDefault(i => i.user_id == uname && i.password == pwd);            
                return obj;
            }
        }
        public PERSON GetByID(string id)
        {
            using (TweetContext db = new TweetContext())
            {
                PERSON obj = db.persons.Find(id);
                return obj;
            }

        }
        public void DeleteProfile(string id)
        {
            using (TweetContext db = new TweetContext())
            {
                PERSON obj = db.persons.Include("followers").Include("following").Include("tweet").Single(i => i.user_id == id);
                db.persons.Remove(obj);
                db.SaveChanges();
            }
        }
        public void Edit(PERSON person)
        {
            using (TweetContext db = new TweetContext())
            {
                PERSON obj = db.persons.Find(person.user_id);
                obj.fullName = person.fullName;
                obj.password = person.password;
                obj.email = person.email;                
                db.SaveChanges();

            }

        }
        public List<string> GetAllUserId(string name)
        {
            using (TweetContext db = new TweetContext())
            {
                List<string> listId= db.persons.Where(i => i.user_id.StartsWith(name)).Select(i => i.user_id).Distinct().ToList();
                return listId;
            }
        }
        
        public void Following( PERSON followingPerson, PERSON followerPerson)
        {
            using (TweetContext db = new TweetContext())
            {
                //PERSON obj = db.persons.Find(person.user_id);

                PERSON objFollowing = db.persons.Include("followers").Include("following").Single(i=>i.user_id== followingPerson.user_id);
                PERSON objFollowers = db.persons.Include("followers").Include("following").Single(i => i.user_id == followerPerson.user_id);
                 List<PERSON> followingList = new List<PERSON>();
                foreach(var item in objFollowers.followers)
                {
                    followingList.Add(item);
                }
                 followingList.Add(objFollowing);
                 //followerList.Add(followerPerson);

                 objFollowers.followers = followingList;
                

                db.SaveChanges();
            }
        }
        public void UnFollowing(PERSON followingPerson, PERSON followerPerson)
        {
            using (TweetContext db = new TweetContext())
            {
                //PERSON obj = db.persons.Find(person.user_id);

                PERSON objFollowing = db.persons.Include("followers").Include("following").Single(i => i.user_id == followingPerson.user_id);
                PERSON objFollowers = db.persons.Include("followers").Include("following").Single(i => i.user_id == followerPerson.user_id);
                List<PERSON> followingList = new List<PERSON>();
                foreach (var item in objFollowers.following)
                {
                    if (item == objFollowing)
                    {
                        objFollowers.following.Remove(objFollowing);
                        break;
                    }
                    
                }
                //followingList.Add(objFollowing);
                ////followerList.Add(followerPerson);

                //objFollowers.followers = followingList;


                db.SaveChanges();
            }
        }
        public List<string> GetAllFollowerId(string name)
        {
            using (TweetContext db = new TweetContext())
            {

                PERSON obj = db.persons.Include("followers").Include("following").Single(i => i.user_id == name);
                List<string> followerList = new List<string>();
                foreach(var follower in obj.followers)
                {
                    followerList.Add(follower.user_id);
                }
                return followerList;
            }
        }
        public List<string> GetAllFollowingId(string name)
        {
            using (TweetContext db = new TweetContext())
            {

                PERSON obj = db.persons.Include("followers").Include("following").Single(i => i.user_id == name);
                List<string> followingList = new List<string>();
                foreach (var following in obj.following)
                {
                    followingList.Add(following.user_id);
                }
                return followingList;
            }
        }
        public List<PERSON> GetAllFollowingDetails(string name)
        {
            using (TweetContext db = new TweetContext())
            {
                PERSON objFollowing = db.persons.Include("followers").Include("following").Single(i => i.user_id == name);                
                List<PERSON> followingList = new List<PERSON>();
                foreach (var item in objFollowing.following)
                {
                    followingList.Add(item);
                }
                return followingList;
            }
        }
        public List<string> SearchAllFollowingList(string SearchName,string Username)
        {
            UserBL obj = new UserBL();
            List<string>allFollowingId= obj.GetAllFollowingId(Username);
            List<string> searchId = allFollowingId.Where(i => i.StartsWith(SearchName)).ToList();
            return searchId;
        }
        

    }
}
