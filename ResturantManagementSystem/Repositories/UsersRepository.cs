using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturantManagementSystem.Data;
using ResturantManagementSystem.Helpers;
using ResturantManagementSystem.Interfaces.IRepository;
using ResturantManagementSystem.Services;
using ResturantManagementSystem.ViewModels;

namespace ResturantManagementSystem.Repositories
{
    public class UsersRepository : GenericRepository<IdentityUser>, IUsersRepository
	{
        private readonly ApplicationDbContext appDbContext;
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly IMapper mapper;

		public UsersRepository(ApplicationDbContext appDbContext, UserManager<IdentityUser> userManager, IMapper mapper, SignInManager<IdentityUser> signInManager) : base(appDbContext)
        {
            this.appDbContext = appDbContext;
			this.userManager = userManager;
			this.mapper = mapper;
			this.signInManager = signInManager;
		}

		public IMapper Mapper { get; }

		public bool IsUniqueUser(string Email)
        {
            var result = appDbContext.Users.FirstOrDefault(x => x.NormalizedEmail == Email.ToUpper());
			return result == null;
		}
	}
}
