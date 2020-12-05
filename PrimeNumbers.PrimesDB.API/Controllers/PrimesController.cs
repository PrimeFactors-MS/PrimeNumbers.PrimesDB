using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrimeNumbers.PrimesDB.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeNumbers.PrimesDB.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrimesController : ControllerBase
    {
        private readonly ILogger<PrimesController> _logger;
        private readonly PrimeCacheDb _primeCacheDb;

        public PrimesController(ILogger<PrimesController> logger, PrimeCacheDb primeCacheDb)
        {
            _logger = logger;
            _primeCacheDb = primeCacheDb;
        }

        [HttpGet]
        public IActionResult GetPrimeRecord([FromQuery] ulong number)
        {
            if (_primeCacheDb.TryGetPrimeRecord(number, out PrimeRecord record))
            {
                return Ok(record);
            }
            return NotFound(null);
        }

        [HttpPost]
        public IActionResult InsertPrimeRecord([FromBody] PrimeRecord record)
        {
            if (record != null)
            {
                _primeCacheDb.InsertPrimeRecord(record);
            }
            return Ok();
        }
    }
}
