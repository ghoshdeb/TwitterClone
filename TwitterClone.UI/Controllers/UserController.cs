using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterClone.UI.Models;
using TwitterClone.BusinessLayer;
using TwitterClone.DataLayer;
using TwitterClone.UI.CustomFilter;
namespace TwitterClone.UI.Controllers
{
    public class UserController : Controller
    {
        UserBL obj = new UserBL();
        TweetBL tweetObj = new TweetBL();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ViewResult signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult signup(PersonVM item)
        {
            if (ModelState.IsValid)
            {
                PERSON p = new PERSON()
                {
                    user_id = item.Username,
                    password = item.Pwd,
                    email = item.Email,
                    fullName = item.Name,
                    active = true,
                    joined = DateTime.Now
                };
                List<string> listAllUsers = obj.GetAllUserId(p.user_id);
                if (listAllUsers.Count > 0)
                {
                   Session["ExistingUser"] = "Username already exist  Please try different name";
                    return View();
                }
                else
                {
                    obj.AddUser(p);
                    return RedirectToAction("Login");
                }
                

            }
            else
                return View();
        }
        public ViewResult Login()
        {
            Session["UserId"] = null;
            Session["Username"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string uname, string pwd)
        {
            PERSON p = obj.Validate(uname, pwd);
            if (p != null)
            {
                Session["UserId"] = p.user_id;
                Session["Username"] = p.fullName;
                return RedirectToAction("home");
            }
            else
            {
                TempData["err"] = "Invalid Login Details";
                return View();
            }
        }
        [MyErrorHandler]
        public ActionResult home()
        {
            if (Session["UserId"].ToString() != null)
            {
                if (ModelState.IsValid)
                {
                    PERSON p = obj.GetByID(Session["UserId"].ToString());

                    Session["MessageCount"] = tweetObj.GetById(p.user_id).Count();
                    ViewBag.Followers = obj.GetAllFollowerId(p.user_id).Count();
                    ViewBag.Following = obj.GetAllFollowingId(p.user_id).Count();
                    p.tweet = tweetObj.GetById(Session["UserId"].ToString());
                    return View();

                }
                else
                    return RedirectToAction("Login");
            }
            else
                return View("Login");
        }
        public ViewResult profile(string Username)
        {
            Username = Session["UserId"].ToString();
            if (Username != null)
            {
                if (ModelState.IsValid)
                {
                    PERSON p = new PERSON()
                    {
                        user_id = Username,

                    };
                    PERSON per = obj.GetByID(p.user_id);
                    Session["joinDate"] = per.joined;
                    PersonVM Pvm = new PersonVM()
                    {
                        Username = per.user_id,
                        Pwd = per.password,
                        Name = per.fullName,
                        Email = per.email
                    };
                    return View(Pvm);
                }

                else
                {
                    return View();
                }
            }
            else
                return View("Login");

        }
        [HttpPost]
        public ActionResult profile(PersonVM pv)
        {
            if (ModelState.IsValid)
            {
                PERSON p = new PERSON()
                {
                    user_id = pv.Username,
                    password = pv.Pwd,
                    email = pv.Email,
                    fullName = pv.Name,
                    active = true,
                    joined = (DateTime)Session["joinDate"]
                };
                obj.Edit(p);
                return RedirectToAction("Login");

            }
            else
                return View();
        }
        [HttpPost]
        public JsonResult DeleteProfile(string userid)
        {
            PERSON p = obj.GetByID(userid);
            obj.DeleteProfile(p.user_id);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllTweet()
        {

            return Json(tweetObj.GetById(Session["UserId"].ToString()), JsonRequestBehavior.AllowGet);

        }
        
        public JsonResult AddTweet(TWEET tweet)
        {
            tweet.user_id = Session["UserId"].ToString();
            tweet.created = DateTime.Now;
            tweetObj.AddTweet(tweet);
            Session["MessageCount"] = tweetObj.GetById(tweet.user_id).Count();
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateTweet(TWEET tweet)
        {
            tweet.user_id = Session["UserId"].ToString();
            tweet.created = DateTime.Now;
            tweetObj.UpdateTweet(tweet);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteTweet(TWEET tweet)
        {          
            tweetObj.DeleteTweet(tweet);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchFollowing(string name)
       {
            List<string> allUsers = obj.GetAllUserId(name);
            foreach(var users in allUsers)
            {
                if(users == Session["UserId"].ToString())
                {
                    allUsers.Remove(users);
                    break;
                }
               
            }
            return Json(allUsers, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchUnFollowing(string name)
        {
            List<string> allUsers = obj.SearchAllFollowingList(name, Session["UserId"].ToString());
            foreach (var users in allUsers)
            {
                if (users == Session["UserId"].ToString())
                {
                    allUsers.Remove(users);
                    break;
                }

            }
            return Json(allUsers, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult following(string Search)
        {
            PERSON following = obj.GetByID(Search);
            PERSON person = obj.GetByID(Session["UserId"].ToString());            
            obj.Following(person, following);
            return RedirectToAction("home");
        }
        public ActionResult UnFollowing(string Search)
        {
            PERSON following = obj.GetByID(Search);
            PERSON follower = obj.GetByID(Session["UserId"].ToString());
            obj.UnFollowing(following, follower);
            return RedirectToAction("home");
        }


    }
}