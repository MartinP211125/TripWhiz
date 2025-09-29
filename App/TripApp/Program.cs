using Core.ApplicationOptions;
using Core.Interfaces.Repositories;
using Infrastructure.Jobs;
using Infrastructure.Services.Groq;
using Infrastructure.Services.RegisterServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Service.Services.RegisterServices;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.Configure<ConnectionOptions>(
    builder.Configuration.GetSection(ConnectionOptions.Key));

builder.Services.RegisterInfrastructure(builder.Configuration);
builder.Services.RegisterServiceServices(builder.Configuration);

builder.Services.AddQuartz(configure =>
{
    configure.UseMicrosoftDependencyInjectionJobFactory();

    var updateOffersKey = new JobKey(nameof(UpdateOffersJob));
    configure.AddJob<UpdateOffersJob>(opts => opts.WithIdentity(updateOffersKey));
    configure.AddTrigger(trigger => trigger
        .ForJob(updateOffersKey)
        .WithIdentity($"{nameof(UpdateOffersJob)}-trigger")
        .WithDailyTimeIntervalSchedule(x => x
            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(5, 55))
            .OnEveryDay()
        )
    );

    var deleteBookingsKey = new JobKey(nameof(DeletePassedBookingRecordsJob));
    configure.AddJob<DeletePassedBookingRecordsJob>(opts => opts.WithIdentity(deleteBookingsKey));
    configure.AddTrigger(trigger => trigger
        .ForJob(deleteBookingsKey)
        .WithIdentity($"{nameof(DeletePassedBookingRecordsJob)}-trigger")
        .WithDailyTimeIntervalSchedule(x => x
            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(3, 0))
            .OnEveryDay()
        )
    );

    var getMissingAccomodationsKey = new JobKey(nameof(GetMissingAccomodationsJob));
    configure.AddJob<GetMissingAccomodationsJob>(opts => opts.WithIdentity(getMissingAccomodationsKey).StoreDurably());

    var updateRecomendedKey = new JobKey(nameof(UpdateRecomendationsForUserJob));
    configure.AddJob<UpdateRecomendationsForUserJob>(opts => opts.WithIdentity(updateRecomendedKey).StoreDurably());
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Services.AddSingleton(provider =>
{
    var schedulerFactory = provider.GetRequiredService<ISchedulerFactory>();
    return schedulerFactory.GetScheduler().Result;
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("PexelsClient", client =>
{
    client.BaseAddress = new Uri("https://api.pexels.com/v1/");
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue(builder.Configuration["Pexels:ApiKey"]);
});

builder.Services.AddScoped<IGroqService>(provider =>
{
    var groqClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    groqClient.BaseAddress = new Uri("https://api.groq.com/");
    var groqApiKey = builder.Configuration["Groq:ApiKey"];
    groqClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", groqApiKey);

    var pexelsClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient("PexelsClient");
    var config = provider.GetRequiredService<IConfiguration>();
    var pexelsApiKey = config["Pexels:ApiKey"];

    return new GroqService(
        groqClient,
        provider.GetRequiredService<IAccomodationRepository>(),
        provider.GetRequiredService<IOfferRepository>(),
        provider.GetRequiredService<ITransportationRepository>(),
        pexelsClient,
        pexelsApiKey
    );
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
