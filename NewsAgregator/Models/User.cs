using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAgregator.Models;
using NewsAgregator.ViewModels;
using NewsAgregator.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AutoMapper;

namespace NewsAgregator.Models
{
    public enum Roles
    {
        User,
        Admin
    }
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Email { get; set; }
        [Required]
        public String Password { get; set; }
        public Roles Role { get; set; }
    }
}
