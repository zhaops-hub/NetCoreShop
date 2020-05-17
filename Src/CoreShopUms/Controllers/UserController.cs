using AutoMapper;
using CoreShopUms.Conf;
using CoreShopUms.Infrastructure;
using CoreShopUms.Infrastructure.Entity;
using CoreShopUms.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoreShopUms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly UmsContext _umsContext;
        private readonly IMapper _mapper;

        public UserController(IOptionsSnapshot<AppSettings> settings, UmsContext umsContext, IMapper mapper)
        {
            _settings = settings;
            _umsContext = umsContext;
            _mapper = mapper;

            _umsContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/[controller]/{id}
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserModel>> GetUserById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var item = await _umsContext.Users.SingleOrDefaultAsync(ci => ci.Id == id);


            if (item != null)
            {
                return _mapper.Map<UserModel>(item);
            }

            return NotFound();
        }

        // POST api/[controller]/AddUser
        [HttpPost]
        [Route("AddUser")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> AddUser([FromBody]UserModel user)
        {

            if (_umsContext.Users.Count(d => d.Account == user.Account) > 0) return BadRequest(new { Msg = "acount 不能重复！" });

            var entity = new User
            {
                Id = Guid.NewGuid().ToString(),
                RealName = user.RealName,
                Account = user.Account,
                Password = user.Password
            };

            _umsContext.Users.Add(entity);

            await _umsContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = entity.Id }, null);
        }

        // PUT api/[controller]/UpdateUser
        [HttpPut]
        [Route("UpdateUser")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateUser([FromBody]UserModel user)
        {
            var entity = await _umsContext.Users.SingleOrDefaultAsync(d => d.Id == user.Id);
            if (entity == null)
            {
                return NotFound(new { Msg = $"{user.Id} 找不到记录" });
            }


            if (!string.IsNullOrWhiteSpace(user.RealName)) entity.RealName = user.RealName;
            if (!string.IsNullOrWhiteSpace(user.Password)) entity.Password = user.Password;

            _umsContext.Update(entity);
            
            await _umsContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = entity.Id }, null);
        }


        // POST api/[controller]/
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(PaginatedItemsModel<UserModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PaginatedItemsModel<UserModel>>> GetUsers([FromBody]UserModel whereModel, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var root = (IQueryable<User>)_umsContext.Users;

            var totalItems = await root
               .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItemsModel<UserModel>(pageIndex, pageSize, totalItems, _mapper.Map<List<UserModel>>(itemsOnPage));
        }

    }
}