using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Xml;

namespace SimpleNet.ServiceHost.Validators
{
    public class CustomUserNamePasswordValidatorSecurityTokenHandler : UserNameSecurityTokenHandler
    {
        public UserNamePasswordValidator Validator { get; set; }

        public override bool CanValidateToken
        {
            get { return true; }
        }

        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException();
            }

            var userNameToken = token as UserNameSecurityToken;
            if (userNameToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }


            Validator.Validate( userNameToken.UserName, userNameToken.Password );


            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(
                    "http://schemas.microsoft.com/ws/2008/06/identity/claims/ClaimTypes.AuthenticationInstant",
                    XmlConvert.ToString( DateTime.UtcNow, "yyyy-MM-ddTHH:mm:ss.fffZ" ),
                    "http://www.w3.org/2001/XMLSchema#dateTime" ),
                new Claim( System.IdentityModel.Claims.ClaimTypes.Name, userNameToken.UserName)
            };

            return new ReadOnlyCollection<ClaimsIdentity>(new List<ClaimsIdentity> { new ClaimsIdentity(claims, "Password") });
        }
    }
}