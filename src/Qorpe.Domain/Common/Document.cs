﻿namespace Qorpe.Domain.Common;

public abstract class Document
{
    public string? Id { get; set; }
    public string? TenantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
