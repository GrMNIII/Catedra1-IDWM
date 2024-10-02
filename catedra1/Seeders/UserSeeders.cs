public static class UserSeeder
{
    public static void Seed(UserDbContext context)
    {
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User { RUT = "12345678-9", Nombre = "Kevin Araya", Correo = "kevin@gmail.com", Genero = "masculino", FechaNacimiento = new DateTime(2002, 10, 10) },
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
