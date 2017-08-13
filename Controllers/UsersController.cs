using CessInvest.Models.Domain;
using CessInvest.Models.Domain.Account;
using CessInvest.Models.Domain.Users;
using CessInvest.Models.Helpers;
using CessInvest.Models.OutApi;
using CessInvest.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CessInvest.Controllers
{
    [CustomAuthorize(Roles = "admin, club_member")]
    public class UsersController : BaseController
    {        
        [HttpGet]
        public ActionResult Index(int user_id)
        {
            if (!_isUserValid(user_id))
                return RedirectToAction("Index", "Home", null);

            try
            {                
                if (_connected)
                {
                    UserManager _manager = new UserManager();
                    VM_User model = _manager.GetUser(user_id);                    
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return View("NotFound");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return RedirectToAction("Error", "Error", null);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return RedirectToAction("Error", "Error", null);
            }
        }
        [HttpGet]
        public ActionResult Edit(int user_id)
        {
            if (!_isUserValid(user_id))
                return RedirectToAction("Index", "Home", null);

            try
            {
                if (_connected)
                {
                    UserManager _manager = new UserManager();
                    VM_User model = _manager.GetUser(user_id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return View("NotFound");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return RedirectToAction("Error", "Error", null);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return RedirectToAction("Error", "Error", null);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VM_User model)
        {
            if (!_isUserValid(model.Id))
                return RedirectToAction("Index", "Home", null);

            try
            {
                if (_connected)
                {
                    AccountManager manager = new AccountManager();
                    if (manager.NicExists(model.Nic, model.Id))
                    {
                        ModelState.AddModelError("Nic", "Указанный Ник уже зарегистрирован в системе!");
                        return View(model);
                    }
                    if (manager.MailExists(model.Email, model.Id))
                    {
                        ModelState.AddModelError("Email", "Указанный Email уже зарегистрирован в системе!");
                        return View(model);
                    }

                    if (ModelState.IsValid)
                    {
                        UserManager _manager = new UserManager();
                        if (_manager.UpdateUserProfile(model))
                        {
                            return View("Index", model);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Во время сохранения данных произошла ошибка! Повторите попытку позже.");
                            log.Error(_manager.LastError);
                            return View(model);
                        }
                    }
                    else
                    {
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return RedirectToAction("Error", "Error", null);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                return RedirectToAction("Error", "Error", null);
            }
        }
        
        #region ДОЧЕРНИЕ МЕТОДЫ
        [ChildActionOnly]
        public PartialViewResult _getModuleSideUserMenu(string cur_item = "none")
        {
            try
            {
                return PartialView("_modelSideUserMenu", cur_item);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        #endregion

        [AllowAnonymous]
        private bool _isUserValid(int user_id)
        {
            try
            {
                if (String.IsNullOrEmpty(CessInvest.Models.Security.SessionPersister.UserEmail))
                    return false;
                //if (CessInvest.Models.Security.SessionPersister.CurrentUser == null)
                //    return false;
                if (CessInvest.Models.Security.SessionPersister.UserId != user_id)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
