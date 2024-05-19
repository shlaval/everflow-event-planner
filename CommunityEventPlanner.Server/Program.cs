using CommunityEventPlanner.Shared;
using CommunityEventPlanner.Server.Logic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
//builder.Services
//    .AddAuthentication()
//    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthorizationBuilder();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseInMemoryDatabase("AppDb"));

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddTransient<ICommunityEventService, CommunityEventService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(policy =>
{
    policy.AddPolicy("debugCorsPolicy",
        builder => builder.WithOrigins("https://localhost:7011")
         .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowCredentials());
});

var app = builder.Build();

app.MapIdentityApi<ApplicationUser>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("debugCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
