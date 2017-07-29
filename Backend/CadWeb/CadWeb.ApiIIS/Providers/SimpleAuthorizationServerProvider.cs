using CadWeb.ApiIIS.Utils;
using CadWeb.Dados.DataContexts;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace OAuthServer.Api.Security
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }



        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var header = context.OwinContext.Response.Headers.SingleOrDefault(h => h.Key == "Access-Control-Allow-Origin");
            if (header.Equals(default(KeyValuePair<string, string[]>)))
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            }
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            //using (IUserRepository _repository = new UserRepository(new Data.DataContexts.OAuthServerDataContext()))
                try
            {
                Encrypt encrypt = new Encrypt();
                CadWebDataContext db = new CadWebDataContext();
                var user = context.UserName;
                var password = context.Password;
                var values = db.Usuarios.Where(x=> x.Email == user).ToList();
                if (values.Count <=0) {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                bool found = false;
                foreach (var value in values) {
                    if (encrypt.ComparaMD5(value.Senha, password)) {

                        found =true;
                    }
                }
                
                if (!found)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var roles = new List<string>();
                roles.Add(user);
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user));
               // identity.AddClaim(new Claim("role", "user"));

                GenericPrincipal principal = new GenericPrincipal(identity,roles.ToArray() );
                Thread.CurrentPrincipal = principal;

                context.Validated(identity);
            }
            catch { context.SetError("invalid_grant", "Não foi possivel executar a operação"); }
        }
    }
}