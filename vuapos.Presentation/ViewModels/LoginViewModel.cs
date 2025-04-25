using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Services.Interfaces;

namespace vuapos.Presentation.ViewModels
{
    // LoginViewModel.cs
    public class LoginViewModel
    {
        private readonly IUserSession _userSession;

        public LoginViewModel(IUserSession userSession)
        {
            _userSession = userSession;
        }

        public void OnLoginSuccess(/*IUserSession response*/)
        {
            _userSession.Username = "khoi";
            _userSession.Token = "123";
            _userSession.UserId = "12";
            _userSession.role = "admin";
        }
    }

}
