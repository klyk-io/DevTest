using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Logging;

[assembly: HostingStartup(typeof(Klyk.Startup))]
namespace Klyk
{

    public class Startup : IHostingStartup
    {
        private IWebHostEnvironment _hostingEnvironment;
        public static IConfiguration Configuration { get; set; }

        public Startup()
        {

        }

      
        public void ConfigureServices(IServiceCollection services)
        {

            var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            



        }
        public void Configure(IWebHostBuilder builder)
        {
            builder.UseStaticWebAssets();
            
            
            builder.ConfigureServices((context, services) =>
            {
                Configuration = context.Configuration;

                _hostingEnvironment = context.HostingEnvironment;

                ConfigureServices(services);

                services.AddMvc()                
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });

                //var viewsService = services.AddControllersWithViews(
                //    mvcOptions => mvcOptions.EnableEndpointRouting = false
                //    ).ConfigureApiBehaviorOptions(options =>
                //    {
                //                        //options.SuppressConsumesConstraintForFormFileParameters = true;
                //                        //options.SuppressInferBindingSourcesForParameters = true;
                //                        //options.SuppressModelStateInvalidFilter = true;
                //                        //options.SuppressMapClientErrors = true;
                //                        //options.ClientErrorMapping[StatusCodes.Status500InternalServerError].Link = "https://httpstatuses.com/500";
                //                        //options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
                //                    }).AddJsonOptions(options =>
                //                    {

                //                        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

                //                    });

            });

            //            builder.ConfigureServices((context, services) => {

            //                Configuration = context.Configuration;
            //                _hostingEnvironment = context.HostingEnvironment;

            //                services.AddDbContext<ApplicationContext>(options =>

            //                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), o =>
            //                o.UseNetTopologySuite().MigrationsHistoryTable("ef_migrations_history").UseNodaTime()));

            //                services.AddDbContext<IdentityContext>(options =>
            //                    options.UseNpgsql(Configuration.GetConnectionString("IdentityConnection"), o =>
            //                    o.MigrationsHistoryTable("ef_migrations_history")));




            //                if (Configuration.GetSection("Application:JWT").Exists())
            //                {
            //                    var securityKey = Configuration.GetSection("Application:JWT:SecurityKey").Value;
            //                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //                    //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //                    authentication.AddJwtBearer(options =>
            //                    {
            //                        options.TokenValidationParameters = new TokenValidationParameters
            //                        {
            //                            ValidateIssuer = true,
            //                            ValidateAudience = true,
            //                            ValidateLifetime = true,
            //                            ValidateIssuerSigningKey = true,
            //                            ValidIssuer = Configuration.GetSection("Application:Domain").Value,
            //                            ValidAudience = Configuration.GetSection("Application:Domain").Value,
            //                            IssuerSigningKey = key
            //                        };
            //                        //options.Audience = "precisionfarms.com";
            //                        //options.Authority = "http://localhost:5000/";
            //                    });

            //                }

            //                services.ConfigureApplicationCookie(options =>
            //                {
            //                    options.Cookie.HttpOnly = true;
            //                    //options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            //                    if(domain is not null)
            //                        options.Cookie.Name = domain;

            //                    options.LoginPath = new PathString("/Account/Login");
            //                    options.AccessDeniedPath = "/Denied";
            //                    options.SlidingExpiration = true;

            //                });

            //                services.Configure<CookiePolicyOptions>(options =>
            //                {
            //                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //                    options.CheckConsentNeeded = context => true;
            //                    options.MinimumSameSitePolicy = SameSiteMode.None;
            //                });

            //                services.Configure<AuthMessageSenderOptions>(Configuration);

            //                var physicalProvider = _hostingEnvironment.ContentRootFileProvider;

            //                var embeddedProvider = new EmbeddedFileProvider(Assembly.GetEntryAssembly());
            //                var compositeProvider = new CompositeFileProvider(physicalProvider, embeddedProvider);

            //                // choose one provider to use for the app and register it
            //                //services.AddSingleton<IFileProvider>(physicalProvider);
            //                //services.AddSingleton<IFileProvider>(embeddedProvider);
            //                services.AddSingleton<IFileProvider>(compositeProvider);

            //                //var compositeProvider =
            //                //   new CompositeFileProvider(physicalProvider, manifestEmbeddedProvider);

            //                services.AddSingleton(physicalProvider);

            //                services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            //                //services.AddProblemDetails();

            //                services.AddMvc().AddJsonOptions(opt =>
            //                {
            //                    //opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //                    //opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //                });

            //                //services.AddApiVersioning(options =>
            //                //{
            //                //    options.AssumeDefaultVersionWhenUnspecified = true;
            //                //    options.DefaultApiVersion = new ApiVersion(1, 0);
            //                //});

            //                services.AddDistributedMemoryCache();

            //                services.AddSession(options =>
            //                {
            //                    // Set a short timeout for easy testing.
            //                    options.IdleTimeout = TimeSpan.FromMinutes(20);
            //                    options.Cookie.HttpOnly = true;
            //                    options.Cookie.IsEssential = true;
            //                });

            //                var razorPages = services.AddRazorPages(options =>
            //                {
            //                    options.Conventions.Add(
            //                        new PageRouteTransformerConvention(
            //                            new SlugifyParameterTransformer()));

            //                    //options.Conventions.AddPageRoute("/Areas/Events/Pages/Details", "/Events/{id?}/{title?}");

