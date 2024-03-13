public class UpdatedUserDTO
{
    public int IdType { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public DateTime Age { get; set; }
    public byte[] Img { get; set; } // Cambiado a byte[] para almacenar la imagen como bytes
}
