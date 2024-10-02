public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync(string sort, string gender)
    {
        // Implementar filtrado y ordenación aquí...
    }

    // Implementar los demás métodos...
}
