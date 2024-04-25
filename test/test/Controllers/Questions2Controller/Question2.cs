using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static test.Controllers.TaskController.Detail;

namespace test.Controllers.Questions2Controller
{
    [Route("api/questions/[controller]")]
    [ApiController]
    public class Question2 : Controller
    {
        public class Q2Request
        {
            [MaxLength(99)]
            public string Str { get; set; }
        }

        public class Q2Response
        {
            public string Rank { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Q2Response>>> GetDbTaskById([FromBody] Q2Request request)
        {
            string[] stringReq = request.Str.Split(',');
            var strings = stringReq.Where(s => !int.TryParse(s, out _)).OrderBy(s => s);
            var numbers = stringReq.Where(s => int.TryParse(s, out _)).OrderBy(s => int.Parse(s));
            string[] sortedStringReq = strings.Concat(numbers).ToArray();
            List<Q2Response> response = new List<Q2Response>();

            foreach (string str in sortedStringReq)
            {
                response.Add(new Q2Response { Rank = str.Trim() });
            }
            return response;
        }
    }
}
