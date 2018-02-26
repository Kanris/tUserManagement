using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementLibrary
{
    [Table("User")]
    public class User
    {
        [Key]
        [Column("ID")]
        public int Id { set; get; }

        [Required]
        [MinLength(3)]
        [Column("Name")]
        public string Name { set; get; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { set; get; }

        [Required]
        [Range(18, 100)]
        [Column("Age")]
        public int Age { set; get; }
    }
}
