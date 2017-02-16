# Wprowadzenie do ASP.NET Core
> **Dokumentacja**
> 
> https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel
> https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel
#### 1. Utwórz nowy pusty projekt asp.net core 

* Pokaż **Program.cs** [ASP.NET Core Fundamentals](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/)
  -  _Kestrel_
     > Kestrel is a cross-platform web server for ASP.NET Core based on **libuv**, a cross-platform asynchronous I/O library.
     > 
     > **Libuv** is a multi-platform support library with a focus on asynchronous I/O. It was primarily developed for use by Node.js, but it's also used by Luvit, Julia, pyuv, and others.
  - _Content root_
     > The content root is the base path to any content used by the app, such as its views and web content. By default the content root is the same as application base path for the executable hosting the app; an alternative location can be specified with WebHostBuilder.
  - _Web root_
     > The web root of your app is the directory in your project for public, static resources like css, js, and image files. The static files middleware will only serve files from the web root directory (and sub-directories) by default. The web root path defaults to /wwwroot, but you can specify a different location using the WebHostBuilder.
  - UseIISIntegration - *web.config*
* Pokaż **Startup.cs** [ASP.NET Core Application Startup](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup)
  - _The Configure method__
     > The Configure method is used to specify how the ASP.NET application will respond to HTTP requests. The request pipeline is configured by adding [middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware) 
  - _The ConfigureServices method__
     > The ConfigureServices method in the Startup class is responsible for defining the services the application will use, including platform features like Entity Framework Core and ASP.NET Core MVC. Initially,

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

[!code-csharp[Main](start-mvc/sample/src/MvcMovie/Startup.cs)]
