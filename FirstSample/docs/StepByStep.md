# Wprowadzenie do ASP.NET Core
> **Dokumentacja**
> 
> https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel
> https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel

# ASP.NET Pierwsze kroki
##### 1. Utwórz nowy pusty projekt asp.net core
* Pokaż **Program.cs** [ASP.NET Core Fundamentals](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/)
  - _Kestrel_
     > Kestrel is a cross-platform web server for ASP.NET Core based on **libuv**, a cross-platform asynchronous I/O library.
     > 
     > **Libuv** is a multi-platform support library with a focus on asynchronous I/O. It was primarily developed for use by Node.js, but it's also used by Luvit, Julia, pyuv, and others.
  - _Content root_
     > The content root is the base path to any content used by the app, such as its views and web content. By default the content root is the same as application base path for the executable hosting the app; an alternative location can be specified with WebHostBuilder.
  - _Web root_
     > The web root of your app is the directory in your project for public, static resources like css, js, and image files. The static files middleware will only serve files from the web root directory (and sub-directories) by default. The web root path defaults to /wwwroot, but you can specify a different location using the WebHostBuilder.
  - UseIISIntegration - *web.config*
* Pokaż **Startup.cs** [ASP.NET Core Application Startup](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup)
  - _The Configure method_
     > The Configure method is used to specify how the ASP.NET application will respond to HTTP requests. The request pipeline is configured by adding [middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware) 
  - _The ConfigureServices method_
     > The ConfigureServices method in the Startup class is responsible for defining the services the application will use, including platform features like Entity Framework Core and ASP.NET Core MVC. Initially,

     - !! Usuń obsłuę wyjatków. Pokażemy ją później !!
	```c#
	public void ConfigureServices(IServiceCollection services)
	{
		// Add framework services.
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

		services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		services.AddMvc();

		// Add application services.
		services.AddTransient<IEmailSender, AuthMessageSender>();
		services.AddTransient<ISmsSender, AuthMessageSender>();
	}
	```


##### 2. Utwórz plik Index.html

* Utwórz plik index.html, wrzuć do wwwroot.
* Uruchom projekt w przeglądarce i pokaż że plik się nie wyświetli
* Dodaj obsługę plików statycznych
    ```c#
    app.UseStaticFiles();
    ```
    > Pokaż dodane referencje w project.json 
    > Pokaż że zadziałało

    * Alternatywnie można dodać referencje poprzez project.json
    ```c#
    "Microsoft.AspNetCore.StaticFiles": "1.0.0"
    ```
* Dodaj obsługę domyślnych plików
  * Zła kolejność (pokaż i napraw)
    ```c#
    app.UseStaticFiles();
    app.UseDefaultFiles();
    ```
# MVC 6
 
##### 1. Utwórz kontroler
- Dodaj referencje do MVC bezpośrednio przez Project.Json
    ```json
       "Microsoft.AspNetCore.Mvc": "1.1.1"
    ```

##### 2. Dodaj Widok
##### 3. Dodaj middleware dla MVC
- Włącz MVC
   ```c#
    app.UseMvc();
   ```
- Pokaż że wciąż MVC nie działa
- Dodaj DI dla MVC
    ```c#
    services.AddMvc();
    ```
- Pokaż że wciąż MVC nie działa
- Dodaj domyślną route
    ```c#
            app.UseMvc(config =>
           {
               config.MapRoute(
                   name: "Default",
                   template: "{controller}/{action}/{id?}",
                   defaults: new { controller = "Home", action = "Index" }
                   );
           });
    ```

##### 4. Dodaj plik layoutu /Views/Shared/_Layout.cshtml
- Utwórz _ViewStart.cshtml aby przypisać na wszystkich widoków domyślny layout
##### 5. Pokaż obsługę wyjątków
- Dodaj wyjatek w akcji Index, pokaż że pomimo błędu nie widzimy detali błędu.
- Dodaj middleware dla Develoepr Exception Page
    ```c#
    app.UseDeveloperExceptionPage();
    ```
- Pokaż 
   ```c#
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
     ```
     lub

     ```c#
    if (env.IsEnvironment("Development"))
    {
        app.UseDeveloperExceptionPage();
    }
    ```
- Pokaż zmienna ASPNETCORE_ENVIRONMENT w  konfiguracji pliku projektu, zakładka debug

##### 6. Tag Helpers
   > Tag Helpers dla bootstrap: https://github.com/dpaquette/TagHelperSamples
- Dodaj i pokaż że jeszcze nie działa
    ```html
    <a asp-controller="Home" asp-action="Index">Home</a>
    ```
- Dodaj dependencies (project.json)
    ```json
    "Microsoft.AspNetCore.Mvc.TagHelpers": "1.1.1"
    ```
