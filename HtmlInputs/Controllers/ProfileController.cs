using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlInputs.Models;
namespace HtmlInputs.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index()
        {
            if(Session["LogedUserRole"].ToString().Equals("4"))
            {
                return RedirectPermanent("~/Profile/Methodist");
            }
            DiplomEntities5 dc = new DiplomEntities5();
            ViewBag.news = dc.News.OrderByDescending(a => a.Id);
            
            return View();
        }
        public ActionResult AddNews()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNews(News getNews)
        {
            if(ModelState.IsValid == true)
            { 
                DiplomEntities5 dc = new DiplomEntities5();
                dc.News.Add(getNews);
                dc.SaveChanges();
                getNews = null;
                ViewBag.Message = "Новость успешно опубликована";
            }
            return View(getNews);
        }
        public ActionResult WhoIsUserMiss()
        {
            DiplomEntities5 dc = new DiplomEntities5();
            int SessionRole = Convert.ToInt32(Session["LogedUserRole"].ToString());
            var userRole = dc.Roles.Where(a => a.RoleId.Equals(SessionRole)).FirstOrDefault();
            switch(userRole.RoleId.ToString())
            {
                case "1":
                    return Redirect("~/Profile/MissForStudents");
                case "2":
                    return Redirect("~/Profile/MissForPraepostor");
                case "3":
                    return Redirect("~/Profile/MissForModerator");
                case "4":
                    return Redirect("~/Profile/MissForMethodist");
                case "5":
                    return Redirect("~/Profile/MissForZDean");
                case "6":
                    return Redirect("~/Profile/MissForDean");

            }
                return Redirect("~/Home/Index");
            
        }
        public ActionResult Methodist()
        {
            DiplomEntities5 dc = new DiplomEntities5();
            ViewBag.news = dc.News.OrderByDescending(a => a.Id);
            return View();
        }
        public ActionResult MethodistStudents()
        {
            return View();
        }
        public ActionResult MissForDean()
        {
            return View();
        }
        public ActionResult MissForZDean()
        {
            return View();
        }
        public ActionResult MissForMethidist()
        {
            //Пока нет
            return View();
        }
        public ActionResult MissForModerator()
        {
            return View();
        }
        public ActionResult MissForStudents()
        {
            DiplomEntities5 dc = new DiplomEntities5();
            ViewBag.miss = dc.Missings;

            return View();
        }
        public ActionResult MissForPraepostor()
        {
            return View();
        }
        public ActionResult CourseOne()
        {
            Session["NumCourse"] = 1;
            return View("ChooseGroup");
        }
        public ActionResult CourseTwo()
        {
            Session["NumCourse"] = 2;
            return View("ChooseGroup");
        }
        public ActionResult CourseThree()
        {
            Session["NumCourse"] = 3;
            return View("ChooseGroup");
        }
        public ActionResult CourseFour()
        {
            Session["NumCourse"] = 4;
            return View("ChooseGroup");
        }
        public ActionResult GroupOne()
        {
            Session["NumGroup"] = 1;
            return View("GroupMiss");
        }
        public ActionResult GroupTwo()
        {
            Session["NumGroup"] = 2;
            return View("GroupMiss");
        }
        public ActionResult GroupThree()
        {
            Session["NumGroup"] = 3;
            return View("GroupMiss");
        }
        public ActionResult GroupOneMethodist()
        {
            Session["NumGroup"] = 1;
            return View("GroupList");
        }
        public ActionResult GroupTwoMethodist()
        {
            Session["NumGroup"] = 2;
            return View("GroupList");
        }

        public ActionResult GroupThreeMethodist()
        {
            Session["NumGroup"] = 3;
            return View("GroupList");
        }
        public ActionResult EditGroup()
        {
            return View("GroupListEdit");
        }
        public ActionResult WeekReportForZDean()
        {
            return View();
        }
        public ActionResult MonthReportForZDean()
        {
            return View();
        }
        public ActionResult WhoIsUserApp()
        {
            if(Session["LogedUserRole"].ToString().Equals("1") ||
                Session["LogedUserRole"].ToString().Equals("2"))
            {
                return Redirect("~/Profile/Appliance");
            }
            else if(Session["LogedUserRole"].ToString().Equals("5"))
            {
                return Redirect("~/Profile/ApplianceForZDean");
            }
            return null;
        }
        public ActionResult Appliance()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Appliance(ApplicationView getApp)
        {
            Applications newApp = new Applications();
            newApp.StudentId = Convert.ToInt32(Session["LogedUserId"].ToString());
            newApp.Head = "Декану факультета инновационной подготовки Зеневич А. М. ";
            if(getApp.isMale == 1)
            {
                newApp.Head += "Студента ";
            }
            else
            {
                newApp.Head += "Студентки ";
            }
            newApp.Head += Session["LogedUserCourse"].ToString();
            newApp.Head += " курса дневной формы обучения ";
            newApp.Head += "специальности управление информационными ресурсами ";
            newApp.Head += "Института управленческих кадров";
            newApp.Reason = getApp.Reason;
            newApp.isRead = 0;
            newApp.DateOfCreation = DateTime.Now;
            newApp.DateOfMiss = new DateTime(getApp.YearOfMiss, getApp.MonthOfMiss, getApp.DayOfMiss);
            DiplomEntities5 dc = new DiplomEntities5();
            dc.Applications.Add(newApp);
            dc.SaveChanges();
            ViewBag.Message = "Заявление успешно отправлено.";
            return View();
        }
        public ActionResult ApplianceForZDean()
        {
            return View();
        }
        public ActionResult CourseOneApp()
        {
            Session["CourseApp"] = 1;
            return View("GroupListApp");
        }
        public ActionResult CourseTwoApp()
        {
            Session["CourseApp"] = 2;
            return View("GroupListApp");
        }
        public ActionResult CourseTreeApp()
        {
            Session["CourseApp"] = 3;
            return View("GroupListApp");
        }
        public ActionResult CourseFourApp()
        {
            Session["CourseApp"] = 4;
            return View("GroupListApp");
        }
        public ActionResult GroupOneApp()
        {
            Session["GroupApp"] = 1;
            DiplomEntities5 dc = new DiplomEntities5();
            int course = Convert.ToInt32(Session["CourseApp"]);
            var GroupOne = dc.Applications.Where(a => a.Users.Group == 1 &&
                                                a.Users.Course == course).ToList<Applications>();
            return View("ViewApplications",GroupOne);
        }
        public ActionResult GroupTwoApp()
        {
            Session["GroupApp"] = 2;
            DiplomEntities5 dc = new DiplomEntities5();
            int course = Convert.ToInt32(Session["CourseApp"]);
            var GroupTwo = dc.Applications.Where(a => a.Users.Group == 2 &&
                                                a.Users.Course == course).ToList<Applications>();
            return View("ViewApplications", GroupTwo);
        }
        public ActionResult GroupThreeApp()
        {
            Session["GroupApp"] = 3;
            DiplomEntities5 dc = new DiplomEntities5();
            int course = Convert.ToInt32(Session["CourseApp"]);
            var GroupThree = dc.Applications.Where(a => a.Users.Group == 3 &&
                                                a.Users.Course == course).ToList<Applications>();
            return View("ViewApplications", GroupThree);
        }
        [HttpPost]
        public ActionResult SaveChangesApp(List<Applications> getChanges)
        {
            DiplomEntities5 dc = new DiplomEntities5();
            return View("ViewApplications");
        }
        public ActionResult Messages()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Messages(Messages newMessage)
        {
            using(DiplomEntities5 dc = new DiplomEntities5())
            {
                if(ModelState.IsValid == true)
                {
                    dc.Messages.Add(newMessage);
                    dc.SaveChanges();
                    newMessage = null;
                }
            }
            return View(newMessage);
        }

        public ActionResult AddMiss()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMiss(AddMiss getMiss)
        {
            if(ModelState.IsValid == true)
            {
                DiplomEntities5 dc = new DiplomEntities5();
                Missings newRow = new Missings();
                newRow.StudentId = getMiss.StudentId;
                newRow.Form = getMiss.Form;
                newRow.Discipline = getMiss.Discipline;
                newRow.IsRead = getMiss.IsRead;
                newRow.IsValid = getMiss.IsValid;
                newRow.Date = new DateTime(getMiss.Year, getMiss.Month, getMiss.Day);
                dc.Missings.Add(newRow);
                dc.SaveChanges();
                ViewBag.Message = "Запись добавлена";
                getMiss = null;
            }
            return View(getMiss);
            
        }
        public ActionResult WeekReport()
        {
            return View();
        }
        public ActionResult WeekReportForStudent()
        {
            return View();
        }
        public ActionResult WeekReportForModerator()
        {
            return View();
        }
        public ActionResult MonthReportForStudent()
        {
            return View();
        }
        public ActionResult MonthReportForPraepostor()
        {
            return View();
        }
        public ActionResult MonthReportForModerator()
        {
            return View();
        }
        public ActionResult Changes()
        {
            if(Session["LogedUserRole"].ToString().Equals("4"))
            {
                return Redirect("~/Profile/ChangesForMethodist");
            }
            DiplomEntities5 dc = new DiplomEntities5();
            foreach(Changes item in dc.Changes)
            {
                if(item.isRead == 0 && item.UserId.ToString().Equals(Session["LogedUserId"].ToString()))
                {
                    item.isRead = 1;
                }
            }
            dc.SaveChanges();
            ViewBag.chan = dc.Changes;
            return View();
        }
        public ActionResult ChangesForMethodist()
        {
            DiplomEntities5 dc = new DiplomEntities5();
            ViewBag.chan = dc.Changes.OrderByDescending(a => a.Id);
            return View();
        }
        public ActionResult AddChanges()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddChanges(Changes getChanges)
        {
            DiplomEntities5 dc = new DiplomEntities5();
            List<Changes> RecordSet = new List<Changes>();
            foreach(Users item in dc.Users)
            {
                if(item.RoleId == 1 || item.RoleId == 2 || item.RoleId == 4)
                {
                    var obj = new Changes();
                    obj.UserId = item.UserId;
                    obj.isRead = 0;
                    obj.Date = DateTime.Now;
                    obj.Content = getChanges.Content;
                    RecordSet.Add(obj);
                }
            }
            foreach(Changes item in RecordSet)
            {
                dc.Changes.Add(item);
                dc.SaveChanges();
            }
            getChanges = null;
            ViewBag.Message = "Изменение успешно опубликовано";
            return View(getChanges);
        }
    }
}
