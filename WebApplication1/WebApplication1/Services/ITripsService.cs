﻿using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface ITripsService
{
    Task<IEnumerable<TripDTO>> GetTripsAsync(CancellationToken cancellationToken);
}