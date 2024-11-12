﻿using System;
using System.Collections.Generic;

namespace Profescipta_Sales_Order.Models;

public partial class SoOrder
{
    public long SoOrderId { get; set; }

    public string OrderNo { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public int ComCustomerId { get; set; }

    public string Address { get; set; } = null!;
}