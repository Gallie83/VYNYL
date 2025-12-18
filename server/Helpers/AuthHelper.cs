using System.Security.Claims;

public static class AuthHelper
{
    // Extract Cognito user Id (sub claim) from JWT Token
    public static string? GetCognitoIdFromClaims(ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? user.FindFirst("sub")?.Value;
    }

    // Extract username from JWT Token
    public static string? GetUsernameFromClaims(ClaimsPrincipal user)
    {
        return user.FindFirst("cognito:username")?.Value;
    }

    // Extract email from JWT Token
    public static string? GetEmailFromClaims(ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Email)?.Value
            ?? user.FindFirst("email")?.Value;
    }
}