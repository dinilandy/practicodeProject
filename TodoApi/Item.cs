using System;
using System.Collections.Generic;

namespace TodoApi;

public partial class Item
{
    public int Id { get; set; }

    public string? Nmae { get; set; }

    public bool? IsComplete { get; set; }
}
