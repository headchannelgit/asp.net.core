using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers.Api
{
    [Route("api/contacts")]
    public class ContactsController: Controller
    {
        private ILogger _logger;
        private IEFRepository _repository;

        public ContactsController(IEFRepository repository, ILogger<ContactsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

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
                _logger.LogError($"Erroor {ex.Message}");
                return BadRequest("Error");
            }
        }

        [HttpPost("")]
        public IActionResult Post([FromBody]ContactVM model)
        {
            if (ModelState.IsValid)
            {
                var contact = Mapper.Map<Contact>(model);

                return Created($"api/contacts/{model.Name}", Mapper.Map<ContactVM>(contact));
            }
            return BadRequest(ModelState);
        }
    }
}
