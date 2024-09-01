﻿using Qorpe.Domain.Common;

namespace Qorpe.Application.Common.Interfaces.Repositories;

public interface IRouteRepository<TDocument> : IRepository<TDocument>
    where TDocument : Document
{
}
