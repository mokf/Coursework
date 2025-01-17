﻿using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string? KnownAs { get; set; }

    [Required]
    public string? Gender { get; set; }
    
    [Required]
    [StringLength(30, MinimumLength = 4)]
    public string Password { get; set; }
}