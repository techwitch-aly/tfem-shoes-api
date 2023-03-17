using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfemshoes.Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }
    }
}
