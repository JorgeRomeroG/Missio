﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Missio.Registration;
using Missio.Users;

namespace Missio.LogIn
{
    public class WebUserRepository : IUserRepository
    {
        private readonly HttpClient _httpClient;

        public WebUserRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <inheritdoc />
        public async Task AttemptToRegisterUser(RegistrationDTO registrationDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", registrationDto);
            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new UserRegistrationException(new List<string> { response.ReasonPhrase });
        }

        public async Task<User> GetUserIfValid(string userName, string password)
        {
            var requestUri = $@"api/users/{userName}/{password}";
            var response = await _httpClient.GetAsync(requestUri);
            if(response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadAsAsync<User>();
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new LogInException(await response.Content.ReadAsAsync<string>());
            throw new HttpRequestException(response.StatusCode + " " + response.ReasonPhrase);
        }
    }
}