﻿using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Web_Assignment.Models;

namespace Web_Assignment.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            populateDDL();
        }


        protected void populateDDL()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var q1 = (from sp in db.Sports
                      orderby sp.ID
                      select sp.sportName).ToList();

            ddlSportName.DataSource = q1;
            ddlSportName.DataBind();
        }
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            int favSport = ddlSportName.SelectedIndex > -1 ? ddlSportName.SelectedIndex + 1 : ddlSportName.SelectedIndex;

            User newUser = new User() { email=Email.Text, SportID = favSport };

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }


    }
}