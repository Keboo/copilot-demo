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
    },
    // Sports activities
    ["Soccer Team"] = new Activity
    {
        Description = "Practice teamwork and compete in interschool soccer matches",
        Schedule = "Tuesdays and Thursdays, 4:00 PM - 5:30 PM",
        MaxParticipants = 22,
        Participants = new List<string> { "liam@mergington.edu", "noah@mergington.edu" }
    },
    ["Basketball Club"] = new Activity
    {
        Description = "Train skills and scrimmage to prepare for tournaments",
        Schedule = "Wednesdays, 3:30 PM - 5:00 PM",
        MaxParticipants = 15,
        Participants = new List<string> { "ava@mergington.edu", "isabella@mergington.edu" }
    },
    // Artistic activities
    ["Art Workshop"] = new Activity
    {
        Description = "Explore painting, drawing, and mixed media techniques",
        Schedule = "Mondays, 3:30 PM - 5:00 PM",
        MaxParticipants = 18,
        Participants = new List<string> { "mia@mergington.edu", "amelia@mergington.edu" }
    },
    ["School Choir"] = new Activity
    {
        Description = "Develop vocal skills and perform at school events",
        Schedule = "Fridays, 2:30 PM - 4:00 PM",
        MaxParticipants = 25,
        Participants = new List<string> { "harper@mergington.edu", "evelyn@mergington.edu" }
    },
    // Intellectual activities
    ["Math Club"] = new Activity
    {
        Description = "Problem-solving sessions and math competition prep",
        Schedule = "Thursdays, 3:30 PM - 4:30 PM",
        MaxParticipants = 20,
        Participants = new List<string> { "benjamin@mergington.edu", "lucas@mergington.edu" }
    },
    ["Debate Team"] = new Activity
    {
        Description = "Learn argumentation and practice formal debates",
        Schedule = "Wednesdays, 4:00 PM - 5:30 PM",
        MaxParticipants = 16,
        Participants = new List<string> { "charlotte@mergington.edu", "ella@mergington.edu" }
    }
};

// API Endpoints
app.MapGet("/api/activities", () => Results.Ok(activities))
    .WithName("GetActivities");

app.MapPost("/api/activities/{activityName}/signup", (string activityName, SignupRequest request) =>
{
    if (!activities.ContainsKey(activityName))
    {
        return Results.NotFound(new { detail = "Activity not found" });
    }

    var activity = activities[activityName];

    // Check if student is already registered
    if (activity.Participants.Contains(request.Email))
    {
        return Results.BadRequest(new { detail = $"{request.Email} is already registered for {activityName}" });
    }

    // Check capacity
    if (activity.Participants.Count >= activity.MaxParticipants)
    {
        return Results.BadRequest(new { detail = "Activity is full" });
    }

    activity.Participants.Add(request.Email);
    return Results.Ok(new { message = $"Signed up {request.Email} for {activityName}" });
})
    .WithName("SignupForActivity");

// SPA fallback - serve index.html for client-side routes
app.MapFallbackToFile("index.html");

app.Run();
