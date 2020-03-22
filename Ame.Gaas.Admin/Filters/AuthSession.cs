using System.Net;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace Ame.Gaas.Admin.Models
{
    public class AuthSession : ActionFilterAttribute, IActionFilter
    {

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session != null)
            {
                var id = filterContext.HttpContext.Session["GuidSessao"];

                if (id == null)
                    filterContext.Result = new RedirectResult("~/Home");

                base.OnActionExecuting(filterContext);
                return;
            }

            filterContext.Result = new RedirectResult("~/Home");

        }
        

    }
}