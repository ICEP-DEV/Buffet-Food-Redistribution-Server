﻿using System.ComponentModel.DataAnnotations;

namespace FoodShareNetwork.Models.DTO
{
    public class UserLoginRequest
    {
        
        public string Email { get; set; } = string.Empty;


        public string Password { get; set; } = string.Empty;
    }
}
