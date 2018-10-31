﻿using System;
using JetBrains.Annotations;

namespace Missio.Users
{
    public class CreateUserDTO : IEquatable<CreateUserDTO>
    {
        public string UserName { get; }
        public string Password { get; }
        public string Email { get; }

        [UsedImplicitly]
        public CreateUserDTO()
        {
        }

        public CreateUserDTO(string userName, string password, string email)
        {
            UserName = userName;
            Password = password;
            Email = email;
        }

        /// <inheritdoc />
        public bool Equals(CreateUserDTO other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(UserName, other.UserName) && string.Equals(Password, other.Password) && string.Equals(Email, other.Email);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CreateUserDTO) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = UserName.GetHashCode();
                hashCode = (hashCode * 397) ^ Password.GetHashCode();
                hashCode = (hashCode * 397) ^ Email.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(CreateUserDTO left, CreateUserDTO right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CreateUserDTO left, CreateUserDTO right)
        {
            return !Equals(left, right);
        }
    }
}