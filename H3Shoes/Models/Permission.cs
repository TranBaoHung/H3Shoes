namespace H3Shoes.Models
{
    public class Permission
    {
        public int ID { get; set; }
        public string PermissionName { get; set; }
        public bool IsActive { get; set; }
        public int Category { get; set; }
    }
}
