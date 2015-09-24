using System;
using BeerCellier.Core;
using BeerCellier.Entities;

namespace BeerCellier.Tests.Fakes
{
    class FakeSessionContext : ISessionContext
    {
        private readonly User _currentLoggedUser;

        public FakeSessionContext(User currentLoggedUser)
        {
            _currentLoggedUser = currentLoggedUser;
        }

        public User GetCurrentLoggedUser()
        {
            return _currentLoggedUser;
        }

        public bool SignIn(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void signOut()
        {
            throw new NotImplementedException();
        }
    }
}
