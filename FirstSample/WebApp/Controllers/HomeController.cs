using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using WebApp.Services;
using Microsoft.Extensions.Configuration;
using WebApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private IEFRepository _repository;
        private ILogger<HomeController> _logger;

        public HomeController(IMailService mailService, IConfigurationRoot config, IEFRepository repository, ILogger<HomeController> logger)
        {
            _mailService = mailService;
            _config = config;
            _repository = repository;
            _logger = logger;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
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
        }


        [Authorize]
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactVM model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "sdfdsf", model.Message);
                return RedirectToAction("Contact");
            }
            return View();
        }
    }
}
