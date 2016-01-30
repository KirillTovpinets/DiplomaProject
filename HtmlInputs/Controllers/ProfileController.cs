using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlInputs.Models;
using System.Web.UI.DataVisualization;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
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
        public ActionResult ShowAvatar()
        {
            DiplomEntities5 dc = new DiplomEntities5();
            int Login;
            if(Request["id"] != null)
            {
                Login = Convert.ToInt32(Request["id"]);
            }
            else
            {
                Login = Convert.ToInt32(Session["LogedUserId"]);
            }
            var v = dc.Users.Where(a => a.UserId.Equals(Login)).FirstOrDefault();
            byte[] AvatarBytes = v.Avatar;
            Stream mc = new MemoryStream(AvatarBytes);
            Image avka = Image.FromStream(mc);

            avka.Save(this.Response.OutputStream, ImageFormat.Jpeg);
            avka.Dispose();
            return null;
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

        public ActionResult EditNews()
        {
            DiplomEntities5 dc = new DiplomEntities5();
            List<News> ListNews = new List<News>();
            ListNews = dc.News.OrderByDescending(a => a.Id).ToList();
            return View(ListNews);
        }

        [HttpPost]
        public ActionResult EditNews(List<News> getChanges)
        {
            DiplomEntities5 dc = new DiplomEntities5();
            foreach(News item in getChanges)
            {
                var DbItem = dc.News.Where(a => a.Id.Equals(item.Id)).FirstOrDefault();
                if(DbItem != item)
                {
                    DbItem.Title = item.Title;
                    DbItem.Content = item.Content;
                    DbItem.Date = item.Date;
                    dc.SaveChanges();
                }
            }
            string mess = "Изменения успешно сохранены";
            return Redirect("~/Profile/Methodist?Message=" + mess);
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
            if(Request["Message"] != null)
            {
                ViewBag.Message = Request["Message"].ToString();
            }
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
        public ActionResult DeleteMiss()
        {
            int Id = Convert.ToInt32(Request["id"]);
            DiplomEntities5 dc = new DiplomEntities5();
            Missings RowToDel = dc.Missings.Where(a => a.Id.Equals(Id)).FirstOrDefault();
            dc.Missings.Remove(RowToDel);
            dc.SaveChanges();
            return View("MissForPraepostor");
        }
        public ActionResult EditMiss()
        {
            
            List<Missings> MissingDb = new List<Missings>();
            DiplomEntities5 dc = new DiplomEntities5();
            foreach (HtmlInputs.Models.Missings item in dc.Missings.OrderByDescending(a => a.Id))
            {
                if (item.Users.Group.ToString().Equals(Session["LogedUserGroup"].ToString()) && item.Users.Course.ToString().Equals(Session["LogedUserCourse"].ToString()))
                {
                    MissingDb.Add(item);
                }
            }
            return View(MissingDb);
        }
        [HttpPost]
        public ActionResult EditMiss(List<Missings> getChanges)
        {
            DiplomEntities5 dc = new DiplomEntities5();
            foreach (Missings miss in getChanges)
            {
                var DbMiss = dc.Missings.Where(a => a.Id.Equals(miss.Id)).FirstOrDefault();
                //var element = 
                if (!DbMiss.IsValid.Equals(miss.IsValid))
                {
                    DbMiss.IsValid = miss.IsValid;
                }
            }
            ViewBag.Message = "Изменения успешно сохранены";
            dc.SaveChanges();
            List<Missings> MissingDb = new List<Missings>();
            foreach (HtmlInputs.Models.Missings item in dc.Missings.OrderByDescending(a => a.Id))
            {
                if (item.Users.Group.ToString().Equals(Session["LogedUserGroup"].ToString()) && item.Users.Course.ToString().Equals(Session["LogedUserCourse"].ToString()))
                {
                    MissingDb.Add(item);
                }
            }
            return View(MissingDb);
        }
        [HttpGet]
        public ActionResult SearchSirname()
        {
            DiplomEntities5 dc = new DiplomEntities5();
            string sirname = Request["Sirname"].ToString();
            var Db = dc.Missings.Where(a => a.Users.Sirname.Equals(sirname)).ToList();
            return View("EditMiss",Db);
        }
        public ActionResult SearchName()
        {
            DiplomEntities5 dc = new DiplomEntities5();
            string name = Request["Name"].ToString();
            var Db = dc.Missings.Where(a => a.Users.Name.Equals(name)).ToList();
            return View("EditMiss", Db);
        }
        public ActionResult SearchDate()
        {
            int year = Convert.ToInt32(Request["year"].ToString());
            int month = Convert.ToInt32(Request["month"].ToString());
            int day = Convert.ToInt32(Request["day"].ToString());
            DateTime date = new DateTime(year, month, day);
            DiplomEntities5 dc = new DiplomEntities5();
            var Db = dc.Missings.Where(a => a.Date.Equals(date)).ToList();
            return View("EditMiss", Db);
        }
        public ActionResult SearchForm()
        {
            DiplomEntities5 dc = new DiplomEntities5();
            string form = Request["Form"].ToString();
            var Db = dc.Missings.Where(a => a.Form.Equals(form)).ToList();
            return View("EditMiss", Db);
        }
        public ActionResult SearchDis()
        {
            DiplomEntities5 dc = new DiplomEntities5();
            string dis = Request["Dis"].ToString();
            var Db = dc.Missings.Where(a => a.Discipline.Equals(dis)).ToList();
            return View("EditMiss", Db);
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
            List<Users> GroupDb = new List<Users>();
                HtmlInputs.Models.DiplomEntities5 dc = new HtmlInputs.Models.DiplomEntities5();
                foreach (HtmlInputs.Models.Users item in dc.Users.OrderBy(a => a.Sirname))
                {
                    if (item.Group.ToString().Equals(Session["NumGroup"].ToString()) &&
                    item.Course.ToString().Equals(Session["NumCourse"].ToString()) &&
                    item.RoleId != 3)
                    {
                        GroupDb.Add(item);
                    }
                }
            return View("GroupListEdit",GroupDb);
        }
        [HttpPost]
        public ActionResult EditGroup(List<Users> getStudent)
        {
            DiplomEntities5 dc = new DiplomEntities5();
            foreach (Users stud in getStudent)
            {
                var DbStud = dc.Users.Where(a => a.UserId.Equals(stud.UserId)).FirstOrDefault();
                if (!DbStud.RoleId.Equals(stud.RoleId))
                {
                    DbStud.RoleId = stud.RoleId;
                }
            }
            ViewBag.Message = "Изменения успешно сохранены";
            dc.SaveChanges();
            if(Session["NumGroup"].ToString().Equals("1"))
            {
                return Redirect("~/Profile/GroupOneMethodist");
            }
            else if (Session["NumGroup"].ToString().Equals("2"))
            {
                return Redirect("~/Profile/GroupTwoMethodist");
            }
            else if (Session["NumGroup"].ToString().Equals("3"))
            {
                return Redirect("~/Profile/GroupThreeMethodist");
            }
            return null;
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
                return Redirect("~/Profile/ShowOrWriteApp");
            }
            else if(Session["LogedUserRole"].ToString().Equals("5"))
            {
                return Redirect("~/Profile/ApplianceForZDean");
            }
            return null;
        }
        public ActionResult ShowOrWriteApp()
        {
            return View();
        }
        public ActionResult ShowAnsApp()
        {
            return View();
        }
        public ActionResult Appliance()
        {
            DiplomEntities5 dc = new DiplomEntities5();
            DateTime date = DateTime.Now;
            DateTime startWeek = date;
            DateTime endWeek = date;
            int numApp = 0;
            int AllowAmountApp = 0;
            while(startWeek.DayOfWeek != System.DayOfWeek.Monday)
            {
                startWeek = startWeek.AddDays(-1);
            }
            foreach(Applications item in dc.Applications)
            {
                DateTime temp = item.DateOfCreation;
                while(temp.DayOfWeek != System.DayOfWeek.Monday)
                {
                    temp = temp.AddDays(-1);
                }
                if(temp == startWeek)
                {
                    numApp++;
                }
            }
            AllowAmountApp = 5 - numApp;
            if(AllowAmountApp < 0)
            {
                ViewBag.NumAllowApp = 0;
            }
            else
            {
                ViewBag.NumAllowApp = numApp;
            }
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
            newApp.DateOfMiss = getApp.DateOfMiss;
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
        public ActionResult CourseThreeApp()
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
                                                a.Users.Course == course).OrderByDescending(a => a.Id).ToList<Applications>();
            return View("ViewApplications",GroupOne);
        }
        public ActionResult GroupTwoApp()
        {
            Session["GroupApp"] = 2;
            DiplomEntities5 dc = new DiplomEntities5();
            int course = Convert.ToInt32(Session["CourseApp"]);
            var GroupTwo = dc.Applications.Where(a => a.Users.Group == 2 &&
                                                a.Users.Course == course).OrderByDescending(a => a.Id).ToList<Applications>();
            return View("ViewApplications", GroupTwo);
        }
        public ActionResult GroupThreeApp()
        {
            Session["GroupApp"] = 3;
            DiplomEntities5 dc = new DiplomEntities5();
            int course = Convert.ToInt32(Session["CourseApp"]);
            var GroupThree = dc.Applications.Where(a => a.Users.Group == 3 &&
                                                a.Users.Course == course).OrderByDescending(a => a.Id).ToList<Applications>();
            return View("ViewApplications", GroupThree);
        }
        [HttpPost]
        public ActionResult SaveChangesApp(List<Applications> getChanges)
        {
            DiplomEntities5 dc = new DiplomEntities5();
            foreach(Applications app in getChanges)
            {
                if(app.isRead == 0)
                { 
                    var appToChange = dc.Applications.Where(a => a.Id.Equals(app.Id)).FirstOrDefault();
                    appToChange.isConfirmed = app.isConfirmed;
                    appToChange.DateOfRead = DateTime.Now;
                    appToChange.isRead = 1;
                    dc.SaveChanges();
                }
            }
            ViewBag.Message = "Изменения успешно сохранены";
            int course = Convert.ToInt32(Session["CourseApp"]);
            int group = Convert.ToInt32(Session["GroupApp"]);
            var Group = dc.Applications.Where(a => a.Users.Group == group &&
                                                a.Users.Course == course).OrderByDescending(a => a.Id).ToList<Applications>();
            return View("ViewApplications", Group);
        }
        public ActionResult WhomToMessage()
        {
            return View();
        }
        public ActionResult MessageStudent()
        {
            Session["WhomToMess"] = 1;
            return View();
        }
        public ActionResult CourseOneMess()
        {
            Session["CourseMess"] = 1;
            return View("ChooseGroupMess");
        }
        public ActionResult CourseTwoMess()
        {
            Session["CourseMess"] = 2;
            return View("ChooseGroupMess");
        }
        public ActionResult CourseThreeMess()
        {
            Session["CourseMess"] = 3;
            return View("ChooseGroupMess");
        }
        public ActionResult CourseFourMess()
        {
            Session["CourseMess"] = 4;
            return View("ChooseGroupMess");
        }
        public ActionResult GroupOneMess()
        {
            Session["GroupMess"] = 1;
            return Redirect("~/Profile/MessagesStaff");
        }
        public ActionResult GroupTwoMess()
        {
            Session["GroupMess"] = 2;
            return Redirect("~/Profile/MessagesStaff");
        }
        public ActionResult GroupThreeMess()
        {
            Session["GroupMess"] = 3;
            return Redirect("~/Profile/MessagesStaff");
        }
        public ActionResult MessageDeans()
        {
            Session["WhomToMess"] = 2;
            Session["CourseMess"] = null;
            Session["GroupMess"] = null;
            return Redirect("~/Profile/MessagesStaff");
        }
        public ActionResult MessagesStaff()
        {
            return View();
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
                newRow.Date = getMiss.Date;
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
            ViewBag.chan = dc.Changes.OrderByDescending(a => a.Id);
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
                    if(item.RoleId != 4)
                    { 
                        obj.isRead = 0;
                    }
                    else
                    {
                        obj.isRead = 1;
                    }
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
        public ActionResult MonthStatistics()
        {
            return View();
        }
        public FileContentResult getPersonalMonthStatistics()
        {
            var getFile = getMonthStatistics(1);
            return getFile;
        }
        public FileContentResult getMonthStatistics(int isStudent = 0)
        {
            DiplomEntities5 dc = new DiplomEntities5();
            int[] months = new int[12];
            int isSaved = 0;
            int numMonths = 0;
            foreach (HtmlInputs.Models.Missings item in dc.Missings.OrderByDescending(a => a.Id)) //Разбиваем записи таблицы на недели. В массив записываем дату понедельника
            {
                if(isStudent == 0)
                { 
                    if (item.Users.Group.ToString() != Session["LogedUserGroup"].ToString() &&
                        item.Users.Course.ToString() != Session["LogedUserCourse"].ToString())
                    {
                        continue;
                    }
                }
                else
                {
                    int student = Convert.ToInt32(Session["LogedUserId"]);
                    if (item.Users.Group.ToString() != Session["LogedUserGroup"].ToString() &&
                        item.Users.Course.ToString() != Session["LogedUserCourse"].ToString() &&
                        item.Users.UserId != student)
                    {
                        continue;
                    }
                }
                int temp;
                temp = item.Date.Month;

                foreach (int savedWeeks in months)
                {
                    if (savedWeeks == temp)
                    {
                        isSaved = 1;
                        break;
                    }
                }
                if (isSaved == 0)
                {
                    months[numMonths++] = temp;
                }
                else
                {
                    isSaved = 0;
                }
            }
                int[] IsValidGroupMiss = new int[50]; //Массив определяет количество недель, ключ - ИД стдуента, значение - кол-во пропусков
                int[] IsNotValidGroupMiss = new int[50];
                List<string> ListMonth = new List<string>();
                int group = Convert.ToInt32(Session["LogedUserGroup"]);
                int course = Convert.ToInt32(Session["LogedUserCourse"]);
                int StudentId = Convert.ToInt32(Session["LogedUserId"]);
                foreach (HtmlInputs.Models.Missings item in dc.Missings)
                {
                    if(item.Users.Course.Equals(course) &&
                        item.Users.Group.Equals(group))
                    { 
                        if(isStudent == 1 && item.Users.UserId != StudentId)
                        {
                            continue;
                        }
                        int recMonth = item.Date.Month;

                        for (int i = 0; i < numMonths; i++)
                        {
                            if (recMonth == months[i])
                            {
                                if (item.IsValid == 1)
                                {
                                    IsValidGroupMiss[i] += 2;
                                    break;
                                }
                                else
                                {
                                    IsNotValidGroupMiss[i] += 2;
                                    break;
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < numMonths; i++)
                {
                    string month = new DateTime(2000, months[i], 1).ToString("MMMM");
                    ListMonth.Add(month);
                }
                FileContentResult chart = CreateChart(IsValidGroupMiss, IsNotValidGroupMiss, ListMonth);
                return chart;
        }
        public FileContentResult CreateChart(int[] isValid, int[] isNotValid, List<string> Months)
        {
            SeriesChartType chartType = SeriesChartType.Column;
            List<Users> student = new List<Users>();
            Chart statistic = new Chart();
            statistic.Width = 650;
            statistic.Height = 300;
            statistic.BackColor = Color.FromArgb(211, 223, 240);
            statistic.BorderlineDashStyle = ChartDashStyle.Solid;
            statistic.BackSecondaryColor = Color.White;
            statistic.BackGradientStyle = GradientStyle.TopBottom;
            statistic.BorderlineWidth = 1;
            statistic.Palette = ChartColorPalette.BrightPastel;
            statistic.BorderlineColor = Color.FromArgb(26, 59, 105);
            statistic.RenderType = RenderType.BinaryStreaming;
            statistic.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            statistic.AntiAliasing = AntiAliasingStyles.All;
            statistic.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            statistic.Titles.Add(CreateTitle());
            statistic.Legends.Add(CreateLegend());
            statistic.Series.Add(CreateSeries(0, isNotValid, Months, chartType));
            statistic.Series.Add(CreateSeries(1, isValid, Months, chartType));
            statistic.ChartAreas.Add(CreateChartArea());
            

            MemoryStream ms = new MemoryStream();
            statistic.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }
        public Title CreateTitle()
        {
            Title title = new Title();
            title.Text = "Статистика";
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            title.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = Color.FromArgb(26, 59, 105);
            return title;
        }
        public Legend CreateLegend()
        {
            Legend legend = new Legend();
            legend.Alignment = StringAlignment.Center;
            legend.Name = "Result Chart";
            legend.Docking = Docking.Bottom;
            legend.BackColor = Color.Transparent;
            legend.Font = new Font(new FontFamily("Trebuchet MS"), 9);
            legend.LegendStyle = LegendStyle.Row;
            return legend;
        }
        public Series CreateSeries(int isValid, int[] numMiss, List<string> months, SeriesChartType chartType)
        {
            Series seriesDetail = new Series();
            if(isValid == 1)
            { 
                seriesDetail.Name = "Уважительные";
                seriesDetail.Color = Color.LightGreen;
            }
            else
            {
                seriesDetail.Name = "Неуважительные";
                seriesDetail.Color = Color.OrangeRed;
            }
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;
            seriesDetail["DrawingStyle"] = "Cylinder";
            seriesDetail["PieDrawingStyle"] = "SoftEdge";
            DataPoint IsValidPoint;
            int numOfmonth = 0;
            foreach(string item in months)
            {
                IsValidPoint = new DataPoint();
                IsValidPoint.AxisLabel = item;
                IsValidPoint.YValues = new double[] { numMiss[numOfmonth] };
                numOfmonth++;
                seriesDetail.Points.Add(IsValidPoint);
            }
            seriesDetail.ChartArea = "Result";
            return seriesDetail;
        }
        public ChartArea CreateChartArea()
        {
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "Result";
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font = new Font("Verdana, Arial, Helvetica, sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font = new Font("Verdana, Arial, Helvetica, sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1;
            return chartArea;
        }
        [HttpPost]
        public ActionResult ChangeAvatar(HttpPostedFileBase NewAvatar)
        {
            if(NewAvatar != null)
            {
                DiplomEntities5 dc = new DiplomEntities5();
                MemoryStream memory = new MemoryStream();
                
                string fileName = System.IO.Path.GetFileName(NewAvatar.FileName);
                NewAvatar.SaveAs(Server.MapPath("~/Content/Avatars/" + fileName));
                NewAvatar.InputStream.CopyTo(memory);
                int userId = Convert.ToInt32(Session["LogedUserId"]);
                var UserToSave = dc.Users.Where(a => a.UserId.Equals(userId)).FirstOrDefault();
                UserToSave.Avatar = memory.GetBuffer();
                dc.SaveChanges();
            }
            return Redirect("~/Profile/Index");
        }
    }
}
