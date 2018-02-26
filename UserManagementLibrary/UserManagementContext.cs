using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Entity;

namespace UserManagementLibrary
{
    public class UserManagementContext : DbContext
    {
        public UserManagementContext(string dbconnect) : base(dbconnect)
        { }

        public DbSet<User> Users { set; get; }
    }
}
