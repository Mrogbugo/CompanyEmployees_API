﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.DataTransferObjects
{
    public record EmployeeDto(Guid Id, string Name, int Age ,string Position);
   
}
