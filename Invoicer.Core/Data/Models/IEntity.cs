﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoicer.Core.Data.Models;

public interface IEntity
{
    public string Id { get; set; }
}
