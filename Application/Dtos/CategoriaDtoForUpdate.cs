﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public partial class CategoriaDtoForUpdate : Entity
    {
        public string Nombre { get; set; }
    }
}