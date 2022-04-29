// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="Policies.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Authorization;


namespace KC.Security.AuthorizationHelpers
{
    /// <summary>
    /// Class Policies.
    /// </summary>
    public static class Policies
    {
        /// <summary>
        /// Adds the policies.
        /// </summary>
        /// <param name="authorizationOptions">The authorization options.</param>
        public static void AddPolicies(AuthorizationOptions authorizationOptions)
        {
            authorizationOptions.AddPolicy(
                "IsAuthenticatedUser",
                policyBuilder => { policyBuilder.RequireAuthenticatedUser(); });
        
            authorizationOptions.AddPolicy(
                Roles.DefaultRole,
                policyBuilder => { policyBuilder.RequireClaim("user_roles", Roles.DefaultRole);  });
            
            authorizationOptions.AddPolicy(
                Roles.AdminRole,
                policyBuilder => { policyBuilder.RequireClaim("user_roles", Roles.AdminRole); });
            
            authorizationOptions.AddPolicy(
                Roles.DataReader,
                policyBuilder => { policyBuilder.RequireClaim("user_roles", Roles.DataReader); });
            
            authorizationOptions.AddPolicy(
                Roles.DataWriter,
                policyBuilder => { policyBuilder.RequireClaim("user_roles", Roles.DataWriter); });
           
        }

       
    }
}