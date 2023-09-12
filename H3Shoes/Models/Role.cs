namespace H3Shoes.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public List<Permission> Permissions  { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
        public string Description { get; set; }
    }
}
