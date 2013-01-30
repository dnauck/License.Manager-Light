using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;
namespace LightSwitchApplication
{
    public partial class License
    {
        partial void License_Created()
        {
            if (LicenseId == null || LicenseId == Guid.Empty)
                LicenseId = Guid.NewGuid();
        }
    }
}
