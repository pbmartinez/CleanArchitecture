﻿using Domain.Entities;
using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Peripheral : Entity
    {
        public string Vendor { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public Guid GatewayId { get; set; }

        public virtual Gateway Gateway { get; set; } = null!;
        public virtual Provider? Provider { get; set; } 
    }
}
