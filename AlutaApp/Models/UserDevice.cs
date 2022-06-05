using System;
namespace AlutaApp.Models
{
	public class UserDevice
	{
        public int Id { get; set; }

        public int UserId { get; set; }

        public string DeviceHash { get; set; }

        public string DeviceToken { get; set; }

        public bool IsAndroid { get; set; }

        public UserDevice()
		{
		}
	}
}

