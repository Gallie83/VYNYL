public class AuthenticatedControllerBase : ControllerBase
{
    protected readonly ApplicationDbContext _context;

    public AuthenticatedControllerBase(ApplicationDbContext context)
    {
        _context = context;
    }

    protected async Task<User?> GetAuthenticatedUserAsync()
    {
        var cognitoId = AuthHelper.GetCognitoIdFromClaims(User);

        if(string.IsNullOrEmpty(cognitoId))
        {
            return null;
        }

        return await _context.Users
            .FirstOrDefaultAsync(u => u.CognitoId == cognitoId);
    }
}