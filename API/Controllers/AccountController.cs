using API.Data;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        public DataContext Context { get; }
        public AccountController(DataContext context)
        {
            this.Context = context;
        }
    }
}