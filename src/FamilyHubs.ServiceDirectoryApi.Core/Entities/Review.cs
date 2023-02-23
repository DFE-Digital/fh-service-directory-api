using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Review : EntityBase<string>, IAggregateRoot
{
    private Review() { }


    public Review(
        string id, 
        string serviceId, 
        string title, 
        string? description, 
        DateTime date, 
        string? score, 
        string? url, 
        string? widget
        )
    {
        Id = id;
        Title = title;
        Description = description;
        Date = date;
        Score = score;
        Url = url;
        Widget = widget;
        ServiceId = serviceId;
    }
    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public DateTime Date { get; private set; }
    public string? Score { get; private set; }
    public string? Url { get; private set; }
    public string? Widget { get; private set; }
    public string ServiceId { get; private set; } = default!;

    public void Update(Review review)
    {
        Title = review.Title;
        Description = review.Description;
        Date = review.Date;
        Score = review.Score;
        Url = review.Url;
        Widget = review.Widget;
        ServiceId = review.ServiceId;
    }

}
