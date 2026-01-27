using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MocnyDom.Domain.Entities
{
    public class Building
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Floor> Floors { get; set; }
        public ICollection<BuildingManager> Managers { get; set; }

    }
}
