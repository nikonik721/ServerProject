using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PS.DL.PackGenerator.Interface;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnvelopeController : ControllerBase
    {
        private readonly IPackGenerator _packGenerator;
        public EnvelopeController(IPackGenerator packGenerator)
        {
            _packGenerator = packGenerator;
        }

        [HttpPost(nameof(CreateCalculationPacks))]
        public IActionResult CreateCalculationPacks(int envelopeNum)
        {
            if (envelopeNum <= 0)
            {
                return BadRequest(envelopeNum);
            }

            _packGenerator.GeneratePacks(envelopeNum);

            return Ok(envelopeNum);
        }
    }
}
