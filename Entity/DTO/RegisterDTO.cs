namespace Entity.DTO;

public class RegisterDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public RegisterDTO(string firstName, string lastName, string username, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Password = password;
    }
}