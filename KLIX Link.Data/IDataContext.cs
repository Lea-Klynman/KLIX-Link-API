using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace KLIX_Link.Data
{
    public interface IDataContext
    {
        public DbSet<User> _Users { get; set; }
        public DbSet<UserFile> _Files { get; set; }

        Task<int> SaveChangesAsync();
    }
}
