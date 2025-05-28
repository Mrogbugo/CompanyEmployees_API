using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
   public class LinkResourceBase
    { 
        public LinkResourceBase()
        { 
            Links = new List<Link>(); // you must initialize Link in the LinkResourceBase to avoid getting NullReferenceExpection
        } 
        public List<Link> Links { get; set; }   
    }
}
