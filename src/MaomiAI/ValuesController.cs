using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaomiAI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 测试 API
        /// </summary>
        /// <returns>测试数据</returns>
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(new { message = "Hello World" });
        }

        /// <summary>
        /// 获取指定 ID 的数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>数据</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(new { id, message = $"Item {id}" });
        }

        /// <summary>
        /// 创建新数据
        /// </summary>
        /// <param name="value">数据值</param>
        /// <returns>创建结果</returns>
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return Ok(new { message = $"Created: {value}" });
        }
    }
}
