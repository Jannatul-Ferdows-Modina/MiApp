using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_USER
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
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

		public DateTime UsrPwdCreatedTs
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

		public DateTime UsrCreatedTs
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

		public virtual LG_CONTACT LG_CONTACT
		{
			get;
			set;
		}
		
	}
}
