using MergingtonHighSchool.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure JSON serialization to use camelCase
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

// In-memory activity database
var activities = new Dictionary<string, Activity>
{
    ["Chess Club"] = new Activity
    {
        Description = "Learn strategies and compete in chess tournaments",
        Schedule = "Fridays, 3:30 PM - 5:00 PM",
        MaxParticipants = 12,
        Participants = new List<string> { "michael@mergington.edu", "daniel@mergington.edu" }
    },
    ["Programming Class"] = new Activity
    {
        Description = "Learn programming fundamentals and build software projects",
        Schedule = "Tuesdays and Thursdays, 3:30 PM - 4:30 PM",
        MaxParticipants = 20,
        Participants = new List<string> { "emma@mergington.edu", "sophia@mergington.edu" }
    },
    ["Gym Class"] = new Activity
    {
        Description = "Physical education and sports activities",
        Schedule = "Mondays, Wednesdays, Fridays, 2:00 PM - 3:00 PM",
        MaxParticipants = 30,
        Participants = new List<string> { "john@mergington.edu", "olivia@mergington.edu" }
    }
};

// API Endpoints
app.MapGet("/api/activities", () => Results.Ok(activities))
    .WithName("GetActivities");

app.MapPost("/api/activities/{activityName}/signup", (string activityName, SignupRequest request) =>
{
    // Validate activity exists
    if (!activities.ContainsKey(activityName))
    {
        return Results.NotFound(new { detail = "Activity not found" });
    }

    var activity = activities[activityName];

    // Add student
    activity.Participants.Add(request.Email);
    return Results.Ok(new { message = $"Signed up {request.Email} for {activityName}" });
})
    .WithName("SignupForActivity");

// SPA fallback - serve index.html for client-side routes
app.MapFallbackToFile("index.html");

app.Run();
