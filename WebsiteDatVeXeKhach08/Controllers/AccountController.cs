﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteDatVeXeKhach08.Controllers
{
    public class AccountController : Controller
    {
        BanVeXeKhachEntities1 db = new BanVeXeKhachEntities1();
        public ActionResult LogIn()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult LogIn(FormCollection collection)
        {
            var username = collection["Username"];
            var password = collection["Password"];
            var loggedInUser = db.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            if (loggedInUser.RoleID == 1)
            {
                ViewBag.Name = loggedInUser.FullName;
                Session["UserID"] = loggedInUser.UserID;
                return RedirectToAction("AdminView");
            }
            else if (loggedInUser.RoleID == 2)
            {
                ViewBag.Name = loggedInUser.FullName;
                Session["UserID"] = loggedInUser.UserID;
                return RedirectToAction("CustomerView");
            }
            else
            {
                return View();
            }
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User u)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(u.Username) || string.IsNullOrWhiteSpace(u.Password) ||
                   string.IsNullOrWhiteSpace(u.FullName) || string.IsNullOrWhiteSpace(u.Email) ||
                   string.IsNullOrWhiteSpace(u.Phone))
                {
                    ModelState.AddModelError("", "All fields are required.");
                    return View();
                }
                var existingUser = db.Users
                    .FirstOrDefault(s => s.Username == u.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View();
                }
                u.RoleID = 2;
                db.Users.Add(u); 
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
        public ActionResult AdminView()
        {
            return View();
        }
        public ActionResult CustomerView()
        {
            return View();
        }
    }
}
