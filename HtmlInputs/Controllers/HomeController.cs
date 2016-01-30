using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HtmlInputs.Models;
using HtmlInputs.Tools;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;

namespace HtmlInputs.Controllers
{
    //Формат логинов для пользователей разных категорий
    //Студент:
    //  Год поступления (2 цифры)|uir|№ группы|фио (строчными)
    //Куратор:
    //  фио (строчными)|№ группы курирования|K(прописная латинская буква)|Год рождения (2 цифры)
    //Методист:
    //  M(прописная латинская буква)|Год рождения (2 цифры)|фио (строчными)
    //Зам. декана:
    //  фио (строчными)|Год рождения (2 цифры)|Z(прописная латинская буква)
    //Декан:
    //  uir2003|фио (строчными)|Год рождения (2 цифры)
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(LogedUser user)
        {
            using(DiplomEntities5 dc = new DiplomEntities5())
            {
                if(ModelState.IsValid)
                { 
                    var v = dc.Users.Where(a => a.Login.Equals(user.Login) && a.Password.Equals(user.Password)).FirstOrDefault();
                    if(v != null)
                    {
                        if (v.RoleId == 7)
                        {
                            MvcHtmlString str = new MvcHtmlString("<script>alert('Вы отчислены из университета. Доступ закрыт')</script>");
                            ViewBag.Message = str;
                            return View(user);
                        }
                        Session["LogedUserId"] = v.UserId.ToString();
                        Session["LogedUserName"] = v.Name.ToString();
                        Session["LogedUserSname"] = v.Sirname.ToString();
                        Session["LogedUserPname"] = v.Patername.ToString();
                        Session["LogedUserRole"] = v.RoleId.ToString();
                        Session["LogedUserGroup"] = v.Group.ToString();
                        Session["LogedUserCourse"] = v.Course.ToString();
                        return RedirectPermanent("Profile/Index");
                    }
                    else
                    {
                        ViewBag.Message = "Ошибка авторизации";
                    }
                }
                
            }
            return View(user);
        }
        public ActionResult HowToRegister()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Register(RegUser FormRegUser, HttpPostedFileBase Avatar)
        {
            using (DiplomEntities5 dc = new DiplomEntities5())
            {
                var anyUserEmail = dc.Users.Any(a => a.Email.Equals(FormRegUser.Email));
                if (anyUserEmail)
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email уже зарегистрирован");
                }

                var anyUserLogin = dc.Users.Any(a => a.Login.Equals(FormRegUser.Login));
                if (anyUserLogin)
                {
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже зарегистрирован");
                }
                if (FormRegUser.Captcha != (string)Session[CaptchaImage.CaptchaValueKey])
                {
                    ModelState.AddModelError("Captcha", "Текст с картинки введён не верно");
                    return View(FormRegUser);
                }
                if(ModelState.IsValid == true)
                {
                    MemoryStream memory = new MemoryStream();
                    if (Avatar == null)
                    {
                        Bitmap avatar = new Bitmap(Server.MapPath("~/Content/Avatars/default_avatar.gif"));
                        avatar.Save(memory, ImageFormat.Gif);
                    }
                    else
                    {
                        string fileName = System.IO.Path.GetFileName(Avatar.FileName);
                        Avatar.SaveAs(Server.MapPath("~/Content/Avatars/" + fileName));
                        Avatar.InputStream.CopyTo(memory);
                    }
                    Users UserToSave = new Users();
                    UserToSave.Name = FormRegUser.Name;
                    UserToSave.Sirname = FormRegUser.Sirname;
                    UserToSave.Patername = FormRegUser.Patername;
                    UserToSave.Password = FormRegUser.Password;
                    UserToSave.RoleId = FormRegUser.RoleId;
                    UserToSave.Group = FormRegUser.Group;
                    UserToSave.Login = FormRegUser.Login;
                    UserToSave.Course = FormRegUser.Course;
                    UserToSave.Email = FormRegUser.Email;
                    UserToSave.Avatar = memory.GetBuffer();
                    UserToSave.Birthday = new DateTime(FormRegUser.YearOfBirth, FormRegUser.MonthOfBirth, FormRegUser.DayOfBirth);
                        dc.Users.Add(UserToSave);
                        dc.SaveChanges();
                        ModelState.Clear();
                        FormRegUser = null;
                        ViewBag.Message = "Регистрация прошла успешно";
                }
            }
            return View(FormRegUser);
        }
        public ActionResult RegisterModerator()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterModerator(RegModerator FormRegUser, HttpPostedFileBase Avatar)
        {
            using (DiplomEntities5 dc = new DiplomEntities5())
            {
                var anyUserEmail = dc.Users.Any(a => a.Email.Equals(FormRegUser.Email));
                if (anyUserEmail)
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email уже зарегистрирован");
                }

                var anyUserLogin = dc.Users.Any(a => a.Login.Equals(FormRegUser.Login));
                if (anyUserLogin)
                {
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже зарегистрирован");
                }
                if (FormRegUser.Captcha != (string)Session[CaptchaImage.CaptchaValueKey])
                {
                    ModelState.AddModelError("Captcha", "Текст с картинки введён не верно");
                    return View(FormRegUser);
                }
                if (ModelState.IsValid == true)
                {
                    MemoryStream memory = new MemoryStream();
                    if (Avatar == null)
                    {
                        Bitmap avatar = new Bitmap(Server.MapPath("~/Content/Avatars/default_avatar.gif"));
                        avatar.Save(memory, ImageFormat.Gif);
                    }
                    else
                    {
                        string fileName = System.IO.Path.GetFileName(Avatar.FileName);
                        Avatar.SaveAs(Server.MapPath("~/Content/Avatars/" + fileName));
                        Avatar.InputStream.CopyTo(memory);
                    }
                    Users UserToSave = new Users();
                    UserToSave.Name = FormRegUser.Name;
                    UserToSave.Sirname = FormRegUser.Sirname;
                    UserToSave.Patername = FormRegUser.Patername;
                    UserToSave.Password = FormRegUser.Password;
                    UserToSave.RoleId = FormRegUser.RoleId;
                    UserToSave.Group = FormRegUser.Group;
                    UserToSave.Login = FormRegUser.Login;
                    UserToSave.Course = FormRegUser.Course;
                    UserToSave.Email = FormRegUser.Email;
                    UserToSave.Avatar = memory.GetBuffer();
                    UserToSave.Birthday = new DateTime(FormRegUser.YearOfBirth, FormRegUser.MonthOfBirth, FormRegUser.DayOfBirth);
                    dc.Users.Add(UserToSave);
                    dc.SaveChanges();
                    ModelState.Clear();
                    FormRegUser = null;
                    ViewBag.Message = "Регистрация прошла успешно";
                }
            }
            return View(FormRegUser);   
        }
        public ActionResult RegisterMethodist()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterMethodist(RegMethodist FormRegUser, HttpPostedFileBase Avatar)
        {
            using (DiplomEntities5 dc = new DiplomEntities5())
            {
                var anyUserEmail = dc.Users.Any(a => a.Email.Equals(FormRegUser.Email));
                if (anyUserEmail)
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email уже зарегистрирован");
                }

                var anyUserLogin = dc.Users.Any(a => a.Login.Equals(FormRegUser.Login));
                if (anyUserLogin)
                {
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже зарегистрирован");
                }
                if (FormRegUser.Captcha != (string)Session[CaptchaImage.CaptchaValueKey])
                {
                    ModelState.AddModelError("Captcha", "Текст с картинки введён не верно");
                    return View(FormRegUser);
                }
                if (ModelState.IsValid == true)
                {
                    MemoryStream memory = new MemoryStream();
                    if (Avatar == null)
                    {
                        Bitmap avatar = new Bitmap(Server.MapPath("~/Content/Avatars/default_avatar.gif"));
                        avatar.Save(memory, ImageFormat.Gif);
                    }
                    else
                    {
                        string fileName = System.IO.Path.GetFileName(Avatar.FileName);
                        Avatar.SaveAs(Server.MapPath("~/Content/Avatars/" + fileName));
                        Avatar.InputStream.CopyTo(memory);
                    }
                    Users UserToSave = new Users();
                    UserToSave.Name = FormRegUser.Name;
                    UserToSave.Sirname = FormRegUser.Sirname;
                    UserToSave.Patername = FormRegUser.Patername;
                    UserToSave.Password = FormRegUser.Password;
                    UserToSave.RoleId = FormRegUser.RoleId;
                    UserToSave.Group = FormRegUser.Group;
                    UserToSave.Login = FormRegUser.Login;
                    UserToSave.Course = FormRegUser.Course;
                    UserToSave.Email = FormRegUser.Email;
                    UserToSave.Avatar = memory.GetBuffer();
                    UserToSave.Birthday = new DateTime(FormRegUser.YearOfBirth, FormRegUser.MonthOfBirth, FormRegUser.DayOfBirth);
                    dc.Users.Add(UserToSave);
                    dc.SaveChanges();
                    ModelState.Clear();
                    FormRegUser = null;
                    ViewBag.Message = "Регистрация прошла успешно";
                }
            }
            return View(FormRegUser);
        }
        public ActionResult RegisterZDean()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterZDean(RegZDean FormRegUser, HttpPostedFileBase Avatar)
        {
            using (DiplomEntities5 dc = new DiplomEntities5())
            {
                var anyUserEmail = dc.Users.Any(a => a.Email.Equals(FormRegUser.Email));
                if (anyUserEmail)
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email уже зарегистрирован");
                }

                var anyUserLogin = dc.Users.Any(a => a.Login.Equals(FormRegUser.Login));
                if (anyUserLogin)
                {
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже зарегистрирован");
                }
                if (FormRegUser.Captcha != (string)Session[CaptchaImage.CaptchaValueKey])
                {
                    ModelState.AddModelError("Captcha", "Текст с картинки введён не верно");
                    return View(FormRegUser);
                }
                if (ModelState.IsValid == true)
                {
                    MemoryStream memory = new MemoryStream();
                    if (Avatar == null)
                    {
                        Bitmap avatar = new Bitmap(Server.MapPath("~/Content/Avatars/default_avatar.gif"));
                        avatar.Save(memory, ImageFormat.Gif);
                    }
                    else
                    {
                        string fileName = System.IO.Path.GetFileName(Avatar.FileName);
                        Avatar.SaveAs(Server.MapPath("~/Content/Avatars/" + fileName));
                        Avatar.InputStream.CopyTo(memory);
                    }
                    Users UserToSave = new Users();
                    UserToSave.Name = FormRegUser.Name;
                    UserToSave.Sirname = FormRegUser.Sirname;
                    UserToSave.Patername = FormRegUser.Patername;
                    UserToSave.Password = FormRegUser.Password;
                    UserToSave.RoleId = FormRegUser.RoleId;
                    UserToSave.Group = FormRegUser.Group;
                    UserToSave.Login = FormRegUser.Login;
                    UserToSave.Course = FormRegUser.Course;
                    UserToSave.Email = FormRegUser.Email;
                    UserToSave.Avatar = memory.GetBuffer();
                    UserToSave.Birthday = new DateTime(FormRegUser.YearOfBirth, FormRegUser.MonthOfBirth, FormRegUser.DayOfBirth);
                    dc.Users.Add(UserToSave);
                    dc.SaveChanges();
                    ModelState.Clear();
                    FormRegUser = null;
                    ViewBag.Message = "Регистрация прошла успешно";
                }
            }
            return View(FormRegUser);
        }
        public ActionResult RegisterDean()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterDean(RegDean FormRegUser, HttpPostedFileBase Avatar)
        {
            using (DiplomEntities5 dc = new DiplomEntities5())
            {
                var anyUserEmail = dc.Users.Any(a => a.Email.Equals(FormRegUser.Email));
                if (anyUserEmail)
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email уже зарегистрирован");
                }

                var anyUserLogin = dc.Users.Any(a => a.Login.Equals(FormRegUser.Login));
                if (anyUserLogin)
                {
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже зарегистрирован");
                }
                if (FormRegUser.Captcha != (string)Session[CaptchaImage.CaptchaValueKey])
                {
                    ModelState.AddModelError("Captcha", "Текст с картинки введён не верно");
                    return View(FormRegUser);
                }
                if (ModelState.IsValid == true)
                {
                    MemoryStream memory = new MemoryStream();
                    if (Avatar == null)
                    {
                        Bitmap avatar = new Bitmap(Server.MapPath("~/Content/Avatars/default_avatar.gif"));
                        avatar.Save(memory, ImageFormat.Gif);
                    }
                    else
                    {
                        string fileName = System.IO.Path.GetFileName(Avatar.FileName);
                        Avatar.SaveAs(Server.MapPath("~/Content/Avatars/" + fileName));
                        Avatar.InputStream.CopyTo(memory);
                    }
                    Users UserToSave = new Users();
                    UserToSave.Name = FormRegUser.Name;
                    UserToSave.Sirname = FormRegUser.Sirname;
                    UserToSave.Patername = FormRegUser.Patername;
                    UserToSave.Password = FormRegUser.Password;
                    UserToSave.RoleId = FormRegUser.RoleId;
                    UserToSave.Group = FormRegUser.Group;
                    UserToSave.Login = FormRegUser.Login;
                    UserToSave.Course = FormRegUser.Course;
                    UserToSave.Email = FormRegUser.Email;
                    UserToSave.Avatar = memory.GetBuffer();
                    UserToSave.Birthday = new DateTime(FormRegUser.YearOfBirth, FormRegUser.MonthOfBirth, FormRegUser.DayOfBirth);
                    dc.Users.Add(UserToSave);
                    dc.SaveChanges();
                    ModelState.Clear();
                    FormRegUser = null;
                    ViewBag.Message = "Регистрация прошла успешно";
                }
            }
            return View(FormRegUser);
        }
        public ActionResult Captcha()
        {
            Session[CaptchaImage.CaptchaValueKey] = new Random(DateTime.Now.Millisecond).Next(1111, 9999).ToString();
            var ci = new CaptchaImage(Session[CaptchaImage.CaptchaValueKey].ToString(), 211, 50, "Arial");
            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";

            ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

            ci.Dispose();
            return null;
        }
    }
}
