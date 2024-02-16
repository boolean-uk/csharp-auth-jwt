using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.Data.Models
{
    public interface IEntity
    {
        [Column("id")]
        public string Id { get; set; }
    }
}
