using SYJ.Domain.Managers;
using System.Threading.Tasks;
using System.Web.Http;

namespace SueldosYjornales.Controllers.Api {
    public class ConceptosIngreEgresController : ApiController {
        [HttpGet]
        public async Task<IHttpActionResult> Get() {
            var ciem = new ConceptosIngreEgresManagers();
            var listado = await ciem.Listado();

            if (listado == null) {
                return NotFound();
            }
            return Ok(listado);
        }
    }
}
