using Microsoft.AspNetCore.Authorization;


namespace KC.Security.AuthorizationHelpers
{
    public static class Policies
    {
        public static void AddPolicies(AuthorizationOptions authorizationOptions)
        {
            authorizationOptions.AddPolicy(
                "IsAuthenticatedUser",
                policyBuilder => { policyBuilder.RequireAuthenticatedUser(); });
        
            authorizationOptions.AddPolicy(
                Roles.DefaultRole,
                policyBuilder => { policyBuilder.RequireClaim("hlm_user_roles", Roles.DefaultRole);  });
            
            authorizationOptions.AddPolicy(
                Roles.AdminRole,
                policyBuilder => { policyBuilder.RequireClaim("hlm_user_roles", Roles.AdminRole); });
            
            authorizationOptions.AddPolicy(
                Roles.DataReader,
                policyBuilder => { policyBuilder.RequireClaim("hlm_user_roles", Roles.DataReader); });
            
            authorizationOptions.AddPolicy(
                Roles.DataWriter,
                policyBuilder => { policyBuilder.RequireClaim("hlm_user_roles", Roles.DataWriter); });
            authorizationOptions.AddPolicy(
                Roles.CRMRole,
                policyBuilder => { policyBuilder.RequireClaim("hlm_user_roles", Roles.CRMRole); });
            authorizationOptions.AddPolicy(
                Roles.DispatcherRole,
                policyBuilder => { policyBuilder.RequireClaim("hlm_user_roles", Roles.DispatcherRole); });            
        }

       
    }
}