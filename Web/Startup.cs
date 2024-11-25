using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Stripe;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Twilio;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Logging;

namespace MountainMap.Web
{
    public class Startup
    {
        IWebHostEnvironment _hostingEnvironment;

        internal JsonSerializerOptions jsonSerializerOptions = new()
        {
            //WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public Startup(IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            //Klyk.Startup.Configuration = Configuration;
            //Klyk.Startup.HostingEnvironment = _hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Klyk.Startup.ConfigureServices(services);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;

                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });


            if (_hostingEnvironment.IsDevelopment())
            {
                //services.AddDatabaseDeveloperPageExceptionFilter();
            }
                

            var authentication = services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });


            var authConfig = Configuration.GetSection("Authentication");

            authentication.AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = authConfig["Microsoft:ApplicationId"];
                microsoftOptions.ClientSecret = authConfig["Microsoft:Password"];
                microsoftOptions.SaveTokens = true;
                //microsoftOptions.Events.OnTicketReceived += OnTicketRecieved;

            });

            authentication.AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = authConfig["Google:ClientId"];
                googleOptions.ClientSecret = authConfig["Google:ClientSecret"];
                googleOptions.SaveTokens = true;
                //googleOptions.Events.OnTicketReceived += OnTicketRecieved;

                //googleOptions.Scope.Add("https://www.googleapis.com/auth/youtube.upload");

                googleOptions.Events.OnCreatingTicket = ctx =>
                {

                    List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

                    tokens.Add(new AuthenticationToken()
                    {
                        Name = "TicketCreated",
                        Value = DateTime.UtcNow.ToString()
                    });

                    ctx.Properties.StoreTokens(tokens);

                    return Task.CompletedTask;
                };

                googleOptions.Events.OnRemoteFailure = ctx =>
                {
                    var authProperties = googleOptions.StateDataFormat.Unprotect(ctx.Request.Query["state"]);
                    // do something
                    ctx.HandleResponse();

                    ctx.Response.Redirect("Account/Login?error=" + ctx.Failure.Message);

                    return Task.FromResult(0);

                    //return Task.CompletedTask;
                };

                //googleoptions.Events.OnRemoteFailure += OnRemoteFailure;
                //googleoptions.Events.OnCreatingTicket += OnCreatingTicket;
                //googleoptions.Events.OnRedirectToAuthorizationEndpoint += OnRedirect;

                //googleoptions.CorrelationCookie.SameSite = SameSiteMode.None;

                //googleoptions.AuthorizationEndpoint = "https://3597-75-169-0-173.ngrok.io/signin-google";

                //googleoptions.CorrelationCookie = new Microsoft.AspNetCore.Http.CookieBuilder
                //{
                //    HttpOnly = false,
                //    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                //    SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.None,
                //    Expiration = TimeSpan.FromMinutes(10)
                //};

                //googleoptions.CallbackPath = "/auth/google";

            });

            authentication.AddStrava(options =>
            {
                options.ClientId = authConfig["Strava:ClientId"];
                options.ClientSecret = authConfig["Strava:ClientSecret"];
                options.SaveTokens = true;
                options.Scope.Add("activity:write");
                options.Scope.Add("profile:read_all");

                //options.Events.OnTicketReceived += OnTicketRecieved;

            });

            
            authentication.AddInstagram(options =>
             {

                 options.ClientId = authConfig["Instagram:AppId"];
                 options.ClientSecret = authConfig["Instagram:AppSecret"];
                 options.SaveTokens = true;

                 //options.Events.OnTicketReceived += OnTicketRecieved;

             });
            //.AddFacebook(options =>
            //{
            //    options.AppId = Configuration["Authentication:Facebook:AppId"];
            //    options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            //    options.SaveTokens = true;
            //    //options.Events.OnTicketReceived += OnTicketRecieved;
            //});

            //authentication.AddOpenIdConnect("apple", async options =>
            // {
            //     options.ResponseType = "code id_token"; // hybrid flow due to lack of PKCE support
            //     options.ResponseMode = "form_post"; // form post due to prevent PII in the URL
            //     options.UsePkce = false; // apple does not currently support PKCE (April 2021)
            //     options.DisableTelemetry = true;

            //     options.Scope.Clear(); // apple does not support the profile scope
            //     options.Scope.Add("openid");
            //     options.Scope.Add("email");
            //     options.Scope.Add("name");

            //     // TODO
            // });

            var Apple = Configuration.GetSection("Authentication:Apple");

            if(_hostingEnvironment.IsDevelopment())
                IdentityModelEventSource.ShowPII = true;

            if (Apple.Exists())
            {
                authentication.AddApple(options =>
                {
                    options.ClientId = Apple["ClientId"];
                    options.KeyId = Apple["KeyId"];
                    options.TeamId = Apple["TeamId"];
                    //options.UsePkce = false;
                    options.SaveTokens = true;

                    //options.Scope.Clear();
                    //options.Scope.Add("email name");

                    //options.Events.OnTicketReceived += OnTicketRecieved;

                    if (_hostingEnvironment.IsDevelopment())
                    {
                        //options.ReturnUrlParameter = "dev.klyk.app";
                    }

                    options.Events.OnCreatingTicket = ctx =>
                    {
                        List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

                        tokens.Add(new AuthenticationToken()
                        {
                            Name = "TicketCreated",
                            Value = DateTime.UtcNow.ToString()
                        });

                        ctx.Properties.StoreTokens(tokens);

                        return Task.CompletedTask;
                    };

                    //options.BackchannelHttpHandler = new HttpClientHandler()
                    //{
                    //    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                    //    Proxy = new WebProxy(Configuration["System:Proxy"])
                    //};

                    options.UsePrivateKey((keyId) =>
                        _hostingEnvironment.ContentRootFileProvider.GetFileInfo($"AuthKey_{keyId}.p8"));

                });

            }

            var key = Encoding.UTF8.GetBytes(Configuration.GetSection("Application")["JWT:SecurityKey"]);

            authentication
            .AddCookie(options =>
            {

                //options.Cookie.HttpOnly = true;
                //options.SlidingExpiration = true;
                //options.LoginPath = "/Identity/Account/Login";
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                //options.Cookie.Name = "mm_auth_cookie";

                //options.Cookie.SameSite = SameSiteMode.None;
                //options.Events = new CookieAuthenticationEvents
                //{
                //    OnRedirectToLogin = redirectContext =>
                //    {
                //        if (redirectContext.Request.Path.StartsWithSegments("/api") && redirectContext.Response.StatusCode == 200)
                //        {
                //            redirectContext.Response.StatusCode = 401;
                //        }

                //        return Task.CompletedTask;
                //    }
                //};

            })
            .AddJwtBearer(options =>
            {
                //options.Audience = "http://localhost:5001/";
                //options.Authority = "https://localhost:5001/";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "mountainmap.com",
                    ValidAudience = "mountainmap.com",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };


                // We have to hook the OnMessageReceived event in order to
                // allow the JWT authentication handler to read the access
                // token from the query string when a WebSocket or 
                // Server-Sent Events request comes in.

                // Sending the access token in the query string is required when using WebSockets or ServerSentEvents
                // due to a limitation in Browser APIs. We restrict it to only calls to the
                // SignalR hub in this code.
                // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
                // for more information about security considerations when using
                // the query string to transmit the access token.
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/hubs/")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });


            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });


            var physicalProvider = _hostingEnvironment.ContentRootFileProvider;

            services.AddSingleton(physicalProvider);

            services.AddProblemDetails();



            services.AddControllersWithViews()
                .ConfigureApiBehaviorOptions(options =>
            {
                //options.SuppressConsumesConstraintForFormFileParameters = true;
                //options.SuppressInferBindingSourcesForParameters = true;
                //options.SuppressModelStateInvalidFilter = true;
                //options.SuppressMapClientErrors = true;
                //options.ClientErrorMapping[StatusCodes.Status500InternalServerError].Link = "https://httpstatuses.com/500";
                //options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
            }).AddJsonOptions(options =>
            {

                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            });

            services.AddHttpContextAccessor();

            //services.AddTransient<AuthenticationStateHandler>();

            //services.AddHttpClient("HttpMessageHandler")
            //    .AddHttpMessageHandler<AuthenticationStateHandler>();

            //services.AddQuickGridEntityFrameworkAdapter();
            services.AddRazorComponents();
                
            services.AddAuthorizationCore();
            services.AddCascadingAuthenticationState();

            services.AddServerSideBlazor();


           // razorPages.AddRazorRuntimeCompilation();
            
            if (_hostingEnvironment.IsDevelopment())
            {

//                services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
//                {

//                    var libraryPath = Path.GetFullPath(
//                        Path.Combine(_hostingEnvironment.ContentRootPath, "..", "..", "Klyk", "Klyk"));

//                    if (Directory.Exists(libraryPath))
//                    {
//#if DEBUG
//                        options.FileProviders.Add(new PhysicalFileProvider(libraryPath));
//#endif
//                    }

//                    libraryPath = Path.GetFullPath(
//                        Path.Combine(_hostingEnvironment.ContentRootPath, "..", "MountainMap"));

//                    if (Directory.Exists(libraryPath))
//                    {
//                        options.FileProviders.Add(new PhysicalFileProvider(libraryPath));
//                    }

//                });

            }
            else
            {
                
            }

            var application = Configuration.GetSection("Application");

            if (application.Exists())
            {
                services.AddSession(options =>
                {
                    // Set a short timeout for easy testing.
                    options.IdleTimeout = TimeSpan.FromMinutes(20);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                    options.Cookie.Name = application["Domain"];
                });


               
            }

         
        

            if (_hostingEnvironment.IsDevelopment())
            {
                //services.AddHostedService<TunnelService>();
            }
                

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime, ILogger<Startup> logger)
        {
            

            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            var domainName = "www.mountainmap.com";

            //if (env.IsStaging()) {            
            //    domainName = "staging.mountainmap.com";
            //}

            var ApplePayDomainCreateOptions = new ApplePayDomainCreateOptions
            {
                DomainName = domainName
            };

            var service = new ApplePayDomainService();

            try
            {
                var domain = service.Create(ApplePayDomainCreateOptions);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            string accountSid = Configuration.GetSection("Application:Twilio")["SID"];
            string authToken = Configuration.GetSection("Application:Twilio")["Token"];

            TwilioClient.Init(accountSid, authToken);

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Error");
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            }

            //app.UseExceptionHandler("/Error");

            var options = new RewriteOptions();

            if (env.IsProduction())
            {

#if !DEBUG
                //options.AddRedirectToHttps(StatusCodes.Status303SeeOther);
                options.AddRedirectToWwwPermanent();
#endif
            }
            else
            {

            }


            //app.UseWhen(context => context.Request.Host == new HostString("localhost"), config=>
            //{
            //    //await next();
            
                
            //});

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), config =>
            {
                config.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var problem = new ProblemDetails()
                        {
                            Status = context.Response.StatusCode,
                        };

                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        string exceptionMessage = null;

                        var error = exceptionHandlerPathFeature?.Error;
                        if (error != null)
                        {

                            var message = error.Message;

                            exceptionMessage += message;

                            if (error.InnerException != null)
                            {
                                exceptionMessage += "Inner Exception: " + error.InnerException.Message;
                            }

                            exceptionMessage += "Inner Exception: " + error.StackTrace;

                            var domain = context.Request.Host.Value.ToString();

                            exceptionMessage += "<br><br>";
                            exceptionMessage += "Path: " + exceptionHandlerPathFeature.Path;

                            problem.Detail = exceptionMessage;

                            //await _emailSender.SendEmailAsync("support@klyk.io", domain + " Error", ExceptionMessage);

                        }


                        await context.Response.WriteAsJsonAsync(problem);
                    });
                });
            });

   
            //app.Use(async (context, next) =>
            //{
            //    context.Response.OnStarting(() =>
            //    {
            //        int responseStatusCode = context.Response.StatusCode;
            //        if (responseStatusCode == (int)HttpStatusCode.Created)
            //        {
            //            IHeaderDictionary headers = context.Response.Headers;
            //            StringValues locationHeaderValue = string.Empty;
            //            //if (headers.TryGetValue("location", out locationHeaderValue))
            //            //{
            //            //    context.Response.Headers.Remove("location");
            //            //    context.Response.Headers.Add("location", "new location header value");
            //            //}
            //        }
            //        else if (responseStatusCode == (int)HttpStatusCode.Redirect)
            //        {
            //            IHeaderDictionary headers = context.Response.Headers;
            //            StringValues locationHeaderValue = string.Empty;
            //            if (headers.TryGetValue("location", out locationHeaderValue))
            //            {
            //                context.Response.Headers.Remove("location");
            //                StringWriter myWriter = new StringWriter();

            //                var test = HttpUtility.UrlDecode(locationHeaderValue.ToString(), Encoding.UTF8);

            //                //if (test.Contains("redirect_uri")) { 

            //                //    var uri = new Uri(locationHeaderValue);

            //                //    string param1 = HttpUtility.ParseQueryString(uri.Query).Get("redirect_uri");

            //                //    if(param1 != null)
            //                //    {
            //                //        var newValue = test.Replace("https://localhost:44375", "https://61e7057729f4.ngrok.io");
            //                //        context.Response.Headers.Add("location", new Uri(newValue).ToString());
            //                //    }

            //                //}
            //            }
            //        }
            //        return Task.FromResult(0);
            //    });
            //    await next();
            //});


            //options.AddRedirectToHttps();

            app.UseRewriter(options);

            app.UseHttpsRedirection();
            //app.UseStaticFiles();


            //app.MapStaticAssets();

            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain"
            });

            //app.UseCookiePolicy(new CookiePolicyOptions()
            //{
            //    HttpOnly = HttpOnlyPolicy.Always,
            //    Secure = CookieSecurePolicy.Always,
            //    MinimumSameSitePolicy = SameSiteMode.Strict
            //});

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();

            // app.UseApiVersioning();

            app.UseEndpoints(endpoints =>
            {

 
                endpoints.MapControllers();


                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();

                endpoints.MapBlazorHub();

            });



        }


        //private static IEdmModel GetEdmModel()
        //{
        //    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

        //    builder.EntitySet<Resort>("Resorts").EntityType.Ignore(ui => ui.Boundary);
        //    builder.EntitySet<Event>("Events");
        //    builder.EntitySet<Track>("Tracks");
        //    builder.EntitySet<WayPoint>("WayPoints");

        //    builder.EnableLowerCamelCase();

        //    return builder.GetEdmModel();
        //}

        public static bool DisallowsSameSiteNone(string userAgent)
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
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
        private void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                // TODO: Use your User Agent library of choice here.
                if (DisallowsSameSiteNone(userAgent))
                {
                    // For .NET Core < 3.1 set SameSite = (SameSiteMode)(-1)
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }


    
        private void OnShutdown()
        {
            Debug.WriteLine("");

            //Wait while the data is flushed
            System.Threading.Thread.Sleep(1000);
        }


    }
}