            //                });

            //                services.AddCors(options =>
            //                {
            //                    //options.AddPolicy(name: "AllowedHosts",
            //                    //    builder =>
            //                    //    {
            //                    //        //builder.WithOrigins("https://www.poorboyfarms.com",
            //                    //        //    "https://localhost:44325",
            //                    //        //    "http://localhost:3000");

            //                    //        //builder.AllowAnyHeader();
            //                    //    });
            //                });

            //                services.AddServerSideBlazor();

            //                var viewsService = services.AddControllersWithViews(
            //                    mvcOptions => mvcOptions.EnableEndpointRouting = false
            //                    ).ConfigureApiBehaviorOptions(options =>
            //                    {
            //                        //options.SuppressConsumesConstraintForFormFileParameters = true;
            //                        //options.SuppressInferBindingSourcesForParameters = true;
            //                        //options.SuppressModelStateInvalidFilter = true;
            //                        //options.SuppressMapClientErrors = true;
            //                        //options.ClientErrorMapping[StatusCodes.Status500InternalServerError].Link = "https://httpstatuses.com/500";
            //                        //options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
            //                    }).AddJsonOptions(options =>
            //                    {

            //                        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            //                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            //                    }); ;

            //                //services.AddAutoMapper(typeof(Startup));

            //                services.AddHttpContextAccessor();

            //                services.AddTransient<IEmailSender, EmailSender>();
            //                services.AddTransient<GISHelper>();

            //                services.Configure<StripeSettings>(Configuration.GetSection("Application:Stripe"));

            //                services.Configure<ApplicationSettings>(Configuration.GetSection("Application"));
            //                services.Configure<ApiKeys>(Configuration.GetSection("Application:APIKeys"));

            //                var _applicationSettings = new ApplicationSettings();

            //                var paypal = Configuration.GetSection("Application:PayPal");

            //                if (paypal.Exists())
            //                {
            //                    services.AddScoped<PayPalService>();
            //                }

            //                Configuration.GetSection("Application").Bind(_applicationSettings);

            //                //services.Configure<RECaptchaKeys>(Configuration.GetSection("RECaptchaKeys"));

            //                services.AddSingleton(new HttpClient());

            //                services.AddSingleton(_applicationSettings);


            //                services.AddScoped<Klyk.Services.StripeService>();
            //                services.AddScoped<CartService>();
            //                services.AddScoped<ApplicationService>();
            //                services.AddScoped<Klyk.Services.AccountService>();

            //                services.AddSingleton<LocationsService>();

            //                services.AddScoped<TokenService>();

            //                //var zohoCRM = Configuration.GetSection("ZohoCRM");

            //                //if (zohoCRM.Exists())
            //                //{
            //                //    services.Configure<ZohoCRMSettings>(zohoCRM);
            //                //    services.AddSingleton<ZohoCRM>();
            //                //}

            //                services.AddScoped<SMSHelper>();

            //                services.AddSingleton<GeocodeService>();

            //                services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            //                {
            //                    var filesPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Views", "Shared", "Layouts");
            //                    var libraryPath = Path.GetFullPath(filesPath);
            //                    if (!Directory.Exists(libraryPath))
            //                        Directory.CreateDirectory(libraryPath);

            //                    options.FileProviders.Add(new PhysicalFileProvider(libraryPath));
            //                });

            //                if (_hostingEnvironment.IsDevelopment())
            //                {
            //#if DEBUG
            //                    services.AddSassCompiler();
            //#endif

            //                    razorPages.AddRazorRuntimeCompilation();

            //                    services.AddDatabaseDeveloperPageExceptionFilter();

            //                    services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            //                    {

            //                        var klykPath = Path.GetFullPath(Path.Combine(_hostingEnvironment.ContentRootPath, "..", "..", "Klyk", "Klyk"));

            //                        if (Directory.Exists(klykPath))
            //                            options.FileProviders.Add(new PhysicalFileProvider(klykPath));

            //                    });

            //                }

            //                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            //            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env, 
            IHostApplicationLifetime applicationLifetime,
            ILogger<Startup> logger)
        {

            //applicationLifetime.ApplicationStopping.Register(OnShutdown);

            var domainName = "www.mountainmap.com";

            //if (env.IsStaging()) {            
            //    domainName = "staging.mountainmap.com";
            //}

           

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
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

                        //var problem = new ProblemDetails()
                        //{
                        //    Status = context.Response.StatusCode,
                        //};

                        //var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        //string exceptionMessage = null;

                        //var error = exceptionHandlerPathFeature?.Error;
                        //if (error != null)
                        //{

                        //    var message = error.Message;

                        //    exceptionMessage += message;

                        //    if (error.InnerException != null)
                        //    {
                        //        exceptionMessage += "Inner Exception: " + error.InnerException.Message;
                        //    }

                        //    exceptionMessage += "Inner Exception: " + error.StackTrace;

                        //    var domain = context.Request.Host.Value.ToString();

                        //    exceptionMessage += "<br><br>";
                        //    exceptionMessage += "Path: " + exceptionHandlerPathFeature.Path;

                        //    problem.Detail = exceptionMessage;

                        //    //await _emailSender.SendEmailAsync("support@klyk.io", domain + " Error", ExceptionMessage);

                        //}


                        //await context.Response.WriteAsJsonAsync(problem);
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

                
            });

        }


    }


}
