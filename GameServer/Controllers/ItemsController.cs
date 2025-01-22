using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameServer.Models;

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly GameContext _gameContext;

        public ItemsController(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _gameContext.Items.ToListAsync();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItem(string id)
        {
            var item = await _gameContext.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return ItemToDTO(item);
        }

        //// sessionId를 받아 users 테이블에서 유저를 찾아 user의 id를 얻는다.
        //// 해당 id를 갖고 있는 items 테이블의 튜플들을 배열로 반환한다.
        //[HttpGet("session/{sessionId}")]
        //public async Task<ActionResult<IEnumerable<Item>>> GetItems(string sessionId)
        //{

        //}

        // PUT: api/Items/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(string id, Item item)
        {
            if (id != item.UserId)
            {
                return BadRequest();
            }

            _gameContext.Entry(item).State = EntityState.Modified;

            try
            {
                await _gameContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Items
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _gameContext.Items.Add(item);
            try
            {
                await _gameContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ItemExists(item.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetItem", new { id = item.UserId }, item);
        }

        // 유저의 아이템 보드 속 아이템 생성
        //
        // user id를 받아 이를 갖는 빈 아이템을 8x7개 만들어
        // items 테이블에 추가한다.
        [HttpPost("{userId}")]
        public async Task<IActionResult> GiveFirstItemTable(string userId)
        {

            for (var row = 0; row < 8; row++)
            {
                for (var col = 0; col < 7; col++)
                {
                    try
                    {
                        await _gameContext.Items.AddAsync(new Item
                        {
                            UserId = userId,
                            State = "",
                            Type = "",
                            Name = "",
                            Level = 1,
                            X = row,
                            Y = col
                        });
                    }
                    catch (DbUpdateException)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError,
                        new { Message = "An error occurred while producing the item table." });
                    }
                }
            }

            try
            {
                await _gameContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "An error occurred while updating database after producing the item table." });
            }

            return NoContent();
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            var item = await _gameContext.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _gameContext.Items.Remove(item);
            await _gameContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(string id)
        {
            return _gameContext.Items.Any(e => e.UserId == id);
        }

        private static ItemDTO ItemToDTO(Item item) =>
        new ItemDTO
        {
            State = item.State,
            Type = item.Type,
            Name = item.Name,
            Level = item.Level,
            X = item.X,
            Y = item.Y
        };
    }
}
