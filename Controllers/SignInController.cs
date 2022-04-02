using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class SignInController : Controller
    {
        ToDoListTYEntities context = new ToDoListTYEntities();
        // GET: SignIn
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users model)
        {
            if (ModelState.IsValid)
            {
                Users user = null;
                using (context)
                {
                    user = context.Users.FirstOrDefault(u => u.login == model.login && u.password == model.password);

                    if (user != null)
                    {
                        UserID.ID = context.Users.Where(b => b.login == model.login && b.password == model.password).Select(b => b.id).FirstOrDefault();
                        FormsAuthentication.SetAuthCookie(model.login, true);
                        return RedirectToAction("Index", "ToDoList");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                    }
                }
            }

            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Users model)
        {
            if (ModelState.IsValid)
            {
                Users user = null;
                using (ToDoListTYEntities db = new ToDoListTYEntities())
                {
                    user = db.Users.FirstOrDefault(u => u.login == model.login);
                }
                if (user == null)
                {
                    using (ToDoListTYEntities db = new ToDoListTYEntities())
                    {
                        db.Users.Add(new Users { login = model.login, password = model.password});
                        db.SaveChanges();

                        user = db.Users.Where(u => u.login == model.login && u.password == model.password).FirstOrDefault();
                    }
                    if (user != null)
                    {
                        return RedirectToAction("Login", "SignIn");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }
    }
}