﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Provider : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Guid PeripheralId { get; set; }
        public virtual Peripheral Peripheral { get; set; }
    }
}
