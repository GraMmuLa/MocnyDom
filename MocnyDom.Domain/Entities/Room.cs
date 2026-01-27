using System;
using System.Collections.Generic;
using System.Text;

namespace MocnyDom.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; }

        public int FloorId { get; set; }
        public Floor Floor { get; set; }
    }
}
