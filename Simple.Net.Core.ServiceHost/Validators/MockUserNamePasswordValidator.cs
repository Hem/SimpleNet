using System;
using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace Simple.Net.Core.ServiceHost.Validators
{
    public class MockUserNamePasswordValidator: UserNamePasswordValidator
    {
        public override void Validate( string userName, string password )
        {
            if(userName == null)
                throw new ArgumentNullException("userName");

            if (password == null)
                throw new ArgumentNullException("password");

            if( ! userName.Equals( "username" ) && password.Equals( "password" ) )
                throw new FaultException( "Invalid username and password provided" );

        }
    }
}
