using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend_.DataBase.ControllerDB;
using backend_.DataBase.UserDB;
using backend_.Connection;
using Microsoft.AspNetCore.CookiePolicy;


namespace backend_
{ 
    class Program
    {
        private static WebApplicationBuilder builder;
        private static void SetPolitical()
        {
            var strArr = new List<string>();
            //var tmp=builder.Configuration["APIController:2:name"];

            //требуется пройти циклом по всем допустимым значениям ${"APIController:"i":x"}

/*            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(builder.Configuration["APIController"])
            });*/
        }

        private static void Cors(WebApplicationBuilder builder)
        {
            builder.Services.AddCors();
/*            builder.Services.AddCors(option =>
            {
                option.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                     .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });*/
        }

        private static void Config(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ControllerDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<ControllerGroupDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<ControllerOutputDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<ControllerOutputRangeDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<ControllerOutputStateDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<ControllerOutputValueDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<m2mControllerOutputGroupDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<m2mControllerOutputGroupDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<UserDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<GroupDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));

            builder.Services.AddSingleton<ConnectionController>();
            builder.Services.AddHostedService<ConnectionController>(
                services => services.GetService<ConnectionController>()
                );
            builder.Services.AddSingleton<backend_.Connection.UserConnection.UserConnectionController>();

           
        }

        public static void Main(string[] args)
        {
            builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            Config(builder);
            Cors(builder);
            builder.Configuration.AddJsonFile("appsettings.Development.json");

            SetPolitical();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
                    };

                });



            var app = builder.Build();

            app.UseCors(x => x
                .WithOrigins("https://localhost:3000") // путь к нашему SPA клиенту
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());
           
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });


            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies["some.Text"];
                if (!string.IsNullOrEmpty(token))
                    context.Request.Headers.Add("Authorization", "Bearer " + token);

                await next();
            });
            app.UseAuthentication();
            app.UseAuthorization();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.UseHttpsRedirection();
            app.MapControllers();


            app.Run();
        }
    }
}



