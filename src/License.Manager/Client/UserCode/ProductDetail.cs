using System;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Windows;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Framework.Client;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;
using Microsoft.LightSwitch.Threading;

namespace LightSwitchApplication
{
    public partial class ProductDetail
    {
        partial void Product_Loaded(bool succeeded)
        {
            // Write your code here.
            this.SetDisplayNameFromEntity(this.Product);
        }

        partial void Product_Changed()
        {
            // Write your code here.
            this.SetDisplayNameFromEntity(this.Product);
        }

        partial void ProductDetail_Saved()
        {
            // Write your code here.
            this.SetDisplayNameFromEntity(this.Product);
        }

        partial void CopyPrivateKeyToClipBoard_Execute()
        {
            Dispatchers.Main.BeginInvoke(() => Clipboard.SetText(Product.KeyPair.PrivateKey));
        }

        partial void CopyPublicKeyToClipBoard_Execute()
        {
            Dispatchers.Main.BeginInvoke(() => Clipboard.SetText(Product.KeyPair.PublicKey));
        }
    }
}