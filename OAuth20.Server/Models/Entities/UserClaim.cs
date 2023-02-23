using System;
namespace OAuth20.Server.Models.Entities
{
	public class UserClaim
	{
		public UserClaim()
		{
		}

		public string Type { get; set; }

        public string Value { get; set; }
    }
}

