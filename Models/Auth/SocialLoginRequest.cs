using System.ComponentModel.DataAnnotations;

public class SocialLoginRequest
{
    [Required]
    public string IdToken { get; set; }
}