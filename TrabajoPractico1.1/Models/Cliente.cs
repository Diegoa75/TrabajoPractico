using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrabajoPractico.Models
{
    public class Cliente
    {
        public string email { get; set; }
        public int dni { get; set; }

        public Cliente()
        {
            this.dni = 1234;
            this.email = "mail";
        }
    }
}