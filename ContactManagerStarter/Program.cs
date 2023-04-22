using ContactManager.Data;
using ContactManager.Hubs;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddDbContext<ApplicationContext>(options =>
                options
                    .UseSqlServer(builder.Configuration.GetConnectionString("ContactDb"),
                        opts => opts.CommandTimeout(600)));

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dataContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ContactHub>("/contacthub");

app.Run();


/*
 
Requirements


Any database changes must be done using a migration with Entity Framework

{done}
Add a warning if an email or mail address was typed, but not added to the list, in New/Edit Contact when saving

{done}
Add a new feature: The ability to set a primary Email from the list in New/Edit Contact to display in the Contact Grid


Update the grid to show the primary email address, if none is set use the first one


Bonus
Add ELMAH for unhandled exception logging
Add logging to file or DB, using the ILogger interface
Add error handling to the controller methods
 */