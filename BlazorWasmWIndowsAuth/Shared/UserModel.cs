using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWasmWIndowsAuth.Shared;
public class UserModel
{

    public string? Login { get; set; }

    public string? JWT { get; set; }
    public List<string> Roles { get; set; } = new List<string>();

    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }



}
