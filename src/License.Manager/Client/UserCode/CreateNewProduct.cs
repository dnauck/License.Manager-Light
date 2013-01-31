using System;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Framework.Client;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;

namespace LightSwitchApplication
{
    public partial class CreateNewProduct
    {
        partial void CreateNewProduct_InitializeDataWorkspace(global::System.Collections.Generic.List<global::Microsoft.LightSwitch.IDataService> saveChangesTo)
        {
            // Write your code here.
            this.ProductProperty = new Product();
        }

        partial void CreateNewProduct_Saved()
        {
            // Write your code here.
            this.Close(false);
            Application.Current.ShowDefaultScreen(this.ProductProperty);
        }

        partial void CreateNewProduct_Saving(ref bool handled)
        {
            if (ProductProperty.KeyPair != null)
                return;

            ProductProperty.KeyPair = new KeyPair();

            if (!string.IsNullOrWhiteSpace(ProductProperty.KeyPair.PrivateKey))
                return;

            var passPhrase = this.ShowInputBox("Please enter the pass phrase to encrypt the private key.",
                                               "Private Key Generator");

            if (string.IsNullOrWhiteSpace(passPhrase))
            {
                this.ShowMessageBox("Invalid pass phrase!", "Private Key Generator", MessageBoxOption.Ok);
                handled = false;
                return;
            }

            var keyPair = Portable.Licensing.Security.Cryptography.KeyGenerator.Create().GenerateKeyPair();
            ProductProperty.KeyPair.PrivateKey = keyPair.ToEncryptedPrivateKeyString(passPhrase);
            ProductProperty.KeyPair.PublicKey = keyPair.ToPublicKeyString();
        }
    }
}