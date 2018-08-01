using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpE.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpE.Repository
{
    public class SQLContext : DbContext
    {
        public SQLContext(DbContextOptions<SQLContext> options) : base(options)
        {
        }

        public DbSet<DropDownOptions> DropDownOptionses { get; set; }
        public DbSet<TemplateOptions> TemplateOptionses { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<MyForm> MyForms { get; set; }
    }
}
