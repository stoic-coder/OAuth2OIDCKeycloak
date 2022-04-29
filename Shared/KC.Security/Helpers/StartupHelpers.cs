// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="StartupHelpers.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using KC.Security.AuthorizationHelpers;
using KC.Security.HttpHandlers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace KC.Security.Helpers;

/// <summary>
/// Class StartupHelpers.
/// </summary>
public static class StartupHelpers
    {
    /// <summary>
    /// Configures the HTTP clients.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="services">The services.</param>
    public static void ConfigureHttpClients(IConfiguration configuration, IServiceCollection services)
        {
            services.AddTransient(typeof(BearerTokenHandler));
            services.AddHttpContextAccessor();
            services.AddHttpClient(HttpClients.IdpApiClient, client =>
            {
                var idpApiUri = configuration.GetValue<string>("Options:Authority");
                client.BaseAddress = new Uri(idpApiUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            }).AddHttpMessageHandler<BearerTokenHandler>();

          
        }
    /// <summary>
    /// Configures the JWT bearer.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="currentEnvironment">The current environment.</param>
    /// <param name="services">The services.</param>
    public static void ConfigureJwtBearer(IConfiguration configuration,IWebHostEnvironment  currentEnvironment,IServiceCollection services)
        {
            services.AddAuthorization();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthorization();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.Authority = configuration["Authority"];
                options.Audience = configuration["Audience"];

                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";

                        return c.Response.WriteAsync(currentEnvironment.IsDevelopment() ? c.Exception.ToString() : "An error occured processing your authentication.");
                    }
                };
            });
        }
    /// <summary>
    /// Configures the open identifier connect.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="currentEnvironment">The current environment.</param>
    /// <param name="services">The services.</param>
    /// <param name="isSameSiteCookieSecure">if set to <c>true</c> [is same site cookie secure].</param>
    public static void ConfigureOpenIdConnect(IConfiguration configuration,IWebHostEnvironment  currentEnvironment,IServiceCollection services, bool isSameSiteCookieSecure = true)
        {
            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                
                if (!isSameSiteCookieSecure) return;
                
                options.Secure = CookieSecurePolicy.None;
                options.OnAppendCookie = cookieContext => 
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext => 
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = configuration.GetValue<string>("Options:Authority");
                    options.RequireHttpsMetadata = configuration.GetValue<bool>("Options:RequireHttpsMetadata");
                    options.ClientId = configuration.GetValue<string>("Options:ClientId");
                    options.ResponseType = "code id_token";
                    options.SaveTokens = true;
                    options.ClientSecret = configuration.GetValue<string>("Options:ClientSecret");
                    options.Scope.Add("profile");
                    options.Scope.Add("openid");
                    options.Scope.Add("email");
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Events = new OpenIdConnectEvents
                    {
                        OnAccessDenied = context =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/");
                            return Task.CompletedTask;
                        },
                        OnRedirectToIdentityProvider = (context) =>
                        {
                            var isBehindProxy = configuration.GetValue<bool>("IsBehindProxy");
                            var isAuthEnabled = configuration.GetValue<bool>("IsAuthEnabled");
                            var redirectUri = configuration.GetValue<string>("Options:RedirectUri");
                            if (isBehindProxy)
                            {
                                context.ProtocolMessage.RedirectUri = redirectUri;
                            }

                            return Task.FromResult(0);
                        },
                    };
                });

        }
    /// <summary>
    /// Checks the same site.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="options">The options.</param>
    private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
           if (options.SameSite != SameSiteMode.None) return;
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            if (DisallowsSameSiteNone(userAgent))
            {
                 options.SameSite = SameSiteMode.Unspecified;
            }
        }
    /// <summary>
    /// Disallowses the same site none.
    /// </summary>
    /// <param name="userAgent">The user agent.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    private static bool DisallowsSameSiteNone(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return false;
            }

            // Cover all iOS based browsers here. This includes:
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the iOS networking stack
            if (userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack. This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") && 
                userAgent.Contains("Version/") && userAgent.Contains("Safari"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None, 
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions, 
            // but pre-Chromium Edge does not require SameSite=None.
            return userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6") || userAgent.Contains("Chrome/7") || userAgent.Contains("Chrome/8") || userAgent.Contains("Chrome/9")|| userAgent.Contains("Chrome/10") || userAgent.Contains("Chrome/11");
        }
    }
