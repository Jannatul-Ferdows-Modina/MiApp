using AppMGL.DAL.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Configuration;

namespace AppMGL.DAL.Models
{
	public class AppUserManager : UserManager<IdentityUser>
	{
		public AppUserManager(IUserStore<IdentityUser> store)
			: base(store)
		{
		}

		public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
		{
			AppUserManager appUserManager = new AppUserManager(new UserStore<IdentityUser>(context.Get<AppMGL>()));
			appUserManager.UserLockoutEnabledByDefault = true;
			appUserManager.MaxFailedAccessAttemptsBeforeLockout = Convert.ToInt32(ConfigurationManager.AppSettings["MaxFailedAccess"]);
			appUserManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromDays(73000.0);
			appUserManager.EmailService = new EmailService();
			IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				appUserManager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(dataProtectionProvider.Create("ASP.NET Identity"))
				{
					TokenLifespan = TimeSpan.FromHours(24.0)
				};
			}
			return appUserManager;
		}
	}
}
