﻿using BusinessLayerInterfaces.BusinessModels;
using BusinessLayerInterfaces.UserServices;
using DALInterfaces.Repositories;

namespace BusinessLayer.UserServices
{
	public class HomeServices : IHomeServices
	{
		private IUserRepository _userRepository;

<<<<<<< HEAD
=======
		public HomeServices(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

>>>>>>> main
		public IEnumerable<UserBlm> GetLastLoginUsers()
			=> _userRepository
				.GetAll()
				.Take(5)
				.Select(x => new UserBlm
				{
					Id = x.Id,
					Name = x.Name,
				});
	}
}