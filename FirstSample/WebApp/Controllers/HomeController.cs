using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using WebApp.Services;
using Microsoft.Extensions.Configuration;
using WebApp.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private IEFRepository _repository;

        public HomeController(IMailService mailService, IConfigurationRoot config, IEFRepository repository)
        {
            _mailService = mailService;
            _config = config;
            _repository = repository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var data = _repository.GetAllContacts();
            return View(data);
        }


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
