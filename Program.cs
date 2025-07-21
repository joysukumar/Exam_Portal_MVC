using Exam_Portal.Data;
using Exam_Portal.Interface;
using Exam_Portal.Models;
using Exam_Portal.QuestionService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Appdbcontext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Defaultconnection")));
builder.Services.AddIdentity<User, IdentityRole>(
    option =>
    {
        option.User.RequireUniqueEmail = true;
        option.Password.RequireNonAlphanumeric = false;
        option.Password.RequireUppercase = true;
        option.Password.RequireLowercase = true;
        option.Password.RequiredLength = 6;
    }
)
    .AddEntityFrameworkStores<Appdbcontext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Shared/AccessDenied";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRoles_AdminAccounts.SeedRolesAndAdminAsync(services);
}
app.Run();
