# Hotel BookingSystem Backend

## Scaffolding

### DB Migrations

~~~bash
dotnet ef migrations --project App.DAL.EF --startup-project WebApp add Initial
dotnet ef migrations --project App.DAL.EF --startup-project WebApp remove Initial
dotnet ef database --project App.DAL.EF --startup-project WebApp update
dotnet ef database --project App.DAL.EF --startup-project WebApp drop
~~~

### Controllers

#### Web API
~~~bash
cd WebApp

dotnet aspnet-codegenerator controller -name AmenitiyController -actions -m App.Domain.Amenity -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name BookingController -actions -m App.Domain.Booking -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name GuestController -actions -m App.Domain.Guest -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name HotelController -actions -m App.Domain.Hotel -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name RoomController -actions -m App.Domain.Room -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name RoomTypeController -actions -m App.Domain.RoomType -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name StayController -actions -m App.Domain.Stay -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
~~~
