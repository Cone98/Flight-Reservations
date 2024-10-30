using Letovi.Web.Controllers;
using Letovi.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Letovi.Web.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            // kreiranje potrebnih instanci za UserManager<IdentityUser> i koristi ih za instanciranje SignInManager<IdentityUser>
            var userStore = new Mock<IUserStore<IdentityUser>>().Object;
            var identityOptions = new Mock<IOptions<IdentityOptions>>().Object;
            var passwordHasher = new Mock<IPasswordHasher<IdentityUser>>().Object;
            var userValidators = new List<IUserValidator<IdentityUser>> { new Mock<IUserValidator<IdentityUser>>().Object };
            var passwordValidators = new List<IPasswordValidator<IdentityUser>> { new Mock<IPasswordValidator<IdentityUser>>().Object };
            var keyNormalizer = new Mock<ILookupNormalizer>().Object;
            var errors = new Mock<IdentityErrorDescriber>().Object;
            var services = new Mock<IServiceProvider>().Object;
            var logger = new Mock<ILogger<UserManager<IdentityUser>>>().Object;

            //  UserManager instanca
            var userManager = new UserManager<IdentityUser>(userStore, identityOptions, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger);

            // Mock SignInManager
            _mockSignInManager = new Mock<SignInManager<IdentityUser>>(userManager, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);

            _controller = new AccountController(_mockSignInManager.Object);
        }

        [Fact]
        public async Task Login()
        {
            
            var loginModel = new LoginViewModel
            {
                Username = "testuser",
                Password = "Test123!"
            };
            

            _mockSignInManager.Setup(m => m.PasswordSignInAsync(loginModel.Username, loginModel.Password, false, false))
                              .ReturnsAsync(SignInResult.Success);

            
            var result = await _controller.Login(loginModel) as RedirectToActionResult;


            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }
       
       

        [Fact]
        public async Task Logout()
        {
          
            var result = await _controller.Logout() as RedirectToActionResult;

          
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }

        [Fact]
        public void AccessDenied()
        {
            
            var result = _controller.AccessDenied() as ViewResult;

            
            Assert.NotNull(result);
        }
    }
}
