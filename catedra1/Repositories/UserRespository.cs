using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    // Obtener todos los usuarios con soporte para ordenación y filtrado por género
    public async Task<List<User>> GetAllUsersAsync(string sort, string gender)
    {
        var query = _context.Users.AsQueryable();

        // Filtrar por género
        if (!string.IsNullOrWhiteSpace(gender))
        {
            query = query.Where(u => u.Genero == gender);
        }

        // Ordenar por nombre
        if (sort == "asc")
        {
            query = query.OrderBy(u => u.Nombre);
        }
        else if (sort == "desc")
        {
            query = query.OrderByDescending(u => u.Nombre);
        }

        return await query.ToListAsync();
    }

    // Obtener un usuario por ID
    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    // Crear un nuevo usuario
    public async Task CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    // Actualizar un usuario existente
    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    // Eliminar un usuario por ID
    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