- Dodaj _ViewImports.cshtml, pokaż ze juz działa
    ```c#
    @addTagHelper "*, Microsoft.AspNetCore.Mvc.TagHelpers"
    ```
- Utwórz strone Contact (controller, model, widok), pokaż ze tag helpers działają
    - widok
    ```html
    <form method="post">
    <label asp-for="Name"></label>
    <input asp-for="Name" />
    <label asp-for="Email"></label>
    <input type="email" asp-for="Email" />
    <label asp-for="Message"></label>
    <textarea cols="40" rows="4" asp-for="Message"></textarea>
    <div>
        <input type="submit" value="Send Message" />
    </div>
    </form>
    ```
    - model
    ```c#
    public class ContactVM
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
    ```
    Jeśli intelisense nie działa:
    ```json
      "Microsoft.AspNetCore.Razor.Tools": {
      "version": "1.0.0-preview2-final",
      "type": "build"
    },
    ```
    jako dependencies i tools

- Pokaż jak utworzyć własny tag helper
    - TagHelpers/EmailTagHelper.cs
    ```c#
    namespace TheWorld.TagHelpers
    {
        public class EmailTagHelper : TagHelper
        {
            public string Address { get; set; }
            public override void Process(TagHelperContext context, TagHelperOutput output)
            {
                output.TagName = "a";
                output.Attributes.SetAttribute("href", "mailto:" + Address);
                output.Content.SetContent(Address);
            }
        }
    }
    ```
    - _ViewImports.cshtml
    ```html
    @addTagHelper "*, WebApp"
    ```
 #### 7. Javascript (jQuery)
 - Dodaj biblioteki w bower.json
 ```json
    "jquery": "~3.1.1",
    "jquery-validation": "~1.16.0",
    "jquery-validation-unobtrusive": "~3.2.6"
```
- Dodaj script w contact.cshtml
```html
  <script src="~/lib/jquery-validation/dist/jquery.validate.min.js"></script>
  <script src="~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"></script>
  ...
  <div asp-validation-summary="ModelOnly"></div>
  ...
  <span asp-validation-for="Name"></span>
```
- Zmodyfikuj kontroler
#### 8. IMailService - Wstrzykiwanie zależności
- Dodaj interfejs i klase DebugMailService
```c#
    public interface IMailService
    {
        void SendMail(string to, string from, string subject, string body);
    }

    public class DebugMailService : IMailService
    {
        public void SendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending Mail: To : {to} From: {from}, Subject: {subject}, Body: {body}");
        }
    }
```
- Wstrzyknij IMailService do kontrolera
```c#
    private IMailService _mailService;

    public HomeController(IMailService mailService)
    {
        _mailService = mailService;
    }
```
- Skonfiguruj DI
```c#
    services.AddTransient<IMailService, DebugMailService>();
```
- DI Environment settings
```c#
    private IHostingEnvironment _env;

    public Startup(IHostingEnvironment env)
    {
        _env = env;
    }
    --------------
    if (_env.IsDevelopment())
    {
        services.AddTransient<IMailService, DebugMailService>();
    }
    else
    {
        /// real mail service
    }
```
#### 9. Configuration file
- Add config.json
```json
    {
      "MailSettings": {
        "ToAddress": "sdfds@sdfds.pl"
      },
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=_CHANGE_ME;Trusted_Connection=True;MultipleActiveResultSets=true"
      }
    }
```
- Startup.cs - zmodyfikuj konstruktor
```c#
private IConfigurationRoot _config;
...
public Startup(IHostingEnvironment env)
...
    var builder = new ConfigurationBuilder()
        .SetBasePath(_env.ContentRootPath)
        .AddJsonFile("config.json")
        .AddEnvironmentVariables();

    _config = builder.Build();
}
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton(_config);
...
```
- Dodaj DI do kontrolera, i użyj wartości z configa
```c#
    private IConfigurationRoot _config;

    public HomeController(IMailService mailService, IConfigurationRoot config)
    {
        _mailService = mailService;
        _config = config;
    }
...
    _config["MailSettings:ToAddress"]
...
```
#### 10. Bootstrap
- Dodaj bootstrap do bower.json
```json
    "bootstrap": "~3.3.7"
```
- Dodaj style do _layout.cshtml, nadaj styl dla przycisku submit

