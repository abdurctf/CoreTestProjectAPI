using Core.DepositBLL;
using Core.TestProjectBLL.BLLContracts;
using Core.TestProjectBLL.BLLRepositories;
using DBContext;
using DBAccessor.Contracts;
using DBAccessor.Repositories;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Authorization.AuthDBAccess.NFT.Contracts;
using Authorization.AuthDBAccess.NFT.Repositories;

namespace CoreDepositAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration config)
        {
            // var Gateway = config.GetValue<string>("APIGateway");
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()//WithOrigins(Gateway)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
        }
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }
        public static void ConfigureLoggerService(this IServiceCollection services)//For logger service
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
        public static void ConfigureOracleContext(this IServiceCollection services, IConfiguration config)//for passing database credentials
        {
            var connectionString = config.GetConnectionString("DatabaseTest");
            var oracleVersion = config.GetValue<string>("Oracleversion");

            if (!string.IsNullOrEmpty(oracleVersion))
            {
                services.AddDbContext<DatabaseContext>(o => o.UseOracle(connectionString, options => options
                    .UseOracleSQLCompatibility(oracleVersion)));
            }
            else
            {
                services.AddDbContext<DatabaseContext>(o => o.UseOracle(connectionString));
            }

            var readConnString = new DatabaseContextReadOnly(connectionString);
            services.AddTransient<DatabaseContextReadOnly>(provider => readConnString);
        }      
        public static void ConfigureAutoMapper(this IServiceCollection services) //tagging automapper profile
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }
        public static void ConfigureRepositoryWrapper(this IServiceCollection services) //calling repositories
        {
            //services.AddTransient<ICommonBLL, CommonBLL>();
            //services.AddTransient<ICommonService, CommonService>();

            services.AddTransient<ICustomer_ProfileBLL, Customer_ProfileBLL>();
            services.AddTransient<ICustomer_ProfileService, Customer_ProfileService>();

            services.AddTransient<ICustomer_AddressBLL, Customer_AddressBLL>();
            services.AddTransient<ICustomer_AddressService, Customer_AddressService>();
            
            services.AddTransient<ICustomer_IntroducerBLL, Customer_IntroducerBLL>();
            services.AddTransient<ICustomer_IntroducerService, Customer_IntroducerService>();

            services.AddTransient<ICoreAuthorizeSaveLogService, CoreAuthorizeSaveLogService>();
            services.AddTransient<ICoreAuthorizeGenerateLogService, CoreAuthorizeGenerateLogService>();




        }
    }
}
