using IkariDoTrainingBackend.Data;
using IkariDoTrainingBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace IkariDoTrainingBackend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User entity)
        {
            // Passwort-Hashing
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(entity.PasswordHash);

            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            // Beispiel-Implementierung
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<User> UpdateAsync(User entity)
        {
            // Optional: Nur wenn PasswordHash != null => neu hashen
            if (!string.IsNullOrEmpty(entity.PasswordHash))
            {
                entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(entity.PasswordHash);
            }

            var existing = await _context.Users.FindAsync(entity.Id);
            if (existing == null) return null;

            // Felder übernehmen
            existing.Name = entity.Name;
            existing.PasswordHash = entity.PasswordHash;
            // ggf. weitere Felder ...

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
