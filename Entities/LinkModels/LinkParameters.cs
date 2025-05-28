using Microsoft.AspNetCore.Http;
using Share.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public record LinkParameters(EmployeeParameters EmployeeParameters, HttpContext Context);

}
