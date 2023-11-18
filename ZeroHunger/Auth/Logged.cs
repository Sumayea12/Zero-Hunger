using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistrationModule.Auth
{
    public class Logged : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["email"] != null && httpContext.Session["Id"] != null) return true;
            return false;
        }
    }
}