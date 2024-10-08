﻿using LiteDB;
using Qorpe.Application.Common.Interfaces.Repositories;
using Qorpe.Domain.Entities;

namespace Qorpe.Infrastructure.Data.Lite;

public class ClusterRepository(ILiteDatabase database) : Repository<ClusterConfig>(database), IClusterRepository
{
}
