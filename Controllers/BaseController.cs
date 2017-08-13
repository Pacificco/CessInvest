using CessInvest.Models.Domain;
using CessInvest.Models.Domain.Account;
using CessInvest.Models.Domain.Articles;
using CessInvest.Models.Domain.Comments;
using CessInvest.Models.Domain.Other;
using CessInvest.Models.Domain.Users;
using CessInvest.Models.Helpers;
using CessInvest.Models.Infrastructure;
using CessInvest.Models.Security;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CessInvest.Controllers
{
    public abstract class BaseController : Controller
    {
        #region ПОЛЯ И СВОЙСТВА
        /// <summary>
        /// Подключение
        /// </summary>
        protected SqlConnection _connection;
        /// <summary>
        /// Подключение установлено
        /// </summary>
        protected bool _connected;

        /// <summary>
        /// Сообщение об ошибках
        /// </summary>
        protected string _errMassage;
        /// <summary>
        /// Предупреждение
        /// </summary>
        protected string _warningMassage;
        /// <summary>
        /// Сообщение
        /// </summary>
        protected string _infoMassage;

        /// <summary>
        /// Страница с ошибкой
        /// </summary>
        protected string _errPage;
        /// <summary>
        /// Страница с ошибкой для частичных представлений
        /// </summary>
        protected string _errPartialPage;

        public static readonly ILog log = LogManager.GetLogger(typeof(BaseController));
        #endregion

        public BaseController()
            : base()
        {
            _connection = null;
            _connected = false;

            _errMassage = String.Empty;
            _warningMassage = String.Empty;
            _infoMassage = String.Empty;

            _errPage = "~/Views/Shared/Error.cshtml";
            _errPartialPage = "~/Views/Shared/PartialError.cshtml";

            //AccountManager _manager = new AccountManager();
            //string password = _manager._getMd5Hash("SupperPassword");
            //if (password == "111") return;

            //AccountManager m = new AccountManager();
            //_errMassage = m._getMd5Hash("sE~x6y34");

            Connect();
        }

        /// <summary>
        /// Подключается к базе данных
        /// </summary>
        private void Connect()
        {
            try
            {
                _connection = GlobalParams.GetConnection();
                if (_connection == null)
                {
                    _errMassage = GlobalParams._connectionError;
                    _connected = false;
                }
                else
                {
                    _connected = true;
                }                
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                _errMassage = e.ToString();
                _connected = false;
            }
        }

        #region СПИСКИ
        [ChildActionOnly]
        public ActionResult _getRegionsDropDownList(Guid selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                if (_connected)
                {
                    VM_DropDownRegions model = new VM_DropDownRegions();
                    model.SelectedId = selectedId;
                    model.Items = DbHelper.GetRegions();
                    model.FirstItem = firstItem;
                    model.Name = id;
                    return PartialView("_regionsDropDownList", model);
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getCategoriesDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                if (_connected)
                {
                    ArticlesManager manager = new ArticlesManager();
                    VM_DropDownCategories model = new VM_DropDownCategories();
                    model.SelectedId = selectedId;
                    model.Items = manager.GetCategories();
                    model.FirstItem = firstItem;
                    model.Name = id;
                    return PartialView("_categoriesDropDownList", model);
                }
                else
                {
                    log.Error(_errMassage);                    
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]        
        public ActionResult _getActiveDropDownList(EnumBoolType selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_DropDownActive model = new VM_DropDownActive();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_activeDropDownList", model);                
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getArchiveDropDownList(EnumBoolType selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_DropDownArchive model = new VM_DropDownArchive();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_archiveDropDownList", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getConfirmDropDownList(EnumBoolType selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_DropDownConfirm model = new VM_DropDownConfirm();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_confirmDropDownList", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        #endregion

        #region МОДУЛИ
        [ChildActionOnly]
        //[OutputCache(Duration = 86400, VaryByParam = "current_item")]
        public ActionResult _getModuleMainMenu(string current_item = "home")
        {
            return PartialView("_moduleMainMenu", current_item);
        }        
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "categoryId;show_article_link")]
        public ActionResult _getModuleLastComments(int categoryId = 0, int row_count = 3, bool show_article_link = true)
        {
            try
            {
                if (_connected)
                {               
                    ArticlesManager manager = new ArticlesManager();
                    VM_Comments model = manager.GetLastComments(categoryId, row_count, show_article_link);
                    if (model != null)
                    {
                        return PartialView("_moduleLastComments", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "art_id")]
        public ActionResult _getModuleLastArtComments(int art_id)
        {
            try
            {
                if (art_id <= 0)
                {
                    log.Error("Идентификатор публикации не определен");
                    return PartialView(_errPartialPage);
                }
                if (_connected)
                {
                    ArticlesManager manager = new ArticlesManager();
                    VM_Comments model = manager.GetLastComments(art_id);
                    if (model != null)
                    {
                        return PartialView("_moduleLastArtComments", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        //[OutputCache(Duration = 60, VaryByParam = "exclude_ids;cat_ids")]
        public ActionResult _getModuleLastArticlesLinkList(List<int> exclude_ids, List<int> cat_ids, int row_count = 3)
        {
            try
            {
                if (_connected)
                {
                    ArticlesManager manager = new ArticlesManager();
                    List<VM_ArtItem> model = manager.GetArtItems(exclude_ids, cat_ids, row_count);
                    if (model != null)
                    {
                        return PartialView("_moduleLastArticlesLinkList", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        //[OutputCache(Duration = 60, VaryByParam = "exclude_ids;cat_ids")]
        public PartialViewResult _getModuleLastArticlesImageList(List<int> exclude_ids, List<int> cat_ids, int row_count = 3)
        {
            try
            {
                if (_connected)
                {
                    ArticlesManager manager = new ArticlesManager();
                    List<VM_ArtItem> model = manager.GetArtItems(exclude_ids, cat_ids, row_count);
                    if (model != null)
                    {
                        return PartialView("_moduleLastArticlesImageList", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }        
        [ChildActionOnly]
        public ActionResult _getModuleCaptchaBlock()
        {
            try
            {
                return PartialView("_moduleCaptchaBlock");
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 120)]
        public ActionResult _getModuleCounters()
        {
            return PartialView("_moduleCounters");
        }
        [ChildActionOnly]
        public ActionResult _getModuleLogin()
        {
            try
            {
                return PartialView("_moduleFormLogin", new VM_UserLogin());
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60)]
        public ActionResult _getModuleShareInSocial()
        {
            try
            {
                return PartialView("_moduleShareInSocial");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getModuleWellcomeBlock(string user_name)
        {
            try
            {
                AccountManager manager = new AccountManager();
                VM_User user = manager.FindUser(user_name);
                if (user == null)
                    return PartialView(_errPartialPage);
                else
                    return PartialView("_moduleWellcomeBlock", user);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        #endregion

        #region СОЦ СЕТИ
        [ChildActionOnly]
        public PartialViewResult _getWidget_VK()
        {
            try
            {
                return PartialView("_widget_vk");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public PartialViewResult _getWidget_OK()
        {
            try
            {
                return PartialView("_widget_ok");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public PartialViewResult _getWidget_FB()
        {
            try
            {
                return PartialView("_widget_fb");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        #endregion

        #region CAPTCHA
        [HttpPost]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public ActionResult CaptchaAjax()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_moduleCaptchaBlock");
                }
                else
                {
                    return PartialView("_partialView", "<p class=\"text-red\">Не удалось загрузить Контрольную строку!</p>");
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView("_partialView", "<p class=\"text-red\">Не удалось загрузить Контрольную строку!</p>");
            }
        }
        [HttpGet]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public CaptchaImageResult ShowCaptchaImage()
        {
            return new CaptchaImageResult(140, 50, 5);
        }
        #endregion

        #region Http404 handling

        protected override void HandleUnknownAction(string actionName)
        {
            // Если контроллер - ErrorController, то не нужно снова вызывать исключение
            if (this.GetType() != typeof(ErrorController))
                this.InvokeHttp404(HttpContext);
        }

        public ActionResult InvokeHttp404(HttpContextBase httpContext)
        {
            IController errorController = new ErrorController();
            var errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");
            errorRoute.Values.Add("action", "Http404");
            errorRoute.Values.Add("url", httpContext.Request.Url.OriginalString);
            errorController.Execute(new RequestContext(httpContext, errorRoute));

            return new EmptyResult();
        }

        #endregion

    }
}