﻿using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.PasswordCryptography;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSensorServerAPI.Utils
{
    public class UserAuthenticator
    {
        public User Authenticate(IEnumerable<User> users, LoginCredentials login)
        {
            User user = null;

            var requestant = users.FirstOrDefault(u => u.Login == login.Username);

            //no matching user, return 
            if (requestant == null)
                return null;

            user = requestant;

            var cryptoService = new PasswordCryptoSerivce();

            //we found matching login, check password
            if (cryptoService.IsPasswordMatching(requestant.Password, login.Password))
            {
                user.IsSuccessfullyAuthenticated = true;
                user.LastValidLogin = DateTime.Now;
            }
            else
            {
                user.IsSuccessfullyAuthenticated = false;
                user.LastInvalidLogin = DateTime.Now;
            }

            return user;
        }
    }
}
