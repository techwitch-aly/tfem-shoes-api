using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tfemshoes.Domain.Context;
using tfemshoes.Domain.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;

namespace tfemshoes.Domain.Service
{
    public class UserService : IUserService
    {
        private readonly TFemShoesContext _context;
        private readonly RandomNumberGenerator random;
        private readonly ILogger<UserService> _logger;

        public UserService(TFemShoesContext context, ILogger<UserService> logger)
        {
            _context = context;
            random = RandomNumberGenerator.Create();
            _logger = logger;
        }

        public async Task<User?> Authenticate(string username, string password)
        {
            _logger.LogInformation("Begin authentication for {Username}", username);

            // Lookup username and hashed password combo
            var user = await _context.Users.Where(u => u.Username == username).SingleAsync();

            if (user == null)
            {
                _logger.LogInformation("Failed to find user with username {Username}", username);
                // Return null if not found or not matched
                return null;
            }

            if (user.Password != Hash(password, user.Salt))
            {
                return null;
            }
            
            // Return the user object if found
            return user;
        }

        public void Register(string username, string password)
        {
            if (_context.Users.Any(x => x.Username == username))
            {
                throw new ArgumentException($"Username {username} is already in use");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password is required");
            }

            string salt = GenerateSalt();
            var user = new User();
            user.Username = username;
            user.Salt = salt;
            user.Password = Hash(password, salt);

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        private string GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            random.GetBytes(salt, 0, salt.Length);
            return Convert.ToBase64String(salt);
        }

        private string Hash(string password, string salt)
        {
            byte[] hashBytes = KeyDerivation.Pbkdf2(
                password,
                Convert.FromBase64String(salt),
                KeyDerivationPrf.HMACSHA256,
                100000,
                256 / 8);
            string hashed = Convert.ToBase64String(hashBytes);
            return hashed;
        }
    }
}
