using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNet.OData.Extensions;

using Hangfire;
using Hangfire.SqlServer;

using Zircon.Core.Authorization;
using Zircon.Core.Authorization.SystemProvider;
using Zircon.Core.DB;
using Zircon.Core.HttpContextHelper;
using Zircon.Core.Log;

using AutoMapper;
using AutoMapperOdata;

using CRM.Data.Database;
using CRM.Data.NumberProvider;
using CRM.Server.Middlewares;
using CRM.Operation.Jobs;
using CRM.Operation.JsonConverters;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Models;
using Newtonsoft.Json;
using Zircon.Core.OperationModel;


namespace CRM.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }  

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            DBAuthorization.Authorize();
            services.AddDbContext<CRMDbContext>(options =>
            options.UseLazyLoadingProxies()
            .UseSqlServer(DBConnectionManager.ConnectionString), ServiceLifetime.Transient);

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
            {
                //options.Cookie.HttpOnly = true;
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.LoginPath = "/auth/Login";
                options.LogoutPath = "/auth/Logout";
                options.ExpireTimeSpan = System.TimeSpan.FromDays(5);
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtIssuer"],
                    ValidAudience = Configuration["JwtAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            //This region is used for session
            #region Session 

            services.AddDistributedMemoryCache();
            services.AddSession(options => 
            { 
                options.IdleTimeout = TimeSpan.FromDays(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddHttpContextAccessor();
            #endregion

            services.AddScoped<IApplicationProvider, CRMApplicationProvider>();
            services.AddScoped<ITechnicalLogWriter, TechnicalLogWriter>();

          

            #region hangfire

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(DBConnectionManager.ConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();


            #endregion

            #region odata
            services.AddOData();
            services.AddAutoMapper(typeof(AutoMapperProfile).GetTypeInfo().Assembly);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
              app.UseHsts(); // testde sil
            }

            app.UseHttpsRedirection(); // testde sil
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseHangfireDashboard();

            #region Session 
            //This region is used for session
            app.UseSession(options: new SessionOptions()
            {
                IdleTimeout = TimeSpan.FromDays(5)
            });
            ContextHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            #endregion

            app.UseRequestResponseLogging();

           

            app.UseRouting();
           
            app.UseAuthentication();
            
            app.UseAuthorization();

         

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");

                endpoints.EnableDependencyInjection();
                endpoints.Expand().Select().Filter().OrderBy().Count().MaxTop(200);
                endpoints.MapODataRoute("api", "api", GetEdmModel());
            });

            CommonUserContextManager.SetSystem(service.GetService<IApplicationProvider>());
            TechnicalLog.SetLogger(service.GetService<ITechnicalLogWriter>());


            app.UseHangfireDashboard();


            app.UseHangfireServer(new BackgroundJobServerOptions
            {

                SchedulePollingInterval = TimeSpan.FromSeconds(30),

                WorkerCount = 1//Environment.ProcessorCount * 5
            });

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 25 });




            RecurringJob.AddOrUpdate<JobManager>(recurringJobId: "JobManagerJobManager",
                    methodCall: operation => operation.Run("client"), "*/10 * * * * *", timeZone: TimeZoneInfo.Local);
        }


        private IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<ClientContract>("ClientContract").EntityType.HasKey(x => x.INN);
            builder.EntitySet<DocumentContract>("Document").EntityType.HasKey(x => x.Id);
            builder.EntitySet<ContactInfoContract>("Contact").EntityType.HasKey(x => x.Id);
            builder.EntitySet<CommentContract>("Comment").EntityType.HasKey(x => x.Id);
            builder.EntitySet<RelationContract>("Relation").EntityType.HasKey(x => x.Id);
            builder.EntitySet<TagContract>("Tag").EntityType.HasKey(x => x.Id);
            builder.EntitySet<AddressContract>("Address").EntityType.HasKey(x => x.Id);
            builder.EntitySet<DealListItem>("Deal").EntityType.HasKey(x => x.DealGuid);
            builder.EntitySet<AssetContract>("Asset").EntityType.HasKey(x => x.Id);
            builder.EntitySet<LeadObjectModel>("LeadObject").EntityType.HasKey(x => x.Id);

            var contactList = builder.EntitySet<ContactListItemModel>("PhysicalPerson");
            contactList.EntityType.DerivesFrom<ClientContract>();

            var birthdayContactList = builder.EntitySet<ContactListItemModel>("BirthdayPersons");
            birthdayContactList.EntityType.DerivesFrom<ClientContract>();

            var companyList = builder.EntitySet<CompanyListItemModel>("Company");
            companyList.EntityType.DerivesFrom<ClientContract>();

            var clientrelationList = builder.EntitySet<ClientRelationModel>("ClientRelation");
            clientrelationList.EntityType.DerivesFrom<ClientContract>();

            var physicalpersonList = builder.EntitySet<ContactListItemModel>("SearchPhysicalPerson");
            physicalpersonList.EntityType.DerivesFrom<ClientContract>();

            var companySearchList = builder.EntitySet<CompanyListItemModel>("SearchCompany");
            companySearchList.EntityType.DerivesFrom<ClientContract>();

            var assetSearchList = builder.EntitySet<CompanyListItemModel>("SearchAsset");
            assetSearchList.EntityType.DerivesFrom<ClientContract>();


            var clientFunction = builder.Function("GetClientContractByDocNumber");
            builder.EntitySet<ContactListItemModel>("ContactListItemModel");
            clientFunction.Parameter<string>("DocNumber");
            clientFunction.ReturnsCollectionFromEntitySet<ContactListItemModel>("ContactListItemModel");

            var GetclientByTinFunction = builder.Function("GetCompanyContractByDocNumber");
            builder.EntitySet<ContactListItemModel>("ContactListItemModel");
            GetclientByTinFunction.Parameter<string>("DocNumber");
            GetclientByTinFunction.ReturnsCollectionFromEntitySet<ContactListItemModel>("ContactListItemModel");

            var clientContractFunction = builder.Function("GetClientContractByContactInfo");
            builder.EntitySet<ContactListItemModel>("ContactListItemModel");
            clientContractFunction.Parameter<string>("contactsinfoValue");
            clientContractFunction.ReturnsCollectionFromEntitySet<ContactListItemModel>("ContactListItemModel");


            var clientAssetFunction = builder.Function("findClientByAsset");
            builder.EntitySet<ContactListItemModel>("ContactListItemModel");
            clientAssetFunction.Parameter<string>("AssetInfo");
            clientAssetFunction.ReturnsCollectionFromEntitySet<ContactListItemModel>("ContactListItemModel");



            return builder.GetEdmModel();
        }
    }
}
