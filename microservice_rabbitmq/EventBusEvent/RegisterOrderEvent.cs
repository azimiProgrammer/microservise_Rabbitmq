﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusEvent
{
    public class RegisterOrderEvent
    {
        public long Id { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
