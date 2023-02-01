﻿using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Funding : EntityBase<string>, IAggregateRoot
{
    private Funding() { }
    public Funding(
        string id, 
        string source)
    {
        Id = id;
        Source = source;
    }
    public string Source { get; init; } = default!;
}