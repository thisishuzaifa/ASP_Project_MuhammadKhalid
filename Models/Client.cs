using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Project_MuhammadKhalid.Models
{
    public class Client
    {
        [Key]
        public int clientID { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string email { get; set; }

        public virtual ICollection<ClientAccount>
            ClientAccount
        { get; set; }
    }
}
