namespace UserService.DTOs
{
    public record RoleCreateUpdateDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
