﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class MaxAgeRangeBadRequestException : BadRequestException
    {
        public MaxAgeRangeBadRequestException() 
               :base("Max Age Can't be Less Than Min Age.")
        {
            
        }
    }
}
