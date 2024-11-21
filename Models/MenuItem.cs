using Microsoft.AspNetCore.Mvc;

namespace BT.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Link { get; set; }
    }
}
