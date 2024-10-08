﻿using Qorpe.Domain.Enums;

namespace Qorpe.Application.Common.DTOs;

public sealed class SessionAffinityCookieConfigDto
{
    public string? Path { get; set; }
    public string? Domain { get; set; }
    public bool? HttpOnly { get; set; }
    public CookieSecurePolicy? SecurePolicy { get; set; }
    public SameSiteMode? SameSite { get; set; }
    public TimeSpan? Expiration { get; set; }
    public TimeSpan? MaxAge { get; set; }
    public bool? IsEssential { get; set; }
}
