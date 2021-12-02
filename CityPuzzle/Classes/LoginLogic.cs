using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public class LoginLogic
    {
        public event EventHandler OnUserIsLogedIn;
        public event EventHandler OnUserNotFound;
        public void LogIn(string username, string pass)
        {
            SimpleUser current = Sql.GetCurrentUser();
            var tempUser = new User(new UserVerifier());
            if (current != null && tempUser.CheckHachedPassword(current.UserName, current.HashedPass))
            {
                OnUserIsLogedIn?.Invoke(this, EventArgs.Empty);
            }
            else if (tempUser.CheckPassword(username, pass))
            {
                OnUserIsLogedIn?.Invoke(this, EventArgs.Empty);
            }
            else if (username == null && pass == null) // App just launched without loged in user
            {
                return;
            }
            else
            {
                OnUserNotFound?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
