using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using ForgeAppTesting.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForgeAppTesting.Controllers
{
    [ApiController]
    public class GoodPracticesController : ControllerBase
    {
        private readonly GoodPracticesService _gpService;
        public GoodPracticesController(GoodPracticesService gpService)
        {
            _gpService = gpService;
        }

        [HttpGet]
        [Route("api/goodpractices/{projectId}")]
        public async Task<dynamic> GetGoodpracticesAsync(string projectId)
        {
            var all = _gpService.GetFromProject(projectId);

            return all;
        }

        [HttpGet]
        [Route("api/goodpractices/{projectId}/versions/{versionId:int}")]
        public async Task<dynamic> GetVersionGoodPracticesAsync(string projectId, int versionId)
        {
            var versions = _gpService.GetVersionFromProject(projectId, versionId);

            return versions;
        }

    }
}