using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend_.DataBase.ControllerDB;
using backend_.DataBase.UserDB;
using backend_.Connection;



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
            builder.Services.AddCors(option =>
            {
                option.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }

        private static void Config(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ControllerDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<ControllerGroupDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<ControllerOutputDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<ControllerOutputRangeDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<ControllerOutputStateDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<ControllerOutputValueDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<ControllerQueryDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<m2mControllerGroupDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<m2mControllerOutputGroupDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<m2mControllerOutputGroupDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<UserDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddDbContext<GroupDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));

            builder.Services.AddSingleton<ConnectionController>();
            builder.Services.AddHostedService<ConnectionController>(
                services => services.GetService<ConnectionController>()
                );

           
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
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
                    };

                });


            var app = builder.Build();

            app.UseCors();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
            app.MapControllers();


            app.Run();
        }
    }
}



