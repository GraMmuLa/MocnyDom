using System;
using System.Collections.Generic;
using System.Text;

namespace MocnyDom.Domain.Entities
{
    public class Floor
    {
        public int Id { get; set; }
        public int Number { get; set; }

        public int BuildingId { get; set; }
        public Building Building { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
