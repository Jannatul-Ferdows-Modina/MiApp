using AppMGL.DAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AppMGL.DAL.Repository.Security
{
	public class AuthRepository : IDisposable
	{
		private readonly AppMGL.DAL.Models.AppMGL _ctx;

		private readonly UserManager<IdentityUser> _userManager;

		public AuthRepository()
		{
			_ctx = new AppMGL.DAL.Models.AppMGL();
			_userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
			_userManager.UserLockoutEnabledByDefault = true;
			_userManager.MaxFailedAccessAttemptsBeforeLockout = Convert.ToInt32(ConfigurationManager.AppSettings["MaxFailedAccess"]);
			_userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromDays(73000.0);
		}

		public async Task<IdentityResult> RegisterUser(UserModel userModel)
		{
			IdentityUser user = new IdentityUser
			{
				UserName = userModel.UserName
			};
			return await _userManager.CreateAsync(user, userModel.Password);
		}

		public async Task<IdentityUser> FindByName(string userName)
		{
			return await _userManager.FindByNameAsync(userName);
		}

		public async Task<bool> CheckPassword(IdentityUser user, string password)
		{
			return await _userManager.CheckPasswordAsync(user, password);
		}

		public async Task<IdentityResult> ResetAccessFailedCount(IdentityUser user)
		{
			return await _userManager.ResetAccessFailedCountAsync(user.Id);
		}

		public async Task<IdentityResult> AccessFailed(IdentityUser user)
		{
			return await _userManager.AccessFailedAsync(user.Id);
		}

		public async Task<DateTimeOffset> GetLockoutEnabled(IdentityUser user)
		{
			return await _userManager.GetLockoutEndDateAsync(user.Id);
		}

		public async Task<IdentityUser> FindUser(string userName, string password)
		{
			return await _userManager.FindAsync(userName, password);
		}

		public LG_USER FindUser(string aspNetUserId)
		{
			return _ctx.LG_USER.SingleOrDefault((LG_USER x) => x.AspNetUserId == aspNetUserId);
		}

		public List<LG_SITE> FindSite(string userId)
		{
			IQueryable<LG_CONTACT> source = from u in _ctx.LG_USER
			join c in _ctx.LG_CONTACT on u.CntId equals c.CntId
			where u.UsrId.ToString() == userId
			select c;
			if (source.FirstOrDefault() != null)
			{
				decimal? cwtId = source.FirstOrDefault().CwtId;
				decimal d = 1;
				if (!(cwtId.GetValueOrDefault() == d) || !cwtId.HasValue)
				{
					cwtId = source.FirstOrDefault().CwtId;
					if (!(cwtId.GetValueOrDefault() == default(decimal)) || !cwtId.HasValue)
					{
						goto IL_058e;
					}
				}
				return (from sr in _ctx.LG_SITE_CONTACT_ROLE
				join u in _ctx.LG_USER on sr.CntId equals u.CntId
				from s in _ctx.LG_SITE
				where u.UsrId.ToString() == userId
				select s).Distinct().ToList();
			}
			goto IL_058e;
			IL_058e:
			return (from sr in _ctx.LG_SITE_CONTACT_ROLE
			join s in _ctx.LG_SITE on sr.SitId equals s.SitId
			join u in _ctx.LG_USER on sr.CntId equals u.CntId
			where u.UsrId.ToString() == userId
			select s).Distinct().ToList();
		}

		public AspNetClient FindClient(string clientId)
		{
			return _ctx.AspNetClient.Find(clientId);
		}

		public async Task<bool> AddRefreshToken(AspNetRefreshToken token)
		{
			AspNetRefreshToken aspNetRefreshToken = _ctx.AspNetRefreshToken.SingleOrDefault((AspNetRefreshToken r) => r.Subject == token.Subject && r.ClientId == token.ClientId);
			if (aspNetRefreshToken != null)
			{
				await RemoveRefreshToken(aspNetRefreshToken);
			}
			_ctx.AspNetRefreshToken.Add(token);
			return await _ctx.SaveChangesAsync() > 0;
		}

		public async Task<bool> RemoveRefreshToken(string refreshTokenId)
		{
			AspNetRefreshToken aspNetRefreshToken = await _ctx.AspNetRefreshToken.FindAsync(refreshTokenId);
			if (aspNetRefreshToken != null)
			{
				_ctx.AspNetRefreshToken.Remove(aspNetRefreshToken);
				return await _ctx.SaveChangesAsync() > 0;
			}
			return false;
		}

		public async Task<bool> RemoveRefreshToken(AspNetRefreshToken refreshToken)
		{
			_ctx.AspNetRefreshToken.Remove(refreshToken);
			return await _ctx.SaveChangesAsync() > 0;
		}

		public async Task<AspNetRefreshToken> FindRefreshToken(string refreshTokenId)
		{
			return await _ctx.AspNetRefreshToken.FindAsync(refreshTokenId);
		}

		public List<AspNetRefreshToken> GetAllRefreshTokens()
		{
			return _ctx.AspNetRefreshToken.ToList();
		}

		public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
		{
			return await _userManager.FindAsync(loginInfo);
		}

		public async Task<IdentityResult> CreateAsync(IdentityUser user)
		{
			return await _userManager.CreateAsync(user);
		}

		public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
		{
			return await _userManager.AddLoginAsync(userId, login);
		}

		public void Dispose()
		{
			_ctx.Dispose();
			_userManager.Dispose();
		}

		public void InsertLoginHistory(string userEmail, bool isValid)
		{
			try
			{
				_ctx.ExecuteQuery<int>("EXEC dbo.USP_LG_USER_LOGIN_HISTORY @USER_EMAIL,@IS_VALID", new object[2]
				{
					new SqlParameter("USER_EMAIL", userEmail),
					new SqlParameter("IS_VALID", isValid)
				}).ToList();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}