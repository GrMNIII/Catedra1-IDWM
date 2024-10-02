using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Crear un usuario
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(user.RUT) || 
            user.Nombre.Length < 3 || user.Nombre.Length > 100 ||
            !IsValidEmail(user.Correo) ||
            !IsValidGender(user.Genero) ||
            user.FechaNacimiento >= DateTime.Now)
        {
            return BadRequest("Validación fallida: datos incorrectos.");
        }

        // Comprobar unicidad de RUT
        var existingUser = await _userRepository.GetAllUsersAsync(null, null);
        if (existingUser.Any(u => u.RUT == user.RUT))
        {
            return Conflict("El RUT ya existe.");
        }

        await _userRepository.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
    }

    // Obtener todos los usuarios
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] string sort, [FromQuery] string gender)
    {
        if (!string.IsNullOrWhiteSpace(sort) && sort != "asc" && sort != "desc")
        {
            return BadRequest("Parámetro de ordenación inválido.");
        }

        if (!string.IsNullOrWhiteSpace(gender) && !IsValidGender(gender))
        {
            return BadRequest("Parámetro de género inválido.");
        }

        var users = await _userRepository.GetAllUsersAsync(sort, gender);
        return Ok(users);
    }

    // Editar un usuario
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        // Validaciones
        if (id <= 0 || user.Id != id ||
            string.IsNullOrWhiteSpace(user.RUT) ||
            user.Nombre.Length < 3 || user.Nombre.Length > 100 ||
            !IsValidEmail(user.Correo) ||
            !IsValidGender(user.Genero) ||
            user.FechaNacimiento >= DateTime.Now)
        {
            return BadRequest("Validación fallida: datos incorrectos.");
        }

        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        await _userRepository.UpdateUserAsync(user);
        return Ok(user);
    }

    // Eliminar un usuario
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (id <= 0)
        {
            return BadRequest("ID inválido.");
        }

        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        await _userRepository.DeleteUserAsync(id);
        return Ok("Usuario eliminado exitosamente.");
    }

    // Métodos de validación
    private bool IsValidEmail(string email)
    {
        // Implementar validación de correo electrónico (puedes usar una expresión regular)
        return !string.IsNullOrWhiteSpace(email) && email.Contains("@");
    }

    private bool IsValidGender(string gender)
    {
        var validGenders = new[] { "masculino", "femenino", "otro", "prefiero no decirlo" };
        return validGenders.Contains(gender);
    }
}
