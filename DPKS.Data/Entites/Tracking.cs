using DPKS.Data.Entites;
using System;
namespace DPKS.Data.Entities
{
    public class Tracking
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string ErorMessage { get; set; }
        public bool IsError { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}