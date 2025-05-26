using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApi.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hotel Booking API",
        Version = "v1",
        Description = "API for managing properties, room types, and reservations"
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("HotelBookingDb"));

builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage);

        return new BadRequestObjectResult(new { Errors = errors });
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Booking API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    SeedData(context);
}

app.Run();
return;

static void SeedData(ApplicationDbContext context)
{
    if (context.Properties.Any()) return;

    var property1 = new Property
    {
        Id = Guid.NewGuid(),
        Name = "Grand Hotel Moscow",
        Country = "Russia",
        City = "Moscow",
        Address = "Red Square, 1",
        Latitude = (decimal)55.7558,
        Longitude = (decimal)37.6176,
        RoomTypes = new List<RoomType>()
    };

    var property2 = new Property
    {
        Id = Guid.NewGuid(),
        Name = "Hotel Saint Petersburg",
        Country = "Russia",
        City = "Saint Petersburg",
        Address = "Nevsky Prospect, 100",
        Latitude = (decimal)59.9311,
        Longitude = (decimal)30.3609,
        RoomTypes = new List<RoomType>()
    };

    var roomType1 = new RoomType
    {
        Id = Guid.NewGuid(),
        PropertyId = property1.Id,
        Name = "Standard Room",
        DailyPrice = 5000,
        Currency = "RUB",
        MinPersonCount = 1,
        MaxPersonCount = 2,
        Services = ["WiFi", "TV", "Air Conditioning"],
        Amenities = ["Bathroom", "Minibar", "Safe"]
    };

    var roomType2 = new RoomType
    {
        Id = Guid.NewGuid(),
        PropertyId = property1.Id,
        Name = "Deluxe Suite",
        DailyPrice = 12000,
        Currency = "RUB",
        MinPersonCount = 1,
        MaxPersonCount = 4,
        Services = ["WiFi", "TV", "Air Conditioning", "Room Service"],
        Amenities = ["Bathroom", "Minibar", "Safe", "Balcony", "Living Room"]
    };

    var roomType3 = new RoomType
    {
        Id = Guid.NewGuid(),
        PropertyId = property2.Id,
        Name = "Economy Room",
        DailyPrice = 3000,
        Currency = "RUB",
        MinPersonCount = 1,
        MaxPersonCount = 2,
        Services = ["WiFi", "TV"],
        Amenities = ["Bathroom", "Desk"]
    };

    property1.RoomTypes.Add(roomType1);
    property1.RoomTypes.Add(roomType2);
    property2.RoomTypes.Add(roomType3);

    context.Properties.AddRange(property1, property2);
    context.SaveChanges();
}