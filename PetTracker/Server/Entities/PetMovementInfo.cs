using Microsoft.EntityFrameworkCore;

namespace PetTracker.Server.Entities
{
    [Keyless]
    public class PetMovementInfo
    {
        public int PetId { get; set; }
        public int MovementSeconds { get; set; }
    }
}
