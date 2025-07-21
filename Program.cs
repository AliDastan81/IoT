using IoTAuth.Supabase;
using Microsoft.Extensions.Options;
//using Supabase;

var builder = WebApplication.CreateBuilder(args);

//var supabaseUrl = builder.Configuration["Supabase:Url"];
//var supabaseKey = builder.Configuration["Supabase:ApiKey"];

builder.Services.Configure<SupabaseOptions>(
    builder.Configuration.GetSection("Supabase"));

builder.Services.AddHttpClient<ISupabaseService, SupabaseService>();
builder.Services.AddHttpClient<SupabaseService>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<SupabaseOptions>>().Value;

    client.BaseAddress = new Uri(options.Url);
    client.DefaultRequestHeaders.Add("apikey", options.ApiKey);
});


//builder.Services.AddSingleton(_ =>
//{
//    var options = new SupabaseOptions
//    {
//        AutoRefreshToken = true,
//        AutoConnectRealtime = true
//    };
//    var client = new Client(supabaseUrl, supabaseKey, options);
//    client.InitializeAsync().Wait();
//    return client;
//});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

if (1 == 2)
{
    builder.Services.AddControllersWithViews()
        .AddRazorRuntimeCompilation();
}
else
{
    builder.Services.AddControllersWithViews();
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Dashboard}");

app.Run();