#### 11. Entity framework Core
- Tworzymy model
```csharp
namespace WebApp.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
```
- Tworzymy DBContext
```c#
namespace WebApp.Models
{
    public class EFContext : DbContext
    {
        private IConfigurationRoot _config;

        public EFContext(IConfigurationRoot config, DbContextOptions options): base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:DefaultConnection"]);
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
```
- Konfigurujemy DI
```c#
        public void ConfigureServices(IServiceCollection services)
        {
            ...
            services.AddDbContext<EFContext>();
            ...
        }
```
- Wykorzystujemy w kontrolerze
```csharp
        public HomeController(IMailService mailService, IConfigurationRoot config, EFContext context)
        {
            ...
            _context = context;
            ...
        }

        public IActionResult Index()
        {
            var data = _context.Contacts.ToList();
            return View(data);
        }
```
- Dodajemy dependency i tooling
```json
    "Microsoft.EntityFrameworkCore.Tools": {
      "version": "1.0.0-preview2-final",
      "type": "build"
    }
    ...
     "Microsoft.EntityFrameworkCore.Tools": {
      "version": "1.0.0-preview2-final"
    }
```
- Cmd
```bat
dotnet ef migrations add InitialDatabase
```
następnie
```bat
dotnet ef database update
```
Pokaż że działa
- Seeding the Database

```csharp
namespace WebApp.Models
{
    public class EFSeedData
    {
        private EFContext _context;

        public EFSeedData(EFContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedData()
        {
            if(!_context.Contacts.Any())
            {
                var contact_1 = new Contact()
                {
                    Email = "sdfsd@sdfsd.pl",
                    Message = "sdfsdfs",
                    Name = "sdfds"
                };
                _context.Contacts.Add(contact_1);

                await _context.SaveChangesAsync();
            }
        }
    }
}
```
dodaj IoC
```csharp
services.AddTransient<EFSeedData>();
...
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, EFSeedData seedData)
...
seedData.EnsureSeedData().Wait();
```
uruchom
- Repository pattern
```csharp
namespace WebApp.Models
{
    public class EFRepository: IEFRepository
    {
        private EFContext _context;

        public EFRepository(EFContext context)
        {
            _context = context;
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            return _context.Contacts.ToList();
        }
    }
}
```
utwórz takze interfejs, skonfiguruj zależnosc jako scoped, i popraw kontroler
Popraw model w widoku dla Home/Index
```html
@model IEnumerable<WebApp.Models.Contact>
```
#### 12. Logging
- Skonfiguruj DI
```csharp
    services.AddLogging();
```
- Skonfiguruj ILoggerFactory
```csharp
    if (env.IsEnvironment("Development"))
    {
        app.UseDeveloperExceptionPage();
        loggerFactory.AddDebug(LogLevel.Information);
    }
    else
    {
        loggerFactory.AddDebug(LogLevel.Error);
    }
```
- Obsłuż logowanie w kontrolerze
Konstruktor
```csharp
ILogger<HomeController> logger
```
Index
```csharp
  try
    {
        var data = _repository.GetAllContacts();
        return View(data);

    }
    catch (Exception ex)
    {
        _logger.LogError($"Error: {ex.Message}");
        return Redirect("/error");
    }
```
#### API
- Kontroler
```csharp
namespace WebApp.Controllers.Api
{
    public class ContactController: Controller
    {
        private IEFRepository _repository;

        public ContactController(IEFRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("api/contacts")]
        public IActionResult Get()
        {
            return Ok(_repository.GetAllContacts());
        }
    }
}
```
- CamelCase
```csharp
services.AddMvc()
    .AddJsonOptions(config =>
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
```
- Post
```csharp
        [HttpPost("")]
        public IActionResult Post([FromBody]ContactVM model)
        {
            if (ModelState.IsValid)
            {
                return Created($"api/contacts/{model.Name}", model);
            }
            return BadRequest("Bad data");
        }
```
Dodaj Route do kontrolera
```csharp
    [Route("api/contacts")]
    public class ContactsController: Controller
```
Zamien (chcemy pokazac ze mozemy zwrocic z serwera błędy walidacyjne)
```csharp
 return BadRequest("Bad data");
 ```
 na
 ```csharp
 return BadRequest(ModelState);
 ```
 - Automapper
   - project.json
   ```json
   "AutoMapper": "5.2.0"
   ```
   - ContactsController
   ```csharp
    var contact = Mapper.Map<Contact>(model);

    return Created($"api/contacts/{model.Name}", Mapper.Map<ContactVM>(contact));
    ```
    - Skonfiguruj automappera
    ```csharp
    Mapper.Initialize(config =>
    {
        config.CreateMap<ContactVM, Contact>().ReverseMap();
    });
    ```
    - Popraw Get
    ```csharp
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = _repository.GetAllContacts();
                return Ok(Mapper.Map<IEnumerable<ContactVM>>(results));
            }
            catch (Exception ex)
            {
                return BadRequest("Error");
            }
        }
      ```
#### AngularJS
