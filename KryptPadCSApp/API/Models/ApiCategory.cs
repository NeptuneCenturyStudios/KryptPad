﻿using KryptPadCSApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryptPadCSApp.API.Models
{
    class ApiCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ApiItem> Items { get; set; } = new List<ApiItem>();

        public ApiProfile Profile { get; set; }
    }
}
