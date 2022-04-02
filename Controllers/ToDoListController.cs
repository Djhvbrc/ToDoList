using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class ToDoListController : Controller
    {
        ToDoListTYEntities context = new ToDoListTYEntities();
        [Authorize]
        public ActionResult Index()
        {
            var lists = context.Lists.Where(b => b.id_user == UserID.ID).ToList();
            var madelists = context.MadeLists.Where(b => b.id_user == UserID.ID).ToList();

            IndexVM model = new IndexVM();
            model.lists = lists;
            model.madeLists = madelists;
            return View(model);
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(Lists newList)
        {

            if (ModelState.IsValid)
            {
                newList.id_user = UserID.ID;
                context.Lists.Add(newList);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(newList);
            }
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            // Get movie to delete
            var listToDelete = context.Lists.First(l => l.id == id);

            // Delete 
            context.Lists.Remove(listToDelete);
            context.SaveChanges();

            // Show Index view
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Made(int id)
        {
            // Get movie to delete
            var listToDelete = context.Lists.First(l => l.id == id);
            
            MadeLists madeLists = new MadeLists();
            madeLists.id_user = UserID.ID;

            madeLists.name = listToDelete.name;
            madeLists.id_user = listToDelete.id_user;

            context.MadeLists.Add(madeLists);
            
            context.Lists.Remove(listToDelete);
            context.SaveChanges();

            // Show Index view
            return RedirectToAction("Index");
        }


    }
}