﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Remotion.Linq.Clauses.Expressions;

namespace LaoZhangCoreTwo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;

        public ValuesController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, "laozhang"),
                new Claim(JwtRegisteredClaimNames.Email, "123456@qq.com"),
                new Claim(JwtRegisteredClaimNames.Sub, "1"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("laozhanglaozhang"));

            var token=new JwtSecurityToken(
                issuer:"http://localhost:5000",
                audience:"http://localhost:5001",
                claims:claims,
                expires:DateTime.Now.AddHours(1),
                signingCredentials:new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
                );

            var jwtToken=new JwtSecurityTokenHandler().WriteToken(token);

            return new string[] { jwtToken };
        }

        // GET api/values/5
        [HttpGet("{jwtStr}")]
        public ActionResult<IEnumerable<string>> Get(string jwtStr)
        {
//            var jwtHandler=new JwtSecurityTokenHandler();
//            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
//
//            var sub = User.FindFirst(d => d.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            var name = _accessor.HttpContext.User.Identity.Name;
            var claims = _accessor.HttpContext.User.Claims;
            var claimTypeVal = (from claim in claims
                where claim.Type == JwtRegisteredClaimNames.Email
                select claim.Value).ToList();

            return new string[]
            {
                name,
                JsonConvert.SerializeObject(claimTypeVal)
            };

        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
