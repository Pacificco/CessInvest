using CessInvest.Models.Domain;
using CessInvest.Models.Domain.Articles;
using CessInvest.Models.Domain.Comments;
using CessInvest.Models.Helpers;
using CessInvest.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CessInvest.Controllers
{
    public class ArticlesController : BaseController
    {
        [HttpGet]
        public ActionResult List(string cat_id, int page = 1)
        {
            try
            {
                if (_connected)
                {                    
                    ArticlesManager _manager = new ArticlesManager();
                    VM_Articles model = _manager.GetArticles(cat_id, page);
                    if (!String.IsNullOrEmpty(cat_id))
                    {
                        model.CurrentCategory = _manager.GetCategory(cat_id);
                        if (model.CurrentCategory == null)
                        {
                            return RedirectToAction("Error", "Error", null);
                        }
                    }                    
                    if (model != null)
                    {                        
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return RedirectToAction("Error", "Error", null);
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
        public ActionResult Art(string cat_id, string art_id)
        {
            try
            {
                if (_connected)
                {
                    ArticlesManager _manager = new ArticlesManager();
                    VM_Article model = _manager.GetArticle(art_id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return RedirectToAction("NotFound", "Error", null);
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
        public ActionResult StatArticles(string page_name)
        {
            try
            {
                if (_connected)
                {
                    ArticlesManager _manager = new ArticlesManager();
                    VM_Article model = _manager.GetArticle(page_name, false, false);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("NotFound", "Error", null);
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
        public ActionResult Comment(VM_CommentResponse model)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        ArticlesManager _manager = new ArticlesManager();
                        if (Session["captcha_code"] != null)
                        {
                            if (!model.CaptchaCode.ToLower().Equals(Session["captcha_code"].ToString().ToLower()))
                            {
                                ModelState.AddModelError("CaptchaCode", "Код с картинки указан не верно!");
                                VM_Article art = _manager.GetArticle(model.ArtId);
                                return RedirectToRoute("art_item", new { art_id = art.Alias, cat_id = art.Category.Alias });
                            }
                        }
                        if (_manager.CreateComment(model))
                        {
                            ViewBag.InfoMessage = "Ваш комментарий успешно отправлен! После модерации он будет размещен на сайте.";
                            VM_Article art = _manager.GetArticle(model.ArtId);
                            return RedirectToRoute("art_item", new { art_id = art.Alias, cat_id = art.Category.Alias });
                        }
                        else
                        {
                            ViewBag.ErrorMessage = _manager.LastError;
                            return RedirectToAction("Error", "Error", null);
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
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return RedirectToAction("Error", "Error", null);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public ActionResult CommentAjax(VM_CommentResponse model)
        {
            try
            {
                if (_connected)
                {
                    if (Request.IsAjaxRequest())
                    {
                        if (ModelState.IsValid)
                        {
                            ArticlesManager _manager = new ArticlesManager();
                            if (Session["captcha_code"] != null)
                            {
                                if (!model.CaptchaCode.ToLower().Equals(Session["captcha_code"].ToString().ToLower()))
                                {
                                    ModelState.AddModelError("CaptchaCode", "Код с картинки указан не верно!");
                                    return PartialView("_moduleFormComment", model);
                                }
                            }
                            if (_manager.CreateComment(model))
                            {
                                //ViewBag.InfoMessage = "Ваш комментарий успешно отправлен! После модерации он будет размещен на сайте.";
                                //VM_Article art = _manager.GetArt(model.ArtId);
                                return PartialView("_moduleFormComment", new VM_CommentResponse() { ArtId = model.ArtId, 
                                    SuccessMessage = "Ваш комментарий успешно отправлен!" });
                            }
                            else
                            {
                                return PartialView("_partialView", "<p class=\"text-navy\">Ошибка во время создания комментария!</p>");
                            }
                        }
                        else
                        {
                            return PartialView("_moduleFormComment", model);
                        }
                    }
                    else
                    {
                        return PartialView("_partialView", "<p class=\"text-red\">Ошибка во время создания комментария!</p>");
                    }
                }
                else
                {
                    //ViewBag.ErrorMessage = _errMassage;
                    return PartialView("_partialView", "<p class=\"text-red\">Ошибка во время создания комментария!</p>");
                }
            }
            catch //(Exception e)
            {
                //ViewBag.ErrorMessage = e.ToString();
                return PartialView("_partialView", "<p class=\"text-red\">Ошибка во время создания комментария!</p>");
            }
        }
        [HttpGet]
        public ActionResult Comments(string art_id, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    ArticlesManager _manager = new ArticlesManager();
                    VM_ArtComments model = _manager.GetArtComments(art_id, page);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return RedirectToAction("NotFound", "Error", null);
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
        public ActionResult Comment(string art_id, int comment_id)
        {
            try
            {
                if (_connected)
                {
                    ArticlesManager _manager = new ArticlesManager();
                    VM_ArtComment model = _manager.GetArtComment(art_id, comment_id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return RedirectToAction("NotFound", "Error", null);
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
                        
        [ChildActionOnly]
        public ActionResult _getArticleById(int art_id, bool as_section = false)
        {
            try
            {
                ArticlesManager manager = new ArticlesManager();
                VM_Article model = manager.GetArticle(art_id, as_section, false);              
                return PartialView("_moduleArticle", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getArticleByAlias(string art_alias, bool as_section = false)
        {
            try
            {
                ArticlesManager manager = new ArticlesManager();
                VM_Article model = manager.GetArticle(art_alias, as_section, false);              
                return PartialView("_moduleArticle", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getModuleLeaveArtComment(int art_id)
        {
            try
            {
                VM_CommentResponse model = new VM_CommentResponse();
                model.ArtId = art_id;
                return PartialView("_moduleFormComment", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
    }
}
