﻿using Domain.Entities;
using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ProviderDto:Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }= string.Empty;

        public virtual Guid PeripheralId { get; set; }
        public virtual PeripheralDto Peripheral { get; set; } = null!;
    }
}
