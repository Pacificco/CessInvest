using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CessInvest
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region ПУБЛИКАЦИИ
            routes.MapRoute(
                name: "art_comment",
                url: "publikacii/{cat_id}/{art_id}/comments/{id}",
                defaults: new { controller = "Articles", action = "Comment" },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            routes.MapRoute(
                name: "art_comments",
                url: "publikacii/{cat_id}/{art_id}/comments",
                defaults: new { controller = "Articles", action = "Comments" },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            routes.MapRoute(
                name: "art_item",
                url: "publikacii/{cat_id}/{art_id}",
                defaults: new { controller = "Articles", action = "Art" },
                namespaces: new[] { "CessInvest.Controllers" }
            );            
            routes.MapRoute(
                name: "art_comment_send_ajax",
                url: "publikacii/comment-ajax",
                defaults: new { controller = "Articles", action = "CommentAjax" },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            routes.MapRoute(
                name: "art_comment_send",
                url: "publikacii/comment",
                defaults: new { controller = "Articles", action = "Comment" },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            routes.MapRoute(
                name: "art_items",
                url: "publikacii/{cat_id}",
                defaults: new { controller = "Articles", action = "List" },
                namespaces: new[] { "CessInvest.Controllers" }
            );            
            #endregion

            #region ПОЛЬЗОВАТЕЛИ
            routes.MapRoute(
                name: "user_profile",
                url: "lk/{user_id}/{action}",
                defaults: new { controller = "Users", action = "Index" },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            routes.MapRoute(
                name: "user_forecasts",
                url: "lk/{user_id}/forecasts/{section_id}/{item_id}",
                defaults: new { controller = "Users", action = "Forecasts", section_id = "dollar" },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            routes.MapRoute(
                name: "user_archive",
                url: "lk/{user_id}/archive/{page}",
                defaults: new { controller = "Users", action = "Archive", page = UrlParameter.Optional },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            #endregion
                        
            #region КУРСЫ ВАЛЮТ И КАТЕРОВКИ
            routes.MapRoute(
                name: "rates",
                url: "currency",
                defaults: new { controller = "Rates", action = "Index" },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            #endregion

            #region ОБЩИЕ МАРШРУТЫ
            routes.MapRoute(
                name: "stat_page",
                url: "{page_name}",
                defaults: new { controller = "Home", action = "StatPage" },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            routes.MapRoute(
                name: "art_captcha_refresh",
                url: "captcha-ajax",
                defaults: new { controller = "Home", action = "CaptchaAjax" },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            #endregion

            #region ОШИБКИ + 404
            //Ошибки
            routes.MapRoute(
              name: "error",
              url: "Error/{action}",
              defaults: new { controller = "Error", action = "Error" },
              namespaces: new[] { "CessInvest.Controllers" }
            );
            //404
            routes.MapRoute(
                name: "NotFound",
                url: "{*url}",
                defaults: new { controller = "Error", action = "Http404" },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            //Остальное
            routes.MapRoute(
                name: "404_catch_all",
                url: "{*catchall}",
                defaults: new { controller = "Error", action = "NotFound" },
                namespaces: new[] { "CessInvest.Controllers" }
            );
            #endregion

        }
    }
}