using BlazorWebApp.Client.Pages;
using BlazorWebApp.Components;
using Data;
using Data.Models.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents()
       .AddInteractiveWebAssemblyComponents();

builder.Services.AddOptions<BlogApiJsonDirectAccessSettings>()
       .Configure(options => 
       { 
           options.DataPath = @"../../Data/"; 
           options.PostsDirectory = "Posts"; 
           options.CategoriesDirectory = "Categories"; 
           options.TagsDirectory = "Tags"; 
           options.CommentsDirectory = "Comments"; 
       });

builder.Services.AddScoped<IBlogApi, BlogApiJsonDirectAccess>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode()
   .AddInteractiveWebAssemblyRenderMode()
   .AddAdditionalAssemblies(typeof(BlazorWebApp.Client._Imports).Assembly)
   .AddAdditionalAssemblies(typeof(SharedComponents.Pages.Home).Assembly);

app.Run();