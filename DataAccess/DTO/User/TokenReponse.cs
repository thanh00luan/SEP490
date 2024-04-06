namespace DataAccess.DTO.User
{
    public class TokenReponse
    {
        public UserDTO User { get; set; }
        public string AccessToken { get; set; }
    }
}