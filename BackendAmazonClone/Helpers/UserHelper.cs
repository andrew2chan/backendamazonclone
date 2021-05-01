using BackendAmazonClone.Models;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendAmazonClone.Repositories;
using System.Collections;
using Newtonsoft.Json;
using BackendAmazonClone.DTOs;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BackendAmazonClone.Helpers
{
    public class UserHelper
    {
        public UserHelper() 
        {
        }

        //@return basic validation for email registration
        public bool ValidateEmail(Users user)
        {
            string email = user.Email;
            Regex rx = new Regex(@".+\@.+\..+");
            MatchCollection mc = rx.Matches(email);

            if (mc.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //@return true means there is an empty field in registration
        public bool EmptyStrings(Users user)
        {
            if(user.Address == "" || user.Email == "" || user.Name == "")
            {
                return true;
            }
            return false;
        }

        //@return true means email exists in db already
        public bool DuplicateEmail(Users user, IEnumerator email)
        {
            var emailToCheck = user.Email;

            while (email.MoveNext())
            {
                dynamic currentEmail = email.Current;
                if (emailToCheck == currentEmail.field)
                {
                    return true;
                }
            }

            return false;
        }

        //@return true means missing login field
        public bool MissingLoginField(string email, string password)
        {
            if (email == "" || password == "")
            {
                return true;
            }

            return false;
        }

        //@return true means that no emails matched
        public int CheckExistingAccount(string emailToCheck, string passwordToCheck, IEnumerator acc)
        {
            while(acc.MoveNext())
            {
                dynamic currentAcc = acc.Current;

                string hashPasswordToTest = HashPassword(passwordToCheck, currentAcc.salt);

                if(emailToCheck == currentAcc.email && hashPasswordToTest == currentAcc.hashPassword)
                {
                    return (int)currentAcc.id;
                }
            }

            return -1;
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        public string HashPassword(string passwordToHash, byte[] saltToUse)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordToHash,
                salt: saltToUse,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return hashed;
        }

        public UserDTO ConstructUserDTO(Users user)
        {
            UserDTO dto = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address
            };

            return dto;
        }
    }
}
