﻿using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Contact : EntityBase<string>, IAggregateRoot
{
    private Contact() { }
    public Contact(
        string id, 
        string? title, 
        string name, 
        string telephone, 
        string textPhone,
        string? url, 
        string? email
        )
    {
        Id = id;
        Title = title;
        Name = name;
        Telephone = telephone;
        TextPhone = textPhone;
        Url = url;
        Email = email;
    }
    public string? Title { get; set; }
    public string Name { get; set; } = default!;
    public string Telephone { get; set; } = default!;
    public string TextPhone { get; set; } = default!;
    public string? Url { get; set; }
    public string? Email { get; set; }
}