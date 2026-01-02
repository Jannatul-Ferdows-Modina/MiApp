using AppMGL.DTO.Setup;
using System;

namespace AppMGL.DTO.Security
{
	public class UserDTO
	{
		public decimal UsrId
		{
			get;
			set;
		}

		public decimal CntId
		{
			get;
			set;
		}

		public string AspNetUserId
		{
			get;
			set;
		}

		public string UsrPwd
		{
			get;
			set;
		}

		public string UsrTempPwd
		{
			get;
			set;
		}

		public DateTime? UsrPwdCreatedTs
		{
			get;
			set;
		}

		public DateTime? UsrValidFrom
		{
			get;
			set;
		}

		public DateTime? UsrValidTo
		{
			get;
			set;
		}

		public decimal? UsrFailedCount
		{
			get;
			set;
		}

		public decimal? UsrIsLocked
		{
			get;
			set;
		}

		public string UsrLockedComments
		{
			get;
			set;
		}

		public decimal? UsrPwdNeverExpire
		{
			get;
			set;
		}

		public DateTime? UsrLastLoginTs
		{
			get;
			set;
		}

		public bool? UsrIsPowerUser
		{
			get;
			set;
		}

		public DateTime? UsrCreatedTs
		{
			get;
			set;
		}

		public DateTime? UsrUpdatedTs
		{
			get;
			set;
		}

		public decimal? UsrCreatedBy
		{
			get;
			set;
		}

		public decimal? UsrUpdatedBy
		{
			get;
			set;
		}

		public string UsrSmtpUsername
		{
			get;
			set;
		}

		public string UsrSmtpPassword
		{
			get;
			set;
		}

		public string FullName
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string DptName
		{
			get;
			set;
		}
        public string ContactType
        {
            get;
            set;
        }

        public int? TotalCount
		{
			get;
			set;
		}

		public ContactDTO Contact
		{
			get;
			set;
		}

        public SiteRoleDTO[] UserUnitRoleList
        {
            get;
            set;
        }
	}
}