﻿using Domain.Entities;
using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class BrandDto :Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Sponsor { get; set; } = string.Empty;

        public Guid GatewayId { get; set; }
        public virtual GatewayDto Gateway { get; set; } = null!;
    }
}
