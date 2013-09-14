﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using IWorld.BLL;
using IWorld.Setting;

namespace IWorld.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new WebSetting();
            new ComprehensiveInformation();
            using (WebMapContext db = new WebMapContext())
            {
                if (!db.Database.Exists())
                {
                    DefaultManager dm = new DefaultManager(db);
                    dm.Initialize();
                }
            }
            EventManager.Initialization();
            TimeLineManager.Initialize();
            CollectionManager.Initialize();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}