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
using Portable.Licensing;

namespace LightSwitchApplication
{
    public partial class LicenseDetail
    {
        partial void License_Loaded(bool succeeded)
        {
            // Write your code here.
            this.SetDisplayNameFromEntity(this.License);
        }

        partial void License_Changed()
        {
            // Write your code here.
            this.SetDisplayNameFromEntity(this.License);
        }

        partial void LicenseDetail_Saved()
        {
            // Write your code here.
            this.SetDisplayNameFromEntity(this.License);
        }

        partial void ExportLicense_Execute()
        {
            this.Save();

            var passPhrase = this.ShowInputBox("Please enter the pass phrase to decrypt the private key.",
                                               "License Signature");

            if (string.IsNullOrWhiteSpace(passPhrase))
            {
                this.ShowMessageBox("Invalid pass phrase!", "License Signature", MessageBoxOption.Ok);
                return;
            }

            var license = Portable.Licensing.License.New()
                                  .WithUniqueIdentifier(License.LicenseId)
                                  .As((LicenseType) Enum.Parse(typeof (LicenseType), License.Type, true))
                                  .ExpiresAt(License.Expiration.HasValue ? License.Expiration.Value : DateTime.MaxValue)
                                  .WithMaximumUtilization(License.Quantity.HasValue ? License.Quantity.Value : 1)
                                  .LicensedTo(License.Customer.Name, License.Customer.EMail)
                                  .WithAdditionalAttributes(License.LicenseAdditionalAttributes
                                                                   .ToDictionary(k => k.Name, v => v.Value))
                                  .WithProductFeatures(
                                      License.LicenseProductFeatures
                                             .ToDictionary(k => k.ProductFeature.Name, v => v.Value))
                                  .CreateAndSignWithPrivateKey(License.Product.KeyPair.PrivateKey, passPhrase);

            Dispatchers.Main.BeginInvoke(() => Clipboard.SetText(license.ToString()));
            this.ShowMessageBox("License copied to clip board!", "License Generator", MessageBoxOption.Ok);
        }
    }
}