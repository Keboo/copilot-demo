using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MergingtonHighSchool.Models;
using Xunit;

namespace MergingtonHighSchool.Tests;

public class ActivitiesApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public ActivitiesApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task GetActivities_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/activities");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetActivities_ReturnsActivitiesDictionary()
    {
        // Act
        var response = await _client.GetAsync("/api/activities");
        var activities = await response.Content.ReadFromJsonAsync<Dictionary<string, Activity>>(_jsonOptions);

        // Assert
        Assert.NotNull(activities);
        Assert.NotEmpty(activities);
        Assert.Contains("Chess Club", activities.Keys);
        Assert.Contains("Programming Class", activities.Keys);
    }

    [Fact]
    public async Task GetActivities_ActivityHasCorrectProperties()
    {
        // Act
        var response = await _client.GetAsync("/api/activities");
        var activities = await response.Content.ReadFromJsonAsync<Dictionary<string, Activity>>(_jsonOptions);

        // Assert
        Assert.NotNull(activities);
        var chessClub = activities["Chess Club"];
        Assert.NotEmpty(chessClub.Description);
        Assert.NotEmpty(chessClub.Schedule);
        Assert.True(chessClub.MaxParticipants > 0);
        Assert.NotNull(chessClub.Participants);
    }

    [Fact]
    public async Task SignupForActivity_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var signupRequest = new SignupRequest
        {
            Email = "test@mergington.edu"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/activities/Math Club/signup", signupRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SignupForActivity_WithInvalidActivityName_ReturnsNotFound()
    {
        // Arrange
        var signupRequest = new SignupRequest
        {
            Email = "test@mergington.edu"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/activities/NonExistentActivity/signup", signupRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task SignupForActivity_WhenAlreadyRegistered_ReturnsBadRequest()
    {
        // Arrange - First, sign up a user
        var email = $"duplicate{Guid.NewGuid()}@mergington.edu";
        var signupRequest = new SignupRequest
        {
            Email = email
        };

        // Act - Sign up for the first time (should succeed)
        var firstSignup = await _client.PostAsJsonAsync("/api/activities/Chess Club/signup", signupRequest);
        firstSignup.EnsureSuccessStatusCode();

        // Act - Try to sign up again (should fail)
        var secondSignup = await _client.PostAsJsonAsync("/api/activities/Chess Club/signup", signupRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, secondSignup.StatusCode);
    }

    [Fact]
    public async Task SignupForActivity_ResponseContainsMessage()
    {
        // Arrange
        var email = $"newstudent{Guid.NewGuid()}@mergington.edu";
        var signupRequest = new SignupRequest
        {
            Email = email
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/activities/Art Workshop/signup", signupRequest);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Contains("Signed up", content);
        Assert.Contains(email, content);
    }

    [Fact]
    public async Task UnregisterFromActivity_WithValidEmail_ReturnsSuccess()
    {
        // Arrange - First sign up a user
        var email = $"unregister{Guid.NewGuid()}@mergington.edu";
        var signupRequest = new SignupRequest { Email = email };
        await _client.PostAsJsonAsync("/api/activities/Chess Club/signup", signupRequest);

        // Act
        var response = await _client.DeleteAsync($"/api/activities/Chess Club/unregister?email={email}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UnregisterFromActivity_WithInvalidActivityName_ReturnsNotFound()
    {
        // Arrange
        var email = "test@mergington.edu";

        // Act
        var response = await _client.DeleteAsync($"/api/activities/NonExistentActivity/unregister?email={email}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UnregisterFromActivity_WhenNotRegistered_ReturnsNotFound()
    {
        // Arrange
        var email = "notregistered@mergington.edu";

        // Act
        var response = await _client.DeleteAsync($"/api/activities/Chess Club/unregister?email={email}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UnregisterFromActivity_ResponseContainsMessage()
    {
        // Arrange - First sign up a user
        var email = $"messagetest{Guid.NewGuid()}@mergington.edu";
        var signupRequest = new SignupRequest { Email = email };
        await _client.PostAsJsonAsync("/api/activities/Chess Club/signup", signupRequest);

        // Act
        var response = await _client.DeleteAsync($"/api/activities/Chess Club/unregister?email={email}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Contains("Unregistered", content);
        Assert.Contains(email, content);
    }

    [Fact]
    public async Task SignupAndUnregister_WorkflowTest()
    {
        // Arrange
        var email = $"workflow{Guid.NewGuid()}@mergington.edu";
        var signupRequest = new SignupRequest
        {
            Email = email
        };

        // Act - Sign up
        var signupResponse = await _client.PostAsJsonAsync("/api/activities/Soccer Team/signup", signupRequest);
        signupResponse.EnsureSuccessStatusCode();

        // Verify user is registered
        var activitiesResponse = await _client.GetAsync("/api/activities");
        var activities = await activitiesResponse.Content.ReadFromJsonAsync<Dictionary<string, Activity>>(_jsonOptions);
        Assert.NotNull(activities);
        Assert.Contains(email, activities["Soccer Team"].Participants);

        // Act - Unregister
        var unregisterResponse = await _client.DeleteAsync($"/api/activities/Soccer Team/unregister?email={email}");

        // Assert
        unregisterResponse.EnsureSuccessStatusCode();
    }
}
