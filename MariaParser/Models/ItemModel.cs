using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;


namespace MariaParser.Models
{
    public  class ItemModel
    {
        public ItemModel() {
            Publications = new List<Publication>();
        }

        [Key]
        public int Id { get; set; }
        public String Html { get; set; }
        public String Info { get; set; }
        public virtual List<Publication> Publications { get; set; }


    }

    public  class Publication
    {
        public Publication() {
            Authors = new List<Authors>();
        }
        [Key]
        public int Id { get; set; }
        public String title { get; set; }
        public String link { get; set; }
        public String html { get; set; }
        public String AuthorsString { get; set; }
        public virtual List<Authors> Authors { get; set; }

    }

    public class Authors
    {
        public Authors() { }
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String link { get; set; }
        public int Priority { get; set; }

    }

    public class WofraContext : DbContext
    {

        public DbSet<ItemModel> ItemModels { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Authors> Authors { get; set; }

    }

}
